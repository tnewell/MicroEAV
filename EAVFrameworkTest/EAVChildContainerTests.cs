using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using EAV.Model;

using EAVFramework.Model;


namespace EAVFrameworkTest
{
    public partial class EAVFrameworkTestHarness
    {
        [TestMethod]
        public void ChildContainerCreate()
        {
            EAVChildContainer aChildContainer = new EAVChildContainer();

            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            Assert.IsNull(aChildContainer.ContainerID, "Property 'ContainerID' should be null on creation.");

            Assert.IsNull(aChildContainer.Name, "Property 'Name' should be null on creation.");
            Assert.IsNull(aChildContainer.DataName, "Property 'DataName' should be null on creation.");
            Assert.IsNull(aChildContainer.DisplayText, "Property 'DisplayText' should be null on creation.");
            Assert.IsFalse(aChildContainer.IsRepeating, "Property 'IsRepeating' should be false on creation.");

            Assert.IsNull(aChildContainer.Context, "Property 'Context' should be null on creation.");
            Assert.IsNull(aChildContainer.ParentContainer, "Property 'ParentContainer' should be null on creation.");

            Assert.IsNotNull(aChildContainer.ChildContainers, "Property 'ChildContainers' should not be null on creation.");
            Assert.IsFalse(aChildContainer.ChildContainers.Any(), "Property 'ChildContainers' should be empty on creation.");

            Assert.IsNotNull(aChildContainer.Attributes, "Property 'Attributes' should not be null on creation.");
            Assert.IsFalse(aChildContainer.Attributes.Any(), "Property 'Attributes' should be empty on creation.");

            Assert.IsNotNull(aChildContainer.Instances, "Property 'Instances' should not be null on creation.");
            Assert.IsFalse(aChildContainer.Instances.Any(), "Property 'Instances' should be empty on creation.");
        }

