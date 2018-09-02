using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace EAVModelTest
{
    public partial class EAVModelTestHarness
    {
        [TestMethod]
        public void ChildContainerCreate()
        {
            EAVModelLibrary.ModelChildContainer aChildContainer = new EAVModelLibrary.ModelChildContainer();

            Assert.AreEqual(EAV.Model.ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            Assert.IsNull(aChildContainer.ContainerID, "Property 'ContainerID' should be null on creation.");

            Assert.IsNull(aChildContainer.Name, "Property 'Name' should be null on creation.");
            Assert.IsNull(aChildContainer.DataName, "Property 'DataName' should be null on creation.");
            Assert.IsNull(aChildContainer.DisplayText, "Property 'DisplayText' should be null on creation.");
            Assert.AreEqual(0, aChildContainer.Sequence, "Property 'Sequence' should be zero on creation.");
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
            EAVModelLibrary.ModelChildContainer aChildContainer = new EAVModelLibrary.ModelChildContainer();

            Assert.AreEqual(EAV.Model.ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            aChildContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aChildContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        public void ChildContainerStateTransitionNewToUnmodifiedWithValidID()
        {
            EAVModelLibrary.ModelChildContainer aChildContainer = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            aChildContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aChildContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChildContainerStateTransitionNewToDeleted()
        {
            EAVModelLibrary.ModelChildContainer aChildContainer = new EAVModelLibrary.ModelChildContainer();

            Assert.AreEqual(EAV.Model.ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            aChildContainer.MarkDeleted();
        }

        [TestMethod]
        public void ChildContainerStateTransitionUnmodifiedToDeleted()
        {
            EAVModelLibrary.ModelChildContainer aChildContainer = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            aChildContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aChildContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aChildContainer.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, aChildContainer.ObjectState, "Object state failed to transition to 'Deleted'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChildContainerStateTransitionDeletedToUnmodified()
        {
            EAVModelLibrary.ModelChildContainer aChildContainer = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            aChildContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aChildContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aChildContainer.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, aChildContainer.ObjectState, "Object state failed to transition to 'Deleted'.");

            aChildContainer.MarkUnmodified();
        }
        #endregion

        #region ID Property
        [TestMethod]
        public void ChildContainerSetIDWhenNew()
        {
            EAVModelLibrary.ModelChildContainer aChildContainer = new EAVModelLibrary.ModelChildContainer();

            Assert.AreEqual(EAV.Model.ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            int id = rng.Next();
            aChildContainer.ContainerID = id;

            Assert.AreEqual(id, aChildContainer.ContainerID, "Property 'ContainerID' not properly set.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aChildContainer.ObjectState, "Object state after setting property 'ContainerID' should be 'New'.");
        }

        [TestMethod]
        public void ChildContainerSetIDBeforeUnmodified()
        {
            EAVModelLibrary.ModelChildContainer aChildContainer = new EAVModelLibrary.ModelChildContainer();

            Assert.AreEqual(EAV.Model.ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            int id = rng.Next();
            aChildContainer.ContainerID = id;

            Assert.AreEqual(id, aChildContainer.ContainerID, "Property 'ContainerID' not properly set.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aChildContainer.ObjectState, "Object state after setting property 'ContainerID' should be 'New'.");

            aChildContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aChildContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChildContainerSetIDAfterUnmodified()
        {
            EAVModelLibrary.ModelChildContainer aChildContainer = new EAVModelLibrary.ModelChildContainer();

            Assert.AreEqual(EAV.Model.ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            aChildContainer.MarkUnmodified();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChildContainerSetIDWhenDeleted()
        {
            EAVModelLibrary.ModelChildContainer aChildContainer = new EAVModelLibrary.ModelChildContainer();

            Assert.AreEqual(EAV.Model.ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            int id = rng.Next();
            aChildContainer.ContainerID = id;

            Assert.AreEqual(id, aChildContainer.ContainerID, "Property 'ContainerID' not properly set.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aChildContainer.ObjectState, "Object state after setting property 'ContainerID' should be 'New'.");

            aChildContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aChildContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aChildContainer.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, aChildContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            id = rng.Next();
            aChildContainer.ContainerID = id;
        }
        #endregion

        #region Primitive Properties
        #region Name
        [TestMethod]
        public void ChildContainerSetNameWhenNew()
        {
            EAVModelLibrary.ModelChildContainer aChildContainer = new EAVModelLibrary.ModelChildContainer();

            Assert.AreEqual(EAV.Model.ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            string value = Guid.NewGuid().ToString();
            aChildContainer.Name = value;

            Assert.AreEqual(value, aChildContainer.Name, "Property 'Name' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aChildContainer.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ChildContainerSetNameWhenUnmodified()
        {
            EAVModelLibrary.ModelChildContainer aChildContainer = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            aChildContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aChildContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            aChildContainer.Name = value;

            Assert.AreEqual(value, aChildContainer.Name, "Property 'Name' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aChildContainer.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void ChildContainerSetNameWhenModified()
        {
            EAVModelLibrary.ModelChildContainer aChildContainer = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            aChildContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aChildContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            aChildContainer.Name = value;

            Assert.AreEqual(value, aChildContainer.Name, "Property 'Name' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aChildContainer.ObjectState, "Object state failed to transition to 'Modified'.");

            value = Guid.NewGuid().ToString();
            aChildContainer.Name = value;

            Assert.AreEqual(value, aChildContainer.Name, "Property 'Name' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aChildContainer.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChildContainerSetNameWhenDeleted()
        {
            EAVModelLibrary.ModelChildContainer aChildContainer = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            aChildContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aChildContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aChildContainer.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, aChildContainer.ObjectState, "Object state failed to transition to 'Deleted'.");

            aChildContainer.Name = Guid.NewGuid().ToString();
        }
        #endregion

        #region DataName
        [TestMethod]
        public void ChildContainerSetDataNameWhenNew()
        {
            EAVModelLibrary.ModelChildContainer aChildContainer = new EAVModelLibrary.ModelChildContainer();

            Assert.AreEqual(EAV.Model.ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            string value = Guid.NewGuid().ToString();
            aChildContainer.DataName = value;

            Assert.AreEqual(value, aChildContainer.DataName, "Property 'DataName' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aChildContainer.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ChildContainerSetDataNameWhenUnmodified()
        {
            EAVModelLibrary.ModelChildContainer aChildContainer = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            aChildContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aChildContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            aChildContainer.DataName = value;

            Assert.AreEqual(value, aChildContainer.DataName, "Property 'DataName' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aChildContainer.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void ChildContainerSetDataNameWhenModified()
        {
            EAVModelLibrary.ModelChildContainer aChildContainer = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            aChildContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aChildContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            aChildContainer.DataName = value;

            Assert.AreEqual(value, aChildContainer.DataName, "Property 'DataName' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aChildContainer.ObjectState, "Object state failed to transition to 'Modified'.");

            value = Guid.NewGuid().ToString();
            aChildContainer.DataName = value;

            Assert.AreEqual(value, aChildContainer.DataName, "Property 'DataName' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aChildContainer.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChildContainerSetDataNameWhenDeleted()
        {
            EAVModelLibrary.ModelChildContainer aChildContainer = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            aChildContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aChildContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aChildContainer.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, aChildContainer.ObjectState, "Object state failed to transition to 'Deleted'.");

            aChildContainer.DataName = Guid.NewGuid().ToString();
        }
        #endregion

        #region DisplayText
        [TestMethod]
        public void ChildContainerSetDisplayTextWhenNew()
        {
            EAVModelLibrary.ModelChildContainer aChildContainer = new EAVModelLibrary.ModelChildContainer();

            Assert.AreEqual(EAV.Model.ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            string value = Guid.NewGuid().ToString();
            aChildContainer.DisplayText = value;

            Assert.AreEqual(value, aChildContainer.DisplayText, "Property 'DisplayText' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aChildContainer.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ChildContainerSetDisplayTextWhenUnmodified()
        {
            EAVModelLibrary.ModelChildContainer aChildContainer = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            aChildContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aChildContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            aChildContainer.DisplayText = value;

            Assert.AreEqual(value, aChildContainer.DisplayText, "Property 'DisplayText' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aChildContainer.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void ChildContainerSetDisplayTextWhenModified()
        {
            EAVModelLibrary.ModelChildContainer aChildContainer = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            aChildContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aChildContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            aChildContainer.DisplayText = value;

            Assert.AreEqual(value, aChildContainer.DisplayText, "Property 'DisplayText' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aChildContainer.ObjectState, "Object state failed to transition to 'Modified'.");

            value = Guid.NewGuid().ToString();
            aChildContainer.DisplayText = value;

            Assert.AreEqual(value, aChildContainer.DisplayText, "Property 'DisplayText' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aChildContainer.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChildContainerSetDisplayTextWhenDeleted()
        {
            EAVModelLibrary.ModelChildContainer aChildContainer = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            aChildContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aChildContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aChildContainer.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, aChildContainer.ObjectState, "Object state failed to transition to 'Deleted'.");

            aChildContainer.DisplayText = Guid.NewGuid().ToString();
        }
        #endregion

        #region Sequence
        [TestMethod]
        public void ChildContainerSetSequenceWhenNew()
        {
            EAVModelLibrary.ModelChildContainer anChildContainer = new EAVModelLibrary.ModelChildContainer();

            Assert.AreEqual(EAV.Model.ObjectState.New, anChildContainer.ObjectState, "Object state should be 'New' on creation.");

            int value = rng.Next();
            anChildContainer.Sequence = value;

            Assert.AreEqual(value, anChildContainer.Sequence, "Property 'Sequence' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, anChildContainer.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ChildContainerSetSequenceWhenUnmodified()
        {
            EAVModelLibrary.ModelChildContainer anChildContainer = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, anChildContainer.ObjectState, "Object state should be 'New' on creation.");

            anChildContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, anChildContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            int value = rng.Next();
            anChildContainer.Sequence = value;

            Assert.AreEqual(value, anChildContainer.Sequence, "Property 'Sequence' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, anChildContainer.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void ChildContainerSetSequenceWhenModified()
        {
            EAVModelLibrary.ModelChildContainer anChildContainer = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, anChildContainer.ObjectState, "Object state should be 'New' on creation.");

            anChildContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, anChildContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            int value = rng.Next();
            anChildContainer.Sequence = value;

            Assert.AreEqual(value, anChildContainer.Sequence, "Property 'Sequence' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, anChildContainer.ObjectState, "Object state failed to transition to 'Modified'.");

            value = rng.Next();
            anChildContainer.Sequence = value;

            Assert.AreEqual(value, anChildContainer.Sequence, "Property 'Sequence' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, anChildContainer.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChildContainerSetSequenceWhenDeleted()
        {
            EAVModelLibrary.ModelChildContainer anChildContainer = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, anChildContainer.ObjectState, "Object state should be 'New' on creation.");

            anChildContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, anChildContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anChildContainer.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, anChildContainer.ObjectState, "Object state failed to transition to 'Deleted'.");

            anChildContainer.Sequence = rng.Next();
        }
        #endregion

        #region IsRepeating
        [TestMethod]
        public void ChildContainerSetIsRepeatingWhenNew()
        {
            EAVModelLibrary.ModelChildContainer aChildContainer = new EAVModelLibrary.ModelChildContainer();

            Assert.AreEqual(EAV.Model.ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            aChildContainer.IsRepeating = true;

            Assert.AreEqual(true, aChildContainer.IsRepeating, "Property 'IsRepeating' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aChildContainer.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ChildContainerSetIsRepeatingWhenUnmodified()
        {
            EAVModelLibrary.ModelChildContainer aChildContainer = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            aChildContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aChildContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aChildContainer.IsRepeating = true;

            Assert.AreEqual(true, aChildContainer.IsRepeating, "Property 'IsRepeating' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aChildContainer.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void ChildContainerSetIsRepeatingWhenModified()
        {
            EAVModelLibrary.ModelChildContainer aChildContainer = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            aChildContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aChildContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aChildContainer.IsRepeating = true;

            Assert.AreEqual(true, aChildContainer.IsRepeating, "Property 'IsRepeating' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aChildContainer.ObjectState, "Object state failed to transition to 'Modified'.");

            aChildContainer.IsRepeating = false;

            Assert.AreEqual(false, aChildContainer.IsRepeating, "Property 'IsRepeating' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aChildContainer.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChildContainerSetIsRepeatingWhenDeleted()
        {
            EAVModelLibrary.ModelChildContainer aChildContainer = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            aChildContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aChildContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aChildContainer.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, aChildContainer.ObjectState, "Object state failed to transition to 'Deleted'.");

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
            EAVModelLibrary.ModelChildContainer aChildContainer = new EAVModelLibrary.ModelChildContainer();

            Assert.AreEqual(EAV.Model.ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            aChildContainer.Context = new EAVModelLibrary.ModelContext() { ContextID = rng.Next() };
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChildContainerSetContextWhenUnmodified()
        {
            EAVModelLibrary.ModelChildContainer aChildContainer = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            aChildContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aChildContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aChildContainer.Context = new EAVModelLibrary.ModelContext() { ContextID = rng.Next() };
        }

        // Set When Modified - N/A

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChildContainerSetContextWhenDeleted()
        {
            EAVModelLibrary.ModelChildContainer aChildContainer = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            aChildContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aChildContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aChildContainer.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, aChildContainer.ObjectState, "Object state failed to transition to 'Deleted'.");

            aChildContainer.Context = new EAVModelLibrary.ModelContext() { ContextID = rng.Next() };
        }
        #endregion

        #region ParentContainer
        [TestMethod]
        public void ChildContainerSetParentContainerWithObjectWhenNew()
        {
            EAVModelLibrary.ModelChildContainer aChildContainer = new EAVModelLibrary.ModelChildContainer();

            Assert.AreEqual(EAV.Model.ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelRootContainer aContainer = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };
            aChildContainer.ParentContainer = aContainer;

            Assert.AreEqual(aContainer, aChildContainer.ParentContainer, "Property 'ParentContainer' was not set properly.");
            Assert.AreEqual(aContainer.ContainerID, aChildContainer.ParentContainerID, "Property 'ParentContainerID' was not reported properly.");
            Assert.IsTrue(aContainer.ChildContainers.Contains(aChildContainer), "Property 'ChildContainers' was not updated properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aChildContainer.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ChildContainerSetParentContainerWithObjectWhenUnmodified()
        {
            EAVModelLibrary.ModelChildContainer aChildContainer = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            aChildContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aChildContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVModelLibrary.ModelRootContainer aContainer = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };
            aChildContainer.ParentContainer = aContainer;

            Assert.AreEqual(aContainer, aChildContainer.ParentContainer, "Property 'ParentContainer' was not set properly.");
            Assert.AreEqual(aContainer.ContainerID, aChildContainer.ParentContainerID, "Property 'ParentContainerID' was not reported properly.");
            Assert.IsTrue(aContainer.ChildContainers.Contains(aChildContainer), "Property 'ChildContainers' was not updated properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aChildContainer.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void ChildContainerSetParentContainerWithObjectWhenModified()
        {
            EAVModelLibrary.ModelChildContainer aChildContainer = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            aChildContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aChildContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVModelLibrary.ModelRootContainer aContainer = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };
            aChildContainer.ParentContainer = aContainer;

            Assert.AreEqual(aContainer, aChildContainer.ParentContainer, "Property 'ParentContainer' was not set properly.");
            Assert.AreEqual(aContainer.ContainerID, aChildContainer.ParentContainerID, "Property 'ParentContainerID' was not reported properly.");
            Assert.IsTrue(aContainer.ChildContainers.Contains(aChildContainer), "Property 'ChildContainers' was not updated properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aChildContainer.ObjectState, "Object state failed to transition to 'Modified'.");

            aContainer = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };
            aChildContainer.ParentContainer = aContainer;

            Assert.AreEqual(aContainer, aChildContainer.ParentContainer, "Property 'ParentContainer' was not set properly.");
            Assert.AreEqual(aContainer.ContainerID, aChildContainer.ParentContainerID, "Property 'ParentContainerID' was not reported properly.");
            Assert.IsTrue(aContainer.ChildContainers.Contains(aChildContainer), "Property 'ChildContainers' was not updated properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aChildContainer.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChildContainerSetParentContainerWithObjectWhenDeleted()
        {
            EAVModelLibrary.ModelChildContainer aChildContainer = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            aChildContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aChildContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aChildContainer.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, aChildContainer.ObjectState, "Object state failed to transition to 'Deleted'.");

            aChildContainer.ParentContainer = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };
        }
        #endregion
        #endregion

        #region Collection Properties
        #region ChildContainers
        [TestMethod]
        public void ChildContainerAddToChildContainersWhenNew()
        {
            EAVModelLibrary.ModelChildContainer aParentContainer = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aParentContainer.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelChildContainer value = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };
            aParentContainer.ChildContainers.Add(value);

            Assert.IsTrue(aParentContainer.ChildContainers.Contains(value), "Property 'ChildContainers' was not updated properly.");
            Assert.AreEqual(aParentContainer, value.ParentContainer, "Property 'ParentContainer' was not set properly.");
            Assert.AreEqual(aParentContainer.ContainerID, value.ParentContainerID, "Property 'ParentContainerID' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aParentContainer.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ChildContainerAddToChildContainersWhenUnmodified()
        {
            EAVModelLibrary.ModelChildContainer aParentContainer = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };

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
        public void ChildContainerAddToChildContainersWhenModified()
        {
            EAVModelLibrary.ModelChildContainer aParentContainer = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };

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
        public void ChildContainerAddToChildContainersWhenDeleted()
        {
            EAVModelLibrary.ModelChildContainer aParentContainer = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aParentContainer.ObjectState, "Object state should be 'New' on creation.");

            aParentContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aParentContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aParentContainer.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, aParentContainer.ObjectState, "Object state failed to transition to 'Deleted'.");

            aParentContainer.ChildContainers.Add(new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() });
        }

        [TestMethod]
        public void ChildContainerRemoveFromChildContainersWhenNew()
        {
            EAVModelLibrary.ModelChildContainer aParentContainer = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };

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
        public void ChildContainerRemoveFromChildContainersWhenUnmodified()
        {
            EAVModelLibrary.ModelChildContainer aParentContainer = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };

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
        public void ChildContainerRemoveFromChildContainersWhenModified()
        {
            EAVModelLibrary.ModelChildContainer aParentContainer = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };

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
        public void ChildContainerRemoveFromChildContainersWhenDeleted()
        {
            EAVModelLibrary.ModelChildContainer aParentContainer = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };

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
        public void ChildContainerClearChildContainersWhenNew()
        {
            EAVModelLibrary.ModelChildContainer aParentContainer = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };

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
        public void ChildContainerClearChildContainersWhenUnmodified()
        {
            EAVModelLibrary.ModelChildContainer aParentContainer = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };

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
        public void ChildContainerClearChildContainersWhenModified()
        {
            EAVModelLibrary.ModelChildContainer aParentContainer = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };

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
        public void ChildContainerClearChildContainersWhenDeleted()
        {
            EAVModelLibrary.ModelChildContainer aParentContainer = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };

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
        public void ChildContainerAddToAttributesWhenNew()
        {
            EAVModelLibrary.ModelChildContainer aContainer = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelAttribute value = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() };
            aContainer.Attributes.Add(value);

            Assert.IsTrue(aContainer.Attributes.Contains(value), "Property 'Attributes' was not updated properly.");
            Assert.AreEqual(aContainer, value.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(aContainer.ContainerID, value.ContainerID, "Property 'ContainerID' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aContainer.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ChildContainerAddToAttributesWhenUnmodified()
        {
            EAVModelLibrary.ModelChildContainer aContainer = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };

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
        public void ChildContainerAddToAttributesWhenModified()
        {
            EAVModelLibrary.ModelChildContainer aContainer = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };

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
        public void ChildContainerAddToAttributesWhenDeleted()
        {
            EAVModelLibrary.ModelChildContainer aContainer = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aContainer.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, aContainer.ObjectState, "Object state failed to transition to 'Deleted'.");

            aContainer.Attributes.Add(new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() });
        }

        [TestMethod]
        public void ChildContainerRemoveFromAttributesWhenNew()
        {
            EAVModelLibrary.ModelChildContainer aContainer = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };

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
        public void ChildContainerRemoveFromAttributesWhenUnmodified()
        {
            EAVModelLibrary.ModelChildContainer aContainer = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };

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
        public void ChildContainerRemoveFromAttributesWhenModified()
        {
            EAVModelLibrary.ModelChildContainer aContainer = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };

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
        public void ChildContainerRemoveFromAttributesWhenDeleted()
        {
            EAVModelLibrary.ModelChildContainer aContainer = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };

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
        public void ChildContainerClearAttributesWhenNew()
        {
            EAVModelLibrary.ModelChildContainer aContainer = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };

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
        public void ChildContainerClearAttributesWhenUnmodified()
        {
            EAVModelLibrary.ModelChildContainer aContainer = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };

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
        public void ChildContainerClearAttributesWhenModified()
        {
            EAVModelLibrary.ModelChildContainer aContainer = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };

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
        public void ChildContainerClearAttributesWhenDeleted()
        {
            EAVModelLibrary.ModelChildContainer aContainer = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };

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
        public void ChildContainerAddToInstancesWhenNew()
        {
            EAVModelLibrary.ModelChildContainer aContainer = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelChildInstance value = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };
            aContainer.Instances.Add(value);

            Assert.IsTrue(aContainer.Instances.Contains(value), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(aContainer, value.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(aContainer.ContainerID, value.ContainerID, "Property 'ContainerID' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aContainer.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ChildContainerAddToInstancesWhenUnmodified()
        {
            EAVModelLibrary.ModelChildContainer aContainer = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVModelLibrary.ModelChildInstance value = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };
            aContainer.Instances.Add(value);

            Assert.IsTrue(aContainer.Instances.Contains(value), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(aContainer, value.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(aContainer.ContainerID, value.ContainerID, "Property 'ContainerID' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aContainer.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void ChildContainerAddToInstancesWhenModified()
        {
            EAVModelLibrary.ModelChildContainer aContainer = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVModelLibrary.ModelChildInstance value = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };
            aContainer.Instances.Add(value);

            Assert.IsTrue(aContainer.Instances.Contains(value), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(aContainer, value.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(aContainer.ContainerID, value.ContainerID, "Property 'ContainerID' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aContainer.ObjectState, "Object state failed to transition to 'Modified'.");

            value = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };
            aContainer.Instances.Add(value);

            Assert.IsTrue(aContainer.Instances.Contains(value), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(aContainer, value.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(aContainer.ContainerID, value.ContainerID, "Property 'ContainerID' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aContainer.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ChildContainerAddToInstancesWhenDeleted()
        {
            EAVModelLibrary.ModelChildContainer aContainer = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            aContainer.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aContainer.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, aContainer.ObjectState, "Object state failed to transition to 'Deleted'.");

            aContainer.Instances.Add(new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() });
        }

        [TestMethod]
        public void ChildContainerRemoveFromInstancesWhenNew()
        {
            EAVModelLibrary.ModelChildContainer aContainer = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelChildInstance value = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };
            aContainer.Instances.Add(value);
            value = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };
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
        public void ChildContainerRemoveFromInstancesWhenUnmodified()
        {
            EAVModelLibrary.ModelChildContainer aContainer = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelChildInstance value = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };
            aContainer.Instances.Add(value);
            value = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };
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
        public void ChildContainerRemoveFromInstancesWhenModified()
        {
            EAVModelLibrary.ModelChildContainer aContainer = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelChildInstance value = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };
            aContainer.Instances.Add(value);
            value = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };
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
        public void ChildContainerRemoveFromInstancesWhenDeleted()
        {
            EAVModelLibrary.ModelChildContainer aContainer = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelChildInstance value = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };
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
        public void ChildContainerClearInstancesWhenNew()
        {
            EAVModelLibrary.ModelChildContainer aContainer = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelChildInstance value = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };
            aContainer.Instances.Add(value);
            value = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };
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
        public void ChildContainerClearInstancesWhenUnmodified()
        {
            EAVModelLibrary.ModelChildContainer aContainer = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelChildInstance value = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };
            aContainer.Instances.Add(value);
            value = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };
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
        public void ChildContainerClearInstancesWhenModified()
        {
            EAVModelLibrary.ModelChildContainer aContainer = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelChildInstance value = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };
            aContainer.Instances.Add(value);
            value = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };
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
        public void ChildContainerClearInstancesWhenDeleted()
        {
            EAVModelLibrary.ModelChildContainer aContainer = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aContainer.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelChildInstance value = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };
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
