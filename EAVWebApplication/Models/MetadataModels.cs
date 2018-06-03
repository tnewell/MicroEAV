using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

using EAV.Model;
using EAVFramework.Model;

namespace EAVWebApplication.Models.Metadata
{
    public class MetadataModel
    {
        public MetadataModel()
        {
            NextContextID = Int32.MinValue + 1;
            NextContainerID = Int32.MinValue + 1;
            NextAttributeID = Int32.MinValue + 1;

            contexts = new List<EAVContext>();
        }

        private Stack<object> dialogStack = new Stack<object>();
        public Stack<object> DialogStack { get { return (dialogStack); } }

        private List<EAVContext> contexts;
        public ICollection<EAVContext> Contexts { get { return (contexts); } }

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

        public EAVContext CurrentContext { get { return (Contexts.SingleOrDefault(it => it.ContextID.GetValueOrDefault() == SelectedContextID)); } }
    }

    public class ContextModel
    {
        public static explicit operator ContextModel(EAVContext context)
        {
            return (new ContextModel(context));
        }

        public ContextModel()
        {
            containers = new List<ContainerModel>();
        }

        public ContextModel(EAVContext context)
        {
            ID = context.ContextID.GetValueOrDefault();
            Name = context.Name;
            DataName = context.DataName;
            DisplayText = context.DisplayText;

            containers = new List<ContainerModel>();
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

        private List<ContainerModel> containers;
        public ICollection<ContainerModel> Containers { get { return (containers); } }

        public bool Existing { get; set; }

        public bool IsValid { get { return (!String.IsNullOrWhiteSpace(Name) && !String.IsNullOrWhiteSpace(DataName)); } }

        protected void InitializeContainers(IEnumerable<EAVContainer> containers)
        {
            this.containers.Clear();
            this.containers.AddRange(containers.Where(it => it.ObjectState != ObjectState.Deleted).Select(it => (ContainerModel) it).OrderBy(it => it.Sequence));
        }

        public void FixupContainerOrder()
        {
            containers.Sort((ContainerModel c1, ContainerModel c2) => { return (c1.Sequence == c2.Sequence ? 0 : (c1.Sequence > c2.Sequence ? 1 : -1)); });
        }
    }

    public class ContainerModel
    {
        public static explicit operator ContainerModel(EAVContainer container)
        {
            return (new ContainerModel(container));
        }

        public static explicit operator ContainerModel(EAVRootContainer container)
        {
            return (new ContainerModel(container));
        }

        public static explicit operator ContainerModel(EAVChildContainer container)
        {
            return (new ContainerModel(container));
        }

        public ContainerModel()
        {
            childContainers = new List<ContainerModel>();
            attributes = new List<AttributeModel>();
        }

        public ContainerModel(EAVContainer container)
        {
            ID = container.ContainerID.GetValueOrDefault();
            ParentID = container.ParentContainerID.GetValueOrDefault();
            Name = container.Name;
            DataName = container.DataName;
            DisplayText = container.DisplayText;
            IsRepeating = container.IsRepeating;
            Sequence = container.Sequence;

            childContainers = new List<ContainerModel>();
            InitializeContainers(container.ChildContainers);

            attributes = new List<AttributeModel>();
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

        private List<ContainerModel> childContainers;
        public ICollection<ContainerModel> ChildContainers { get { return (childContainers); } }

        private List<AttributeModel> attributes;
        public ICollection<AttributeModel> Attributes { get { return (attributes); } }

        public bool Existing { get; set; }

        public bool IsValid { get { return (!String.IsNullOrWhiteSpace(Name) && !String.IsNullOrWhiteSpace(DataName)); } }

        protected void InitializeContainers(IEnumerable<EAVContainer> containers)
        {
            this.childContainers.Clear();
            this.childContainers.AddRange(containers.Where(it => it.ObjectState != ObjectState.Deleted).Select(it => (ContainerModel) it).OrderBy(it => it.Sequence));
        }

        public void FixupContainerOrder()
        {
            childContainers.Sort((ContainerModel c1, ContainerModel c2) => { return (c1.Sequence == c2.Sequence ? 0 : (c1.Sequence > c2.Sequence ? 1 : -1)); });
        }

        protected void InitializeAttributes(IEnumerable<EAVAttribute> attributes)
        {
            this.attributes.Clear();
            this.attributes.AddRange(attributes.Where(it => it.ObjectState != ObjectState.Deleted).Select(it => (AttributeModel) it).OrderBy(it => it.Sequence));
        }

        public void FixupAttributeOrder()
        {
            attributes.Sort((AttributeModel a1, AttributeModel a2) => { return (a1.Sequence == a2.Sequence ? 0 : (a1.Sequence > a2.Sequence ? 1 : -1)); });
        }
    }

    public class AttributeModel
    {
        public static explicit operator AttributeModel(EAVAttribute attribute)
        {
            return (new AttributeModel(attribute));
        }

        public AttributeModel()
        {
        }

        public AttributeModel(EAVAttribute attribute)
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
        public EAVDataType DataType { get; set; }

        [Display(Name = "Key")]
        public bool IsKey { get; set; }

        public int Sequence { get; set; }

        public bool Existing { get; set; }

        public bool IsValid { get { return (!String.IsNullOrWhiteSpace(Name) && !String.IsNullOrWhiteSpace(DataName)); } }
    }
}
