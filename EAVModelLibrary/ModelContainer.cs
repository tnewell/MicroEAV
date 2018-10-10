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


namespace EAVModelLibrary
{
    [DataContract(IsReference = true)]
    [KnownType(typeof(ModelRootContainer))]
    [KnownType(typeof(ModelChildContainer))]
    public abstract class ModelContainer : ModelObject, EAV.Model.IModelContainer
    {
        public ModelContainer()
        {
            childContainers = new ObservableCollection<EAV.Model.IModelChildContainer>();
            childContainers.CollectionChanged += ChildContainers_CollectionChanged;

            attributes = new ObservableCollection<EAV.Model.IModelAttribute>();
            attributes.CollectionChanged += Attributes_CollectionChanged;

            instances = new ObservableCollection<EAV.Model.IModelInstance>();
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
                    if (ObjectState == EAV.Model.ObjectState.Unmodified) ObjectState = EAV.Model.ObjectState.Modified;

                    if (e.OldItems != null)
                    {
                        foreach (ModelContainer container in e.OldItems)
                        {
                            if (container.ParentContainer == this)
                            {
                                container.ParentContainer = null;
                            }
                        }
                    }

                    if (e.NewItems != null)
                    {
                        foreach (ModelContainer container in e.NewItems)
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
                    if (ObjectState == EAV.Model.ObjectState.Unmodified) ObjectState = EAV.Model.ObjectState.Modified;

                    if (e.OldItems != null)
                    {
                        foreach (ModelAttribute attribute in e.OldItems)
                        {
                            if (attribute.Container == this)
                            {
                                attribute.Container = null;
                            }
                        }
                    }

                    if (e.NewItems != null)
                    {
                        foreach (ModelAttribute attribute in e.NewItems)
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
                    if (ObjectState == EAV.Model.ObjectState.Unmodified) ObjectState = EAV.Model.ObjectState.Modified;

                    if (e.OldItems != null)
                    {
                        foreach (ModelInstance instance in e.OldItems)
                        {
                            if (instance.Container == this)
                            {
                                instance.Container = null;
                            }
                        }
                    }

                    if (e.NewItems != null)
                    {
                        foreach (ModelInstance instance in e.NewItems)
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
                if (ObjectState == EAV.Model.ObjectState.New || (ObjectState == EAV.Model.ObjectState.Unmodified && containerID == null))
                {
                    containerID = value;
                }
                else
                {
                    throw (new InvalidOperationException("This property has already been set."));
                }
            }
        }

        [DataMember(Name = "ContextID")]
        protected int? contextID;
        [IgnoreDataMember]
        public int? ContextID { get { return (Context != null ? Context.ContextID : contextID); } }

        [DataMember(Name = "ParentContainerID")]
        protected int? parentContainerID;
        [IgnoreDataMember]
        public int? ParentContainerID { get { return (ParentContainer != null ? ParentContainer.ContainerID : parentContainerID); } }

        [DataMember(Name = "Context")]
        protected EAV.Model.IModelContext context;
        [IgnoreDataMember]
        public abstract EAV.Model.IModelContext Context
        {
            get;
            set;
        }

        [DataMember(Name = "ParentContainer")]
        protected EAV.Model.IModelContainer parentContainer;
        [IgnoreDataMember]
        public abstract EAV.Model.IModelContainer ParentContainer
        {
            get;
            set;
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
                    if (ObjectState == EAV.Model.ObjectState.Deleted)
                        throw (new InvalidOperationException("Operation failed. Property 'IsRepeating' may not be modified when object in 'Deleted' state."));

                    isRepeating = value;

                    if (ObjectState != EAV.Model.ObjectState.New) ObjectState = EAV.Model.ObjectState.Modified;
                }
            }
        }

        [DataMember(Name = "ChildContainers")]
        private ObservableCollection<EAV.Model.IModelChildContainer> childContainers;
        [IgnoreDataMember]
        public ICollection<EAV.Model.IModelChildContainer> ChildContainers
        {
            get { return (childContainers); }
        }

        [DataMember(Name = "Attributes")]
        private ObservableCollection<EAV.Model.IModelAttribute> attributes;
        [IgnoreDataMember]
        public ICollection<EAV.Model.IModelAttribute> Attributes
        {
            get { return (attributes); }
        }

        [DataMember(Name = "Instances")]
        private ObservableCollection<EAV.Model.IModelInstance> instances;
        [IgnoreDataMember]
        public ICollection<EAV.Model.IModelInstance> Instances
        {
            get { return (instances); }
        }

        protected void SetStateRecursive(EAV.Model.ObjectState state)
        {
            this.ObjectState = state;
            foreach (ModelContainer childContainer in ChildContainers)
                childContainer.SetStateRecursive(state);
        }

        public override void MarkUnmodified()
        {
            if (ObjectState == EAV.Model.ObjectState.Deleted)
                throw (new InvalidOperationException("Operation failed. Object in 'Deleted' state may not be marked created."));

            if (ContainerID == null)
                throw (new InvalidOperationException("Operation failed. Object with null 'ContainerID' property may not be marked unmodified."));

            ObjectState = EAV.Model.ObjectState.Unmodified;
        }

