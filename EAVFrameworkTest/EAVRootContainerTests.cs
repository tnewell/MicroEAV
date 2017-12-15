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
        public void RootContainerCreate()
        {
            EAVRootContainer aRootContainer = new EAVRootContainer();

            Assert.AreEqual(ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            Assert.IsNull(aRootContainer.ContainerID, "Property 'ContainerID' should be null on creation.");

            Assert.IsNull(aRootContainer.Name, "Property 'Name' should be null on creation.");
            Assert.IsNull(aRootContainer.DataName, "Property 'DataName' should be null on creation.");
            Assert.IsNull(aRootContainer.DisplayText, "Property 'DisplayText' should be null on creation.");
            Assert.IsFalse(aRootContainer.IsRepeating, "Property 'IsRepeating' should be false on creation.");

            Assert.IsNull(aRootContainer.Context, "Property 'Context' should be null on creation.");
            Assert.IsNull(aRootContainer.ParentContainer, "Property 'ParentContainer' should be null on creation.");

            Assert.IsNotNull(aRootContainer.ChildContainers, "Property 'ChildContainers' should not be null on creation.");
            Assert.IsFalse(aRootContainer.ChildContainers.Any(), "Property 'ChildContainers' should be empty on creation.");

            Assert.IsNotNull(aRootContainer.Attributes, "Property 'Attributes' should not be null on creation.");
            Assert.IsFalse(aRootContainer.Attributes.Any(), "Property 'Attributes' should be empty on creation.");
        }

        #region State Transitions
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RootContainerStateTransitionNewToUnmodifiedWithNullID()
        {
            EAVRootContainer aRootContainer = new EAVRootContainer();

            Assert.AreEqual(ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            aRootContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aRootContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        public void RootContainerStateTransitionNewToUnmodifiedWithValidID()
        {
            EAVRootContainer aRootContainer = new EAVRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            aRootContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aRootContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RootContainerStateTransitionNewToDeleted()
        {
            EAVRootContainer aRootContainer = new EAVRootContainer();

            Assert.AreEqual(ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            aRootContainer.MarkDeleted();
        }

        [TestMethod]
        public void RootContainerStateTransitionUnmodifiedToDeleted()
        {
            EAVRootContainer aRootContainer = new EAVRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            aRootContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aRootContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aRootContainer.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aRootContainer.ObjectState, "Object state failed to transition to 'Deleted'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RootContainerStateTransitionDeletedToUnmodified()
        {
            EAVRootContainer aRootContainer = new EAVRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            aRootContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aRootContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aRootContainer.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aRootContainer.ObjectState, "Object state failed to transition to 'Deleted'.");

            aRootContainer.MarkUnmodified();
        }
        #endregion

        #region ID Property
        [TestMethod]
        public void RootContainerSetIDWhenNew()
        {
            EAVRootContainer aRootContainer = new EAVRootContainer();

            Assert.AreEqual(ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            int id = rng.Next();
            aRootContainer.ContainerID = id;

            Assert.AreEqual(id, aRootContainer.ContainerID, "Property 'ContainerID' not properly set.");
            Assert.AreEqual(ObjectState.New, aRootContainer.ObjectState, "Object state after setting property 'ContainerID' should be 'New'.");
        }

        [TestMethod]
        public void RootContainerSetIDBeforeUnmodified()
        {
            EAVRootContainer aRootContainer = new EAVRootContainer();

            Assert.AreEqual(ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            int id = rng.Next();
            aRootContainer.ContainerID = id;

            Assert.AreEqual(id, aRootContainer.ContainerID, "Property 'ContainerID' not properly set.");
            Assert.AreEqual(ObjectState.New, aRootContainer.ObjectState, "Object state after setting property 'ContainerID' should be 'New'.");

            aRootContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aRootContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RootContainerSetIDAfterUnmodified()
        {
            EAVRootContainer aRootContainer = new EAVRootContainer();

            Assert.AreEqual(ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            aRootContainer.MarkUnmodified();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RootContainerSetIDWhenDeleted()
        {
            EAVRootContainer aRootContainer = new EAVRootContainer();

            Assert.AreEqual(ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            int id = rng.Next();
            aRootContainer.ContainerID = id;

            Assert.AreEqual(id, aRootContainer.ContainerID, "Property 'ContainerID' not properly set.");
            Assert.AreEqual(ObjectState.New, aRootContainer.ObjectState, "Object state after setting property 'ContainerID' should be 'New'.");

            aRootContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aRootContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aRootContainer.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aRootContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            id = rng.Next();
            aRootContainer.ContainerID = id;
        }
        #endregion

        #region Primitive Properties
        #region Name
        [TestMethod]
        public void RootContainerSetNameWhenNew()
        {
            EAVRootContainer aRootContainer = new EAVRootContainer();

            Assert.AreEqual(ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            string value = Guid.NewGuid().ToString();
            aRootContainer.Name = value;

            Assert.AreEqual(value, aRootContainer.Name, "Property 'Name' was not set properly.");
            Assert.AreEqual(ObjectState.New, aRootContainer.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void RootContainerSetNameWhenUnmodified()
        {
            EAVRootContainer aRootContainer = new EAVRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            aRootContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aRootContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            aRootContainer.Name = value;

            Assert.AreEqual(value, aRootContainer.Name, "Property 'Name' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aRootContainer.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void RootContainerSetNameWhenModified()
        {
            EAVRootContainer aRootContainer = new EAVRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            aRootContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aRootContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            aRootContainer.Name = value;

            Assert.AreEqual(value, aRootContainer.Name, "Property 'Name' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aRootContainer.ObjectState, "Object state failed to transition to 'Modified'.");

            value = Guid.NewGuid().ToString();
            aRootContainer.Name = value;

            Assert.AreEqual(value, aRootContainer.Name, "Property 'Name' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aRootContainer.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RootContainerSetNameWhenDeleted()
        {
            EAVRootContainer aRootContainer = new EAVRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            aRootContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aRootContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aRootContainer.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aRootContainer.ObjectState, "Object state failed to transition to 'Deleted'.");

            aRootContainer.Name = Guid.NewGuid().ToString();
        }
        #endregion

        #region DataName
        [TestMethod]
        public void RootContainerSetDataNameWhenNew()
        {
            EAVRootContainer aRootContainer = new EAVRootContainer();

            Assert.AreEqual(ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            string value = Guid.NewGuid().ToString();
            aRootContainer.DataName = value;

            Assert.AreEqual(value, aRootContainer.DataName, "Property 'DataName' was not set properly.");
            Assert.AreEqual(ObjectState.New, aRootContainer.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void RootContainerSetDataNameWhenUnmodified()
        {
            EAVRootContainer aRootContainer = new EAVRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            aRootContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aRootContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            aRootContainer.DataName = value;

            Assert.AreEqual(value, aRootContainer.DataName, "Property 'DataName' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aRootContainer.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void RootContainerSetDataNameWhenModified()
        {
            EAVRootContainer aRootContainer = new EAVRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            aRootContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aRootContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            aRootContainer.DataName = value;

            Assert.AreEqual(value, aRootContainer.DataName, "Property 'DataName' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aRootContainer.ObjectState, "Object state failed to transition to 'Modified'.");

            value = Guid.NewGuid().ToString();
            aRootContainer.DataName = value;

            Assert.AreEqual(value, aRootContainer.DataName, "Property 'DataName' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aRootContainer.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RootContainerSetDataNameWhenDeleted()
        {
            EAVRootContainer aRootContainer = new EAVRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            aRootContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aRootContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aRootContainer.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aRootContainer.ObjectState, "Object state failed to transition to 'Deleted'.");

            aRootContainer.DataName = Guid.NewGuid().ToString();
        }
        #endregion

        #region DisplayText
        [TestMethod]
        public void RootContainerSetDisplayTextWhenNew()
        {
            EAVRootContainer aRootContainer = new EAVRootContainer();

            Assert.AreEqual(ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            string value = Guid.NewGuid().ToString();
            aRootContainer.DisplayText = value;

            Assert.AreEqual(value, aRootContainer.DisplayText, "Property 'DisplayText' was not set properly.");
            Assert.AreEqual(ObjectState.New, aRootContainer.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void RootContainerSetDisplayTextWhenUnmodified()
        {
            EAVRootContainer aRootContainer = new EAVRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            aRootContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aRootContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            aRootContainer.DisplayText = value;

            Assert.AreEqual(value, aRootContainer.DisplayText, "Property 'DisplayText' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aRootContainer.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void RootContainerSetDisplayTextWhenModified()
        {
            EAVRootContainer aRootContainer = new EAVRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            aRootContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aRootContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            aRootContainer.DisplayText = value;

            Assert.AreEqual(value, aRootContainer.DisplayText, "Property 'DisplayText' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aRootContainer.ObjectState, "Object state failed to transition to 'Modified'.");

            value = Guid.NewGuid().ToString();
            aRootContainer.DisplayText = value;

            Assert.AreEqual(value, aRootContainer.DisplayText, "Property 'DisplayText' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aRootContainer.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RootContainerSetDisplayTextWhenDeleted()
        {
            EAVRootContainer aRootContainer = new EAVRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            aRootContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aRootContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aRootContainer.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aRootContainer.ObjectState, "Object state failed to transition to 'Deleted'.");

            aRootContainer.DisplayText = Guid.NewGuid().ToString();
        }
        #endregion

        #region IsRepeating
        [TestMethod]
        public void RootContainerSetIsRepeatingWhenNew()
        {
            EAVRootContainer aRootContainer = new EAVRootContainer();

            Assert.AreEqual(ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            aRootContainer.IsRepeating = true;

            Assert.AreEqual(true, aRootContainer.IsRepeating, "Property 'IsRepeating' was not set properly.");
            Assert.AreEqual(ObjectState.New, aRootContainer.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void RootContainerSetIsRepeatingWhenUnmodified()
        {
            EAVRootContainer aRootContainer = new EAVRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            aRootContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aRootContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aRootContainer.IsRepeating = true;

            Assert.AreEqual(true, aRootContainer.IsRepeating, "Property 'IsRepeating' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aRootContainer.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void RootContainerSetIsRepeatingWhenModified()
        {
            EAVRootContainer aRootContainer = new EAVRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            aRootContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aRootContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aRootContainer.IsRepeating = true;

            Assert.AreEqual(true, aRootContainer.IsRepeating, "Property 'IsRepeating' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aRootContainer.ObjectState, "Object state failed to transition to 'Modified'.");

            aRootContainer.IsRepeating = false;

            Assert.AreEqual(false, aRootContainer.IsRepeating, "Property 'IsRepeating' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aRootContainer.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RootContainerSetIsRepeatingWhenDeleted()
        {
            EAVRootContainer aRootContainer = new EAVRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            aRootContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aRootContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aRootContainer.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aRootContainer.ObjectState, "Object state failed to transition to 'Deleted'.");

            aRootContainer.IsRepeating = true;
        }
        #endregion
        #endregion

        #region Object Properties
        #region Context
        [TestMethod]
        public void RootContainerSetContextWhenNew()
        {
            EAVRootContainer aRootContainer = new EAVRootContainer();

            Assert.AreEqual(ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            EAVContext value = new EAVContext() { ContextID = rng.Next() };
            aRootContainer.Context = value;

            Assert.AreEqual(value, aRootContainer.Context, "Property 'Context' was not set properly.");
            Assert.AreEqual(value.ContextID, aRootContainer.ContextID, "Property 'ContextID' was not reported properly.");
            Assert.AreEqual(ObjectState.New, aRootContainer.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void RootContainerSetContextWhenUnmodified()
        {
            EAVRootContainer aRootContainer = new EAVRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            aRootContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aRootContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVContext value = new EAVContext() { ContextID = rng.Next() };
            aRootContainer.Context = value;

            Assert.AreEqual(value, aRootContainer.Context, "Property 'Context' was not set properly.");
            Assert.AreEqual(value.ContextID, aRootContainer.ContextID, "Property 'ContextID' was not reported properly.");
            Assert.AreEqual(ObjectState.Modified, aRootContainer.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void RootContainerSetContextWhenModified()
        {
            EAVRootContainer aRootContainer = new EAVRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            aRootContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aRootContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVContext value = new EAVContext() { ContextID = rng.Next() };
            aRootContainer.Context = value;

            Assert.AreEqual(value, aRootContainer.Context, "Property 'Context' was not set properly.");
            Assert.AreEqual(value.ContextID, aRootContainer.ContextID, "Property 'ContextID' was not reported properly.");
            Assert.AreEqual(ObjectState.Modified, aRootContainer.ObjectState, "Object state failed to transition to 'Modified'.");

            value = new EAVContext() { ContextID = rng.Next() };
            aRootContainer.Context = value;

            Assert.AreEqual(value, aRootContainer.Context, "Property 'Context' was not set properly.");
            Assert.AreEqual(value.ContextID, aRootContainer.ContextID, "Property 'ContextID' was not reported properly.");
            Assert.AreEqual(ObjectState.Modified, aRootContainer.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RootContainerSetContextWhenDeleted()
        {
            EAVRootContainer aRootContainer = new EAVRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            aRootContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aRootContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aRootContainer.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aRootContainer.ObjectState, "Object state failed to transition to 'Deleted'.");

            aRootContainer.Context = new EAVContext() { ContextID = rng.Next() };
        }
        #endregion

        #region ParentContainer
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RootContainerSetParentContainerWithObjectWhenNew()
        {
            EAVRootContainer aRootContainer = new EAVRootContainer();

            Assert.AreEqual(ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            aRootContainer.ParentContainer = new EAVRootContainer() { ContainerID = rng.Next() };
        }

        [TestMethod]
        public void RootContainerSetParentContainerWithNullWhenNew()
        {
            EAVRootContainer aRootContainer = new EAVRootContainer();

            Assert.AreEqual(ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            aRootContainer.ParentContainer = null;

            Assert.IsNull(aRootContainer.ParentContainer, "Property 'ParentContainer' was not set properly.");
            Assert.IsNull(aRootContainer.ParentContainerID, "Property 'ParentContainerID' was not reported properly.");
            Assert.AreEqual(ObjectState.New, aRootContainer.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RootContainerSetParentContainerWithObjectWhenUnmodified()
        {
            EAVRootContainer aRootContainer = new EAVRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            aRootContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aRootContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aRootContainer.ParentContainer = new EAVRootContainer() { ContainerID = rng.Next() };
        }

        [TestMethod]
        public void RootContainerSetParentContainerWithNullWhenUnmodified()
        {
            EAVRootContainer aRootContainer = new EAVRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            aRootContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aRootContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aRootContainer.ParentContainer = null;

            Assert.IsNull(aRootContainer.ParentContainer, "Property 'ParentContainer' was not set properly.");
            Assert.IsNull(aRootContainer.ParentContainerID, "Property 'ParentContainerID' was not reported properly.");
            Assert.AreEqual(ObjectState.Unmodified, aRootContainer.ObjectState, "Object state should remain 'Unmodified' when property set.");
        }

        // Set When Modified - N/A

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RootContainerSetParentContainerWithObjectWhenDeleted()
        {
            EAVRootContainer aRootContainer = new EAVRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            aRootContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aRootContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aRootContainer.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aRootContainer.ObjectState, "Object state failed to transition to 'Deleted'.");

            aRootContainer.ParentContainer = new EAVRootContainer() { ContainerID = rng.Next() };
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RootContainerSetParentContainerWithNullWhenDeleted()
        {
            EAVRootContainer aRootContainer = new EAVRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            aRootContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aRootContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aRootContainer.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aRootContainer.ObjectState, "Object state failed to transition to 'Deleted'.");

            aRootContainer.ParentContainer = null;
        }
        #endregion
        #endregion

        #region Collection Properties
        //    Add When New
        //    Add When Unmodified
        //        Add When Modified
        //    Add When Deleted


        //    Remove When New
        //    Remove When Unmodified
        //        Remove When Modified
        //    Remove When Deleted


        //    Clear When New
        //    Clear When Unmodified
        //        Clear When Modified
        //    Clear When Deleted
        #endregion
    }
}
