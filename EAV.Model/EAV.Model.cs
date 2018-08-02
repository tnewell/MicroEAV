// MicroEAV - EAV.Model - Class interfaces for the core EAV objects as well as basic implementations of those interfaces.
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

/// <summary>
/// This namespace contains class interfaces for the core EAV objects as well as basic implementations of those interfaces.
/// </summary>
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
        int Sequence { get; set; }
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
        int Sequence { get; set; }
        bool IsKey { get; set; }
        bool? VariableUnits { get; set; }
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
    /// <summary>
    /// An Entity represents a "thing" in the world about which you would like to collect/organize data.
    /// </summary>
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

    /// <summary>
    /// A Context represents a scope within which you will collect/organize data about Entities.
    /// Examples might be demographics, collectibles, investments, etc.
    /// </summary>
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

    /// <summary>
    /// A Container represents a related group of data items intended to be collected/organized together.
    /// An example might be an address (street name, city, postal code, etc.).
    /// Containers can be organized in hierachical relationships.
    /// </summary>
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
        public int Sequence { get; set; }
        public bool IsRepeating { get; set; }
    }

    /// <summary>
    /// An Attribute is a particular data item that will be collected/organized.
    /// Examples might be "name", "status", "price", etc.
    /// </summary>
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
        public int Sequence { get; set; }
        public bool IsKey { get; set; }
        public bool? VariableUnits { get; set; }
    }
    #endregion

    #region Data Objects
    /// <summary>
    /// A Subject is an instance of an Entity in a Context.
    /// An Entity could have data collected about it within multiple Contexts
    /// and even multiple times within the same Context.
    /// Each instance is represented by a Subject.
    /// </summary>
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

    /// <summary>
    /// An Instance is, well, just that. It represents an instance of a Container.
    /// If Container is a class, Instance is an object of that class.
    /// </summary>
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

    /// <summary>
    /// A Value is a value. It represents that actual piece of data referred to by an Attribute.
    /// </summary>
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
