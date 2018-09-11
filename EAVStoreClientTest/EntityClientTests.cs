using System;
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
            EAV.Store.Clients.IEntityStoreClient client = factory.Create<EAV.Store.Clients.IEntityStoreClient>();

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
            EAV.Store.Clients.IEntityStoreClient client = factory.Create<EAV.Store.Clients.IEntityStoreClient>();

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
                EAV.Store.Clients.IEntityStoreClient client = factory.Create<EAV.Store.Clients.IEntityStoreClient>();

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
            EAV.Store.Clients.IEntityStoreClient client = factory.Create<EAV.Store.Clients.IEntityStoreClient>();
            string entityDescriptor = Guid.NewGuid().ToString();

            EAV.Store.IStoreEntity entity = client.CreateEntity(new EAVStoreLibrary.StoreEntity() { Descriptor = entityDescriptor });

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

            EAV.Store.Clients.IEntityStoreClient client = factory.Create<EAV.Store.Clients.IEntityStoreClient>();

            var entity = (EAVStoreLibrary.StoreEntity)dbEntity;

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
            EAV.Store.Clients.IEntityStoreClient client = factory.Create<EAV.Store.Clients.IEntityStoreClient>();
            EAVStoreClient.Entity dbEntityIn = CreateEntity(Guid.NewGuid().ToString());

            client.DeleteEntity(dbEntityIn.Entity_ID);

            EAVStoreClient.Entity dbEntityOut = this.DbContext.Entities.SingleOrDefault(it => it.Entity_ID == dbEntityIn.Entity_ID);

            Assert.IsNull(dbEntityOut, "Failed to delete entity ID {0} from the database.", dbEntityIn.Entity_ID);
        }
    }
}
