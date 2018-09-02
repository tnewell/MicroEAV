using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace EAVModelTest
{
    public partial class EAVModelTestHarness
    {
        [TestMethod]
        public void RootContainerCreate()
        {
            EAVModelLibrary.ModelRootContainer aRootContainer = new EAVModelLibrary.ModelRootContainer();

            Assert.AreEqual(EAV.Model.ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

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
            EAVModelLibrary.ModelRootContainer aRootContainer = new EAVModelLibrary.ModelRootContainer();

            Assert.AreEqual(EAV.Model.ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            aRootContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aRootContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        public void RootContainerStateTransitionNewToUnmodifiedWithValidID()
        {
            EAVModelLibrary.ModelRootContainer aRootContainer = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            aRootContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aRootContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RootContainerStateTransitionNewToDeleted()
        {
            EAVModelLibrary.ModelRootContainer aRootContainer = new EAVModelLibrary.ModelRootContainer();

            Assert.AreEqual(EAV.Model.ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            aRootContainer.MarkDeleted();
        }

        [TestMethod]
        public void RootContainerStateTransitionUnmodifiedToDeleted()
        {
            EAVModelLibrary.ModelRootContainer aRootContainer = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            aRootContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aRootContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aRootContainer.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, aRootContainer.ObjectState, "Object state failed to transition to 'Deleted'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RootContainerStateTransitionDeletedToUnmodified()
        {
            EAVModelLibrary.ModelRootContainer aRootContainer = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            aRootContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aRootContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aRootContainer.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, aRootContainer.ObjectState, "Object state failed to transition to 'Deleted'.");

            aRootContainer.MarkUnmodified();
        }
        #endregion

        #region ID Property
        [TestMethod]
        public void RootContainerSetIDWhenNew()
        {
            EAVModelLibrary.ModelRootContainer aRootContainer = new EAVModelLibrary.ModelRootContainer();

            Assert.AreEqual(EAV.Model.ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            int id = rng.Next();
            aRootContainer.ContainerID = id;

            Assert.AreEqual(id, aRootContainer.ContainerID, "Property 'ContainerID' not properly set.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aRootContainer.ObjectState, "Object state after setting property 'ContainerID' should be 'New'.");
        }

        [TestMethod]
        public void RootContainerSetIDBeforeUnmodified()
        {
            EAVModelLibrary.ModelRootContainer aRootContainer = new EAVModelLibrary.ModelRootContainer();

            Assert.AreEqual(EAV.Model.ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            int id = rng.Next();
            aRootContainer.ContainerID = id;

            Assert.AreEqual(id, aRootContainer.ContainerID, "Property 'ContainerID' not properly set.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aRootContainer.ObjectState, "Object state after setting property 'ContainerID' should be 'New'.");

            aRootContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aRootContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RootContainerSetIDAfterUnmodified()
        {
            EAVModelLibrary.ModelRootContainer aRootContainer = new EAVModelLibrary.ModelRootContainer();

            Assert.AreEqual(EAV.Model.ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            aRootContainer.MarkUnmodified();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RootContainerSetIDWhenDeleted()
        {
            EAVModelLibrary.ModelRootContainer aRootContainer = new EAVModelLibrary.ModelRootContainer();

            Assert.AreEqual(EAV.Model.ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            int id = rng.Next();
            aRootContainer.ContainerID = id;

            Assert.AreEqual(id, aRootContainer.ContainerID, "Property 'ContainerID' not properly set.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aRootContainer.ObjectState, "Object state after setting property 'ContainerID' should be 'New'.");

            aRootContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aRootContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aRootContainer.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, aRootContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            id = rng.Next();
            aRootContainer.ContainerID = id;
        }
        #endregion

        #region Primitive Properties
        #region Name
        [TestMethod]
        public void RootContainerSetNameWhenNew()
        {
            EAVModelLibrary.ModelRootContainer aRootContainer = new EAVModelLibrary.ModelRootContainer();

            Assert.AreEqual(EAV.Model.ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            string value = Guid.NewGuid().ToString();
            aRootContainer.Name = value;

            Assert.AreEqual(value, aRootContainer.Name, "Property 'Name' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aRootContainer.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void RootContainerSetNameWhenUnmodified()
        {
            EAVModelLibrary.ModelRootContainer aRootContainer = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            aRootContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aRootContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            aRootContainer.Name = value;

            Assert.AreEqual(value, aRootContainer.Name, "Property 'Name' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aRootContainer.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void RootContainerSetNameWhenModified()
        {
            EAVModelLibrary.ModelRootContainer aRootContainer = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            aRootContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aRootContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            aRootContainer.Name = value;

            Assert.AreEqual(value, aRootContainer.Name, "Property 'Name' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aRootContainer.ObjectState, "Object state failed to transition to 'Modified'.");

            value = Guid.NewGuid().ToString();
            aRootContainer.Name = value;

            Assert.AreEqual(value, aRootContainer.Name, "Property 'Name' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aRootContainer.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RootContainerSetNameWhenDeleted()
        {
            EAVModelLibrary.ModelRootContainer aRootContainer = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            aRootContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aRootContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aRootContainer.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, aRootContainer.ObjectState, "Object state failed to transition to 'Deleted'.");

            aRootContainer.Name = Guid.NewGuid().ToString();
        }
        #endregion

        #region DataName
        [TestMethod]
        public void RootContainerSetDataNameWhenNew()
        {
            EAVModelLibrary.ModelRootContainer aRootContainer = new EAVModelLibrary.ModelRootContainer();

            Assert.AreEqual(EAV.Model.ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            string value = Guid.NewGuid().ToString();
            aRootContainer.DataName = value;

            Assert.AreEqual(value, aRootContainer.DataName, "Property 'DataName' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aRootContainer.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void RootContainerSetDataNameWhenUnmodified()
        {
            EAVModelLibrary.ModelRootContainer aRootContainer = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            aRootContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aRootContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            aRootContainer.DataName = value;

            Assert.AreEqual(value, aRootContainer.DataName, "Property 'DataName' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aRootContainer.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void RootContainerSetDataNameWhenModified()
        {
            EAVModelLibrary.ModelRootContainer aRootContainer = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            aRootContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aRootContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            aRootContainer.DataName = value;

            Assert.AreEqual(value, aRootContainer.DataName, "Property 'DataName' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aRootContainer.ObjectState, "Object state failed to transition to 'Modified'.");

            value = Guid.NewGuid().ToString();
            aRootContainer.DataName = value;

            Assert.AreEqual(value, aRootContainer.DataName, "Property 'DataName' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aRootContainer.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RootContainerSetDataNameWhenDeleted()
        {
            EAVModelLibrary.ModelRootContainer aRootContainer = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            aRootContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aRootContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aRootContainer.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, aRootContainer.ObjectState, "Object state failed to transition to 'Deleted'.");

            aRootContainer.DataName = Guid.NewGuid().ToString();
        }
        #endregion

        #region DisplayText
        [TestMethod]
        public void RootContainerSetDisplayTextWhenNew()
        {
            EAVModelLibrary.ModelRootContainer aRootContainer = new EAVModelLibrary.ModelRootContainer();

            Assert.AreEqual(EAV.Model.ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            string value = Guid.NewGuid().ToString();
            aRootContainer.DisplayText = value;

            Assert.AreEqual(value, aRootContainer.DisplayText, "Property 'DisplayText' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aRootContainer.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void RootContainerSetDisplayTextWhenUnmodified()
        {
            EAVModelLibrary.ModelRootContainer aRootContainer = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            aRootContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aRootContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            aRootContainer.DisplayText = value;

            Assert.AreEqual(value, aRootContainer.DisplayText, "Property 'DisplayText' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aRootContainer.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void RootContainerSetDisplayTextWhenModified()
        {
            EAVModelLibrary.ModelRootContainer aRootContainer = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            aRootContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aRootContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            aRootContainer.DisplayText = value;

            Assert.AreEqual(value, aRootContainer.DisplayText, "Property 'DisplayText' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aRootContainer.ObjectState, "Object state failed to transition to 'Modified'.");

            value = Guid.NewGuid().ToString();
            aRootContainer.DisplayText = value;

            Assert.AreEqual(value, aRootContainer.DisplayText, "Property 'DisplayText' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aRootContainer.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RootContainerSetDisplayTextWhenDeleted()
        {
            EAVModelLibrary.ModelRootContainer aRootContainer = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            aRootContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aRootContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aRootContainer.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, aRootContainer.ObjectState, "Object state failed to transition to 'Deleted'.");

            aRootContainer.DisplayText = Guid.NewGuid().ToString();
        }
        #endregion

        #region Sequence
        [TestMethod]
        public void RootContainerSetSequenceWhenNew()
        {
            EAVModelLibrary.ModelRootContainer anRootContainer = new EAVModelLibrary.ModelRootContainer();

            Assert.AreEqual(EAV.Model.ObjectState.New, anRootContainer.ObjectState, "Object state should be 'New' on creation.");

            int value = rng.Next();
            anRootContainer.Sequence = value;

            Assert.AreEqual(value, anRootContainer.Sequence, "Property 'Sequence' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, anRootContainer.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void RootContainerSetSequenceWhenUnmodified()
        {
            EAVModelLibrary.ModelRootContainer anRootContainer = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, anRootContainer.ObjectState, "Object state should be 'New' on creation.");

            anRootContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, anRootContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            int value = rng.Next();
            anRootContainer.Sequence = value;

            Assert.AreEqual(value, anRootContainer.Sequence, "Property 'Sequence' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, anRootContainer.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void RootContainerSetSequenceWhenModified()
        {
            EAVModelLibrary.ModelRootContainer anRootContainer = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, anRootContainer.ObjectState, "Object state should be 'New' on creation.");

            anRootContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, anRootContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            int value = rng.Next();
            anRootContainer.Sequence = value;

            Assert.AreEqual(value, anRootContainer.Sequence, "Property 'Sequence' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, anRootContainer.ObjectState, "Object state failed to transition to 'Modified'.");

            value = rng.Next();
            anRootContainer.Sequence = value;

            Assert.AreEqual(value, anRootContainer.Sequence, "Property 'Sequence' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, anRootContainer.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RootContainerSetSequenceWhenDeleted()
        {
            EAVModelLibrary.ModelRootContainer anRootContainer = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, anRootContainer.ObjectState, "Object state should be 'New' on creation.");

            anRootContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, anRootContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anRootContainer.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, anRootContainer.ObjectState, "Object state failed to transition to 'Deleted'.");

            anRootContainer.Sequence = rng.Next();
        }
        #endregion

        #region IsRepeating
        [TestMethod]
        public void RootContainerSetIsRepeatingWhenNew()
        {
            EAVModelLibrary.ModelRootContainer aRootContainer = new EAVModelLibrary.ModelRootContainer();

            Assert.AreEqual(EAV.Model.ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            aRootContainer.IsRepeating = true;

            Assert.AreEqual(true, aRootContainer.IsRepeating, "Property 'IsRepeating' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aRootContainer.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void RootContainerSetIsRepeatingWhenUnmodified()
        {
            EAVModelLibrary.ModelRootContainer aRootContainer = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            aRootContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aRootContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aRootContainer.IsRepeating = true;

            Assert.AreEqual(true, aRootContainer.IsRepeating, "Property 'IsRepeating' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aRootContainer.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void RootContainerSetIsRepeatingWhenModified()
        {
            EAVModelLibrary.ModelRootContainer aRootContainer = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            aRootContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aRootContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aRootContainer.IsRepeating = true;

            Assert.AreEqual(true, aRootContainer.IsRepeating, "Property 'IsRepeating' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aRootContainer.ObjectState, "Object state failed to transition to 'Modified'.");

            aRootContainer.IsRepeating = false;

            Assert.AreEqual(false, aRootContainer.IsRepeating, "Property 'IsRepeating' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aRootContainer.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RootContainerSetIsRepeatingWhenDeleted()
        {
            EAVModelLibrary.ModelRootContainer aRootContainer = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            aRootContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aRootContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aRootContainer.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, aRootContainer.ObjectState, "Object state failed to transition to 'Deleted'.");

            aRootContainer.IsRepeating = true;
        }
        #endregion
        #endregion

        #region Object Properties
        #region Context
        [TestMethod]
        public void RootContainerSetContextWhenNew()
        {
            EAVModelLibrary.ModelRootContainer aRootContainer = new EAVModelLibrary.ModelRootContainer();

            Assert.AreEqual(EAV.Model.ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelContext value = new EAVModelLibrary.ModelContext() { ContextID = rng.Next() };
            aRootContainer.Context = value;

            Assert.AreEqual(value, aRootContainer.Context, "Property 'Context' was not set properly.");
            Assert.AreEqual(value.ContextID, aRootContainer.ContextID, "Property 'ContextID' was not reported properly.");
            Assert.IsTrue(value.Containers.Contains(aRootContainer), "Property 'Containers' was not updated properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aRootContainer.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void RootContainerSetContextWhenUnmodified()
        {
            EAVModelLibrary.ModelRootContainer aRootContainer = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            aRootContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aRootContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVModelLibrary.ModelContext value = new EAVModelLibrary.ModelContext() { ContextID = rng.Next() };
            aRootContainer.Context = value;

            Assert.AreEqual(value, aRootContainer.Context, "Property 'Context' was not set properly.");
            Assert.AreEqual(value.ContextID, aRootContainer.ContextID, "Property 'ContextID' was not reported properly.");
            Assert.IsTrue(value.Containers.Contains(aRootContainer), "Property 'Containers' was not updated properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aRootContainer.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void RootContainerSetContextWhenModified()
        {
            EAVModelLibrary.ModelRootContainer aRootContainer = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            aRootContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aRootContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVModelLibrary.ModelContext value = new EAVModelLibrary.ModelContext() { ContextID = rng.Next() };
            aRootContainer.Context = value;

            Assert.AreEqual(value, aRootContainer.Context, "Property 'Context' was not set properly.");
            Assert.AreEqual(value.ContextID, aRootContainer.ContextID, "Property 'ContextID' was not reported properly.");
            Assert.IsTrue(value.Containers.Contains(aRootContainer), "Property 'Containers' was not updated properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aRootContainer.ObjectState, "Object state failed to transition to 'Modified'.");

            value = new EAVModelLibrary.ModelContext() { ContextID = rng.Next() };
            aRootContainer.Context = value;

            Assert.AreEqual(value, aRootContainer.Context, "Property 'Context' was not set properly.");
            Assert.AreEqual(value.ContextID, aRootContainer.ContextID, "Property 'ContextID' was not reported properly.");
            Assert.IsTrue(value.Containers.Contains(aRootContainer), "Property 'Containers' was not updated properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aRootContainer.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RootContainerSetContextWhenDeleted()
        {
            EAVModelLibrary.ModelRootContainer aRootContainer = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            aRootContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aRootContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aRootContainer.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, aRootContainer.ObjectState, "Object state failed to transition to 'Deleted'.");

            aRootContainer.Context = new EAVModelLibrary.ModelContext() { ContextID = rng.Next() };
        }
        #endregion

        #region ParentContainer
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RootContainerSetParentContainerWithObjectWhenNew()
        {
            EAVModelLibrary.ModelRootContainer aRootContainer = new EAVModelLibrary.ModelRootContainer();

            Assert.AreEqual(EAV.Model.ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            aRootContainer.ParentContainer = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };
        }

        [TestMethod]
        public void RootContainerSetParentContainerWithNullWhenNew()
        {
            EAVModelLibrary.ModelRootContainer aRootContainer = new EAVModelLibrary.ModelRootContainer();

            Assert.AreEqual(EAV.Model.ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            aRootContainer.ParentContainer = null;

            Assert.IsNull(aRootContainer.ParentContainer, "Property 'ParentContainer' was not set properly.");
            Assert.IsNull(aRootContainer.ParentContainerID, "Property 'ParentContainerID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aRootContainer.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RootContainerSetParentContainerWithObjectWhenUnmodified()
        {
            EAVModelLibrary.ModelRootContainer aRootContainer = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            aRootContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aRootContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aRootContainer.ParentContainer = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };
        }

        [TestMethod]
        public void RootContainerSetParentContainerWithNullWhenUnmodified()
        {
            EAVModelLibrary.ModelRootContainer aRootContainer = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            aRootContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aRootContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aRootContainer.ParentContainer = null;

            Assert.IsNull(aRootContainer.ParentContainer, "Property 'ParentContainer' was not set properly.");
            Assert.IsNull(aRootContainer.ParentContainerID, "Property 'ParentContainerID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aRootContainer.ObjectState, "Object state should remain 'Unmodified' when property set.");
        }

        // Set When Modified - N/A

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RootContainerSetParentContainerWithObjectWhenDeleted()
        {
            EAVModelLibrary.ModelRootContainer aRootContainer = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            aRootContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aRootContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aRootContainer.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, aRootContainer.ObjectState, "Object state failed to transition to 'Deleted'.");

            aRootContainer.ParentContainer = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RootContainerSetParentContainerWithNullWhenDeleted()
        {
            EAVModelLibrary.ModelRootContainer aRootContainer = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aRootContainer.ObjectState, "Object state should be 'New' on creation.");

            aRootContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aRootContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aRootContainer.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, aRootContainer.ObjectState, "Object state failed to transition to 'Deleted'.");

            aRootContainer.ParentContainer = null;
        }
        #endregion
        #endregion

        #region Collection Properties
        #region ChildContainers
        [TestMethod]
        public void RootContainerAddToChildContainersWhenNew()
        {
            EAVModelLibrary.ModelRootContainer aParentContainer = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aParentContainer.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelChildContainer value = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };
            aParentContainer.ChildContainers.Add(value);

            Assert.IsTrue(aParentContainer.ChildContainers.Contains(value), "Property 'ChildContainers' was not updated properly.");
            Assert.AreEqual(aParentContainer, value.ParentContainer, "Property 'ParentContainer' was not set properly.");
            Assert.AreEqual(aParentContainer.ContainerID, value.ParentContainerID, "Property 'ParentContainerID' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aParentContainer.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void RootContainerAddToChildContainersWhenUnmodified()
        {
            EAVModelLibrary.ModelRootContainer aParentContainer = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aParentContainer.ObjectState, "Object state should be 'New' on creation.");

            aParentContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aParentContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVModelLibrary.ModelChildContainer value = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };
            aParentContainer.ChildContainers.Add(value);

            Assert.IsTrue(aParentContainer.ChildContainers.Contains(value), "Property 'ChildContainers' was not updated properly.");
            Assert.AreEqual(aParentContainer, value.ParentContainer, "Property 'ParentContainer' was not set properly.");
            Assert.AreEqual(aParentContainer.ContainerID, value.ParentContainerID, "Property 'ParentContainerID' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aParentContainer.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void RootContainerAddToChildContainersWhenModified()
        {
            EAVModelLibrary.ModelRootContainer aParentContainer = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aParentContainer.ObjectState, "Object state should be 'New' on creation.");

            aParentContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aParentContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVModelLibrary.ModelChildContainer value = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };
            aParentContainer.ChildContainers.Add(value);

            Assert.IsTrue(aParentContainer.ChildContainers.Contains(value), "Property 'ChildContainers' was not updated properly.");
            Assert.AreEqual(aParentContainer, value.ParentContainer, "Property 'ParentContainer' was not set properly.");
            Assert.AreEqual(aParentContainer.ContainerID, value.ParentContainerID, "Property 'ParentContainerID' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aParentContainer.ObjectState, "Object state failed to transition to 'Modified'.");

            value = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };
            aParentContainer.ChildContainers.Add(value);

            Assert.IsTrue(aParentContainer.ChildContainers.Contains(value), "Property 'ChildContainers' was not updated properly.");
            Assert.AreEqual(aParentContainer, value.ParentContainer, "Property 'ParentContainer' was not set properly.");
            Assert.AreEqual(aParentContainer.ContainerID, value.ParentContainerID, "Property 'ParentContainerID' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aParentContainer.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void RootContainerAddToChildContainersWhenDeleted()
        {
            EAVModelLibrary.ModelRootContainer aParentContainer = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aParentContainer.ObjectState, "Object state should be 'New' on creation.");

            aParentContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aParentContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aParentContainer.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, aParentContainer.ObjectState, "Object state failed to transition to 'Deleted'.");

            aParentContainer.ChildContainers.Add(new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() });
        }

        [TestMethod]
        public void RootContainerRemoveFromChildContainersWhenNew()
        {
            EAVModelLibrary.ModelRootContainer aParentContainer = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aParentContainer.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelChildContainer value = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };
            aParentContainer.ChildContainers.Add(value);
            value = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };
            aParentContainer.ChildContainers.Add(value);

            Assert.IsTrue(aParentContainer.ChildContainers.Contains(value), "Property 'ChildContainers' was not updated properly.");
            Assert.AreEqual(aParentContainer, value.ParentContainer, "Property 'ParentContainer' was not set properly.");
            Assert.AreEqual(aParentContainer.ContainerID, value.ParentContainerID, "Property 'ParentContainerID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aParentContainer.ObjectState, "Object state should remain 'New' when property set.");

            aParentContainer.ChildContainers.Remove(value);

            Assert.IsFalse(aParentContainer.ChildContainers.Contains(value), "Property 'ChildContainers' was not updated properly.");
            Assert.IsNull(value.ParentContainer, "Property 'ParentContainer' was not set properly.");
            Assert.IsNull(value.ParentContainerID, "Property 'ParentContainerID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aParentContainer.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void RootContainerRemoveFromChildContainersWhenUnmodified()
        {
            EAVModelLibrary.ModelRootContainer aParentContainer = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aParentContainer.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelChildContainer value = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };
            aParentContainer.ChildContainers.Add(value);
            value = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };
            aParentContainer.ChildContainers.Add(value);

            Assert.IsTrue(aParentContainer.ChildContainers.Contains(value), "Property 'ChildContainers' was not updated properly.");
            Assert.AreEqual(aParentContainer, value.ParentContainer, "Property 'ParentContainer' was not set properly.");
            Assert.AreEqual(aParentContainer.ContainerID, value.ParentContainerID, "Property 'ParentContainerID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aParentContainer.ObjectState, "Object state should remain 'New' when property set.");

            aParentContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aParentContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aParentContainer.ChildContainers.Remove(value);

            Assert.IsFalse(aParentContainer.ChildContainers.Contains(value), "Property 'ChildContainers' was not updated properly.");
            Assert.IsNull(value.ParentContainer, "Property 'ParentContainer' was not set properly.");
            Assert.IsNull(value.ParentContainerID, "Property 'ParentContainerID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aParentContainer.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void RootContainerRemoveFromChildContainersWhenModified()
        {
            EAVModelLibrary.ModelRootContainer aParentContainer = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aParentContainer.ObjectState, "Object state should be 'New' on creation.");

            aParentContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aParentContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVModelLibrary.ModelChildContainer value = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };
            aParentContainer.ChildContainers.Add(value);
            value = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };
            aParentContainer.ChildContainers.Add(value);

            Assert.IsTrue(aParentContainer.ChildContainers.Contains(value), "Property 'ChildContainers' was not updated properly.");
            Assert.AreEqual(aParentContainer, value.ParentContainer, "Property 'ParentContainer' was not set properly.");
            Assert.AreEqual(aParentContainer.ContainerID, value.ParentContainerID, "Property 'ParentContainerID' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aParentContainer.ObjectState, "Object state failed to transition to 'Modified'.");

            aParentContainer.ChildContainers.Remove(value);

            Assert.IsFalse(aParentContainer.ChildContainers.Contains(value), "Property 'ChildContainers' was not updated properly.");
            Assert.IsNull(value.ParentContainer, "Property 'ParentContainer' was not set properly.");
            Assert.IsNull(value.ParentContainerID, "Property 'ParentContainerID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aParentContainer.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void RootContainerRemoveFromChildContainersWhenDeleted()
        {
            EAVModelLibrary.ModelRootContainer aParentContainer = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aParentContainer.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelChildContainer value = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };
            aParentContainer.ChildContainers.Add(value);

            aParentContainer.MarkUnmodified();
            value.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aParentContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");
            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, value.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aParentContainer.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, aParentContainer.ObjectState, "Object state failed to transition to 'Deleted'.");
            Assert.AreEqual(EAV.Model.ObjectState.Deleted, value.ObjectState, "Object state failed to transition to 'Deleted'.");

            aParentContainer.ChildContainers.Remove(value);
        }

        [TestMethod]
        public void RootContainerClearChildContainersWhenNew()
        {
            EAVModelLibrary.ModelRootContainer aParentContainer = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aParentContainer.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelChildContainer value = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };
            aParentContainer.ChildContainers.Add(value);
            value = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };
            aParentContainer.ChildContainers.Add(value);

            Assert.IsTrue(aParentContainer.ChildContainers.Contains(value), "Property 'ChildContainers' was not updated properly.");
            Assert.AreEqual(aParentContainer, value.ParentContainer, "Property 'ParentContainer' was not set properly.");
            Assert.AreEqual(aParentContainer.ContainerID, value.ParentContainerID, "Property 'ParentContainerID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aParentContainer.ObjectState, "Object state should remain 'New' when property set.");

            aParentContainer.ChildContainers.Clear();

            Assert.IsFalse(aParentContainer.ChildContainers.Contains(value), "Property 'ChildContainers' was not updated properly.");
            Assert.IsNull(value.ParentContainer, "Property 'ParentContainer' was not set properly.");
            Assert.IsNull(value.ParentContainerID, "Property 'ParentContainerID' was not reported properly.");
            Assert.IsFalse(aParentContainer.ChildContainers.Any(), "Property 'ChildContainers' was not updated properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aParentContainer.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void RootContainerClearChildContainersWhenUnmodified()
        {
            EAVModelLibrary.ModelRootContainer aParentContainer = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aParentContainer.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelChildContainer value = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };
            aParentContainer.ChildContainers.Add(value);
            value = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };
            aParentContainer.ChildContainers.Add(value);

            Assert.IsTrue(aParentContainer.ChildContainers.Contains(value), "Property 'ChildContainers' was not updated properly.");
            Assert.AreEqual(aParentContainer, value.ParentContainer, "Property 'ParentContainer' was not set properly.");
            Assert.AreEqual(aParentContainer.ContainerID, value.ParentContainerID, "Property 'ParentContainerID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aParentContainer.ObjectState, "Object state should remain 'New' when property set.");

            aParentContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aParentContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aParentContainer.ChildContainers.Clear();

            Assert.IsFalse(aParentContainer.ChildContainers.Contains(value), "Property 'ChildContainers' was not updated properly.");
            Assert.IsNull(value.ParentContainer, "Property 'ParentContainer' was not set properly.");
            Assert.IsNull(value.ParentContainerID, "Property 'ParentContainerID' was not reported properly.");
            Assert.IsFalse(aParentContainer.ChildContainers.Any(), "Property 'ChildContainers' was not updated properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aParentContainer.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void RootContainerClearChildContainersWhenModified()
        {
            EAVModelLibrary.ModelRootContainer aParentContainer = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aParentContainer.ObjectState, "Object state should be 'New' on creation.");

            aParentContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aParentContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVModelLibrary.ModelChildContainer value = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };
            aParentContainer.ChildContainers.Add(value);
            value = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };
            aParentContainer.ChildContainers.Add(value);

            Assert.IsTrue(aParentContainer.ChildContainers.Contains(value), "Property 'ChildContainers' was not updated properly.");
            Assert.AreEqual(aParentContainer, value.ParentContainer, "Property 'ParentContainer' was not set properly.");
            Assert.AreEqual(aParentContainer.ContainerID, value.ParentContainerID, "Property 'ParentContainerID' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aParentContainer.ObjectState, "Object state failed to transition to 'Modified'.");

            aParentContainer.ChildContainers.Clear();

            Assert.IsFalse(aParentContainer.ChildContainers.Contains(value), "Property 'ChildContainers' was not updated properly.");
            Assert.IsNull(value.ParentContainer, "Property 'ParentContainer' was not set properly.");
            Assert.IsNull(value.ParentContainerID, "Property 'ParentContainerID' was not reported properly.");
            Assert.IsFalse(aParentContainer.ChildContainers.Any(), "Property 'ChildContainers' was not updated properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aParentContainer.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void RootContainerClearChildContainersWhenDeleted()
        {
            EAVModelLibrary.ModelRootContainer aParentContainer = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aParentContainer.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelChildContainer value = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };
            aParentContainer.ChildContainers.Add(value);

            aParentContainer.MarkUnmodified();
            value.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aParentContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");
            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, value.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aParentContainer.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, aParentContainer.ObjectState, "Object state failed to transition to 'Deleted'.");
            Assert.AreEqual(EAV.Model.ObjectState.Deleted, value.ObjectState, "Object state failed to transition to 'Deleted'.");

            aParentContainer.ChildContainers.Clear();
        }
        #endregion

        #region Attributes
        [TestMethod]
        public void RootContainerAddToAttributesWhenNew()
        {
            EAVModelLibrary.ModelRootContainer aContainer = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelAttribute value = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() };
            aContainer.Attributes.Add(value);

            Assert.IsTrue(aContainer.Attributes.Contains(value), "Property 'Attributes' was not updated properly.");
            Assert.AreEqual(aContainer, value.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(aContainer.ContainerID, value.ContainerID, "Property 'ContainerID' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aContainer.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void RootContainerAddToAttributesWhenUnmodified()
        {
            EAVModelLibrary.ModelRootContainer aContainer = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVModelLibrary.ModelAttribute value = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() };
            aContainer.Attributes.Add(value);

            Assert.IsTrue(aContainer.Attributes.Contains(value), "Property 'Attributes' was not updated properly.");
            Assert.AreEqual(aContainer, value.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(aContainer.ContainerID, value.ContainerID, "Property 'ContainerID' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aContainer.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void RootContainerAddToAttributesWhenModified()
        {
            EAVModelLibrary.ModelRootContainer aContainer = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVModelLibrary.ModelAttribute value = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() };
            aContainer.Attributes.Add(value);

            Assert.IsTrue(aContainer.Attributes.Contains(value), "Property 'Attributes' was not updated properly.");
            Assert.AreEqual(aContainer, value.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(aContainer.ContainerID, value.ContainerID, "Property 'ContainerID' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aContainer.ObjectState, "Object state failed to transition to 'Modified'.");

            value = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() };
            aContainer.Attributes.Add(value);

            Assert.IsTrue(aContainer.Attributes.Contains(value), "Property 'Attributes' was not updated properly.");
            Assert.AreEqual(aContainer, value.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(aContainer.ContainerID, value.ContainerID, "Property 'ContainerID' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aContainer.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void RootContainerAddToAttributesWhenDeleted()
        {
            EAVModelLibrary.ModelRootContainer aContainer = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aContainer.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, aContainer.ObjectState, "Object state failed to transition to 'Deleted'.");

            aContainer.Attributes.Add(new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() });
        }

        [TestMethod]
        public void RootContainerRemoveFromAttributesWhenNew()
        {
            EAVModelLibrary.ModelRootContainer aContainer = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelAttribute value = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() };
            aContainer.Attributes.Add(value);
            value = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() };
            aContainer.Attributes.Add(value);

            Assert.IsTrue(aContainer.Attributes.Contains(value), "Property 'Attributes' was not updated properly.");
            Assert.AreEqual(aContainer, value.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(aContainer.ContainerID, value.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aContainer.ObjectState, "Object state should remain 'New' when property set.");

            aContainer.Attributes.Remove(value);

            Assert.IsFalse(aContainer.Attributes.Contains(value), "Property 'Attributes' was not updated properly.");
            Assert.IsNull(value.Container, "Property 'Container' was not set properly.");
            Assert.IsNull(value.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aContainer.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void RootContainerRemoveFromAttributesWhenUnmodified()
        {
            EAVModelLibrary.ModelRootContainer aContainer = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelAttribute value = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() };
            aContainer.Attributes.Add(value);
            value = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() };
            aContainer.Attributes.Add(value);

            Assert.IsTrue(aContainer.Attributes.Contains(value), "Property 'Attributes' was not updated properly.");
            Assert.AreEqual(aContainer, value.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(aContainer.ContainerID, value.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aContainer.ObjectState, "Object state should remain 'New' when property set.");

            aContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aContainer.Attributes.Remove(value);

            Assert.IsFalse(aContainer.Attributes.Contains(value), "Property 'Attributes' was not updated properly.");
            Assert.IsNull(value.Container, "Property 'Container' was not set properly.");
            Assert.IsNull(value.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aContainer.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void RootContainerRemoveFromAttributesWhenModified()
        {
            EAVModelLibrary.ModelRootContainer aContainer = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelAttribute value = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() };
            aContainer.Attributes.Add(value);
            value = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() };
            aContainer.Attributes.Add(value);

            Assert.IsTrue(aContainer.Attributes.Contains(value), "Property 'Attributes' was not updated properly.");
            Assert.AreEqual(aContainer, value.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(aContainer.ContainerID, value.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aContainer.ObjectState, "Object state should remain 'New' when property set.");

            aContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aContainer.Attributes.Remove(value);

            Assert.IsFalse(aContainer.Attributes.Contains(value), "Property 'Attributes' was not updated properly.");
            Assert.IsNull(value.Container, "Property 'Container' was not set properly.");
            Assert.IsNull(value.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aContainer.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void RootContainerRemoveFromAttributesWhenDeleted()
        {
            EAVModelLibrary.ModelRootContainer aContainer = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelAttribute value = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() };
            aContainer.Attributes.Add(value);

            Assert.IsTrue(aContainer.Attributes.Contains(value), "Property 'Attributes' was not updated properly.");
            Assert.AreEqual(aContainer, value.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(aContainer.ContainerID, value.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aContainer.ObjectState, "Object state should remain 'New' when property set.");

            aContainer.MarkUnmodified();
            value.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");
            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, value.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aContainer.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, aContainer.ObjectState, "Object state failed to transition to 'Deleted'.");
            Assert.AreEqual(EAV.Model.ObjectState.Deleted, value.ObjectState, "Object state failed to transition to 'Deleted'.");

            aContainer.Attributes.Remove(value);
        }

        [TestMethod]
        public void RootContainerClearAttributesWhenNew()
        {
            EAVModelLibrary.ModelRootContainer aContainer = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelAttribute value = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() };
            aContainer.Attributes.Add(value);
            value = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() };
            aContainer.Attributes.Add(value);

            Assert.IsTrue(aContainer.Attributes.Contains(value), "Property 'Attributes' was not updated properly.");
            Assert.AreEqual(aContainer, value.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(aContainer.ContainerID, value.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aContainer.ObjectState, "Object state should remain 'New' when property set.");

            aContainer.Attributes.Clear();

            Assert.IsFalse(aContainer.Attributes.Contains(value), "Property 'Attributes' was not updated properly.");
            Assert.IsNull(value.Container, "Property 'Container' was not set properly.");
            Assert.IsNull(value.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.IsFalse(aContainer.Attributes.Any(), "Property 'Attributes' was not updated properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aContainer.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void RootContainerClearAttributesWhenUnmodified()
        {
            EAVModelLibrary.ModelRootContainer aContainer = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelAttribute value = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() };
            aContainer.Attributes.Add(value);
            value = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() };
            aContainer.Attributes.Add(value);

            Assert.IsTrue(aContainer.Attributes.Contains(value), "Property 'Attributes' was not updated properly.");
            Assert.AreEqual(aContainer, value.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(aContainer.ContainerID, value.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aContainer.ObjectState, "Object state should remain 'New' when property set.");

            aContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aContainer.Attributes.Clear();

            Assert.IsFalse(aContainer.Attributes.Contains(value), "Property 'Attributes' was not updated properly.");
            Assert.IsNull(value.Container, "Property 'Container' was not set properly.");
            Assert.IsNull(value.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.IsFalse(aContainer.Attributes.Any(), "Property 'Attributes' was not updated properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aContainer.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void RootContainerClearAttributesWhenModified()
        {
            EAVModelLibrary.ModelRootContainer aContainer = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelAttribute value = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() };
            aContainer.Attributes.Add(value);
            value = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() };
            aContainer.Attributes.Add(value);

            Assert.IsTrue(aContainer.Attributes.Contains(value), "Property 'Attributes' was not updated properly.");
            Assert.AreEqual(aContainer, value.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(aContainer.ContainerID, value.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aContainer.ObjectState, "Object state should remain 'New' when property set.");

            aContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aContainer.Attributes.Clear();

            Assert.IsFalse(aContainer.Attributes.Contains(value), "Property 'Attributes' was not updated properly.");
            Assert.IsNull(value.Container, "Property 'Container' was not set properly.");
            Assert.IsNull(value.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.IsFalse(aContainer.Attributes.Any(), "Property 'Attributes' was not updated properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aContainer.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void RootContainerClearAttributesWhenDeleted()
        {
            EAVModelLibrary.ModelRootContainer aContainer = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelAttribute value = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() };
            aContainer.Attributes.Add(value);

            Assert.IsTrue(aContainer.Attributes.Contains(value), "Property 'Attributes' was not updated properly.");
            Assert.AreEqual(aContainer, value.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(aContainer.ContainerID, value.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aContainer.ObjectState, "Object state should remain 'New' when property set.");

            aContainer.MarkUnmodified();
            value.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");
            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, value.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aContainer.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, aContainer.ObjectState, "Object state failed to transition to 'Deleted'.");
            Assert.AreEqual(EAV.Model.ObjectState.Deleted, value.ObjectState, "Object state failed to transition to 'Deleted'.");

            aContainer.Attributes.Clear();
        }
        #endregion

        #region Instances
        [TestMethod]
        public void RootContainerAddToInstancesWhenNew()
        {
            EAVModelLibrary.ModelRootContainer aContainer = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelRootInstance value = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() };
            aContainer.Instances.Add(value);

            Assert.IsTrue(aContainer.Instances.Contains(value), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(aContainer, value.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(aContainer.ContainerID, value.ContainerID, "Property 'ContainerID' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aContainer.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void RootContainerAddToInstancesWhenUnmodified()
        {
            EAVModelLibrary.ModelRootContainer aContainer = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVModelLibrary.ModelRootInstance value = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() };
            aContainer.Instances.Add(value);

            Assert.IsTrue(aContainer.Instances.Contains(value), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(aContainer, value.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(aContainer.ContainerID, value.ContainerID, "Property 'ContainerID' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aContainer.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void RootContainerAddToInstancesWhenModified()
        {
            EAVModelLibrary.ModelRootContainer aContainer = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVModelLibrary.ModelRootInstance value = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() };
            aContainer.Instances.Add(value);

            Assert.IsTrue(aContainer.Instances.Contains(value), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(aContainer, value.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(aContainer.ContainerID, value.ContainerID, "Property 'ContainerID' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aContainer.ObjectState, "Object state failed to transition to 'Modified'.");

            value = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() };
            aContainer.Instances.Add(value);

            Assert.IsTrue(aContainer.Instances.Contains(value), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(aContainer, value.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(aContainer.ContainerID, value.ContainerID, "Property 'ContainerID' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aContainer.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void RootContainerAddToInstancesWhenDeleted()
        {
            EAVModelLibrary.ModelRootContainer aContainer = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aContainer.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, aContainer.ObjectState, "Object state failed to transition to 'Deleted'.");

            aContainer.Instances.Add(new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() });
        }

        [TestMethod]
        public void RootContainerRemoveFromInstancesWhenNew()
        {
            EAVModelLibrary.ModelRootContainer aContainer = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelRootInstance value = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() };
            aContainer.Instances.Add(value);
            value = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() };
            aContainer.Instances.Add(value);

            Assert.IsTrue(aContainer.Instances.Contains(value), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(aContainer, value.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(aContainer.ContainerID, value.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aContainer.ObjectState, "Object state should remain 'New' when property set.");

            aContainer.Instances.Remove(value);

            Assert.IsFalse(aContainer.Instances.Contains(value), "Property 'Instances' was not updated properly.");
            Assert.IsNull(value.Container, "Property 'Container' was not set properly.");
            Assert.IsNull(value.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aContainer.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void RootContainerRemoveFromInstancesWhenUnmodified()
        {
            EAVModelLibrary.ModelRootContainer aContainer = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelRootInstance value = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() };
            aContainer.Instances.Add(value);
            value = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() };
            aContainer.Instances.Add(value);

            Assert.IsTrue(aContainer.Instances.Contains(value), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(aContainer, value.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(aContainer.ContainerID, value.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aContainer.ObjectState, "Object state should remain 'New' when property set.");

            aContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aContainer.Instances.Remove(value);

            Assert.IsFalse(aContainer.Instances.Contains(value), "Property 'Instances' was not updated properly.");
            Assert.IsNull(value.Container, "Property 'Container' was not set properly.");
            Assert.IsNull(value.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aContainer.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void RootContainerRemoveFromInstancesWhenModified()
        {
            EAVModelLibrary.ModelRootContainer aContainer = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelRootInstance value = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() };
            aContainer.Instances.Add(value);
            value = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() };
            aContainer.Instances.Add(value);

            Assert.IsTrue(aContainer.Instances.Contains(value), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(aContainer, value.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(aContainer.ContainerID, value.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aContainer.ObjectState, "Object state should remain 'New' when property set.");

            aContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aContainer.Instances.Remove(value);

            Assert.IsFalse(aContainer.Instances.Contains(value), "Property 'Instances' was not updated properly.");
            Assert.IsNull(value.Container, "Property 'Container' was not set properly.");
            Assert.IsNull(value.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aContainer.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void RootContainerRemoveFromInstancesWhenDeleted()
        {
            EAVModelLibrary.ModelRootContainer aContainer = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelRootInstance value = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() };
            aContainer.Instances.Add(value);

            Assert.IsTrue(aContainer.Instances.Contains(value), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(aContainer, value.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(aContainer.ContainerID, value.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aContainer.ObjectState, "Object state should remain 'New' when property set.");

            aContainer.MarkUnmodified();
            value.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");
            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, value.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aContainer.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, aContainer.ObjectState, "Object state failed to transition to 'Deleted'.");
            Assert.AreEqual(EAV.Model.ObjectState.Deleted, value.ObjectState, "Object state failed to transition to 'Deleted'.");

            aContainer.Instances.Remove(value);
        }

        [TestMethod]
        public void RootContainerClearInstancesWhenNew()
        {
            EAVModelLibrary.ModelRootContainer aContainer = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelRootInstance value = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() };
            aContainer.Instances.Add(value);
            value = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() };
            aContainer.Instances.Add(value);

            Assert.IsTrue(aContainer.Instances.Contains(value), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(aContainer, value.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(aContainer.ContainerID, value.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aContainer.ObjectState, "Object state should remain 'New' when property set.");

            aContainer.Instances.Clear();

            Assert.IsFalse(aContainer.Instances.Contains(value), "Property 'Instances' was not updated properly.");
            Assert.IsNull(value.Container, "Property 'Container' was not set properly.");
            Assert.IsNull(value.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.IsFalse(aContainer.Instances.Any(), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aContainer.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void RootContainerClearInstancesWhenUnmodified()
        {
            EAVModelLibrary.ModelRootContainer aContainer = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelRootInstance value = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() };
            aContainer.Instances.Add(value);
            value = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() };
            aContainer.Instances.Add(value);

            Assert.IsTrue(aContainer.Instances.Contains(value), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(aContainer, value.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(aContainer.ContainerID, value.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aContainer.ObjectState, "Object state should remain 'New' when property set.");

            aContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aContainer.Instances.Clear();

            Assert.IsFalse(aContainer.Instances.Contains(value), "Property 'Instances' was not updated properly.");
            Assert.IsNull(value.Container, "Property 'Container' was not set properly.");
            Assert.IsNull(value.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.IsFalse(aContainer.Instances.Any(), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aContainer.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void RootContainerClearInstancesWhenModified()
        {
            EAVModelLibrary.ModelRootContainer aContainer = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelRootInstance value = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() };
            aContainer.Instances.Add(value);
            value = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() };
            aContainer.Instances.Add(value);

            Assert.IsTrue(aContainer.Instances.Contains(value), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(aContainer, value.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(aContainer.ContainerID, value.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aContainer.ObjectState, "Object state should remain 'New' when property set.");

            aContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aContainer.Instances.Clear();

            Assert.IsFalse(aContainer.Instances.Contains(value), "Property 'Instances' was not updated properly.");
            Assert.IsNull(value.Container, "Property 'Container' was not set properly.");
            Assert.IsNull(value.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.IsFalse(aContainer.Instances.Any(), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aContainer.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void RootContainerClearInstancesWhenDeleted()
        {
            EAVModelLibrary.ModelRootContainer aContainer = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelRootInstance value = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() };
            aContainer.Instances.Add(value);

            Assert.IsTrue(aContainer.Instances.Contains(value), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(aContainer, value.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(aContainer.ContainerID, value.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aContainer.ObjectState, "Object state should remain 'New' when property set.");

            aContainer.MarkUnmodified();
            value.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");
            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, value.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aContainer.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, aContainer.ObjectState, "Object state failed to transition to 'Deleted'.");
            Assert.AreEqual(EAV.Model.ObjectState.Deleted, value.ObjectState, "Object state failed to transition to 'Deleted'.");

            aContainer.Instances.Clear();
        }
        #endregion
        #endregion
    }
}
