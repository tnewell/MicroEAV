using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

using EAV.Model;
using EAVModelLibrary;


namespace EAVWebApplication.Models.Metadata
{
    public class MetadataModel
    {
        public MetadataModel()
        {
            NextContextID = Int32.MinValue + 1;
            NextContainerID = Int32.MinValue + 1;
            NextAttributeID = Int32.MinValue + 1;

            contexts = new List<IModelContext>();
        }

        private Stack<object> dialogStack = new Stack<object>();
        public Stack<object> DialogStack { get { return (dialogStack); } }

        private List<IModelContext> contexts;
        public ICollection<IModelContext> Contexts { get { return (contexts); } }

        private object contextIDLock = new object();
        private int nextContextID;
        public int NextContextID
        {
            get { lock (contextIDLock) { return (nextContextID++); } }
            private set { lock (contextIDLock) { nextContextID = value; } }
        }

        private object containerIDLock = new object();
        private int nextContainerID;
        public int NextContainerID
        {
            get { lock (containerIDLock) { return (nextContainerID++); } }
            private set { lock (containerIDLock) { nextContainerID = value; } }
        }

        private object attributeIDLock = new object();
        private int nextAttributeID;
        public int NextAttributeID
        {
            get { lock (attributeIDLock) { return (nextAttributeID++); } }
            private set { lock (attributeIDLock) { nextAttributeID = value; } }
        }

        public int SelectedContextID { get; set; }

        public IModelContext CurrentContext { get { return (Contexts.SingleOrDefault(it => it.ContextID.GetValueOrDefault() == SelectedContextID)); } }
    }

    public class ContextViewModel
    {
        public ContextViewModel()
        {
            containers = new List<ContainerViewModel>();
        }

        public ContextViewModel(IModelContext context)
        {
            ID = context.ContextID.GetValueOrDefault();
            Name = context.Name;
            DataName = context.DataName;
            DisplayText = context.DisplayText;

            containers = new List<ContainerViewModel>();
            InitializeContainers(context.Containers);
        }

        public int ID { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "(required)")]
        public string Name { get; set; }

        [Display(Name = "Data Name")]
        [Required(ErrorMessage = "(required)")]
        [MaxLength(256)]
        public string DataName { get; set; }

        [Display(Name = "Display Text")]
        public string DisplayText { get; set; }

        private List<ContainerViewModel> containers;
        public ICollection<ContainerViewModel> Containers { get { return (containers); } }

        public bool Existing { get; set; }
        
        public bool IsValid { get { return (!String.IsNullOrWhiteSpace(Name) && !String.IsNullOrWhiteSpace(DataName)); } }

        protected void InitializeContainers(IEnumerable<IModelContainer> containers)
        {
            this.containers.Clear();
            this.containers.AddRange(containers.Where(it => it.ObjectState != ObjectState.Deleted).Select(it => new ContainerViewModel(it)).OrderBy(it => it.Sequence));
        }

        public void FixupContainerOrder()
        {
            containers.Sort((ContainerViewModel c1, ContainerViewModel c2) => { return (c1.Sequence == c2.Sequence ? 0 : (c1.Sequence > c2.Sequence ? 1 : -1)); });
        }
    }

    public class ContainerViewModel
    {
        public ContainerViewModel()
        {
            childContainers = new List<ContainerViewModel>();
            attributes = new List<AttributeViewModel>();
        }

        public ContainerViewModel(IModelContainer container)
        {
            ID = container.ContainerID.GetValueOrDefault();
            ParentID = container.ParentContainerID.GetValueOrDefault();
            Name = container.Name;
            DataName = container.DataName;
            DisplayText = container.DisplayText;
            IsRepeating = container.IsRepeating;
            Sequence = container.Sequence;

            childContainers = new List<ContainerViewModel>();
            InitializeContainers(container.ChildContainers);

            attributes = new List<AttributeViewModel>();
            InitializeAttributes(container.Attributes);
        }

        public bool ChildContainer { get; set; }

        public int ID { get; set; }

        public int ParentID { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "(required)")]
        public string Name { get; set; }

        [Display(Name = "Data Name")]
        [Required(ErrorMessage = "(required)")]
        [MaxLength(256)]
        public string DataName { get; set; }

        [Display(Name = "Display Text")]
        public string DisplayText { get; set; }

        [Display(Name = "Repeating")]
        public bool IsRepeating { get; set; }

        public int Sequence { get; set; }

        private List<ContainerViewModel> childContainers;
        public ICollection<ContainerViewModel> ChildContainers { get { return (childContainers); } }

        private List<AttributeViewModel> attributes;
        public ICollection<AttributeViewModel> Attributes { get { return (attributes); } }

        public bool Existing { get; set; }

        public bool IsValid { get { return (!String.IsNullOrWhiteSpace(Name) && !String.IsNullOrWhiteSpace(DataName)); } }

        protected void InitializeContainers(IEnumerable<IModelContainer> containers)
        {
            this.childContainers.Clear();
            this.childContainers.AddRange(containers.Where(it => it.ObjectState != ObjectState.Deleted).Select(it => new ContainerViewModel(it)).OrderBy(it => it.Sequence));
        }

        public void FixupContainerOrder()
        {
            childContainers.Sort((ContainerViewModel c1, ContainerViewModel c2) => { return (c1.Sequence == c2.Sequence ? 0 : (c1.Sequence > c2.Sequence ? 1 : -1)); });
        }

        protected void InitializeAttributes(IEnumerable<IModelAttribute> attributes)
        {
            this.attributes.Clear();
            this.attributes.AddRange(attributes.Where(it => it.ObjectState != ObjectState.Deleted).Select(it => new AttributeViewModel(it)).OrderBy(it => it.Sequence));
        }

        public void FixupAttributeOrder()
        {
            attributes.Sort((AttributeViewModel a1, AttributeViewModel a2) => { return (a1.Sequence == a2.Sequence ? 0 : (a1.Sequence > a2.Sequence ? 1 : -1)); });
        }
    }

    public class AttributeViewModel
    {
        public AttributeViewModel()
        {
        }

        public AttributeViewModel(IModelAttribute attribute)
        {
            ID = attribute.AttributeID.GetValueOrDefault();
            ContainerID = attribute.ContainerID.GetValueOrDefault();
            Name = attribute.Name;
            DataName = attribute.DataName;
            DisplayText = attribute.DisplayText;
            DataType = attribute.DataType;
            IsKey = attribute.IsKey;
            Sequence = attribute.Sequence;
        }

        public int ID { get; set; }

        public int ContainerID { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "(required)")]
        public string Name { get; set; }

        [Display(Name = "Data Name")]
        [Required(ErrorMessage = "(required)")]
        [MaxLength(256)]
        public string DataName { get; set; }

        [Display(Name = "Display Text")]
        public string DisplayText { get; set; }

        [Display(Name = "Data Type")]
        public EAV.EAVDataType DataType { get; set; }

        [Display(Name = "Key")]
        public bool IsKey { get; set; }

        public int Sequence { get; set; }

        public bool Existing { get; set; }

        public bool IsValid { get { return (!String.IsNullOrWhiteSpace(Name) && !String.IsNullOrWhiteSpace(DataName)); } }
    }
}
