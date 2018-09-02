using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using EAV.Model;


namespace EAVModelTest
{
    public partial class EAVModelTestHarness
    {
        [TestMethod]
        public void ValueCreate()
        {
            ModelValue aValue = new ModelValue();

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
            ModelValue aValue = new ModelValue();

            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            aValue.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aValue.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ValueStateTransitionNewToUnmodifiedWithNewID()
        {
            ModelValue aValue = new ModelValue() { Instance = new ModelRootInstance() { InstanceID = rng.Next() }, Attribute = new ModelAttribute() { AttributeID = rng.Next() } };

            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            aValue.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aValue.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        public void ValueStateTransitionNewToUnmodifiedWithValidID()
        {
            ModelValue aValue = new ModelValue() { Instance = new ModelRootInstance() { InstanceID = rng.Next() }, Attribute = new ModelAttribute() { AttributeID = rng.Next() } };

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
            ModelValue aValue = new ModelValue();

            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            aValue.MarkDeleted();
        }

        [TestMethod]
        public void ValueStateTransitionUnmodifiedToDeleted()
        {
            ModelValue aValue = new ModelValue() { Instance = new ModelRootInstance() { InstanceID = rng.Next() }, Attribute = new ModelAttribute() { AttributeID = rng.Next() } };

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
            ModelValue aValue = new ModelValue() { Instance = new ModelRootInstance() { InstanceID = rng.Next() }, Attribute = new ModelAttribute() { AttributeID = rng.Next() } };

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
            ModelValue aValue = new ModelValue();

            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            string value = Guid.NewGuid().ToString();
            aValue.RawValue = value;

            Assert.AreEqual(value, aValue.RawValue, "Property 'RawValue' was not set properly.");
            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ValueSetRawValueWhenUnmodified()
        {
            ModelValue aValue = new ModelValue() { Instance = new ModelRootInstance() { InstanceID = rng.Next() }, Attribute = new ModelAttribute() { AttributeID = rng.Next() } };

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
            ModelValue aValue = new ModelValue() { Instance = new ModelRootInstance() { InstanceID = rng.Next() }, Attribute = new ModelAttribute() { AttributeID = rng.Next() } };

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
            ModelValue aValue = new ModelValue() { Instance = new ModelRootInstance() { InstanceID = rng.Next() }, Attribute = new ModelAttribute() { AttributeID = rng.Next() } };

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
            ModelValue aValue = new ModelValue();

            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            ModelChildInstance value = new ModelChildInstance() { InstanceID = rng.Next() };
            aValue.Instance = value;

            Assert.AreEqual(value, aValue.Instance, "Property 'Instance' was not set properly.");
            Assert.AreEqual(value.InstanceID, aValue.InstanceID, "Property 'InstanceID' was not reported properly.");
            Assert.IsTrue(value.Values.Contains(aValue), "Property 'Values' was not updated properly.");
            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ValueSetInstanceWhenUnmodified()
        {
            ModelValue aValue = new ModelValue() { Attribute = new ModelAttribute() { AttributeID = rng.Next() }, Instance = new ModelChildInstance() { InstanceID = rng.Next() } };

            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            aValue.Attribute.MarkUnmodified();
            aValue.Instance.MarkUnmodified();

            aValue.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aValue.ObjectState, "Object state failed to transition to 'Unmodified'.");

            ModelChildInstance instance = new ModelChildInstance() { InstanceID = rng.Next() };
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
            ModelValue aValue = new ModelValue() { Instance = new ModelChildInstance() { InstanceID = rng.Next() } };

            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            aValue.Instance.MarkUnmodified();

            aValue.MarkUnmodified();
        }

        [TestMethod]
        public void ValueSetInstanceWhenModified()
        {
            ModelValue aValue = new ModelValue() { Attribute = new ModelAttribute() { AttributeID = rng.Next() }, Instance = new ModelChildInstance() { InstanceID = rng.Next() } };

            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            aValue.Attribute.MarkUnmodified();
            aValue.Instance.MarkUnmodified();
            aValue.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aValue.ObjectState, "Object state failed to transition to 'Unmodified'.");

            ModelChildInstance instance = new ModelChildInstance() { InstanceID = rng.Next() };
            aValue.Instance = instance;

            Assert.AreEqual(instance, aValue.Instance, "Property 'Instance' was not set properly.");
            Assert.AreEqual(instance.InstanceID, aValue.InstanceID, "Property 'InstanceID' was not reported properly.");
            Assert.IsTrue(instance.Values.Contains(aValue), "Property 'Values' was not updated properly.");
            Assert.AreEqual(ObjectState.Modified, aValue.ObjectState, "Object state failed to transition to 'Modified'.");

            instance = new ModelChildInstance() { InstanceID = rng.Next() };
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
            ModelValue aValue = new ModelValue();

            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            aValue.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aValue.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aValue.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aValue.ObjectState, "Object state failed to transition to 'Deleted'.");

            aValue.Instance = new ModelChildInstance() { InstanceID = rng.Next() };
        }
        #endregion

        #region Attribute
        [TestMethod]
        public void ValueSetAttributeWhenNew()
        {
            ModelValue aValue = new ModelValue();

            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            ModelAttribute attribute = new ModelAttribute() { AttributeID = rng.Next() };
            aValue.Attribute = attribute;

            Assert.AreEqual(attribute, aValue.Attribute, "Property 'Attribute' was not set properly.");
            Assert.AreEqual(attribute.AttributeID, aValue.AttributeID, "Property 'AttributeID' was not reported properly.");
            Assert.IsTrue(attribute.Values.Contains(aValue), "Property 'Values' was not updated properly.");
            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ValueSetAttributeWhenUnmodified()
        {
            ModelValue aValue = new ModelValue() { Attribute = new ModelAttribute() { AttributeID = rng.Next() }, Instance = new ModelChildInstance() { InstanceID = rng.Next() } };

            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            aValue.Attribute.MarkUnmodified();
            aValue.Instance.MarkUnmodified();

            aValue.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aValue.ObjectState, "Object state failed to transition to 'Unmodified'.");

            ModelAttribute attribute = new ModelAttribute() { AttributeID = rng.Next() };
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
            ModelValue aValue = new ModelValue() { Attribute = new ModelAttribute() { AttributeID = rng.Next() } };

            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            aValue.Attribute.MarkUnmodified();

            aValue.MarkUnmodified();
        }

        [TestMethod]
        public void ValueSetAttributeWhenModified()
        {
            ModelValue aValue = new ModelValue() { Attribute = new ModelAttribute() { AttributeID = rng.Next() }, Instance = new ModelChildInstance() { InstanceID = rng.Next() } };

            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            aValue.Attribute.MarkUnmodified();
            aValue.Instance.MarkUnmodified();

            aValue.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aValue.ObjectState, "Object state failed to transition to 'Unmodified'.");

            ModelAttribute attribute = new ModelAttribute() { AttributeID = rng.Next() };
            aValue.Attribute = attribute;

            Assert.AreEqual(attribute, aValue.Attribute, "Property 'Attribute' was not set properly.");
            Assert.AreEqual(attribute.AttributeID, aValue.AttributeID, "Property 'AttributeID' was not reported properly.");
            Assert.IsTrue(attribute.Values.Contains(aValue), "Property 'Values' was not updated properly.");
            Assert.AreEqual(ObjectState.Modified, aValue.ObjectState, "Object state failed to transition to 'Modified'.");

            attribute = new ModelAttribute() { AttributeID = rng.Next() };
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
            ModelValue aValue = new ModelValue();

            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            aValue.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aValue.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aValue.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aValue.ObjectState, "Object state failed to transition to 'Deleted'.");

