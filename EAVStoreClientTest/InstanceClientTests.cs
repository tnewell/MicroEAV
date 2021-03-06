﻿using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace EAVStoreClientTestHarness
{
    public partial class EAVStoreClientTestHarness
    {
        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Retrieve")]
        [TestCategory("Instance")]
        public void RetrieveAllInstances()
        {
            EAV.Store.Clients.IInstanceStoreClient client = factory.Create<EAV.Store.Clients.IInstanceStoreClient>();

            int nDbInstances = this.DbContext.Instances.Where(it => it.Parent_Instance_ID == null).Count();
            int nClientInstances = client.RetrieveRootInstances(null, null).Count();

            Assert.AreEqual(nDbInstances, nClientInstances, "The number of instances retrieved by the client does not match the number in the database.");
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Retrieve")]
        [TestCategory("Instance")]
        public void RetrieveNonExistentInstance()
        {
            EAV.Store.Clients.IInstanceStoreClient client = factory.Create<EAV.Store.Clients.IInstanceStoreClient>();

            var instance = client.RetrieveInstance(-1);

            Assert.IsNull(instance, "Unexpected instance object retrieved.");
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Retrieve")]
        [TestCategory("Instance")]
        public void RetrieveRandomInstance()
        {
            EAV.Store.Clients.IInstanceStoreClient client = factory.Create<EAV.Store.Clients.IInstanceStoreClient>();

            var dbInstance = SelectRandomItem(this.DbContext.Instances);

            if (dbInstance != null)
            {
                var instance = client.RetrieveInstance(dbInstance.Instance_ID);

                Assert.IsNotNull(instance, "Failed to retrieve instance {0}.", dbInstance.Instance_ID);
                Assert.AreEqual(dbInstance.Instance_ID, instance.InstanceID, "Instance ID values do not match.");
            }
            else
            {
                Assert.Inconclusive("No instances were found in the database.");
            }
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Create")]
        [TestCategory("Instance")]
        public void CreateRootInstance()
        {
            EAV.Store.Clients.IInstanceStoreClient client = factory.Create<EAV.Store.Clients.IInstanceStoreClient>();
            int subjectID = SelectRandomItem(this.DbContext.Subjects).Subject_ID;
            int containerID = SelectRandomItem(this.DbContext.Containers.Where(it => it.Parent_Container_ID == null)).Container_ID;

            EAV.Store.IStoreInstance instance = client.CreateRootInstance(new EAVStoreLibrary.StoreInstance(), containerID, subjectID);

            Assert.IsNotNull(instance, "Failed to create instance for container ID {0} and subject ID {1}.", containerID, subjectID);

            ResetDatabaseContext();

            var dbInstance = this.DbContext.Instances.SingleOrDefault(it => it.Instance_ID == instance.InstanceID);

            Assert.IsNotNull(dbInstance, String.Format("Failed to retrieve instance ID {0} from the database.", instance.InstanceID));

            Assert.IsNull(dbInstance.Parent_Instance_ID, "Instance has parent reference defined when it should not.");
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Create")]
        [TestCategory("Instance")]
        public void CreateChildInstance()
        {
            EAV.Store.Clients.IInstanceStoreClient client = factory.Create<EAV.Store.Clients.IInstanceStoreClient>();
            var dbParentInstance = SelectRandomItem(this.DbContext.Instances);

            EAV.Store.IStoreInstance instance = client.CreateChildInstance(new EAVStoreLibrary.StoreInstance(), dbParentInstance.Container_ID, dbParentInstance.Instance_ID);

            Assert.IsNotNull(instance, "Failed to create instance for container ID {0} and parent instance ID {1}.", dbParentInstance.Container_ID, dbParentInstance.Instance_ID);

            ResetDatabaseContext();

            var dbInstance = this.DbContext.Instances.SingleOrDefault(it => it.Instance_ID == instance.InstanceID);

            Assert.IsNotNull(dbInstance, String.Format("Failed to retrieve instance ID {0} from the database.", instance.InstanceID));

            Assert.IsNotNull(dbInstance.Parent_Instance_ID, "Instance has no parent reference defined when it should.");
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Update")]
        [TestCategory("Instance")]
        public void UpdateInstance()
        {
            var dbInstance = SelectRandomItem(this.DbContext.Instances);

            EAV.Store.Clients.IInstanceStoreClient client = factory.Create<EAV.Store.Clients.IInstanceStoreClient>();

            var instance = (EAVStoreLibrary.StoreInstance)dbInstance;

            // There are currently no properties for this object to update
            // So this is just here for completeness and future expansion.
            client.UpdateInstance(instance);

            ResetDatabaseContext();

            dbInstance = this.dbContext.Instances.Single(it => it.Instance_ID == instance.InstanceID);
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Delete")]
        [TestCategory("Instance")]
        public void DeleteRootInstance()
        {
            EAV.Store.Clients.IInstanceStoreClient client = factory.Create<EAV.Store.Clients.IInstanceStoreClient>();
            EAVStoreClient.Subject dbSubject = SelectRandomItem(this.DbContext.Subjects);
            EAVStoreClient.Container dbContainer = SelectRandomItem(this.DbContext.Containers);
            EAVStoreClient.Instance dbInstanceIn = CreateInstance(dbContainer.Container_ID, dbSubject.Subject_ID, null);

            client.DeleteInstance(dbInstanceIn.Instance_ID);

            EAVStoreClient.Instance dbInstanceOut = this.DbContext.Instances.SingleOrDefault(it => it.Instance_ID == dbInstanceIn.Instance_ID);

            Assert.IsNull(dbInstanceOut, "Failed to delete instance ID {0} from the database.", dbInstanceIn.Instance_ID);
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Delete")]
        [TestCategory("Instance")]
        public void DeleteChildInstance()
        {
            EAV.Store.Clients.IInstanceStoreClient client = factory.Create<EAV.Store.Clients.IInstanceStoreClient>();
            EAVStoreClient.Instance dbParentInstance = SelectRandomItem(this.DbContext.Instances);
            EAVStoreClient.Container dbContainer = SelectRandomItem(this.DbContext.Containers);
            EAVStoreClient.Instance dbInstanceIn = CreateInstance(dbContainer.Container_ID, dbParentInstance.Subject_ID, dbParentInstance.Instance_ID);

            client.DeleteInstance(dbInstanceIn.Instance_ID);

            EAVStoreClient.Instance dbInstanceOut = this.DbContext.Instances.SingleOrDefault(it => it.Instance_ID == dbInstanceIn.Instance_ID);

            Assert.IsNull(dbInstanceOut, "Failed to delete instance ID {0} from the database.", dbInstanceIn.Instance_ID);
        }
    }
}