        public override void MarkDeleted()
        {
            if (ObjectState == EAV.Model.ObjectState.New)
                throw (new InvalidOperationException("Operation failed. Object in 'New' state may not be marked deleted."));

            if (ObjectState != EAV.Model.ObjectState.Deleted)
            {
                ObjectState = EAV.Model.ObjectState.Deleted;

                foreach (ModelAttribute attribute in attributes)
                    attribute.MarkDeleted();

                foreach (ModelContainer container in childContainers)
                    container.MarkDeleted();

                foreach (ModelInstance instance in instances)
                    instance.MarkDeleted();
            }
        }
    }

    [DataContract(IsReference = true)]
    public class ModelRootContainer : ModelContainer, EAV.Model.IModelRootContainer
    {
        public ModelRootContainer()
        {
            parentContainer = null;
        }

        public override EAV.Model.IModelContext Context
        {
            get
            {
                if (context != null && !context.Containers.Contains(this))
                {
                    context = null;
                    contextID = null;
                }

                return (context);
            }
            set
            {
                if (context != value)
                {
                    if (ObjectState == EAV.Model.ObjectState.Deleted)
                        throw (new InvalidOperationException("Operation failed. Property 'Context' may not be modified when object in 'Deleted' state."));
                    else if (value != null && value.ObjectState == EAV.Model.ObjectState.Deleted)
                        throw (new InvalidOperationException("Operation failed. Property 'Context' may not be assigned object in 'Deleted' state."));

                    if (context != null && context.Containers.Contains(this))
                    {
                        context.Containers.Remove(this);
                    }

                    context = value;
                    contextID = context.ContextID != null ? context.ContextID : null;

                    SetStateRecursive(ObjectState != EAV.Model.ObjectState.New ? EAV.Model.ObjectState.Modified : ObjectState);

                    if (context != null && !context.Containers.Contains(this))
                    {
                        context.Containers.Add(this);
                    }
                }
                else if (value != null && value.ContextID != contextID)
                {
                    contextID = value.ContextID;

                    if (ObjectState != EAV.Model.ObjectState.New) ObjectState = EAV.Model.ObjectState.Modified;
                }
            }
        }

        public override EAV.Model.IModelContainer ParentContainer
        {
            get
            {
                return (null);
            }
            set
            {
                if (ObjectState == EAV.Model.ObjectState.Deleted)
                    throw (new InvalidOperationException("Operation failed. Property 'ParentContainer' may not be modified when object in 'Deleted' state."));

                if (value != null) throw (new InvalidOperationException("The ParentContainer property may only accept 'null' as a value."));
            }
        }
    }

    [DataContract(IsReference = true)]
    public class ModelChildContainer : ModelContainer, EAV.Model.IModelChildContainer
    {
        public ModelChildContainer()
        {
        }

        public override EAV.Model.IModelContext Context
        {
            get
            {
                context = parentContainer != null ? parentContainer.Context : null;
                contextID = context != null ? context.ContextID : null;

                return (context);
            }
            set
            {
                if (ObjectState == EAV.Model.ObjectState.Deleted)
                    throw (new InvalidOperationException("Operation failed. Property 'Context' may not be modified when object in 'Deleted' state."));

                throw (new InvalidOperationException("The Context property must be set on the root container for this container."));
            }
        }

        public override EAV.Model.IModelContainer ParentContainer
        {
            get
            {
                if (parentContainer != null && !parentContainer.ChildContainers.Contains(this))
                {
                    parentContainer = null;
                    parentContainerID = null;
                }

                return (parentContainer);
            }
            set
            {
                if (parentContainer != value)
                {
                    if (ObjectState == EAV.Model.ObjectState.Deleted)
                        throw (new InvalidOperationException("Operation failed. Property 'ParentContainer' may not be modified when object in 'Deleted' state."));
                    else if (value != null && value.ObjectState == EAV.Model.ObjectState.Deleted)
                        throw (new InvalidOperationException("Operation failed. Property 'ParentContainer' may not be assigned object in 'Deleted' state."));

                    if (parentContainer != null && parentContainer.ChildContainers.Contains(this))
                    {
                        parentContainer.ChildContainers.Remove(this);
                    }

                    parentContainer = value;
                    parentContainerID = parentContainer != null ? parentContainer.ContainerID : null;

                    if (ObjectState != EAV.Model.ObjectState.New) ObjectState = EAV.Model.ObjectState.Modified;

                    if (parentContainer != null && !parentContainer.ChildContainers.Contains(this))
                    {
                        parentContainer.ChildContainers.Add(this);
                    }
                }
                else if (value != null && value.ParentContainerID != parentContainerID)
                {
                    parentContainerID = value.ParentContainerID;

                    if (ObjectState != EAV.Model.ObjectState.New) ObjectState = EAV.Model.ObjectState.Modified;
                }
            }
        }
    }
}
