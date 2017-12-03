using System;
using System.Collections.Generic;


namespace EAV.Model
{
    #region Interfaces
    
    #region Metadata Interfaces
    public interface IEAVEntity
    {
        int? EntityID { get; }

        string Descriptor { get; set; }
    }

    public interface IEAVContext
    {
        int? ContextID { get; }

        string Name { get; set; }
        string DataName { get; set; }
        string DisplayText { get; set; }
    }

    public interface IEAVContainer
    {
        int? ContainerID { get; }
        int? ParentContainerID { get; }
        int? ContextID { get; }

        string Name { get; set; }
        string DataName { get; set; }
        string DisplayText { get; set; }
        bool IsRepeating { get; set; }
    }

    public enum EAVDataType { String, Boolean, Integer, Float, DateTime }

    public interface IEAVAttribute
    {
        int? AttributeID { get; }
        int? ContainerID { get; }

        string Name { get; set; }
        string DataName { get; set; }
        string DisplayText { get; set; }
        EAVDataType DataType { get; set; }
        bool IsKey { get; set; }
    }
    #endregion

    #region Data Interfaces
    public interface IEAVSubject
    {
        int? SubjectID { get; }
        int? EntityID { get; }
        int? ContextID { get; }

        string Identifier { get; set; }
    }

    public interface IEAVInstance
    {
        int? InstanceID { get; }
        int? ParentInstanceID { get; }
        int? SubjectID { get; }
        int? ContainerID { get; }
    }

    public interface IEAVValue
    {
        int? InstanceID { get; }
        int? AttributeID { get; }

        string RawValue { get; set; }
        string Units { get; set; }
    }
    #endregion

    #endregion

    #region Base Implementation

    #region Metadata Objects
    public class BaseEAVEntity : IEAVEntity
    {
        public BaseEAVEntity() { }

        public BaseEAVEntity(IEAVEntity entity)
        {
            this.EntityID = entity.EntityID;
            this.Descriptor = entity.Descriptor;
        }

        public int? EntityID { get; set; }
        public string Descriptor { get; set; }
    }

    public class BaseEAVContext : IEAVContext
    {
        public BaseEAVContext()
        {
        }

        public BaseEAVContext(IEAVContext context)
        {
            this.ContextID = context.ContextID;
            this.Name = context.Name;
            this.DataName = context.DataName;
            this.DisplayText = context.DisplayText;
        }

        public int? ContextID { get; set; }

        public string Name { get; set; }
        public string DataName { get; set; }
        public string DisplayText { get; set; }
    }

    public class BaseEAVContainer : IEAVContainer
    {
        public BaseEAVContainer()
        {
        }

        public BaseEAVContainer(IEAVContainer container)
        {
            this.ContainerID = container.ContainerID;
            this.ParentContainerID = container.ParentContainerID;
            this.ContextID = container.ContextID;
            this.Name = container.Name;
            this.DataName = container.DataName;
            this.DisplayText = container.DisplayText;
            this.IsRepeating = container.IsRepeating;
        }

        public int? ContainerID { get; set; }
        public int? ParentContainerID { get; set; }
        public int? ContextID { get; set; }

        public string Name { get; set; }
        public string DataName { get; set; }
        public string DisplayText { get; set; }
        public bool IsRepeating { get; set; }
    }

    public class BaseEAVAttribute : IEAVAttribute
    {
        public BaseEAVAttribute() { }

        public BaseEAVAttribute(IEAVAttribute attribute)
        {
            this.AttributeID = attribute.AttributeID;
            this.ContainerID = attribute.ContainerID;
            this.Name = attribute.Name;
            this.DataName = attribute.DataName;
            this.DisplayText = attribute.DisplayText;
            this.DataType = attribute.DataType;
            this.IsKey = attribute.IsKey;
        }

        public int? AttributeID { get; set; }
        public int? ContainerID { get; set; }

        public string Name { get; set; }
        public string DataName { get; set; }
        public string DisplayText { get; set; }
        public EAVDataType DataType { get; set; }
        public bool IsKey { get; set; }
    }
    #endregion

    #region Data Objects
    public class BaseEAVSubject : IEAVSubject
    {
        public BaseEAVSubject()
        {
        }

        public BaseEAVSubject(IEAVSubject subject)
        {
            this.SubjectID = subject.SubjectID;
            this.EntityID = subject.EntityID;
            this.ContextID = subject.ContextID;
            this.Identifier = subject.Identifier;
        }

        public int? SubjectID { get; set; }
        public int? EntityID { get; set; }
        public int? ContextID { get; set; }

        public string Identifier { get; set; }
    }

    public class BaseEAVInstance : IEAVInstance
    {
        public BaseEAVInstance()
        {
        }

        public BaseEAVInstance(IEAVInstance instance)
        {
            this.InstanceID = instance.InstanceID;
            this.ParentInstanceID = instance.ParentInstanceID;
            this.SubjectID = instance.SubjectID;
            this.ContainerID = instance.ContainerID;
        }

        public int? InstanceID { get; set; }
        public int? ParentInstanceID { get; set; }
        public int? SubjectID { get; set; }
        public int? ContainerID { get; set; }
    }

    public class BaseEAVValue : IEAVValue
    {
        public BaseEAVValue() { }

        public BaseEAVValue(IEAVValue value)
        {
            this.InstanceID = value.InstanceID;
            this.AttributeID = value.AttributeID;
            this.RawValue = value.RawValue;
            this.Units = value.Units;
        }

        public int? InstanceID { get; set; }
        public int? AttributeID { get; set; }

        public string RawValue { get; set; }
        public string Units { get; set; }
    }
    #endregion

    #endregion
}
