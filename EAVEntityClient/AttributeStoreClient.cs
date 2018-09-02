// MicroEAV
//
// Copyright(C) 2017  Tim Newell

// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with this program.If not, see<http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;


namespace EAVStoreClient
{
    public partial class AttributeStoreClient : EAV.Store.Clients.IAttributeStoreClient
    {
        public IEnumerable<EAV.Store.IStoreAttribute> RetrieveAttributes(int? containerID)
        {
            using (EAVStoreClient.MicroEAVContext ctx = new MicroEAVContext())
            {
                if (containerID != null)
                {
                    return (ctx.Attributes.Include("Data_Type").Where(it => it.Container_ID == containerID).AsEnumerable().Select(it => (EAV.Store.StoreAttribute)it).ToList());
                }
                else
                {
                    return (ctx.Attributes.Include("Data_Type").AsEnumerable().Select(it => (EAV.Store.StoreAttribute)it).ToList());
                }
            }
        }

        public EAV.Store.IStoreAttribute RetrieveAttribute(int attributeID)
        {
            using (EAVStoreClient.MicroEAVContext ctx = new MicroEAVContext())
            {
                return ((EAV.Store.StoreAttribute)ctx.Attributes.Include("Data_Type").SingleOrDefault(it => it.Attribute_ID == attributeID));
            }
        }

        public EAV.Store.IStoreAttribute CreateAttribute(EAV.Store.IStoreAttribute attribute, int containerID)
        {
            if (attribute == null)
                return (null);

            using (EAVStoreClient.MicroEAVContext ctx = new MicroEAVContext())
            {
                Attribute dbAttribute = new Attribute(attribute);

                dbAttribute.Container_ID = containerID;
                dbAttribute.Data_Type = ctx.LookupDataType(attribute.DataType);

                ctx.Attributes.Add(dbAttribute);

                ctx.SaveChanges();

                return ((EAV.Store.StoreAttribute)dbAttribute);
            }
        }

        public void UpdateAttribute(EAV.Store.IStoreAttribute attribute)
        {
            using (EAVStoreClient.MicroEAVContext ctx = new MicroEAVContext())
            {
                EAVStoreClient.Attribute dbAttribute = ctx.Attributes.SingleOrDefault(it => it.Attribute_ID == attribute.AttributeID);

                if (dbAttribute != null)
                {
                    if (dbAttribute.Name != attribute.Name)
                        dbAttribute.Name = attribute.Name;

                    if (dbAttribute.Data_Name != attribute.DataName)
                        dbAttribute.Data_Name = attribute.DataName;

                    if (dbAttribute.Display_Text != attribute.DisplayText)
                        dbAttribute.Display_Text = attribute.DisplayText;

                    if (dbAttribute.Is_Key != attribute.IsKey)
                        dbAttribute.Is_Key = attribute.IsKey;

                    if (dbAttribute.Sequence != attribute.Sequence)
                        dbAttribute.Sequence = attribute.Sequence;

                    if (dbAttribute.Variable_Units != attribute.VariableUnits)
                        dbAttribute.Variable_Units = attribute.VariableUnits;

                    ctx.SaveChanges();
                }
                else
                    throw (new Exception(String.Format("Unable to retrieve attribute ID {0}.", attribute.AttributeID)));
            }
        }

        public void DeleteAttribute(int attributeID)
        {
            using (EAVStoreClient.MicroEAVContext ctx = new MicroEAVContext())
            {
                var dbAttribute = ctx.Attributes.SingleOrDefault(it => it.Attribute_ID == attributeID);

                if (dbAttribute != null)
                {
                    // TODO: Right way to do this?
                    dbAttribute.Units.Clear();

                    ctx.Attributes.Remove(dbAttribute);

                    ctx.SaveChanges();
                }
            }
        }

        public IEnumerable<EAV.Store.IStoreUnit> RetrieveAttributeUnits(int attributeID)
        {
            using (EAVStoreClient.MicroEAVContext ctx = new MicroEAVContext())
            {
                var dbAttribute = ctx.Attributes.SingleOrDefault(it => it.Attribute_ID == attributeID);

                if (dbAttribute != null)
                {
                    return(dbAttribute.Units.Select(it => (EAV.Store.StoreUnit) it));
                }
                else
                    throw(new Exception(String.Format("Unable to retrieve attribute ID {0}.", attributeID)));
            }
        }

        public void UpdateAttributeUnits(int attributeID, IEnumerable<EAV.Store.IStoreUnit> units)
        {
            using (EAVStoreClient.MicroEAVContext ctx = new MicroEAVContext())
            {
                var dbAttribute = ctx.Attributes.SingleOrDefault(it => it.Attribute_ID == attributeID);

                if (dbAttribute != null)
                {
                    var unitsToDelete = dbAttribute.Units.GroupJoin(units, left => left.Unit_ID, right => right.UnitID.GetValueOrDefault(), (left,right) => new { UnitToDelete = left, SentinelUnit = right.FirstOrDefault() }).Where(it => it.SentinelUnit == null).Select(it => it.UnitToDelete.Unit_ID);
                    var unitsToAdd = units.GroupJoin(dbAttribute.Units, left => left.UnitID.GetValueOrDefault(), right => right.Unit_ID, (left, right) => new { UnitToAdd = left, SentinelUnit = right.FirstOrDefault() }).Where(it => it.SentinelUnit == null && it.UnitToAdd.UnitID != 0).Select(it => it.UnitToAdd.UnitID.GetValueOrDefault());

                    foreach (int id in unitsToDelete)
                        dbAttribute.Units.Remove(dbAttribute.Units.Single(it => it.Unit_ID == id));

                    foreach (int id in unitsToAdd)
                        dbAttribute.Units.Add(ctx.Units.Single(it => it.Unit_ID == id));

                    ctx.SaveChanges();
                }
                else
                    throw(new Exception(String.Format("Unable to retrieve attribute ID {0}.", attributeID)));
            }
        }
    }
}
