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
        public void AttributeCreate()
        {
            EAVAttribute anAttribute = new EAVAttribute();

            Assert.AreEqual(ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            Assert.IsNull(anAttribute.AttributeID, "Property 'AttributeID' should be null on creation.");

            Assert.IsNull(anAttribute.Name, "Property 'Name' should be null on creation.");
            Assert.IsNull(anAttribute.DataName, "Property 'DataName' should be null on creation.");
            Assert.IsNull(anAttribute.DisplayText, "Property 'DisplayText' should be null on creation.");
            Assert.AreEqual(default(EAVDataType), anAttribute.DataType, "Property 'DataType' should be default on creation.");
            Assert.IsFalse(anAttribute.IsKey, "Property 'IsKey' should be false on creation.");

            Assert.IsNull(anAttribute.Container, "Property 'Container' should be null on creation.");

            Assert.IsNotNull(anAttribute.Values, "Property 'Values' should not be null on creation.");
            Assert.IsFalse(anAttribute.Values.Any(), "Property 'Values' should be empty on creation.");
        }

        #region State Transitions
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AttributeStateTransitionNewToUnmodifiedWithNullID()
        {
            EAVAttribute anAttribute = new EAVAttribute();

            Assert.AreEqual(ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        public void AttributeStateTransitionNewToUnmodifiedWithValidID()
        {
            EAVAttribute anAttribute = new EAVAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AttributeStateTransitionNewToDeleted()
        {
            EAVAttribute anAttribute = new EAVAttribute();

            Assert.AreEqual(ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.MarkDeleted();
        }

        [TestMethod]
        public void AttributeStateTransitionUnmodifiedToDeleted()
        {
            EAVAttribute anAttribute = new EAVAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anAttribute.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, anAttribute.ObjectState, "Object state failed to transition to 'Deleted'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AttributeStateTransitionDeletedToUnmodified()
        {
            EAVAttribute anAttribute = new EAVAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anAttribute.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, anAttribute.ObjectState, "Object state failed to transition to 'Deleted'.");

            anAttribute.MarkUnmodified();
        }
        #endregion

        #region ID Property
        [TestMethod]
        public void AttributeSetIDWhenNew()
        {
            EAVAttribute anAttribute = new EAVAttribute();

            Assert.AreEqual(ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            int id = rng.Next();
            anAttribute.AttributeID = id;

            Assert.AreEqual(id, anAttribute.AttributeID, "Property 'AttributeID' not properly set.");
            Assert.AreEqual(ObjectState.New, anAttribute.ObjectState, "Object state after setting property 'AttributeID' should be 'New'.");
        }

        [TestMethod]
        public void AttributeSetIDBeforeUnmodified()
        {
            EAVAttribute anAttribute = new EAVAttribute();

            Assert.AreEqual(ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            int id = rng.Next();
            anAttribute.AttributeID = id;

            Assert.AreEqual(id, anAttribute.AttributeID, "Property 'AttributeID' not properly set.");
            Assert.AreEqual(ObjectState.New, anAttribute.ObjectState, "Object state after setting property 'AttributeID' should be 'New'.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AttributeSetIDAfterUnmodified()
        {
            EAVAttribute anAttribute = new EAVAttribute();

            Assert.AreEqual(ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.MarkUnmodified();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AttributeSetIDWhenDeleted()
        {
            EAVAttribute anAttribute = new EAVAttribute();

            Assert.AreEqual(ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            int id = rng.Next();
            anAttribute.AttributeID = id;

            Assert.AreEqual(id, anAttribute.AttributeID, "Property 'AttributeID' not properly set.");
            Assert.AreEqual(ObjectState.New, anAttribute.ObjectState, "Object state after setting property 'AttributeID' should be 'New'.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anAttribute.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            id = rng.Next();
            anAttribute.AttributeID = id;
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
