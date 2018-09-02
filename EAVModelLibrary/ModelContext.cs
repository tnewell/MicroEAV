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
    public class ModelContext : ModelObject, EAV.Model.IModelContext
    {
        public ModelContext()
        {
            containers = new ObservableCollection<EAV.Model.IModelRootContainer>();
            containers.CollectionChanged += Containers_CollectionChanged;

            subjects = new ObservableCollection<EAV.Model.IModelSubject>();
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
                    if (ObjectState != EAV.Model.ObjectState.New) ObjectState = EAV.Model.ObjectState.Modified;

                    if (e.OldItems != null)
                    {
                        foreach (ModelContainer container in e.OldItems)
                        {
                            if (container.Context == this)
                            {
                                container.Context = null;
                            }
                        }
                    }

                    if (e.NewItems != null)
                    {
                        foreach (ModelContainer container in e.NewItems)
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
                    if (ObjectState != EAV.Model.ObjectState.New) ObjectState = EAV.Model.ObjectState.Modified;

                    if (e.OldItems != null)
                    {
                        foreach (ModelSubject subject in e.OldItems)
                        {
                            if (subject.Context == this)
                            {
                                subject.Context = null;
                            }
                        }
                    }

                    if (e.NewItems != null)
                    {
                        foreach (ModelSubject subject in e.NewItems)
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
                if (ObjectState == EAV.Model.ObjectState.New || (ObjectState == EAV.Model.ObjectState.Unmodified && contextID == null))
                {
                    contextID = value;
                }
                else
                {
                    throw (new InvalidOperationException("This property has already been set."));
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

        [DataMember(Name = "Containers")]
        private ObservableCollection<EAV.Model.IModelRootContainer> containers;
        [IgnoreDataMember]
        public ICollection<EAV.Model.IModelRootContainer> Containers
        {
            get { if (ObjectState != EAV.Model.ObjectState.Deleted) return (containers); else return (new ReadOnlyObservableCollection<EAV.Model.IModelRootContainer>(containers)); }
        }

        [DataMember(Name = "Subjects")]
        private ObservableCollection<EAV.Model.IModelSubject> subjects;
        [IgnoreDataMember]
        public ICollection<EAV.Model.IModelSubject> Subjects
        {
            get { if (ObjectState != EAV.Model.ObjectState.Deleted) return (subjects); else return (new ReadOnlyObservableCollection<EAV.Model.IModelSubject>(subjects)); }
        }

        public override void MarkUnmodified()
        {
            if (ObjectState == EAV.Model.ObjectState.Deleted)
                throw (new InvalidOperationException("Operation failed. Object in 'Deleted' state may not be marked unmodified."));

            if (ContextID == null)
                throw (new InvalidOperationException("Operation failed. Object with null 'ContextID' property may not be marked unmodified."));

            ObjectState = EAV.Model.ObjectState.Unmodified;
        }

        public override void MarkDeleted()
        {
            if (ObjectState == EAV.Model.ObjectState.New)
                throw (new InvalidOperationException("Operation failed. Object in 'New' state may not be marked deleted."));

            if (ObjectState != EAV.Model.ObjectState.Deleted)
            {
                ObjectState = EAV.Model.ObjectState.Deleted;

                foreach (ModelContainer container in containers)
                    container.MarkDeleted();

                foreach (ModelSubject subject in subjects)
                    subject.MarkDeleted();
            }
        }
    }
}
