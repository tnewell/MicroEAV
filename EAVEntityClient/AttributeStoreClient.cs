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
    public partial class EAVAttributeClient : EAV.Store.IEAVAttributeClient
    {
        public IEnumerable<EAV.Model.IEAVAttribute> RetrieveAttributes(int? containerID)
        {
            using (EAVStoreClient.MicroEAVContext ctx = new MicroEAVContext())
            {
                if (containerID != null)
                {
                    return (ctx.Attributes.Include("Data_Type").Where(it => it.Container_ID == containerID).AsEnumerable().Select(it => (EAV.Model.BaseEAVAttribute)it).ToList());
                }
                else
                {
                    return (ctx.Attributes.Include("Data_Type").AsEnumerable().Select(it => (EAV.Model.BaseEAVAttribute)it).ToList());
                }
            }
        }

        public EAV.Model.IEAVAttribute RetrieveAttribute(int attributeID)
        {
            using (EAVStoreClient.MicroEAVContext ctx = new MicroEAVContext())
            {
                return ((EAV.Model.BaseEAVAttribute)ctx.Attributes.Include("Data_Type").SingleOrDefault(it => it.Attribute_ID == attributeID));
            }
        }

        public EAV.Model.IEAVAttribute CreateAttribute(EAV.Model.IEAVAttribute attribute, int containerID)
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

                return ((EAV.Model.BaseEAVAttribute)dbAttribute);
            }
        }

        public void UpdateAttribute(EAV.Model.IEAVAttribute attribute)
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
                    ctx.Attributes.Remove(dbAttribute);

                    ctx.SaveChanges();
                }
            }
        }
    }
}
