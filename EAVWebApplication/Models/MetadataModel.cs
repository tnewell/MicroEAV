using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

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

    public EAVContext CurrentContext { get { return (Contexts.SingleOrDefault(it => it.ContextID == SelectedContextID)); } }
}

public class ContextModel
{
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
    public AttributeModel()
    {
    }

    public int ID { get; set; }
    [Required(ErrorMessage = "(required)")]
    public string Name { get; set; }
    [Required(ErrorMessage = "(required)")]
    public string DataName { get; set; }
    public string DisplayText { get; set; }
    public bool IsKey { get; set; }
}
