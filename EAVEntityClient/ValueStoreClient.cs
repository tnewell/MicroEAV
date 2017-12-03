using System;
using System.Collections.Generic;
using System.Linq;


namespace EAVStoreClient
{
    public partial class EAVValueClient : EAV.Store.IEAVValueClient
    {
        public IEnumerable<EAV.Model.IEAVValue> RetrieveValues(int? attributeID, int? instanceID)
        {
            using (EAVStoreClient.MicroEAVContext ctx = new MicroEAVContext())
            {
                if (instanceID != null && attributeID != null)
                {
                    return (ctx.Values.Where(it => it.Instance_ID == instanceID && it.Attribute_ID == attributeID).AsEnumerable().Select(it => (EAV.Model.BaseEAVValue)it).ToList());
                }
                else if (instanceID != null && attributeID == null)
                {
                    return (ctx.Values.Where(it => it.Instance_ID == instanceID).AsEnumerable().Select(it => (EAV.Model.BaseEAVValue)it).ToList());
                }
                else if (instanceID == null && attributeID != null)
                {
                    return (ctx.Values.Where(it => it.Attribute_ID == attributeID).AsEnumerable().Select(it => (EAV.Model.BaseEAVValue)it).ToList());
                }
                else
                {
                    return (ctx.Values.AsEnumerable().Select(it => (EAV.Model.BaseEAVValue)it).ToList());
                }
            }
        }

        public EAV.Model.IEAVValue RetrieveValue(int attributeID, int instanceID)
        {
            using (EAVStoreClient.MicroEAVContext ctx = new MicroEAVContext())
            {
                return ((EAV.Model.BaseEAVValue)ctx.Values.SingleOrDefault(it => it.Instance_ID == instanceID && it.Attribute_ID == attributeID));
            }
        }

        public EAV.Model.IEAVValue CreateValue(EAV.Model.IEAVValue value, int instanceID, int attributeID)
        {
            if (value == null)
                return (null);

            using (EAVStoreClient.MicroEAVContext ctx = new MicroEAVContext())
            {
                Value dbValue = new Value(value);

                dbValue.Instance_ID = instanceID;
                dbValue.Attribute_ID = attributeID;

                ctx.Values.Add(dbValue);

                ctx.SaveChanges();

                return ((EAV.Model.BaseEAVValue) dbValue);
            }
        }

        public void UpdateValue(EAV.Model.IEAVValue value)
        {
            using (EAVStoreClient.MicroEAVContext ctx = new MicroEAVContext())
            {
                EAVStoreClient.Value dbValue = ctx.Values.SingleOrDefault(it => it.Instance_ID == value.InstanceID && it.Attribute_ID == value.AttributeID);

                if (dbValue != null)
                {
                    if (dbValue.Raw_Value != value.RawValue)
                        dbValue.Raw_Value = value.RawValue;

                    if (dbValue.Units != value.Units)
                        dbValue.Units = value.Units;

                    ctx.SaveChanges();
                }
                else
                    throw (new Exception(String.Format("Unable to retrieve value for instance ID {0}, attribute ID {1}.", value.InstanceID, value.AttributeID)));
            }
        }

        public void DeleteValue(int attributeID, int instanceID)
        {
            using (EAVStoreClient.MicroEAVContext ctx = new MicroEAVContext())
            {
                var dbValue = ctx.Values.SingleOrDefault(it => it.Instance_ID == instanceID && it.Attribute_ID == attributeID);

                if (dbValue != null)
                {
                    ctx.Values.Remove(dbValue);

                    ctx.SaveChanges();
                }
            }
        }
    }
}
