using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using EAV.Model;

using EAVFramework.Model;


namespace EAVFrameworkTest
{
    public partial class EAVFrameworkTestHarness
    {
        #region Object State Tests
        [TestMethod]
        public void CreateAttribute()
        {
            EAVAttribute anAttribute = new EAVAttribute();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            Assert.IsNull(anAttribute.AttributeID, "Property 'AttributeID' is not 'null' on creation.");

            Assert.IsNull(anAttribute.Name, "Property 'Name' is not 'null' on creation.");
            Assert.IsNull(anAttribute.DataName, "Property 'DataName' is not 'null' on creation.");
            Assert.IsNull(anAttribute.DisplayText, "Property 'DisplayText' is not 'null' on creation.");
            Assert.IsFalse(anAttribute.IsKey, "Property 'IsKey' is not 'false' on creation.");
            Assert.AreEqual(default(EAVDataType), anAttribute.DataType, "Property 'DataType' is not default value on creation.");

            Assert.IsNull(anAttribute.Container, "Property 'Container' is not 'null' on creation.");

            Assert.IsNotNull(anAttribute.Values, "Property 'Values' is null on creation.");
            Assert.IsTrue(anAttribute.Values.Count == 0, "Property 'Values' is not empty on creation.");
        }

        [TestMethod]
        public void SetAttributeUnmodifiedFromNew()
        {
            EAVAttribute anAttribute = new EAVAttribute();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Unmodified, "Object state not properly set to 'Unmodified'.");
        }

        [TestMethod]
        public void SetAttributeModifiedFromNew()
        {
            EAVAttribute anAttribute = new EAVAttribute();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            string name = Guid.NewGuid().ToString();
            anAttribute.Name = name;

            Assert.AreEqual(name, anAttribute.Name, "Property 'Name' not properly set.");

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.New, "Setting property in 'New' state should not alter object state.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SetAttributeDeletedFromNew()
        {
            EAVAttribute anAttribute = new EAVAttribute();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            anAttribute.MarkDeleted();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.New, "Setting object state to 'Deleted' from 'New' state should not alter object state.");
        }

        [TestMethod]
        public void SetAttributeModifiedFromUnmodified()
        {
            EAVAttribute anAttribute = new EAVAttribute();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Unmodified, "Object state not properly set to 'Unmodified'.");

            string name = Guid.NewGuid().ToString();
            anAttribute.Name = name;