        #region State Transitions
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChildContainerStateTransitionNewToUnmodifiedWithNullID()
        {
            EAVChildContainer aChildContainer = new EAVChildContainer();

            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            aChildContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aChildContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        public void ChildContainerStateTransitionNewToUnmodifiedWithValidID()
        {
            EAVChildContainer aChildContainer = new EAVChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            aChildContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aChildContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChildContainerStateTransitionNewToDeleted()
        {
            EAVChildContainer aChildContainer = new EAVChildContainer();

            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            aChildContainer.MarkDeleted();
        }

        [TestMethod]
        public void ChildContainerStateTransitionUnmodifiedToDeleted()
        {
            EAVChildContainer aChildContainer = new EAVChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            aChildContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aChildContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aChildContainer.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aChildContainer.ObjectState, "Object state failed to transition to 'Deleted'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChildContainerStateTransitionDeletedToUnmodified()
        {
            EAVChildContainer aChildContainer = new EAVChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            aChildContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aChildContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aChildContainer.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aChildContainer.ObjectState, "Object state failed to transition to 'Deleted'.");

            aChildContainer.MarkUnmodified();
        }
        #endregion

        #region ID Property
        [TestMethod]
        public void ChildContainerSetIDWhenNew()
        {
            EAVChildContainer aChildContainer = new EAVChildContainer();

            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            int id = rng.Next();
            aChildContainer.ContainerID = id;

            Assert.AreEqual(id, aChildContainer.ContainerID, "Property 'ContainerID' not properly set.");
            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state after setting property 'ContainerID' should be 'New'.");
        }

        [TestMethod]
        public void ChildContainerSetIDBeforeUnmodified()
        {
            EAVChildContainer aChildContainer = new EAVChildContainer();

            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            int id = rng.Next();
            aChildContainer.ContainerID = id;

            Assert.AreEqual(id, aChildContainer.ContainerID, "Property 'ContainerID' not properly set.");
            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state after setting property 'ContainerID' should be 'New'.");

            aChildContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aChildContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChildContainerSetIDAfterUnmodified()
        {
            EAVChildContainer aChildContainer = new EAVChildContainer();

            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            aChildContainer.MarkUnmodified();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChildContainerSetIDWhenDeleted()
        {
            EAVChildContainer aChildContainer = new EAVChildContainer();

            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            int id = rng.Next();
            aChildContainer.ContainerID = id;

            Assert.AreEqual(id, aChildContainer.ContainerID, "Property 'ContainerID' not properly set.");
            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state after setting property 'ContainerID' should be 'New'.");

            aChildContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aChildContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aChildContainer.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aChildContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            id = rng.Next();
            aChildContainer.ContainerID = id;
        }
        #endregion

        #region Primitive Properties
        #region Name
        [TestMethod]
        public void ChildContainerSetNameWhenNew()
        {
            EAVChildContainer aChildContainer = new EAVChildContainer();

            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            string value = Guid.NewGuid().ToString();
            aChildContainer.Name = value;

            Assert.AreEqual(value, aChildContainer.Name, "Property 'Name' was not set properly.");
            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ChildContainerSetNameWhenUnmodified()
        {
            EAVChildContainer aChildContainer = new EAVChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            aChildContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aChildContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            aChildContainer.Name = value;

            Assert.AreEqual(value, aChildContainer.Name, "Property 'Name' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aChildContainer.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void ChildContainerSetNameWhenModified()
        {
            EAVChildContainer aChildContainer = new EAVChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            aChildContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aChildContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            aChildContainer.Name = value;

            Assert.AreEqual(value, aChildContainer.Name, "Property 'Name' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aChildContainer.ObjectState, "Object state failed to transition to 'Modified'.");

            value = Guid.NewGuid().ToString();
            aChildContainer.Name = value;

            Assert.AreEqual(value, aChildContainer.Name, "Property 'Name' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aChildContainer.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChildContainerSetNameWhenDeleted()
        {
            EAVChildContainer aChildContainer = new EAVChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            aChildContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aChildContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aChildContainer.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aChildContainer.ObjectState, "Object state failed to transition to 'Deleted'.");

            aChildContainer.Name = Guid.NewGuid().ToString();
        }
        #endregion

        #region DataName
        [TestMethod]
        public void ChildContainerSetDataNameWhenNew()
        {
            EAVChildContainer aChildContainer = new EAVChildContainer();

            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            string value = Guid.NewGuid().ToString();
            aChildContainer.DataName = value;

            Assert.AreEqual(value, aChildContainer.DataName, "Property 'DataName' was not set properly.");
            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ChildContainerSetDataNameWhenUnmodified()
        {
            EAVChildContainer aChildContainer = new EAVChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            aChildContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aChildContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            aChildContainer.DataName = value;

            Assert.AreEqual(value, aChildContainer.DataName, "Property 'DataName' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aChildContainer.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void ChildContainerSetDataNameWhenModified()
        {
            EAVChildContainer aChildContainer = new EAVChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            aChildContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aChildContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            aChildContainer.DataName = value;

            Assert.AreEqual(value, aChildContainer.DataName, "Property 'DataName' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aChildContainer.ObjectState, "Object state failed to transition to 'Modified'.");

            value = Guid.NewGuid().ToString();
            aChildContainer.DataName = value;

            Assert.AreEqual(value, aChildContainer.DataName, "Property 'DataName' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aChildContainer.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChildContainerSetDataNameWhenDeleted()
        {
            EAVChildContainer aChildContainer = new EAVChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            aChildContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aChildContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aChildContainer.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aChildContainer.ObjectState, "Object state failed to transition to 'Deleted'.");

            aChildContainer.DataName = Guid.NewGuid().ToString();
        }
        #endregion

        #region DisplayText
        [TestMethod]
        public void ChildContainerSetDisplayTextWhenNew()
        {
            EAVChildContainer aChildContainer = new EAVChildContainer();

            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            string value = Guid.NewGuid().ToString();
            aChildContainer.DisplayText = value;

            Assert.AreEqual(value, aChildContainer.DisplayText, "Property 'DisplayText' was not set properly.");
            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ChildContainerSetDisplayTextWhenUnmodified()
        {
            EAVChildContainer aChildContainer = new EAVChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            aChildContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aChildContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            aChildContainer.DisplayText = value;

            Assert.AreEqual(value, aChildContainer.DisplayText, "Property 'DisplayText' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aChildContainer.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void ChildContainerSetDisplayTextWhenModified()
        {
            EAVChildContainer aChildContainer = new EAVChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            aChildContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aChildContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            aChildContainer.DisplayText = value;

            Assert.AreEqual(value, aChildContainer.DisplayText, "Property 'DisplayText' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aChildContainer.ObjectState, "Object state failed to transition to 'Modified'.");

            value = Guid.NewGuid().ToString();
            aChildContainer.DisplayText = value;

            Assert.AreEqual(value, aChildContainer.DisplayText, "Property 'DisplayText' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aChildContainer.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChildContainerSetDisplayTextWhenDeleted()
        {
            EAVChildContainer aChildContainer = new EAVChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            aChildContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aChildContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aChildContainer.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aChildContainer.ObjectState, "Object state failed to transition to 'Deleted'.");

            aChildContainer.DisplayText = Guid.NewGuid().ToString();
        }
        #endregion

        #region IsRepeating
        [TestMethod]
        public void ChildContainerSetIsRepeatingWhenNew()
        {
            EAVChildContainer aChildContainer = new EAVChildContainer();

            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            aChildContainer.IsRepeating = true;

            Assert.AreEqual(true, aChildContainer.IsRepeating, "Property 'IsRepeating' was not set properly.");
            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ChildContainerSetIsRepeatingWhenUnmodified()
        {
            EAVChildContainer aChildContainer = new EAVChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            aChildContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aChildContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aChildContainer.IsRepeating = true;

            Assert.AreEqual(true, aChildContainer.IsRepeating, "Property 'IsRepeating' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aChildContainer.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void ChildContainerSetIsRepeatingWhenModified()
        {
            EAVChildContainer aChildContainer = new EAVChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            aChildContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aChildContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aChildContainer.IsRepeating = true;

            Assert.AreEqual(true, aChildContainer.IsRepeating, "Property 'IsRepeating' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aChildContainer.ObjectState, "Object state failed to transition to 'Modified'.");

            aChildContainer.IsRepeating = false;

            Assert.AreEqual(false, aChildContainer.IsRepeating, "Property 'IsRepeating' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aChildContainer.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChildContainerSetIsRepeatingWhenDeleted()
        {
            EAVChildContainer aChildContainer = new EAVChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            aChildContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aChildContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aChildContainer.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aChildContainer.ObjectState, "Object state failed to transition to 'Deleted'.");

            aChildContainer.IsRepeating = true;
        }
        #endregion
        #endregion

        #region Object Properties
        #region Context
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChildContainerSetContextWhenNew()
        {
            EAVChildContainer aChildContainer = new EAVChildContainer();

            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            aChildContainer.Context = new EAVContext() { ContextID = rng.Next() };
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChildContainerSetContextWhenUnmodified()
        {
            EAVChildContainer aChildContainer = new EAVChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            aChildContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aChildContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aChildContainer.Context = new EAVContext() { ContextID = rng.Next() };
        }

        // Set When Modified - N/A

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChildContainerSetContextWhenDeleted()
        {
            EAVChildContainer aChildContainer = new EAVChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            aChildContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aChildContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aChildContainer.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aChildContainer.ObjectState, "Object state failed to transition to 'Deleted'.");

            aChildContainer.Context = new EAVContext() { ContextID = rng.Next() };
        }
        #endregion

        #region ParentContainer
        [TestMethod]
        public void ChildContainerSetParentContainerWithObjectWhenNew()
        {
            EAVChildContainer aChildContainer = new EAVChildContainer();

            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            EAVRootContainer aContainer = new EAVRootContainer() { ContainerID = rng.Next() };
            aChildContainer.ParentContainer = aContainer;

            Assert.AreEqual(aContainer, aChildContainer.ParentContainer, "Property 'ParentContainer' was not set properly.");
            Assert.AreEqual(aContainer.ContainerID, aChildContainer.ParentContainerID, "Property 'ParentContainerID' was not reported properly.");
            Assert.IsTrue(aContainer.ChildContainers.Contains(aChildContainer), "Property 'ChildContainers' was not updated properly.");
            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ChildContainerSetParentContainerWithObjectWhenUnmodified()
        {
            EAVChildContainer aChildContainer = new EAVChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            aChildContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aChildContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVRootContainer aContainer = new EAVRootContainer() { ContainerID = rng.Next() };
            aChildContainer.ParentContainer = aContainer;

            Assert.AreEqual(aContainer, aChildContainer.ParentContainer, "Property 'ParentContainer' was not set properly.");
            Assert.AreEqual(aContainer.ContainerID, aChildContainer.ParentContainerID, "Property 'ParentContainerID' was not reported properly.");
            Assert.IsTrue(aContainer.ChildContainers.Contains(aChildContainer), "Property 'ChildContainers' was not updated properly.");
            Assert.AreEqual(ObjectState.Modified, aChildContainer.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void ChildContainerSetParentContainerWithObjectWhenModified()
        {
            EAVChildContainer aChildContainer = new EAVChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            aChildContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aChildContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVRootContainer aContainer = new EAVRootContainer() { ContainerID = rng.Next() };
            aChildContainer.ParentContainer = aContainer;

            Assert.AreEqual(aContainer, aChildContainer.ParentContainer, "Property 'ParentContainer' was not set properly.");
            Assert.AreEqual(aContainer.ContainerID, aChildContainer.ParentContainerID, "Property 'ParentContainerID' was not reported properly.");
            Assert.IsTrue(aContainer.ChildContainers.Contains(aChildContainer), "Property 'ChildContainers' was not updated properly.");
            Assert.AreEqual(ObjectState.Modified, aChildContainer.ObjectState, "Object state failed to transition to 'Modified'.");

            aContainer = new EAVRootContainer() { ContainerID = rng.Next() };
            aChildContainer.ParentContainer = aContainer;

            Assert.AreEqual(aContainer, aChildContainer.ParentContainer, "Property 'ParentContainer' was not set properly.");
            Assert.AreEqual(aContainer.ContainerID, aChildContainer.ParentContainerID, "Property 'ParentContainerID' was not reported properly.");
            Assert.IsTrue(aContainer.ChildContainers.Contains(aChildContainer), "Property 'ChildContainers' was not updated properly.");
            Assert.AreEqual(ObjectState.Modified, aChildContainer.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChildContainerSetParentContainerWithObjectWhenDeleted()
        {
            EAVChildContainer aChildContainer = new EAVChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            aChildContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aChildContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aChildContainer.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aChildContainer.ObjectState, "Object state failed to transition to 'Deleted'.");

            aChildContainer.ParentContainer = new EAVRootContainer() { ContainerID = rng.Next() };
        }
        #endregion
        #endregion

        #region Collection Properties
        #region ChildContainers
        [TestMethod]
        public void ChildContainerSetChildContainersWhenNew()
        {
            EAVChildContainer aParentContainer = new EAVChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aParentContainer.ObjectState, "Object state should be 'New' on creation.");

            EAVChildContainer value = new EAVChildContainer() { ContainerID = rng.Next() };
            aParentContainer.ChildContainers.Add(value);

            Assert.IsTrue(aParentContainer.ChildContainers.Contains(value), "Property 'ChildContainers' was not updated properly.");
            Assert.AreEqual(aParentContainer, value.ParentContainer, "Property 'ParentContainer' was not set properly.");
            Assert.AreEqual(aParentContainer.ContainerID, value.ParentContainerID, "Property 'ParentContainerID' was not set properly.");
            Assert.AreEqual(ObjectState.New, aParentContainer.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ChildContainerSetChildContainersWhenUnmodified()
        {
            EAVChildContainer aParentContainer = new EAVChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aParentContainer.ObjectState, "Object state should be 'New' on creation.");

            aParentContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aParentContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVChildContainer value = new EAVChildContainer() { ContainerID = rng.Next() };
            aParentContainer.ChildContainers.Add(value);

            Assert.IsTrue(aParentContainer.ChildContainers.Contains(value), "Property 'ChildContainers' was not updated properly.");
            Assert.AreEqual(aParentContainer, value.ParentContainer, "Property 'ParentContainer' was not set properly.");
            Assert.AreEqual(aParentContainer.ContainerID, value.ParentContainerID, "Property 'ParentContainerID' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aParentContainer.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void ChildContainerSetChildContainersWhenModified()
        {
            EAVChildContainer aParentContainer = new EAVChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aParentContainer.ObjectState, "Object state should be 'New' on creation.");

            aParentContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aParentContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVChildContainer value = new EAVChildContainer() { ContainerID = rng.Next() };
            aParentContainer.ChildContainers.Add(value);

            Assert.IsTrue(aParentContainer.ChildContainers.Contains(value), "Property 'ChildContainers' was not updated properly.");
            Assert.AreEqual(aParentContainer, value.ParentContainer, "Property 'ParentContainer' was not set properly.");
            Assert.AreEqual(aParentContainer.ContainerID, value.ParentContainerID, "Property 'ParentContainerID' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aParentContainer.ObjectState, "Object state failed to transition to 'Modified'.");

            value = new EAVChildContainer() { ContainerID = rng.Next() };
            aParentContainer.ChildContainers.Add(value);

            Assert.IsTrue(aParentContainer.ChildContainers.Contains(value), "Property 'ChildContainers' was not updated properly.");
            Assert.AreEqual(aParentContainer, value.ParentContainer, "Property 'ParentContainer' was not set properly.");
            Assert.AreEqual(aParentContainer.ContainerID, value.ParentContainerID, "Property 'ParentContainerID' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aParentContainer.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ChildContainerSetChildContainersWhenDeleted()
        {
            EAVChildContainer aParentContainer = new EAVChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aParentContainer.ObjectState, "Object state should be 'New' on creation.");

            aParentContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aParentContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aParentContainer.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aParentContainer.ObjectState, "Object state failed to transition to 'Deleted'.");

            aParentContainer.ChildContainers.Add(new EAVChildContainer() { ContainerID = rng.Next() });
        }
        #endregion

        #region Attributes
        [TestMethod]
        public void ChildContainerSetAttributesWhenNew()
        {
            EAVChildContainer aContainer = new EAVChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            EAVAttribute value = new EAVAttribute() { AttributeID = rng.Next() };
            aContainer.Attributes.Add(value);

            Assert.IsTrue(aContainer.Attributes.Contains(value), "Property 'Attributes' was not updated properly.");
            Assert.AreEqual(aContainer, value.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(aContainer.ContainerID, value.ContainerID, "Property 'ContainerID' was not set properly.");
            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ChildContainerSetAttributesWhenUnmodified()
        {
            EAVChildContainer aContainer = new EAVChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVAttribute value = new EAVAttribute() { AttributeID = rng.Next() };
            aContainer.Attributes.Add(value);

            Assert.IsTrue(aContainer.Attributes.Contains(value), "Property 'Attributes' was not updated properly.");
            Assert.AreEqual(aContainer, value.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(aContainer.ContainerID, value.ContainerID, "Property 'ContainerID' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aContainer.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void ChildContainerSetAttributesWhenModified()
        {
            EAVChildContainer aContainer = new EAVChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVAttribute value = new EAVAttribute() { AttributeID = rng.Next() };
            aContainer.Attributes.Add(value);

            Assert.IsTrue(aContainer.Attributes.Contains(value), "Property 'Attributes' was not updated properly.");
            Assert.AreEqual(aContainer, value.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(aContainer.ContainerID, value.ContainerID, "Property 'ContainerID' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aContainer.ObjectState, "Object state failed to transition to 'Modified'.");

            value = new EAVAttribute() { AttributeID = rng.Next() };
            aContainer.Attributes.Add(value);

            Assert.IsTrue(aContainer.Attributes.Contains(value), "Property 'Attributes' was not updated properly.");
            Assert.AreEqual(aContainer, value.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(aContainer.ContainerID, value.ContainerID, "Property 'ContainerID' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aContainer.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ChildContainerSetAttributesWhenDeleted()
        {
            EAVChildContainer aContainer = new EAVChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aContainer.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aContainer.ObjectState, "Object state failed to transition to 'Deleted'.");

            aContainer.Attributes.Add(new EAVAttribute() { AttributeID = rng.Next() });
        }
        #endregion

        #region Instances
        [TestMethod]
        public void ChildContainerSetInstancesWhenNew()
        {
            EAVChildContainer aContainer = new EAVChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            EAVChildInstance value = new EAVChildInstance() { InstanceID = rng.Next() };
            aContainer.Instances.Add(value);

            Assert.IsTrue(aContainer.Instances.Contains(value), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(aContainer, value.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(aContainer.ContainerID, value.ContainerID, "Property 'ContainerID' was not set properly.");
            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ChildContainerSetInstancesWhenUnmodified()
        {
            EAVChildContainer aContainer = new EAVChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVChildInstance value = new EAVChildInstance() { InstanceID = rng.Next() };
            aContainer.Instances.Add(value);

            Assert.IsTrue(aContainer.Instances.Contains(value), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(aContainer, value.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(aContainer.ContainerID, value.ContainerID, "Property 'ContainerID' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aContainer.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void ChildContainerSetInstancesWhenModified()
        {
            EAVChildContainer aContainer = new EAVChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVChildInstance value = new EAVChildInstance() { InstanceID = rng.Next() };
            aContainer.Instances.Add(value);

            Assert.IsTrue(aContainer.Instances.Contains(value), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(aContainer, value.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(aContainer.ContainerID, value.ContainerID, "Property 'ContainerID' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aContainer.ObjectState, "Object state failed to transition to 'Modified'.");

            value = new EAVChildInstance() { InstanceID = rng.Next() };
            aContainer.Instances.Add(value);

            Assert.IsTrue(aContainer.Instances.Contains(value), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(aContainer, value.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(aContainer.ContainerID, value.ContainerID, "Property 'ContainerID' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aContainer.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ChildContainerSetInstancesWhenDeleted()
        {
            EAVChildContainer aContainer = new EAVChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aContainer.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aContainer.ObjectState, "Object state failed to transition to 'Deleted'.");

            aContainer.Instances.Add(new EAVChildInstance() { InstanceID = rng.Next() });
        }
        #endregion
        #endregion
    }
}
