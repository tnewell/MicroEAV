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
        #region Name
        [TestMethod]
        public void ContextSetNameWhenNew()
        {
            EAVContext aContext = new EAVContext();

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            string value = Guid.NewGuid().ToString();
            aContext.Name = value;

            Assert.AreEqual(value, aContext.Name, "Property 'Name' was not set properly.");
            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ContextSetNameWhenUnmodified()
        {
            EAVContext aContext = new EAVContext() { ContextID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            aContext.Name = value;

            Assert.AreEqual(value, aContext.Name, "Property 'Name' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aContext.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void ContextSetNameWhenModified()
        {
            EAVContext aContext = new EAVContext() { ContextID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            aContext.Name = value;

            Assert.AreEqual(value, aContext.Name, "Property 'Name' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aContext.ObjectState, "Object state failed to transition to 'Modified'.");

            value = Guid.NewGuid().ToString();
            aContext.Name = value;

            Assert.AreEqual(value, aContext.Name, "Property 'Name' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aContext.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ContextSetNameWhenDeleted()
        {
            EAVContext aContext = new EAVContext() { ContextID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aContext.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aContext.ObjectState, "Object state failed to transition to 'Deleted'.");

            aContext.Name = Guid.NewGuid().ToString();
        }
        #endregion

        #region DataName
        [TestMethod]
        public void ContextSetDataNameWhenNew()
        {
            EAVContext aContext = new EAVContext();

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            string value = Guid.NewGuid().ToString();
            aContext.DataName = value;

            Assert.AreEqual(value, aContext.DataName, "Property 'DataName' was not set properly.");
            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ContextSetDataNameWhenUnmodified()
        {
            EAVContext aContext = new EAVContext() { ContextID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            aContext.DataName = value;

            Assert.AreEqual(value, aContext.DataName, "Property 'DataName' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aContext.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void ContextSetDataNameWhenModified()
        {
            EAVContext aContext = new EAVContext() { ContextID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            aContext.DataName = value;

            Assert.AreEqual(value, aContext.DataName, "Property 'DataName' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aContext.ObjectState, "Object state failed to transition to 'Modified'.");

            value = Guid.NewGuid().ToString();
            aContext.DataName = value;

            Assert.AreEqual(value, aContext.DataName, "Property 'DataName' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aContext.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ContextSetDataNameWhenDeleted()
        {
            EAVContext aContext = new EAVContext() { ContextID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aContext.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aContext.ObjectState, "Object state failed to transition to 'Deleted'.");

            aContext.DataName = Guid.NewGuid().ToString();
        }
        #endregion
        
        #region DisplayText
        [TestMethod]
        public void ContextSetDisplayTextWhenNew()
        {
            EAVContext aContext = new EAVContext();

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            string value = Guid.NewGuid().ToString();
            aContext.DisplayText = value;

            Assert.AreEqual(value, aContext.DisplayText, "Property 'DisplayText' was not set properly.");
            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ContextSetDisplayTextWhenUnmodified()
        {
            EAVContext aContext = new EAVContext() { ContextID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            aContext.DisplayText = value;

            Assert.AreEqual(value, aContext.DisplayText, "Property 'DisplayText' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aContext.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void ContextSetDisplayTextWhenModified()
        {
            EAVContext aContext = new EAVContext() { ContextID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            aContext.DisplayText = value;

            Assert.AreEqual(value, aContext.DisplayText, "Property 'DisplayText' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aContext.ObjectState, "Object state failed to transition to 'Modified'.");

            value = Guid.NewGuid().ToString();
            aContext.DisplayText = value;

            Assert.AreEqual(value, aContext.DisplayText, "Property 'DisplayText' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aContext.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ContextSetDisplayTextWhenDeleted()
        {
            EAVContext aContext = new EAVContext() { ContextID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aContext.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aContext.ObjectState, "Object state failed to transition to 'Deleted'.");

            aContext.DisplayText = Guid.NewGuid().ToString();
        }
        #endregion
        #endregion

        #region Object Properties
        // N/A
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