            Assert.AreEqual(name, anAttribute.Name, "Property 'Name' not properly set.");
            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Modified, "Setting property should alter object state to 'Modified'.");
        }

        [TestMethod]
        public void SetAttributeDeletedFromUnmodified()
        {
            EAVAttribute anAttribute = new EAVAttribute();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Unmodified, "Object state not properly set to 'Unmodified'.");

            anAttribute.MarkDeleted();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Deleted, "Object state not properly set to 'Deleted'.");
        }

        [TestMethod]
        public void SetAttributeUnmodifiedFromModified()
        {
            EAVAttribute anAttribute = new EAVAttribute();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Unmodified, "Object state not properly set to 'Unmodified'.");

            string name = Guid.NewGuid().ToString();
            anAttribute.Name = name;

            Assert.AreEqual(name, anAttribute.Name, "Property 'Name' not properly set.");
            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Modified, "Setting property should alter object state to 'Modified'.");

            anAttribute.MarkUnmodified();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Unmodified, "Object state not properly set to 'Unmodified'.");
        }

        [TestMethod]
        public void SetAttributeDeletedFromModified()
        {
            EAVAttribute anAttribute = new EAVAttribute();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Unmodified, "Object state not properly set to 'Unmodified'.");

            string name = Guid.NewGuid().ToString();
            anAttribute.Name = name;

            Assert.AreEqual(name, anAttribute.Name, "Property 'Name' not properly set.");
            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Modified, "Setting property should alter object state to 'Modified'.");

            anAttribute.MarkDeleted();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Deleted, "Object state not properly set to 'Deleted'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SetAttributeUnmodifiedFromDeleted()
        {
            EAVAttribute anAttribute = new EAVAttribute();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Unmodified, "Object state not properly set to 'Unmodified'.");

            anAttribute.MarkDeleted();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Deleted, "Object state not properly set to 'Deleted'.");

            anAttribute.MarkUnmodified();
        }

        [TestMethod]
        public void SetAttributeModifiedFromDeleted()
        {
            EAVAttribute anAttribute = new EAVAttribute();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Unmodified, "Object state not properly set to 'Unmodified'.");

            anAttribute.MarkDeleted();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Deleted, "Object state not properly set to 'Deleted'.");

            anAttribute.Name = Guid.NewGuid().ToString();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Deleted, "Setting property should not alter object state from 'Deleted'.");
        }
        #endregion

        #region Attribute ID Property Tests
        [TestMethod]
        public void TestAttributeIDForNewAttribute()
        {
            EAVAttribute anAttribute = new EAVAttribute();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            int id = rng.Next();
            anAttribute.AttributeID = id;

            Assert.AreEqual(id, anAttribute.AttributeID, "Property 'AttributeID' was not set correctly.");
            Assert.IsTrue(anAttribute.ObjectState == ObjectState.New, "Setting property in 'New' state should not alter object state.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestAttributeIDForUnmodifiedAttribute()
        {
            EAVAttribute anAttribute = new EAVAttribute();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            int id = rng.Next();
            anAttribute.AttributeID = id;

            Assert.AreEqual(id, anAttribute.AttributeID, "Property 'AttributeID' was not set correctly.");
            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Unmodified, "Setting null property 'AttributeID' in 'Unmodified' state should not alter object state.");

            anAttribute.AttributeID = id;
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestAttributeIDForModifiedAttribute()
        {
            EAVAttribute anAttribute = new EAVAttribute();

            int id = rng.Next();
            anAttribute.AttributeID = id;

            Assert.AreEqual(id, anAttribute.AttributeID, "Property 'AttributeID' was not set correctly.");

            anAttribute.MarkUnmodified();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            string name = Guid.NewGuid().ToString();
            anAttribute.Name = name;

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Modified, "Object was not correctly marked 'Modified'.");

            anAttribute.AttributeID = id;
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestAttributeIDForDeletedAttribute()
        {
            EAVAttribute anAttribute = new EAVAttribute();

            anAttribute.MarkUnmodified();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            anAttribute.MarkDeleted();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Deleted, "Object was not correctly marked 'Deleted'.");

            anAttribute.AttributeID = rng.Next();
        }
        #endregion

        #region Name Property Tests
        [TestMethod]
        public void TestNameForNewAttribute()
        {
            EAVAttribute anAttribute = new EAVAttribute();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            string name = Guid.NewGuid().ToString();
            anAttribute.Name = name;

            Assert.AreEqual(name, anAttribute.Name, "Property 'Name' was not set correctly.");
            Assert.IsTrue(anAttribute.ObjectState == ObjectState.New, "Setting property in 'New' state should not alter object state.");
        }

        [TestMethod]
        public void TestNameForUnmodifiedAttribute()
        {
            EAVAttribute anAttribute = new EAVAttribute();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            string name = Guid.NewGuid().ToString();
            anAttribute.Name = name;

            Assert.AreEqual(name, anAttribute.Name, "Property 'Name' was not set correctly.");
            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Modified, "Setting property in 'Unmodified' state should alter state to 'Modified'.");
        }

        [TestMethod]
        public void TestNameForModifiedAttribute()
        {
            EAVAttribute anAttribute = new EAVAttribute();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            string name = Guid.NewGuid().ToString();
            anAttribute.Name = name;

            Assert.AreEqual(name, anAttribute.Name, "Property 'Name' was not set correctly.");
            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Modified, "Setting property in 'Unmodified' state should alter state to 'Modified'.");

            string oldName = name;
            name = Guid.NewGuid().ToString();

            Assert.AreNotEqual(oldName, name, "New value selected for name is the same as the old value.");

            anAttribute.Name = name;

            Assert.AreEqual(name, anAttribute.Name, "Property 'Name' was not set correctly.");
            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Modified, "Setting property in 'Modified' state should not alter object state.");
        }

        [TestMethod]
        public void TestNameForDeletedAttribute()
        {
            EAVAttribute anAttribute = new EAVAttribute();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            anAttribute.MarkDeleted();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Deleted, "Object was not correctly marked 'Deleted'.");

            anAttribute.Name = Guid.NewGuid().ToString();

            Assert.IsNull(anAttribute.Name, "Setting property in 'Deleted' state should not alter property value.");
            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Deleted, "Setting property in 'Deleted' state should not alter object state.");
        }
        #endregion

        #region DataName Property Tests
        [TestMethod]
        public void TestDataNameForNewAttribute()
        {
            EAVAttribute anAttribute = new EAVAttribute();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            string dataName = Guid.NewGuid().ToString();
            anAttribute.DataName = dataName;

            Assert.AreEqual(dataName, anAttribute.DataName, "Property 'DataName' was not set correctly.");
            Assert.IsTrue(anAttribute.ObjectState == ObjectState.New, "Setting property in 'New' state should not alter object state.");
        }

        [TestMethod]
        public void TestDataNameForUnmodifiedAttribute()
        {
            EAVAttribute anAttribute = new EAVAttribute();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            string dataName = Guid.NewGuid().ToString();
            anAttribute.DataName = dataName;

            Assert.AreEqual(dataName, anAttribute.DataName, "Property 'DataName' was not set correctly.");
            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Modified, "Setting property in 'Unmodified' state should alter state to 'Modified'.");
        }

        [TestMethod]
        public void TestDataNameForModifiedAttribute()
        {
            EAVAttribute anAttribute = new EAVAttribute();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            string dataName = Guid.NewGuid().ToString();
            anAttribute.DataName = dataName;

            Assert.AreEqual(dataName, anAttribute.DataName, "Property 'DataName' was not set correctly.");
            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Modified, "Setting property in 'Unmodified' state should alter state to 'Modified'.");

            string oldDataName = dataName;
            dataName = Guid.NewGuid().ToString();

            Assert.AreNotEqual(oldDataName, dataName, "New value selected for name is the same as the old value.");

            anAttribute.DataName = dataName;

            Assert.AreEqual(dataName, anAttribute.DataName, "Property 'DataName' was not set correctly.");
            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Modified, "Setting property in 'Modified' state should not alter object state.");
        }

        [TestMethod]
        public void TestDataNameForDeletedAttribute()
        {
            EAVAttribute anAttribute = new EAVAttribute();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            anAttribute.MarkDeleted();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Deleted, "Object was not correctly marked 'Deleted'.");

            anAttribute.DataName = Guid.NewGuid().ToString();

            Assert.IsNull(anAttribute.DataName, "Setting property in 'Deleted' state should not alter property value.");
            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Deleted, "Setting property in 'Deleted' state should not alter object state.");
        }
        #endregion

        #region DisplayText Property Tests
        [TestMethod]
        public void TestDisplayTextForNewAttribute()
        {
            EAVAttribute anAttribute = new EAVAttribute();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            string displayText = Guid.NewGuid().ToString();
            anAttribute.DisplayText = displayText;

            Assert.AreEqual(displayText, anAttribute.DisplayText, "Property 'DisplayText' was not set correctly.");
            Assert.IsTrue(anAttribute.ObjectState == ObjectState.New, "Setting property in 'New' state should not alter object state.");
        }

        [TestMethod]
        public void TestDisplayTextForUnmodifiedAttribute()
        {
            EAVAttribute anAttribute = new EAVAttribute();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            string displayText = Guid.NewGuid().ToString();
            anAttribute.DisplayText = displayText;

            Assert.AreEqual(displayText, anAttribute.DisplayText, "Property 'DisplayText' was not set correctly.");
            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Modified, "Setting property in 'Unmodified' state should alter state to 'Modified'.");
        }

        [TestMethod]
        public void TestDisplayTextForModifiedAttribute()
        {
            EAVAttribute anAttribute = new EAVAttribute();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            string displayText = Guid.NewGuid().ToString();
            anAttribute.DisplayText = displayText;

            Assert.AreEqual(displayText, anAttribute.DisplayText, "Property 'DisplayText' was not set correctly.");
            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Modified, "Setting property in 'Unmodified' state should alter state to 'Modified'.");

            string oldDisplayText = displayText;
            displayText = Guid.NewGuid().ToString();

            Assert.AreNotEqual(oldDisplayText, displayText, "New value selected for name is the same as the old value.");

            anAttribute.DisplayText = displayText;

            Assert.AreEqual(displayText, anAttribute.DisplayText, "Property 'DisplayText' was not set correctly.");
            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Modified, "Setting property in 'Modified' state should not alter object state.");
        }

        [TestMethod]
        public void TestDisplayTextForDeletedAttribute()
        {
            EAVAttribute anAttribute = new EAVAttribute();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            anAttribute.MarkDeleted();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Deleted, "Object was not correctly marked 'Deleted'.");

            anAttribute.DisplayText= Guid.NewGuid().ToString();

            Assert.IsNull(anAttribute.DisplayText, "Setting property in 'Deleted' state should not alter property value.");
            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Deleted, "Setting property in 'Deleted' state should not alter object state.");
        }
        #endregion

        #region IsKey Property Tests
        [TestMethod]
        public void TestIsKeyForNewAttribute()
        {
            EAVAttribute anAttribute = new EAVAttribute();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            anAttribute.IsKey = true;

            Assert.AreEqual(true, anAttribute.IsKey, "Property 'IsKey' was not set correctly.");
            Assert.IsTrue(anAttribute.ObjectState == ObjectState.New, "Setting property in 'New' state should not alter object state.");
        }

        [TestMethod]
        public void TestIsKeyForUnmodifiedAttribute()
        {
            EAVAttribute anAttribute = new EAVAttribute();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            anAttribute.IsKey = true;

            Assert.AreEqual(true, anAttribute.IsKey, "Property 'IsKey' was not set correctly.");
            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Modified, "Setting property in 'Unmodified' state should alter state to 'Modified'.");
        }

        [TestMethod]
        public void TestIsKeyForModifiedAttribute()
        {
            EAVAttribute anAttribute = new EAVAttribute();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            anAttribute.IsKey = true;

            Assert.AreEqual(true, anAttribute.IsKey, "Property 'IsKey' was not set correctly.");
            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Modified, "Setting property in 'Unmodified' state should alter state to 'Modified'.");

            anAttribute.IsKey = false;

            Assert.AreEqual(false, anAttribute.IsKey, "Property 'IsKey' was not set correctly.");
            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Modified, "Setting property in 'Modified' state should not alter object state.");
        }

        [TestMethod]
        public void TestIsKeyForDeletedAttribute()
        {
            EAVAttribute anAttribute = new EAVAttribute();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            anAttribute.MarkDeleted();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Deleted, "Object was not correctly marked 'Deleted'.");

            anAttribute.IsKey = true;

            Assert.IsFalse(anAttribute.IsKey, "Setting property in 'Deleted' state should not alter property value.");
            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Deleted, "Setting property in 'Deleted' state should not alter object state.");
        }
        #endregion

        #region DataType Property Tests
        [TestMethod]
        public void TestDataTypeForNewAttribute()
        {
            EAVAttribute anAttribute = new EAVAttribute();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            anAttribute.DataType = EAVDataType.Float;

            Assert.AreEqual(EAVDataType.Float, anAttribute.DataType, "Property 'DataType' was not set correctly.");
            Assert.IsTrue(anAttribute.ObjectState == ObjectState.New, "Setting property in 'New' state should not alter object state.");
        }

        [TestMethod]
        public void TestDataTypeForUnmodifiedAttribute()
        {
            EAVAttribute anAttribute = new EAVAttribute();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            anAttribute.DataType = EAVDataType.Float;

            Assert.AreEqual(EAVDataType.Float, anAttribute.DataType, "Property 'DataType' was not set correctly.");
            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Modified, "Setting property in 'Unmodified' state should alter state to 'Modified'.");
        }

        [TestMethod]
        public void TestDataTypeForModifiedAttribute()
        {
            EAVAttribute anAttribute = new EAVAttribute();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            anAttribute.DataType = EAVDataType.Float;

            Assert.AreEqual(EAVDataType.Float, anAttribute.DataType, "Property 'DataType' was not set correctly.");
            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Modified, "Setting property in 'Unmodified' state should alter state to 'Modified'.");

            anAttribute.DataType = EAVDataType.Boolean;

            Assert.AreEqual(EAVDataType.Boolean, anAttribute.DataType, "Property 'DataType' was not set correctly.");
            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Modified, "Setting property in 'Modified' state should not alter object state.");
        }

        [TestMethod]
        public void TestDataTypeForDeletedAttribute()
        {
            EAVAttribute anAttribute = new EAVAttribute();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            anAttribute.MarkDeleted();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Deleted, "Object was not correctly marked 'Deleted'.");

            anAttribute.DataType = EAVDataType.Float;

            Assert.AreNotEqual(EAVDataType.Float, anAttribute.DataType, "Setting property in 'Deleted' state should not alter property value.");
            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Deleted, "Setting property in 'Deleted' state should not alter object state.");
        }
        #endregion

        #region Container Property Tests
        [TestMethod]
        public void TestContainerForNewAttribute()
        {
            EAVAttribute anAttribute = new EAVAttribute();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            EAVRootContainer aContainer = new EAVRootContainer();
            anAttribute.Container = aContainer;

            Assert.AreEqual(aContainer, anAttribute.Container, "Property 'Container' was not set correctly.");
            Assert.IsTrue(anAttribute.ObjectState == ObjectState.New, "Setting property in 'New' state should not alter object state.");
        }

        [TestMethod]
        public void TestContainerForUnmodifiedAttribute()
        {
            EAVAttribute anAttribute = new EAVAttribute();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            EAVRootContainer aContainer = new EAVRootContainer();
            anAttribute.Container = aContainer;

            Assert.AreEqual(aContainer, anAttribute.Container, "Property 'Container' was not set correctly.");
            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Modified, "Setting property in 'Unmodified' state should alter state to 'Modified'.");
        }

        [TestMethod]
        public void TestContainerForModifiedAttribute()
        {
            EAVAttribute anAttribute = new EAVAttribute();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            EAVRootContainer aContainer = new EAVRootContainer();
            anAttribute.Container = aContainer;

            Assert.AreEqual(aContainer, anAttribute.Container, "Property 'Container' was not set correctly.");
            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Modified, "Setting property in 'Unmodified' state should alter state to 'Modified'.");

            aContainer = new EAVRootContainer();
            anAttribute.Container = aContainer;

            Assert.AreEqual(aContainer, anAttribute.Container, "Property 'Container' was not set correctly.");
            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Modified, "Setting property in 'Modified' state should not alter object state.");
        }

        [TestMethod]
        public void TestContainerForDeletedAttribute()
        {
            EAVAttribute anAttribute = new EAVAttribute();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            anAttribute.MarkDeleted();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Deleted, "Object was not correctly marked 'Deleted'.");

            EAVRootContainer aContainer = new EAVRootContainer();
            anAttribute.Container = aContainer;

            Assert.AreNotEqual(aContainer, anAttribute.Container, "Setting property in 'Deleted' state should not alter property value.");
            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Deleted, "Setting property in 'Deleted' state should not alter object state.");
        }

        [TestMethod]
        public void TestContainerInteractionWithAttribute()
        {
            int attributeID = rng.Next();
            EAVAttribute anAttribute = new EAVAttribute() { AttributeID = attributeID };
            int containerID = rng.Next();
            EAVRootContainer aContainer = new EAVRootContainer() { ContainerID = containerID };

            Assert.AreEqual(attributeID, anAttribute.AttributeID, "Property 'AttributeID' was not set properly.");
            Assert.AreEqual(containerID, aContainer.ContainerID, "Property 'ContainerID' was not set properly.");

            Assert.IsNull(anAttribute.Container, "Property 'Container' should be null.");
            Assert.IsTrue(aContainer.Attributes.Count == 0, "Collection property 'Attributes' of EAVRootContainer object should be empty.");

            anAttribute.Container = aContainer;

            Assert.AreEqual(aContainer, anAttribute.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(containerID, anAttribute.Container.ContainerID, "Property 'ContainerID' was not set properly.");

            Assert.IsTrue(aContainer.Attributes.Count == 1, "Collection property 'Attributes' was not set properly.");
            Assert.AreEqual(attributeID, aContainer.Attributes.First().AttributeID, "Property 'AttributeID' was not set properly.");

            attributeID = rng.Next();
            anAttribute = new EAVAttribute() { AttributeID = attributeID };
            containerID = rng.Next();
            aContainer = new EAVRootContainer() { ContainerID = containerID };

            aContainer.Attributes.Add(anAttribute);

            Assert.AreEqual(aContainer, anAttribute.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(containerID, anAttribute.Container.ContainerID, "Property 'ContainerID' was not set properly.");

            Assert.IsTrue(aContainer.Attributes.Count == 1, "Collection property 'Attributes' was not set properly.");
            Assert.AreEqual(attributeID, aContainer.Attributes.First().AttributeID, "Property 'AttributeID' was not set properly.");
        }
        #endregion

        #region Values Property Tests
        [TestMethod]
        public void TestValueForNewAttribute()
        {
            EAVAttribute anAttribute = new EAVAttribute();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            anAttribute.Values.Add(new EAVValue());
            anAttribute.Values.Add(new EAVValue());

            Assert.IsTrue(anAttribute.Values.Count == 2, "Property 'Values' was not set correctly.");
            Assert.IsTrue(anAttribute.ObjectState == ObjectState.New, "Modifying property in 'New' state should not alter object state.");

            anAttribute.Values.Remove(anAttribute.Values.First());

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.New, "Modifying property in 'New' state should not alter object state.");

            anAttribute.Values.Clear();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.New, "Modifying property in 'New' state should not alter object state.");
        }

        [TestMethod]
        public void TestValueForUnmodifiedAttribute()
        {
            EAVAttribute anAttribute = new EAVAttribute();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            anAttribute.Values.Add(new EAVValue());
            anAttribute.Values.Add(new EAVValue());

            Assert.IsTrue(anAttribute.Values.Count == 2, "Property 'Values' was not set correctly.");
            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Modified, "Modifying property in 'Unmodified' state should alter object state to 'Modified'.");

            anAttribute.MarkUnmodified();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            anAttribute.Values.Remove(anAttribute.Values.First());

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Modified, "Modifying property in 'Unmodified' state should alter object state to 'Modified'.");

            anAttribute.MarkUnmodified();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            anAttribute.Values.Clear();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Modified, "Modifying property in 'Unmodified' state should alter object state to 'Modified'.");
        }

        [TestMethod]
        public void TestValueForModifiedAttribute()
        {
            EAVAttribute anAttribute = new EAVAttribute();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            anAttribute.Name = Guid.NewGuid().ToString();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Modified, "Object was not correctly marked 'Modified'.");

            anAttribute.Values.Add(new EAVValue());
            anAttribute.Values.Add(new EAVValue());

            Assert.IsTrue(anAttribute.Values.Count == 2, "Property 'Values' was not set correctly.");
            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Modified, "Modifying property in 'Modified' state should not alter object state.");

            anAttribute.Values.Remove(anAttribute.Values.First());

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Modified, "Modifying property in 'Modified' state should not alter object state.");

            anAttribute.Values.Clear();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Modified, "Modifying property in 'Modified' state should not alter object state.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestValueForDeletedAttributeWithAdd()
        {
            EAVAttribute anAttribute = new EAVAttribute();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            anAttribute.MarkUnmodified();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            anAttribute.MarkDeleted();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Deleted, "Object was not correctly marked 'Deleted'.");

            anAttribute.Values.Add(new EAVValue());
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestValueForDeletedAttributeWithRemove()
        {
            EAVAttribute anAttribute = new EAVAttribute();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            anAttribute.Values.Add(new EAVValue() { Instance = new EAVRootInstance() });
            anAttribute.Values.Add(new EAVValue() { Instance = new EAVRootInstance() });

            anAttribute.MarkUnmodified();

            foreach (EAVValue value in anAttribute.Values)
            {
                value.Instance.MarkUnmodified();
                value.MarkUnmodified();
            }

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            anAttribute.MarkDeleted();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Deleted, "Object was not correctly marked 'Deleted'.");

            anAttribute.Values.Remove(anAttribute.Values.First());
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestValueForDeletedAttributeWithClear()
        {
            EAVAttribute anAttribute = new EAVAttribute();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            anAttribute.Values.Add(new EAVValue() { Instance = new EAVRootInstance() });

            anAttribute.MarkUnmodified();

            foreach (EAVValue value in anAttribute.Values)
            {
                value.Instance.MarkUnmodified();
                value.MarkUnmodified();
            }

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            anAttribute.MarkDeleted();

            Assert.IsTrue(anAttribute.ObjectState == ObjectState.Deleted, "Object was not correctly marked 'Deleted'.");

            anAttribute.Values.Clear();
        }

        [TestMethod]
        public void TestValueInteractionWithAttribute()
        {
            EAVAttribute anAttribute = new EAVAttribute();

            int id = rng.Next();
            anAttribute.AttributeID = id;

            EAVValue aValue = new EAVValue();
            anAttribute.Values.Add(aValue);

            Assert.IsTrue(anAttribute.Values.Count == 1, "Collection property 'Values' is empty.");
            Assert.IsTrue(anAttribute.Values.Single() == aValue, "Collection property 'Values' not modified properly.");

            Assert.IsNotNull(aValue.Attribute, "Property 'Attribute' is null.");
            Assert.AreEqual(anAttribute, aValue.Attribute, "Property 'Attribute' not set properly.");

            Assert.IsNotNull(aValue.AttributeID, "Propert 'AttributeID' is null.");
            Assert.AreEqual(id, aValue.AttributeID, "Property 'AttributeID' not set properly.");
        }
        #endregion
    }
}
