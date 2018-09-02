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
        [TestCategory("Container")]
        public void RetrieveNonExistentContainer()
        {
            HttpResponseMessage response = WebClient.GetAsync(String.Format("api/meta/containers/{0}", -1)).Result;
            if (response.IsSuccessStatusCode)
            {
                var container = response.Content.ReadAsAsync<EAV.Store.StoreContainer>().Result;

                Assert.IsNull(container, "Unexpected container object retrieved.");
            }
            else
            {
                Assert.Fail(response.Content.ReadAsStringAsync().Result);
            }
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Retrieve")]
        [TestCategory("Container")]
        public void RetrieveRandomContainer()
        {
            var dbContainer = SelectRandomItem(this.DbContext.Containers);

            if (dbContainer != null)
            {
                HttpResponseMessage response = WebClient.GetAsync(String.Format("api/meta/containers/{0}", dbContainer.Container_ID)).Result;
                if (response.IsSuccessStatusCode)
                {
                    var container = response.Content.ReadAsAsync<EAV.Store.StoreContainer>().Result;

                    Assert.IsNotNull(container, "Failed to retrieve container {0}.", dbContainer.Container_ID);
                    Assert.AreEqual(dbContainer.Container_ID, container.ContainerID, "Container ID values do not match.");
                }
                else
                {
                    Assert.Fail(response.Content.ReadAsStringAsync().Result);
                }
            }
            else
            {
                Assert.Inconclusive("No containers were found in the database.");
            }
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Update")]
        [TestCategory("Container")]
        public void UpdateContainer()
        {
            var dbContainer = SelectRandomItem(this.DbContext.Containers);
            string oldName = dbContainer.Name;
            string oldDataName = dbContainer.Data_Name;
            string oldDisplayText = dbContainer.Display_Text;
            bool oldIsRepeating = dbContainer.Is_Repeating;
            int oldSequence = dbContainer.Sequence;

            var container = (EAV.Store.StoreContainer)dbContainer;

            container.Name = oldName.Flip();
            container.DataName = oldDataName.Flip();
            container.DisplayText = oldDisplayText.Flip();
            container.IsRepeating = !oldIsRepeating;
            container.Sequence = -oldSequence;

            HttpResponseMessage response = WebClient.PatchAsJsonAsync<EAV.Store.StoreContainer>("api/meta/containers", container).Result;
            if (response.IsSuccessStatusCode)
            {
                ResetDatabaseContext();

                dbContainer = this.dbContext.Containers.Single(it => it.Container_ID == container.ContainerID);

                Assert.AreEqual(container.Name, dbContainer.Name);
                Assert.AreNotEqual(oldName, dbContainer.Name);
                Assert.AreEqual(container.DataName, dbContainer.Data_Name);
                Assert.AreNotEqual(oldDataName, dbContainer.Data_Name);
                Assert.AreEqual(container.DisplayText, dbContainer.Display_Text);
                Assert.AreNotEqual(oldDisplayText, dbContainer.Display_Text);
                Assert.AreEqual(container.IsRepeating, dbContainer.Is_Repeating);
                Assert.AreNotEqual(oldIsRepeating, dbContainer.Is_Repeating);
                Assert.AreEqual(container.Sequence, dbContainer.Sequence);
                Assert.AreNotEqual(oldSequence, dbContainer.Sequence);
            }
            else
            {
                Assert.Fail(response.Content.ReadAsStringAsync().Result);
            }
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Delete")]
        [TestCategory("Container")]
        public void DeleteContainer()
        {
            var dbContext = SelectRandomItem(this.DbContext.Contexts);
            EAVStoreClient.Container dbContainerIn = CreateContainer(dbContext.Context_ID, null, Guid.NewGuid().ToString(), rng.Next(), true);

            HttpResponseMessage response = WebClient.DeleteAsync(String.Format("api/meta/containers/{0}", dbContainerIn.Container_ID)).Result;
            if (response.IsSuccessStatusCode)
            {
                ResetDatabaseContext();

                EAVStoreClient.Container dbContainerOut = this.DbContext.Containers.SingleOrDefault(it => it.Container_ID == dbContainerIn.Container_ID);

                Assert.IsNull(dbContainerOut, "Failed to delete container ID {0} from the database.", dbContainerIn.Container_ID);
            }
            else
            {
                Assert.Fail(response.Content.ReadAsStringAsync().Result);
            }
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Retrieve")]
        [TestCategory("Container")]
        public void RetrieveChildContainers()
        {
            var dbParentContainer = SelectRandomItem(this.DbContext.Containers.Where(it => it.Parent_Container_ID == null));
            int nDbChildContainers = this.DbContext.Containers.Where(it => it.Parent_Container_ID == dbParentContainer.Container_ID).Count();

            HttpResponseMessage response = WebClient.GetAsync(String.Format("api/meta/containers/{0}/containers", dbParentContainer.Container_ID)).Result;
            if (response.IsSuccessStatusCode)
            {
                var containers = response.Content.ReadAsAsync<IEnumerable<EAV.Store.StoreContainer>>().Result;
                int nClientContainers = containers.Count();

                Assert.AreEqual(nDbChildContainers, nClientContainers, "The number of containers retrieved by the client does not match the number in the database.");
            }
            else
            {
                Assert.Fail(response.Content.ReadAsStringAsync().Result);
            }
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Create")]
        [TestCategory("Container")]
        public void CreateChildContainer()
        {
            var dbParentContainer = SelectRandomItem(this.DbContext.Containers);
            string childContainerName = Guid.NewGuid().ToString();

            HttpResponseMessage response = WebClient.PostAsJsonAsync<EAV.Store.StoreContainer>(String.Format("api/meta/containers/{0}/containers", dbParentContainer.Container_ID), new EAV.Store.StoreContainer() { Name = childContainerName, DataName = childContainerName.ToUpper(), DisplayText = childContainerName + ":", IsRepeating = true }).Result;
            if (response.IsSuccessStatusCode)
            {
                var container = response.Content.ReadAsAsync<EAV.Store.StoreContainer>().Result;

                Assert.IsNotNull(container, "Failed to create container with name '{0}'", childContainerName);

                var dbContainer = this.DbContext.Containers.SingleOrDefault(it => it.Container_ID == container.ContainerID);

                Assert.IsNotNull(dbContainer, String.Format("Failed to retrieve container ID {0} from the database.", container.ContainerID));

                Assert.AreEqual(container.Name, dbContainer.Name, "Property 'Name' was not created correctly.");
                Assert.AreEqual(container.DataName, dbContainer.Data_Name, "Property 'DataName' was not created correctly.");
                Assert.AreEqual(container.DisplayText, dbContainer.Display_Text, "Property 'DisplayText' was not created correctly.");
                Assert.AreEqual(container.IsRepeating, dbContainer.Is_Repeating, "Property 'IsRepeating' was not created correctly.");
                Assert.AreEqual(container.Sequence, dbContainer.Sequence, "Property 'Sequence' was not created correctly.");
            }
            else
            {
                Assert.Fail(response.Content.ReadAsStringAsync().Result);
            }
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Retrieve")]
        [TestCategory("Attribute")]
        public void RetrieveAttributes()
        {
            var dbContainer = SelectRandomItem(this.DbContext.Containers);
            int nDbAttributes = this.DbContext.Attributes.Where(it => it.Container_ID == dbContainer.Container_ID).Count();

            HttpResponseMessage response = WebClient.GetAsync(String.Format("api/meta/containers/{0}/attributes", dbContainer.Container_ID)).Result;
            if (response.IsSuccessStatusCode)
            {
                var attributes = response.Content.ReadAsAsync<IEnumerable<EAV.Store.StoreAttribute>>().Result;
                int nClientAttributes = attributes.Count();

                Assert.AreEqual(nDbAttributes, nClientAttributes, "The number of attributes retrieved by the client does not match the number in the database.");
            }
            else
            {
                Assert.Fail(response.Content.ReadAsStringAsync().Result);
            }
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Create")]
        [TestCategory("Attribute")]
        public void CreateAttribute()
        {
            var dbContainer = SelectRandomItem(this.DbContext.Containers);
            string attributeName = Guid.NewGuid().ToString();

            HttpResponseMessage response = WebClient.PostAsJsonAsync<EAV.Store.StoreAttribute>(String.Format("api/meta/containers/{0}/attributes", dbContainer.Container_ID), new EAV.Store.StoreAttribute() { Name = attributeName, DataName = attributeName.ToUpper(), DisplayText = attributeName + ":", IsKey = true }).Result;
            if (response.IsSuccessStatusCode)
            {
                var attribute = response.Content.ReadAsAsync<EAV.Store.StoreAttribute>().Result;

                Assert.IsNotNull(attribute, "Failed to create attribute with name '{0}'", attributeName);

                var dbAttribute = this.DbContext.Attributes.SingleOrDefault(it => it.Attribute_ID == attribute.AttributeID);

                Assert.IsNotNull(dbAttribute, String.Format("Failed to retrieve container ID {0} from the database.", attribute.ContainerID));

                Assert.AreEqual(attribute.Name, dbAttribute.Name, "Property 'Name' was not created correctly.");
                Assert.AreEqual(attribute.DataName, dbAttribute.Data_Name, "Property 'DataName' was not created correctly.");
                Assert.AreEqual(attribute.DisplayText, dbAttribute.Display_Text, "Property 'DisplayText' was not created correctly.");
                Assert.AreEqual(attribute.IsKey, dbAttribute.Is_Key, "Property 'IsKey' was not created correctly.");
                Assert.AreEqual(attribute.Sequence, dbAttribute.Sequence, "Property 'Sequence' was not created correctly.");
            }
            else
            {
                Assert.Fail(response.Content.ReadAsStringAsync().Result);
            }
        }
    }
}
