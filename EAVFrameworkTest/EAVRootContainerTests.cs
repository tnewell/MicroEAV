using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using EAV.Model;

using EAVFramework.Model;


namespace EAVFrameworkTest
{
    public partial class EAVFrameworkTestHarness
    {
        #region Object State Tests
        [TestMethod]
        public void CreateRootContainer()
        {
            EAVRootContainer aContainer = new EAVRootContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            Assert.IsNull(aContainer.ContainerID, "Property 'ContainerID' is not 'null' on creation.");

            Assert.IsNull(aContainer.Name, "Property 'Name' is not 'null' on creation.");
            Assert.IsNull(aContainer.DataName, "Property 'DataName' is not 'null' on creation.");
            Assert.IsNull(aContainer.DisplayText, "Property 'DisplayText' is not 'null' on creation.");
            Assert.IsFalse(aContainer.IsRepeating, "Property 'IsRepeating' is not 'false' on creation.");

            Assert.IsNull(aContainer.Context, "Property 'Context' is not 'null' on creation.");

            Assert.IsNull(aContainer.ParentContainer, "Property 'ParentContainer' is not 'null' on creation.");

            Assert.IsNotNull(aContainer.ChildContainers, "Property 'ChildContainers' is null on creation.");
            Assert.IsTrue(aContainer.ChildContainers.Count == 0, "Property 'ChildContainers' is not empty on creation.");

            Assert.IsNotNull(aContainer.Attributes, "Property 'Attributes' is null on creation.");
            Assert.IsTrue(aContainer.Attributes.Count == 0, "Property 'Attributes' is not empty on creation.");

            Assert.IsNotNull(aContainer.Instances, "Property 'Instances' is null on creation.");
            Assert.IsTrue(aContainer.Instances.Count == 0, "Property 'Instances' is not empty on creation.");
        }

        [TestMethod]
        public void SetRootContainerUnmodifiedFromNew()
        {
            EAVRootContainer aContainer = new EAVRootContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Unmodified, "Object state not properly set to 'Unmodified'.");
        }

        [TestMethod]
        public void SetRootContainerModifiedFromNew()
        {
            EAVRootContainer aContainer = new EAVRootContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            string name = Guid.NewGuid().ToString();
            aContainer.Name = name;

            Assert.AreEqual(name, aContainer.Name, "Property 'Name' not properly set.");

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Setting property in 'New' state should not alter object state.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SetRootContainerDeletedFromNew()
        {
            EAVRootContainer aContainer = new EAVRootContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContainer.MarkDeleted();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Setting object state to 'Deleted' from 'New' state should not alter object state.");
        }

        [TestMethod]
        public void SetRootContainerModifiedFromUnmodified()
        {
            EAVRootContainer aContainer = new EAVRootContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Unmodified, "Object state not properly set to 'Unmodified'.");

            string name = Guid.NewGuid().ToString();
            aContainer.Name = name;

            Assert.AreEqual(name, aContainer.Name, "Property 'Name' not properly set.");
            Assert.IsTrue(aContainer.ObjectState == ObjectState.Modified, "Setting property should alter object state to 'Modified'.");
        }

        [TestMethod]
        public void SetRootContainerDeletedFromUnmodified()
        {
            EAVRootContainer aContainer = new EAVRootContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Unmodified, "Object state not properly set to 'Unmodified'.");

