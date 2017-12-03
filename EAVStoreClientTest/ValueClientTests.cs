using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace EAVStoreClientTestHarness
{
    public partial class EAVStoreClientTestHarness
    {
        internal class ValueKey
        {
            public int InstanceID { get; set; }
            public int AttributeID { get; set; }
        }

        private IEnumerable<ValueKey> RetrieveAvailableValueKeys()
        {
            return(this.DbContext.Database.SqlQuery<ValueKey>(
                @"select it.Instance_ID as InstanceID, at.Attribute_ID as AttributeID
                from Container c
                join Instance it on it.Container_ID = c.Container_ID
                join Attribute at on at.Container_ID = c.Container_ID
                left join Value val on val.Instance_ID = it.Instance_ID and val.Attribute_ID = at.Attribute_ID
                where val.Raw_Value is null"));
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Retrieve")]
        [TestCategory("Value")]
        public void RetrieveAllValues()
        {
            EAVStoreClient.EAVValueClient client = new EAVStoreClient.EAVValueClient();

            int nDbValues = this.DbContext.Values.Count();
            int nClientValues = client.RetrieveValues(null, null).Count();

            Assert.AreEqual(nDbValues, nClientValues, "The number of values retrieved by the client does not match the number in the database.");
       }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Retrieve")]
        [TestCategory("Value")]
        public void RetrieveNonExistentValue()
        {
            EAVStoreClient.EAVValueClient client = new EAVStoreClient.EAVValueClient();

            var value = client.RetrieveValue(-1, -1);

            Assert.IsNull(value, "Unexpected value object retrieved.");
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Retrieve")]
        [TestCategory("Value")]
        public void RetrieveRandomValue()
        {
            EAVStoreClient.Value dbValue = SelectRandomItem(this.DbContext.Values);

            if (dbValue != null)
            {
                EAVStoreClient.EAVValueClient client = new EAVStoreClient.EAVValueClient();

                var value = client.RetrieveValue(dbValue.Attribute_ID, dbValue.Instance_ID);

                Assert.IsNotNull(value, "Failed to retrieve value [{0}, {1}].", dbValue.Attribute_ID, dbValue.Instance_ID);
                Assert.AreEqual(dbValue.Attribute_ID, value.AttributeID, "Attribute ID values do not match.");
                Assert.AreEqual(dbValue.Instance_ID, value.InstanceID, "Instance ID values do not match.");
            }
            else
            {
                Assert.Inconclusive("No values were found in the database.");
            }
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Create")]
        [TestCategory("Value")]
        public void CreateValue()
        {
            EAVStoreClient.EAVValueClient client = new EAVStoreClient.EAVValueClient();
            var valueKey = SelectRandomItem(RetrieveAvailableValueKeys());
            string rawValue = Guid.NewGuid().ToString();

            EAV.Model.IEAVValue value = client.CreateValue(new EAV.Model.BaseEAVValue()
            {
                RawValue = rawValue,
            }, valueKey.InstanceID, valueKey.AttributeID);

            Assert.IsNotNull(value, "Failed to create value with value '{0}' for instance ID {1} and attribute ID {2}.", rawValue, valueKey.InstanceID, valueKey.AttributeID);

            ResetDatabaseContext();

            var dbValue = this.DbContext.Values.SingleOrDefault(it => it.Instance_ID == valueKey.InstanceID && it.Attribute_ID == valueKey.AttributeID);

            Assert.IsNotNull(dbValue, String.Format("Failed to retrieve instance ID {0}, attribute ID {1} from the database.", value.InstanceID, value.AttributeID));

            Assert.AreEqual(value.RawValue, dbValue.Raw_Value, "Property 'RawValue' was not created correctly.");
            Assert.AreEqual(value.Units, dbValue.Units, "Property 'Units' was not created correctly.");
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Update")]
        [TestCategory("Value")]
        public void UpdateValue()
        {
            var dbValue = SelectRandomItem(this.DbContext.Values.AsEnumerable().Where(it => !String.IsNullOrWhiteSpace(it.Units)));
            string oldValue = dbValue.Raw_Value;
            string oldUnits = dbValue.Units;

            EAVStoreClient.EAVValueClient client = new EAVStoreClient.EAVValueClient();

            var value = (EAV.Model.BaseEAVValue)dbValue;

            value.RawValue = oldValue.Flip();
            value.Units = oldUnits.Flip();

            client.UpdateValue(value);

            ResetDatabaseContext();

            dbValue = this.dbContext.Values.Single(it => it.Instance_ID == value.InstanceID && it.Attribute_ID == value.AttributeID);

            Assert.AreEqual(value.RawValue, dbValue.Raw_Value);
            Assert.AreNotEqual(oldValue, dbValue.Raw_Value);
            Assert.AreEqual(value.Units, dbValue.Units);
            Assert.AreNotEqual(oldUnits, dbValue.Units);
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Delete")]
        [TestCategory("Value")]
        public void DeleteValue()
        {

            EAVStoreClient.EAVValueClient client = new EAVStoreClient.EAVValueClient();
            var valueKey = SelectRandomItem(RetrieveAvailableValueKeys());
            EAVStoreClient.Value dbValueIn = CreateValue(valueKey.AttributeID, valueKey.InstanceID, Guid.NewGuid().ToString());

            client.DeleteValue(dbValueIn.Attribute_ID, dbValueIn.Instance_ID);

            EAVStoreClient.Value dbValueOut = this.DbContext.Values.SingleOrDefault(it => it.Attribute_ID == dbValueIn.Attribute_ID && it.Instance_ID == dbValueIn.Instance_ID);

            Assert.IsNull(dbValueOut, "Failed to delete value for attribute ID {0} and instance ID {1} from the database.", dbValueIn.Attribute_ID, dbValueIn.Instance_ID);
        }
    }
}
