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


namespace EAV.Model
{
    [DataContract(IsReference = true)]
    public class ModelValue : ModelObject, EAV.Model.IModelValue
    {
        public static IModelValue Create(IModelAttribute attribute, IModelInstance instance)
        {
            ModelValue value = new ModelValue()
            {
                Attribute = attribute,
                Instance = instance,
            };

            return (value);
        }

        public ModelValue() { }

        public int? InstanceID { get { return (Instance != null ? Instance.InstanceID : null); } }

        public int? AttributeID { get { return (Attribute != null ? Attribute.AttributeID : null); } }

        private int? unitID;
        public int? UnitID
        {
            get
            {
                return (Unit != null ? Unit.UnitID : unitID);
            }
            set
            {
                if (unitID != value)
                {
                    if (ObjectState == ObjectState.Deleted)
                        throw (new InvalidOperationException("Operation failed. Property 'UnitID' may not be modified when object in 'Deleted' state."));
                    else if (attribute != null && attribute.VariableUnits == null)
                        throw (new InvalidOperationException("Operation failed. Property 'UnitID' may not be modified when property 'Attribute.VariableUnits' is null."));

                    if (Unit == null)
                        unitID = value;
                    else
                        throw (new InvalidOperationException("Property 'UnitID' may not be modified when property 'Unit' has a value."));

                    if (ObjectState != ObjectState.New) ObjectState = ObjectState.Modified;
                }
            }
        }

        [DataMember(Name = "Instance")]
        protected IModelInstance instance;
        [IgnoreDataMember]
        public IModelInstance Instance
        {
            get
            {
                if (instance != null && !instance.Values.Contains(this))
                {
                    instance = null;
                }

                return (instance);
            }
            set
            {
                if (instance != value)
                {
                    if (ObjectState == ObjectState.Deleted)
                        throw (new InvalidOperationException("Operation failed. Property 'Instance' may not be modified when object in 'Deleted' state."));

                    if (instance != null && instance.Values.Contains(this))
                    {
                        instance.Values.Remove(this);
                    }

                    instance = value;
                    if (ObjectState != ObjectState.New) ObjectState = ObjectState.Modified;

                    if (instance != null && !instance.Values.Contains(this))
                    {
                        instance.Values.Add(this);
                    }
                }
            }
        }

        [DataMember(Name = "Attribute")]
        protected IModelAttribute attribute;
        [IgnoreDataMember]
        public IModelAttribute Attribute
        {
            get
            {
                if (attribute != null && !attribute.Values.Contains(this))
                {
                    attribute = null;
                }

                return (attribute);
            }
            set
            {
                if (attribute != value)
                {
                    if (ObjectState == ObjectState.Deleted)
                        throw (new InvalidOperationException("Operation failed. Property 'Attribute' may not be modified when object in 'Deleted' state."));
                    else if (value == null && UnitID != null)
                        throw (new InvalidOperationException("Operation failed. Property 'Attribute' may not be assigned a value of 'null' when property 'UnitID' has a value."));

                    if (attribute != null && attribute.Values.Contains(this))
                    {
                        attribute.Values.Remove(this);
                    }

                    attribute = value;
                    if (ObjectState != ObjectState.New) ObjectState = ObjectState.Modified;

                    if (attribute != null && !attribute.Values.Contains(this))
                    {
                        attribute.Values.Add(this);
                    }
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
                    if (ObjectState == ObjectState.Deleted)
                        throw (new InvalidOperationException("Operation failed. Property 'RawValue' may not be modified when object in 'Deleted' state."));

                    rawValue = value;

                    if (ObjectState != ObjectState.New) ObjectState = ObjectState.Modified;
                }
            }
        }

        [DataMember(Name = "Unit")]
        protected IModelUnit unit;
        [IgnoreDataMember]
        public IModelUnit Unit
        {
            get
            {
                return (unit);
            }
            set
            {
                if (unit != value)
                {
                    if (ObjectState == ObjectState.Deleted)
                        throw (new InvalidOperationException("Operation failed. Property 'Unit' may not be modified when object in 'Deleted' state."));
                    else if (attribute != null && attribute.VariableUnits == null)
                        throw (new InvalidOperationException("Operation failed. Property 'Unit' may not be modified when property 'Attribute.VariableUnits' is null."));
                    else if (value != null && value.ObjectState == ObjectState.Deleted)
                        throw (new InvalidOperationException("Operation failed. Property 'Unit' may not be assigned object in 'Deleted' state."));

                    unit = value;
                    unitID = value != null ? value.UnitID : null;

                    if (ObjectState != ObjectState.New) ObjectState = ObjectState.Modified;
                }
            }
        }

        public override void MarkUnmodified()
        {
            if (ObjectState == ObjectState.Deleted)
                throw (new InvalidOperationException("Operation failed. Object in 'Deleted' state may not be marked 'Unmodified'."));

            if (Attribute == null || Attribute.ObjectState == ObjectState.New || Instance == null || Instance.ObjectState == ObjectState.New)
                throw (new InvalidOperationException("Operation failed. Object with null or new 'Attribute' property or null or new Instance property may not be marked unmodified."));

            ObjectState = ObjectState.Unmodified;
        }

        public override void MarkDeleted()
        {
            if (ObjectState == ObjectState.New)
                throw (new InvalidOperationException("Operation failed. Object in 'New' state may not be marked 'Deleted'."));

            ObjectState = ObjectState.Deleted;
        }
    }
}
