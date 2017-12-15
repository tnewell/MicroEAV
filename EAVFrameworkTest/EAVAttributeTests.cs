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
            Array types = Enum.GetValues(typeof(EAVDataType));
            return ((EAVDataType) types.GetValue(rng.Next(0, types.Length)));
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
        #region Name
        [TestMethod]
        public void AttributeSetNameWhenNew()
        {
            EAVAttribute aAttribute = new EAVAttribute();

            Assert.AreEqual(ObjectState.New, aAttribute.ObjectState, "Object state should be 'New' on creation.");

            string value = Guid.NewGuid().ToString();
            aAttribute.Name = value;

            Assert.AreEqual(value, aAttribute.Name, "Property 'Name' was not set properly.");
            Assert.AreEqual(ObjectState.New, aAttribute.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void AttributeSetNameWhenUnmodified()
        {
            EAVAttribute aAttribute = new EAVAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aAttribute.ObjectState, "Object state should be 'New' on creation.");

            aAttribute.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            aAttribute.Name = value;

            Assert.AreEqual(value, aAttribute.Name, "Property 'Name' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aAttribute.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void AttributeSetNameWhenModified()
        {
            EAVAttribute aAttribute = new EAVAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aAttribute.ObjectState, "Object state should be 'New' on creation.");

            aAttribute.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            aAttribute.Name = value;

            Assert.AreEqual(value, aAttribute.Name, "Property 'Name' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aAttribute.ObjectState, "Object state failed to transition to 'Modified'.");

            value = Guid.NewGuid().ToString();
            aAttribute.Name = value;

            Assert.AreEqual(value, aAttribute.Name, "Property 'Name' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aAttribute.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AttributeSetNameWhenDeleted()
        {
            EAVAttribute aAttribute = new EAVAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aAttribute.ObjectState, "Object state should be 'New' on creation.");

            aAttribute.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aAttribute.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aAttribute.ObjectState, "Object state failed to transition to 'Deleted'.");

            aAttribute.Name = Guid.NewGuid().ToString();
        }
        #endregion

        #region DataName
        [TestMethod]
        public void AttributeSetDataNameWhenNew()
        {
            EAVAttribute aAttribute = new EAVAttribute();

            Assert.AreEqual(ObjectState.New, aAttribute.ObjectState, "Object state should be 'New' on creation.");

            string value = Guid.NewGuid().ToString();
            aAttribute.DataName = value;

            Assert.AreEqual(value, aAttribute.DataName, "Property 'DataName' was not set properly.");
            Assert.AreEqual(ObjectState.New, aAttribute.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void AttributeSetDataNameWhenUnmodified()
        {
            EAVAttribute aAttribute = new EAVAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aAttribute.ObjectState, "Object state should be 'New' on creation.");

            aAttribute.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            aAttribute.DataName = value;

            Assert.AreEqual(value, aAttribute.DataName, "Property 'DataName' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aAttribute.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void AttributeSetDataNameWhenModified()
        {
            EAVAttribute aAttribute = new EAVAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aAttribute.ObjectState, "Object state should be 'New' on creation.");

            aAttribute.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            aAttribute.DataName = value;

            Assert.AreEqual(value, aAttribute.DataName, "Property 'DataName' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aAttribute.ObjectState, "Object state failed to transition to 'Modified'.");

            value = Guid.NewGuid().ToString();
            aAttribute.DataName = value;

            Assert.AreEqual(value, aAttribute.DataName, "Property 'DataName' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aAttribute.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AttributeSetDataNameWhenDeleted()
        {
            EAVAttribute aAttribute = new EAVAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aAttribute.ObjectState, "Object state should be 'New' on creation.");

            aAttribute.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aAttribute.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aAttribute.ObjectState, "Object state failed to transition to 'Deleted'.");

            aAttribute.DataName = Guid.NewGuid().ToString();
        }
        #endregion

        #region DisplayText
        [TestMethod]
        public void AttributeSetDisplayTextWhenNew()
        {
            EAVAttribute aAttribute = new EAVAttribute();

            Assert.AreEqual(ObjectState.New, aAttribute.ObjectState, "Object state should be 'New' on creation.");

            string value = Guid.NewGuid().ToString();
            aAttribute.DisplayText = value;

            Assert.AreEqual(value, aAttribute.DisplayText, "Property 'DisplayText' was not set properly.");
            Assert.AreEqual(ObjectState.New, aAttribute.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void AttributeSetDisplayTextWhenUnmodified()
        {
            EAVAttribute aAttribute = new EAVAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aAttribute.ObjectState, "Object state should be 'New' on creation.");

            aAttribute.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            aAttribute.DisplayText = value;

            Assert.AreEqual(value, aAttribute.DisplayText, "Property 'DisplayText' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aAttribute.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void AttributeSetDisplayTextWhenModified()
        {
            EAVAttribute aAttribute = new EAVAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aAttribute.ObjectState, "Object state should be 'New' on creation.");

            aAttribute.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            aAttribute.DisplayText = value;

            Assert.AreEqual(value, aAttribute.DisplayText, "Property 'DisplayText' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aAttribute.ObjectState, "Object state failed to transition to 'Modified'.");

            value = Guid.NewGuid().ToString();
            aAttribute.DisplayText = value;

            Assert.AreEqual(value, aAttribute.DisplayText, "Property 'DisplayText' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aAttribute.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AttributeSetDisplayTextWhenDeleted()
        {
            EAVAttribute aAttribute = new EAVAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aAttribute.ObjectState, "Object state should be 'New' on creation.");

            aAttribute.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aAttribute.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aAttribute.ObjectState, "Object state failed to transition to 'Deleted'.");

            aAttribute.DisplayText = Guid.NewGuid().ToString();
        }
        #endregion

        #region DataType
        [TestMethod]
        public void AttributeSetDataTypeWhenNew()
        {
            EAVAttribute aAttribute = new EAVAttribute();

            Assert.AreEqual(ObjectState.New, aAttribute.ObjectState, "Object state should be 'New' on creation.");

            EAVDataType dataType = GetRandomDataType();
            aAttribute.DataType = dataType;

            Assert.AreEqual(dataType, aAttribute.DataType, "Property 'DataType' was not set properly.");
            Assert.AreEqual(ObjectState.New, aAttribute.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void AttributeSetDataTypeWhenUnmodified()
        {
            EAVAttribute aAttribute = new EAVAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aAttribute.ObjectState, "Object state should be 'New' on creation.");

            aAttribute.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVDataType dataType = GetRandomDataType();
            aAttribute.DataType = dataType;

            Assert.AreEqual(dataType, aAttribute.DataType, "Property 'DataType' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aAttribute.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void AttributeSetDataTypeWhenModified()
        {
            EAVAttribute aAttribute = new EAVAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aAttribute.ObjectState, "Object state should be 'New' on creation.");

            aAttribute.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVDataType dataType = GetRandomDataType();
            aAttribute.DataType = dataType;

            Assert.AreEqual(dataType, aAttribute.DataType, "Property 'DataType' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aAttribute.ObjectState, "Object state failed to transition to 'Modified'.");

            dataType = GetRandomDataType();
            aAttribute.DataType = dataType;

            Assert.AreEqual(dataType, aAttribute.DataType, "Property 'DataType' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aAttribute.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AttributeSetDataTypeWhenDeleted()
        {
            EAVAttribute aAttribute = new EAVAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aAttribute.ObjectState, "Object state should be 'New' on creation.");

            aAttribute.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aAttribute.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aAttribute.ObjectState, "Object state failed to transition to 'Deleted'.");

            aAttribute.DataType = GetRandomDataType();
        }
        #endregion

        #region IsKey
        [TestMethod]
        public void AttributeSetIsKeyWhenNew()
        {
            EAVAttribute aAttribute = new EAVAttribute();

            Assert.AreEqual(ObjectState.New, aAttribute.ObjectState, "Object state should be 'New' on creation.");

            aAttribute.IsKey = true;

            Assert.AreEqual(true, aAttribute.IsKey, "Property 'IsKey' was not set properly.");
            Assert.AreEqual(ObjectState.New, aAttribute.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void AttributeSetIsKeyWhenUnmodified()
        {
            EAVAttribute aAttribute = new EAVAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aAttribute.ObjectState, "Object state should be 'New' on creation.");

            aAttribute.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aAttribute.IsKey = true;

            Assert.AreEqual(true, aAttribute.IsKey, "Property 'IsKey' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aAttribute.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void AttributeSetIsKeyWhenModified()
        {
            EAVAttribute aAttribute = new EAVAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aAttribute.ObjectState, "Object state should be 'New' on creation.");

            aAttribute.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aAttribute.IsKey = true;

            Assert.AreEqual(true, aAttribute.IsKey, "Property 'IsKey' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aAttribute.ObjectState, "Object state failed to transition to 'Modified'.");

            aAttribute.IsKey = false;

            Assert.AreEqual(false, aAttribute.IsKey, "Property 'IsKey' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aAttribute.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void AttributeSetIsKeyWhenDeleted()
        {
            EAVAttribute aAttribute = new EAVAttribute() { AttributeID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aAttribute.ObjectState, "Object state should be 'New' on creation.");

            aAttribute.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aAttribute.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aAttribute.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aAttribute.ObjectState, "Object state failed to transition to 'Deleted'.");

            aAttribute.IsKey = true;
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
