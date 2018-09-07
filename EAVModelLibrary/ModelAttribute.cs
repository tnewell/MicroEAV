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
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;


namespace EAVModelLibrary
{
    [DataContract(IsReference = true)]
    public class ModelAttribute : ModelObject, EAV.Model.IModelAttribute
    {
        public ModelAttribute()
        {
            units = new ObservableCollection<EAV.Model.IModelUnit>();
            units.CollectionChanged += Units_CollectionChanged;

            values = new ObservableCollection<EAV.Model.IModelValue>();
            values.CollectionChanged += Values_CollectionChanged;
        }

        private void Units_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:

                    if (VariableUnits.GetValueOrDefault(true))
                    {
                        throw (new InvalidOperationException("Operation failed. Property 'VariableUnits' must have the value 'false' before units may be added to the Units collection."));
                    }
                    else if (e.NewItems != null)
                    {
                        IEnumerable<ModelUnit> addedUnits = e.NewItems.OfType<ModelUnit>();

                        if (addedUnits.Any(it => it.ObjectState == EAV.Model.ObjectState.Deleted || it.ObjectState == EAV.Model.ObjectState.New || it.UnitID == null))
                        {
                            throw (new InvalidOperationException("Operation failed. Only existing units may be added to the Units collection."));
                        }
                    }

