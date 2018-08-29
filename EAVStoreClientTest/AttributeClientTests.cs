using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace EAVStoreClientTestHarness
{
    public partial class EAVStoreClientTestHarness
    {
        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Retrieve")]
        [TestCategory("Attribute")]
        public void RetrieveAllAttributes()
        {
            EAVStoreClient.EAVAttributeClient client = new EAVStoreClient.EAVAttributeClient();

            int nDbAttributes = this.DbContext.Attributes.Count();
            int nClientAttributes = client.RetrieveAttributes(null).Count();

            Assert.AreEqual(nDbAttributes, nClientAttributes, "The number of attributes retrieved by the client does not match the number in the database.");
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Retrieve")]
        [TestCategory("Attribute")]
        public void RetrieveNonExistentAttribute()
        {
            EAVStoreClient.EAVAttributeClient client = new EAVStoreClient.EAVAttributeClient();

            var attribute = client.RetrieveAttribute(-1);

            Assert.IsNull(attribute, "Unexpected attribute object retrieved.");
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Retrieve")]
        [TestCategory("Attribute")]
        public void RetrieveRandomAttribute()
        {
            var dbAttribute = SelectRandomItem(this.DbContext.Attributes);

            if (dbAttribute != null)
            {
                EAVStoreClient.EAVAttributeClient client = new EAVStoreClient.EAVAttributeClient();

                var attribute = client.RetrieveAttribute(dbAttribute.Attribute_ID);

                Assert.IsNotNull(attribute, "Failed to retrieve attribute {0}.", dbAttribute.Attribute_ID);
                Assert.AreEqual(dbAttribute.Attribute_ID, attribute.AttributeID, "Attribute ID values do not match.");
            }
            else
            {
                Assert.Inconclusive("No attributes were found in the database.");
            }
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Create")]
        [TestCategory("Attribute")]
        public void CreateAttribute()
        {
            EAVStoreClient.EAVAttributeClient client = new EAVStoreClient.EAVAttributeClient();
            int containerID = SelectRandomItem(this.DbContext.Containers).Context_ID;
            string attributeName = Guid.NewGuid().ToString();

            EAV.Store.IStoreAttribute attribute = client.CreateAttribute(new EAV.Store.StoreAttribute()
            {
                Name = attributeName,
                DataName = attributeName.ToUpper(),
                DisplayText = attributeName + ":",
                DataType = EAV.EAVDataType.String,
                IsKey = true,
                Sequence = rng.Next(),
                VariableUnits = true,
            }, containerID);

            Assert.IsNotNull(attribute, "Failed to create attribute with name '{0}' for container ID {1}.", attributeName, containerID);

            ResetDatabaseContext();

            var dbAttribute = this.DbContext.Attributes.SingleOrDefault(it => it.Attribute_ID == attribute.AttributeID);

            Assert.IsNotNull(dbAttribute, String.Format("Failed to retrieve attribute ID {0} from the database.", attribute.AttributeID));

            Assert.AreEqual(attribute.Name, dbAttribute.Name, "Property 'Name' was not created correctly.");
            Assert.AreEqual(attribute.DataName, dbAttribute.Data_Name, "Property 'DataName' was not created correctly.");
            Assert.AreEqual(attribute.DisplayText, dbAttribute.Display_Text, "Property 'DisplayText' was not created correctly.");
            Assert.AreEqual(attribute.DataType.ToString(), dbAttribute.Data_Type.Name, "Property 'DataType' was not created correctly.");
            Assert.AreEqual(attribute.IsKey, dbAttribute.Is_Key, "Property 'IsKey' was not created correctly.");
            Assert.AreEqual(attribute.Sequence, dbAttribute.Sequence, "Property 'Sequence' was not created correctly.");
            Assert.AreEqual(attribute.VariableUnits, dbAttribute.Variable_Units, "Property 'VariableUnits' was not created correctly.");
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Create")]
        [TestCategory("Attribute")]
        [ExpectedException(typeof(System.Data.Entity.Infrastructure.DbUpdateException))]
        public void CreateDuplicateAttribute_Name()
        {
            EAVStoreClient.EAVAttributeClient client = new EAVStoreClient.EAVAttributeClient();
            int containerID = SelectRandomItem(this.DbContext.Containers).Context_ID;
            string attributeName = Guid.NewGuid().ToString();

            EAV.Store.IStoreAttribute attribute = client.CreateAttribute(new EAV.Store.StoreAttribute()
            {
                Name = attributeName,
                DataName = attributeName.ToUpper(),
                DisplayText = attributeName + ":",
                DataType = EAV.EAVDataType.String,
                IsKey = true,
                Sequence = rng.Next(),
                VariableUnits = true,
            }, containerID);

            Assert.IsNotNull(attribute, "Failed to create attribute with name '{0}' for container ID {1}.", attributeName, containerID);

            client.CreateAttribute(new EAV.Store.StoreAttribute()
            {
                Name = attributeName,
                DataName = attributeName.ToUpper() + "1",
                DisplayText = attributeName + ":",
                DataType = EAV.EAVDataType.String,
                IsKey = true,
                Sequence = rng.Next(),
                VariableUnits = true,
            }, containerID);

            Assert.Fail("Failed to throw exception creating attribute with duplicate name.");
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Create")]
        [TestCategory("Attribute")]
        [ExpectedException(typeof(System.Data.Entity.Infrastructure.DbUpdateException))]
        public void CreateDuplicateAttribute_Data_Name()
        {
            EAVStoreClient.EAVAttributeClient client = new EAVStoreClient.EAVAttributeClient();
            int containerID = SelectRandomItem(this.DbContext.Containers).Context_ID;
            string attributeName = Guid.NewGuid().ToString();

            EAV.Store.IStoreAttribute attribute = client.CreateAttribute(new EAV.Store.StoreAttribute()
            {
                Name = attributeName,
                DataName = attributeName.ToUpper(),
                DisplayText = attributeName + ":",
                DataType = EAV.EAVDataType.String,
                IsKey = true,
                Sequence = rng.Next(),
                VariableUnits = true,
            }, containerID);

            Assert.IsNotNull(attribute, "Failed to create attribute with name '{0}' for container ID {1}.", attributeName, containerID);

            client.CreateAttribute(new EAV.Store.StoreAttribute()
            {
                Name = attributeName + "1",
                DataName = attributeName.ToUpper(),
                DisplayText = attributeName + ":",
                DataType = EAV.EAVDataType.String,
                IsKey = true,
                Sequence = rng.Next(),
                VariableUnits = true,
            }, containerID);

            Assert.Fail("Failed to throw exception creating attribute with duplicate data name.");
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Update")]
        [TestCategory("Attribute")]
        public void UpdateAttribute()
        {
            var dbAttribute = SelectRandomItem(this.DbContext.Attributes);
            string oldName = dbAttribute.Name;
            string oldDataName = dbAttribute.Data_Name;
            string oldDisplayText = dbAttribute.Display_Text;
            bool oldIsKey = dbAttribute.Is_Key;
            int oldSequence = dbAttribute.Sequence;
            bool oldVariableUnits = dbAttribute.Variable_Units.GetValueOrDefault();

            EAVStoreClient.EAVAttributeClient client = new EAVStoreClient.EAVAttributeClient();

            var attribute = (EAV.Store.StoreAttribute)dbAttribute;

            attribute.Name = oldName.Flip();
            attribute.DataName = oldDataName.Flip();
            attribute.DisplayText = oldDisplayText.Flip();
            attribute.IsKey = !oldIsKey;
            attribute.Sequence = -oldSequence;
            attribute.VariableUnits = !oldVariableUnits;

            client.UpdateAttribute(attribute);

            ResetDatabaseContext();

            dbAttribute = this.dbContext.Attributes.Single(it => it.Attribute_ID == attribute.AttributeID);

            Assert.AreEqual(attribute.Name, dbAttribute.Name);
            Assert.AreNotEqual(oldName, dbAttribute.Name);
            Assert.AreEqual(attribute.DataName, dbAttribute.Data_Name);
            Assert.AreNotEqual(oldDataName, dbAttribute.Data_Name);
            Assert.AreEqual(attribute.DisplayText, dbAttribute.Display_Text);
            Assert.AreNotEqual(oldDisplayText, dbAttribute.Display_Text);
            Assert.AreEqual(attribute.IsKey, dbAttribute.Is_Key);
            Assert.AreNotEqual(oldIsKey, dbAttribute.Is_Key);
            Assert.AreEqual(attribute.Sequence, dbAttribute.Sequence);
            Assert.AreNotEqual(oldSequence, dbAttribute.Sequence);
            Assert.AreEqual(attribute.VariableUnits, dbAttribute.Variable_Units.GetValueOrDefault());
            Assert.AreNotEqual(oldVariableUnits, dbAttribute.Variable_Units.GetValueOrDefault());
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Delete")]
        [TestCategory("Attribute")]
        public void DeleteAttribute()
        {
            EAVStoreClient.EAVAttributeClient client = new EAVStoreClient.EAVAttributeClient();
            EAVStoreClient.Container dbContainer = SelectRandomItem(this.DbContext.Containers);
            EAVStoreClient.Attribute dbAttributeIn = CreateAttribute(dbContainer.Container_ID, Guid.NewGuid().ToString(), EAV.EAVDataType.String, rng.Next(), true);

            client.DeleteAttribute(dbAttributeIn.Attribute_ID);

            using (EAVStoreClient.MicroEAVContext ctx = new EAVStoreClient.MicroEAVContext())
            {
                EAVStoreClient.Attribute dbAttributeOut = ctx.Attributes.SingleOrDefault(it => it.Attribute_ID == dbAttributeIn.Attribute_ID);

                Assert.IsNull(dbAttributeOut, "Failed to delete attribute ID {0} from the database.", dbAttributeIn.Attribute_ID);
            }
        }
    }
}
