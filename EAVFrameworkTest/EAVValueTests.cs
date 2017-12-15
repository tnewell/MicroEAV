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
        #region RawValue
        [TestMethod]
        public void ValueSetRawValueWhenNew()
        {
            EAVValue aValue = new EAVValue();

            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            string value = Guid.NewGuid().ToString();
            aValue.RawValue = value;

            Assert.AreEqual(value, aValue.RawValue, "Property 'RawValue' was not set properly.");
            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ValueSetRawValueWhenUnmodified()
        {
            EAVValue aValue = new EAVValue() { Instance = new EAVRootInstance() { InstanceID = rng.Next() }, Attribute = new EAVAttribute() { AttributeID = rng.Next() } };

            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            aValue.Instance.MarkUnmodified();
            aValue.Attribute.MarkUnmodified();

            aValue.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aValue.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            aValue.RawValue = value;

            Assert.AreEqual(value, aValue.RawValue, "Property 'RawValue' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aValue.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void ValueSetRawValueWhenModified()
        {
            EAVValue aValue = new EAVValue() { Instance = new EAVRootInstance() { InstanceID = rng.Next() }, Attribute = new EAVAttribute() { AttributeID = rng.Next() } };

            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            aValue.Instance.MarkUnmodified();
            aValue.Attribute.MarkUnmodified();

            aValue.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aValue.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            aValue.RawValue = value;

            Assert.AreEqual(value, aValue.RawValue, "Property 'RawValue' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aValue.ObjectState, "Object state failed to transition to 'Modified'.");

            value = Guid.NewGuid().ToString();
            aValue.RawValue = value;

            Assert.AreEqual(value, aValue.RawValue, "Property 'RawValue' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aValue.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ValueSetRawValueWhenDeleted()
        {
            EAVValue aValue = new EAVValue() { Instance = new EAVRootInstance() { InstanceID = rng.Next() }, Attribute = new EAVAttribute() { AttributeID = rng.Next() } };

            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            aValue.Instance.MarkUnmodified();
            aValue.Attribute.MarkUnmodified();

            aValue.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aValue.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aValue.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aValue.ObjectState, "Object state failed to transition to 'Deleted'.");

            aValue.RawValue = Guid.NewGuid().ToString();
        }
        #endregion

        #region Units
        [TestMethod]
        public void ValueSetUnitsWhenNew()
        {
            EAVValue aValue = new EAVValue();

            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            string value = Guid.NewGuid().ToString();
            aValue.Units = value;

            Assert.AreEqual(value, aValue.Units, "Property 'Units' was not set properly.");
            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ValueSetUnitsWhenUnmodified()
        {
            EAVValue aValue = new EAVValue() { Instance = new EAVRootInstance() { InstanceID = rng.Next() }, Attribute = new EAVAttribute() { AttributeID = rng.Next() } };

            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            aValue.Instance.MarkUnmodified();
            aValue.Attribute.MarkUnmodified();

            aValue.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aValue.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            aValue.Units = value;

            Assert.AreEqual(value, aValue.Units, "Property 'Units' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aValue.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void ValueSetUnitsWhenModified()
        {
            EAVValue aValue = new EAVValue() { Instance = new EAVRootInstance() { InstanceID = rng.Next() }, Attribute = new EAVAttribute() { AttributeID = rng.Next() } };

            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            aValue.Instance.MarkUnmodified();
            aValue.Attribute.MarkUnmodified();

            aValue.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aValue.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            aValue.Units = value;

            Assert.AreEqual(value, aValue.Units, "Property 'Units' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aValue.ObjectState, "Object state failed to transition to 'Modified'.");

            value = Guid.NewGuid().ToString();
            aValue.Units = value;

            Assert.AreEqual(value, aValue.Units, "Property 'Units' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aValue.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ValueSetUnitsWhenDeleted()
        {
            EAVValue aValue = new EAVValue() { Instance = new EAVRootInstance() { InstanceID = rng.Next() }, Attribute = new EAVAttribute() { AttributeID = rng.Next() } };

            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            aValue.Instance.MarkUnmodified();
            aValue.Attribute.MarkUnmodified();

            aValue.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aValue.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aValue.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aValue.ObjectState, "Object state failed to transition to 'Deleted'.");

            aValue.Units = Guid.NewGuid().ToString();
        }
        #endregion
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
