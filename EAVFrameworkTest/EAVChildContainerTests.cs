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
        public void CreateChildContainer()
        {
            EAVChildContainer aContainer = new EAVChildContainer();

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
        }

        [TestMethod]
        public void SetChildContainerUnmodifiedFromNew()
        {
            EAVChildContainer aContainer = new EAVChildContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Unmodified, "Object state not properly set to 'Unmodified'.");
        }

        [TestMethod]
        public void SetChildContainerModifiedFromNew()
        {
            EAVChildContainer aContainer = new EAVChildContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            string name = Guid.NewGuid().ToString();
            aContainer.Name = name;

            Assert.AreEqual(name, aContainer.Name, "Property 'Name' not properly set.");

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Setting property in 'New' state should not alter object state.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SetChildContainerDeletedFromNew()
        {
            EAVChildContainer aContainer = new EAVChildContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContainer.MarkDeleted();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Setting object state to 'Deleted' from 'New' state should not alter object state.");
        }

        [TestMethod]
        public void SetChildContainerModifiedFromUnmodified()
        {
            EAVChildContainer aContainer = new EAVChildContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Unmodified, "Object state not properly set to 'Unmodified'.");

            string name = Guid.NewGuid().ToString();
            aContainer.Name = name;

            Assert.AreEqual(name, aContainer.Name, "Property 'Name' not properly set.");
            Assert.IsTrue(aContainer.ObjectState == ObjectState.Modified, "Setting property should alter object state to 'Modified'.");
        }

        [TestMethod]
        public void SetChildContainerDeletedFromUnmodified()
        {
            EAVChildContainer aContainer = new EAVChildContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Unmodified, "Object state not properly set to 'Unmodified'.");

            aContainer.MarkDeleted();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Deleted, "Object state not properly set to 'Deleted'.");
        }

        [TestMethod]
        public void SetChildContainerUnmodifiedFromModified()
        {
            EAVChildContainer aContainer = new EAVChildContainer();

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
        public void SetChildContainerDeletedFromModified()
        {
            EAVChildContainer aContainer = new EAVChildContainer();

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
        public void SetChildContainerUnmodifiedFromDeleted()
        {
            EAVChildContainer aContainer = new EAVChildContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Unmodified, "Object state not properly set to 'Unmodified'.");

            aContainer.MarkDeleted();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Deleted, "Object state not properly set to 'Deleted'.");

            aContainer.MarkUnmodified();
        }

        [TestMethod]
        public void SetChildContainerModifiedFromDeleted()
        {
            EAVChildContainer aContainer = new EAVChildContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Unmodified, "Object state not properly set to 'Unmodified'.");

            aContainer.MarkDeleted();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Deleted, "Object state not properly set to 'Deleted'.");

            aContainer.Name = Guid.NewGuid().ToString();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Deleted, "Setting property should not alter object state from 'Deleted'.");
        }
        #endregion

        #region ChildContainer ID Property Tests
        [TestMethod]
        public void TestChildContainerIDForNewChildContainer()
        {
            EAVChildContainer aContainer = new EAVChildContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            int id = rng.Next();
            aContainer.ContainerID = id;

            Assert.AreEqual(id, aContainer.ContainerID, "Property 'ChildContainerID' was not set correctly.");
            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Setting property in 'New' state should not alter object state.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestChildContainerIDForUnmodifiedChildContainer()
        {
            EAVChildContainer aContainer = new EAVChildContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            int id = rng.Next();
            aContainer.ContainerID = id;

            Assert.AreEqual(id, aContainer.ContainerID, "Property 'ChildContainerID' was not set correctly.");
            Assert.IsTrue(aContainer.ObjectState == ObjectState.Unmodified, "Setting null property 'ChildContainerID' in 'Unmodified' state should not alter object state.");

            aContainer.ContainerID = id;
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestChildContainerIDForModifiedChildContainer()
        {
            EAVChildContainer aContainer = new EAVChildContainer();

            int id = rng.Next();
            aContainer.ContainerID = id;

            Assert.AreEqual(id, aContainer.ContainerID, "Property 'ChildContainerID' was not set correctly.");

            aContainer.MarkUnmodified();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            string name = Guid.NewGuid().ToString();
            aContainer.Name = name;

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Modified, "Object was not correctly marked 'Modified'.");

            aContainer.ContainerID = id;
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestChildContainerIDForDeletedChildContainer()
        {
            EAVChildContainer aContainer = new EAVChildContainer();

            aContainer.MarkUnmodified();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            aContainer.MarkDeleted();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Deleted, "Object was not correctly marked 'Deleted'.");

            aContainer.ContainerID = rng.Next();
        }
        #endregion

        #region Name Property Tests
        [TestMethod]
        public void TestNameForNewChildContainer()
        {
            EAVChildContainer aContainer = new EAVChildContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            string name = Guid.NewGuid().ToString();
            aContainer.Name = name;

            Assert.AreEqual(name, aContainer.Name, "Property 'Name' was not set correctly.");
            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Setting property in 'New' state should not alter object state.");
        }

        [TestMethod]
        public void TestNameForUnmodifiedChildContainer()
        {
            EAVChildContainer aContainer = new EAVChildContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            string name = Guid.NewGuid().ToString();
            aContainer.Name = name;

            Assert.AreEqual(name, aContainer.Name, "Property 'Name' was not set correctly.");
            Assert.IsTrue(aContainer.ObjectState == ObjectState.Modified, "Setting property in 'Unmodified' state should alter state to 'Modified'.");
        }

        [TestMethod]
        public void TestNameForModifiedChildContainer()
        {
            EAVChildContainer aContainer = new EAVChildContainer();

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
        public void TestNameForDeletedChildContainer()
        {
            EAVChildContainer aContainer = new EAVChildContainer();

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
        public void TestDataNameForNewChildContainer()
        {
            EAVChildContainer aContainer = new EAVChildContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            string dataName = Guid.NewGuid().ToString();
            aContainer.DataName = dataName;

            Assert.AreEqual(dataName, aContainer.DataName, "Property 'DataName' was not set correctly.");
            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Setting property in 'New' state should not alter object state.");
        }

        [TestMethod]
        public void TestDataNameForUnmodifiedChildContainer()
        {
            EAVChildContainer aContainer = new EAVChildContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            string dataName = Guid.NewGuid().ToString();
            aContainer.DataName = dataName;

            Assert.AreEqual(dataName, aContainer.DataName, "Property 'DataName' was not set correctly.");
            Assert.IsTrue(aContainer.ObjectState == ObjectState.Modified, "Setting property in 'Unmodified' state should alter state to 'Modified'.");
        }

        [TestMethod]
        public void TestDataNameForModifiedChildContainer()
        {
            EAVChildContainer aContainer = new EAVChildContainer();

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
        public void TestDataNameForDeletedChildContainer()
        {
            EAVChildContainer aContainer = new EAVChildContainer();

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
        public void TestDisplayTextForNewChildContainer()
        {
            EAVChildContainer aContainer = new EAVChildContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            string displayText = Guid.NewGuid().ToString();
            aContainer.DisplayText = displayText;

            Assert.AreEqual(displayText, aContainer.DisplayText, "Property 'DisplayText' was not set correctly.");
            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Setting property in 'New' state should not alter object state.");
        }

        [TestMethod]
        public void TestDisplayTextForUnmodifiedChildContainer()
        {
            EAVChildContainer aContainer = new EAVChildContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            string displayText = Guid.NewGuid().ToString();
            aContainer.DisplayText = displayText;

            Assert.AreEqual(displayText, aContainer.DisplayText, "Property 'DisplayText' was not set correctly.");
            Assert.IsTrue(aContainer.ObjectState == ObjectState.Modified, "Setting property in 'Unmodified' state should alter state to 'Modified'.");
        }

        [TestMethod]
        public void TestDisplayTextForModifiedChildContainer()
        {
            EAVChildContainer aContainer = new EAVChildContainer();

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
        public void TestDisplayTextForDeletedChildContainer()
        {
            EAVChildContainer aContainer = new EAVChildContainer();

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
        public void TestIsRepeatingForNewChildContainer()
        {
            EAVChildContainer aContainer = new EAVChildContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContainer.IsRepeating = true;

            Assert.AreEqual(true, aContainer.IsRepeating, "Property 'IsRepeating' was not set correctly.");
            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Setting property in 'New' state should not alter object state.");
        }

        [TestMethod]
        public void TestIsRepeatingForUnmodifiedChildContainer()
        {
            EAVChildContainer aContainer = new EAVChildContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            aContainer.IsRepeating = true;

            Assert.AreEqual(true, aContainer.IsRepeating, "Property 'IsRepeating' was not set correctly.");
            Assert.IsTrue(aContainer.ObjectState == ObjectState.Modified, "Setting property in 'Unmodified' state should alter state to 'Modified'.");
        }

        [TestMethod]
        public void TestIsRepeatingForModifiedChildContainer()
        {
            EAVChildContainer aContainer = new EAVChildContainer();

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
        public void TestIsRepeatingForDeletedChildContainer()
        {
            EAVChildContainer aContainer = new EAVChildContainer();

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
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestContextForNewChildContainer()
        {
            EAVContainer aContainer = new EAVChildContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            EAVContext aContext = new EAVContext();
            aContainer.Context = aContext;
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestContextForUnmodifiedChildContainer()
        {
            EAVContainer aContainer = new EAVChildContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            EAVContext aContext = new EAVContext();
            aContainer.Context = aContext;
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestContextForModifiedChildContainer()
        {
            EAVContainer aContainer = new EAVChildContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            EAVContext aContext = new EAVContext();
            aContainer.Context = aContext;
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestContextForDeletedChildContainer()
        {
            EAVContainer aContainer = new EAVChildContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            aContainer.MarkDeleted();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Deleted, "Object was not correctly marked 'Deleted'.");

            EAVContext aContext = new EAVContext();
            aContainer.Context = aContext;
        }
        #endregion

        #region ParentContainer Property Tests
        [TestMethod]
        public void TestParentContainerForNewContainer()
        {
            EAVChildContainer aContainer = new EAVChildContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            EAVRootContainer aParentContainer = new EAVRootContainer();
            aContainer.ParentContainer = aParentContainer;

            Assert.AreEqual(aParentContainer, aContainer.ParentContainer, "Property 'ParentContainer' was not set correctly.");
            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Setting property in 'New' state should not alter object state.");
        }

        [TestMethod]
        public void TestParentContainerForUnmodifiedContainer()
        {
            EAVChildContainer aContainer = new EAVChildContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            EAVRootContainer aParentContainer = new EAVRootContainer();
            aContainer.ParentContainer = aParentContainer;

            Assert.AreEqual(aParentContainer, aContainer.ParentContainer, "Property 'ParentContainer' was not set correctly.");
            Assert.IsTrue(aContainer.ObjectState == ObjectState.Modified, "Setting property in 'Unmodified' state should alter state to 'Modified'.");
        }

        [TestMethod]
        public void TestParentContainerForModifiedContainer()
        {
            EAVChildContainer aContainer = new EAVChildContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            EAVRootContainer aParentContainer = new EAVRootContainer();
            aContainer.ParentContainer = aParentContainer;

            Assert.AreEqual(aParentContainer, aContainer.ParentContainer, "Property 'ParentContainer' was not set correctly.");
            Assert.IsTrue(aContainer.ObjectState == ObjectState.Modified, "Setting property in 'Unmodified' state should alter state to 'Modified'.");

            aParentContainer = new EAVRootContainer();
            aContainer.ParentContainer = aParentContainer;

            Assert.AreEqual(aParentContainer, aContainer.ParentContainer, "Property 'ParentContainer' was not set correctly.");
            Assert.IsTrue(aContainer.ObjectState == ObjectState.Modified, "Setting property in 'Modified' state should not alter object state.");
        }

        [TestMethod]
        public void TestParentContainerForDeletedContainer()
        {
            EAVChildContainer aContainer = new EAVChildContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            aContainer.MarkDeleted();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Deleted, "Object was not correctly marked 'Deleted'.");

            EAVRootContainer aParentContainer = new EAVRootContainer();
            aContainer.ParentContainer = aParentContainer;

            Assert.AreNotEqual(aParentContainer, aContainer.ParentContainer, "Setting property in 'Deleted' state should not alter property value.");
            Assert.IsTrue(aContainer.ObjectState == ObjectState.Deleted, "Setting property in 'Deleted' state should not alter object state.");
        }

        [TestMethod]
        public void TestParentContainerInteractionWithContainer()
        {
            int childContainerID = rng.Next();
            EAVChildContainer aChildContainer = new EAVChildContainer() { ContainerID = childContainerID };
            int rootContainerID = rng.Next();
            EAVRootContainer aParentContainer = new EAVRootContainer() { ContainerID = rootContainerID };

            Assert.AreEqual(childContainerID, aChildContainer.ContainerID, "Property 'ContainerID' was not set properly.");
            Assert.AreEqual(rootContainerID, aParentContainer.ContainerID, "Property 'ContainerID' was not set properly.");

            Assert.IsNull(aChildContainer.ParentContainer, "Property 'ParentContainer' should be null.");
            Assert.IsTrue(aParentContainer.ChildContainers.Count == 0, "Collection property 'ChildContainers' of EAVRootContainer object should be empty.");

            aChildContainer.ParentContainer = aParentContainer;

            Assert.AreEqual(aParentContainer, aChildContainer.ParentContainer, "Property 'ParentContainer' was not set properly.");
            Assert.AreEqual(rootContainerID, aChildContainer.ParentContainer.ContainerID, "Property 'ContainerID' was not set properly.");

            Assert.IsTrue(aParentContainer.ChildContainers.Count == 1, "Collection property 'ChildContainers' was not set properly.");
            Assert.AreEqual(childContainerID, aParentContainer.ChildContainers.First().ContainerID, "Property 'ContainerID' was not set properly.");

            childContainerID = rng.Next();
            aChildContainer = new EAVChildContainer() { ContainerID = childContainerID };
            rootContainerID = rng.Next();
            aParentContainer = new EAVRootContainer() { ContainerID = rootContainerID };

            aParentContainer.ChildContainers.Add(aChildContainer);

            Assert.AreEqual(aParentContainer, aChildContainer.ParentContainer, "Property 'ParentContainer' was not set properly.");
            Assert.AreEqual(rootContainerID, aChildContainer.ParentContainer.ContainerID, "Property 'ContainerID' was not set properly.");

            Assert.IsTrue(aParentContainer.ChildContainers.Count == 1, "Collection property 'ChildContainers' was not set properly.");
            Assert.AreEqual(childContainerID, aParentContainer.ChildContainers.First().ContainerID, "Property 'ContainerID' was not set properly.");
        }
        #endregion

        #region ChildContainers Property Tests
        [TestMethod]
        public void TestChildContainersForNewChildContainer()
        {
            EAVChildContainer aContainer = new EAVChildContainer();

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
        public void TestChildContainersForUnmodifiedChildContainer()
        {
            EAVChildContainer aContainer = new EAVChildContainer();

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
        public void TestChildContainersForModifiedChildContainer()
        {
            EAVChildContainer aContainer = new EAVChildContainer();

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
        public void TestChildContainersForDeletedChildContainerWithAdd()
        {
            EAVChildContainer aContainer = new EAVChildContainer();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            aContainer.MarkDeleted();

            Assert.IsTrue(aContainer.ObjectState == ObjectState.Deleted, "Object was not correctly marked 'Deleted'.");

            aContainer.ChildContainers.Add(new EAVChildContainer());
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestChildContainersForDeletedChildContainerWithRemove()
        {
            EAVChildContainer aContainer = new EAVChildContainer();

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
        public void TestChildContainersForDeletedChildContainerWithClear()
        {
            EAVChildContainer aContainer = new EAVChildContainer();

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
        public void TestChildContainersInteractionWithChildContainer()
        {
            EAVChildContainer aContainer = new EAVChildContainer();

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
    }
}
