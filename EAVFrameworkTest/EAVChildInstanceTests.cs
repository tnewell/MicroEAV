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
        public void ChildInstanceCreate()
        {
            EAVChildInstance aChildInstance = new EAVChildInstance();

            Assert.AreEqual(ObjectState.New, aChildInstance.ObjectState, "Object state should be 'New' on creation.");

            Assert.IsNull(aChildInstance.InstanceID, "Property 'InstanceID' should be null on creation.");

            Assert.IsNull(aChildInstance.Subject, "Property 'Subject' should be null on creation.");
            Assert.IsNull(aChildInstance.Container, "Property 'Container' should be null on creation.");
            Assert.IsNull(aChildInstance.ParentInstance, "Property 'ParentInstance' should be null on creation.");

            Assert.IsNotNull(aChildInstance.ChildInstances, "Property 'ChildInstances' should not be null on creation.");
            Assert.IsFalse(aChildInstance.ChildInstances.Any(), "Property 'ChildInstances' should be empty on creation.");

            Assert.IsNotNull(aChildInstance.Values, "Property 'Values' should not be null on creation.");
            Assert.IsFalse(aChildInstance.Values.Any(), "Property 'Values' should be empty on creation.");
        }

        #region State Transitions
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChildInstanceStateTransitionNewToUnmodifiedWithNullID()
        {
            EAVChildInstance aChildInstance = new EAVChildInstance();

            Assert.AreEqual(ObjectState.New, aChildInstance.ObjectState, "Object state should be 'New' on creation.");

            aChildInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aChildInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        public void ChildInstanceStateTransitionNewToUnmodifiedWithValidID()
        {
            EAVChildInstance aChildInstance = new EAVChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aChildInstance.ObjectState, "Object state should be 'New' on creation.");

            aChildInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aChildInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChildInstanceStateTransitionNewToDeleted()
        {
            EAVChildInstance aChildInstance = new EAVChildInstance();

            Assert.AreEqual(ObjectState.New, aChildInstance.ObjectState, "Object state should be 'New' on creation.");

            aChildInstance.MarkDeleted();
        }

        [TestMethod]
        public void ChildInstanceStateTransitionUnmodifiedToDeleted()
        {
            EAVChildInstance aChildInstance = new EAVChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aChildInstance.ObjectState, "Object state should be 'New' on creation.");

            aChildInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aChildInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aChildInstance.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aChildInstance.ObjectState, "Object state failed to transition to 'Deleted'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChildInstanceStateTransitionDeletedToUnmodified()
        {
            EAVChildInstance aChildInstance = new EAVChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aChildInstance.ObjectState, "Object state should be 'New' on creation.");

            aChildInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aChildInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aChildInstance.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aChildInstance.ObjectState, "Object state failed to transition to 'Deleted'.");

            aChildInstance.MarkUnmodified();
        }
        #endregion

        #region ID Property
        [TestMethod]
        public void ChildInstanceSetIDWhenNew()
        {
            EAVChildInstance aChildInstance = new EAVChildInstance();

            Assert.AreEqual(ObjectState.New, aChildInstance.ObjectState, "Object state should be 'New' on creation.");

            int id = rng.Next();
            aChildInstance.InstanceID = id;

            Assert.AreEqual(id, aChildInstance.InstanceID, "Property 'InstanceID' not properly set.");
            Assert.AreEqual(ObjectState.New, aChildInstance.ObjectState, "Object state after setting property 'InstanceID' should be 'New'.");
        }

        [TestMethod]
        public void ChildInstanceSetIDBeforeUnmodified()
        {
            EAVChildInstance aChildInstance = new EAVChildInstance();

            Assert.AreEqual(ObjectState.New, aChildInstance.ObjectState, "Object state should be 'New' on creation.");

            int id = rng.Next();
            aChildInstance.InstanceID = id;

            Assert.AreEqual(id, aChildInstance.InstanceID, "Property 'InstanceID' not properly set.");
            Assert.AreEqual(ObjectState.New, aChildInstance.ObjectState, "Object state after setting property 'InstanceID' should be 'New'.");

            aChildInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aChildInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChildInstanceSetIDAfterUnmodified()
        {
            EAVChildInstance aChildInstance = new EAVChildInstance();

            Assert.AreEqual(ObjectState.New, aChildInstance.ObjectState, "Object state should be 'New' on creation.");

            aChildInstance.MarkUnmodified();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChildInstanceSetIDWhenDeleted()
        {
            EAVChildInstance aChildInstance = new EAVChildInstance();

            Assert.AreEqual(ObjectState.New, aChildInstance.ObjectState, "Object state should be 'New' on creation.");

            int id = rng.Next();
            aChildInstance.InstanceID = id;

            Assert.AreEqual(id, aChildInstance.InstanceID, "Property 'InstanceID' not properly set.");
            Assert.AreEqual(ObjectState.New, aChildInstance.ObjectState, "Object state after setting property 'InstanceID' should be 'New'.");

            aChildInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aChildInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aChildInstance.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aChildInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            id = rng.Next();
            aChildInstance.InstanceID = id;
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
