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
using System.Runtime.Serialization;


namespace EAVFramework.Model
{
    public enum ObjectState { New, Unmodified, Modified, Deleted }

    #region Infrastructure
    [DataContract(IsReference = true)]
    [KnownType(typeof(EAVMetadataObject))]
    public abstract class EAVObject
    {
        public EAVObject() { }

        [DataMember()]
        public ObjectState ObjectState { get; set; }

        public abstract void MarkUnmodified();

        public abstract void MarkDeleted();
    }

    [DataContract(IsReference = true)]
    [KnownType(typeof(EAVContext))]
    [KnownType(typeof(EAVContainer))]
    [KnownType(typeof(EAVAttribute))]
    public abstract class EAVMetadataObject : EAVObject
    {
        public EAVMetadataObject() { }

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
                    if (ObjectState == ObjectState.Deleted)
                        throw (new InvalidOperationException("Operation failed. Property 'Name' may not be modified when object in 'Deleted' state."));

                    name = value;

                    if (ObjectState != ObjectState.New) ObjectState = ObjectState.Modified;
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
                    if (ObjectState == ObjectState.Deleted)
                        throw (new InvalidOperationException("Operation failed. Property 'DataName' may not be modified when object in 'Deleted' state."));

                    dataName = value;

                    if (ObjectState != ObjectState.New) ObjectState = ObjectState.Modified;
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
                    if (ObjectState == ObjectState.Deleted)
                        throw (new InvalidOperationException("Operation failed. Property 'DisplayText' may not be modified when object in 'Deleted' state."));

                    displayText = value;

