using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using EAVFramework.Model;
using System.Linq;

namespace EAVFrameworkTest
{
    public partial class EAVFrameworkTestHarness
    {
        [TestMethod]
        public void ContextCreate()
        {
            EAVContext aContext = new EAVContext();

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            Assert.IsNull(aContext.ContextID, "Property 'ContextID' should be null on creation.");

            Assert.IsNull(aContext.Name, "Property 'Name' should be null on creation.");
            Assert.IsNull(aContext.DataName, "Property 'DataName' should be null on creation.");
            Assert.IsNull(aContext.DisplayText, "Property 'DisplayText' should be null on creation.");
        }

        #region State Transitions
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ContextStateTransitionNewToUnmodifiedWithNullID()
        {
            EAVContext aContext = new EAVContext();

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        public void ContextStateTransitionNewToUnmodifiedWithValidID()
        {
            EAVContext aContext = new EAVContext() { ContextID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ContextStateTransitionNewToDeleted()
        {
            EAVContext aContext = new EAVContext();

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            aContext.MarkDeleted();
        }

        [TestMethod]
        public void ContextStateTransitionUnmodifiedToDeleted()
        {
            EAVContext aContext = new EAVContext() { ContextID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aContext.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aContext.ObjectState, "Object state failed to transition to 'Deleted'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ContextStateTransitionDeletedToUnmodified()
        {
            EAVContext aContext = new EAVContext() { ContextID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aContext.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aContext.ObjectState, "Object state failed to transition to 'Deleted'.");

            aContext.MarkUnmodified();
        }
        #endregion

        #region ID Property
        [TestMethod]
        public void ContextSetIDWhenNew()
        {
            EAVContext aContext = new EAVContext();

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            int id = rng.Next();
            aContext.ContextID = id;

            Assert.AreEqual(id, aContext.ContextID, "Property 'ContextID' not properly set.");
            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state after setting property 'ContextID' should be 'New'.");
        }

        [TestMethod]
        public void ContextSetIDBeforeUnmodified()
        {
            EAVContext aContext = new EAVContext();

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            int id = rng.Next();
            aContext.ContextID = id;

            Assert.AreEqual(id, aContext.ContextID, "Property 'ContextID' not properly set.");
            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state after setting property 'ContextID' should be 'New'.");

            aContext.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ContextSetIDAfterUnmodified()
        {
            EAVContext aContext = new EAVContext();

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            aContext.MarkUnmodified();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ContextSetIDWhenDeleted()
        {
            EAVContext aContext = new EAVContext();

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            int id = rng.Next();
            aContext.ContextID = id;

            Assert.AreEqual(id, aContext.ContextID, "Property 'ContextID' not properly set.");
            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state after setting property 'ContextID' should be 'New'.");

            aContext.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aContext.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");

            id = rng.Next();
            aContext.ContextID = id;
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
