using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace EAVModelTest
{
    public partial class EAVModelTestHarness
    {
        private EAV.EAVDataType GetRandomDataType()
        {
            var types = Enum.GetValues(typeof(EAV.EAVDataType)).Cast<EAV.EAVDataType>().Where(it => it != default(EAV.EAVDataType));
            return (types.ElementAt(rng.Next(0, types.Count())));
        }

        [TestMethod]
        public void AttributeCreate()
        {
            EAVModelLibrary.ModelAttribute anAttribute = new EAVModelLibrary.ModelAttribute();

            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            Assert.IsNull(anAttribute.AttributeID, "Property 'AttributeID' should be null on creation.");

            Assert.IsNull(anAttribute.Name, "Property 'Name' should be null on creation.");
            Assert.IsNull(anAttribute.DataName, "Property 'DataName' should be null on creation.");
            Assert.IsNull(anAttribute.DisplayText, "Property 'DisplayText' should be null on creation.");
            Assert.AreEqual(default(EAV.EAVDataType), anAttribute.DataType, "Property 'DataType' should be default on creation.");
            Assert.AreEqual(0, anAttribute.Sequence, "Property 'Sequence' should be zero on creation.");
            Assert.IsFalse(anAttribute.IsKey, "Property 'IsKey' should be false on creation.");
            Assert.IsNull(anAttribute.VariableUnits, "Property 'VariableUnits' should be null on creation.");

            Assert.IsNull(anAttribute.Container, "Property 'Container' should be null on creation.");

            Assert.IsNotNull(anAttribute.Units, "Property 'Units' should not be null on creation.");
            Assert.IsFalse(anAttribute.Units.Any(), "Property 'Units' should be empty on creation.");

            Assert.IsNotNull(anAttribute.Values, "Property 'Values' should not be null on creation.");
            Assert.IsFalse(anAttribute.Values.Any(), "Property 'Values' should be empty on creation.");
        }

        #region State Transitions
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AttributeStateTransitionNewToUnmodifiedWithNullID()
        {
            EAVModelLibrary.ModelAttribute anAttribute = new EAVModelLibrary.ModelAttribute();

            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        public void AttributeStateTransitionNewToUnmodifiedWithValidID()
        {
            EAVModelLibrary.ModelAttribute anAttribute = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AttributeStateTransitionNewToDeleted()
        {
            EAVModelLibrary.ModelAttribute anAttribute = new EAVModelLibrary.ModelAttribute();

            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.MarkDeleted();
        }

        [TestMethod]
        public void AttributeStateTransitionUnmodifiedToDeleted()
        {
            EAVModelLibrary.ModelAttribute anAttribute = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anAttribute.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, anAttribute.ObjectState, "Object state failed to transition to 'Deleted'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AttributeStateTransitionDeletedToUnmodified()
        {
            EAVModelLibrary.ModelAttribute anAttribute = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anAttribute.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, anAttribute.ObjectState, "Object state failed to transition to 'Deleted'.");

            anAttribute.MarkUnmodified();
        }
        #endregion

        #region ID Property
        [TestMethod]
        public void AttributeSetIDWhenNew()
        {
            EAVModelLibrary.ModelAttribute anAttribute = new EAVModelLibrary.ModelAttribute();

            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            int id = rng.Next();
            anAttribute.AttributeID = id;

            Assert.AreEqual(id, anAttribute.AttributeID, "Property 'AttributeID' not properly set.");
            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state after setting property 'AttributeID' should be 'New'.");
        }

        [TestMethod]
        public void AttributeSetIDBeforeUnmodified()
        {
            EAVModelLibrary.ModelAttribute anAttribute = new EAVModelLibrary.ModelAttribute();

            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            int id = rng.Next();
            anAttribute.AttributeID = id;

            Assert.AreEqual(id, anAttribute.AttributeID, "Property 'AttributeID' not properly set.");
            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state after setting property 'AttributeID' should be 'New'.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AttributeSetIDAfterUnmodified()
        {
            EAVModelLibrary.ModelAttribute anAttribute = new EAVModelLibrary.ModelAttribute();

            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.MarkUnmodified();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AttributeSetIDWhenDeleted()
        {
            EAVModelLibrary.ModelAttribute anAttribute = new EAVModelLibrary.ModelAttribute();

            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            int id = rng.Next();
            anAttribute.AttributeID = id;

            Assert.AreEqual(id, anAttribute.AttributeID, "Property 'AttributeID' not properly set.");
            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state after setting property 'AttributeID' should be 'New'.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anAttribute.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            id = rng.Next();
            anAttribute.AttributeID = id;
        }
        #endregion

        #region Primitive Properties
        #region Name
        [TestMethod]
        public void AttributeSetNameWhenNew()
        {
            EAVModelLibrary.ModelAttribute anAttribute = new EAVModelLibrary.ModelAttribute();

            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            string value = Guid.NewGuid().ToString();
            anAttribute.Name = value;

            Assert.AreEqual(value, anAttribute.Name, "Property 'Name' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void AttributeSetNameWhenUnmodified()
        {
            EAVModelLibrary.ModelAttribute anAttribute = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            anAttribute.Name = value;

            Assert.AreEqual(value, anAttribute.Name, "Property 'Name' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, anAttribute.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void AttributeSetNameWhenModified()
        {
            EAVModelLibrary.ModelAttribute anAttribute = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            anAttribute.Name = value;

            Assert.AreEqual(value, anAttribute.Name, "Property 'Name' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, anAttribute.ObjectState, "Object state failed to transition to 'Modified'.");

            value = Guid.NewGuid().ToString();
            anAttribute.Name = value;

            Assert.AreEqual(value, anAttribute.Name, "Property 'Name' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, anAttribute.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AttributeSetNameWhenDeleted()
        {
            EAVModelLibrary.ModelAttribute anAttribute = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anAttribute.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, anAttribute.ObjectState, "Object state failed to transition to 'Deleted'.");

            anAttribute.Name = Guid.NewGuid().ToString();
        }
        #endregion

        #region DataName
        [TestMethod]
        public void AttributeSetDataNameWhenNew()
        {
            EAVModelLibrary.ModelAttribute anAttribute = new EAVModelLibrary.ModelAttribute();

            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            string value = Guid.NewGuid().ToString();
            anAttribute.DataName = value;

            Assert.AreEqual(value, anAttribute.DataName, "Property 'DataName' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void AttributeSetDataNameWhenUnmodified()
        {
            EAVModelLibrary.ModelAttribute anAttribute = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            anAttribute.DataName = value;

            Assert.AreEqual(value, anAttribute.DataName, "Property 'DataName' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, anAttribute.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void AttributeSetDataNameWhenModified()
        {
            EAVModelLibrary.ModelAttribute anAttribute = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            anAttribute.DataName = value;

            Assert.AreEqual(value, anAttribute.DataName, "Property 'DataName' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, anAttribute.ObjectState, "Object state failed to transition to 'Modified'.");

            value = Guid.NewGuid().ToString();
            anAttribute.DataName = value;

            Assert.AreEqual(value, anAttribute.DataName, "Property 'DataName' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, anAttribute.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AttributeSetDataNameWhenDeleted()
        {
            EAVModelLibrary.ModelAttribute anAttribute = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anAttribute.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, anAttribute.ObjectState, "Object state failed to transition to 'Deleted'.");

            anAttribute.DataName = Guid.NewGuid().ToString();
        }
        #endregion

        #region DisplayText
        [TestMethod]
        public void AttributeSetDisplayTextWhenNew()
        {
            EAVModelLibrary.ModelAttribute anAttribute = new EAVModelLibrary.ModelAttribute();

            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            string value = Guid.NewGuid().ToString();
            anAttribute.DisplayText = value;

            Assert.AreEqual(value, anAttribute.DisplayText, "Property 'DisplayText' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void AttributeSetDisplayTextWhenUnmodified()
        {
            EAVModelLibrary.ModelAttribute anAttribute = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            anAttribute.DisplayText = value;

            Assert.AreEqual(value, anAttribute.DisplayText, "Property 'DisplayText' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, anAttribute.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void AttributeSetDisplayTextWhenModified()
        {
            EAVModelLibrary.ModelAttribute anAttribute = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            anAttribute.DisplayText = value;

            Assert.AreEqual(value, anAttribute.DisplayText, "Property 'DisplayText' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, anAttribute.ObjectState, "Object state failed to transition to 'Modified'.");

            value = Guid.NewGuid().ToString();
            anAttribute.DisplayText = value;

            Assert.AreEqual(value, anAttribute.DisplayText, "Property 'DisplayText' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, anAttribute.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AttributeSetDisplayTextWhenDeleted()
        {
            EAVModelLibrary.ModelAttribute anAttribute = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anAttribute.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, anAttribute.ObjectState, "Object state failed to transition to 'Deleted'.");

            anAttribute.DisplayText = Guid.NewGuid().ToString();
        }
        #endregion

        #region DataType
        [TestMethod]
        public void AttributeSetDataTypeWhenNew()
        {
            EAVModelLibrary.ModelAttribute anAttribute = new EAVModelLibrary.ModelAttribute();

            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            EAV.EAVDataType dataType = GetRandomDataType();
            anAttribute.DataType = dataType;

            Assert.AreEqual(dataType, anAttribute.DataType, "Property 'DataType' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void AttributeSetDataTypeWhenUnmodified()
        {
            EAVModelLibrary.ModelAttribute anAttribute = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAV.EAVDataType dataType = GetRandomDataType();
            anAttribute.DataType = dataType;

            Assert.AreEqual(dataType, anAttribute.DataType, "Property 'DataType' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, anAttribute.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void AttributeSetDataTypeWhenModified()
        {
            EAVModelLibrary.ModelAttribute anAttribute = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAV.EAVDataType dataType = GetRandomDataType();
            anAttribute.DataType = dataType;

            Assert.AreEqual(dataType, anAttribute.DataType, "Property 'DataType' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, anAttribute.ObjectState, "Object state failed to transition to 'Modified'.");

            dataType = GetRandomDataType();
            anAttribute.DataType = dataType;

            Assert.AreEqual(dataType, anAttribute.DataType, "Property 'DataType' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, anAttribute.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AttributeSetDataTypeWhenDeleted()
        {
            EAVModelLibrary.ModelAttribute anAttribute = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anAttribute.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, anAttribute.ObjectState, "Object state failed to transition to 'Deleted'.");

            anAttribute.DataType = GetRandomDataType();
        }
        #endregion

        #region Sequence
        [TestMethod]
        public void AttributeSetSequenceWhenNew()
        {
            EAVModelLibrary.ModelAttribute anAttribute = new EAVModelLibrary.ModelAttribute();

            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            int value = rng.Next();
            anAttribute.Sequence = value;

            Assert.AreEqual(value, anAttribute.Sequence, "Property 'Sequence' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void AttributeSetSequenceWhenUnmodified()
        {
            EAVModelLibrary.ModelAttribute anAttribute = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            int value = rng.Next();
            anAttribute.Sequence = value;

            Assert.AreEqual(value, anAttribute.Sequence, "Property 'Sequence' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, anAttribute.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void AttributeSetSequenceWhenModified()
        {
            EAVModelLibrary.ModelAttribute anAttribute = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            int value = rng.Next();
            anAttribute.Sequence = value;

            Assert.AreEqual(value, anAttribute.Sequence, "Property 'Sequence' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, anAttribute.ObjectState, "Object state failed to transition to 'Modified'.");

            value = rng.Next();
            anAttribute.Sequence = value;

            Assert.AreEqual(value, anAttribute.Sequence, "Property 'Sequence' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, anAttribute.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AttributeSetSequenceWhenDeleted()
        {
            EAVModelLibrary.ModelAttribute anAttribute = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anAttribute.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, anAttribute.ObjectState, "Object state failed to transition to 'Deleted'.");

            anAttribute.Sequence = rng.Next();
        }
        #endregion

        #region IsKey
        [TestMethod]
        public void AttributeSetIsKeyWhenNew()
        {
            EAVModelLibrary.ModelAttribute anAttribute = new EAVModelLibrary.ModelAttribute();

            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.IsKey = true;

            Assert.AreEqual(true, anAttribute.IsKey, "Property 'IsKey' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void AttributeSetIsKeyWhenUnmodified()
        {
            EAVModelLibrary.ModelAttribute anAttribute = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anAttribute.IsKey = true;

            Assert.AreEqual(true, anAttribute.IsKey, "Property 'IsKey' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, anAttribute.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void AttributeSetIsKeyWhenModified()
        {
            EAVModelLibrary.ModelAttribute anAttribute = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anAttribute.IsKey = true;

            Assert.AreEqual(true, anAttribute.IsKey, "Property 'IsKey' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, anAttribute.ObjectState, "Object state failed to transition to 'Modified'.");

            anAttribute.IsKey = false;

            Assert.AreEqual(false, anAttribute.IsKey, "Property 'IsKey' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, anAttribute.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AttributeSetIsKeyWhenDeleted()
        {
            EAVModelLibrary.ModelAttribute anAttribute = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anAttribute.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, anAttribute.ObjectState, "Object state failed to transition to 'Deleted'.");

            anAttribute.IsKey = true;
        }
        #endregion

        #region VariableUnits
        [TestMethod]
        public void AttributeSetVariableUnitsWhenNew()
        {
            EAVModelLibrary.ModelAttribute anAttribute = new EAVModelLibrary.ModelAttribute();

            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.VariableUnits = true;

            Assert.AreEqual(true, anAttribute.VariableUnits, "Property 'VariableUnits' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void AttributeSetVariableUnitsWhenUnmodified()
        {
            EAVModelLibrary.ModelAttribute anAttribute = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anAttribute.VariableUnits = true;

            Assert.AreEqual(true, anAttribute.VariableUnits, "Property 'VariableUnits' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, anAttribute.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void AttributeSetVariableUnitsWhenModified()
        {
            EAVModelLibrary.ModelAttribute anAttribute = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anAttribute.VariableUnits = true;

            Assert.AreEqual(true, anAttribute.VariableUnits, "Property 'VariableUnits' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, anAttribute.ObjectState, "Object state failed to transition to 'Modified'.");

            anAttribute.VariableUnits = false;

            Assert.AreEqual(false, anAttribute.VariableUnits, "Property 'VariableUnits' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, anAttribute.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AttributeSetVariableUnitsWhenDeleted()
        {
            EAVModelLibrary.ModelAttribute anAttribute = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anAttribute.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, anAttribute.ObjectState, "Object state failed to transition to 'Deleted'.");

            anAttribute.VariableUnits = true;
        }
        #endregion
        #endregion

        #region Object Properties
        #region Container
        [TestMethod]
        public void AttributeSetContainerWhenNew()
        {
            EAVModelLibrary.ModelAttribute anAttribute = new EAVModelLibrary.ModelAttribute();

            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelRootContainer value = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };
            anAttribute.Container = value;

            Assert.AreEqual(value, anAttribute.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(value.ContainerID, anAttribute.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.IsTrue(value.Attributes.Contains(anAttribute), "Property 'Attributes' was not updated properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void AttributeSetContainerWhenUnmodified()
        {
            EAVModelLibrary.ModelAttribute anAttribute = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVModelLibrary.ModelRootContainer value = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };
            anAttribute.Container = value;

            Assert.AreEqual(value, anAttribute.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(value.ContainerID, anAttribute.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.IsTrue(value.Attributes.Contains(anAttribute), "Property 'Attributes' was not updated properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, anAttribute.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void AttributeSetContainerWhenModified()
        {
            EAVModelLibrary.ModelAttribute anAttribute = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVModelLibrary.ModelRootContainer value = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };
            anAttribute.Container = value;

            Assert.AreEqual(value, anAttribute.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(value.ContainerID, anAttribute.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.IsTrue(value.Attributes.Contains(anAttribute), "Property 'Attributes' was not updated properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, anAttribute.ObjectState, "Object state failed to transition to 'Modified'.");

            value = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };
            anAttribute.Container = value;

            Assert.AreEqual(value, anAttribute.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(value.ContainerID, anAttribute.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.IsTrue(value.Attributes.Contains(anAttribute), "Property 'Attributes' was not updated properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, anAttribute.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AttributeSetContainerWhenDeleted()
        {
            EAVModelLibrary.ModelAttribute anAttribute = new EAVModelLibrary.ModelAttribute();

            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anAttribute.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, anAttribute.ObjectState, "Object state failed to transition to 'Deleted'.");

            anAttribute.Container = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };
        }
        #endregion
        #endregion

        #region Collection Properties
        #region Units
        [TestMethod]
        public void AttributeAddToUnitsWhenNew()
        {
            EAVModelLibrary.ModelAttribute anAttribute = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next(), VariableUnits = false };

            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelUnit aUnit = new EAVModelLibrary.ModelUnit() { UnitID = rng.Next() };
            aUnit.MarkUnmodified();

            anAttribute.Units.Add(aUnit);

            Assert.IsTrue(anAttribute.Units.Contains(aUnit), "Property 'Units' was not updated properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should remain 'New' when property set.");
            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aUnit.ObjectState, "Unit object state should remain 'Unmodified' when aUnit added to attribute.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AttributeAddToUnitsWhenNewAndVariableUnitsNull()
        {
            EAVModelLibrary.ModelAttribute anAttribute = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next(), VariableUnits = null };

            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelUnit aUnit = new EAVModelLibrary.ModelUnit() { UnitID = rng.Next() };
            aUnit.MarkUnmodified();

            anAttribute.Units.Add(aUnit);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AttributeAddToUnitsWhenNewAndVariableUnitsTrue()
        {
            EAVModelLibrary.ModelAttribute anAttribute = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next(), VariableUnits = true };

            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelUnit aUnit = new EAVModelLibrary.ModelUnit() { UnitID = rng.Next() };
            aUnit.MarkUnmodified();

            anAttribute.Units.Add(aUnit);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AttributeAddToUnitsWhenNewAndUnitIsNew()
        {
            EAVModelLibrary.ModelAttribute anAttribute = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next(), VariableUnits = false };

            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelUnit aUnit = new EAVModelLibrary.ModelUnit() { UnitID = rng.Next() };

            anAttribute.Units.Add(aUnit);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AttributeAddToUnitsWhenNewAndUnitHasNoID()
        {
            EAVModelLibrary.ModelAttribute anAttribute = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next(), VariableUnits = false };

            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelUnit aUnit = new EAVModelLibrary.ModelUnit();
            aUnit.MarkUnmodified();

            anAttribute.Units.Add(aUnit);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AttributeAddToUnitsWhenNewAndUnitIsDeleted()
        {
            EAVModelLibrary.ModelAttribute anAttribute = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next(), VariableUnits = false };

            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelUnit aUnit = new EAVModelLibrary.ModelUnit() { UnitID = rng.Next() };
            aUnit.MarkUnmodified();
            aUnit.MarkDeleted();

            anAttribute.Units.Add(aUnit);
        }

        [TestMethod]
        public void AttributeAddToUnitsWhenUnmodified()
        {
            EAVModelLibrary.ModelAttribute anAttribute = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next(), VariableUnits = false };

            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVModelLibrary.ModelUnit aUnit = new EAVModelLibrary.ModelUnit() { UnitID = rng.Next() };
            aUnit.MarkUnmodified();

            anAttribute.Units.Add(aUnit);

            Assert.IsTrue(anAttribute.Units.Contains(aUnit), "Property 'Units' was not updated properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, anAttribute.ObjectState, "Object state failed to transition to 'Modified'.");
            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aUnit.ObjectState, "Unit object state should remain 'Unmodified' when aUnit added to attribute.");
        }

        [TestMethod]
        public void AttributeAddToUnitsWhenModified()
        {
            EAVModelLibrary.ModelAttribute anAttribute = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next(), VariableUnits = false };

            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVModelLibrary.ModelUnit aUnit = new EAVModelLibrary.ModelUnit() { UnitID = rng.Next() };
            aUnit.MarkUnmodified();

            anAttribute.Units.Add(aUnit);

            Assert.IsTrue(anAttribute.Units.Contains(aUnit), "Property 'Units' was not updated properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, anAttribute.ObjectState, "Object state failed to transition to 'Modified'.");
            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aUnit.ObjectState, "Unit object state should remain 'Unmodified' when aUnit added to attribute.");

            aUnit = new EAVModelLibrary.ModelUnit() { UnitID = rng.Next() };
            aUnit.MarkUnmodified();

            anAttribute.Units.Add(aUnit);

            Assert.IsTrue(anAttribute.Units.Contains(aUnit), "Property 'Units' was not updated properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, anAttribute.ObjectState, "Object state should remain 'Modified' when property set.");
            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aUnit.ObjectState, "Unit object state should remain 'Unmodified' when aUnit added to attribute.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void AttributeAddToUnitsWhenDeleted()
        {
            EAVModelLibrary.ModelAttribute anAttribute = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next(), VariableUnits = false };

            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anAttribute.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, anAttribute.ObjectState, "Object state failed to transition to 'Deleted'.");

            EAVModelLibrary.ModelUnit aUnit = new EAVModelLibrary.ModelUnit() { UnitID = rng.Next() };
            aUnit.MarkUnmodified();

            anAttribute.Units.Add(aUnit);
        }
        #endregion

        #region Values
        [TestMethod]
        public void AttributeAddToValuesWhenNew()
        {
            EAVModelLibrary.ModelAttribute anAttribute = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelValue value = new EAVModelLibrary.ModelValue();
            anAttribute.Values.Add(value);

            Assert.IsTrue(anAttribute.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.AreEqual(anAttribute, value.Attribute, "Property 'Attribute' was not set properly.");
            Assert.AreEqual(anAttribute.AttributeID, value.AttributeID, "Property 'AttributeID' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void AttributeAddToValuesWhenUnmodified()
        {
            EAVModelLibrary.ModelAttribute anAttribute = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVModelLibrary.ModelValue value = new EAVModelLibrary.ModelValue();
            anAttribute.Values.Add(value);

            Assert.IsTrue(anAttribute.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.AreEqual(anAttribute, value.Attribute, "Property 'Attribute' was not set properly.");
            Assert.AreEqual(anAttribute.AttributeID, value.AttributeID, "Property 'AttributeID' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, anAttribute.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void AttributeAddToValuesWhenModified()
        {
            EAVModelLibrary.ModelAttribute anAttribute = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVModelLibrary.ModelValue value = new EAVModelLibrary.ModelValue();
            anAttribute.Values.Add(value);

            Assert.IsTrue(anAttribute.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.AreEqual(anAttribute, value.Attribute, "Property 'Attribute' was not set properly.");
            Assert.AreEqual(anAttribute.AttributeID, value.AttributeID, "Property 'AttributeID' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, anAttribute.ObjectState, "Object state failed to transition to 'Modified'.");

            value = new EAVModelLibrary.ModelValue();
            anAttribute.Values.Add(value);

            Assert.IsTrue(anAttribute.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.AreEqual(anAttribute, value.Attribute, "Property 'Attribute' was not set properly.");
            Assert.AreEqual(anAttribute.AttributeID, value.AttributeID, "Property 'AttributeID' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, anAttribute.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void AttributeAddToValuesWhenDeleted()
        {
            EAVModelLibrary.ModelAttribute anAttribute = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anAttribute.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, anAttribute.ObjectState, "Object state failed to transition to 'Deleted'.");

            anAttribute.Values.Add(new EAVModelLibrary.ModelValue());
        }

        [TestMethod]
        public void AttributeRemoveFromValuesWhenNew()
        {
            EAVModelLibrary.ModelAttribute anAttribute = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelValue value = new EAVModelLibrary.ModelValue();
            anAttribute.Values.Add(value);
            value = new EAVModelLibrary.ModelValue();
            anAttribute.Values.Add(value);

            Assert.IsTrue(anAttribute.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.AreEqual(anAttribute, value.Attribute, "Property 'Attribute' was not set properly.");
            Assert.AreEqual(anAttribute.AttributeID, value.AttributeID, "Property 'AttributeID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should remain 'New' when property set.");

            anAttribute.Values.Remove(value);

            Assert.IsFalse(anAttribute.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.IsNull(value.Attribute, "Property 'Attribute' was not set properly.");
            Assert.IsNull(value.AttributeID, "Property 'AttributeID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void AttributeRemoveFromValuesWhenUnmodified()
        {
            EAVModelLibrary.ModelAttribute anAttribute = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelValue value = new EAVModelLibrary.ModelValue();
            anAttribute.Values.Add(value);
            value = new EAVModelLibrary.ModelValue();
            anAttribute.Values.Add(value);

            Assert.IsTrue(anAttribute.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.AreEqual(anAttribute, value.Attribute, "Property 'Attribute' was not set properly.");
            Assert.AreEqual(anAttribute.AttributeID, value.AttributeID, "Property 'AttributeID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should remain 'New' when property set.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anAttribute.Values.Remove(value);

            Assert.IsFalse(anAttribute.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.IsNull(value.Attribute, "Property 'Attribute' was not set properly.");
            Assert.IsNull(value.AttributeID, "Property 'AttributeID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, anAttribute.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void AttributeRemoveFromValuesWhenModified()
        {
            EAVModelLibrary.ModelAttribute anAttribute = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVModelLibrary.ModelValue value = new EAVModelLibrary.ModelValue();
            anAttribute.Values.Add(value);
            value = new EAVModelLibrary.ModelValue();
            anAttribute.Values.Add(value);

            Assert.IsTrue(anAttribute.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.AreEqual(anAttribute, value.Attribute, "Property 'Attribute' was not set properly.");
            Assert.AreEqual(anAttribute.AttributeID, value.AttributeID, "Property 'AttributeID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, anAttribute.ObjectState, "Object state failed to transition to 'Modified'.");

            anAttribute.Values.Remove(value);

            Assert.IsFalse(anAttribute.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.IsNull(value.Attribute, "Property 'Attribute' was not set properly.");
            Assert.IsNull(value.AttributeID, "Property 'AttributeID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, anAttribute.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void AttributeRemoveFromValuesWhenDeleted()
        {
            EAVModelLibrary.ModelAttribute anAttribute = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelValue value = new EAVModelLibrary.ModelValue() { Instance = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() } };
            anAttribute.Values.Add(value);

            Assert.IsTrue(anAttribute.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.AreEqual(anAttribute, value.Attribute, "Property 'Attribute' was not set properly.");
            Assert.AreEqual(anAttribute.AttributeID, value.AttributeID, "Property 'AttributeID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should remain 'New' when property set.");

            anAttribute.MarkUnmodified();
            value.Instance.MarkUnmodified();
            value.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");
            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, value.Instance.ObjectState, "Object state failed to transition to 'Unmodified'.");
            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, value.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anAttribute.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, anAttribute.ObjectState, "Object state failed to transition to 'Deleted'.");
            Assert.AreEqual(EAV.Model.ObjectState.Deleted, value.ObjectState, "Object state failed to transition to 'Deleted'.");

            anAttribute.Values.Remove(value);
        }

        [TestMethod]
        public void AttributeClearValuesWhenNew()
        {
            EAVModelLibrary.ModelAttribute anAttribute = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelValue value = new EAVModelLibrary.ModelValue();
            anAttribute.Values.Add(value);
            value = new EAVModelLibrary.ModelValue();
            anAttribute.Values.Add(value);

            Assert.IsTrue(anAttribute.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.AreEqual(anAttribute, value.Attribute, "Property 'Attribute' was not set properly.");
            Assert.AreEqual(anAttribute.AttributeID, value.AttributeID, "Property 'AttributeID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should remain 'New' when property set.");

            anAttribute.Values.Clear();

            Assert.IsFalse(anAttribute.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.IsNull(value.Attribute, "Property 'Attribute' was not set properly.");
            Assert.IsNull(value.AttributeID, "Property 'AttributeID' was not reported properly.");
            Assert.IsFalse(anAttribute.Values.Any(), "Property 'Values' was not updated properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void AttributeClearValuesWhenUnmodified()
        {
            EAVModelLibrary.ModelAttribute anAttribute = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelValue value = new EAVModelLibrary.ModelValue();
            anAttribute.Values.Add(value);
            value = new EAVModelLibrary.ModelValue();
            anAttribute.Values.Add(value);

            Assert.IsTrue(anAttribute.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.AreEqual(anAttribute, value.Attribute, "Property 'Attribute' was not set properly.");
            Assert.AreEqual(anAttribute.AttributeID, value.AttributeID, "Property 'AttributeID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should remain 'New' when property set.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anAttribute.Values.Clear();

            Assert.IsFalse(anAttribute.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.IsNull(value.Attribute, "Property 'Attribute' was not set properly.");
            Assert.IsNull(value.AttributeID, "Property 'AttributeID' was not reported properly.");
            Assert.IsFalse(anAttribute.Values.Any(), "Property 'Values' was not updated properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, anAttribute.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void AttributeClearValuesWhenModified()
        {
            EAVModelLibrary.ModelAttribute anAttribute = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVModelLibrary.ModelValue value = new EAVModelLibrary.ModelValue();
            anAttribute.Values.Add(value);
            value = new EAVModelLibrary.ModelValue();
            anAttribute.Values.Add(value);

            Assert.IsTrue(anAttribute.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.AreEqual(anAttribute, value.Attribute, "Property 'Attribute' was not set properly.");
            Assert.AreEqual(anAttribute.AttributeID, value.AttributeID, "Property 'AttributeID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, anAttribute.ObjectState, "Object state failed to transition to 'Modified'.");

            anAttribute.Values.Clear();

            Assert.IsFalse(anAttribute.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.IsNull(value.Attribute, "Property 'Attribute' was not set properly.");
            Assert.IsNull(value.AttributeID, "Property 'AttributeID' was not reported properly.");
            Assert.IsFalse(anAttribute.Values.Any(), "Property 'Values' was not updated properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, anAttribute.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void AttributeClearValuesWhenDeleted()
        {
            EAVModelLibrary.ModelAttribute anAttribute = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelValue value = new EAVModelLibrary.ModelValue() { Instance = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() } };
            anAttribute.Values.Add(value);

            Assert.IsTrue(anAttribute.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.AreEqual(anAttribute, value.Attribute, "Property 'Attribute' was not set properly.");
            Assert.AreEqual(anAttribute.AttributeID, value.AttributeID, "Property 'AttributeID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, anAttribute.ObjectState, "Object state should remain 'New' when property set.");

            anAttribute.MarkUnmodified();
            value.Instance.MarkUnmodified();
            value.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, anAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");
            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, value.Instance.ObjectState, "Object state failed to transition to 'Unmodified'.");
            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, value.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anAttribute.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, anAttribute.ObjectState, "Object state failed to transition to 'Deleted'.");
            Assert.AreEqual(EAV.Model.ObjectState.Deleted, value.ObjectState, "Object state failed to transition to 'Deleted'.");

            anAttribute.Values.Clear();
        }
        #endregion
        #endregion
    }
}
