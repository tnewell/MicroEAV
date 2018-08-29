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
    public class ModelSubject : ModelObject, EAVFramework.Model.IModelSubject
    {
        public ModelSubject()
        {
            instances = new ObservableCollection<IModelRootInstance>();
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
                        foreach (ModelInstance instance in e.OldItems)
                        {
                            if (instance.Subject == this)
                            {
                                instance.Subject = null;
                            }
                        }
                    }

                    if (e.NewItems != null)
                    {
                        foreach (ModelInstance instance in e.NewItems)
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
        protected IModelEntity entity;
        [IgnoreDataMember]
        public IModelEntity Entity
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
        protected IModelContext context;
        [IgnoreDataMember]
        public IModelContext Context
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
        private ObservableCollection<IModelRootInstance> instances;
        [IgnoreDataMember]
        public ICollection<IModelRootInstance> Instances
        {
            get { if (ObjectState != ObjectState.Deleted) return (instances); else return (new ReadOnlyObservableCollection<IModelRootInstance>(instances)); }
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

                foreach (ModelInstance instance in instances)
                    instance.MarkDeleted();
            }
        }
    }
}
