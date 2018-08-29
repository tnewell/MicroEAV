using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace EAVServiceTest
{
    public partial class EAVServiceTestHarness
    {

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Retrieve")]
        [TestCategory("Subject")]
        public void RetrieveNonExistentSubject()
        {
            HttpResponseMessage response = WebClient.GetAsync(String.Format("api/data/subjects/{0}", -1)).Result;
            if (response.IsSuccessStatusCode)
            {
                var subject = response.Content.ReadAsAsync<EAV.Store.StoreSubject>().Result;

                Assert.IsNull(subject, "Unexpected subject object retrieved.");
            }
            else
            {
                Assert.Fail(response.Content.ReadAsStringAsync().Result);
            }
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Retrieve")]
        [TestCategory("Subject")]
        public void RetrieveRandomSubject()
        {
            var dbSubject = SelectRandomItem(this.DbContext.Subjects);

            if (dbSubject != null)
            {
                HttpResponseMessage response = WebClient.GetAsync(String.Format("api/data/subjects/{0}", dbSubject.Subject_ID)).Result;
                if (response.IsSuccessStatusCode)
                {
                    var subject = response.Content.ReadAsAsync<EAV.Store.StoreSubject>().Result;

                    Assert.IsNotNull(subject, "Failed to retrieve subject {0}.", dbSubject.Subject_ID);
                    Assert.AreEqual(dbSubject.Subject_ID, subject.SubjectID, "Subject ID values do not match.");
                }
                else
                {
                    Assert.Fail(response.Content.ReadAsStringAsync().Result);
                }
            }
            else
            {
                Assert.Inconclusive("No subjects were found in the database.");
            }
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Update")]
        [TestCategory("Subject")]
        public void UpdateSubject()
        {
            var dbSubject = SelectRandomItem(this.DbContext.Subjects);
            string oldIdentifier = dbSubject.Identifier;

            var subject = (EAV.Store.StoreSubject)dbSubject;

            subject.Identifier = oldIdentifier.Flip();

            HttpResponseMessage response = WebClient.PatchAsJsonAsync<EAV.Store.StoreSubject>("api/data/subjects", subject).Result;
            if (response.IsSuccessStatusCode)
            {
                ResetDatabaseContext();

                dbSubject = this.dbContext.Subjects.Single(it => it.Subject_ID == subject.SubjectID);

                Assert.AreEqual(subject.Identifier, dbSubject.Identifier);
                Assert.AreNotEqual(oldIdentifier, dbSubject.Identifier);
            }
            else
            {
                Assert.Fail(response.Content.ReadAsStringAsync().Result);
            }
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Delete")]
        [TestCategory("Subject")]
        public void DeleteSubject()
        {
            var dbEntity = SelectRandomItem(this.DbContext.Entities);
            var dbContext = SelectRandomItem(this.DbContext.Contexts);

            EAVStoreClient.Subject dbSubjectIn = CreateSubject(dbContext.Context_ID, dbEntity.Entity_ID, Guid.NewGuid().ToString());

            HttpResponseMessage response = WebClient.DeleteAsync(String.Format("api/data/subjects/{0}", dbSubjectIn.Subject_ID)).Result;
            if (response.IsSuccessStatusCode)
            {
                ResetDatabaseContext();

                EAVStoreClient.Subject dbSubjectOut = this.DbContext.Subjects.SingleOrDefault(it => it.Subject_ID == dbSubjectIn.Subject_ID);

                Assert.IsNull(dbSubjectOut, "Failed to delete subject ID {0} from the database.", dbSubjectIn.Subject_ID);
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
        public void RetrieveRootInstances()
        {
            var dbSubject = SelectRandomItem(this.DbContext.Subjects);
            int nDbRootInstances = this.DbContext.Instances.Where(it => it.Subject_ID == dbSubject.Subject_ID && it.Parent_Instance_ID == null).Count();

            HttpResponseMessage response = WebClient.GetAsync(String.Format("api/data/subjects/{0}/instances", dbSubject.Subject_ID)).Result;
            if (response.IsSuccessStatusCode)
            {
                var instances = response.Content.ReadAsAsync<IEnumerable<EAV.Store.StoreInstance>>().Result;
                int nClientInstances = instances.Count();

                Assert.AreEqual(nDbRootInstances, nClientInstances, "The number of instances retrieved by the client does not match the number in the database.");
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
        public void CreateRootInstance()
        {
            var dbContext = SelectRandomItem(this.DbContext.Contexts);
            var dbContainer = SelectRandomItem(DbContext.Containers);
            var dbSubject = SelectRandomItem(dbContext.Subjects);

            HttpResponseMessage response = WebClient.PostAsJsonAsync<EAV.Store.StoreInstance>(String.Format("api/data/subjects/{0}/instances?container={1}", dbSubject.Subject_ID, dbContainer.Container_ID), new EAV.Store.StoreInstance()).Result;
            if (response.IsSuccessStatusCode)
            {
                var instance = response.Content.ReadAsAsync<EAV.Store.StoreInstance>().Result;

                Assert.IsNotNull(instance, "Failed to create instance.");

                var dbInstance = this.DbContext.Instances.SingleOrDefault(it => it.Instance_ID == instance.InstanceID);

                Assert.IsNotNull(dbContainer, String.Format("Failed to retrieve instance ID {0} from the database.", instance.InstanceID));
            }
            else
            {
                Assert.Fail(response.Content.ReadAsStringAsync().Result);
            }
        }
    }
}
