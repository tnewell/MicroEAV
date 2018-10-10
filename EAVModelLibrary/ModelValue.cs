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
using System.Runtime.Serialization;


namespace EAVModelLibrary
{
    [DataContract(IsReference = true)]
    public class ModelValue : ModelObject, EAV.Model.IModelValue
    {
        public static EAV.Model.IModelValue Create(EAV.Model.IModelAttribute attribute, EAV.Model.IModelInstance instance)
        {
            ModelValue value = new ModelValue()
            {
                Attribute = attribute,
                Instance = instance,
            };

            return (value);
        }

        public ModelValue() { }

        [DataMember(Name = "InstanceID")]
        protected int? instanceID;
        [IgnoreDataMember]
        public int? InstanceID { get { return (Instance != null ? Instance.InstanceID : instanceID); } }

        [DataMember(Name = "AttributeID")]
        protected int? attributeID;
        [IgnoreDataMember]
        public int? AttributeID { get { return (Attribute != null ? Attribute.AttributeID : attributeID); } }

        [DataMember(Name = "UnitID")]
        protected int? unitID;
        [IgnoreDataMember]
        public int? UnitID { get { return (Unit != null ? Unit.UnitID : unitID); } }

        [DataMember(Name = "Instance")]
        protected EAV.Model.IModelInstance instance;
        [IgnoreDataMember]
        public EAV.Model.IModelInstance Instance
        {
            get
            {
                if (instance != null && !instance.Values.Contains(this))
                {
                    instance = null;
                    instanceID = null;
                }

                return (instance);
            }
            set
            {
                if (instance != value)
                {
                    if (ObjectState == EAV.Model.ObjectState.Deleted)
                        throw (new InvalidOperationException("Operation failed. Property 'Instance' may not be modified when object in 'Deleted' state."));
                    else if (value != null && value.ObjectState == EAV.Model.ObjectState.Deleted)
                        throw (new InvalidOperationException("Operation failed. Property 'Instance' may not be assigned object in 'Deleted' state."));

                    if (instance != null && instance.Values.Contains(this))
                    {
                        instance.Values.Remove(this);
                    }

                    instance = value;
                    instanceID = instance != null ? instance.InstanceID : null;

                    if (ObjectState != EAV.Model.ObjectState.New) ObjectState = EAV.Model.ObjectState.Modified;

                    if (instance != null && !instance.Values.Contains(this))
                    {
                        instance.Values.Add(this);
                    }
                }
                else if (value != null && value.InstanceID != instanceID)
                {
                    instanceID = value.InstanceID;

                    if (ObjectState != EAV.Model.ObjectState.New) ObjectState = EAV.Model.ObjectState.Modified;
                }
            }
        }

        [DataMember(Name = "Attribute")]
        protected EAV.Model.IModelAttribute attribute;
        [IgnoreDataMember]
        public EAV.Model.IModelAttribute Attribute
        {
            get
            {
                if (attribute != null && !attribute.Values.Contains(this))
                {
                    attribute = null;
                    attributeID = null;
                }

                return (attribute);
            }
            set
            {
                if (attribute != value)
                {
                    if (ObjectState == EAV.Model.ObjectState.Deleted)
                        throw (new InvalidOperationException("Operation failed. Property 'Attribute' may not be modified when object in 'Deleted' state."));
                    else if (value != null && value.ObjectState == EAV.Model.ObjectState.Deleted)
                        throw (new InvalidOperationException("Operation failed. Property 'Attribute' may not be assigned object in 'Deleted' state."));
                    else if (value == null && UnitID != null)
                        throw (new InvalidOperationException("Operation failed. Property 'Attribute' may not be assigned a value of 'null' when property 'UnitID' has a value."));

                    if (attribute != null && attribute.Values.Contains(this))
                    {
                        attribute.Values.Remove(this);
                    }

                    attribute = value;
                    attributeID = attribute != null ? attribute.AttributeID : null;

                    if (ObjectState != EAV.Model.ObjectState.New) ObjectState = EAV.Model.ObjectState.Modified;

                    if (attribute != null && !attribute.Values.Contains(this))
                    {
                        attribute.Values.Add(this);
                    }
                }
                else if (value != null && value.AttributeID != attributeID)
                {
                    attributeID = value.AttributeID;

                    if (ObjectState != EAV.Model.ObjectState.New) ObjectState = EAV.Model.ObjectState.Modified;
                }
            }
        }

        [DataMember(Name = "RawValue")]
        protected string rawValue;
        [IgnoreDataMember]
        public string RawValue
        {
            get
            {
                return (rawValue);
            }
            set
            {
                if (rawValue != value)
                {
                    if (ObjectState == EAV.Model.ObjectState.Deleted)
                        throw (new InvalidOperationException("Operation failed. Property 'RawValue' may not be modified when object in 'Deleted' state."));

                    rawValue = value;

                    if (ObjectState != EAV.Model.ObjectState.New) ObjectState = EAV.Model.ObjectState.Modified;
                }
            }
        }

        [DataMember(Name = "Unit")]
        protected EAV.Model.IModelUnit unit;
        [IgnoreDataMember]
        public EAV.Model.IModelUnit Unit
        {
            get
            {
                return (unit);
            }
            set
            {
                if (unit != value)
                {
                    if (ObjectState == EAV.Model.ObjectState.Deleted)
                        throw (new InvalidOperationException("Operation failed. Property 'Unit' may not be modified when object in 'Deleted' state."));
                    else if (value != null && value.ObjectState == EAV.Model.ObjectState.Deleted)
                        throw (new InvalidOperationException("Operation failed. Property 'Unit' may not be assigned object in 'Deleted' state."));
                    else if (attribute != null && attribute.VariableUnits == null)
                        throw (new InvalidOperationException("Operation failed. Property 'Unit' may not be modified when property 'Attribute.VariableUnits' is null."));

                    unit = value;
                    unitID = value != null ? value.UnitID : null;

                    if (ObjectState != EAV.Model.ObjectState.New) ObjectState = EAV.Model.ObjectState.Modified;
                }
                else if (value != null && value.UnitID != unitID)
                {
                    unitID = value.UnitID;

                    if (ObjectState != EAV.Model.ObjectState.New) ObjectState = EAV.Model.ObjectState.Modified;
                }
            }
        }

        public override void MarkUnmodified()
        {
            if (ObjectState == EAV.Model.ObjectState.Deleted)
                throw (new InvalidOperationException("Operation failed. Object in 'Deleted' state may not be marked 'Unmodified'."));

            if (Attribute == null || Attribute.ObjectState == EAV.Model.ObjectState.New || Instance == null || Instance.ObjectState == EAV.Model.ObjectState.New)
                throw (new InvalidOperationException("Operation failed. Object with null or new 'Attribute' property or null or new Instance property may not be marked unmodified."));

            ObjectState = EAV.Model.ObjectState.Unmodified;
        }

        public override void MarkDeleted()
        {
            if (ObjectState == EAV.Model.ObjectState.New)
                throw (new InvalidOperationException("Operation failed. Object in 'New' state may not be marked 'Deleted'."));

            ObjectState = EAV.Model.ObjectState.Deleted;
        }
    }
}
