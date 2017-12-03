using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace EAVStoreClientTestHarness
{
    public partial class EAVStoreClientTestHarness
    {
        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Retrieve")]
        [TestCategory("Entity")]
        public void RetrieveAllEntities()
        {
            EAVStoreClient.EAVEntityClient client = new EAVStoreClient.EAVEntityClient();

            int nDbEntities = this.DbContext.Entities.Count();
            int nClientEntities = client.RetrieveEntities().Count();

            Assert.AreEqual(nDbEntities, nClientEntities, "The number of entities retrieved by the client does not match the number in the database.");
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Retrieve")]
        [TestCategory("Entity")]
        public void RetrieveNonExistentEntity()
        {
            EAVStoreClient.EAVEntityClient client = new EAVStoreClient.EAVEntityClient();

            var entity = client.RetrieveEntity(-1);

            Assert.IsNull(entity, "Unexpected entity object retrieved.");
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
                EAVStoreClient.EAVEntityClient client = new EAVStoreClient.EAVEntityClient();

                var entity = client.RetrieveEntity(dbEntity.Entity_ID);

                Assert.IsNotNull(entity, "Failed to retrieve entity {0}.", dbEntity.Entity_ID);
                Assert.AreEqual(dbEntity.Entity_ID, entity.EntityID, "Entity ID values do not match.");
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
            EAVStoreClient.EAVEntityClient client = new EAVStoreClient.EAVEntityClient();
            string entityDescriptor = Guid.NewGuid().ToString();

            EAV.Model.IEAVEntity entity = client.CreateEntity(new EAV.Model.BaseEAVEntity() { Descriptor = entityDescriptor });

            Assert.IsNotNull(entity, "Failed to create entity with descriptor '{0}'", entityDescriptor);

            ResetDatabaseContext();

            var dbEntity = this.DbContext.Entities.SingleOrDefault(it => it.Entity_ID == entity.EntityID);

            Assert.IsNotNull(dbEntity, String.Format("Failed to retrieve entity ID {0} from the database.", entity.EntityID));

            Assert.AreEqual(entity.Descriptor, dbEntity.Descriptor, "Property 'Descriptor' was not created correctly.");
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Update")]
        [TestCategory("Entity")]
        public void UpdateEntity()
        {
            var dbEntity = SelectRandomItem(this.DbContext.Entities);
            string oldDescriptor = dbEntity.Descriptor;

            EAVStoreClient.EAVEntityClient client = new EAVStoreClient.EAVEntityClient();

            var entity = (EAV.Model.BaseEAVEntity)dbEntity;

            entity.Descriptor = oldDescriptor.Flip();

            client.UpdateEntity(entity);

            ResetDatabaseContext();

            dbEntity = this.dbContext.Entities.Single(it => it.Entity_ID == entity.EntityID);

            Assert.AreEqual(entity.Descriptor, dbEntity.Descriptor);
            Assert.AreNotEqual(oldDescriptor, dbEntity.Descriptor);
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Delete")]
        [TestCategory("Entity")]
        public void DeleteEntity()
        {
            EAVStoreClient.EAVEntityClient client = new EAVStoreClient.EAVEntityClient();
            EAVStoreClient.Entity dbEntityIn = CreateEntity(Guid.NewGuid().ToString());

            client.DeleteEntity(dbEntityIn.Entity_ID);

            EAVStoreClient.Entity dbEntityOut = this.DbContext.Entities.SingleOrDefault(it => it.Entity_ID == dbEntityIn.Entity_ID);

            Assert.IsNull(dbEntityOut, "Failed to delete entity ID {0} from the database.", dbEntityIn.Entity_ID);
        }
    }
}
