using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace EAVSandbox
{
    public static class ModelExtensions
    {
        public static async Task<HttpResponseMessage> PatchAsync<T>(this HttpClient client, string requestUri, T value, MediaTypeFormatter formatter)
        {
            return (await client.SendAsync(new HttpRequestMessage(new HttpMethod("PATCH"), requestUri) { Content = new ObjectContent<T>(value, formatter) }));
        }

        public static async Task<HttpResponseMessage> PatchAsJsonAsync<T>(this HttpClient client, string requestUri, T value)
        {
            return (await PatchAsync<T>(client, requestUri, value, new JsonMediaTypeFormatter()));
        }

        public static async Task<HttpResponseMessage> PatchAsXmlAsync<T>(this HttpClient client, string requestUri, T value)
        {
            return (await PatchAsync<T>(client, requestUri, value, new XmlMediaTypeFormatter()));
        }
    }
}

namespace EAVSandbox.Model
{
    public enum ObjectState { New, Unmodified, Modified, Deleted }

    [DataContract(IsReference = true)]
    [KnownType(typeof(EAVMetadataObject))]
    public abstract class EAVObject
    {
        public EAVObject() { }

        [DataMember()]
        public ObjectState ObjectState { get; set; }

        public abstract void MarkCreated(EAVObject obj);

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
                if (name != value && ObjectState != ObjectState.Deleted)
                {
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
                if (dataName != value && ObjectState != ObjectState.Deleted)
                {
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
                if (displayText != value && ObjectState != ObjectState.Deleted)
                {
                    displayText = value;
                    if (ObjectState != ObjectState.New) ObjectState = ObjectState.Modified;
                }
            }
        }
    }

    [DataContract(IsReference = true)]
    public abstract class EAVDataObject : EAVObject
    {
        public EAVDataObject() { }
    }

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

