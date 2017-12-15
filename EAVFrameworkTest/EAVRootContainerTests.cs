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
