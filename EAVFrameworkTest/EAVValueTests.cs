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
            Assert.IsNull(aValue.Unit, "Property 'Units' should be null on creation.");

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

        #endregion

        #region Object Properties
        #region Instance
        [TestMethod]
        public void ValueSetInstanceWhenNew()
        {
            EAVValue aValue = new EAVValue();

            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            EAVChildInstance value = new EAVChildInstance() { InstanceID = rng.Next() };
            aValue.Instance = value;

            Assert.AreEqual(value, aValue.Instance, "Property 'Instance' was not set properly.");
            Assert.AreEqual(value.InstanceID, aValue.InstanceID, "Property 'InstanceID' was not reported properly.");
            Assert.IsTrue(value.Values.Contains(aValue), "Property 'Values' was not updated properly.");
            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ValueSetInstanceWhenUnmodified()
        {
            EAVValue aValue = new EAVValue() { Attribute = new EAVAttribute() { AttributeID = rng.Next() }, Instance = new EAVChildInstance() { InstanceID = rng.Next() } };

            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            aValue.Attribute.MarkUnmodified();
            aValue.Instance.MarkUnmodified();

            aValue.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aValue.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVChildInstance instance = new EAVChildInstance() { InstanceID = rng.Next() };
            aValue.Instance = instance;

            Assert.AreEqual(instance, aValue.Instance, "Property 'Instance' was not set properly.");
            Assert.AreEqual(instance.InstanceID, aValue.InstanceID, "Property 'InstanceID' was not reported properly.");
            Assert.IsTrue(instance.Values.Contains(aValue), "Property 'Values' was not updated properly.");
            Assert.AreEqual(ObjectState.Modified, aValue.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ValueSetInstanceWhenUnmodifiedWithNullAttribute()
        {
            EAVValue aValue = new EAVValue() { Instance = new EAVChildInstance() { InstanceID = rng.Next() } };

            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            aValue.Instance.MarkUnmodified();

            aValue.MarkUnmodified();
        }

        [TestMethod]
        public void ValueSetInstanceWhenModified()
        {
            EAVValue aValue = new EAVValue() { Attribute = new EAVAttribute() { AttributeID = rng.Next() }, Instance = new EAVChildInstance() { InstanceID = rng.Next() } };

            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            aValue.Attribute.MarkUnmodified();
            aValue.Instance.MarkUnmodified();
            aValue.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aValue.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVChildInstance instance = new EAVChildInstance() { InstanceID = rng.Next() };
            aValue.Instance = instance;

            Assert.AreEqual(instance, aValue.Instance, "Property 'Instance' was not set properly.");
            Assert.AreEqual(instance.InstanceID, aValue.InstanceID, "Property 'InstanceID' was not reported properly.");
            Assert.IsTrue(instance.Values.Contains(aValue), "Property 'Values' was not updated properly.");
            Assert.AreEqual(ObjectState.Modified, aValue.ObjectState, "Object state failed to transition to 'Modified'.");

            instance = new EAVChildInstance() { InstanceID = rng.Next() };
            aValue.Instance = instance;

            Assert.AreEqual(instance, aValue.Instance, "Property 'Instance' was not set properly.");
            Assert.AreEqual(instance.InstanceID, aValue.InstanceID, "Property 'InstanceID' was not reported properly.");
            Assert.IsTrue(instance.Values.Contains(aValue), "Property 'Values' was not updated properly.");
            Assert.AreEqual(ObjectState.Modified, aValue.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ValueSetInstanceWhenDeleted()
        {
            EAVValue aValue = new EAVValue();

            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            aValue.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aValue.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aValue.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aValue.ObjectState, "Object state failed to transition to 'Deleted'.");

            aValue.Instance = new EAVChildInstance() { InstanceID = rng.Next() };
        }
        #endregion

        #region Attribute
        [TestMethod]
        public void ValueSetAttributeWhenNew()
        {
            EAVValue aValue = new EAVValue();

            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            EAVAttribute attribute = new EAVAttribute() { AttributeID = rng.Next() };
            aValue.Attribute = attribute;

            Assert.AreEqual(attribute, aValue.Attribute, "Property 'Attribute' was not set properly.");
            Assert.AreEqual(attribute.AttributeID, aValue.AttributeID, "Property 'AttributeID' was not reported properly.");
            Assert.IsTrue(attribute.Values.Contains(aValue), "Property 'Values' was not updated properly.");
            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ValueSetAttributeWhenUnmodified()
        {
            EAVValue aValue = new EAVValue() { Attribute = new EAVAttribute() { AttributeID = rng.Next() }, Instance = new EAVChildInstance() { InstanceID = rng.Next() } };

            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            aValue.Attribute.MarkUnmodified();
            aValue.Instance.MarkUnmodified();

            aValue.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aValue.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVAttribute attribute = new EAVAttribute() { AttributeID = rng.Next() };
            aValue.Attribute = attribute;

            Assert.AreEqual(attribute, aValue.Attribute, "Property 'Attribute' was not set properly.");
            Assert.AreEqual(attribute.AttributeID, aValue.AttributeID, "Property 'AttributeID' was not reported properly.");
            Assert.IsTrue(attribute.Values.Contains(aValue), "Property 'Values' was not updated properly.");
            Assert.AreEqual(ObjectState.Modified, aValue.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ValueSetAttributeWhenUnmodifiedWithNullInstance()
        {
            EAVValue aValue = new EAVValue() { Attribute = new EAVAttribute() { AttributeID = rng.Next() } };

            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            aValue.Attribute.MarkUnmodified();

            aValue.MarkUnmodified();
        }

        [TestMethod]
        public void ValueSetAttributeWhenModified()
        {
            EAVValue aValue = new EAVValue() { Attribute = new EAVAttribute() { AttributeID = rng.Next() }, Instance = new EAVChildInstance() { InstanceID = rng.Next() } };

            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            aValue.Attribute.MarkUnmodified();
            aValue.Instance.MarkUnmodified();

            aValue.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aValue.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVAttribute attribute = new EAVAttribute() { AttributeID = rng.Next() };
            aValue.Attribute = attribute;

            Assert.AreEqual(attribute, aValue.Attribute, "Property 'Attribute' was not set properly.");
            Assert.AreEqual(attribute.AttributeID, aValue.AttributeID, "Property 'AttributeID' was not reported properly.");
            Assert.IsTrue(attribute.Values.Contains(aValue), "Property 'Values' was not updated properly.");
            Assert.AreEqual(ObjectState.Modified, aValue.ObjectState, "Object state failed to transition to 'Modified'.");

            attribute = new EAVAttribute() { AttributeID = rng.Next() };
            aValue.Attribute = attribute;

            Assert.AreEqual(attribute, aValue.Attribute, "Property 'Attribute' was not set properly.");
            Assert.AreEqual(attribute.AttributeID, aValue.AttributeID, "Property 'AttributeID' was not reported properly.");
            Assert.IsTrue(attribute.Values.Contains(aValue), "Property 'Values' was not updated properly.");
            Assert.AreEqual(ObjectState.Modified, aValue.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ValueSetAttributeWhenDeleted()
        {
            EAVValue aValue = new EAVValue();

            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            aValue.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aValue.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aValue.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aValue.ObjectState, "Object state failed to transition to 'Deleted'.");

            aValue.Attribute = new EAVAttribute() { AttributeID = rng.Next() };
        }
        #endregion

        #region Unit
        [TestMethod]
        public void ValueSetUnitWhenNewWithNewUnit()
        {
            EAVValue aValue = new EAVValue();

            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            EAVUnit aUnit = new EAVUnit() { UnitID = rng.Next() };
            aValue.Unit = aUnit;

            Assert.AreEqual(aUnit, aValue.Unit, "Property 'Units' was not set properly.");
            Assert.AreEqual(aUnit.UnitID, aValue.UnitID, "Property 'UnitID' was not reported properly.");
            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should remain 'New' when property set.");
            Assert.AreEqual(ObjectState.New, aUnit.ObjectState, String.Format("Object state for Unit object incorrectly transitioned to '{0}' when 'Unit' property set.", aUnit.ObjectState));
        }

        [TestMethod]
        public void ValueSetUnitWhenNewWithUnmodifiedUnit()
        {
            EAVValue aValue = new EAVValue();

            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            EAVUnit aUnit = new EAVUnit() { UnitID = rng.Next() };

            aUnit.MarkUnmodified();
            Assert.AreEqual(ObjectState.Unmodified, aUnit.ObjectState, "Object state for 'Unit' object failed to transition to 'Unmodified'.");

            aValue.Unit = aUnit;

            Assert.AreEqual(aUnit, aValue.Unit, "Property 'Unit' was not set properly.");
            Assert.AreEqual(aUnit.UnitID, aValue.UnitID, "Property 'UnitID' was not reported properly.");
            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should remain 'New' when property set.");
            Assert.AreEqual(ObjectState.Unmodified, aUnit.ObjectState, String.Format("Object state for Unit object incorrectly transitioned to '{0}' when 'Unit' property set.", aUnit.ObjectState));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ValueSetUnitWhenNewWithDeletedUnit()
        {
            EAVValue aValue = new EAVValue();

            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            EAVUnit aUnit = new EAVUnit() { UnitID = rng.Next() };

            aUnit.MarkUnmodified();
            Assert.AreEqual(ObjectState.Unmodified, aUnit.ObjectState, "Object state for 'Unit' object failed to transition to 'Unmodified'.");

            aUnit.MarkDeleted();
            Assert.AreEqual(ObjectState.Deleted, aUnit.ObjectState, "Object state for 'Unit' object failed to transition to 'Deleted'.");

            aValue.Unit = aUnit;
        }

        [TestMethod]
        public void ValueSetUnitWhenUnmodifiedVariableUnitsIsTrue()
        {
            EAVValue aValue = new EAVValue() { Instance = new EAVRootInstance() { InstanceID = rng.Next() }, Attribute = new EAVAttribute() { AttributeID = rng.Next(), VariableUnits = true } };

            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            aValue.Instance.MarkUnmodified();
            aValue.Attribute.MarkUnmodified();

            aValue.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aValue.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVUnit aUnit = new EAVUnit() { UnitID = rng.Next() };
            aValue.Unit = aUnit;

            Assert.AreEqual(aUnit, aValue.Unit, "Property 'Units' was not set properly.");
            Assert.AreEqual(aUnit.UnitID, aValue.UnitID, "Property 'UnitID' was not reported properly.");
            Assert.AreEqual(ObjectState.Modified, aValue.ObjectState, "Object state failed to transition to 'Modified'.");
            Assert.AreEqual(ObjectState.New, aUnit.ObjectState, String.Format("Object state for Unit object incorrectly transitioned to '{0}' when 'Unit' property set.", aUnit.ObjectState));
        }

        [TestMethod]
        public void ValueSetUnitWhenUnmodifiedVariableUnitsIsFalse()
        {
            EAVValue aValue = new EAVValue() { Instance = new EAVRootInstance() { InstanceID = rng.Next() }, Attribute = new EAVAttribute() { AttributeID = rng.Next(), VariableUnits = false } };

            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            aValue.Instance.MarkUnmodified();
            aValue.Attribute.MarkUnmodified();

            aValue.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aValue.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVUnit aUnit = new EAVUnit() { UnitID = rng.Next() };
            aValue.Unit = aUnit;

            Assert.AreEqual(aUnit, aValue.Unit, "Property 'Units' was not set properly.");
            Assert.AreEqual(aUnit.UnitID, aValue.UnitID, "Property 'UnitID' was not reported properly.");
            Assert.AreEqual(ObjectState.Modified, aValue.ObjectState, "Object state failed to transition to 'Modified'.");
            Assert.AreEqual(ObjectState.New, aUnit.ObjectState, String.Format("Object state for Unit object incorrectly transitioned to '{0}' when 'Unit' property set.", aUnit.ObjectState));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ValueSetUnitWhenUnmodifiedVariableUnitsIsNull()
        {
            EAVValue aValue = new EAVValue() { Instance = new EAVRootInstance() { InstanceID = rng.Next() }, Attribute = new EAVAttribute() { AttributeID = rng.Next(), VariableUnits = null } };

            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            aValue.Instance.MarkUnmodified();
            aValue.Attribute.MarkUnmodified();

            aValue.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aValue.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aValue.Unit = new EAVUnit() { UnitID = rng.Next() };
        }

        [TestMethod]
        public void ValueSetUnitWhenModified()
        {
            EAVValue aValue = new EAVValue() { Instance = new EAVRootInstance() { InstanceID = rng.Next() }, Attribute = new EAVAttribute() { AttributeID = rng.Next(), VariableUnits = true } };

            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            aValue.Instance.MarkUnmodified();
            aValue.Attribute.MarkUnmodified();

            aValue.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aValue.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVUnit aUnit = new EAVUnit() { UnitID = rng.Next() };
            aValue.Unit = aUnit;

            Assert.AreEqual(aUnit, aValue.Unit, "Property 'Units' was not set properly.");
            Assert.AreEqual(aUnit.UnitID, aValue.UnitID, "Property 'UnitID' was not reported properly.");
            Assert.AreEqual(ObjectState.Modified, aValue.ObjectState, "Object state failed to transition to 'Modified'.");
            Assert.AreEqual(ObjectState.New, aUnit.ObjectState, String.Format("Object state for Unit object incorrectly transitioned to '{0}' when 'Unit' property set.", aUnit.ObjectState));

            aUnit = new EAVUnit() { UnitID = rng.Next() };
            aValue.Unit = aUnit;

            Assert.AreEqual(aUnit, aValue.Unit, "Property 'Units' was not set properly.");
            Assert.AreEqual(aUnit.UnitID, aValue.UnitID, "Property 'UnitID' was not reported properly.");
            Assert.AreEqual(ObjectState.Modified, aValue.ObjectState, "Object state should remain 'Modified' when property set.");
            Assert.AreEqual(ObjectState.New, aUnit.ObjectState, String.Format("Object state for Unit object incorrectly transitioned to '{0}' when 'Unit' property set.", aUnit.ObjectState));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ValueSetUnitWhenDeleted()
        {
            EAVValue aValue = new EAVValue() { Instance = new EAVRootInstance() { InstanceID = rng.Next() }, Attribute = new EAVAttribute() { AttributeID = rng.Next() } };

            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            aValue.Instance.MarkUnmodified();
            aValue.Attribute.MarkUnmodified();

            aValue.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aValue.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aValue.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aValue.ObjectState, "Object state failed to transition to 'Deleted'.");

            aValue.Unit = new EAVUnit() { UnitID = rng.Next() };
        }

        [TestMethod]
        public void ValueSetUnitIDWhenNew()
        {
            EAVValue aValue = new EAVValue();

            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            int unitID = rng.Next();
            aValue.UnitID = unitID;

            Assert.IsNull(aValue.Unit, "Property 'Unit' was not reported properly.");
            Assert.AreEqual(unitID, aValue.UnitID, "Property 'UnitID' was not set properly.");
            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ValueSetUnitIDWhenUnmodified()
        {
            EAVValue aValue = new EAVValue() { Instance = new EAVRootInstance() { InstanceID = rng.Next() }, Attribute = new EAVAttribute() { AttributeID = rng.Next(), VariableUnits = true } };

            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            aValue.Instance.MarkUnmodified();
            aValue.Attribute.MarkUnmodified();

            aValue.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aValue.ObjectState, "Object state failed to transition to 'Unmodified'.");

            int unitID = rng.Next();
            aValue.UnitID = unitID;

            Assert.IsNull(aValue.Unit, "Property 'Unit' was not reported properly.");
            Assert.AreEqual(unitID, aValue.UnitID, "Property 'UnitID' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aValue.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void ValueSetUnitIDWhenModified()
        {
            EAVValue aValue = new EAVValue() { Instance = new EAVRootInstance() { InstanceID = rng.Next() }, Attribute = new EAVAttribute() { AttributeID = rng.Next(), VariableUnits = true } };

            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            aValue.Instance.MarkUnmodified();
            aValue.Attribute.MarkUnmodified();

            aValue.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aValue.ObjectState, "Object state failed to transition to 'Unmodified'.");

            int unitID = rng.Next();
            aValue.UnitID = unitID;

            Assert.IsNull(aValue.Unit, "Property 'Unit' was not reported properly.");
            Assert.AreEqual(unitID, aValue.UnitID, "Property 'UnitID' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aValue.ObjectState, "Object state failed to transition to 'Modified'.");

            unitID = rng.Next();
            aValue.UnitID = unitID;

            Assert.IsNull(aValue.Unit, "Property 'Unit' was not reported properly.");
            Assert.AreEqual(unitID, aValue.UnitID, "Property 'UnitID' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aValue.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ValueSetUnitIDWhenDeleted()
        {
            EAVValue aValue = new EAVValue() { Instance = new EAVRootInstance() { InstanceID = rng.Next() }, Attribute = new EAVAttribute() { AttributeID = rng.Next() } };

            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            aValue.Instance.MarkUnmodified();
            aValue.Attribute.MarkUnmodified();

            aValue.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aValue.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aValue.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aValue.ObjectState, "Object state failed to transition to 'Deleted'.");

            aValue.UnitID = rng.Next();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ValueSetUnitWhenNewWithNewUnitThenUnitID()
        {
            EAVValue aValue = new EAVValue();

            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            EAVUnit aUnit = new EAVUnit() { UnitID = rng.Next() };
            aValue.Unit = aUnit;

            Assert.AreEqual(aUnit, aValue.Unit, "Property 'Units' was not set properly.");
            Assert.AreEqual(aUnit.UnitID, aValue.UnitID, "Property 'UnitID' was not reported properly.");
            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should remain 'New' when property set.");
            Assert.AreEqual(ObjectState.New, aUnit.ObjectState, String.Format("Object state for Unit object incorrectly transitioned to '{0}' when 'Unit' property set.", aUnit.ObjectState));

            aValue.UnitID = rng.Next();
        }

        [TestMethod]
        public void ValueSetUnitIDWhenNewWithNewUnit()
        {
            EAVValue aValue = new EAVValue();

            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            int unitID = rng.Next();
            aValue.UnitID = unitID;

            Assert.IsNull(aValue.Unit, "Property 'Unit' was not reported properly.");
            Assert.AreEqual(unitID, aValue.UnitID, "Property 'UnitID' was not set properly.");
            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should remain 'New' when property set.");

            EAVUnit aUnit = new EAVUnit() { UnitID = rng.Next() };
            aValue.Unit = aUnit;

            if (aUnit.UnitID == unitID)
                Assert.Inconclusive("The same value was selected for unit ID.");

            Assert.AreEqual(aUnit, aValue.Unit, "Property 'Units' was not set properly.");
            Assert.AreEqual(aUnit.UnitID, aValue.UnitID, "Property 'UnitID' was not reported properly.");
            Assert.AreNotEqual(unitID, aValue.UnitID, "Property 'UnitID' was not set properly.");
            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should remain 'New' when property set.");
            Assert.AreEqual(ObjectState.New, aUnit.ObjectState, String.Format("Object state for Unit object incorrectly transitioned to '{0}' when 'Unit' property set.", aUnit.ObjectState));
        }
        #endregion
        #endregion

        #region Collection Properties
        // N/A
        #endregion
    }
}
