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
            Assert.AreEqual(0, aRootContainer.Sequence, "Property 'Sequence' should be zero on creation.");
            Assert.IsFalse(aRootContainer.IsRepeating, "Property 'IsRepeating' should be false on creation.");

            Assert.IsNull(aRootContainer.Context, "Property 'Context' should be null on creation.");
            Assert.IsNull(aRootContainer.ParentContainer, "Property 'ParentContainer' should be null on creation.");

            Assert.IsNotNull(aRootContainer.ChildContainers, "Property 'ChildContainers' should not be null on creation.");
            Assert.IsFalse(aRootContainer.ChildContainers.Any(), "Property 'ChildContainers' should be empty on creation.");

            Assert.IsNotNull(aRootContainer.Attributes, "Property 'Attributes' should not be null on creation.");
            Assert.IsFalse(aRootContainer.Attributes.Any(), "Property 'Attributes' should be empty on creation.");

            Assert.IsNotNull(aRootContainer.Instances, "Property 'Instances' should not be null on creation.");
            Assert.IsFalse(aRootContainer.Instances.Any(), "Property 'Instances' should be empty on creation.");
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

        #region Sequence
        [TestMethod]
        public void RootContainerSetSequenceWhenNew()
        {
            EAVRootContainer anRootContainer = new EAVRootContainer();

            Assert.AreEqual(ObjectState.New, anRootContainer.ObjectState, "Object state should be 'New' on creation.");

            int value = rng.Next();
            anRootContainer.Sequence = value;

            Assert.AreEqual(value, anRootContainer.Sequence, "Property 'Sequence' was not set properly.");
            Assert.AreEqual(ObjectState.New, anRootContainer.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void RootContainerSetSequenceWhenUnmodified()
        {
            EAVRootContainer anRootContainer = new EAVRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anRootContainer.ObjectState, "Object state should be 'New' on creation.");

            anRootContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anRootContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            int value = rng.Next();
            anRootContainer.Sequence = value;

            Assert.AreEqual(value, anRootContainer.Sequence, "Property 'Sequence' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, anRootContainer.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void RootContainerSetSequenceWhenModified()
        {
            EAVRootContainer anRootContainer = new EAVRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anRootContainer.ObjectState, "Object state should be 'New' on creation.");

            anRootContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anRootContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            int value = rng.Next();
            anRootContainer.Sequence = value;

            Assert.AreEqual(value, anRootContainer.Sequence, "Property 'Sequence' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, anRootContainer.ObjectState, "Object state failed to transition to 'Modified'.");

            value = rng.Next();
            anRootContainer.Sequence = value;

            Assert.AreEqual(value, anRootContainer.Sequence, "Property 'Sequence' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, anRootContainer.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RootContainerSetSequenceWhenDeleted()
        {
            EAVRootContainer anRootContainer = new EAVRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anRootContainer.ObjectState, "Object state should be 'New' on creation.");

            anRootContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anRootContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anRootContainer.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, anRootContainer.ObjectState, "Object state failed to transition to 'Deleted'.");

            anRootContainer.Sequence = rng.Next();
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
            Assert.IsTrue(value.Containers.Contains(aRootContainer), "Property 'Containers' was not updated properly.");
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
            Assert.IsTrue(value.Containers.Contains(aRootContainer), "Property 'Containers' was not updated properly.");
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
            Assert.IsTrue(value.Containers.Contains(aRootContainer), "Property 'Containers' was not updated properly.");
            Assert.AreEqual(ObjectState.Modified, aRootContainer.ObjectState, "Object state failed to transition to 'Modified'.");

            value = new EAVContext() { ContextID = rng.Next() };
            aRootContainer.Context = value;

            Assert.AreEqual(value, aRootContainer.Context, "Property 'Context' was not set properly.");
            Assert.AreEqual(value.ContextID, aRootContainer.ContextID, "Property 'ContextID' was not reported properly.");
            Assert.IsTrue(value.Containers.Contains(aRootContainer), "Property 'Containers' was not updated properly.");
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
        #region ChildContainers
        [TestMethod]
        public void RootContainerAddToChildContainersWhenNew()
        {
            EAVRootContainer aParentContainer = new EAVRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aParentContainer.ObjectState, "Object state should be 'New' on creation.");

            EAVChildContainer value = new EAVChildContainer() { ContainerID = rng.Next() };
            aParentContainer.ChildContainers.Add(value);

            Assert.IsTrue(aParentContainer.ChildContainers.Contains(value), "Property 'ChildContainers' was not updated properly.");
            Assert.AreEqual(aParentContainer, value.ParentContainer, "Property 'ParentContainer' was not set properly.");
            Assert.AreEqual(aParentContainer.ContainerID, value.ParentContainerID, "Property 'ParentContainerID' was not set properly.");
            Assert.AreEqual(ObjectState.New, aParentContainer.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void RootContainerAddToChildContainersWhenUnmodified()
        {
            EAVRootContainer aParentContainer = new EAVRootContainer() { ContainerID = rng.Next() };

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
        public void RootContainerAddToChildContainersWhenModified()
        {
            EAVRootContainer aParentContainer = new EAVRootContainer() { ContainerID = rng.Next() };

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
        public void RootContainerAddToChildContainersWhenDeleted()
        {
            EAVRootContainer aParentContainer = new EAVRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aParentContainer.ObjectState, "Object state should be 'New' on creation.");

            aParentContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aParentContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aParentContainer.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aParentContainer.ObjectState, "Object state failed to transition to 'Deleted'.");

            aParentContainer.ChildContainers.Add(new EAVChildContainer() { ContainerID = rng.Next() });
        }

        [TestMethod]
        public void RootContainerRemoveFromChildContainersWhenNew()
        {
            EAVRootContainer aParentContainer = new EAVRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aParentContainer.ObjectState, "Object state should be 'New' on creation.");

            EAVChildContainer value = new EAVChildContainer() { ContainerID = rng.Next() };
            aParentContainer.ChildContainers.Add(value);
            value = new EAVChildContainer() { ContainerID = rng.Next() };
            aParentContainer.ChildContainers.Add(value);

            Assert.IsTrue(aParentContainer.ChildContainers.Contains(value), "Property 'ChildContainers' was not updated properly.");
            Assert.AreEqual(aParentContainer, value.ParentContainer, "Property 'ParentContainer' was not set properly.");
            Assert.AreEqual(aParentContainer.ContainerID, value.ParentContainerID, "Property 'ParentContainerID' was not reported properly.");
            Assert.AreEqual(ObjectState.New, aParentContainer.ObjectState, "Object state should remain 'New' when property set.");

            aParentContainer.ChildContainers.Remove(value);

            Assert.IsFalse(aParentContainer.ChildContainers.Contains(value), "Property 'ChildContainers' was not updated properly.");
            Assert.IsNull(value.ParentContainer, "Property 'ParentContainer' was not set properly.");
            Assert.IsNull(value.ParentContainerID, "Property 'ParentContainerID' was not reported properly.");
            Assert.AreEqual(ObjectState.New, aParentContainer.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void RootContainerRemoveFromChildContainersWhenUnmodified()
        {
            EAVRootContainer aParentContainer = new EAVRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aParentContainer.ObjectState, "Object state should be 'New' on creation.");

            EAVChildContainer value = new EAVChildContainer() { ContainerID = rng.Next() };
            aParentContainer.ChildContainers.Add(value);
            value = new EAVChildContainer() { ContainerID = rng.Next() };
            aParentContainer.ChildContainers.Add(value);

            Assert.IsTrue(aParentContainer.ChildContainers.Contains(value), "Property 'ChildContainers' was not updated properly.");
            Assert.AreEqual(aParentContainer, value.ParentContainer, "Property 'ParentContainer' was not set properly.");
            Assert.AreEqual(aParentContainer.ContainerID, value.ParentContainerID, "Property 'ParentContainerID' was not reported properly.");
            Assert.AreEqual(ObjectState.New, aParentContainer.ObjectState, "Object state should remain 'New' when property set.");

            aParentContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aParentContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aParentContainer.ChildContainers.Remove(value);

            Assert.IsFalse(aParentContainer.ChildContainers.Contains(value), "Property 'ChildContainers' was not updated properly.");
            Assert.IsNull(value.ParentContainer, "Property 'ParentContainer' was not set properly.");
            Assert.IsNull(value.ParentContainerID, "Property 'ParentContainerID' was not reported properly.");
            Assert.AreEqual(ObjectState.Modified, aParentContainer.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void RootContainerRemoveFromChildContainersWhenModified()
        {
            EAVRootContainer aParentContainer = new EAVRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aParentContainer.ObjectState, "Object state should be 'New' on creation.");

            aParentContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aParentContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVChildContainer value = new EAVChildContainer() { ContainerID = rng.Next() };
            aParentContainer.ChildContainers.Add(value);
            value = new EAVChildContainer() { ContainerID = rng.Next() };
            aParentContainer.ChildContainers.Add(value);

            Assert.IsTrue(aParentContainer.ChildContainers.Contains(value), "Property 'ChildContainers' was not updated properly.");
            Assert.AreEqual(aParentContainer, value.ParentContainer, "Property 'ParentContainer' was not set properly.");
            Assert.AreEqual(aParentContainer.ContainerID, value.ParentContainerID, "Property 'ParentContainerID' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aParentContainer.ObjectState, "Object state failed to transition to 'Modified'.");

            aParentContainer.ChildContainers.Remove(value);

            Assert.IsFalse(aParentContainer.ChildContainers.Contains(value), "Property 'ChildContainers' was not updated properly.");
            Assert.IsNull(value.ParentContainer, "Property 'ParentContainer' was not set properly.");
            Assert.IsNull(value.ParentContainerID, "Property 'ParentContainerID' was not reported properly.");
            Assert.AreEqual(ObjectState.Modified, aParentContainer.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void RootContainerRemoveFromChildContainersWhenDeleted()
        {
            EAVRootContainer aParentContainer = new EAVRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aParentContainer.ObjectState, "Object state should be 'New' on creation.");

            EAVChildContainer value = new EAVChildContainer() { ContainerID = rng.Next() };
            aParentContainer.ChildContainers.Add(value);

            aParentContainer.MarkUnmodified();
            value.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aParentContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");
            Assert.AreEqual(ObjectState.Unmodified, value.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aParentContainer.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aParentContainer.ObjectState, "Object state failed to transition to 'Deleted'.");
            Assert.AreEqual(ObjectState.Deleted, value.ObjectState, "Object state failed to transition to 'Deleted'.");

            aParentContainer.ChildContainers.Remove(value);
        }

        [TestMethod]
        public void RootContainerClearChildContainersWhenNew()
        {
            EAVRootContainer aParentContainer = new EAVRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aParentContainer.ObjectState, "Object state should be 'New' on creation.");

            EAVChildContainer value = new EAVChildContainer() { ContainerID = rng.Next() };
            aParentContainer.ChildContainers.Add(value);
            value = new EAVChildContainer() { ContainerID = rng.Next() };
            aParentContainer.ChildContainers.Add(value);

            Assert.IsTrue(aParentContainer.ChildContainers.Contains(value), "Property 'ChildContainers' was not updated properly.");
            Assert.AreEqual(aParentContainer, value.ParentContainer, "Property 'ParentContainer' was not set properly.");
            Assert.AreEqual(aParentContainer.ContainerID, value.ParentContainerID, "Property 'ParentContainerID' was not reported properly.");
            Assert.AreEqual(ObjectState.New, aParentContainer.ObjectState, "Object state should remain 'New' when property set.");

            aParentContainer.ChildContainers.Clear();

            Assert.IsFalse(aParentContainer.ChildContainers.Contains(value), "Property 'ChildContainers' was not updated properly.");
            Assert.IsNull(value.ParentContainer, "Property 'ParentContainer' was not set properly.");
            Assert.IsNull(value.ParentContainerID, "Property 'ParentContainerID' was not reported properly.");
            Assert.IsFalse(aParentContainer.ChildContainers.Any(), "Property 'ChildContainers' was not updated properly.");
            Assert.AreEqual(ObjectState.New, aParentContainer.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void RootContainerClearChildContainersWhenUnmodified()
        {
            EAVRootContainer aParentContainer = new EAVRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aParentContainer.ObjectState, "Object state should be 'New' on creation.");

            EAVChildContainer value = new EAVChildContainer() { ContainerID = rng.Next() };
            aParentContainer.ChildContainers.Add(value);
            value = new EAVChildContainer() { ContainerID = rng.Next() };
            aParentContainer.ChildContainers.Add(value);

            Assert.IsTrue(aParentContainer.ChildContainers.Contains(value), "Property 'ChildContainers' was not updated properly.");
            Assert.AreEqual(aParentContainer, value.ParentContainer, "Property 'ParentContainer' was not set properly.");
            Assert.AreEqual(aParentContainer.ContainerID, value.ParentContainerID, "Property 'ParentContainerID' was not reported properly.");
            Assert.AreEqual(ObjectState.New, aParentContainer.ObjectState, "Object state should remain 'New' when property set.");

            aParentContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aParentContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aParentContainer.ChildContainers.Clear();

            Assert.IsFalse(aParentContainer.ChildContainers.Contains(value), "Property 'ChildContainers' was not updated properly.");
            Assert.IsNull(value.ParentContainer, "Property 'ParentContainer' was not set properly.");
            Assert.IsNull(value.ParentContainerID, "Property 'ParentContainerID' was not reported properly.");
            Assert.IsFalse(aParentContainer.ChildContainers.Any(), "Property 'ChildContainers' was not updated properly.");
            Assert.AreEqual(ObjectState.Modified, aParentContainer.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void RootContainerClearChildContainersWhenModified()
        {
            EAVRootContainer aParentContainer = new EAVRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aParentContainer.ObjectState, "Object state should be 'New' on creation.");

            aParentContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aParentContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVChildContainer value = new EAVChildContainer() { ContainerID = rng.Next() };
            aParentContainer.ChildContainers.Add(value);
            value = new EAVChildContainer() { ContainerID = rng.Next() };
            aParentContainer.ChildContainers.Add(value);

            Assert.IsTrue(aParentContainer.ChildContainers.Contains(value), "Property 'ChildContainers' was not updated properly.");
            Assert.AreEqual(aParentContainer, value.ParentContainer, "Property 'ParentContainer' was not set properly.");
            Assert.AreEqual(aParentContainer.ContainerID, value.ParentContainerID, "Property 'ParentContainerID' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aParentContainer.ObjectState, "Object state failed to transition to 'Modified'.");

            aParentContainer.ChildContainers.Clear();

            Assert.IsFalse(aParentContainer.ChildContainers.Contains(value), "Property 'ChildContainers' was not updated properly.");
            Assert.IsNull(value.ParentContainer, "Property 'ParentContainer' was not set properly.");
            Assert.IsNull(value.ParentContainerID, "Property 'ParentContainerID' was not reported properly.");
            Assert.IsFalse(aParentContainer.ChildContainers.Any(), "Property 'ChildContainers' was not updated properly.");
            Assert.AreEqual(ObjectState.Modified, aParentContainer.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void RootContainerClearChildContainersWhenDeleted()
        {
            EAVRootContainer aParentContainer = new EAVRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aParentContainer.ObjectState, "Object state should be 'New' on creation.");

            EAVChildContainer value = new EAVChildContainer() { ContainerID = rng.Next() };
            aParentContainer.ChildContainers.Add(value);

            aParentContainer.MarkUnmodified();
            value.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aParentContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");
            Assert.AreEqual(ObjectState.Unmodified, value.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aParentContainer.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aParentContainer.ObjectState, "Object state failed to transition to 'Deleted'.");
            Assert.AreEqual(ObjectState.Deleted, value.ObjectState, "Object state failed to transition to 'Deleted'.");

            aParentContainer.ChildContainers.Clear();
        }
        #endregion

        #region Attributes
        [TestMethod]
        public void RootContainerAddToAttributesWhenNew()
        {
            EAVRootContainer aContainer = new EAVRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            EAVAttribute value = new EAVAttribute() { AttributeID = rng.Next() };
            aContainer.Attributes.Add(value);

            Assert.IsTrue(aContainer.Attributes.Contains(value), "Property 'Attributes' was not updated properly.");
            Assert.AreEqual(aContainer, value.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(aContainer.ContainerID, value.ContainerID, "Property 'ContainerID' was not set properly.");
            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void RootContainerAddToAttributesWhenUnmodified()
        {
            EAVRootContainer aContainer = new EAVRootContainer() { ContainerID = rng.Next() };

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
        public void RootContainerAddToAttributesWhenModified()
        {
            EAVRootContainer aContainer = new EAVRootContainer() { ContainerID = rng.Next() };

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
        public void RootContainerAddToAttributesWhenDeleted()
        {
            EAVRootContainer aContainer = new EAVRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aContainer.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aContainer.ObjectState, "Object state failed to transition to 'Deleted'.");

            aContainer.Attributes.Add(new EAVAttribute() { AttributeID = rng.Next() });
        }

        [TestMethod]
        public void RootContainerRemoveFromAttributesWhenNew()
        {
            EAVRootContainer aContainer = new EAVRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            EAVAttribute value = new EAVAttribute() { AttributeID = rng.Next() };
            aContainer.Attributes.Add(value);
            value = new EAVAttribute() { AttributeID = rng.Next() };
            aContainer.Attributes.Add(value);

            Assert.IsTrue(aContainer.Attributes.Contains(value), "Property 'Attributes' was not updated properly.");
            Assert.AreEqual(aContainer, value.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(aContainer.ContainerID, value.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should remain 'New' when property set.");

            aContainer.Attributes.Remove(value);

            Assert.IsFalse(aContainer.Attributes.Contains(value), "Property 'Attributes' was not updated properly.");
            Assert.IsNull(value.Container, "Property 'Container' was not set properly.");
            Assert.IsNull(value.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void RootContainerRemoveFromAttributesWhenUnmodified()
        {
            EAVRootContainer aContainer = new EAVRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            EAVAttribute value = new EAVAttribute() { AttributeID = rng.Next() };
            aContainer.Attributes.Add(value);
            value = new EAVAttribute() { AttributeID = rng.Next() };
            aContainer.Attributes.Add(value);

            Assert.IsTrue(aContainer.Attributes.Contains(value), "Property 'Attributes' was not updated properly.");
            Assert.AreEqual(aContainer, value.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(aContainer.ContainerID, value.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should remain 'New' when property set.");

            aContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aContainer.Attributes.Remove(value);

            Assert.IsFalse(aContainer.Attributes.Contains(value), "Property 'Attributes' was not updated properly.");
            Assert.IsNull(value.Container, "Property 'Container' was not set properly.");
            Assert.IsNull(value.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.AreEqual(ObjectState.Modified, aContainer.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void RootContainerRemoveFromAttributesWhenModified()
        {
            EAVRootContainer aContainer = new EAVRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            EAVAttribute value = new EAVAttribute() { AttributeID = rng.Next() };
            aContainer.Attributes.Add(value);
            value = new EAVAttribute() { AttributeID = rng.Next() };
            aContainer.Attributes.Add(value);

            Assert.IsTrue(aContainer.Attributes.Contains(value), "Property 'Attributes' was not updated properly.");
            Assert.AreEqual(aContainer, value.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(aContainer.ContainerID, value.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should remain 'New' when property set.");

            aContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aContainer.Attributes.Remove(value);

            Assert.IsFalse(aContainer.Attributes.Contains(value), "Property 'Attributes' was not updated properly.");
            Assert.IsNull(value.Container, "Property 'Container' was not set properly.");
            Assert.IsNull(value.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.AreEqual(ObjectState.Modified, aContainer.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void RootContainerRemoveFromAttributesWhenDeleted()
        {
            EAVRootContainer aContainer = new EAVRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            EAVAttribute value = new EAVAttribute() { AttributeID = rng.Next() };
            aContainer.Attributes.Add(value);

            Assert.IsTrue(aContainer.Attributes.Contains(value), "Property 'Attributes' was not updated properly.");
            Assert.AreEqual(aContainer, value.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(aContainer.ContainerID, value.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should remain 'New' when property set.");

            aContainer.MarkUnmodified();
            value.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");
            Assert.AreEqual(ObjectState.Unmodified, value.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aContainer.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aContainer.ObjectState, "Object state failed to transition to 'Deleted'.");
            Assert.AreEqual(ObjectState.Deleted, value.ObjectState, "Object state failed to transition to 'Deleted'.");

            aContainer.Attributes.Remove(value);
        }

        [TestMethod]
        public void RootContainerClearAttributesWhenNew()
        {
            EAVRootContainer aContainer = new EAVRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            EAVAttribute value = new EAVAttribute() { AttributeID = rng.Next() };
            aContainer.Attributes.Add(value);
            value = new EAVAttribute() { AttributeID = rng.Next() };
            aContainer.Attributes.Add(value);

            Assert.IsTrue(aContainer.Attributes.Contains(value), "Property 'Attributes' was not updated properly.");
            Assert.AreEqual(aContainer, value.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(aContainer.ContainerID, value.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should remain 'New' when property set.");

            aContainer.Attributes.Clear();

            Assert.IsFalse(aContainer.Attributes.Contains(value), "Property 'Attributes' was not updated properly.");
            Assert.IsNull(value.Container, "Property 'Container' was not set properly.");
            Assert.IsNull(value.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.IsFalse(aContainer.Attributes.Any(), "Property 'Attributes' was not updated properly.");
            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void RootContainerClearAttributesWhenUnmodified()
        {
            EAVRootContainer aContainer = new EAVRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            EAVAttribute value = new EAVAttribute() { AttributeID = rng.Next() };
            aContainer.Attributes.Add(value);
            value = new EAVAttribute() { AttributeID = rng.Next() };
            aContainer.Attributes.Add(value);

            Assert.IsTrue(aContainer.Attributes.Contains(value), "Property 'Attributes' was not updated properly.");
            Assert.AreEqual(aContainer, value.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(aContainer.ContainerID, value.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should remain 'New' when property set.");

            aContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aContainer.Attributes.Clear();

            Assert.IsFalse(aContainer.Attributes.Contains(value), "Property 'Attributes' was not updated properly.");
            Assert.IsNull(value.Container, "Property 'Container' was not set properly.");
            Assert.IsNull(value.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.IsFalse(aContainer.Attributes.Any(), "Property 'Attributes' was not updated properly.");
            Assert.AreEqual(ObjectState.Modified, aContainer.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void RootContainerClearAttributesWhenModified()
        {
            EAVRootContainer aContainer = new EAVRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            EAVAttribute value = new EAVAttribute() { AttributeID = rng.Next() };
            aContainer.Attributes.Add(value);
            value = new EAVAttribute() { AttributeID = rng.Next() };
            aContainer.Attributes.Add(value);

            Assert.IsTrue(aContainer.Attributes.Contains(value), "Property 'Attributes' was not updated properly.");
            Assert.AreEqual(aContainer, value.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(aContainer.ContainerID, value.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should remain 'New' when property set.");

            aContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aContainer.Attributes.Clear();

            Assert.IsFalse(aContainer.Attributes.Contains(value), "Property 'Attributes' was not updated properly.");
            Assert.IsNull(value.Container, "Property 'Container' was not set properly.");
            Assert.IsNull(value.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.IsFalse(aContainer.Attributes.Any(), "Property 'Attributes' was not updated properly.");
            Assert.AreEqual(ObjectState.Modified, aContainer.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void RootContainerClearAttributesWhenDeleted()
        {
            EAVRootContainer aContainer = new EAVRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            EAVAttribute value = new EAVAttribute() { AttributeID = rng.Next() };
            aContainer.Attributes.Add(value);

            Assert.IsTrue(aContainer.Attributes.Contains(value), "Property 'Attributes' was not updated properly.");
            Assert.AreEqual(aContainer, value.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(aContainer.ContainerID, value.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should remain 'New' when property set.");

            aContainer.MarkUnmodified();
            value.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");
            Assert.AreEqual(ObjectState.Unmodified, value.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aContainer.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aContainer.ObjectState, "Object state failed to transition to 'Deleted'.");
            Assert.AreEqual(ObjectState.Deleted, value.ObjectState, "Object state failed to transition to 'Deleted'.");

            aContainer.Attributes.Clear();
        }
        #endregion

        #region Instances
        [TestMethod]
        public void RootContainerAddToInstancesWhenNew()
        {
            EAVRootContainer aContainer = new EAVRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            EAVRootInstance value = new EAVRootInstance() { InstanceID = rng.Next() };
            aContainer.Instances.Add(value);

            Assert.IsTrue(aContainer.Instances.Contains(value), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(aContainer, value.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(aContainer.ContainerID, value.ContainerID, "Property 'ContainerID' was not set properly.");
            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void RootContainerAddToInstancesWhenUnmodified()
        {
            EAVRootContainer aContainer = new EAVRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVRootInstance value = new EAVRootInstance() { InstanceID = rng.Next() };
            aContainer.Instances.Add(value);

            Assert.IsTrue(aContainer.Instances.Contains(value), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(aContainer, value.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(aContainer.ContainerID, value.ContainerID, "Property 'ContainerID' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aContainer.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void RootContainerAddToInstancesWhenModified()
        {
            EAVRootContainer aContainer = new EAVRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVRootInstance value = new EAVRootInstance() { InstanceID = rng.Next() };
            aContainer.Instances.Add(value);

            Assert.IsTrue(aContainer.Instances.Contains(value), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(aContainer, value.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(aContainer.ContainerID, value.ContainerID, "Property 'ContainerID' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aContainer.ObjectState, "Object state failed to transition to 'Modified'.");

            value = new EAVRootInstance() { InstanceID = rng.Next() };
            aContainer.Instances.Add(value);

            Assert.IsTrue(aContainer.Instances.Contains(value), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(aContainer, value.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(aContainer.ContainerID, value.ContainerID, "Property 'ContainerID' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aContainer.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void RootContainerAddToInstancesWhenDeleted()
        {
            EAVRootContainer aContainer = new EAVRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aContainer.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aContainer.ObjectState, "Object state failed to transition to 'Deleted'.");

            aContainer.Instances.Add(new EAVRootInstance() { InstanceID = rng.Next() });
        }

        [TestMethod]
        public void RootContainerRemoveFromInstancesWhenNew()
        {
            EAVRootContainer aContainer = new EAVRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            EAVRootInstance value = new EAVRootInstance() { InstanceID = rng.Next() };
            aContainer.Instances.Add(value);
            value = new EAVRootInstance() { InstanceID = rng.Next() };
            aContainer.Instances.Add(value);

            Assert.IsTrue(aContainer.Instances.Contains(value), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(aContainer, value.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(aContainer.ContainerID, value.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should remain 'New' when property set.");

            aContainer.Instances.Remove(value);

            Assert.IsFalse(aContainer.Instances.Contains(value), "Property 'Instances' was not updated properly.");
            Assert.IsNull(value.Container, "Property 'Container' was not set properly.");
            Assert.IsNull(value.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void RootContainerRemoveFromInstancesWhenUnmodified()
        {
            EAVRootContainer aContainer = new EAVRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            EAVRootInstance value = new EAVRootInstance() { InstanceID = rng.Next() };
            aContainer.Instances.Add(value);
            value = new EAVRootInstance() { InstanceID = rng.Next() };
            aContainer.Instances.Add(value);

            Assert.IsTrue(aContainer.Instances.Contains(value), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(aContainer, value.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(aContainer.ContainerID, value.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should remain 'New' when property set.");

            aContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aContainer.Instances.Remove(value);

            Assert.IsFalse(aContainer.Instances.Contains(value), "Property 'Instances' was not updated properly.");
            Assert.IsNull(value.Container, "Property 'Container' was not set properly.");
            Assert.IsNull(value.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.AreEqual(ObjectState.Modified, aContainer.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void RootContainerRemoveFromInstancesWhenModified()
        {
            EAVRootContainer aContainer = new EAVRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            EAVRootInstance value = new EAVRootInstance() { InstanceID = rng.Next() };
            aContainer.Instances.Add(value);
            value = new EAVRootInstance() { InstanceID = rng.Next() };
            aContainer.Instances.Add(value);

            Assert.IsTrue(aContainer.Instances.Contains(value), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(aContainer, value.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(aContainer.ContainerID, value.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should remain 'New' when property set.");

            aContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aContainer.Instances.Remove(value);

            Assert.IsFalse(aContainer.Instances.Contains(value), "Property 'Instances' was not updated properly.");
            Assert.IsNull(value.Container, "Property 'Container' was not set properly.");
            Assert.IsNull(value.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.AreEqual(ObjectState.Modified, aContainer.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void RootContainerRemoveFromInstancesWhenDeleted()
        {
            EAVRootContainer aContainer = new EAVRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            EAVRootInstance value = new EAVRootInstance() { InstanceID = rng.Next() };
            aContainer.Instances.Add(value);

            Assert.IsTrue(aContainer.Instances.Contains(value), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(aContainer, value.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(aContainer.ContainerID, value.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should remain 'New' when property set.");

            aContainer.MarkUnmodified();
            value.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");
            Assert.AreEqual(ObjectState.Unmodified, value.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aContainer.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aContainer.ObjectState, "Object state failed to transition to 'Deleted'.");
            Assert.AreEqual(ObjectState.Deleted, value.ObjectState, "Object state failed to transition to 'Deleted'.");

            aContainer.Instances.Remove(value);
        }

        [TestMethod]
        public void RootContainerClearInstancesWhenNew()
        {
            EAVRootContainer aContainer = new EAVRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            EAVRootInstance value = new EAVRootInstance() { InstanceID = rng.Next() };
            aContainer.Instances.Add(value);
            value = new EAVRootInstance() { InstanceID = rng.Next() };
            aContainer.Instances.Add(value);

            Assert.IsTrue(aContainer.Instances.Contains(value), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(aContainer, value.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(aContainer.ContainerID, value.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should remain 'New' when property set.");

            aContainer.Instances.Clear();

            Assert.IsFalse(aContainer.Instances.Contains(value), "Property 'Instances' was not updated properly.");
            Assert.IsNull(value.Container, "Property 'Container' was not set properly.");
            Assert.IsNull(value.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.IsFalse(aContainer.Instances.Any(), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void RootContainerClearInstancesWhenUnmodified()
        {
            EAVRootContainer aContainer = new EAVRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            EAVRootInstance value = new EAVRootInstance() { InstanceID = rng.Next() };
            aContainer.Instances.Add(value);
            value = new EAVRootInstance() { InstanceID = rng.Next() };
            aContainer.Instances.Add(value);

            Assert.IsTrue(aContainer.Instances.Contains(value), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(aContainer, value.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(aContainer.ContainerID, value.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should remain 'New' when property set.");

            aContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aContainer.Instances.Clear();

            Assert.IsFalse(aContainer.Instances.Contains(value), "Property 'Instances' was not updated properly.");
            Assert.IsNull(value.Container, "Property 'Container' was not set properly.");
            Assert.IsNull(value.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.IsFalse(aContainer.Instances.Any(), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(ObjectState.Modified, aContainer.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void RootContainerClearInstancesWhenModified()
        {
            EAVRootContainer aContainer = new EAVRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            EAVRootInstance value = new EAVRootInstance() { InstanceID = rng.Next() };
            aContainer.Instances.Add(value);
            value = new EAVRootInstance() { InstanceID = rng.Next() };
            aContainer.Instances.Add(value);

            Assert.IsTrue(aContainer.Instances.Contains(value), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(aContainer, value.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(aContainer.ContainerID, value.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should remain 'New' when property set.");

            aContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aContainer.Instances.Clear();

            Assert.IsFalse(aContainer.Instances.Contains(value), "Property 'Instances' was not updated properly.");
            Assert.IsNull(value.Container, "Property 'Container' was not set properly.");
            Assert.IsNull(value.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.IsFalse(aContainer.Instances.Any(), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(ObjectState.Modified, aContainer.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void RootContainerClearInstancesWhenDeleted()
        {
            EAVRootContainer aContainer = new EAVRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            EAVRootInstance value = new EAVRootInstance() { InstanceID = rng.Next() };
            aContainer.Instances.Add(value);

            Assert.IsTrue(aContainer.Instances.Contains(value), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(aContainer, value.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(aContainer.ContainerID, value.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should remain 'New' when property set.");

            aContainer.MarkUnmodified();
            value.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");
            Assert.AreEqual(ObjectState.Unmodified, value.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aContainer.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aContainer.ObjectState, "Object state failed to transition to 'Deleted'.");
            Assert.AreEqual(ObjectState.Deleted, value.ObjectState, "Object state failed to transition to 'Deleted'.");

            aContainer.Instances.Clear();
        }
        #endregion
        #endregion
    }
}
