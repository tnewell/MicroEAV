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
        //    Set When New
        //    Set When Unmodified
        //        Set When Modified
        //    Set When Deleted
        #endregion

        #region Object Properties
        // Include associated ID property
        //    Set When New
        //    Set When Unmodified
        //        Set When Modified
        //    Set When Deleted
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
