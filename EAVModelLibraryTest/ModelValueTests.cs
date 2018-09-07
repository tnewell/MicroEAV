using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

//using EAVModelLibrary;


namespace EAVModelTest
{
    public partial class EAVModelTestHarness
    {
        [TestMethod]
        public void ValueCreate()
        {
            EAVModelLibrary.ModelValue aValue = new EAVModelLibrary.ModelValue();

            Assert.AreEqual(EAV.Model.ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

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
            EAVModelLibrary.ModelValue aValue = new EAVModelLibrary.ModelValue();

            Assert.AreEqual(EAV.Model.ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            aValue.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aValue.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ValueStateTransitionNewToUnmodifiedWithNewID()
        {
            EAVModelLibrary.ModelValue aValue = new EAVModelLibrary.ModelValue() { Instance = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() }, Attribute = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() } };

            Assert.AreEqual(EAV.Model.ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            aValue.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aValue.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        public void ValueStateTransitionNewToUnmodifiedWithValidID()
        {
            EAVModelLibrary.ModelValue aValue = new EAVModelLibrary.ModelValue() { Instance = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() }, Attribute = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() } };

            Assert.AreEqual(EAV.Model.ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            aValue.Instance.MarkUnmodified();
            aValue.Attribute.MarkUnmodified();

            aValue.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aValue.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ValueStateTransitionNewToDeleted()
        {
            EAVModelLibrary.ModelValue aValue = new EAVModelLibrary.ModelValue();

            Assert.AreEqual(EAV.Model.ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            aValue.MarkDeleted();
        }

        [TestMethod]
        public void ValueStateTransitionUnmodifiedToDeleted()
        {
            EAVModelLibrary.ModelValue aValue = new EAVModelLibrary.ModelValue() { Instance = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() }, Attribute = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() } };

            Assert.AreEqual(EAV.Model.ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            aValue.Instance.MarkUnmodified();
            aValue.Attribute.MarkUnmodified();

            aValue.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aValue.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aValue.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, aValue.ObjectState, "Object state failed to transition to 'Deleted'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ValueStateTransitionDeletedToUnmodified()
        {
            EAVModelLibrary.ModelValue aValue = new EAVModelLibrary.ModelValue() { Instance = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() }, Attribute = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() } };

            Assert.AreEqual(EAV.Model.ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            aValue.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aValue.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aValue.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, aValue.ObjectState, "Object state failed to transition to 'Deleted'.");

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
            EAVModelLibrary.ModelValue aValue = new EAVModelLibrary.ModelValue();

            Assert.AreEqual(EAV.Model.ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            string value = Guid.NewGuid().ToString();
            aValue.RawValue = value;

            Assert.AreEqual(value, aValue.RawValue, "Property 'RawValue' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aValue.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ValueSetRawValueWhenUnmodified()
        {
            EAVModelLibrary.ModelValue aValue = new EAVModelLibrary.ModelValue() { Instance = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() }, Attribute = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() } };

            Assert.AreEqual(EAV.Model.ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            aValue.Instance.MarkUnmodified();
            aValue.Attribute.MarkUnmodified();

            aValue.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aValue.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            aValue.RawValue = value;

            Assert.AreEqual(value, aValue.RawValue, "Property 'RawValue' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aValue.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void ValueSetRawValueWhenModified()
        {
            EAVModelLibrary.ModelValue aValue = new EAVModelLibrary.ModelValue() { Instance = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() }, Attribute = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() } };

            Assert.AreEqual(EAV.Model.ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            aValue.Instance.MarkUnmodified();
            aValue.Attribute.MarkUnmodified();

            aValue.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aValue.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            aValue.RawValue = value;

            Assert.AreEqual(value, aValue.RawValue, "Property 'RawValue' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aValue.ObjectState, "Object state failed to transition to 'Modified'.");

            value = Guid.NewGuid().ToString();
            aValue.RawValue = value;

            Assert.AreEqual(value, aValue.RawValue, "Property 'RawValue' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aValue.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ValueSetRawValueWhenDeleted()
        {
            EAVModelLibrary.ModelValue aValue = new EAVModelLibrary.ModelValue() { Instance = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() }, Attribute = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() } };

            Assert.AreEqual(EAV.Model.ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            aValue.Instance.MarkUnmodified();
            aValue.Attribute.MarkUnmodified();

            aValue.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aValue.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aValue.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, aValue.ObjectState, "Object state failed to transition to 'Deleted'.");

            aValue.RawValue = Guid.NewGuid().ToString();
        }
        #endregion

        #endregion

        #region Object Properties
        #region Instance
        [TestMethod]
        public void ValueSetInstanceWhenNew()
        {
            EAVModelLibrary.ModelValue aValue = new EAVModelLibrary.ModelValue();

            Assert.AreEqual(EAV.Model.ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelChildInstance value = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };
            aValue.Instance = value;

            Assert.AreEqual(value, aValue.Instance, "Property 'Instance' was not set properly.");
            Assert.AreEqual(value.InstanceID, aValue.InstanceID, "Property 'InstanceID' was not reported properly.");
            Assert.IsTrue(value.Values.Contains(aValue), "Property 'Values' was not updated properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aValue.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ValueSetInstanceWhenUnmodified()
        {
            EAVModelLibrary.ModelValue aValue = new EAVModelLibrary.ModelValue() { Attribute = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() }, Instance = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() } };

            Assert.AreEqual(EAV.Model.ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            aValue.Attribute.MarkUnmodified();
            aValue.Instance.MarkUnmodified();

            aValue.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aValue.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVModelLibrary.ModelChildInstance instance = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };
            aValue.Instance = instance;

            Assert.AreEqual(instance, aValue.Instance, "Property 'Instance' was not set properly.");
            Assert.AreEqual(instance.InstanceID, aValue.InstanceID, "Property 'InstanceID' was not reported properly.");
            Assert.IsTrue(instance.Values.Contains(aValue), "Property 'Values' was not updated properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aValue.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ValueSetInstanceWhenUnmodifiedWithNullAttribute()
        {
            EAVModelLibrary.ModelValue aValue = new EAVModelLibrary.ModelValue() { Instance = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() } };

            Assert.AreEqual(EAV.Model.ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            aValue.Instance.MarkUnmodified();

            aValue.MarkUnmodified();
        }

        [TestMethod]
        public void ValueSetInstanceWhenModified()
        {
            EAVModelLibrary.ModelValue aValue = new EAVModelLibrary.ModelValue() { Attribute = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() }, Instance = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() } };

            Assert.AreEqual(EAV.Model.ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            aValue.Attribute.MarkUnmodified();
            aValue.Instance.MarkUnmodified();
            aValue.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aValue.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVModelLibrary.ModelChildInstance instance = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };
            aValue.Instance = instance;

            Assert.AreEqual(instance, aValue.Instance, "Property 'Instance' was not set properly.");
            Assert.AreEqual(instance.InstanceID, aValue.InstanceID, "Property 'InstanceID' was not reported properly.");
            Assert.IsTrue(instance.Values.Contains(aValue), "Property 'Values' was not updated properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aValue.ObjectState, "Object state failed to transition to 'Modified'.");

            instance = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };
            aValue.Instance = instance;

            Assert.AreEqual(instance, aValue.Instance, "Property 'Instance' was not set properly.");
            Assert.AreEqual(instance.InstanceID, aValue.InstanceID, "Property 'InstanceID' was not reported properly.");
            Assert.IsTrue(instance.Values.Contains(aValue), "Property 'Values' was not updated properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aValue.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ValueSetInstanceWhenDeleted()
        {
            EAVModelLibrary.ModelValue aValue = new EAVModelLibrary.ModelValue();

            Assert.AreEqual(EAV.Model.ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            aValue.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aValue.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aValue.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, aValue.ObjectState, "Object state failed to transition to 'Deleted'.");

            aValue.Instance = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };
        }
        #endregion

        #region Attribute
        [TestMethod]
        public void ValueSetAttributeWhenNew()
        {
            EAVModelLibrary.ModelValue aValue = new EAVModelLibrary.ModelValue();

            Assert.AreEqual(EAV.Model.ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelAttribute attribute = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() };
            aValue.Attribute = attribute;

            Assert.AreEqual(attribute, aValue.Attribute, "Property 'Attribute' was not set properly.");
            Assert.AreEqual(attribute.AttributeID, aValue.AttributeID, "Property 'AttributeID' was not reported properly.");
            Assert.IsTrue(attribute.Values.Contains(aValue), "Property 'Values' was not updated properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aValue.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ValueSetAttributeWhenUnmodified()
        {
            EAVModelLibrary.ModelValue aValue = new EAVModelLibrary.ModelValue() { Attribute = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() }, Instance = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() } };

            Assert.AreEqual(EAV.Model.ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            aValue.Attribute.MarkUnmodified();
            aValue.Instance.MarkUnmodified();

            aValue.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aValue.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVModelLibrary.ModelAttribute attribute = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() };
            aValue.Attribute = attribute;

            Assert.AreEqual(attribute, aValue.Attribute, "Property 'Attribute' was not set properly.");
            Assert.AreEqual(attribute.AttributeID, aValue.AttributeID, "Property 'AttributeID' was not reported properly.");
            Assert.IsTrue(attribute.Values.Contains(aValue), "Property 'Values' was not updated properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aValue.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ValueSetAttributeWhenUnmodifiedWithNullInstance()
        {
            EAVModelLibrary.ModelValue aValue = new EAVModelLibrary.ModelValue() { Attribute = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() } };

            Assert.AreEqual(EAV.Model.ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            aValue.Attribute.MarkUnmodified();

            aValue.MarkUnmodified();
        }

        [TestMethod]
        public void ValueSetAttributeWhenModified()
        {
            EAVModelLibrary.ModelValue aValue = new EAVModelLibrary.ModelValue() { Attribute = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() }, Instance = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() } };

            Assert.AreEqual(EAV.Model.ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            aValue.Attribute.MarkUnmodified();
            aValue.Instance.MarkUnmodified();

            aValue.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aValue.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVModelLibrary.ModelAttribute attribute = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() };
            aValue.Attribute = attribute;

            Assert.AreEqual(attribute, aValue.Attribute, "Property 'Attribute' was not set properly.");
            Assert.AreEqual(attribute.AttributeID, aValue.AttributeID, "Property 'AttributeID' was not reported properly.");
            Assert.IsTrue(attribute.Values.Contains(aValue), "Property 'Values' was not updated properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aValue.ObjectState, "Object state failed to transition to 'Modified'.");

            attribute = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() };
            aValue.Attribute = attribute;

            Assert.AreEqual(attribute, aValue.Attribute, "Property 'Attribute' was not set properly.");
            Assert.AreEqual(attribute.AttributeID, aValue.AttributeID, "Property 'AttributeID' was not reported properly.");
            Assert.IsTrue(attribute.Values.Contains(aValue), "Property 'Values' was not updated properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aValue.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ValueSetAttributeWhenDeleted()
        {
            EAVModelLibrary.ModelValue aValue = new EAVModelLibrary.ModelValue();

            Assert.AreEqual(EAV.Model.ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            aValue.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aValue.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aValue.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, aValue.ObjectState, "Object state failed to transition to 'Deleted'.");

            aValue.Attribute = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() };
        }
        #endregion

        #region Unit
        [TestMethod]
        public void ValueSetUnitWhenNewWithNewUnit()
        {
            EAVModelLibrary.ModelValue aValue = new EAVModelLibrary.ModelValue();

            Assert.AreEqual(EAV.Model.ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelUnit aUnit = new EAVModelLibrary.ModelUnit() { UnitID = rng.Next() };
            aValue.Unit = aUnit;

            Assert.AreEqual(aUnit, aValue.Unit, "Property 'Units' was not set properly.");
            Assert.AreEqual(aUnit.UnitID, aValue.UnitID, "Property 'UnitID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aValue.ObjectState, "Object state should remain 'New' when property set.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aUnit.ObjectState, String.Format("Object state for Unit object incorrectly transitioned to '{0}' when 'Unit' property set.", aUnit.ObjectState));
        }

        [TestMethod]
        public void ValueSetUnitWhenNewWithUnmodifiedUnit()
        {
            EAVModelLibrary.ModelValue aValue = new EAVModelLibrary.ModelValue();

            Assert.AreEqual(EAV.Model.ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelUnit aUnit = new EAVModelLibrary.ModelUnit() { UnitID = rng.Next() };

            aUnit.MarkUnmodified();
            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aUnit.ObjectState, "Object state for 'Unit' object failed to transition to 'Unmodified'.");

            aValue.Unit = aUnit;

            Assert.AreEqual(aUnit, aValue.Unit, "Property 'Unit' was not set properly.");
            Assert.AreEqual(aUnit.UnitID, aValue.UnitID, "Property 'UnitID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aValue.ObjectState, "Object state should remain 'New' when property set.");
            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aUnit.ObjectState, String.Format("Object state for Unit object incorrectly transitioned to '{0}' when 'Unit' property set.", aUnit.ObjectState));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ValueSetUnitWhenNewWithDeletedUnit()
        {
            EAVModelLibrary.ModelValue aValue = new EAVModelLibrary.ModelValue();

            Assert.AreEqual(EAV.Model.ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelUnit aUnit = new EAVModelLibrary.ModelUnit() { UnitID = rng.Next() };

            aUnit.MarkUnmodified();
            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aUnit.ObjectState, "Object state for 'Unit' object failed to transition to 'Unmodified'.");

            aUnit.MarkDeleted();
            Assert.AreEqual(EAV.Model.ObjectState.Deleted, aUnit.ObjectState, "Object state for 'Unit' object failed to transition to 'Deleted'.");

            aValue.Unit = aUnit;
        }

        [TestMethod]
        public void ValueSetUnitWhenUnmodifiedVariableUnitsIsTrue()
        {
            EAVModelLibrary.ModelValue aValue = new EAVModelLibrary.ModelValue() { Instance = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() }, Attribute = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next(), VariableUnits = true } };

            Assert.AreEqual(EAV.Model.ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            aValue.Instance.MarkUnmodified();
            aValue.Attribute.MarkUnmodified();

            aValue.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aValue.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVModelLibrary.ModelUnit aUnit = new EAVModelLibrary.ModelUnit() { UnitID = rng.Next() };
            aValue.Unit = aUnit;

            Assert.AreEqual(aUnit, aValue.Unit, "Property 'Units' was not set properly.");
            Assert.AreEqual(aUnit.UnitID, aValue.UnitID, "Property 'UnitID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aValue.ObjectState, "Object state failed to transition to 'Modified'.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aUnit.ObjectState, String.Format("Object state for Unit object incorrectly transitioned to '{0}' when 'Unit' property set.", aUnit.ObjectState));
        }

        [TestMethod]
        public void ValueSetUnitWhenUnmodifiedVariableUnitsIsFalse()
        {
            EAVModelLibrary.ModelValue aValue = new EAVModelLibrary.ModelValue() { Instance = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() }, Attribute = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next(), VariableUnits = false } };

            Assert.AreEqual(EAV.Model.ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            aValue.Instance.MarkUnmodified();
            aValue.Attribute.MarkUnmodified();

            aValue.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aValue.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVModelLibrary.ModelUnit aUnit = new EAVModelLibrary.ModelUnit() { UnitID = rng.Next() };
            aValue.Unit = aUnit;

            Assert.AreEqual(aUnit, aValue.Unit, "Property 'Units' was not set properly.");
            Assert.AreEqual(aUnit.UnitID, aValue.UnitID, "Property 'UnitID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aValue.ObjectState, "Object state failed to transition to 'Modified'.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aUnit.ObjectState, String.Format("Object state for Unit object incorrectly transitioned to '{0}' when 'Unit' property set.", aUnit.ObjectState));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ValueSetUnitWhenUnmodifiedVariableUnitsIsNull()
        {
            EAVModelLibrary.ModelValue aValue = new EAVModelLibrary.ModelValue() { Instance = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() }, Attribute = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next(), VariableUnits = null } };

            Assert.AreEqual(EAV.Model.ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            aValue.Instance.MarkUnmodified();
            aValue.Attribute.MarkUnmodified();

            aValue.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aValue.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aValue.Unit = new EAVModelLibrary.ModelUnit() { UnitID = rng.Next() };
        }

        [TestMethod]
        public void ValueSetUnitWhenModified()
        {
            EAVModelLibrary.ModelValue aValue = new EAVModelLibrary.ModelValue() { Instance = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() }, Attribute = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next(), VariableUnits = true } };

            Assert.AreEqual(EAV.Model.ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            aValue.Instance.MarkUnmodified();
            aValue.Attribute.MarkUnmodified();

            aValue.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aValue.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVModelLibrary.ModelUnit aUnit = new EAVModelLibrary.ModelUnit() { UnitID = rng.Next() };
            aValue.Unit = aUnit;

            Assert.AreEqual(aUnit, aValue.Unit, "Property 'Units' was not set properly.");
            Assert.AreEqual(aUnit.UnitID, aValue.UnitID, "Property 'UnitID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aValue.ObjectState, "Object state failed to transition to 'Modified'.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aUnit.ObjectState, String.Format("Object state for Unit object incorrectly transitioned to '{0}' when 'Unit' property set.", aUnit.ObjectState));

            aUnit = new EAVModelLibrary.ModelUnit() { UnitID = rng.Next() };
            aValue.Unit = aUnit;

            Assert.AreEqual(aUnit, aValue.Unit, "Property 'Units' was not set properly.");
            Assert.AreEqual(aUnit.UnitID, aValue.UnitID, "Property 'UnitID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aValue.ObjectState, "Object state should remain 'Modified' when property set.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aUnit.ObjectState, String.Format("Object state for Unit object incorrectly transitioned to '{0}' when 'Unit' property set.", aUnit.ObjectState));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ValueSetUnitWhenDeleted()
        {
            EAVModelLibrary.ModelValue aValue = new EAVModelLibrary.ModelValue() { Instance = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() }, Attribute = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() } };

            Assert.AreEqual(EAV.Model.ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

            aValue.Instance.MarkUnmodified();
            aValue.Attribute.MarkUnmodified();

            aValue.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aValue.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aValue.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, aValue.ObjectState, "Object state failed to transition to 'Deleted'.");

            aValue.Unit = new EAVModelLibrary.ModelUnit() { UnitID = rng.Next() };
        }

        //[TestMethod]
        //public void ValueSetUnitIDWhenNew()
        //{
        //    EAVModelLibrary.ModelValue aValue = new EAVModelLibrary.ModelValue();

        //    Assert.AreEqual(EAV.Model.ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

        //    int unitID = rng.Next();
        //    aValue.UnitID = unitID;

        //    Assert.IsNull(aValue.Unit, "Property 'Unit' was not reported properly.");
        //    Assert.AreEqual(unitID, aValue.UnitID, "Property 'UnitID' was not set properly.");
        //    Assert.AreEqual(EAV.Model.ObjectState.New, aValue.ObjectState, "Object state should remain 'New' when property set.");
        //}

        //[TestMethod]
        //public void ValueSetUnitIDWhenUnmodified()
        //{
        //    EAVModelLibrary.ModelValue aValue = new EAVModelLibrary.ModelValue() { Instance = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() }, Attribute = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next(), VariableUnits = true } };

        //    Assert.AreEqual(EAV.Model.ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

        //    aValue.Instance.MarkUnmodified();
        //    aValue.Attribute.MarkUnmodified();

        //    aValue.MarkUnmodified();

        //    Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aValue.ObjectState, "Object state failed to transition to 'Unmodified'.");

        //    int unitID = rng.Next();
        //    aValue.UnitID = unitID;

        //    Assert.IsNull(aValue.Unit, "Property 'Unit' was not reported properly.");
        //    Assert.AreEqual(unitID, aValue.UnitID, "Property 'UnitID' was not set properly.");
        //    Assert.AreEqual(EAV.Model.ObjectState.Modified, aValue.ObjectState, "Object state failed to transition to 'Modified'.");
        //}

        //[TestMethod]
        //public void ValueSetUnitIDWhenModified()
        //{
        //    EAVModelLibrary.ModelValue aValue = new EAVModelLibrary.ModelValue() { Instance = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() }, Attribute = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next(), VariableUnits = true } };

        //    Assert.AreEqual(EAV.Model.ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

        //    aValue.Instance.MarkUnmodified();
        //    aValue.Attribute.MarkUnmodified();

        //    aValue.MarkUnmodified();

        //    Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aValue.ObjectState, "Object state failed to transition to 'Unmodified'.");

        //    int unitID = rng.Next();
        //    aValue.UnitID = unitID;

        //    Assert.IsNull(aValue.Unit, "Property 'Unit' was not reported properly.");
        //    Assert.AreEqual(unitID, aValue.UnitID, "Property 'UnitID' was not set properly.");
        //    Assert.AreEqual(EAV.Model.ObjectState.Modified, aValue.ObjectState, "Object state failed to transition to 'Modified'.");

        //    unitID = rng.Next();
        //    aValue.UnitID = unitID;

        //    Assert.IsNull(aValue.Unit, "Property 'Unit' was not reported properly.");
        //    Assert.AreEqual(unitID, aValue.UnitID, "Property 'UnitID' was not set properly.");
        //    Assert.AreEqual(EAV.Model.ObjectState.Modified, aValue.ObjectState, "Object state should remain 'Modified' when property set.");
        //}

        //[TestMethod]
        //[ExpectedException(typeof(InvalidOperationException))]
        //public void ValueSetUnitIDWhenDeleted()
        //{
        //    EAVModelLibrary.ModelValue aValue = new EAVModelLibrary.ModelValue() { Instance = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() }, Attribute = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() } };

        //    Assert.AreEqual(EAV.Model.ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

        //    aValue.Instance.MarkUnmodified();
        //    aValue.Attribute.MarkUnmodified();

        //    aValue.MarkUnmodified();

        //    Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aValue.ObjectState, "Object state failed to transition to 'Unmodified'.");

        //    aValue.MarkDeleted();

        //    Assert.AreEqual(EAV.Model.ObjectState.Deleted, aValue.ObjectState, "Object state failed to transition to 'Deleted'.");

        //    aValue.UnitID = rng.Next();
        //}

        //[TestMethod]
        //[ExpectedException(typeof(InvalidOperationException))]
        //public void ValueSetUnitWhenNewWithNewUnitThenUnitID()
        //{
        //    EAVModelLibrary.ModelValue aValue = new EAVModelLibrary.ModelValue();

        //    Assert.AreEqual(EAV.Model.ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

        //    EAVModelLibrary.ModelUnit aUnit = new EAVModelLibrary.ModelUnit() { UnitID = rng.Next() };
        //    aValue.Unit = aUnit;

        //    Assert.AreEqual(aUnit, aValue.Unit, "Property 'Units' was not set properly.");
        //    Assert.AreEqual(aUnit.UnitID, aValue.UnitID, "Property 'UnitID' was not reported properly.");
        //    Assert.AreEqual(EAV.Model.ObjectState.New, aValue.ObjectState, "Object state should remain 'New' when property set.");
        //    Assert.AreEqual(EAV.Model.ObjectState.New, aUnit.ObjectState, String.Format("Object state for Unit object incorrectly transitioned to '{0}' when 'Unit' property set.", aUnit.ObjectState));

        //    aValue.UnitID = rng.Next();
        //}

        //[TestMethod]
        //public void ValueSetUnitIDWhenNewWithNewUnit()
        //{
        //    EAVModelLibrary.ModelValue aValue = new EAVModelLibrary.ModelValue();

        //    Assert.AreEqual(EAV.Model.ObjectState.New, aValue.ObjectState, "Object state should be 'New' on creation.");

        //    int unitID = rng.Next();
        //    aValue.UnitID = unitID;

        //    Assert.IsNull(aValue.Unit, "Property 'Unit' was not reported properly.");
        //    Assert.AreEqual(unitID, aValue.UnitID, "Property 'UnitID' was not set properly.");
        //    Assert.AreEqual(EAV.Model.ObjectState.New, aValue.ObjectState, "Object state should remain 'New' when property set.");

        //    EAVModelLibrary.ModelUnit aUnit = new EAVModelLibrary.ModelUnit() { UnitID = rng.Next() };
        //    aValue.Unit = aUnit;

        //    if (aUnit.UnitID == unitID)
        //        Assert.Inconclusive("The same value was selected for unit ID.");

        //    Assert.AreEqual(aUnit, aValue.Unit, "Property 'Units' was not set properly.");
        //    Assert.AreEqual(aUnit.UnitID, aValue.UnitID, "Property 'UnitID' was not reported properly.");
        //    Assert.AreNotEqual(unitID, aValue.UnitID, "Property 'UnitID' was not set properly.");
        //    Assert.AreEqual(EAV.Model.ObjectState.New, aValue.ObjectState, "Object state should remain 'New' when property set.");
        //    Assert.AreEqual(EAV.Model.ObjectState.New, aUnit.ObjectState, String.Format("Object state for Unit object incorrectly transitioned to '{0}' when 'Unit' property set.", aUnit.ObjectState));
        //}
        #endregion
        #endregion

        #region Collection Properties
        // N/A
        #endregion
    }
}
