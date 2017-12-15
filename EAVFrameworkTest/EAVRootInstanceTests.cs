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
        public void RootInstanceCreate()
        {
            EAVRootInstance aRootInstance = new EAVRootInstance();

            Assert.AreEqual(ObjectState.New, aRootInstance.ObjectState, "Object state should be 'New' on creation.");

            Assert.IsNull(aRootInstance.InstanceID, "Property 'InstanceID' should be null on creation.");

            Assert.IsNull(aRootInstance.Subject, "Property 'Subject' should be null on creation.");
            Assert.IsNull(aRootInstance.Container, "Property 'Container' should be null on creation.");
            Assert.IsNull(aRootInstance.ParentInstance, "Property 'ParentInstance' should be null on creation.");

            Assert.IsNotNull(aRootInstance.ChildInstances, "Property 'ChildInstances' should not be null on creation.");
            Assert.IsFalse(aRootInstance.ChildInstances.Any(), "Property 'ChildInstances' should be empty on creation.");

            Assert.IsNotNull(aRootInstance.Values, "Property 'Values' should not be null on creation.");
            Assert.IsFalse(aRootInstance.Values.Any(), "Property 'Values' should be empty on creation.");
        }

        #region State Transitions
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RootInstanceStateTransitionNewToUnmodifiedWithNullID()
        {
            EAVRootInstance aRootInstance = new EAVRootInstance();

            Assert.AreEqual(ObjectState.New, aRootInstance.ObjectState, "Object state should be 'New' on creation.");

            aRootInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aRootInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        public void RootInstanceStateTransitionNewToUnmodifiedWithValidID()
        {
            EAVRootInstance aRootInstance = new EAVRootInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aRootInstance.ObjectState, "Object state should be 'New' on creation.");

            aRootInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aRootInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RootInstanceStateTransitionNewToDeleted()
        {
            EAVRootInstance aRootInstance = new EAVRootInstance();

            Assert.AreEqual(ObjectState.New, aRootInstance.ObjectState, "Object state should be 'New' on creation.");

            aRootInstance.MarkDeleted();
        }

        [TestMethod]
        public void RootInstanceStateTransitionUnmodifiedToDeleted()
        {
            EAVRootInstance aRootInstance = new EAVRootInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aRootInstance.ObjectState, "Object state should be 'New' on creation.");

            aRootInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aRootInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aRootInstance.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aRootInstance.ObjectState, "Object state failed to transition to 'Deleted'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RootInstanceStateTransitionDeletedToUnmodified()
        {
            EAVRootInstance aRootInstance = new EAVRootInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aRootInstance.ObjectState, "Object state should be 'New' on creation.");

            aRootInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aRootInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aRootInstance.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aRootInstance.ObjectState, "Object state failed to transition to 'Deleted'.");

            aRootInstance.MarkUnmodified();
        }
        #endregion

        #region ID Property
        [TestMethod]
        public void RootInstanceSetIDWhenNew()
        {
            EAVRootInstance aRootInstance = new EAVRootInstance();

            Assert.AreEqual(ObjectState.New, aRootInstance.ObjectState, "Object state should be 'New' on creation.");

            int id = rng.Next();
            aRootInstance.InstanceID = id;

            Assert.AreEqual(id, aRootInstance.InstanceID, "Property 'InstanceID' not properly set.");
            Assert.AreEqual(ObjectState.New, aRootInstance.ObjectState, "Object state after setting property 'InstanceID' should be 'New'.");
        }

        [TestMethod]
        public void RootInstanceSetIDBeforeUnmodified()
        {
            EAVRootInstance aRootInstance = new EAVRootInstance();

            Assert.AreEqual(ObjectState.New, aRootInstance.ObjectState, "Object state should be 'New' on creation.");

            int id = rng.Next();
            aRootInstance.InstanceID = id;

            Assert.AreEqual(id, aRootInstance.InstanceID, "Property 'InstanceID' not properly set.");
            Assert.AreEqual(ObjectState.New, aRootInstance.ObjectState, "Object state after setting property 'InstanceID' should be 'New'.");

            aRootInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aRootInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RootInstanceSetIDAfterUnmodified()
        {
            EAVRootInstance aRootInstance = new EAVRootInstance();

            Assert.AreEqual(ObjectState.New, aRootInstance.ObjectState, "Object state should be 'New' on creation.");

            aRootInstance.MarkUnmodified();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RootInstanceSetIDWhenDeleted()
        {
            EAVRootInstance aRootInstance = new EAVRootInstance();

            Assert.AreEqual(ObjectState.New, aRootInstance.ObjectState, "Object state should be 'New' on creation.");

            int id = rng.Next();
            aRootInstance.InstanceID = id;

            Assert.AreEqual(id, aRootInstance.InstanceID, "Property 'InstanceID' not properly set.");
            Assert.AreEqual(ObjectState.New, aRootInstance.ObjectState, "Object state after setting property 'InstanceID' should be 'New'.");

            aRootInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aRootInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aRootInstance.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aRootInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            id = rng.Next();
            aRootInstance.InstanceID = id;
        }
        #endregion

        #region Primitive Properties
        // N/A
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
