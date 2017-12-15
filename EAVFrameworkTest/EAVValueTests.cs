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
        public void ValueCreate()
        {
            EAVValue aValue = new EAVValue();

            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            Assert.IsNull(aValue.RawValue, "Property 'RawValue' should be null on creation.");
            Assert.IsNull(aValue.Units, "Property 'Units' should be null on creation.");

            Assert.IsNull(aValue.Instance, "Property 'Instance' should be null on creation.");
            Assert.IsNull(aValue.Attribute, "Property 'Attribute' should be null on creation.");
        }

        #region State Transitions
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ValueStateTransitionNewToUnmodifiedWithNullID()
        {
            EAVValue aValue = new EAVValue();

            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            aValue.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aValue.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ValueStateTransitionNewToUnmodifiedWithNewID()
        {
            EAVValue aValue = new EAVValue() { Instance = new EAVRootInstance() { InstanceID = rng.Next() }, Attribute = new EAVAttribute() { AttributeID = rng.Next() } };

            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            aValue.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aValue.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        public void ValueStateTransitionNewToUnmodifiedWithValidID()
        {
            EAVValue aValue = new EAVValue() { Instance = new EAVRootInstance() { InstanceID = rng.Next() }, Attribute = new EAVAttribute() { AttributeID = rng.Next() } };

            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            aValue.Instance.MarkUnmodified();
            aValue.Attribute.MarkUnmodified();

            aValue.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aValue.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ValueStateTransitionNewToDeleted()
        {
            EAVValue aValue = new EAVValue();

            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            aValue.MarkDeleted();
        }

        [TestMethod]
        public void ValueStateTransitionUnmodifiedToDeleted()
        {
            EAVValue aValue = new EAVValue() { Instance = new EAVRootInstance() { InstanceID = rng.Next() }, Attribute = new EAVAttribute() { AttributeID = rng.Next() } };

            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            aValue.Instance.MarkUnmodified();
            aValue.Attribute.MarkUnmodified();

            aValue.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aValue.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aValue.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aValue.ObjectState, "Object state failed to transition to 'Deleted'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ValueStateTransitionDeletedToUnmodified()
        {
            EAVValue aValue = new EAVValue() { Instance = new EAVRootInstance() { InstanceID = rng.Next() }, Attribute = new EAVAttribute() { AttributeID = rng.Next() } };

            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            aValue.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aValue.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aValue.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aValue.ObjectState, "Object state failed to transition to 'Deleted'.");

            aValue.MarkUnmodified();
        }
        #endregion

        #region ID Property
        // N/A
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
        // N/A
        #endregion
    }
}
