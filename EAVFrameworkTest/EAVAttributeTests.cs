using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using EAV.Model;

using EAVFramework.Model;


namespace EAVFrameworkTest
{
    public partial class EAVFrameworkTestHarness
    {
        private EAVDataType GetRandomDataType()
        {
            var types = Enum.GetValues(typeof(EAVDataType)).Cast<EAVDataType>().Where(it => it != default(EAVDataType));
            return (types.ElementAt(rng.Next(0, types.Count())));
        }

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
            Assert.AreEqual(0, anAttribute.Sequence, "Property 'Sequence' should be zero on creation.");
            Assert.IsFalse(anAttribute.IsKey, "Property 'IsKey' should be false on creation.");
            Assert.IsNull(anAttribute.VariableUnits, "Property 'VariableUnits' should be null on creation.");

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
        #region Name
        [TestMethod]
        public void AttributeSetNameWhenNew()
        {
            EAVAttribute anAttribute = new EAVAttribute();

            Assert.AreEqual(ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            string value = Guid.NewGuid().ToString();
            anAttribute.Name = value;

            Assert.AreEqual(value, anAttribute.Name, "Property 'Name' was not set properly.");
            Assert.AreEqual(ObjectState.New, anAttribute.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void AttributeSetNameWhenUnmodified()
        {
            EAVAttribute anAttribute = new EAVAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            anAttribute.Name = value;

            Assert.AreEqual(value, anAttribute.Name, "Property 'Name' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, anAttribute.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void AttributeSetNameWhenModified()
        {
            EAVAttribute anAttribute = new EAVAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            anAttribute.Name = value;

            Assert.AreEqual(value, anAttribute.Name, "Property 'Name' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, anAttribute.ObjectState, "Object state failed to transition to 'Modified'.");

            value = Guid.NewGuid().ToString();
            anAttribute.Name = value;

            Assert.AreEqual(value, anAttribute.Name, "Property 'Name' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, anAttribute.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AttributeSetNameWhenDeleted()
        {
            EAVAttribute anAttribute = new EAVAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anAttribute.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, anAttribute.ObjectState, "Object state failed to transition to 'Deleted'.");

            anAttribute.Name = Guid.NewGuid().ToString();
        }
        #endregion

        #region DataName
        [TestMethod]
        public void AttributeSetDataNameWhenNew()
        {
            EAVAttribute anAttribute = new EAVAttribute();

            Assert.AreEqual(ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            string value = Guid.NewGuid().ToString();
            anAttribute.DataName = value;

            Assert.AreEqual(value, anAttribute.DataName, "Property 'DataName' was not set properly.");
            Assert.AreEqual(ObjectState.New, anAttribute.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void AttributeSetDataNameWhenUnmodified()
        {
            EAVAttribute anAttribute = new EAVAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            anAttribute.DataName = value;

            Assert.AreEqual(value, anAttribute.DataName, "Property 'DataName' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, anAttribute.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void AttributeSetDataNameWhenModified()
        {
            EAVAttribute anAttribute = new EAVAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            anAttribute.DataName = value;

            Assert.AreEqual(value, anAttribute.DataName, "Property 'DataName' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, anAttribute.ObjectState, "Object state failed to transition to 'Modified'.");

            value = Guid.NewGuid().ToString();
            anAttribute.DataName = value;

            Assert.AreEqual(value, anAttribute.DataName, "Property 'DataName' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, anAttribute.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AttributeSetDataNameWhenDeleted()
        {
            EAVAttribute anAttribute = new EAVAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anAttribute.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, anAttribute.ObjectState, "Object state failed to transition to 'Deleted'.");

            anAttribute.DataName = Guid.NewGuid().ToString();
        }
        #endregion

        #region DisplayText
        [TestMethod]
        public void AttributeSetDisplayTextWhenNew()
        {
            EAVAttribute anAttribute = new EAVAttribute();

            Assert.AreEqual(ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            string value = Guid.NewGuid().ToString();
            anAttribute.DisplayText = value;

            Assert.AreEqual(value, anAttribute.DisplayText, "Property 'DisplayText' was not set properly.");
            Assert.AreEqual(ObjectState.New, anAttribute.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void AttributeSetDisplayTextWhenUnmodified()
        {
            EAVAttribute anAttribute = new EAVAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            anAttribute.DisplayText = value;

            Assert.AreEqual(value, anAttribute.DisplayText, "Property 'DisplayText' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, anAttribute.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void AttributeSetDisplayTextWhenModified()
        {
            EAVAttribute anAttribute = new EAVAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            anAttribute.DisplayText = value;

            Assert.AreEqual(value, anAttribute.DisplayText, "Property 'DisplayText' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, anAttribute.ObjectState, "Object state failed to transition to 'Modified'.");

            value = Guid.NewGuid().ToString();
            anAttribute.DisplayText = value;

            Assert.AreEqual(value, anAttribute.DisplayText, "Property 'DisplayText' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, anAttribute.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AttributeSetDisplayTextWhenDeleted()
        {
            EAVAttribute anAttribute = new EAVAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anAttribute.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, anAttribute.ObjectState, "Object state failed to transition to 'Deleted'.");

            anAttribute.DisplayText = Guid.NewGuid().ToString();
        }
        #endregion

        #region DataType
        [TestMethod]
        public void AttributeSetDataTypeWhenNew()
        {
            EAVAttribute anAttribute = new EAVAttribute();

            Assert.AreEqual(ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            EAVDataType dataType = GetRandomDataType();
            anAttribute.DataType = dataType;

            Assert.AreEqual(dataType, anAttribute.DataType, "Property 'DataType' was not set properly.");
            Assert.AreEqual(ObjectState.New, anAttribute.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void AttributeSetDataTypeWhenUnmodified()
        {
            EAVAttribute anAttribute = new EAVAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVDataType dataType = GetRandomDataType();
            anAttribute.DataType = dataType;

            Assert.AreEqual(dataType, anAttribute.DataType, "Property 'DataType' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, anAttribute.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void AttributeSetDataTypeWhenModified()
        {
            EAVAttribute anAttribute = new EAVAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVDataType dataType = GetRandomDataType();
            anAttribute.DataType = dataType;

            Assert.AreEqual(dataType, anAttribute.DataType, "Property 'DataType' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, anAttribute.ObjectState, "Object state failed to transition to 'Modified'.");

            dataType = GetRandomDataType();
            anAttribute.DataType = dataType;

            Assert.AreEqual(dataType, anAttribute.DataType, "Property 'DataType' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, anAttribute.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AttributeSetDataTypeWhenDeleted()
        {
            EAVAttribute anAttribute = new EAVAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anAttribute.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, anAttribute.ObjectState, "Object state failed to transition to 'Deleted'.");

            anAttribute.DataType = GetRandomDataType();
        }
        #endregion

        #region Sequence
        [TestMethod]
        public void AttributeSetSequenceWhenNew()
        {
            EAVAttribute anAttribute = new EAVAttribute();

            Assert.AreEqual(ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            int value = rng.Next();
            anAttribute.Sequence = value;

            Assert.AreEqual(value, anAttribute.Sequence, "Property 'Sequence' was not set properly.");
            Assert.AreEqual(ObjectState.New, anAttribute.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void AttributeSetSequenceWhenUnmodified()
        {
            EAVAttribute anAttribute = new EAVAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            int value = rng.Next();
            anAttribute.Sequence = value;

            Assert.AreEqual(value, anAttribute.Sequence, "Property 'Sequence' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, anAttribute.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void AttributeSetSequenceWhenModified()
        {
            EAVAttribute anAttribute = new EAVAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            int value = rng.Next();
            anAttribute.Sequence = value;

            Assert.AreEqual(value, anAttribute.Sequence, "Property 'Sequence' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, anAttribute.ObjectState, "Object state failed to transition to 'Modified'.");

            value = rng.Next();
            anAttribute.Sequence = value;

            Assert.AreEqual(value, anAttribute.Sequence, "Property 'Sequence' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, anAttribute.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AttributeSetSequenceWhenDeleted()
        {
            EAVAttribute anAttribute = new EAVAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anAttribute.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, anAttribute.ObjectState, "Object state failed to transition to 'Deleted'.");

            anAttribute.Sequence = rng.Next();
        }
        #endregion

        #region IsKey
        [TestMethod]
        public void AttributeSetIsKeyWhenNew()
        {
            EAVAttribute anAttribute = new EAVAttribute();

            Assert.AreEqual(ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.IsKey = true;

            Assert.AreEqual(true, anAttribute.IsKey, "Property 'IsKey' was not set properly.");
            Assert.AreEqual(ObjectState.New, anAttribute.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void AttributeSetIsKeyWhenUnmodified()
        {
            EAVAttribute anAttribute = new EAVAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anAttribute.IsKey = true;

            Assert.AreEqual(true, anAttribute.IsKey, "Property 'IsKey' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, anAttribute.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void AttributeSetIsKeyWhenModified()
        {
            EAVAttribute anAttribute = new EAVAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anAttribute.IsKey = true;

            Assert.AreEqual(true, anAttribute.IsKey, "Property 'IsKey' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, anAttribute.ObjectState, "Object state failed to transition to 'Modified'.");

            anAttribute.IsKey = false;

            Assert.AreEqual(false, anAttribute.IsKey, "Property 'IsKey' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, anAttribute.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AttributeSetIsKeyWhenDeleted()
        {
            EAVAttribute anAttribute = new EAVAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anAttribute.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, anAttribute.ObjectState, "Object state failed to transition to 'Deleted'.");

            anAttribute.IsKey = true;
        }
        #endregion

        #region VariableUnits
        [TestMethod]
        public void AttributeSetVariableUnitsWhenNew()
        {
            EAVAttribute anAttribute = new EAVAttribute();

            Assert.AreEqual(ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.VariableUnits = true;

            Assert.AreEqual(true, anAttribute.VariableUnits, "Property 'VariableUnits' was not set properly.");
            Assert.AreEqual(ObjectState.New, anAttribute.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void AttributeSetVariableUnitsWhenUnmodified()
        {
            EAVAttribute anAttribute = new EAVAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anAttribute.VariableUnits = true;

            Assert.AreEqual(true, anAttribute.VariableUnits, "Property 'VariableUnits' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, anAttribute.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void AttributeSetVariableUnitsWhenModified()
        {
            EAVAttribute anAttribute = new EAVAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anAttribute.VariableUnits = true;

            Assert.AreEqual(true, anAttribute.VariableUnits, "Property 'VariableUnits' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, anAttribute.ObjectState, "Object state failed to transition to 'Modified'.");

            anAttribute.VariableUnits = false;

            Assert.AreEqual(false, anAttribute.VariableUnits, "Property 'VariableUnits' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, anAttribute.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AttributeSetVariableUnitsWhenDeleted()
        {
            EAVAttribute anAttribute = new EAVAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anAttribute.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, anAttribute.ObjectState, "Object state failed to transition to 'Deleted'.");

            anAttribute.VariableUnits = true;
        }
        #endregion
        #endregion

        #region Object Properties
        #region Container
        [TestMethod]
        public void AttributeSetContainerWhenNew()
        {
            EAVAttribute anAttribute = new EAVAttribute();

            Assert.AreEqual(ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            EAVRootContainer value = new EAVRootContainer() { ContainerID = rng.Next() };
            anAttribute.Container = value;

            Assert.AreEqual(value, anAttribute.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(value.ContainerID, anAttribute.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.IsTrue(value.Attributes.Contains(anAttribute), "Property 'Attributes' was not updated properly.");
            Assert.AreEqual(ObjectState.New, anAttribute.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void AttributeSetContainerWhenUnmodified()
        {
            EAVAttribute anAttribute = new EAVAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVRootContainer value = new EAVRootContainer() { ContainerID = rng.Next() };
            anAttribute.Container = value;

            Assert.AreEqual(value, anAttribute.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(value.ContainerID, anAttribute.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.IsTrue(value.Attributes.Contains(anAttribute), "Property 'Attributes' was not updated properly.");
            Assert.AreEqual(ObjectState.Modified, anAttribute.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void AttributeSetContainerWhenModified()
        {
            EAVAttribute anAttribute = new EAVAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVRootContainer value = new EAVRootContainer() { ContainerID = rng.Next() };
            anAttribute.Container = value;

            Assert.AreEqual(value, anAttribute.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(value.ContainerID, anAttribute.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.IsTrue(value.Attributes.Contains(anAttribute), "Property 'Attributes' was not updated properly.");
            Assert.AreEqual(ObjectState.Modified, anAttribute.ObjectState, "Object state failed to transition to 'Modified'.");

            value = new EAVRootContainer() { ContainerID = rng.Next() };
            anAttribute.Container = value;

            Assert.AreEqual(value, anAttribute.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(value.ContainerID, anAttribute.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.IsTrue(value.Attributes.Contains(anAttribute), "Property 'Attributes' was not updated properly.");
            Assert.AreEqual(ObjectState.Modified, anAttribute.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AttributeSetContainerWhenDeleted()
        {
            EAVAttribute anAttribute = new EAVAttribute();

            Assert.AreEqual(ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anAttribute.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, anAttribute.ObjectState, "Object state failed to transition to 'Deleted'.");

            anAttribute.Container = new EAVRootContainer() { ContainerID = rng.Next() };
        }
        #endregion
        #endregion

        #region Collection Properties
        #region Values
        [TestMethod]
        public void AttributeAddToValuesWhenNew()
        {
            EAVAttribute anAttribute = new EAVAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            EAVValue value = new EAVValue();
            anAttribute.Values.Add(value);

            Assert.IsTrue(anAttribute.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.AreEqual(anAttribute, value.Attribute, "Property 'Attribute' was not set properly.");
            Assert.AreEqual(anAttribute.AttributeID, value.AttributeID, "Property 'AttributeID' was not set properly.");
            Assert.AreEqual(ObjectState.New, anAttribute.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void AttributeAddToValuesWhenUnmodified()
        {
            EAVAttribute anAttribute = new EAVAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVValue value = new EAVValue();
            anAttribute.Values.Add(value);

            Assert.IsTrue(anAttribute.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.AreEqual(anAttribute, value.Attribute, "Property 'Attribute' was not set properly.");
            Assert.AreEqual(anAttribute.AttributeID, value.AttributeID, "Property 'AttributeID' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, anAttribute.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void AttributeAddToValuesWhenModified()
        {
            EAVAttribute anAttribute = new EAVAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVValue value = new EAVValue();
            anAttribute.Values.Add(value);

            Assert.IsTrue(anAttribute.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.AreEqual(anAttribute, value.Attribute, "Property 'Attribute' was not set properly.");
            Assert.AreEqual(anAttribute.AttributeID, value.AttributeID, "Property 'AttributeID' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, anAttribute.ObjectState, "Object state failed to transition to 'Modified'.");

            value = new EAVValue();
            anAttribute.Values.Add(value);

            Assert.IsTrue(anAttribute.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.AreEqual(anAttribute, value.Attribute, "Property 'Attribute' was not set properly.");
            Assert.AreEqual(anAttribute.AttributeID, value.AttributeID, "Property 'AttributeID' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, anAttribute.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void AttributeAddToValuesWhenDeleted()
        {
            EAVAttribute anAttribute = new EAVAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anAttribute.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, anAttribute.ObjectState, "Object state failed to transition to 'Deleted'.");

            anAttribute.Values.Add(new EAVValue());
        }

        [TestMethod]
        public void AttributeRemoveFromValuesWhenNew()
        {
            EAVAttribute anAttribute = new EAVAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            EAVValue value = new EAVValue();
            anAttribute.Values.Add(value);
            value = new EAVValue();
            anAttribute.Values.Add(value);

            Assert.IsTrue(anAttribute.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.AreEqual(anAttribute, value.Attribute, "Property 'Attribute' was not set properly.");
            Assert.AreEqual(anAttribute.AttributeID, value.AttributeID, "Property 'AttributeID' was not reported properly.");
            Assert.AreEqual(ObjectState.New, anAttribute.ObjectState, "Object state should remain 'New' when property set.");

            anAttribute.Values.Remove(value);

            Assert.IsFalse(anAttribute.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.IsNull(value.Attribute, "Property 'Attribute' was not set properly.");
            Assert.IsNull(value.AttributeID, "Property 'AttributeID' was not reported properly.");
            Assert.AreEqual(ObjectState.New, anAttribute.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void AttributeRemoveFromValuesWhenUnmodified()
        {
            EAVAttribute anAttribute = new EAVAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            EAVValue value = new EAVValue();
            anAttribute.Values.Add(value);
            value = new EAVValue();
            anAttribute.Values.Add(value);

            Assert.IsTrue(anAttribute.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.AreEqual(anAttribute, value.Attribute, "Property 'Attribute' was not set properly.");
            Assert.AreEqual(anAttribute.AttributeID, value.AttributeID, "Property 'AttributeID' was not reported properly.");
            Assert.AreEqual(ObjectState.New, anAttribute.ObjectState, "Object state should remain 'New' when property set.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anAttribute.Values.Remove(value);

            Assert.IsFalse(anAttribute.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.IsNull(value.Attribute, "Property 'Attribute' was not set properly.");
            Assert.IsNull(value.AttributeID, "Property 'AttributeID' was not reported properly.");
            Assert.AreEqual(ObjectState.Modified, anAttribute.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void AttributeRemoveFromValuesWhenModified()
        {
            EAVAttribute anAttribute = new EAVAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVValue value = new EAVValue();
            anAttribute.Values.Add(value);
            value = new EAVValue();
            anAttribute.Values.Add(value);

            Assert.IsTrue(anAttribute.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.AreEqual(anAttribute, value.Attribute, "Property 'Attribute' was not set properly.");
            Assert.AreEqual(anAttribute.AttributeID, value.AttributeID, "Property 'AttributeID' was not reported properly.");
            Assert.AreEqual(ObjectState.Modified, anAttribute.ObjectState, "Object state failed to transition to 'Modified'.");

            anAttribute.Values.Remove(value);

            Assert.IsFalse(anAttribute.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.IsNull(value.Attribute, "Property 'Attribute' was not set properly.");
            Assert.IsNull(value.AttributeID, "Property 'AttributeID' was not reported properly.");
            Assert.AreEqual(ObjectState.Modified, anAttribute.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void AttributeRemoveFromValuesWhenDeleted()
        {
            EAVAttribute anAttribute = new EAVAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            EAVValue value = new EAVValue() { Instance = new EAVRootInstance() { InstanceID = rng.Next() } };
            anAttribute.Values.Add(value);

            Assert.IsTrue(anAttribute.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.AreEqual(anAttribute, value.Attribute, "Property 'Attribute' was not set properly.");
            Assert.AreEqual(anAttribute.AttributeID, value.AttributeID, "Property 'AttributeID' was not reported properly.");
            Assert.AreEqual(ObjectState.New, anAttribute.ObjectState, "Object state should remain 'New' when property set.");

            anAttribute.MarkUnmodified();
            value.Instance.MarkUnmodified();
            value.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");
            Assert.AreEqual(ObjectState.Unmodified, value.Instance.ObjectState, "Object state failed to transition to 'Unmodified'.");
            Assert.AreEqual(ObjectState.Unmodified, value.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anAttribute.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, anAttribute.ObjectState, "Object state failed to transition to 'Deleted'.");
            Assert.AreEqual(ObjectState.Deleted, value.ObjectState, "Object state failed to transition to 'Deleted'.");

            anAttribute.Values.Remove(value);
        }

        [TestMethod]
        public void AttributeClearValuesWhenNew()
        {
            EAVAttribute anAttribute = new EAVAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            EAVValue value = new EAVValue();
            anAttribute.Values.Add(value);
            value = new EAVValue();
            anAttribute.Values.Add(value);

            Assert.IsTrue(anAttribute.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.AreEqual(anAttribute, value.Attribute, "Property 'Attribute' was not set properly.");
            Assert.AreEqual(anAttribute.AttributeID, value.AttributeID, "Property 'AttributeID' was not reported properly.");
            Assert.AreEqual(ObjectState.New, anAttribute.ObjectState, "Object state should remain 'New' when property set.");

            anAttribute.Values.Clear();

            Assert.IsFalse(anAttribute.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.IsNull(value.Attribute, "Property 'Attribute' was not set properly.");
            Assert.IsNull(value.AttributeID, "Property 'AttributeID' was not reported properly.");
            Assert.IsFalse(anAttribute.Values.Any(), "Property 'Values' was not updated properly.");
            Assert.AreEqual(ObjectState.New, anAttribute.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void AttributeClearValuesWhenUnmodified()
        {
            EAVAttribute anAttribute = new EAVAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            EAVValue value = new EAVValue();
            anAttribute.Values.Add(value);
            value = new EAVValue();
            anAttribute.Values.Add(value);

            Assert.IsTrue(anAttribute.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.AreEqual(anAttribute, value.Attribute, "Property 'Attribute' was not set properly.");
            Assert.AreEqual(anAttribute.AttributeID, value.AttributeID, "Property 'AttributeID' was not reported properly.");
            Assert.AreEqual(ObjectState.New, anAttribute.ObjectState, "Object state should remain 'New' when property set.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anAttribute.Values.Clear();

            Assert.IsFalse(anAttribute.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.IsNull(value.Attribute, "Property 'Attribute' was not set properly.");
            Assert.IsNull(value.AttributeID, "Property 'AttributeID' was not reported properly.");
            Assert.IsFalse(anAttribute.Values.Any(), "Property 'Values' was not updated properly.");
            Assert.AreEqual(ObjectState.Modified, anAttribute.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void AttributeClearValuesWhenModified()
        {
            EAVAttribute anAttribute = new EAVAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVValue value = new EAVValue();
            anAttribute.Values.Add(value);
            value = new EAVValue();
            anAttribute.Values.Add(value);

            Assert.IsTrue(anAttribute.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.AreEqual(anAttribute, value.Attribute, "Property 'Attribute' was not set properly.");
            Assert.AreEqual(anAttribute.AttributeID, value.AttributeID, "Property 'AttributeID' was not reported properly.");
            Assert.AreEqual(ObjectState.Modified, anAttribute.ObjectState, "Object state failed to transition to 'Modified'.");

            anAttribute.Values.Clear();

            Assert.IsFalse(anAttribute.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.IsNull(value.Attribute, "Property 'Attribute' was not set properly.");
            Assert.IsNull(value.AttributeID, "Property 'AttributeID' was not reported properly.");
            Assert.IsFalse(anAttribute.Values.Any(), "Property 'Values' was not updated properly.");
            Assert.AreEqual(ObjectState.Modified, anAttribute.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void AttributeClearValuesWhenDeleted()
        {
            EAVAttribute anAttribute = new EAVAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            EAVValue value = new EAVValue() { Instance = new EAVRootInstance() { InstanceID = rng.Next() } };
            anAttribute.Values.Add(value);

            Assert.IsTrue(anAttribute.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.AreEqual(anAttribute, value.Attribute, "Property 'Attribute' was not set properly.");
            Assert.AreEqual(anAttribute.AttributeID, value.AttributeID, "Property 'AttributeID' was not reported properly.");
            Assert.AreEqual(ObjectState.New, anAttribute.ObjectState, "Object state should remain 'New' when property set.");

            anAttribute.MarkUnmodified();
            value.Instance.MarkUnmodified();
            value.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");
            Assert.AreEqual(ObjectState.Unmodified, value.Instance.ObjectState, "Object state failed to transition to 'Unmodified'.");
            Assert.AreEqual(ObjectState.Unmodified, value.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anAttribute.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, anAttribute.ObjectState, "Object state failed to transition to 'Deleted'.");
            Assert.AreEqual(ObjectState.Deleted, value.ObjectState, "Object state failed to transition to 'Deleted'.");

            anAttribute.Values.Clear();
        }
        #endregion
        #endregion
    }
}