            aValue.Attribute = new ModelAttribute() { AttributeID = rng.Next() };
        }
        #endregion

        #region Unit
        [TestMethod]
        public void ValueSetUnitWhenNewWithNewUnit()
        {
            ModelValue aValue = new ModelValue();

            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            ModelUnit aUnit = new ModelUnit() { UnitID = rng.Next() };
            aValue.Unit = aUnit;

            Assert.AreEqual(aUnit, aValue.Unit, "Property 'Units' was not set properly.");
            Assert.AreEqual(aUnit.UnitID, aValue.UnitID, "Property 'UnitID' was not reported properly.");
            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should remain 'New' when property set.");
            Assert.AreEqual(ObjectState.New, aUnit.ObjectState, String.Format("Object state for Unit object incorrectly transitioned to '{0}' when 'Unit' property set.", aUnit.ObjectState));
        }

        [TestMethod]
        public void ValueSetUnitWhenNewWithUnmodifiedUnit()
        {
            ModelValue aValue = new ModelValue();

            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            ModelUnit aUnit = new ModelUnit() { UnitID = rng.Next() };

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
            ModelValue aValue = new ModelValue();

            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            ModelUnit aUnit = new ModelUnit() { UnitID = rng.Next() };

            aUnit.MarkUnmodified();
            Assert.AreEqual(ObjectState.Unmodified, aUnit.ObjectState, "Object state for 'Unit' object failed to transition to 'Unmodified'.");

            aUnit.MarkDeleted();
            Assert.AreEqual(ObjectState.Deleted, aUnit.ObjectState, "Object state for 'Unit' object failed to transition to 'Deleted'.");

            aValue.Unit = aUnit;
        }

        [TestMethod]
        public void ValueSetUnitWhenUnmodifiedVariableUnitsIsTrue()
        {
            ModelValue aValue = new ModelValue() { Instance = new ModelRootInstance() { InstanceID = rng.Next() }, Attribute = new ModelAttribute() { AttributeID = rng.Next(), VariableUnits = true } };

            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            aValue.Instance.MarkUnmodified();
            aValue.Attribute.MarkUnmodified();

            aValue.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aValue.ObjectState, "Object state failed to transition to 'Unmodified'.");

