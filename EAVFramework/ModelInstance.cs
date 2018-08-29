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
    [DataContract(IsReference = true)]
    [KnownType(typeof(ModelRootInstance))]
    [KnownType(typeof(ModelChildInstance))]
    public abstract class ModelInstance : ModelObject, EAVFramework.Model.IModelInstance
    {
        public ModelInstance()
        {
            childInstances = new ObservableCollection<IModelChildInstance>();
            childInstances.CollectionChanged += ChildInstances_CollectionChanged;

            values = new ObservableCollection<IModelValue>();
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
                        foreach (ModelInstance instance in e.OldItems)
                        {
                            if (instance.ParentInstance == this)
                            {
                                instance.ParentInstance = null;
                            }
                        }
                    }

                    if (e.NewItems != null)
                    {
                        foreach (ModelInstance instance in e.NewItems)
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
                        foreach (ModelValue value in e.OldItems)
                        {
                            if (value.Instance == this)
                            {
                                value.Instance = null;
                            }
                        }
                    }

                    if (e.NewItems != null)
                    {
                        foreach (ModelValue value in e.NewItems)
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
        protected IModelInstance parentInstance;
        [IgnoreDataMember]
        public abstract IModelInstance ParentInstance
        {
            get;
            set;
        }

        [DataMember(Name = "Subject")]
        protected IModelSubject subject;
        [IgnoreDataMember]
        public abstract IModelSubject Subject
        {
            get;
            set;
        }

        [DataMember(Name = "Container")]
        protected IModelContainer container;
        [IgnoreDataMember]
        public IModelContainer Container
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
        private ObservableCollection<IModelChildInstance> childInstances;
        [IgnoreDataMember]
        public ICollection<IModelChildInstance> ChildInstances
        {
            get { if (ObjectState != ObjectState.Deleted) return (childInstances); else return (new ReadOnlyObservableCollection<IModelChildInstance>(childInstances)); }
        }

        [DataMember(Name = "Values")]
        private ObservableCollection<IModelValue> values;
        [IgnoreDataMember]
        public ICollection<IModelValue> Values
        {
            get { if (ObjectState != ObjectState.Deleted) return (values); else return (new ReadOnlyObservableCollection<IModelValue>(values)); }
        }

        protected void SetStateRecursive(ObjectState state)
        {
            this.ObjectState = state;
            foreach (ModelInstance childInstance in ChildInstances)
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

                foreach (ModelValue value in values)
                    value.MarkDeleted();

                foreach (ModelInstance childInstance in childInstances)
                    childInstance.MarkDeleted();
            }
        }
    }

    public class ModelRootInstance : ModelInstance, EAVFramework.Model.IModelRootInstance
    {
        public static IModelRootInstance Create(IModelRootContainer container, IModelSubject subject)
        {
            ModelRootInstance instance = new ModelRootInstance()
            {
                Container = container,
                Subject = subject,
            };

            foreach (ModelAttribute attribute in container.Attributes)
            {
                ModelValue.Create(attribute, instance);
            }

            foreach (ModelChildContainer childContainer in container.ChildContainers)
            {
                ModelChildInstance.Create(childContainer, subject, instance);
            }

            return (instance);
        }

        public ModelRootInstance()
        {
            parentInstance = null;
        }

        public override IModelSubject Subject
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

        public override IModelInstance ParentInstance
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

    public class ModelChildInstance : ModelInstance, EAVFramework.Model.IModelChildInstance
    {
        public static IModelChildInstance Create(IModelChildContainer container, IModelSubject subject, IModelInstance parentInstance)
        {
            ModelChildInstance instance = new ModelChildInstance()
            {
                ParentInstance = parentInstance,
                Container = container,
            };

            foreach (ModelAttribute attribute in container.Attributes)
            {
                ModelValue.Create(attribute, instance);
            }

            foreach (ModelChildContainer childContainer in container.ChildContainers)
            {
                ModelChildInstance.Create(childContainer, subject, instance);
            }

            return (instance);
        }

        public ModelChildInstance() { }

        public override IModelSubject Subject
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

        public override IModelInstance ParentInstance
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
}