        public override void MarkCreated(EAVObject obj)
        {
            if (ObjectState == ObjectState.Deleted)
                throw (new InvalidOperationException("Operation failed. Object in 'Deleted' state may not be marked created."));

            EAV.Model.IEAVContext context = obj as EAV.Model.IEAVContext;

            if (context == null)
                throw (new ArgumentException("Parameter 'obj' must implement the EAV.Model.IEAVContext interface.", "obj"));

            if (context.ContextID == null)
                throw (new InvalidOperationException("Property 'ContextID' of parameter 'obj' may not not be null."));

            if (this.contextID == null)
            {
                contextID = context.ContextID;
                ObjectState = ObjectState.Unmodified;
            }
            else if (this.contextID == context.ContextID)
            {
                ObjectState = ObjectState.Unmodified;
            }
            else
            {
                throw (new InvalidOperationException("Operation failed. Object has already been marked created."));
            }
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

        [DataMember(Name = "ContainerID")]
        protected int? containerID;
        [IgnoreDataMember]
        public int? ContainerID
        {
            get
            {
                return (containerID);
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
                if (isRepeating != value && ObjectState != ObjectState.Deleted)
                {
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

        protected void SetStateRecursive(ObjectState state)
        {
            this.ObjectState = state;
            foreach (EAVContainer childContainer in ChildContainers)
                childContainer.SetStateRecursive(state);
        }

        public override void MarkCreated(EAVObject obj)
        {
            if (ObjectState == ObjectState.Deleted)
                throw (new InvalidOperationException("Operation failed. Object in 'Deleted' state may not be marked created."));

            EAV.Model.IEAVContainer container = obj as EAV.Model.IEAVContainer;

            if (container == null)
                throw (new ArgumentException("Parameter 'obj' must implement the EAV.Model.IEAVContainer interface.", "obj"));

            if (container.ContainerID == null)
                throw (new InvalidOperationException("Property 'ContainerID' of parameter 'obj' may not not be null."));

            if (this.containerID == null)
            {
                containerID = container.ContainerID;
                ObjectState = ObjectState.Unmodified;
            }
            else if (this.containerID == container.ContainerID)
            {
                ObjectState = ObjectState.Unmodified;
            }
            else
            {
                throw (new InvalidOperationException("Operation failed. Object has already been marked created."));
            }
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
                if (context != value && ObjectState != ObjectState.Deleted)
                {
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
                if (parentContainer != value && ObjectState != ObjectState.Deleted)
                {
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
                if (container != value && ObjectState != ObjectState.Deleted)
                {
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
                if (dataType != value && ObjectState != ObjectState.Deleted)
                {
                    dataType = value;
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
                if (isKey != value && ObjectState != ObjectState.Deleted)
                {
                    isKey = value;
                    if (ObjectState != ObjectState.New) ObjectState = ObjectState.Modified;
                }
            }
        }

        public override void MarkCreated(EAVObject obj)
        {
            if (ObjectState == ObjectState.Deleted)
                throw (new InvalidOperationException("Operation failed. Object in 'Deleted' state may not be marked created."));

            EAV.Model.IEAVAttribute attribute = obj as EAV.Model.IEAVAttribute;

            if (attribute == null)
                throw (new ArgumentException("Parameter 'obj' must implement the EAV.Model.IEAVAttribute interface.", "obj"));

            if (attribute.AttributeID == null)
                throw (new InvalidOperationException("Property 'AttributeID' of parameter 'obj' may not not be null."));

            if (this.attributeID == null)
            {
                attributeID = attribute.AttributeID;
                ObjectState = ObjectState.Unmodified;
            }
            else if (this.AttributeID == attribute.AttributeID)
            {
                ObjectState = ObjectState.Unmodified;
            }
            else
            {
                throw (new InvalidOperationException("Operation failed. Object has already been marked created."));
            }
        }

        public override void MarkDeleted()
        {
            if (ObjectState == ObjectState.New)
                throw (new InvalidOperationException("Operation failed. Object in 'New' state may not be marked deleted."));

            if (ObjectState != ObjectState.Deleted)
            {
                ObjectState = ObjectState.Deleted;
            }
        }
    }
    #endregion

    #region Data Objects
    [DataContract(IsReference = true)]
    public class EAVEntity : EAVDataObject, EAV.Model.IEAVEntity
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
                if (descriptor != value && ObjectState != ObjectState.Deleted)
                {
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

        public override void MarkCreated(EAVObject obj)
        {
            if (ObjectState == ObjectState.Deleted)
                throw (new InvalidOperationException("Operation failed. Object in 'Deleted' state may not be marked created."));

            EAV.Model.IEAVEntity entity = obj as EAV.Model.IEAVEntity;

            if (entity == null)
                throw (new ArgumentException("Parameter 'obj' must implement the EAV.Model.IEAVEntity interface.", "obj"));

            if (entity.EntityID == null)
                throw (new InvalidOperationException("Property 'EntityID' of parameter 'obj' may not not be null."));

            if (this.entityID == null)
            {
                entityID = entity.EntityID;
                ObjectState = ObjectState.Unmodified;
            }
            else if (this.EntityID == entity.EntityID)
            {
                ObjectState = ObjectState.Unmodified;
            }
            else
            {
                throw (new InvalidOperationException("Operation failed. Object has already been marked created."));
            }
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

    [DataContract(IsReference = true)]
    public class EAVSubject : EAVDataObject, EAV.Model.IEAVSubject
    {
        public EAVSubject() { }

        [DataMember(Name = "SubjectID")]
        protected int? subjectID;
        [IgnoreDataMember]
        public int? SubjectID
        {
            get
            {
                return (subjectID);
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
                if (entity != value && ObjectState != ObjectState.Deleted)
                {
                    if (entity != null && entity.Subjects.Contains(this))
                    {
                        entity.Subjects.Remove(this);
                    }

                    entity = value;

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
                if (context != value && ObjectState != ObjectState.Deleted)
                {
                    if (context != null && context.Subjects.Contains(this))
                    {
                        context.Subjects.Remove(this);
                    }

                    context = value;

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
                if (identifier != value && ObjectState != ObjectState.Deleted)
                {
                    identifier = value;
                    if (ObjectState != ObjectState.New) ObjectState = ObjectState.Modified;
                }
            }
        }

        public override void MarkCreated(EAVObject obj)
        {
            if (ObjectState == ObjectState.Deleted)
                throw (new InvalidOperationException("Operation failed. Object in 'Deleted' state may not be marked created."));

            EAV.Model.IEAVSubject subject = obj as EAV.Model.IEAVSubject;

            if (subject == null)
                throw (new ArgumentException("Parameter 'obj' must implement the EAV.Model.IEAVSubject interface.", "obj"));

            if (subject.SubjectID == null)
                throw (new InvalidOperationException("Property 'SubjectID' of parameter 'obj' may not not be null."));

            if (this.subjectID == null)
            {
                subjectID = subject.SubjectID;
                ObjectState = ObjectState.Unmodified;
            }
            else if (this.SubjectID == subject.SubjectID)
            {
                ObjectState = ObjectState.Unmodified;
            }
            else
            {
                throw (new InvalidOperationException("Operation failed. Object has already been marked created."));
            }
        }

        public override void MarkDeleted()
        {
            if (ObjectState == ObjectState.New)
                throw (new InvalidOperationException("Operation failed. Object in 'New' state may not be marked deleted."));

            if (ObjectState != ObjectState.Deleted)
            {
                ObjectState = ObjectState.Deleted;
            }
        }
    }

    [DataContract(IsReference = true)]
    public class EAVInstance : EAVDataObject, EAV.Model.IEAVInstance
    {
        public int? InstanceID => throw new NotImplementedException();

        public int? ParentInstanceID => throw new NotImplementedException();

        public int? SubjectID => throw new NotImplementedException();

        public int? ContainerID => throw new NotImplementedException();

        public override void MarkCreated(EAVObject obj)
        {
            throw new NotImplementedException();
        }

        public override void MarkDeleted()
        {
            throw new NotImplementedException();
        }
    }

    [DataContract(IsReference = true)]
    public class EAVValue : EAVDataObject, EAV.Model.IEAVValue
    {
        public int? InstanceID => throw new NotImplementedException();

        public int? AttributeID => throw new NotImplementedException();

        public string RawValue { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public string Units { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void MarkCreated(EAVObject obj)
        {
            throw new NotImplementedException();
        }

        public override void MarkDeleted()
        {
            throw new NotImplementedException();
        }
    }
    #endregion
}
