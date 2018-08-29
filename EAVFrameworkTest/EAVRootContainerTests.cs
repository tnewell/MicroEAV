using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using EAVFramework.Model;


namespace EAVFrameworkTest
{
    public partial class EAVFrameworkTestHarness
    {
        [TestMethod]
        public void RootContainerCreate()
        {
            ModelRootContainer aRootContainer = new ModelRootContainer();

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
            ModelRootContainer aRootContainer = new ModelRootContainer();

            Assert.AreEqual(ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            aRootContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aRootContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        public void RootContainerStateTransitionNewToUnmodifiedWithValidID()
        {
            ModelRootContainer aRootContainer = new ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            aRootContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aRootContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RootContainerStateTransitionNewToDeleted()
        {
            ModelRootContainer aRootContainer = new ModelRootContainer();

            Assert.AreEqual(ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            aRootContainer.MarkDeleted();
        }

        [TestMethod]
        public void RootContainerStateTransitionUnmodifiedToDeleted()
        {
            ModelRootContainer aRootContainer = new ModelRootContainer() { ContainerID = rng.Next() };

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
            ModelRootContainer aRootContainer = new ModelRootContainer() { ContainerID = rng.Next() };

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
            ModelRootContainer aRootContainer = new ModelRootContainer();

            Assert.AreEqual(ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            int id = rng.Next();
            aRootContainer.ContainerID = id;

            Assert.AreEqual(id, aRootContainer.ContainerID, "Property 'ContainerID' not properly set.");
            Assert.AreEqual(ObjectState.New, aRootContainer.ObjectState, "Object state after setting property 'ContainerID' should be 'New'.");
        }

        [TestMethod]
        public void RootContainerSetIDBeforeUnmodified()
        {
            ModelRootContainer aRootContainer = new ModelRootContainer();

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
            ModelRootContainer aRootContainer = new ModelRootContainer();

            Assert.AreEqual(ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            aRootContainer.MarkUnmodified();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RootContainerSetIDWhenDeleted()
        {
            ModelRootContainer aRootContainer = new ModelRootContainer();

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
            ModelRootContainer aRootContainer = new ModelRootContainer();

            Assert.AreEqual(ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            string value = Guid.NewGuid().ToString();
            aRootContainer.Name = value;

            Assert.AreEqual(value, aRootContainer.Name, "Property 'Name' was not set properly.");
            Assert.AreEqual(ObjectState.New, aRootContainer.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void RootContainerSetNameWhenUnmodified()
        {
            ModelRootContainer aRootContainer = new ModelRootContainer() { ContainerID = rng.Next() };

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
            ModelRootContainer aRootContainer = new ModelRootContainer() { ContainerID = rng.Next() };

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
            ModelRootContainer aRootContainer = new ModelRootContainer() { ContainerID = rng.Next() };

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
            ModelRootContainer aRootContainer = new ModelRootContainer();

            Assert.AreEqual(ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            string value = Guid.NewGuid().ToString();
            aRootContainer.DataName = value;

            Assert.AreEqual(value, aRootContainer.DataName, "Property 'DataName' was not set properly.");
            Assert.AreEqual(ObjectState.New, aRootContainer.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void RootContainerSetDataNameWhenUnmodified()
        {
            ModelRootContainer aRootContainer = new ModelRootContainer() { ContainerID = rng.Next() };

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
            ModelRootContainer aRootContainer = new ModelRootContainer() { ContainerID = rng.Next() };

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
            ModelRootContainer aRootContainer = new ModelRootContainer() { ContainerID = rng.Next() };

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
            ModelRootContainer aRootContainer = new ModelRootContainer();

            Assert.AreEqual(ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            string value = Guid.NewGuid().ToString();
            aRootContainer.DisplayText = value;

            Assert.AreEqual(value, aRootContainer.DisplayText, "Property 'DisplayText' was not set properly.");
            Assert.AreEqual(ObjectState.New, aRootContainer.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void RootContainerSetDisplayTextWhenUnmodified()
        {
            ModelRootContainer aRootContainer = new ModelRootContainer() { ContainerID = rng.Next() };

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
            ModelRootContainer aRootContainer = new ModelRootContainer() { ContainerID = rng.Next() };

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
            ModelRootContainer aRootContainer = new ModelRootContainer() { ContainerID = rng.Next() };

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
            ModelRootContainer anRootContainer = new ModelRootContainer();

            Assert.AreEqual(ObjectState.New, anRootContainer.ObjectState, "Object state should be 'New' on creation.");

            int value = rng.Next();
            anRootContainer.Sequence = value;

            Assert.AreEqual(value, anRootContainer.Sequence, "Property 'Sequence' was not set properly.");
            Assert.AreEqual(ObjectState.New, anRootContainer.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void RootContainerSetSequenceWhenUnmodified()
        {
            ModelRootContainer anRootContainer = new ModelRootContainer() { ContainerID = rng.Next() };

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
            ModelRootContainer anRootContainer = new ModelRootContainer() { ContainerID = rng.Next() };

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
            ModelRootContainer anRootContainer = new ModelRootContainer() { ContainerID = rng.Next() };

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
            ModelRootContainer aRootContainer = new ModelRootContainer();

            Assert.AreEqual(ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            aRootContainer.IsRepeating = true;

            Assert.AreEqual(true, aRootContainer.IsRepeating, "Property 'IsRepeating' was not set properly.");
            Assert.AreEqual(ObjectState.New, aRootContainer.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void RootContainerSetIsRepeatingWhenUnmodified()
        {
            ModelRootContainer aRootContainer = new ModelRootContainer() { ContainerID = rng.Next() };

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
            ModelRootContainer aRootContainer = new ModelRootContainer() { ContainerID = rng.Next() };

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
            ModelRootContainer aRootContainer = new ModelRootContainer() { ContainerID = rng.Next() };

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
            ModelRootContainer aRootContainer = new ModelRootContainer();

            Assert.AreEqual(ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            ModelContext value = new ModelContext() { ContextID = rng.Next() };
            aRootContainer.Context = value;

            Assert.AreEqual(value, aRootContainer.Context, "Property 'Context' was not set properly.");
            Assert.AreEqual(value.ContextID, aRootContainer.ContextID, "Property 'ContextID' was not reported properly.");
            Assert.IsTrue(value.Containers.Contains(aRootContainer), "Property 'Containers' was not updated properly.");
            Assert.AreEqual(ObjectState.New, aRootContainer.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void RootContainerSetContextWhenUnmodified()
        {
            ModelRootContainer aRootContainer = new ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            aRootContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aRootContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            ModelContext value = new ModelContext() { ContextID = rng.Next() };
            aRootContainer.Context = value;

            Assert.AreEqual(value, aRootContainer.Context, "Property 'Context' was not set properly.");
            Assert.AreEqual(value.ContextID, aRootContainer.ContextID, "Property 'ContextID' was not reported properly.");
            Assert.IsTrue(value.Containers.Contains(aRootContainer), "Property 'Containers' was not updated properly.");
            Assert.AreEqual(ObjectState.Modified, aRootContainer.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void RootContainerSetContextWhenModified()
        {
            ModelRootContainer aRootContainer = new ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            aRootContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aRootContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            ModelContext value = new ModelContext() { ContextID = rng.Next() };
            aRootContainer.Context = value;

            Assert.AreEqual(value, aRootContainer.Context, "Property 'Context' was not set properly.");
            Assert.AreEqual(value.ContextID, aRootContainer.ContextID, "Property 'ContextID' was not reported properly.");
            Assert.IsTrue(value.Containers.Contains(aRootContainer), "Property 'Containers' was not updated properly.");
            Assert.AreEqual(ObjectState.Modified, aRootContainer.ObjectState, "Object state failed to transition to 'Modified'.");

            value = new ModelContext() { ContextID = rng.Next() };
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
            ModelRootContainer aRootContainer = new ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            aRootContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aRootContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aRootContainer.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aRootContainer.ObjectState, "Object state failed to transition to 'Deleted'.");

            aRootContainer.Context = new ModelContext() { ContextID = rng.Next() };
        }
        #endregion

        #region ParentContainer
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RootContainerSetParentContainerWithObjectWhenNew()
        {
            ModelRootContainer aRootContainer = new ModelRootContainer();

            Assert.AreEqual(ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            aRootContainer.ParentContainer = new ModelRootContainer() { ContainerID = rng.Next() };
        }

        [TestMethod]
        public void RootContainerSetParentContainerWithNullWhenNew()
        {
            ModelRootContainer aRootContainer = new ModelRootContainer();

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
            ModelRootContainer aRootContainer = new ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            aRootContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aRootContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aRootContainer.ParentContainer = new ModelRootContainer() { ContainerID = rng.Next() };
        }

        [TestMethod]
        public void RootContainerSetParentContainerWithNullWhenUnmodified()
        {
            ModelRootContainer aRootContainer = new ModelRootContainer() { ContainerID = rng.Next() };

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
            ModelRootContainer aRootContainer = new ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            aRootContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aRootContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aRootContainer.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aRootContainer.ObjectState, "Object state failed to transition to 'Deleted'.");

            aRootContainer.ParentContainer = new ModelRootContainer() { ContainerID = rng.Next() };
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RootContainerSetParentContainerWithNullWhenDeleted()
        {
            ModelRootContainer aRootContainer = new ModelRootContainer() { ContainerID = rng.Next() };

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
            ModelRootContainer aParentContainer = new ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aParentContainer.ObjectState, "Object state should be 'New' on creation.");

            ModelChildContainer value = new ModelChildContainer() { ContainerID = rng.Next() };
            aParentContainer.ChildContainers.Add(value);

            Assert.IsTrue(aParentContainer.ChildContainers.Contains(value), "Property 'ChildContainers' was not updated properly.");
            Assert.AreEqual(aParentContainer, value.ParentContainer, "Property 'ParentContainer' was not set properly.");
            Assert.AreEqual(aParentContainer.ContainerID, value.ParentContainerID, "Property 'ParentContainerID' was not set properly.");
            Assert.AreEqual(ObjectState.New, aParentContainer.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void RootContainerAddToChildContainersWhenUnmodified()
        {
            ModelRootContainer aParentContainer = new ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aParentContainer.ObjectState, "Object state should be 'New' on creation.");

            aParentContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aParentContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            ModelChildContainer value = new ModelChildContainer() { ContainerID = rng.Next() };
            aParentContainer.ChildContainers.Add(value);

            Assert.IsTrue(aParentContainer.ChildContainers.Contains(value), "Property 'ChildContainers' was not updated properly.");
            Assert.AreEqual(aParentContainer, value.ParentContainer, "Property 'ParentContainer' was not set properly.");
            Assert.AreEqual(aParentContainer.ContainerID, value.ParentContainerID, "Property 'ParentContainerID' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aParentContainer.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void RootContainerAddToChildContainersWhenModified()
        {
            ModelRootContainer aParentContainer = new ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aParentContainer.ObjectState, "Object state should be 'New' on creation.");

            aParentContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aParentContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            ModelChildContainer value = new ModelChildContainer() { ContainerID = rng.Next() };
            aParentContainer.ChildContainers.Add(value);

            Assert.IsTrue(aParentContainer.ChildContainers.Contains(value), "Property 'ChildContainers' was not updated properly.");
            Assert.AreEqual(aParentContainer, value.ParentContainer, "Property 'ParentContainer' was not set properly.");
            Assert.AreEqual(aParentContainer.ContainerID, value.ParentContainerID, "Property 'ParentContainerID' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aParentContainer.ObjectState, "Object state failed to transition to 'Modified'.");

            value = new ModelChildContainer() { ContainerID = rng.Next() };
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
            ModelRootContainer aParentContainer = new ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aParentContainer.ObjectState, "Object state should be 'New' on creation.");

            aParentContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aParentContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aParentContainer.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aParentContainer.ObjectState, "Object state failed to transition to 'Deleted'.");

            aParentContainer.ChildContainers.Add(new ModelChildContainer() { ContainerID = rng.Next() });
        }

        [TestMethod]
        public void RootContainerRemoveFromChildContainersWhenNew()
        {
            ModelRootContainer aParentContainer = new ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aParentContainer.ObjectState, "Object state should be 'New' on creation.");

            ModelChildContainer value = new ModelChildContainer() { ContainerID = rng.Next() };
            aParentContainer.ChildContainers.Add(value);
            value = new ModelChildContainer() { ContainerID = rng.Next() };
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
            ModelRootContainer aParentContainer = new ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aParentContainer.ObjectState, "Object state should be 'New' on creation.");

            ModelChildContainer value = new ModelChildContainer() { ContainerID = rng.Next() };
            aParentContainer.ChildContainers.Add(value);
            value = new ModelChildContainer() { ContainerID = rng.Next() };
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
            ModelRootContainer aParentContainer = new ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aParentContainer.ObjectState, "Object state should be 'New' on creation.");

            aParentContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aParentContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            ModelChildContainer value = new ModelChildContainer() { ContainerID = rng.Next() };
            aParentContainer.ChildContainers.Add(value);
            value = new ModelChildContainer() { ContainerID = rng.Next() };
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
            ModelRootContainer aParentContainer = new ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aParentContainer.ObjectState, "Object state should be 'New' on creation.");

            ModelChildContainer value = new ModelChildContainer() { ContainerID = rng.Next() };
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
            ModelRootContainer aParentContainer = new ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aParentContainer.ObjectState, "Object state should be 'New' on creation.");

            ModelChildContainer value = new ModelChildContainer() { ContainerID = rng.Next() };
            aParentContainer.ChildContainers.Add(value);
            value = new ModelChildContainer() { ContainerID = rng.Next() };
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
            ModelRootContainer aParentContainer = new ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aParentContainer.ObjectState, "Object state should be 'New' on creation.");

            ModelChildContainer value = new ModelChildContainer() { ContainerID = rng.Next() };
            aParentContainer.ChildContainers.Add(value);
            value = new ModelChildContainer() { ContainerID = rng.Next() };
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
            ModelRootContainer aParentContainer = new ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aParentContainer.ObjectState, "Object state should be 'New' on creation.");

            aParentContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aParentContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            ModelChildContainer value = new ModelChildContainer() { ContainerID = rng.Next() };
            aParentContainer.ChildContainers.Add(value);
            value = new ModelChildContainer() { ContainerID = rng.Next() };
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
            ModelRootContainer aParentContainer = new ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aParentContainer.ObjectState, "Object state should be 'New' on creation.");

            ModelChildContainer value = new ModelChildContainer() { ContainerID = rng.Next() };
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
            ModelRootContainer aContainer = new ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            ModelAttribute value = new ModelAttribute() { AttributeID = rng.Next() };
            aContainer.Attributes.Add(value);

            Assert.IsTrue(aContainer.Attributes.Contains(value), "Property 'Attributes' was not updated properly.");
            Assert.AreEqual(aContainer, value.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(aContainer.ContainerID, value.ContainerID, "Property 'ContainerID' was not set properly.");
            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void RootContainerAddToAttributesWhenUnmodified()
        {
            ModelRootContainer aContainer = new ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            ModelAttribute value = new ModelAttribute() { AttributeID = rng.Next() };
            aContainer.Attributes.Add(value);

            Assert.IsTrue(aContainer.Attributes.Contains(value), "Property 'Attributes' was not updated properly.");
            Assert.AreEqual(aContainer, value.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(aContainer.ContainerID, value.ContainerID, "Property 'ContainerID' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aContainer.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void RootContainerAddToAttributesWhenModified()
        {
            ModelRootContainer aContainer = new ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            ModelAttribute value = new ModelAttribute() { AttributeID = rng.Next() };
            aContainer.Attributes.Add(value);

            Assert.IsTrue(aContainer.Attributes.Contains(value), "Property 'Attributes' was not updated properly.");
            Assert.AreEqual(aContainer, value.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(aContainer.ContainerID, value.ContainerID, "Property 'ContainerID' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aContainer.ObjectState, "Object state failed to transition to 'Modified'.");

            value = new ModelAttribute() { AttributeID = rng.Next() };
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
            ModelRootContainer aContainer = new ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aContainer.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aContainer.ObjectState, "Object state failed to transition to 'Deleted'.");

            aContainer.Attributes.Add(new ModelAttribute() { AttributeID = rng.Next() });
        }

        [TestMethod]
        public void RootContainerRemoveFromAttributesWhenNew()
        {
            ModelRootContainer aContainer = new ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            ModelAttribute value = new ModelAttribute() { AttributeID = rng.Next() };
            aContainer.Attributes.Add(value);
            value = new ModelAttribute() { AttributeID = rng.Next() };
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
            ModelRootContainer aContainer = new ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            ModelAttribute value = new ModelAttribute() { AttributeID = rng.Next() };
            aContainer.Attributes.Add(value);
            value = new ModelAttribute() { AttributeID = rng.Next() };
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
            ModelRootContainer aContainer = new ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            ModelAttribute value = new ModelAttribute() { AttributeID = rng.Next() };
            aContainer.Attributes.Add(value);
            value = new ModelAttribute() { AttributeID = rng.Next() };
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
            ModelRootContainer aContainer = new ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            ModelAttribute value = new ModelAttribute() { AttributeID = rng.Next() };
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
            ModelRootContainer aContainer = new ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            ModelAttribute value = new ModelAttribute() { AttributeID = rng.Next() };
            aContainer.Attributes.Add(value);
            value = new ModelAttribute() { AttributeID = rng.Next() };
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
            ModelRootContainer aContainer = new ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            ModelAttribute value = new ModelAttribute() { AttributeID = rng.Next() };
            aContainer.Attributes.Add(value);
            value = new ModelAttribute() { AttributeID = rng.Next() };
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
            ModelRootContainer aContainer = new ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            ModelAttribute value = new ModelAttribute() { AttributeID = rng.Next() };
            aContainer.Attributes.Add(value);
            value = new ModelAttribute() { AttributeID = rng.Next() };
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
            ModelRootContainer aContainer = new ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            ModelAttribute value = new ModelAttribute() { AttributeID = rng.Next() };
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
            ModelRootContainer aContainer = new ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            ModelRootInstance value = new ModelRootInstance() { InstanceID = rng.Next() };
            aContainer.Instances.Add(value);

            Assert.IsTrue(aContainer.Instances.Contains(value), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(aContainer, value.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(aContainer.ContainerID, value.ContainerID, "Property 'ContainerID' was not set properly.");
            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void RootContainerAddToInstancesWhenUnmodified()
        {
            ModelRootContainer aContainer = new ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            ModelRootInstance value = new ModelRootInstance() { InstanceID = rng.Next() };
            aContainer.Instances.Add(value);

            Assert.IsTrue(aContainer.Instances.Contains(value), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(aContainer, value.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(aContainer.ContainerID, value.ContainerID, "Property 'ContainerID' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aContainer.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void RootContainerAddToInstancesWhenModified()
        {
            ModelRootContainer aContainer = new ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            ModelRootInstance value = new ModelRootInstance() { InstanceID = rng.Next() };
            aContainer.Instances.Add(value);

            Assert.IsTrue(aContainer.Instances.Contains(value), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(aContainer, value.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(aContainer.ContainerID, value.ContainerID, "Property 'ContainerID' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aContainer.ObjectState, "Object state failed to transition to 'Modified'.");

            value = new ModelRootInstance() { InstanceID = rng.Next() };
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
            ModelRootContainer aContainer = new ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aContainer.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aContainer.ObjectState, "Object state failed to transition to 'Deleted'.");

            aContainer.Instances.Add(new ModelRootInstance() { InstanceID = rng.Next() });
        }

        [TestMethod]
        public void RootContainerRemoveFromInstancesWhenNew()
        {
            ModelRootContainer aContainer = new ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            ModelRootInstance value = new ModelRootInstance() { InstanceID = rng.Next() };
            aContainer.Instances.Add(value);
            value = new ModelRootInstance() { InstanceID = rng.Next() };
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
            ModelRootContainer aContainer = new ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            ModelRootInstance value = new ModelRootInstance() { InstanceID = rng.Next() };
            aContainer.Instances.Add(value);
            value = new ModelRootInstance() { InstanceID = rng.Next() };
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
            ModelRootContainer aContainer = new ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            ModelRootInstance value = new ModelRootInstance() { InstanceID = rng.Next() };
            aContainer.Instances.Add(value);
            value = new ModelRootInstance() { InstanceID = rng.Next() };
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
            ModelRootContainer aContainer = new ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            ModelRootInstance value = new ModelRootInstance() { InstanceID = rng.Next() };
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
            ModelRootContainer aContainer = new ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            ModelRootInstance value = new ModelRootInstance() { InstanceID = rng.Next() };
            aContainer.Instances.Add(value);
            value = new ModelRootInstance() { InstanceID = rng.Next() };
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
            ModelRootContainer aContainer = new ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            ModelRootInstance value = new ModelRootInstance() { InstanceID = rng.Next() };
            aContainer.Instances.Add(value);
            value = new ModelRootInstance() { InstanceID = rng.Next() };
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
            ModelRootContainer aContainer = new ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            ModelRootInstance value = new ModelRootInstance() { InstanceID = rng.Next() };
            aContainer.Instances.Add(value);
            value = new ModelRootInstance() { InstanceID = rng.Next() };
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
            ModelRootContainer aContainer = new ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            ModelRootInstance value = new ModelRootInstance() { InstanceID = rng.Next() };
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