            ModelUnit aUnit = new ModelUnit() { UnitID = rng.Next() };
            aValue.Unit = aUnit;

            Assert.AreEqual(aUnit, aValue.Unit, "Property 'Units' was not set properly.");
            Assert.AreEqual(aUnit.UnitID, aValue.UnitID, "Property 'UnitID' was not reported properly.");
            Assert.AreEqual(ObjectState.Modified, aValue.ObjectState, "Object state failed to transition to 'Modified'.");
            Assert.AreEqual(ObjectState.New, aUnit.ObjectState, String.Format("Object state for Unit object incorrectly transitioned to '{0}' when 'Unit' property set.", aUnit.ObjectState));
        }

        [TestMethod]
        public void ValueSetUnitWhenUnmodifiedVariableUnitsIsFalse()
        {
            ModelValue aValue = new ModelValue() { Instance = new ModelRootInstance() { InstanceID = rng.Next() }, Attribute = new ModelAttribute() { AttributeID = rng.Next(), VariableUnits = false } };

            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            aValue.Instance.MarkUnmodified();
            aValue.Attribute.MarkUnmodified();

            aValue.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aValue.ObjectState, "Object state failed to transition to 'Unmodified'.");

            ModelUnit aUnit = new ModelUnit() { UnitID = rng.Next() };
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
            ModelValue aValue = new ModelValue() { Instance = new ModelRootInstance() { InstanceID = rng.Next() }, Attribute = new ModelAttribute() { AttributeID = rng.Next(), VariableUnits = null } };

            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            aValue.Instance.MarkUnmodified();
            aValue.Attribute.MarkUnmodified();

            aValue.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aValue.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aValue.Unit = new ModelUnit() { UnitID = rng.Next() };
        }

        [TestMethod]
        public void ValueSetUnitWhenModified()
        {
            ModelValue aValue = new ModelValue() { Instance = new ModelRootInstance() { InstanceID = rng.Next() }, Attribute = new ModelAttribute() { AttributeID = rng.Next(), VariableUnits = true } };

            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            aValue.Instance.MarkUnmodified();
            aValue.Attribute.MarkUnmodified();

            aValue.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aValue.ObjectState, "Object state failed to transition to 'Unmodified'.");

            ModelUnit aUnit = new ModelUnit() { UnitID = rng.Next() };
            aValue.Unit = aUnit;

            Assert.AreEqual(aUnit, aValue.Unit, "Property 'Units' was not set properly.");
            Assert.AreEqual(aUnit.UnitID, aValue.UnitID, "Property 'UnitID' was not reported properly.");
            Assert.AreEqual(ObjectState.Modified, aValue.ObjectState, "Object state failed to transition to 'Modified'.");
            Assert.AreEqual(ObjectState.New, aUnit.ObjectState, String.Format("Object state for Unit object incorrectly transitioned to '{0}' when 'Unit' property set.", aUnit.ObjectState));

            aUnit = new ModelUnit() { UnitID = rng.Next() };
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
            ModelValue aValue = new ModelValue() { Instance = new ModelRootInstance() { InstanceID = rng.Next() }, Attribute = new ModelAttribute() { AttributeID = rng.Next() } };

            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            aValue.Instance.MarkUnmodified();
            aValue.Attribute.MarkUnmodified();

            aValue.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aValue.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aValue.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aValue.ObjectState, "Object state failed to transition to 'Deleted'.");

            aValue.Unit = new ModelUnit() { UnitID = rng.Next() };
        }

        [TestMethod]
        public void ValueSetUnitIDWhenNew()
        {
            ModelValue aValue = new ModelValue();

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
            ModelValue aValue = new ModelValue() { Instance = new ModelRootInstance() { InstanceID = rng.Next() }, Attribute = new ModelAttribute() { AttributeID = rng.Next(), VariableUnits = true } };

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
            ModelValue aValue = new ModelValue() { Instance = new ModelRootInstance() { InstanceID = rng.Next() }, Attribute = new ModelAttribute() { AttributeID = rng.Next(), VariableUnits = true } };

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
            ModelValue aValue = new ModelValue() { Instance = new ModelRootInstance() { InstanceID = rng.Next() }, Attribute = new ModelAttribute() { AttributeID = rng.Next() } };

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
            ModelValue aValue = new ModelValue();

            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            ModelUnit aUnit = new ModelUnit() { UnitID = rng.Next() };
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
            ModelValue aValue = new ModelValue();

            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            int unitID = rng.Next();
            aValue.UnitID = unitID;

            Assert.IsNull(aValue.Unit, "Property 'Unit' was not reported properly.");
            Assert.AreEqual(unitID, aValue.UnitID, "Property 'UnitID' was not set properly.");
            Assert.AreEqual(ObjectState.New, aValue.ObjectState, "Object state should remain 'New' when property set.");

            ModelUnit aUnit = new ModelUnit() { UnitID = rng.Next() };
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