                    if (ObjectState != EAV.Model.ObjectState.New) ObjectState = EAV.Model.ObjectState.Modified;
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                    if (ObjectState != EAV.Model.ObjectState.New) ObjectState = EAV.Model.ObjectState.Modified;
                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Move:
                    break;
            }
        }

        private void Values_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                    if (ObjectState != EAV.Model.ObjectState.New) ObjectState = EAV.Model.ObjectState.Modified;

                    if (e.OldItems != null)
                    {
                        foreach (ModelValue value in e.OldItems)
                        {
                            if (value.Attribute == this)
                            {
                                value.Attribute = null;
                            }
                        }
                    }

                    if (e.NewItems != null)
                    {
                        foreach (ModelValue value in e.NewItems)
                        {
                            if (value.Attribute != this)
                            {
                                value.Attribute = this;
                            }
                        }
                    }

                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Move:
                    break;
            }
        }

        [DataMember(Name = "AttributeID")]
        protected int? attributeID;
        [IgnoreDataMember]
        public int? AttributeID
        {
            get
            {
                return (attributeID);
            }
            set
            {
                if (ObjectState == EAV.Model.ObjectState.New || (ObjectState == EAV.Model.ObjectState.Unmodified && attributeID == null))
                {
                    attributeID = value;
                }
                else
                {
                    throw (new InvalidOperationException("This property has already been set."));
                }
            }
        }

        [DataMember(Name = "ContainerID")]
        protected int? containerID;
        [IgnoreDataMember]
        public int? ContainerID { get { return (Container != null ? Container.ContainerID : containerID); } }

        [DataMember(Name = "Container")]
        protected EAV.Model.IModelContainer container;
        [IgnoreDataMember]
        public EAV.Model.IModelContainer Container
        {
            get
            {
                if (container != null && !container.Attributes.Contains(this))
                {
                    container = null;
                    containerID = null;
                }

                return (container);
            }
            set
            {
                if (container != value)
                {
                    if (ObjectState == EAV.Model.ObjectState.Deleted)
                        throw (new InvalidOperationException("Operation failed. Property 'Container' may not be modified when object in 'Deleted' state."));
                    else if (value != null && value.ObjectState == EAV.Model.ObjectState.Deleted)
                        throw (new InvalidOperationException("Operation failed. Property 'Container' may not be assigned object in 'Deleted' state."));

                    if (container != null && container.Attributes.Contains(this))
                    {
                        container.Attributes.Remove(this);
                    }

                    container = value;
                    containerID = container != null ? container.ContainerID : null;

                    if (ObjectState != EAV.Model.ObjectState.New) ObjectState = EAV.Model.ObjectState.Modified;

                    if (container != null && !container.Attributes.Contains(this))
                    {
                        container.Attributes.Add(this);
                    }
                }
            }
        }

        [DataMember(Name = "Name")]
        protected string name;
        [IgnoreDataMember]
        public string Name
        {
            get
            {
                return (name);
            }
            set
            {
                if (name != value)
                {
                    if (ObjectState == EAV.Model.ObjectState.Deleted)
                        throw (new InvalidOperationException("Operation failed. Property 'Name' may not be modified when object in 'Deleted' state."));

                    name = value;

                    if (ObjectState != EAV.Model.ObjectState.New) ObjectState = EAV.Model.ObjectState.Modified;
                }
            }
        }

        [DataMember(Name = "DataName")]
        protected string dataName;
        [IgnoreDataMember]
        public string DataName
        {
            get
            {
                return (dataName);
            }
            set
            {
                if (dataName != value)
                {
                    if (ObjectState == EAV.Model.ObjectState.Deleted)
                        throw (new InvalidOperationException("Operation failed. Property 'DataName' may not be modified when object in 'Deleted' state."));

                    dataName = value;

                    if (ObjectState != EAV.Model.ObjectState.New) ObjectState = EAV.Model.ObjectState.Modified;
                }
            }
        }

        [DataMember(Name = "DisplayText")]
        protected string displayText;
        [IgnoreDataMember]
        public string DisplayText
        {
            get
            {
                return (displayText);
            }
            set
            {
                if (displayText != value)
                {
                    if (ObjectState == EAV.Model.ObjectState.Deleted)
                        throw (new InvalidOperationException("Operation failed. Property 'DisplayText' may not be modified when object in 'Deleted' state."));

                    displayText = value;

                    if (ObjectState != EAV.Model.ObjectState.New) ObjectState = EAV.Model.ObjectState.Modified;
                }
            }
        }

        [DataMember(Name = "DataType")]
        protected EAV.EAVDataType dataType;
        [IgnoreDataMember]
        public EAV.EAVDataType DataType
        {
            get
            {
                return (dataType);
            }
            set
            {
                if (dataType != value)
                {
                    if (ObjectState == EAV.Model.ObjectState.Deleted)
                        throw (new InvalidOperationException("Operation failed. Property 'DataType' may not be modified when object in 'Deleted' state."));

                    dataType = value;

                    if (ObjectState != EAV.Model.ObjectState.New) ObjectState = EAV.Model.ObjectState.Modified;
                }
            }
        }

        [DataMember(Name = "Sequence")]
        protected int sequence;
        [IgnoreDataMember]
        public int Sequence
        {
            get
            {
                return (sequence);
            }
            set
            {
                if (sequence != value)
                {
                    if (ObjectState == EAV.Model.ObjectState.Deleted)
                        throw (new InvalidOperationException("Operation failed. Property 'Sequence' may not be modified when object in 'Deleted' state."));

                    sequence = value;

                    if (ObjectState != EAV.Model.ObjectState.New) ObjectState = EAV.Model.ObjectState.Modified;
                }
            }
        }

        [DataMember(Name = "IsKey")]
        protected bool isKey;
        [IgnoreDataMember]
        public bool IsKey
        {
            get
            {
                return (isKey);
            }
            set
            {
                if (isKey != value)
                {
                    if (ObjectState == EAV.Model.ObjectState.Deleted)
                        throw (new InvalidOperationException("Operation failed. Property 'IsKey' may not be modified when object in 'Deleted' state."));

                    isKey = value;

                    if (ObjectState != EAV.Model.ObjectState.New) ObjectState = EAV.Model.ObjectState.Modified;
                }
            }
        }

        [DataMember(Name = "VariableUnits")]
        protected bool? variableUnits;
        [IgnoreDataMember]
        public bool? VariableUnits
        {
            get
            {
                return (variableUnits);
            }
            set
            {
                if (variableUnits != value)
                {
                    if (ObjectState == EAV.Model.ObjectState.Deleted)
                        throw (new InvalidOperationException("Operation failed. Property 'VariableUnits' may not be modified when object in 'Deleted' state."));
                    else if (units.Any() && value.GetValueOrDefault(true))
                        throw (new InvalidOperationException("Operation failed. Property 'VariableUnits' may not be assigned any value but false when values exist in the Units collection."));

                    variableUnits = value;

                    if (ObjectState != EAV.Model.ObjectState.New) ObjectState = EAV.Model.ObjectState.Modified;
                }
            }
        }

        [DataMember(Name = "Units")]
        private ObservableCollection<EAV.Model.IModelUnit> units;
        [IgnoreDataMember]
        public ICollection<EAV.Model.IModelUnit> Units
        {
            get { if (ObjectState != EAV.Model.ObjectState.Deleted) return (units); else return (new ReadOnlyObservableCollection<EAV.Model.IModelUnit>(units)); }
        }

        [DataMember(Name = "Values")]
        private ObservableCollection<EAV.Model.IModelValue> values;
        [IgnoreDataMember]
        public ICollection<EAV.Model.IModelValue> Values
        {
            get { if (ObjectState != EAV.Model.ObjectState.Deleted) return (values); else return (new ReadOnlyObservableCollection<EAV.Model.IModelValue>(values)); }
        }

        public override void MarkUnmodified()
        {
            if (ObjectState == EAV.Model.ObjectState.Deleted)
                throw (new InvalidOperationException("Operation failed. Object in 'Deleted' state may not be marked unmodified."));

            if (AttributeID == null)
                throw (new InvalidOperationException("Operation failed. Object with null 'AttributeID' property may not be marked unmodified."));

            ObjectState = EAV.Model.ObjectState.Unmodified;
        }

        public override void MarkDeleted()
        {
            if (ObjectState == EAV.Model.ObjectState.New)
                throw (new InvalidOperationException("Operation failed. Object in 'New' state may not be marked deleted."));

            if (ObjectState != EAV.Model.ObjectState.Deleted)
            {
                ObjectState = EAV.Model.ObjectState.Deleted;

                foreach (ModelValue value in values)
                    value.MarkDeleted();
            }
        }
    }
}
