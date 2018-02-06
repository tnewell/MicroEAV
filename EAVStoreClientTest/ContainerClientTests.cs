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
        [TestCategory("Container")]
        public void RetrieveAllContainers()
        {
            EAVStoreClient.EAVContainerClient client = new EAVStoreClient.EAVContainerClient();

            int nDbContainers = this.DbContext.Containers.Where(it => it.Parent_Container_ID == null).Count();
            int nClientContainers = client.RetrieveRootContainers(null).Count();

            Assert.AreEqual(nDbContainers, nClientContainers, "The number of containers retrieved by the client does not match the number in the database.");
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Retrieve")]
        [TestCategory("Container")]
        public void RetrieveNonExistentContainer()
        {
            EAVStoreClient.EAVContainerClient client = new EAVStoreClient.EAVContainerClient();

            var container = client.RetrieveContainer(-1);

            Assert.IsNull(container, "Unexpected container object retrieved.");
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
                EAVStoreClient.EAVContainerClient client = new EAVStoreClient.EAVContainerClient();

                var container = client.RetrieveContainer(dbContainer.Container_ID);

                Assert.IsNotNull(container, "Failed to retrieve container {0}.", dbContainer.Container_ID);
                Assert.AreEqual(dbContainer.Container_ID, container.ContainerID, "Container ID values do not match.");
            }
            else
            {
                Assert.Inconclusive("No containers were found in the database.");
            }
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Create")]
        [TestCategory("Container")]
        public void CreateRootContainer()
        {
            EAVStoreClient.EAVContainerClient client = new EAVStoreClient.EAVContainerClient();
            int contextID = SelectRandomItem(this.DbContext.Contexts).Context_ID;
            string containerName = Guid.NewGuid().ToString();

            EAV.Model.IEAVContainer container = client.CreateRootContainer(new EAV.Model.BaseEAVContainer()
            {
                Name = containerName,
                DataName = containerName.ToUpper(),
                DisplayText = containerName + ":",
                Sequence = rng.Next(),
            }, contextID);

            Assert.IsNotNull(container, "Failed to create container with name '{0}' for context ID {1}.", containerName, contextID);

            ResetDatabaseContext();

            var dbContainer = this.DbContext.Containers.SingleOrDefault(it => it.Container_ID == container.ContainerID);

            Assert.IsNotNull(dbContainer, String.Format("Failed to retrieve container ID {0} from the database.", container.ContainerID));

            Assert.IsNull(dbContainer.Parent_Container_ID, "Container has parent reference defined when it should not.");

            Assert.AreEqual(container.Name, dbContainer.Name, "Property 'Name' was not created correctly.");
            Assert.AreEqual(container.DataName, dbContainer.Data_Name, "Property 'DataName' was not created correctly.");
            Assert.AreEqual(container.DisplayText, dbContainer.Display_Text, "Property 'DisplayText' was not created correctly.");
            Assert.AreEqual(container.IsRepeating, dbContainer.Is_Repeating, "Property 'IsRepeating' was not created correctly.");
            Assert.AreEqual(container.Sequence, dbContainer.Sequence, "Property 'Sequence' was not created correctly.");
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Create")]
        [TestCategory("Container")]
        [ExpectedException(typeof(System.Data.Entity.Infrastructure.DbUpdateException))]
        public void CreateDuplicateRootContainer_Name()
        {
            EAVStoreClient.EAVContainerClient client = new EAVStoreClient.EAVContainerClient();
            int contextID = SelectRandomItem(this.DbContext.Contexts).Context_ID;
            string containerName = Guid.NewGuid().ToString();

            EAV.Model.IEAVContainer container = client.CreateRootContainer(new EAV.Model.BaseEAVContainer()
            {
                Name = containerName,
                DataName = containerName.ToUpper(),
                DisplayText = containerName + ":",
                Sequence = rng.Next(),
            }, contextID);

            Assert.IsNotNull(container, "Failed to create container with name '{0}' for context ID {1}.", containerName, contextID);

            client.CreateRootContainer(new EAV.Model.BaseEAVContainer()
            {
                Name = containerName,
                DataName = containerName.ToUpper() + "1",
                DisplayText = containerName + ":",
                Sequence = rng.Next(),
            }, contextID);

            Assert.Fail("Failed to throw exception creating container with duplicate name.");
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Create")]
        [TestCategory("Container")]
        [ExpectedException(typeof(System.Data.Entity.Infrastructure.DbUpdateException))]
        public void CreateDuplicateRootContainer_Data_Name()
        {
            EAVStoreClient.EAVContainerClient client = new EAVStoreClient.EAVContainerClient();
            int contextID = SelectRandomItem(this.DbContext.Contexts).Context_ID;
            string containerName = Guid.NewGuid().ToString();

            EAV.Model.IEAVContainer container = client.CreateRootContainer(new EAV.Model.BaseEAVContainer()
            {
                Name = containerName,
                DataName = containerName.ToUpper(),
                DisplayText = containerName + ":",
                Sequence = rng.Next(),
            }, contextID);

            Assert.IsNotNull(container, "Failed to create container with name '{0}' for context ID {1}.", containerName, contextID);

            client.CreateRootContainer(new EAV.Model.BaseEAVContainer()
            {
                Name = containerName + "1",
                DataName = containerName.ToUpper(),
                DisplayText = containerName + ":",
                Sequence = rng.Next(),
            }, contextID);

            Assert.Fail("Failed to throw exception creating root container with duplicate data name.");
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Create")]
        [TestCategory("Container")]
        public void CreateChildContainer()
        {
            EAVStoreClient.EAVContainerClient client = new EAVStoreClient.EAVContainerClient();
            int parentContainerID = SelectRandomItem(this.DbContext.Containers.Where(it => it.Parent_Container_ID == null)).Container_ID;
            string containerName = Guid.NewGuid().ToString();

            EAV.Model.IEAVContainer container = client.CreateChildContainer(new EAV.Model.BaseEAVContainer()
            {
                Name = containerName,
                DataName = containerName.ToUpper(),
                DisplayText = containerName + ":",
                IsRepeating = true,
                Sequence = rng.Next(),
            }, parentContainerID);

            Assert.IsNotNull(container, "Failed to create container with name '{0}' for parent container ID {1}.", containerName, parentContainerID);

            ResetDatabaseContext();

            var dbContainer = this.DbContext.Containers.SingleOrDefault(it => it.Container_ID == container.ContainerID);

            Assert.IsNotNull(dbContainer, String.Format("Failed to retrieve container ID {0} from the database.", container.ContainerID));

            Assert.IsNotNull(dbContainer.Parent_Container_ID, "Container has no parent reference defined when it should.");

            Assert.AreEqual(container.Name, dbContainer.Name, "Property 'Name' was not created correctly.");
            Assert.AreEqual(container.DataName, dbContainer.Data_Name, "Property 'DataName' was not created correctly.");
            Assert.AreEqual(container.DisplayText, dbContainer.Display_Text, "Property 'DisplayText' was not created correctly.");
            Assert.AreEqual(container.IsRepeating, dbContainer.Is_Repeating, "Property 'IsRepeating' was not created correctly.");
            Assert.AreEqual(container.Sequence, dbContainer.Sequence, "Property 'Sequence' was not created correctly.");
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Create")]
        [TestCategory("Container")]
        [ExpectedException(typeof(System.Data.Entity.Infrastructure.DbUpdateException))]
        public void CreateDuplicateChildContainer_Name()
        {
            EAVStoreClient.EAVContainerClient client = new EAVStoreClient.EAVContainerClient();
            int parentContainerID = SelectRandomItem(this.DbContext.Containers.Where(it => it.Parent_Container_ID == null)).Container_ID;
            string containerName = Guid.NewGuid().ToString();

            EAV.Model.IEAVContainer container = client.CreateChildContainer(new EAV.Model.BaseEAVContainer()
            {
                Name = containerName,
                DataName = containerName.ToUpper(),
                DisplayText = containerName + ":",
                IsRepeating = true,
                Sequence = rng.Next(),
            }, parentContainerID);

            Assert.IsNotNull(container, "Failed to create container with name '{0}' for parent container ID {1}.", containerName, parentContainerID);

            client.CreateChildContainer(new EAV.Model.BaseEAVContainer()
            {
                Name = containerName,
                DataName = containerName.ToUpper() + "1",
                DisplayText = containerName + ":",
                IsRepeating = true,
                Sequence = rng.Next(),
            }, parentContainerID);

            Assert.Fail("Failed to throw exception creating child container with duplicate name.");
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Create")]
        [TestCategory("Container")]
        [ExpectedException(typeof(System.Data.Entity.Infrastructure.DbUpdateException))]
        public void CreateDuplicateChildContainer_Data_Name()
        {
            EAVStoreClient.EAVContainerClient client = new EAVStoreClient.EAVContainerClient();
            int parentContainerID = SelectRandomItem(this.DbContext.Containers.Where(it => it.Parent_Container_ID == null)).Container_ID;
            string containerName = Guid.NewGuid().ToString();

            EAV.Model.IEAVContainer container = client.CreateChildContainer(new EAV.Model.BaseEAVContainer()
            {
                Name = containerName,
                DataName = containerName.ToUpper(),
                DisplayText = containerName + ":",
                IsRepeating = true,
                Sequence = rng.Next(),
            }, parentContainerID);

            Assert.IsNotNull(container, "Failed to create container with name '{0}' for parent container ID {1}.", containerName, parentContainerID);

            client.CreateChildContainer(new EAV.Model.BaseEAVContainer()
            {
                Name = containerName + "1",
                DataName = containerName.ToUpper(),
                DisplayText = containerName + ":",
                IsRepeating = true,
                Sequence = rng.Next(),
            }, parentContainerID);

            Assert.Fail("Failed to throw exception creating child container with duplicate name.");
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

            EAVStoreClient.EAVContainerClient client = new EAVStoreClient.EAVContainerClient();

            var container = (EAV.Model.BaseEAVContainer)dbContainer;

            container.Name = oldName.Flip();
            container.DataName = oldDataName.Flip();
            container.DisplayText = oldDisplayText.Flip();
            container.IsRepeating = !oldIsRepeating;
            container.Sequence = -oldSequence;

            client.UpdateContainer(container);

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

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Delete")]
        [TestCategory("Container")]
        public void DeleteRootContainer()
        {
            EAVStoreClient.EAVContainerClient client = new EAVStoreClient.EAVContainerClient();
            EAVStoreClient.Context dbContext = SelectRandomItem(this.DbContext.Contexts);
            EAVStoreClient.Container dbContainerIn = CreateContainer(dbContext.Context_ID, null, Guid.NewGuid().ToString(), rng.Next(), true);

            client.DeleteContainer(dbContainerIn.Container_ID);

            EAVStoreClient.Container dbContainerOut = this.DbContext.Containers.SingleOrDefault(it => it.Container_ID == dbContainerIn.Container_ID);

            Assert.IsNull(dbContainerOut, "Failed to delete container ID {0} from the database.", dbContainerIn.Container_ID);
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Delete")]
        [TestCategory("Container")]
        public void DeleteChildContainer()
        {
            EAVStoreClient.EAVContainerClient client = new EAVStoreClient.EAVContainerClient();
            EAVStoreClient.Container dbParentContainer = SelectRandomItem<EAVStoreClient.Container>(this.DbContext.Containers);
            EAVStoreClient.Container dbContainerIn = CreateContainer(dbParentContainer.Context_ID, dbParentContainer.Container_ID, Guid.NewGuid().ToString(), rng.Next(), true);

            client.DeleteContainer(dbContainerIn.Container_ID);

            EAVStoreClient.Container dbContainerOut = this.DbContext.Containers.SingleOrDefault(it => it.Container_ID == dbContainerIn.Container_ID);

            Assert.IsNull(dbContainerOut, "Failed to delete container ID {0} from the database.", dbContainerIn.Container_ID);
        }
    }
}
