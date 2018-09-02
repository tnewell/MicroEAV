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


namespace EAV.Model
{
    [DataContract(IsReference = true)]
    public class ModelEntity : ModelObject, EAV.Model.IModelEntity
    {
        public ModelEntity()
        {
            subjects = new ObservableCollection<IModelSubject>();
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
                        foreach (ModelSubject subject in e.OldItems)
                        {
                            if (subject.Entity == this)
                            {
                                subject.Entity = null;
                            }
                        }
                    }

                    if (e.NewItems != null)
                    {
                        foreach (ModelSubject subject in e.NewItems)
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
        private ObservableCollection<IModelSubject> subjects;
        [IgnoreDataMember]
        public ICollection<IModelSubject> Subjects
        {
            get { if (ObjectState != ObjectState.Deleted) return (subjects); else return (new ReadOnlyObservableCollection<IModelSubject>(subjects)); }
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

                foreach (ModelSubject subject in subjects)
                    subject.MarkDeleted();
            }
        }
    }
}
