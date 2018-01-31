using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using EAV.Model;
using EAVFramework.Model;


public class MetadataModel
{
    public MetadataModel()
    {
        NextContextID = Int32.MinValue + 1;
        NextContainerID = Int32.MinValue + 1;

        contexts = new List<EAVContext>();
    }

    private Stack<object> theStack = new Stack<object>();
    public Stack<object> TheStack { get { return (theStack); } }

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
    }

    public int ID { get; set; }
    [Required(ErrorMessage = "(required)")]
    public string Name { get; set; }
    [Required(ErrorMessage = "(required)")]
    public string DataName { get; set; }
    public string DisplayText { get; set; }

    private List<ContainerModel> containers;
    public ICollection<ContainerModel> Containers { get { return (containers); } }

    public bool IsValid { get { return (!String.IsNullOrWhiteSpace(Name) && !String.IsNullOrWhiteSpace(DataName)); } }

    public void InitializeContainers(EAVContext context)
    {
        containers.Clear();
        containers.AddRange(context.Containers.Select(it => (ContainerModel) it));
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

        childContainers = new List<ContainerModel>();
        attributes = new List<AttributeModel>();
    }

    public bool ChildContainer { get; set; }

    public int ID { get; set; }
    public int ParentID { get; set; }
    [Required(ErrorMessage = "(required)")]
    public string Name { get; set; }
    [Required(ErrorMessage = "(required)")]
    public string DataName { get; set; }
    public string DisplayText { get; set; }
    public bool IsRepeating { get; set;  }

    private List<ContainerModel> childContainers;
    public ICollection<ContainerModel> ChildContainers { get { return (childContainers); } }

    private List<AttributeModel> attributes;
    public ICollection<AttributeModel> Attributes { get { return (attributes); } }

    public bool IsValid { get { return (!String.IsNullOrWhiteSpace(Name) && !String.IsNullOrWhiteSpace(DataName)); } }

    public void InitializeContainers(EAVContainer container)
    {
        childContainers.Clear();
        childContainers.AddRange(container.ChildContainers.Select(it => (ContainerModel)it));
    }

    public void InitializeAttributes(EAVContainer container)
    {
        attributes.Clear();
        attributes.AddRange(container.Attributes.Select(it => (AttributeModel)it));
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
        Name = attribute.Name;
        DataName = attribute.DataName;
        DisplayText = attribute.DisplayText;
        DataType = attribute.DataType;
        IsKey = attribute.IsKey;
    }

    public int ID { get; set; }
    public int ContainerID { get; set; }
    [Required(ErrorMessage = "(required)")]
    public string Name { get; set; }
    [Required(ErrorMessage = "(required)")]
    public string DataName { get; set; }
    public string DisplayText { get; set; }
    public EAVDataType DataType { get; set; }
    public bool IsKey { get; set; }

    public bool IsValid { get { return (!String.IsNullOrWhiteSpace(Name) && !String.IsNullOrWhiteSpace(DataName)); } }
}
