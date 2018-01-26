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
        return (new ContextModel()
        {
            ID = context.ContextID.GetValueOrDefault(),
            Name = context.Name,
            DataName = context.DataName,
            DisplayText = context.DisplayText,
        });
    }

    public ContextModel()
    {
    }

    public int ID { get; set; }
    [Required(ErrorMessage = "(required)")]
    public string Name { get; set; }
    [Required(ErrorMessage = "(required)")]
    public string DataName { get; set; }
    public string DisplayText { get; set; }
}

public class ContainerModel
{
    public static explicit operator ContainerModel(EAVRootContainer container)
    {
        return (new ContainerModel()
        {
            ID = container.ContainerID.GetValueOrDefault(),
            Name = container.Name,
            DataName = container.DataName,
            DisplayText = container.DisplayText,
            IsRepeating = container.IsRepeating,
        });
    }

    public static explicit operator ContainerModel(EAVChildContainer container)
    {
        return (new ContainerModel()
        {
            ID = container.ContainerID.GetValueOrDefault(),
            ParentID = container.ParentContainerID.GetValueOrDefault(),
            Name = container.Name,
            DataName = container.DataName,
            DisplayText = container.DisplayText,
            IsRepeating = container.IsRepeating,
        });
    }

    public ContainerModel()
    {
    }

    public int ID { get; set; }
    public int ParentID { get; set; }
    [Required(ErrorMessage = "(required)")]
    public string Name { get; set; }
    [Required(ErrorMessage = "(required)")]
    public string DataName { get; set; }
    public string DisplayText { get; set; }
    public bool IsRepeating { get; set;  }
}

public class AttributeModel
{
    public static explicit operator AttributeModel(EAVAttribute attribute)
    {
        return (new AttributeModel()
        {
            ID = attribute.AttributeID.GetValueOrDefault(),
            Name = attribute.Name,
            DataName = attribute.DataName,
            DisplayText = attribute.DisplayText,
            DataType = attribute.DataType,
            IsKey = attribute.IsKey,
        });
    }

    public AttributeModel()
    {
    }

    public int ID { get; set; }
    [Required(ErrorMessage = "(required)")]
    public string Name { get; set; }
    [Required(ErrorMessage = "(required)")]
    public string DataName { get; set; }
    public string DisplayText { get; set; }
    public EAVDataType DataType { get; set; }
    public bool IsKey { get; set; }
}