            aContainer.MarkDeleted();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Deleted, "Object state not properly set to 'Deleted'.");
        }

        [TestMethod]
        public void SetRootContainerUnmodifiedFromModified()
        {
            EAVRootContainer aContainer = new EAVRootContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Unmodified, "Object state not properly set to 'Unmodified'.");

            string name = Guid.NewGuid().ToString();
            aContainer.Name = name;

            Assert.AreEqual(name, aContainer.Name, "Property 'Name' not properly set.");
            Assert.IsTrue(aContainer.ObjectState == ObjectState.Modified, "Setting property should alter object state to 'Modified'.");

            aContainer.MarkUnmodified();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Unmodified, "Object state not properly set to 'Unmodified'.");
        }

        [TestMethod]
        public void SetRootContainerDeletedFromModified()
        {
            EAVRootContainer aContainer = new EAVRootContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Unmodified, "Object state not properly set to 'Unmodified'.");

            string name = Guid.NewGuid().ToString();
            aContainer.Name = name;

            Assert.AreEqual(name, aContainer.Name, "Property 'Name' not properly set.");
            Assert.IsTrue(aContainer.ObjectState == ObjectState.Modified, "Setting property should alter object state to 'Modified'.");

            aContainer.MarkDeleted();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Deleted, "Object state not properly set to 'Deleted'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SetRootContainerUnmodifiedFromDeleted()
        {
            EAVRootContainer aContainer = new EAVRootContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Unmodified, "Object state not properly set to 'Unmodified'.");

            aContainer.MarkDeleted();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Deleted, "Object state not properly set to 'Deleted'.");

            aContainer.MarkUnmodified();
        }

        [TestMethod]
        public void SetRootContainerModifiedFromDeleted()
        {
            EAVRootContainer aContainer = new EAVRootContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Unmodified, "Object state not properly set to 'Unmodified'.");

            aContainer.MarkDeleted();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Deleted, "Object state not properly set to 'Deleted'.");

            aContainer.Name = Guid.NewGuid().ToString();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Deleted, "Setting property should not alter object state from 'Deleted'.");
        }
        #endregion

        #region RootContainerID Property Tests
        [TestMethod]
        public void TestRootContainerIDForNewRootContainer()
        {
            EAVRootContainer aContainer = new EAVRootContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            int id = rng.Next();
            aContainer.ContainerID = id;

            Assert.AreEqual(id, aContainer.ContainerID, "Property 'RootContainerID' was not set correctly.");
            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Setting property in 'New' state should not alter object state.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestRootContainerIDForUnmodifiedRootContainer()
        {
            EAVRootContainer aContainer = new EAVRootContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            int id = rng.Next();
            aContainer.ContainerID = id;

            Assert.AreEqual(id, aContainer.ContainerID, "Property 'RootContainerID' was not set correctly.");
            Assert.IsTrue(aContainer.ObjectState == ObjectState.Unmodified, "Setting null property 'RootContainerID' in 'Unmodified' state should not alter object state.");

            aContainer.ContainerID = id;
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestRootContainerIDForModifiedRootContainer()
        {
            EAVRootContainer aContainer = new EAVRootContainer();

            int id = rng.Next();
            aContainer.ContainerID = id;

            Assert.AreEqual(id, aContainer.ContainerID, "Property 'RootContainerID' was not set correctly.");

            aContainer.MarkUnmodified();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            string name = Guid.NewGuid().ToString();
            aContainer.Name = name;

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Modified, "Object was not correctly marked 'Modified'.");

            aContainer.ContainerID = id;
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestRootContainerIDForDeletedRootContainer()
        {
            EAVRootContainer aContainer = new EAVRootContainer();

            aContainer.MarkUnmodified();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            aContainer.MarkDeleted();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Deleted, "Object was not correctly marked 'Deleted'.");

            aContainer.ContainerID = rng.Next();
        }
        #endregion

        #region Name Property Tests
        [TestMethod]
        public void TestNameForNewRootContainer()
        {
            EAVRootContainer aContainer = new EAVRootContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            string name = Guid.NewGuid().ToString();
            aContainer.Name = name;

            Assert.AreEqual(name, aContainer.Name, "Property 'Name' was not set correctly.");
            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Setting property in 'New' state should not alter object state.");
        }

        [TestMethod]
        public void TestNameForUnmodifiedRootContainer()
        {
            EAVRootContainer aContainer = new EAVRootContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            string name = Guid.NewGuid().ToString();
            aContainer.Name = name;

            Assert.AreEqual(name, aContainer.Name, "Property 'Name' was not set correctly.");
            Assert.IsTrue(aContainer.ObjectState == ObjectState.Modified, "Setting property in 'Unmodified' state should alter state to 'Modified'.");
        }

        [TestMethod]
        public void TestNameForModifiedRootContainer()
        {
            EAVRootContainer aContainer = new EAVRootContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            string name = Guid.NewGuid().ToString();
            aContainer.Name = name;

            Assert.AreEqual(name, aContainer.Name, "Property 'Name' was not set correctly.");
            Assert.IsTrue(aContainer.ObjectState == ObjectState.Modified, "Setting property in 'Unmodified' state should alter state to 'Modified'.");

            string oldName = name;
            name = Guid.NewGuid().ToString();

            Assert.AreNotEqual(oldName, name, "New value selected for name is the same as the old value.");

            aContainer.Name = name;

            Assert.AreEqual(name, aContainer.Name, "Property 'Name' was not set correctly.");
            Assert.IsTrue(aContainer.ObjectState == ObjectState.Modified, "Setting property in 'Modified' state should not alter object state.");
        }

        [TestMethod]
        public void TestNameForDeletedRootContainer()
        {
            EAVRootContainer aContainer = new EAVRootContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            aContainer.MarkDeleted();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Deleted, "Object was not correctly marked 'Deleted'.");

            aContainer.Name = Guid.NewGuid().ToString();

            Assert.IsNull(aContainer.Name, "Setting property in 'Deleted' state should not alter property value.");
            Assert.IsTrue(aContainer.ObjectState == ObjectState.Deleted, "Setting property in 'Deleted' state should not alter object state.");
        }
        #endregion

        #region DataName Property Tests
        [TestMethod]
        public void TestDataNameForNewRootContainer()
        {
            EAVRootContainer aContainer = new EAVRootContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            string dataName = Guid.NewGuid().ToString();
            aContainer.DataName = dataName;

            Assert.AreEqual(dataName, aContainer.DataName, "Property 'DataName' was not set correctly.");
            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Setting property in 'New' state should not alter object state.");
        }

        [TestMethod]
        public void TestDataNameForUnmodifiedRootContainer()
        {
            EAVRootContainer aContainer = new EAVRootContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            string dataName = Guid.NewGuid().ToString();
            aContainer.DataName = dataName;

            Assert.AreEqual(dataName, aContainer.DataName, "Property 'DataName' was not set correctly.");
            Assert.IsTrue(aContainer.ObjectState == ObjectState.Modified, "Setting property in 'Unmodified' state should alter state to 'Modified'.");
        }

        [TestMethod]
        public void TestDataNameForModifiedRootContainer()
        {
            EAVRootContainer aContainer = new EAVRootContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            string dataName = Guid.NewGuid().ToString();
            aContainer.DataName = dataName;

            Assert.AreEqual(dataName, aContainer.DataName, "Property 'DataName' was not set correctly.");
            Assert.IsTrue(aContainer.ObjectState == ObjectState.Modified, "Setting property in 'Unmodified' state should alter state to 'Modified'.");

            string oldDataName = dataName;
            dataName = Guid.NewGuid().ToString();

            Assert.AreNotEqual(oldDataName, dataName, "New value selected for name is the same as the old value.");

            aContainer.DataName = dataName;

            Assert.AreEqual(dataName, aContainer.DataName, "Property 'DataName' was not set correctly.");
            Assert.IsTrue(aContainer.ObjectState == ObjectState.Modified, "Setting property in 'Modified' state should not alter object state.");
        }

        [TestMethod]
        public void TestDataNameForDeletedRootContainer()
        {
            EAVRootContainer aContainer = new EAVRootContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            aContainer.MarkDeleted();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Deleted, "Object was not correctly marked 'Deleted'.");

            aContainer.DataName = Guid.NewGuid().ToString();

            Assert.IsNull(aContainer.DataName, "Setting property in 'Deleted' state should not alter property value.");
            Assert.IsTrue(aContainer.ObjectState == ObjectState.Deleted, "Setting property in 'Deleted' state should not alter object state.");
        }
        #endregion

        #region DisplayText Property Tests
        [TestMethod]
        public void TestDisplayTextForNewRootContainer()
        {
            EAVRootContainer aContainer = new EAVRootContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            string displayText = Guid.NewGuid().ToString();
            aContainer.DisplayText = displayText;

            Assert.AreEqual(displayText, aContainer.DisplayText, "Property 'DisplayText' was not set correctly.");
            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Setting property in 'New' state should not alter object state.");
        }

        [TestMethod]
        public void TestDisplayTextForUnmodifiedRootContainer()
        {
            EAVRootContainer aContainer = new EAVRootContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            string displayText = Guid.NewGuid().ToString();
            aContainer.DisplayText = displayText;

            Assert.AreEqual(displayText, aContainer.DisplayText, "Property 'DisplayText' was not set correctly.");
            Assert.IsTrue(aContainer.ObjectState == ObjectState.Modified, "Setting property in 'Unmodified' state should alter state to 'Modified'.");
        }

        [TestMethod]
        public void TestDisplayTextForModifiedRootContainer()
        {
            EAVRootContainer aContainer = new EAVRootContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            string displayText = Guid.NewGuid().ToString();
            aContainer.DisplayText = displayText;

            Assert.AreEqual(displayText, aContainer.DisplayText, "Property 'DisplayText' was not set correctly.");
            Assert.IsTrue(aContainer.ObjectState == ObjectState.Modified, "Setting property in 'Unmodified' state should alter state to 'Modified'.");

            string oldDisplayText = displayText;
            displayText = Guid.NewGuid().ToString();

            Assert.AreNotEqual(oldDisplayText, displayText, "New value selected for name is the same as the old value.");

            aContainer.DisplayText = displayText;

            Assert.AreEqual(displayText, aContainer.DisplayText, "Property 'DisplayText' was not set correctly.");
            Assert.IsTrue(aContainer.ObjectState == ObjectState.Modified, "Setting property in 'Modified' state should not alter object state.");
        }

        [TestMethod]
        public void TestDisplayTextForDeletedRootContainer()
        {
            EAVRootContainer aContainer = new EAVRootContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            aContainer.MarkDeleted();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Deleted, "Object was not correctly marked 'Deleted'.");

            aContainer.DisplayText= Guid.NewGuid().ToString();

            Assert.IsNull(aContainer.DisplayText, "Setting property in 'Deleted' state should not alter property value.");
            Assert.IsTrue(aContainer.ObjectState == ObjectState.Deleted, "Setting property in 'Deleted' state should not alter object state.");
        }
        #endregion

        #region IsRepeating Property Tests
        [TestMethod]
        public void TestIsRepeatingForNewRootContainer()
        {
            EAVRootContainer aContainer = new EAVRootContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContainer.IsRepeating = true;

            Assert.AreEqual(true, aContainer.IsRepeating, "Property 'IsRepeating' was not set correctly.");
            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Setting property in 'New' state should not alter object state.");
        }

        [TestMethod]
        public void TestIsRepeatingForUnmodifiedRootContainer()
        {
            EAVRootContainer aContainer = new EAVRootContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            aContainer.IsRepeating = true;

            Assert.AreEqual(true, aContainer.IsRepeating, "Property 'IsRepeating' was not set correctly.");
            Assert.IsTrue(aContainer.ObjectState == ObjectState.Modified, "Setting property in 'Unmodified' state should alter state to 'Modified'.");
        }

        [TestMethod]
        public void TestIsRepeatingForModifiedRootContainer()
        {
            EAVRootContainer aContainer = new EAVRootContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            aContainer.IsRepeating = true;

            Assert.AreEqual(true, aContainer.IsRepeating, "Property 'IsRepeating' was not set correctly.");
            Assert.IsTrue(aContainer.ObjectState == ObjectState.Modified, "Setting property in 'Unmodified' state should alter state to 'Modified'.");

            aContainer.IsRepeating = false;

            Assert.AreEqual(false, aContainer.IsRepeating, "Property 'IsRepeating' was not set correctly.");
            Assert.IsTrue(aContainer.ObjectState == ObjectState.Modified, "Setting property in 'Modified' state should not alter object state.");
        }

        [TestMethod]
        public void TestIsRepeatingForDeletedRootContainer()
        {
            EAVRootContainer aContainer = new EAVRootContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            aContainer.MarkDeleted();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Deleted, "Object was not correctly marked 'Deleted'.");

            aContainer.IsRepeating = true;

            Assert.IsFalse(aContainer.IsRepeating, "Setting property in 'Deleted' state should not alter property value.");
            Assert.IsTrue(aContainer.ObjectState == ObjectState.Deleted, "Setting property in 'Deleted' state should not alter object state.");
        }
        #endregion

        #region Context Property Tests
        [TestMethod]
        public void TestContextForNewRootContainer()
        {
            EAVContainer aContainer = new EAVRootContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            EAVContext aContext = new EAVContext();
            aContainer.Context = aContext;

            Assert.AreEqual(aContext, aContainer.Context, "Property 'Context' was not set correctly.");
            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Setting property in 'New' state should not alter object state.");
        }

        [TestMethod]
        public void TestContextForUnmodifiedRootContainer()
        {
            EAVContainer aContainer = new EAVRootContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            EAVContext aContext = new EAVContext();
            aContainer.Context = aContext;

            Assert.AreEqual(aContext, aContainer.Context, "Property 'Context' was not set correctly.");
            Assert.IsTrue(aContainer.ObjectState == ObjectState.Modified, "Setting property in 'Unmodified' state should alter state to 'Modified'.");
        }

        [TestMethod]
        public void TestContextForModifiedRootContainer()
        {
            EAVContainer aContainer = new EAVRootContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            EAVContext aContext = new EAVContext();
            aContainer.Context = aContext;

            Assert.AreEqual(aContext, aContainer.Context, "Property 'Context' was not set correctly.");
            Assert.IsTrue(aContainer.ObjectState == ObjectState.Modified, "Setting property in 'Unmodified' state should alter state to 'Modified'.");

            aContext = new EAVContext();
            aContainer.Context = aContext;

            Assert.AreEqual(aContext, aContainer.Context, "Property 'Context' was not set correctly.");
            Assert.IsTrue(aContainer.ObjectState == ObjectState.Modified, "Setting property in 'Modified' state should not alter object state.");
        }

        [TestMethod]
        public void TestContextForDeletedRootContainer()
        {
            EAVContainer aContainer = new EAVRootContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            aContainer.MarkDeleted();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Deleted, "Object was not correctly marked 'Deleted'.");

            EAVContext aContext = new EAVContext();
            aContainer.Context = aContext;

            Assert.AreNotEqual(aContext, aContainer.Context, "Setting property in 'Deleted' state should not alter property value.");
            Assert.IsTrue(aContainer.ObjectState == ObjectState.Deleted, "Setting property in 'Deleted' state should not alter object state.");
        }

        [TestMethod]
        public void TestContextInteractionWithRootContainer()
        {
            int containerID = rng.Next();
            EAVRootContainer aContainer = new EAVRootContainer() { ContainerID = containerID };
            int contextID = rng.Next();
            EAVContext aContext = new EAVContext() { ContextID = contextID };

            Assert.AreEqual(containerID, aContainer.ContainerID, "Property 'ContainerID' was not set properly.");
            Assert.AreEqual(contextID, aContext.ContextID, "Property 'ContextID' was not set properly.");

            Assert.IsNull(aContainer.Context, "Property 'Context' should be null.");
            Assert.IsTrue(aContext.Containers.Count == 0, "Collection property 'Containers' of EAVContext object should be empty.");

            aContainer.Context = aContext;

            Assert.AreEqual(aContext, aContainer.Context, "Property 'Context' was not set properly.");
            Assert.AreEqual(contextID, aContainer.Context.ContextID, "Property 'ContextID' was not set properly.");

            Assert.IsTrue(aContext.Containers.Count == 1, "Collection property 'Containers' was not set properly.");
            Assert.AreEqual(containerID, aContext.Containers.First().ContainerID, "Property 'ContainerID' was not set properly.");

            containerID = rng.Next();
            aContainer = new EAVRootContainer() { ContainerID = containerID };
            contextID = rng.Next();
            aContext = new EAVContext() { ContextID = contextID };

            aContext.Containers.Add(aContainer);

            Assert.AreEqual(aContext, aContainer.Context, "Property 'Context' was not set properly.");
            Assert.AreEqual(contextID, aContainer.Context.ContextID, "Property 'ContextID' was not set properly.");

            Assert.IsTrue(aContext.Containers.Count == 1, "Collection property 'Containers' was not set properly.");
            Assert.AreEqual(containerID, aContext.Containers.First().ContainerID, "Property 'ContainerID' was not set properly.");
        }
        #endregion

        #region ParentContainer Property Tests
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestParentContainerForNewRootContainer()
        {
            EAVRootContainer aContainer = new EAVRootContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");
            Assert.IsNull(aContainer.ParentContainer, "Property 'ParentContainer' shoud be null on creation.");

            aContainer.ParentContainer = new EAVRootContainer();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestParentContainerForUnmodifiedRootContainer()
        {
            EAVRootContainer aContainer = new EAVRootContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            aContainer.ParentContainer = new EAVRootContainer();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestParentContainerForModifiedRootContainer()
        {
            EAVRootContainer aContainer = new EAVRootContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            aContainer.ParentContainer = new EAVRootContainer();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestParentContainerForDeletedRootContainer()
        {
            EAVRootContainer aContainer = new EAVRootContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            aContainer.MarkDeleted();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Deleted, "Object was not correctly marked 'Deleted'.");

            aContainer.ParentContainer = new EAVRootContainer();
        }
        #endregion

        #region ChildContainers Property Tests
        [TestMethod]
        public void TestChildContainersForNewRootContainer()
        {
            EAVRootContainer aContainer = new EAVRootContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContainer.ChildContainers.Add(new EAVChildContainer());
            aContainer.ChildContainers.Add(new EAVChildContainer());

            Assert.IsTrue(aContainer.ChildContainers.Count == 2, "Property 'ChildContainers' was not set correctly.");
            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Modifying property in 'New' state should not alter object state.");

            aContainer.ChildContainers.Remove(aContainer.ChildContainers.First());

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Modifying property in 'New' state should not alter object state.");

            aContainer.ChildContainers.Clear();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Modifying property in 'New' state should not alter object state.");
        }

        [TestMethod]
        public void TestChildContainersForUnmodifiedRootContainer()
        {
            EAVRootContainer aContainer = new EAVRootContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            aContainer.ChildContainers.Add(new EAVChildContainer());
            aContainer.ChildContainers.Add(new EAVChildContainer());

            Assert.IsTrue(aContainer.ChildContainers.Count == 2, "Property 'ChildContainers' was not set correctly.");
            Assert.IsTrue(aContainer.ObjectState == ObjectState.Modified, "Modifying property in 'Unmodified' state should alter object state to 'Modified'.");

            aContainer.MarkUnmodified();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            aContainer.ChildContainers.Remove(aContainer.ChildContainers.First());

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Modified, "Modifying property in 'Unmodified' state should alter object state to 'Modified'.");

            aContainer.MarkUnmodified();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            aContainer.ChildContainers.Clear();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Modified, "Modifying property in 'Unmodified' state should alter object state to 'Modified'.");
        }

        [TestMethod]
        public void TestChildContainersForModifiedRootContainer()
        {
            EAVRootContainer aContainer = new EAVRootContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            aContainer.Name = Guid.NewGuid().ToString();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Modified, "Object was not correctly marked 'Modified'.");

            aContainer.ChildContainers.Add(new EAVChildContainer());
            aContainer.ChildContainers.Add(new EAVChildContainer());

            Assert.IsTrue(aContainer.ChildContainers.Count == 2, "Property 'ChildContainers' was not set correctly.");
            Assert.IsTrue(aContainer.ObjectState == ObjectState.Modified, "Modifying property in 'Modified' state should not alter object state.");

            aContainer.ChildContainers.Remove(aContainer.ChildContainers.First());

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Modified, "Modifying property in 'Modified' state should not alter object state.");

            aContainer.ChildContainers.Clear();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Modified, "Modifying property in 'Modified' state should not alter object state.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestChildContainersForDeletedRootContainerWithAdd()
        {
            EAVRootContainer aContainer = new EAVRootContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            aContainer.MarkDeleted();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Deleted, "Object was not correctly marked 'Deleted'.");

            aContainer.ChildContainers.Add(new EAVChildContainer());
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestChildContainersForDeletedRootContainerWithRemove()
        {
            EAVRootContainer aContainer = new EAVRootContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContainer.ChildContainers.Add(new EAVChildContainer());
            aContainer.ChildContainers.Add(new EAVChildContainer());

            aContainer.MarkUnmodified();

            foreach (EAVChildContainer childContainer in aContainer.ChildContainers)
            {
                childContainer.MarkUnmodified();
            }

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            aContainer.MarkDeleted();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Deleted, "Object was not correctly marked 'Deleted'.");

            aContainer.ChildContainers.Remove(aContainer.ChildContainers.First());
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestChildContainersForDeletedRootContainerWithClear()
        {
            EAVRootContainer aContainer = new EAVRootContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContainer.ChildContainers.Add(new EAVChildContainer());

            aContainer.MarkUnmodified();

            foreach (EAVChildContainer childContainer in aContainer.ChildContainers)
            {
                childContainer.MarkUnmodified();
            }

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            aContainer.MarkDeleted();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Deleted, "Object was not correctly marked 'Deleted'.");

            aContainer.ChildContainers.Clear();
        }

        [TestMethod]
        public void TestChildContainersInteractionWithRootContainer()
        {
            EAVRootContainer aContainer = new EAVRootContainer();

            int id = rng.Next();
            aContainer.ContainerID = id;

            EAVChildContainer aChildContainer = new EAVChildContainer();
            aContainer.ChildContainers.Add(aChildContainer);

            Assert.IsTrue(aContainer.ChildContainers.Count == 1, "Collection property 'ChildContainers' is empty.");
            Assert.IsTrue(aContainer.ChildContainers.Single() == aChildContainer, "Collection property 'ChildContainers' not modified properly.");

            Assert.IsNotNull(aChildContainer.ParentContainer, "Property 'ParentContainer' is null.");
            Assert.AreEqual(aContainer, aChildContainer.ParentContainer, "Property 'ParentContainer' not set properly.");

            Assert.IsNotNull(aChildContainer.ParentContainerID, "Propert 'ParentContainerID' is null.");
            Assert.AreEqual(id, aChildContainer.ParentContainerID, "Property 'ParentContainerID' not set properly.");
        }
        #endregion

        #region Attributes Property Tests
        [TestMethod]
        public void TestAttributesForNewRootContainer()
        {
            EAVRootContainer aContainer = new EAVRootContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContainer.Attributes.Add(new EAVAttribute());
            aContainer.Attributes.Add(new EAVAttribute());

            Assert.IsTrue(aContainer.Attributes.Count == 2, "Property 'Attributes' was not set correctly.");
            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Modifying property in 'New' state should not alter object state.");

            aContainer.Attributes.Remove(aContainer.Attributes.First());

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Modifying property in 'New' state should not alter object state.");

            aContainer.Attributes.Clear();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Modifying property in 'New' state should not alter object state.");
        }

        [TestMethod]
        public void TestAttributesForUnmodifiedRootContainer()
        {
            EAVRootContainer aContainer = new EAVRootContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            aContainer.Attributes.Add(new EAVAttribute());
            aContainer.Attributes.Add(new EAVAttribute());

            Assert.IsTrue(aContainer.Attributes.Count == 2, "Property 'Attributes' was not set correctly.");
            Assert.IsTrue(aContainer.ObjectState == ObjectState.Modified, "Modifying property in 'Unmodified' state should alter object state to 'Modified'.");

            aContainer.MarkUnmodified();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            aContainer.Attributes.Remove(aContainer.Attributes.First());

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Modified, "Modifying property in 'Unmodified' state should alter object state to 'Modified'.");

            aContainer.MarkUnmodified();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            aContainer.Attributes.Clear();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Modified, "Modifying property in 'Unmodified' state should alter object state to 'Modified'.");
        }

        [TestMethod]
        public void TestAttributesForModifiedRootContainer()
        {
            EAVRootContainer aContainer = new EAVRootContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            aContainer.Name = Guid.NewGuid().ToString();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Modified, "Object was not correctly marked 'Modified'.");

            aContainer.Attributes.Add(new EAVAttribute());
            aContainer.Attributes.Add(new EAVAttribute());

            Assert.IsTrue(aContainer.Attributes.Count == 2, "Property 'Attributes' was not set correctly.");
            Assert.IsTrue(aContainer.ObjectState == ObjectState.Modified, "Modifying property in 'Modified' state should not alter object state.");

            aContainer.Attributes.Remove(aContainer.Attributes.First());

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Modified, "Modifying property in 'Modified' state should not alter object state.");

            aContainer.Attributes.Clear();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Modified, "Modifying property in 'Modified' state should not alter object state.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestAttributesForDeletedRootContainerWithAdd()
        {
            EAVRootContainer aContainer = new EAVRootContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            aContainer.MarkDeleted();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Deleted, "Object was not correctly marked 'Deleted'.");

            aContainer.Attributes.Add(new EAVAttribute());
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestAttributesForDeletedRootContainerWithRemove()
        {
            EAVRootContainer aContainer = new EAVRootContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContainer.Attributes.Add(new EAVAttribute());
            aContainer.Attributes.Add(new EAVAttribute());

            aContainer.MarkUnmodified();

            foreach (EAVAttribute childContainer in aContainer.Attributes)
            {
                childContainer.MarkUnmodified();
            }

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            aContainer.MarkDeleted();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Deleted, "Object was not correctly marked 'Deleted'.");

            aContainer.Attributes.Remove(aContainer.Attributes.First());
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestAttributesForDeletedRootContainerWithClear()
        {
            EAVRootContainer aContainer = new EAVRootContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContainer.Attributes.Add(new EAVAttribute());

            aContainer.MarkUnmodified();

            foreach (EAVAttribute childContainer in aContainer.Attributes)
            {
                childContainer.MarkUnmodified();
            }

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            aContainer.MarkDeleted();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Deleted, "Object was not correctly marked 'Deleted'.");

            aContainer.Attributes.Clear();
        }

        [TestMethod]
        public void TestAttributesInteractionWithRootContainer()
        {
            EAVRootContainer aContainer = new EAVRootContainer();

            int id = rng.Next();
            aContainer.ContainerID = id;

            EAVAttribute anAttribute = new EAVAttribute();
            aContainer.Attributes.Add(anAttribute);

            Assert.IsTrue(aContainer.Attributes.Count == 1, "Collection property 'Attributes' is empty.");
            Assert.IsTrue(aContainer.Attributes.Single() == anAttribute, "Collection property 'Attributes' not modified properly.");

            Assert.IsNotNull(anAttribute.Container, "Property 'Container' is null.");
            Assert.AreEqual(aContainer, anAttribute.Container, "Property 'Container' not set properly.");

            Assert.IsNotNull(anAttribute.ContainerID, "Propert 'ContainerID' is null.");
            Assert.AreEqual(id, anAttribute.ContainerID, "Property 'ContainerID' not set properly.");
        }
        #endregion

        #region Instances Property Tests
        [TestMethod]
        public void TestInstancesForNewRootContainer()
        {
            EAVRootContainer aContainer = new EAVRootContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContainer.Instances.Add(new EAVRootInstance());
            aContainer.Instances.Add(new EAVRootInstance());

            Assert.IsTrue(aContainer.Instances.Count == 2, "Property 'Instances' was not set correctly.");
            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Modifying property in 'New' state should not alter object state.");

            aContainer.Instances.Remove(aContainer.Instances.First());

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Modifying property in 'New' state should not alter object state.");

            aContainer.Instances.Clear();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Modifying property in 'New' state should not alter object state.");
        }

        [TestMethod]
        public void TestInstancesForUnmodifiedRootContainer()
        {
            EAVRootContainer aContainer = new EAVRootContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            aContainer.Instances.Add(new EAVRootInstance());
            aContainer.Instances.Add(new EAVRootInstance());

            Assert.IsTrue(aContainer.Instances.Count == 2, "Property 'Instances' was not set correctly.");
            Assert.IsTrue(aContainer.ObjectState == ObjectState.Modified, "Modifying property in 'Unmodified' state should alter object state to 'Modified'.");

            aContainer.MarkUnmodified();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            aContainer.Instances.Remove(aContainer.Instances.First());

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Modified, "Modifying property in 'Unmodified' state should alter object state to 'Modified'.");

            aContainer.MarkUnmodified();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            aContainer.Instances.Clear();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Modified, "Modifying property in 'Unmodified' state should alter object state to 'Modified'.");
        }

        [TestMethod]
        public void TestInstancesForModifiedRootContainer()
        {
            EAVRootContainer aContainer = new EAVRootContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            aContainer.Name = Guid.NewGuid().ToString();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Modified, "Object was not correctly marked 'Modified'.");

            aContainer.Instances.Add(new EAVRootInstance());
            aContainer.Instances.Add(new EAVRootInstance());

            Assert.IsTrue(aContainer.Instances.Count == 2, "Property 'Instances' was not set correctly.");
            Assert.IsTrue(aContainer.ObjectState == ObjectState.Modified, "Modifying property in 'Modified' state should not alter object state.");

            aContainer.Instances.Remove(aContainer.Instances.First());

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Modified, "Modifying property in 'Modified' state should not alter object state.");

            aContainer.Instances.Clear();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Modified, "Modifying property in 'Modified' state should not alter object state.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestInstancesForDeletedRootContainerWithAdd()
        {
            EAVRootContainer aContainer = new EAVRootContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            aContainer.MarkDeleted();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Deleted, "Object was not correctly marked 'Deleted'.");

            aContainer.Instances.Add(new EAVRootInstance());
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestInstancesForDeletedRootContainerWithRemove()
        {
            EAVRootContainer aContainer = new EAVRootContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContainer.Instances.Add(new EAVRootInstance());
            aContainer.Instances.Add(new EAVRootInstance());

            aContainer.MarkUnmodified();

            foreach (EAVInstance childContainer in aContainer.Instances)
            {
                childContainer.MarkUnmodified();
            }

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            aContainer.MarkDeleted();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Deleted, "Object was not correctly marked 'Deleted'.");

            aContainer.Instances.Remove(aContainer.Instances.First());
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestInstancesForDeletedRootContainerWithClear()
        {
            EAVRootContainer aContainer = new EAVRootContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContainer.Instances.Add(new EAVRootInstance());

            aContainer.MarkUnmodified();

            foreach (EAVInstance childContainer in aContainer.Instances)
            {
                childContainer.MarkUnmodified();
            }

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            aContainer.MarkDeleted();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Deleted, "Object was not correctly marked 'Deleted'.");

            aContainer.Instances.Clear();
        }

        [TestMethod]
        public void TestInstancesInteractionWithRootContainer()
        {
            EAVRootContainer aContainer = new EAVRootContainer();

            int id = rng.Next();
            aContainer.ContainerID = id;

            EAVInstance anInstance = new EAVRootInstance();
            aContainer.Instances.Add(anInstance);

            Assert.IsTrue(aContainer.Instances.Count == 1, "Collection property 'Instances' is empty.");
            Assert.IsTrue(aContainer.Instances.Single() == anInstance, "Collection property 'Instances' not modified properly.");

            Assert.IsNotNull(anInstance.Container, "Property 'Container' is null.");
            Assert.AreEqual(aContainer, anInstance.Container, "Property 'Container' not set properly.");

            Assert.IsNotNull(anInstance.ContainerID, "Propert 'ContainerID' is null.");
            Assert.AreEqual(id, anInstance.ContainerID, "Property 'ContainerID' not set properly.");
        }
        #endregion
    }
}
