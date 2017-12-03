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
        [TestCategory("Entity")]
        public void RetrieveAllEntities()
        {
            int nDbEntities = this.DbContext.Entities.Count();

            HttpResponseMessage response = WebClient.GetAsync(String.Format("api/data/entities")).Result;
            if (response.IsSuccessStatusCode)
            {
                var entities = response.Content.ReadAsAsync<IEnumerable<EAV.Model.BaseEAVEntity>>().Result;
                int nClientEntities = entities.Count();

                Assert.AreEqual(nDbEntities, nClientEntities, "The number of entities retrieved by the client does not match the number in the database.");
            }
            else
            {
                Assert.Fail(response.Content.ReadAsStringAsync().Result);
            }
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Retrieve")]
        [TestCategory("Entity")]
        public void RetrieveNonExistentEntity()
        {
            HttpResponseMessage response = WebClient.GetAsync(String.Format("api/data/entities/{0}", -1)).Result;
            if (response.IsSuccessStatusCode)
            {
                var entity = response.Content.ReadAsAsync<EAV.Model.BaseEAVEntity>().Result;

                Assert.IsNull(entity, "Unexpected entity object retrieved.");
            }
            else
            {
                Assert.Fail(response.Content.ReadAsStringAsync().Result);
            }
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Retrieve")]
        [TestCategory("Entity")]
        public void RetrieveRandomEntity()
        {
            var dbEntity = SelectRandomItem(this.DbContext.Entities);

            if (dbEntity != null)
            {
                HttpResponseMessage response = WebClient.GetAsync(String.Format("api/data/entities/{0}", dbEntity.Entity_ID)).Result;
                if (response.IsSuccessStatusCode)
                {
                    var entity = response.Content.ReadAsAsync<EAV.Model.BaseEAVEntity>().Result;

                    Assert.IsNotNull(entity, "Failed to retrieve entity {0}.", dbEntity.Entity_ID);
                    Assert.AreEqual(dbEntity.Entity_ID, entity.EntityID, "Entity ID values do not match.");
                }
                else
                {
                    Assert.Fail(response.Content.ReadAsStringAsync().Result);
                }
            }
            else
            {
                Assert.Inconclusive("No entities were found in the database.");
            }
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Create")]
        [TestCategory("Entity")]
        public void CreateEntity()
        {
            string entityDescriptor = Guid.NewGuid().ToString();

            HttpResponseMessage response = WebClient.PostAsJsonAsync<EAV.Model.BaseEAVEntity>("api/data/entities", new EAV.Model.BaseEAVEntity() { Descriptor = entityDescriptor }).Result;
            if (response.IsSuccessStatusCode)
            {
                var entity = response.Content.ReadAsAsync<EAV.Model.BaseEAVEntity>().Result;

                Assert.IsNotNull(entity, "Failed to create entity with descriptor '{0}'", entityDescriptor);

                var dbEntity = this.DbContext.Entities.SingleOrDefault(it => it.Entity_ID == entity.EntityID);

                Assert.IsNotNull(dbEntity, String.Format("Failed to retrieve entity ID {0} from the database.", entity.EntityID));

                Assert.AreEqual(entity.Descriptor, dbEntity.Descriptor, "Property 'Descriptor' was not created correctly.");
            }
            else
            {
                Assert.Fail(response.Content.ReadAsStringAsync().Result);
            }
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Update")]
        [TestCategory("Entity")]
        public void UpdateEntity()
        {
            var dbEntity = SelectRandomItem(this.DbContext.Entities);
            string oldDescriptor = dbEntity.Descriptor;

            var entity = (EAV.Model.BaseEAVEntity)dbEntity;

            entity.Descriptor = oldDescriptor.Flip();

            HttpResponseMessage response = WebClient.PatchAsJsonAsync<EAV.Model.BaseEAVEntity>("api/data/entities", entity).Result;
            if (response.IsSuccessStatusCode)
            {
                ResetDatabaseContext();

                dbEntity = this.dbContext.Entities.Single(it => it.Entity_ID == entity.EntityID);

                Assert.AreEqual(entity.Descriptor, dbEntity.Descriptor);
                Assert.AreNotEqual(oldDescriptor, dbEntity.Descriptor);
            }
            else
            {
                Assert.Fail(response.Content.ReadAsStringAsync().Result);
            }
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Delete")]
        [TestCategory("Entity")]
        public void DeleteEntity()
        {
            EAVStoreClient.Entity dbEntityIn = CreateEntity(Guid.NewGuid().ToString());

            HttpResponseMessage response = WebClient.DeleteAsync(String.Format("api/data/entities/{0}", dbEntityIn.Entity_ID)).Result;
            if (response.IsSuccessStatusCode)
            {
                ResetDatabaseContext();

                EAVStoreClient.Entity dbEntityOut = this.DbContext.Entities.SingleOrDefault(it => it.Entity_ID == dbEntityIn.Entity_ID);

                Assert.IsNull(dbEntityOut, "Failed to delete entity ID {0} from the database.", dbEntityIn.Entity_ID);
            }
            else
            {
                Assert.Fail(response.Content.ReadAsStringAsync().Result);
            }
        }

        // TODO: Add subject related method calls
        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Retrieve")]
        [TestCategory("Subject")]
        public void RetrieveAllSubjects()
        {
            var dbEntity = SelectRandomItem(this.DbContext.Entities);
            int nDbSubjects = this.DbContext.Subjects.Where(it => it.Entity_ID == dbEntity.Entity_ID).Count();

            HttpResponseMessage response = WebClient.GetAsync(String.Format("api/data/entities/{0}/subjects", dbEntity.Entity_ID)).Result;
            if (response.IsSuccessStatusCode)
            {
                var subjects = response.Content.ReadAsAsync<IEnumerable<EAV.Model.BaseEAVSubject>>().Result;
                int nClientEntities = subjects.Count();

                Assert.AreEqual(nDbSubjects, nClientEntities, "The number of subjects retrieved by the client does not match the number in the database.");
            }
            else
            {
                Assert.Fail(response.Content.ReadAsStringAsync().Result);
            }
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Create")]
        [TestCategory("Subject")]
        public void CreateSubject()
        {
            var dbContext = SelectRandomItem(this.DbContext.Contexts);
            var dbEntity = SelectRandomItem(this.DbContext.Entities);
            string subjectIdentifier = Guid.NewGuid().ToString();

            HttpResponseMessage response = WebClient.PostAsJsonAsync<EAV.Model.BaseEAVSubject>(String.Format("api/data/entities/{0}/subjects?context={1}", dbEntity.Entity_ID, dbContext.Context_ID), new EAV.Model.BaseEAVSubject() { Identifier = subjectIdentifier }).Result;
            if (response.IsSuccessStatusCode)
            {
                var subject = response.Content.ReadAsAsync<EAV.Model.BaseEAVSubject>().Result;

                Assert.IsNotNull(subject, "Failed to create subject with identifier '{0}'", subjectIdentifier);

                var dbSubject = this.DbContext.Subjects.SingleOrDefault(it => it.Subject_ID == subject.SubjectID);

                Assert.IsNotNull(dbSubject, String.Format("Failed to retrieve subject ID {0} from the database.", subject.SubjectID));

                Assert.AreEqual(subject.Identifier, dbSubject.Identifier, "Property 'Identifier' was not created correctly.");
            }
            else
            {
                Assert.Fail(response.Content.ReadAsStringAsync().Result);
            }
        }
    }
}
