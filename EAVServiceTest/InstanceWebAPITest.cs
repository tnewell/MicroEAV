using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace EAVServiceTest
{
    public partial class EAVWebServiceTestHarness
    {
        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Retrieve")]
        [TestCategory("Instance")]
        public void RetrieveNonExistentInstance()
        {
            HttpResponseMessage response = WebClient.GetAsync(String.Format("api/data/instances/{0}", -1)).Result;
            if (response.IsSuccessStatusCode)
            {
                var instance = response.Content.ReadAsAsync<EAV.Store.StoreInstance>().Result;

                Assert.IsNull(instance, "Unexpected instance object retrieved.");
            }
            else
            {
                Assert.Fail(response.Content.ReadAsStringAsync().Result);
            }
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Retrieve")]
        [TestCategory("Instance")]
        public void RetrieveRandomInstance()
        {
            var dbInstance = SelectRandomItem(this.DbContext.Instances);

            if (dbInstance != null)
            {
                HttpResponseMessage response = WebClient.GetAsync(String.Format("api/data/instances/{0}", dbInstance.Instance_ID)).Result;
                if (response.IsSuccessStatusCode)
                {
                    var instance = response.Content.ReadAsAsync<EAV.Store.StoreInstance>().Result;

                    Assert.IsNotNull(instance, "Failed to retrieve instance {0}.", dbInstance.Instance_ID);
                    Assert.AreEqual(dbInstance.Instance_ID, instance.InstanceID, "Instance ID values do not match.");
                }
                else
                {
                    Assert.Fail(response.Content.ReadAsStringAsync().Result);
                }
            }
            else
            {
                Assert.Inconclusive("No instances were found in the database.");
            }
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Update")]
        [TestCategory("Instance")]
        public void UpdateInstance()
        {
            var dbInstance = SelectRandomItem(this.DbContext.Instances);
            var instance = (EAV.Store.StoreInstance)dbInstance;

            HttpResponseMessage response = WebClient.PatchAsJsonAsync<EAV.Store.StoreInstance>("api/data/instances", instance).Result;
            if (response.IsSuccessStatusCode)
            {
                ResetDatabaseContext();

                dbInstance = this.dbContext.Instances.Single(it => it.Instance_ID == instance.InstanceID);
            }
            else
            {
                Assert.Fail(response.Content.ReadAsStringAsync().Result);
            }
        }

        // TODO: DeleteInstance

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Retrieve")]
        [TestCategory("Instance")]
        public void RetrieveChildInstances()
        {
            var dbParentInstance = SelectRandomItem(this.DbContext.Instances.Where(it => it.Parent_Instance_ID == null));
            int nDbChildInstances = this.DbContext.Instances.Where(it => it.Parent_Instance_ID == dbParentInstance.Instance_ID).Count();

            HttpResponseMessage response = WebClient.GetAsync(String.Format("api/data/instances/{0}/instances", dbParentInstance.Instance_ID)).Result;
            if (response.IsSuccessStatusCode)
            {
                var instances = response.Content.ReadAsAsync<IEnumerable<EAV.Store.StoreInstance>>().Result;
                int nClientInstances = instances.Count();

                Assert.AreEqual(nDbChildInstances, nClientInstances, "The number of instances retrieved by the client does not match the number in the database.");
            }
            else
            {
                Assert.Fail(response.Content.ReadAsStringAsync().Result);
            }
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Create")]
        [TestCategory("Instance")]
        public void CreateChildInstance()
        {
            var dbParentInstance = SelectRandomItem(this.DbContext.Instances.Where(it => it.Container.ChildContainers.Any()));
            var dbContainer = SelectRandomItem(dbParentInstance.Container.ChildContainers);

            HttpResponseMessage response = WebClient.PostAsJsonAsync<EAV.Store.StoreInstance>(String.Format("api/data/instances/{0}/instances?container={1}", dbParentInstance.Instance_ID, dbContainer.Container_ID), new EAV.Store.StoreInstance()).Result;
            if (response.IsSuccessStatusCode)
            {
                var instance = response.Content.ReadAsAsync<EAV.Store.StoreInstance>().Result;

                Assert.IsNotNull(instance, "Failed to create instance.");

                var dbInstance = this.DbContext.Instances.SingleOrDefault(it => it.Instance_ID == instance.InstanceID);

                Assert.IsNotNull(dbInstance, String.Format("Failed to retrieve instance ID {0} from the database.", instance.InstanceID));
            }
            else
            {
                Assert.Fail(response.Content.ReadAsStringAsync().Result);
            }
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Retrieve")]
        [TestCategory("Value")]
        public void RetrieveValues()
        {
            var dbInstance = SelectRandomItem(this.DbContext.Instances);
            int nDbValues = this.DbContext.Values.Where(it => it.Instance_ID == dbInstance.Instance_ID).Count();

            HttpResponseMessage response = WebClient.GetAsync(String.Format("api/data/instances/{0}/values", dbInstance.Instance_ID)).Result;
            if (response.IsSuccessStatusCode)
            {
                var values = response.Content.ReadAsAsync<IEnumerable<EAV.Store.StoreValue>>().Result;
                int nClientValues = values.Count();

                Assert.AreEqual(nDbValues, nClientValues, "The number of values retrieved by the client does not match the number in the database.");
            }
            else
            {
                Assert.Fail(response.Content.ReadAsStringAsync().Result);
            }
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Create")]
        [TestCategory("Value")]
        public void CreateValue()
        {
            var dbPotentialValueKeys = this.DbContext.Instances.AsEnumerable().Join(this.DbContext.Attributes.AsEnumerable(), left => left.Container_ID, right => right.Container_ID, (left, right) => new Tuple<int,int>(right.Attribute_ID, left.Instance_ID));
            var dbUsableValueKeys = dbPotentialValueKeys.GroupJoin(this.DbContext.Values.AsEnumerable(), left => left, right => new Tuple<int, int>(right.Attribute_ID, right.Instance_ID), (left, right) => new { Pair = left, Value = right.FirstOrDefault() }).Where(it => it.Value == null).Select(it => it.Pair);
            var dbValueKey = SelectRandomItem(dbUsableValueKeys);
            string rawValue = Guid.NewGuid().ToString();

            HttpResponseMessage response = WebClient.PostAsJsonAsync<EAV.Store.StoreValue>(String.Format("api/data/instances/{0}/values?attribute={1}", dbValueKey.Item2, dbValueKey.Item1), new EAV.Store.StoreValue() { RawValue = rawValue }).Result;
            if (response.IsSuccessStatusCode)
            {
                var value = response.Content.ReadAsAsync<EAV.Store.StoreValue>().Result;

                Assert.IsNotNull(value, "Failed to create value with value '{0}'", rawValue);

                var dbValue = this.DbContext.Values.SingleOrDefault(it => it.Instance_ID == value.InstanceID && it.Attribute_ID == value.AttributeID);

                Assert.IsNotNull(dbValue, String.Format("Failed to retrieve instance ID {0}, attribute ID {1} from the database.", value.InstanceID, value.AttributeID));
            }
            else
            {
                Assert.Fail(response.Content.ReadAsStringAsync().Result);
            }
        }
    }
}