                    if (ObjectState != ObjectState.New) ObjectState = ObjectState.Modified;
                }
            }
        }
    }

    [DataContract(IsReference = true)]
    [KnownType(typeof(EAVSubject))]
    [KnownType(typeof(EAVInstance))]
    [KnownType(typeof(EAVValue))]
    public abstract class EAVDataObject : EAVObject
    {
        public EAVDataObject() { }
    }
    #endregion

    #region Metadata Objects
    [DataContract(IsReference = true)]
    public class EAVContext : EAVMetadataObject, EAV.Model.IEAVContext
    {
        public EAVContext()
        {
            containers = new ObservableCollection<EAVRootContainer>();
            containers.CollectionChanged += Containers_CollectionChanged;

            subjects = new ObservableCollection<EAVSubject>();
            subjects.CollectionChanged += Subjects_CollectionChanged;
        }

        private void Containers_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                    if (ObjectState != ObjectState.New) ObjectState = ObjectState.Modified;

                    if (e.OldItems != null)
                    {
                        foreach (EAVContainer container in e.OldItems)
                        {
                            if (container.Context == this)
                            {
                                container.Context = null;
                            }
                        }
                    }

                    if (e.NewItems != null)
                    {
                        foreach (EAVContainer container in e.NewItems)
                        {
                            if (container.Context != this)
                            {
                                container.Context = this;
                            }
                        }
                    }

                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Move:
                    break;
            }
        }

        private void Subjects_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                    if (ObjectState != ObjectState.New) ObjectState = ObjectState.Modified;

                    if (e.OldItems != null)
                    {
                        foreach (EAVSubject subject in e.OldItems)
                        {
                            if (subject.Context == this)
                            {
                                subject.Context = null;
                            }
                        }
                    }

                    if (e.NewItems != null)
                    {
                        foreach (EAVSubject subject in e.NewItems)
                        {
                            if (subject.Context != this)
                            {
                                subject.Context = this;
                            }
                        }
                    }

                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Move:
                    break;
            }
        }

        [DataMember(Name = "ContextID")]
        protected int? contextID;
        [IgnoreDataMember]
        public int? ContextID
        {
            get
            {
                return (contextID);
            }
            set
            {
                if (ObjectState == ObjectState.New || (ObjectState == ObjectState.Unmodified && contextID == null))
                {
                    contextID = value;
                }
                else
                {
                    throw (new InvalidOperationException("This property has already been set."));
                }
            }
        }

        [DataMember(Name = "Containers")]
        private ObservableCollection<EAVRootContainer> containers;
        [IgnoreDataMember]
        public ICollection<EAVRootContainer> Containers
        {
            get { if (ObjectState != ObjectState.Deleted) return (containers); else return (new ReadOnlyObservableCollection<EAVRootContainer>(containers)); }
        }

        [DataMember(Name = "Subjects")]
        private ObservableCollection<EAVSubject> subjects;
        [IgnoreDataMember]
        public ICollection<EAVSubject> Subjects
        {
            get { if (ObjectState != ObjectState.Deleted) return (subjects); else return (new ReadOnlyObservableCollection<EAVSubject>(subjects)); }
        }

        public override void MarkUnmodified()
        {
            if (ObjectState == ObjectState.Deleted)
                throw (new InvalidOperationException("Operation failed. Object in 'Deleted' state may not be marked unmodified."));

            if (ContextID == null)
                throw (new InvalidOperationException("Operation failed. Object with null 'ContextID' property may not be marked unmodified."));

            ObjectState = ObjectState.Unmodified;
        }

        public override void MarkDeleted()
        {
            if (ObjectState == ObjectState.New)
                throw (new InvalidOperationException("Operation failed. Object in 'New' state may not be marked deleted."));

            if (ObjectState != ObjectState.Deleted)
            {
                ObjectState = ObjectState.Deleted;

                foreach (EAVContainer container in containers)
                    container.MarkDeleted();

                foreach (EAVSubject subject in subjects)
                    subject.MarkDeleted();
            }
        }
    }

    [DataContract(IsReference = true)]
    [KnownType(typeof(EAVRootContainer))]
    [KnownType(typeof(EAVChildContainer))]
    public abstract class EAVContainer : EAVMetadataObject, EAV.Model.IEAVContainer
    {
        public EAVContainer()
        {
            childContainers = new ObservableCollection<EAVChildContainer>();
            childContainers.CollectionChanged += ChildContainers_CollectionChanged;

            attributes = new ObservableCollection<EAVAttribute>();
            attributes.CollectionChanged += Attributes_CollectionChanged;

            instances = new ObservableCollection<EAVInstance>();
            instances.CollectionChanged += Instances_CollectionChanged;
        }

        private void ChildContainers_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                    if (ObjectState != ObjectState.New) ObjectState = ObjectState.Modified;

                    if (e.OldItems != null)
                    {
                        foreach (EAVContainer container in e.OldItems)
                        {
                            if (container.ParentContainer == this)
                            {
                                container.ParentContainer = null;
                            }
                        }
                    }

                    if (e.NewItems != null)
                    {
                        foreach (EAVContainer container in e.NewItems)
                        {
                            if (container.ParentContainer != this)
                            {
                                container.ParentContainer = this;
                            }
                        }
                    }

                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Move:
                    break;
            }
        }

        private void Attributes_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                    if (ObjectState != ObjectState.New) ObjectState = ObjectState.Modified;

                    if (e.OldItems != null)
                    {
                        foreach (EAVAttribute attribute in e.OldItems)
                        {
                            if (attribute.Container == this)
                            {
                                attribute.Container = null;
                            }
                        }
                    }

                    if (e.NewItems != null)
                    {
                        foreach (EAVAttribute attribute in e.NewItems)
                        {
                            if (attribute.Container != this)
                            {
                                attribute.Container = this;
                            }
                        }
                    }

                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Move:
                    break;
            }
        }

        private void Instances_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                    if (ObjectState != ObjectState.New) ObjectState = ObjectState.Modified;

                    if (e.OldItems != null)
                    {
                        foreach (EAVInstance instance in e.OldItems)
                        {
                            if (instance.Container == this)
                            {
                                instance.Container = null;
                            }
                        }
                    }

                    if (e.NewItems != null)
                    {
                        foreach (EAVInstance instance in e.NewItems)
                        {
                            if (instance.Container != this)
                            {
                                instance.Container = this;
                            }
                        }
                    }

                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Move:
                    break;
            }
        }

        [DataMember(Name = "ContainerID")]
        protected int? containerID;
        [IgnoreDataMember]
        public int? ContainerID
        {
            get
            {
                return (containerID);
            }
            set
            {
                if (ObjectState == ObjectState.New || (ObjectState == ObjectState.Unmodified && containerID == null))
                {
                    containerID = value;
                }
                else
                {
                    throw (new InvalidOperationException("This property has already been set."));
                }
            }
        }

        public int? ContextID { get { return (Context != null ? Context.ContextID : null); } }

        public int? ParentContainerID { get { return (ParentContainer != null ? ParentContainer.ContainerID : null); } }

        [DataMember(Name = "Context")]
        protected EAVContext context;
        [IgnoreDataMember]
        public abstract EAVContext Context
        {
            get;
            set;
        }

        [DataMember(Name = "ParentContainer")]
        protected EAVContainer parentContainer;
        [IgnoreDataMember]
        public abstract EAVContainer ParentContainer
        {
            get;
            set;
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
                    if (ObjectState == ObjectState.Deleted)
                        throw (new InvalidOperationException("Operation failed. Property 'Sequence' may not be modified when object in 'Deleted' state."));

                    sequence = value;

                    if (ObjectState != ObjectState.New) ObjectState = ObjectState.Modified;
                }
            }
        }

        [DataMember(Name = "IsRepeating")]
        protected bool isRepeating;
        [IgnoreDataMember]
        public bool IsRepeating
        {
            get
            {
                return (isRepeating);
            }
            set
            {
                if (isRepeating != value)
                {
                    if (ObjectState == ObjectState.Deleted)
                        throw (new InvalidOperationException("Operation failed. Property 'IsRepeating' may not be modified when object in 'Deleted' state."));

                    isRepeating = value;

                    if (ObjectState != ObjectState.New) ObjectState = ObjectState.Modified;
                }
            }
        }

        [DataMember(Name = "ChildContainers")]
        private ObservableCollection<EAVChildContainer> childContainers;
        [IgnoreDataMember]
        public ICollection<EAVChildContainer> ChildContainers
        {
            get { if (ObjectState != ObjectState.Deleted) return (childContainers); else return (new ReadOnlyObservableCollection<EAVChildContainer>(childContainers)); }
        }

        [DataMember(Name = "Attributes")]
        private ObservableCollection<EAVAttribute> attributes;
        [IgnoreDataMember]
        public ICollection<EAVAttribute> Attributes
        {
            get { if (ObjectState != ObjectState.Deleted) return (attributes); else return (new ReadOnlyObservableCollection<EAVAttribute>(attributes)); }
        }

        [DataMember(Name = "Instances")]
        private ObservableCollection<EAVInstance> instances;
        [IgnoreDataMember]
        public ICollection<EAVInstance> Instances
        {
            get { if (ObjectState != ObjectState.Deleted) return (instances); else return (new ReadOnlyObservableCollection<EAVInstance>(instances)); }
        }

        protected void SetStateRecursive(ObjectState state)
        {
            this.ObjectState = state;
            foreach (EAVContainer childContainer in ChildContainers)
                childContainer.SetStateRecursive(state);
        }

        public override void MarkUnmodified()
        {
            if (ObjectState == ObjectState.Deleted)
                throw (new InvalidOperationException("Operation failed. Object in 'Deleted' state may not be marked created."));

            if (ContainerID == null)
                throw (new InvalidOperationException("Operation failed. Object with null 'ContainerID' property may not be marked unmodified."));

            ObjectState = ObjectState.Unmodified;
        }

        public override void MarkDeleted()
        {
            if (ObjectState == ObjectState.New)
                throw (new InvalidOperationException("Operation failed. Object in 'New' state may not be marked deleted."));

            if (ObjectState != ObjectState.Deleted)
            {
                ObjectState = ObjectState.Deleted;

                foreach (EAVAttribute attribute in attributes)
                    attribute.MarkDeleted();

                foreach (EAVContainer container in childContainers)
                    container.MarkDeleted();

                foreach (EAVInstance instance in instances)
                    instance.MarkDeleted();
            }
        }
    }

    [DataContract(IsReference = true)]
    public class EAVRootContainer : EAVContainer
    {
        public EAVRootContainer()
        {
            parentContainer = null;
        }

        public override EAVContext Context
        {
            get
            {
                if (context != null && !context.Containers.Contains(this))
                {
                    context = null;
                }

                return (context);
            }
            set
            {
                if (context != value)
                {
                    if (ObjectState == ObjectState.Deleted)
                        throw (new InvalidOperationException("Operation failed. Property 'Context' may not be modified when object in 'Deleted' state."));

                    if (context != null && context.Containers.Contains(this))
                    {
                        context.Containers.Remove(this);
                    }

                    context = value;
                    SetStateRecursive(ObjectState != ObjectState.New ? ObjectState.Modified : ObjectState);

                    if (context != null && !context.Containers.Contains(this))
                    {
                        context.Containers.Add(this);
                    }
                }
            }
        }

        public override EAVContainer ParentContainer
        {
            get
            {
                return (null);
            }
            set
            {
                if (ObjectState == ObjectState.Deleted)
                    throw (new InvalidOperationException("Operation failed. Property 'ParentContainer' may not be modified when object in 'Deleted' state."));

                if (value != null) throw (new InvalidOperationException("The ParentContainer property may only accept 'null' as a value."));
            }
        }
    }

    [DataContract(IsReference = true)]
    public class EAVChildContainer : EAVContainer
    {
        public EAVChildContainer()
        {
        }

        public override EAVContext Context
        {
            get
            {
                context = parentContainer != null ? parentContainer.Context : null;
                return (context);
            }
            set
            {
                if (ObjectState == ObjectState.Deleted)
                    throw (new InvalidOperationException("Operation failed. Property 'Context' may not be modified when object in 'Deleted' state."));

                throw (new InvalidOperationException("The Context property must be set on the root container for this container."));
            }
        }

        public override EAVContainer ParentContainer
        {
            get
            {
                if (parentContainer != null && !parentContainer.ChildContainers.Contains(this))
                {
                    parentContainer = null;
                }

                return (parentContainer);
            }
            set
            {
                if (parentContainer != value)
                {
                    if (ObjectState == ObjectState.Deleted)
                        throw (new InvalidOperationException("Operation failed. Property 'ParentContainer' may not be modified when object in 'Deleted' state."));

                    if (parentContainer != null && parentContainer.ChildContainers.Contains(this))
                    {
                        parentContainer.ChildContainers.Remove(this);
                    }

                    parentContainer = value;
                    if (ObjectState != ObjectState.New) ObjectState = ObjectState.Modified;

                    if (parentContainer != null && !parentContainer.ChildContainers.Contains(this))
                    {
                        parentContainer.ChildContainers.Add(this);
                    }
                }
            }
        }
    }

    [DataContract(IsReference = true)]
    public class EAVAttribute : EAVMetadataObject, EAV.Model.IEAVAttribute
    {
        public EAVAttribute()
        {
            values = new ObservableCollection<EAVValue>();
            values.CollectionChanged += Values_CollectionChanged;
        }

        private void Values_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                    if (ObjectState != ObjectState.New) ObjectState = ObjectState.Modified;

                    if (e.OldItems != null)
                    {
                        foreach (EAVValue value in e.OldItems)
                        {
                            if (value.Attribute == this)
                            {
                                value.Attribute = null;
                            }
                        }
                    }

                    if (e.NewItems != null)
                    {
                        foreach (EAVValue value in e.NewItems)
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
                if (ObjectState == ObjectState.New || (ObjectState == ObjectState.Unmodified && attributeID == null))
                {
                    attributeID = value;
                }
                else
                {
                    throw (new InvalidOperationException("This property has already been set."));
                }
            }
        }

        public int? ContainerID { get { return (Container != null ? Container.ContainerID : null); } }

        [DataMember(Name = "Container")]
        protected EAVContainer container;
        [IgnoreDataMember]
        public EAVContainer Container
        {
            get
            {
                if (container != null && !container.Attributes.Contains(this))
                {
                    container = null;
                }

                return (container);
            }
            set
            {
                if (container != value)
                {
                    if (ObjectState == ObjectState.Deleted)
                        throw (new InvalidOperationException("Operation failed. Property 'Container' may not be modified when object in 'Deleted' state."));

                    if (container != null && container.Attributes.Contains(this))
                    {
                        container.Attributes.Remove(this);
                    }

                    container = value;
                    if (ObjectState != ObjectState.New) ObjectState = ObjectState.Modified;

                    if (container != null && !container.Attributes.Contains(this))
                    {
                        container.Attributes.Add(this);
                    }
                }
            }
        }

        [DataMember(Name = "DataType")]
        protected EAV.Model.EAVDataType dataType;
        [IgnoreDataMember]
        public EAV.Model.EAVDataType DataType
        {
            get
            {
                return (dataType);
            }
            set
            {
                if (dataType != value)
                {
                    if (ObjectState == ObjectState.Deleted)
                        throw (new InvalidOperationException("Operation failed. Property 'DataType' may not be modified when object in 'Deleted' state."));

                    dataType = value;

                    if (ObjectState != ObjectState.New) ObjectState = ObjectState.Modified;
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
                    if (ObjectState == ObjectState.Deleted)
                        throw (new InvalidOperationException("Operation failed. Property 'Sequence' may not be modified when object in 'Deleted' state."));

                    sequence = value;

                    if (ObjectState != ObjectState.New) ObjectState = ObjectState.Modified;
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
                    if (ObjectState == ObjectState.Deleted)
                        throw (new InvalidOperationException("Operation failed. Property 'IsKey' may not be modified when object in 'Deleted' state."));

                    isKey = value;

                    if (ObjectState != ObjectState.New) ObjectState = ObjectState.Modified;
                }
            }
        }

        [DataMember(Name = "Values")]
        private ObservableCollection<EAVValue> values;
        [IgnoreDataMember]
        public ICollection<EAVValue> Values
        {
            get { if (ObjectState != ObjectState.Deleted) return (values); else return (new ReadOnlyObservableCollection<EAVValue>(values)); }
        }

        public override void MarkUnmodified()
        {
            if (ObjectState == ObjectState.Deleted)
                throw (new InvalidOperationException("Operation failed. Object in 'Deleted' state may not be marked unmodified."));

            if (AttributeID == null)
                throw (new InvalidOperationException("Operation failed. Object with null 'AttributeID' property may not be marked unmodified."));

            ObjectState = ObjectState.Unmodified;
        }

        public override void MarkDeleted()
        {
            if (ObjectState == ObjectState.New)
                throw (new InvalidOperationException("Operation failed. Object in 'New' state may not be marked deleted."));

            if (ObjectState != ObjectState.Deleted)
            {
                ObjectState = ObjectState.Deleted;

                foreach (EAVValue value in values)
                    value.MarkDeleted();
            }
        }
    }
    #endregion

    [DataContract(IsReference = true)]
    public class EAVEntity : EAVObject, EAV.Model.IEAVEntity
    {
        public EAVEntity()
        {
            subjects = new ObservableCollection<EAVSubject>();
            subjects.CollectionChanged += Subjects_CollectionChanged;
        }

        private void Subjects_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                    if (ObjectState != ObjectState.New) ObjectState = ObjectState.Modified;

                    if (e.OldItems != null)
                    {
                        foreach (EAVSubject subject in e.OldItems)
                        {
                            if (subject.Entity == this)
                            {
                                subject.Entity = null;
                            }
                        }
                    }

                    if (e.NewItems != null)
                    {
                        foreach (EAVSubject subject in e.NewItems)
                        {
                            if (subject.Entity != this)
                            {
                                subject.Entity = this;
                            }
                        }
                    }

                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Move:
                    break;
            }
        }

        [DataMember(Name = "EntityID")]
        protected int? entityID;
        [IgnoreDataMember]
        public int? EntityID
        {
            get
            {
                return (entityID);
            }
            set
            {
                if (ObjectState == ObjectState.New || (ObjectState == ObjectState.Unmodified && entityID == null))
                {
                    entityID = value;
                }
                else
                {
                    throw (new InvalidOperationException("This property has already been set."));
                }
            }
        }

        [DataMember(Name = "Descriptor")]
        private string descriptor;
        [IgnoreDataMember]
        public string Descriptor
        {
            get
            {
                return (descriptor);
            }
            set
            {
                if (descriptor != value)
                {
                    if (ObjectState == ObjectState.Deleted)
                        throw (new InvalidOperationException("Operation failed. Property 'Descriptor' may not be modified when object in 'Deleted' state."));

                    descriptor = value;

                    if (ObjectState != ObjectState.New) ObjectState = ObjectState.Modified;
                }
            }
        }

        [DataMember(Name = "Subjects")]
        private ObservableCollection<EAVSubject> subjects;
        [IgnoreDataMember]
        public ICollection<EAVSubject> Subjects
        {
            get { if (ObjectState != ObjectState.Deleted) return (subjects); else return (new ReadOnlyObservableCollection<EAVSubject>(subjects)); }
        }

        public override void MarkUnmodified()
        {
            if (ObjectState == ObjectState.Deleted)
                throw (new InvalidOperationException("Operation failed. Object in 'Deleted' state may not be marked created."));

            if (EntityID == null)
                throw (new InvalidOperationException("Operation failed. Object with null 'EntityID' property may not be marked unmodified."));

            ObjectState = ObjectState.Unmodified;
        }

        public override void MarkDeleted()
        {
            if (ObjectState == ObjectState.New)
                throw (new InvalidOperationException("Operation failed. Object in 'New' state may not be marked deleted."));

            if (ObjectState != ObjectState.Deleted)
            {
                ObjectState = ObjectState.Deleted;

                foreach (EAVSubject subject in subjects)
                    subject.MarkDeleted();
            }
        }
    }

    #region Data Objects
    [DataContract(IsReference = true)]
    public class EAVSubject : EAVDataObject, EAV.Model.IEAVSubject
    {
        public EAVSubject()
        {
            instances = new ObservableCollection<EAVRootInstance>();
            instances.CollectionChanged += Instances_CollectionChanged;
        }

        private void Instances_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                    if (ObjectState != ObjectState.New) ObjectState = ObjectState.Modified;

                    if (e.OldItems != null)
                    {
                        foreach (EAVInstance instance in e.OldItems)
                        {
                            if (instance.Subject == this)
                            {
                                instance.Subject = null;
                            }
                        }
                    }

                    if (e.NewItems != null)
                    {
                        foreach (EAVInstance instance in e.NewItems)
                        {
                            if (instance.Subject != this)
                            {
                                instance.Subject = this;
                            }
                        }
                    }

                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Move:
                    break;
            }
        }

        [DataMember(Name = "SubjectID")]
        protected int? subjectID;
        [IgnoreDataMember]
        public int? SubjectID
        {
            get
            {
                return (subjectID);
            }
            set
            {
                if (ObjectState == ObjectState.New || (ObjectState == ObjectState.Unmodified && subjectID == null))
                {
                    subjectID = value;
                }
                else
                {
                    throw (new InvalidOperationException("This property has already been set."));
                }
            }
        }

        public int? EntityID { get { return (Entity != null ? Entity.EntityID : null); } }

        public int? ContextID { get { return (Context != null ? Context.ContextID : null); } }

        [DataMember(Name = "Entity")]
        protected EAVEntity entity;
        [IgnoreDataMember]
        public EAVEntity Entity
        {
            get
            {
                if (entity != null && !entity.Subjects.Contains(this))
                {
                    entity = null;
                }

                return (entity);
            }
            set
            {
                if (entity != value)
                {
                    if (ObjectState == ObjectState.Deleted)
                        throw (new InvalidOperationException("Operation failed. Property 'Entity' may not be modified when object in 'Deleted' state."));

                    if (entity != null && entity.Subjects.Contains(this))
                    {
                        entity.Subjects.Remove(this);
                    }

                    entity = value;
                    if (ObjectState != ObjectState.New) ObjectState = ObjectState.Modified;

                    if (entity != null && !entity.Subjects.Contains(this))
                    {
                        entity.Subjects.Add(this);
                    }
                }
            }
        }

        [DataMember(Name = "Context")]
        protected EAVContext context;
        [IgnoreDataMember]
        public EAVContext Context
        {
            get
            {
                if (context != null && !context.Subjects.Contains(this))
                {
                    context = null;
                }

                return (context);
            }
            set
            {
                if (context != value)
                {
                    if (ObjectState == ObjectState.Deleted)
                        throw (new InvalidOperationException("Operation failed. Property 'Context' may not be modified when object in 'Deleted' state."));

                    if (context != null && context.Subjects.Contains(this))
                    {
                        context.Subjects.Remove(this);
                    }

                    context = value;
                    if (ObjectState != ObjectState.New) ObjectState = ObjectState.Modified;

                    if (context != null && !context.Subjects.Contains(this))
                    {
                        context.Subjects.Add(this);
                    }
                }
            }
        }

        [DataMember(Name = "Identifier")]
        private string identifier;
        [IgnoreDataMember]
        public string Identifier
        {
            get
            {
                return (identifier);
            }
            set
            {
                if (identifier != value)
                {
                    if (ObjectState == ObjectState.Deleted)
                        throw (new InvalidOperationException("Operation failed. Property 'Identifier' may not be modified when object in 'Deleted' state."));

                    identifier = value;

                    if (ObjectState != ObjectState.New) ObjectState = ObjectState.Modified;
                }
            }
        }

        [DataMember(Name = "Instances")]
        private ObservableCollection<EAVRootInstance> instances;
        [IgnoreDataMember]
        public ICollection<EAVRootInstance> Instances
        {
            get { if (ObjectState != ObjectState.Deleted) return (instances); else return (new ReadOnlyObservableCollection<EAVRootInstance>(instances)); }
        }

        public override void MarkUnmodified()
        {
            if (ObjectState == ObjectState.Deleted)
                throw (new InvalidOperationException("Operation failed. Object in 'Deleted' state may not be marked created."));

            if (SubjectID == null)
                throw (new InvalidOperationException("Operation failed. Object with null 'SubjectID' property may not be marked unmodified."));

            ObjectState = ObjectState.Unmodified;
        }

        public override void MarkDeleted()
        {
            if (ObjectState == ObjectState.New)
                throw (new InvalidOperationException("Operation failed. Object in 'New' state may not be marked deleted."));

            if (ObjectState != ObjectState.Deleted)
            {
                ObjectState = ObjectState.Deleted;

                foreach (EAVInstance instance in instances)
                    instance.MarkDeleted();
            }
        }
    }

    [DataContract(IsReference = true)]
    [KnownType(typeof(EAVRootInstance))]
    [KnownType(typeof(EAVChildInstance))]
    public abstract class EAVInstance : EAVDataObject, EAV.Model.IEAVInstance
    {
        public EAVInstance()
        {
            childInstances = new ObservableCollection<EAVChildInstance>();
            childInstances.CollectionChanged += ChildInstances_CollectionChanged;

            values = new ObservableCollection<EAVValue>();
            values.CollectionChanged += Values_CollectionChanged;
        }

        private void ChildInstances_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
                case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
                case System.Collections.Specialized.NotifyCollectionChangedAction.Replace:
                case System.Collections.Specialized.NotifyCollectionChangedAction.Reset:
                    if (ObjectState != ObjectState.New) ObjectState = ObjectState.Modified;

                    if (e.OldItems != null)
                    {
                        foreach (EAVInstance instance in e.OldItems)
                        {
                            if (instance.ParentInstance == this)
                            {
                                instance.ParentInstance = null;
                            }
                        }
                    }

                    if (e.NewItems != null)
                    {
                        foreach (EAVInstance instance in e.NewItems)
                        {
                            if (instance.ParentInstance != this)
                            {
                                instance.ParentInstance = this;
                            }
                        }
                    }

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
                    if (ObjectState != ObjectState.New) ObjectState = ObjectState.Modified;

                    if (e.OldItems != null)
                    {
                        foreach (EAVValue value in e.OldItems)
                        {
                            if (value.Instance == this)
                            {
                                value.Instance = null;
                            }
                        }
                    }

                    if (e.NewItems != null)
                    {
                        foreach (EAVValue value in e.NewItems)
                        {
                            if (value.Instance != this)
                            {
                                value.Instance = this;
                            }
                        }
                    }

                    break;
                case System.Collections.Specialized.NotifyCollectionChangedAction.Move:
                    break;
            }
        }

        [DataMember(Name = "InstanceID")]
        protected int? instanceID;
        [IgnoreDataMember]
        public int? InstanceID
        {
            get
            {
                return (instanceID);
            }
            set
            {
                if (ObjectState == ObjectState.New || (ObjectState == ObjectState.Unmodified && instanceID == null))
                {
                    instanceID = value;
                }
                else
                {
                    throw (new InvalidOperationException("This property has already been set."));
                }
            }
        }

        public int? ParentInstanceID { get { return (ParentInstance != null ? ParentInstance.InstanceID : null); } }

        public int? SubjectID { get { return (Subject != null ? Subject.SubjectID : null); } }

        public int? ContainerID { get { return (Container != null ? Container.ContainerID : null); } }

        [DataMember(Name = "ParentInstance")]
        protected EAVInstance parentInstance;
        [IgnoreDataMember]
        public abstract EAVInstance ParentInstance
        {
            get;
            set;
        }

        [DataMember(Name = "Subject")]
        protected EAVSubject subject;
        [IgnoreDataMember]
        public abstract EAVSubject Subject
        {
            get;
            set;
        }

        [DataMember(Name = "Container")]
        protected EAVContainer container;
        [IgnoreDataMember]
        public EAVContainer Container
        {
            get
            {
                if (container != null && !container.Instances.Contains(this))
                {
                    container = null;
                }

                return (container);
            }
            set
            {
                if (container != value)
                {
                    if (ObjectState == ObjectState.Deleted)
                        throw (new InvalidOperationException("Operation failed. Property 'RawValue' may not be modified when object in 'Deleted' state."));

                    if (container != null && container.Instances.Contains(this))
                    {
                        container.Instances.Remove(this);
                    }

                    container = value;
                    if (ObjectState != ObjectState.New) ObjectState = ObjectState.Modified;

                    if (container != null && !container.Instances.Contains(this))
                    {
                        container.Instances.Add(this);
                    }
                }
            }
        }

        [DataMember(Name = "ChildInstances")]
        private ObservableCollection<EAVChildInstance> childInstances;
        [IgnoreDataMember]
        public ICollection<EAVChildInstance> ChildInstances
        {
            get { if (ObjectState != ObjectState.Deleted) return (childInstances); else return (new ReadOnlyObservableCollection<EAVChildInstance>(childInstances)); }
        }

        [DataMember(Name = "Values")]
        private ObservableCollection<EAVValue> values;
        [IgnoreDataMember]
        public ICollection<EAVValue> Values
        {
            get { if (ObjectState != ObjectState.Deleted) return (values); else return (new ReadOnlyObservableCollection<EAVValue>(values)); }
        }

        protected void SetStateRecursive(ObjectState state)
        {
            this.ObjectState = state;
            foreach (EAVInstance childInstance in ChildInstances)
                childInstance.SetStateRecursive(state);
        }

        public override void MarkUnmodified()
        {
            if (ObjectState == ObjectState.Deleted)
                throw (new InvalidOperationException("Operation failed. Object in 'Deleted' state may not be marked created."));

            if (InstanceID == null)
                throw (new InvalidOperationException("Operation failed. Object with null 'InstanceID' property may not be marked unmodified."));

            ObjectState = ObjectState.Unmodified;
        }

        public override void MarkDeleted()
        {
            if (ObjectState == ObjectState.New)
                throw (new InvalidOperationException("Operation failed. Object in 'New' state may not be marked deleted."));

            if (ObjectState != ObjectState.Deleted)
            {
                ObjectState = ObjectState.Deleted;

                foreach (EAVValue value in values)
                    value.MarkDeleted();

                foreach (EAVInstance childInstance in childInstances)
                    childInstance.MarkDeleted();
            }
        }
    }

    public class EAVRootInstance : EAVInstance
    {
        public static EAVRootInstance Create(EAVRootContainer container, EAVSubject subject)
        {
            EAVRootInstance instance = new EAVRootInstance()
            {
                Container = container,
                Subject = subject,
            };

            foreach (EAVAttribute attribute in container.Attributes)
            {
                EAVValue.Create(attribute, instance);
            }

            foreach (EAVChildContainer childContainer in container.ChildContainers)
            {
                EAVChildInstance.Create(childContainer, subject, instance);
            }

            return (instance);
        }

        public EAVRootInstance()
        {
            parentInstance = null;
        }

        public override EAVSubject Subject
        {
            get
            {
                if (subject != null && !subject.Instances.Contains(this))
                {
                    subject = null;
                }

                return (subject);
            }
            set
            {
                if (subject != value)
                {
                    if (ObjectState == ObjectState.Deleted)
                        throw (new InvalidOperationException("Operation failed. Property 'Subject' may not be modified when object in 'Deleted' state."));

                    if (subject != null && subject.Instances.Contains(this))
                    {
                        subject.Instances.Remove(this);
                    }

                    subject = value;
                    SetStateRecursive(ObjectState != ObjectState.New ? ObjectState.Modified : ObjectState);

                    if (subject != null && !subject.Instances.Contains(this))
                    {
                        subject.Instances.Add(this);
                    }
                }
            }
        }

        public override EAVInstance ParentInstance
        {
            get
            {
                return (null);
            }
            set
            {
                if (ObjectState == ObjectState.Deleted)
                    throw (new InvalidOperationException("Operation failed. Property 'ParentInstance' may not be modified when object in 'Deleted' state."));

                if (value != null) throw (new InvalidOperationException("The ParentInstance property may only accept 'null' as a value."));
            }
        }
    }

    public class EAVChildInstance : EAVInstance
    {
        public static EAVChildInstance Create(EAVChildContainer container, EAVSubject subject, EAVInstance parentInstance)
        {
            EAVChildInstance instance = new EAVChildInstance()
            {
                ParentInstance = parentInstance,
                Container = container,
            };

            foreach (EAVAttribute attribute in container.Attributes)
            {
                EAVValue.Create(attribute, instance);
            }

            foreach (EAVChildContainer childContainer in container.ChildContainers)
            {
                EAVChildInstance.Create(childContainer, subject, instance);
            }

            return (instance);
        }

        public EAVChildInstance() { }

        public override EAVSubject Subject
        {
            get
            {
                subject = parentInstance != null ? parentInstance.Subject : null;
                return (subject);
            }
            set
            {
                if (ObjectState == ObjectState.Deleted)
                    throw (new InvalidOperationException("Operation failed. Property 'Subject' may not be modified when object in 'Deleted' state."));

                throw (new InvalidOperationException("The Subject property must be set on the root instance for this instance."));
            }
        }

        public override EAVInstance ParentInstance
        {
            get
            {
                if (parentInstance != null && !parentInstance.ChildInstances.Contains(this))
                {
                    parentInstance = null;
                }

                return (parentInstance);
            }
            set
            {
                if (parentInstance != value)
                {
                    if (ObjectState == ObjectState.Deleted)
                        throw (new InvalidOperationException("Operation failed. Property 'ParentInstance' may not be modified when object in 'Deleted' state."));

                    if (parentInstance != null && parentInstance.ChildInstances.Contains(this))
                    {
                        parentInstance.ChildInstances.Remove(this);
                    }

                    parentInstance = value;
                    if (ObjectState != ObjectState.New) ObjectState = ObjectState.Modified;

                    if (parentInstance != null && !parentInstance.ChildInstances.Contains(this))
                    {
                        parentInstance.ChildInstances.Add(this);
                    }
                }
            }
        }
    }

    [DataContract(IsReference = true)]
    public class EAVValue : EAVDataObject, EAV.Model.IEAVValue
    {
        public static EAVValue Create(EAVAttribute attribute, EAVInstance instance)
        {
            EAVValue value = new EAVValue()
            {
                Attribute = attribute,
                Instance = instance,
            };

            return (value);
        }

        public EAVValue() { }

        public int? InstanceID { get { return (Instance != null ? Instance.InstanceID : null); } }

        public int? AttributeID { get { return (Attribute != null ? Attribute.AttributeID : null); } }

        [DataMember(Name = "Instance")]
        protected EAVInstance instance;
        [IgnoreDataMember]
        public EAVInstance Instance
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
        protected EAVAttribute attribute;
        [IgnoreDataMember]
        public EAVAttribute Attribute
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

        [DataMember(Name = "Units")]
        protected string units;
        [IgnoreDataMember]
        public string Units
        {
            get
            {
                return (units);
            }
            set
            {
                if (units != value)
                {
                    if (ObjectState == ObjectState.Deleted)
                        throw (new InvalidOperationException("Operation failed. Property 'Units' may not be modified when object in 'Deleted' state."));

                    units = value;

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
    #endregion
}
