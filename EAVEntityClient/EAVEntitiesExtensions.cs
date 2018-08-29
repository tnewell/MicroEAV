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
using System.Linq;


namespace EAVStoreClient
{
    public partial class MicroEAVContext
    {
        public Data_Type LookupDataType(EAV.EAVDataType dataType)
        {
            return (Data_Type.Single(it => it.Name == dataType.ToString()));
        }
    }

    public partial class Entity
    {
        public static explicit operator EAV.Store.StoreEntity(Entity dbEntity)
        {
            if (dbEntity == null)
                return (null);

            return (new EAV.Store.StoreEntity()
            {
                EntityID = dbEntity.Entity_ID,
                Descriptor = dbEntity.Descriptor,
            });
        }

        public static explicit operator Entity(EAV.Store.StoreEntity entity)
        {
            if (entity == null)
                return (null);

            return (new Entity()
            {
                Entity_ID = entity.EntityID.GetValueOrDefault(),
                Descriptor = entity.Descriptor,
            });
        }

        public Entity(EAV.Store.IStoreEntity entity)
        {
            Entity_ID = entity.EntityID.GetValueOrDefault();
            Descriptor = entity.Descriptor;
        }
    }

    public partial class Context
    {
        public static explicit operator EAV.Store.StoreContext(Context dbContext)
        {
            if (dbContext == null)
                return (null);

            return (new EAV.Store.StoreContext()
            {
                ContextID = dbContext.Context_ID,
                DataName = dbContext.Data_Name,
                DisplayText = dbContext.Display_Text,
                Name = dbContext.Name,
            });
        }

        public static explicit operator Context(EAV.Store.StoreContext context)
        {
            if (context == null)
                return (null);

            return (new Context()
            {
                Context_ID = context.ContextID.GetValueOrDefault(),
                Data_Name = context.DataName,
                Display_Text = context.DisplayText,
                Name = context.Name,
            });
        }

        public Context(EAV.Store.IStoreContext context)
        {
            Context_ID = context.ContextID.GetValueOrDefault();
            Data_Name = context.DataName;
            Display_Text = context.DisplayText;
            Name = context.Name;
        }
    }

    public partial class Container
    {
        public static explicit operator EAV.Store.StoreContainer(Container dbContainer)
        {
            if (dbContainer == null)
                return (null);

            return (new EAV.Store.StoreContainer()
            {
                ContainerID = dbContainer.Container_ID,
                ContextID = dbContainer.Context_ID,
                DataName = dbContainer.Data_Name,
                DisplayText = dbContainer.Display_Text,
                IsRepeating = dbContainer.Is_Repeating,
                ParentContainerID = dbContainer.Parent_Container_ID,
                Name = dbContainer.Name,
                Sequence = dbContainer.Sequence,
            });
        }

        public static explicit operator Container(EAV.Store.StoreContainer container)
        {
            if (container == null)
                return(null);

            return (new Container()
            {
                Container_ID = container.ContainerID.GetValueOrDefault(),
                Context_ID = container.ContextID.GetValueOrDefault(),
                Data_Name = container.DataName,
                Display_Text = container.DisplayText,
                Is_Repeating = container.IsRepeating,
                Parent_Container_ID = container.ParentContainerID.GetValueOrDefault(),
                Name = container.Name,
                Sequence = container.Sequence,
            });
        }

        public Container(EAV.Store.IStoreContainer container)
        {
            Container_ID = container.ContainerID.GetValueOrDefault();
            Context_ID = container.ContextID.GetValueOrDefault();
            Data_Name = container.DataName;
            Display_Text = container.DisplayText;
            Is_Repeating = container.IsRepeating;
            Parent_Container_ID = container.ParentContainerID;
            Name = container.Name;
            Sequence = container.Sequence;
        }
    }

    public partial class Data_Type
    {
        public EAV.EAVDataType Value
        {
            get
            {
                return ((EAV.EAVDataType)Enum.Parse(typeof(EAV.EAVDataType), Name));
            }
            set
            {
                Name = value.ToString();
            }
        }
    }

    public partial class Attribute
    {
        public static explicit operator EAV.Store.StoreAttribute(Attribute dbAttribute)
        {
            if (dbAttribute == null)
                return (null);

            return (new EAV.Store.StoreAttribute()
            {
                AttributeID = dbAttribute.Attribute_ID,
                ContainerID = dbAttribute.Container_ID,
                DataName = dbAttribute.Data_Name,
                DataType = dbAttribute.Data_Type.Value,
                DisplayText = dbAttribute.Display_Text,
                IsKey = dbAttribute.Is_Key,
                Name = dbAttribute.Name,
                Sequence = dbAttribute.Sequence,
                VariableUnits = dbAttribute.Variable_Units,
            });
        }

        public static explicit operator Attribute(EAV.Store.StoreAttribute attribute)
        {
            if (attribute == null)
                return (null);

            return (new Attribute()
            {
                Attribute_ID = attribute.AttributeID.GetValueOrDefault(),
                Container_ID = attribute.ContainerID.GetValueOrDefault(),
                Data_Name = attribute.DataName,
                Display_Text = attribute.DisplayText,
                Is_Key = attribute.IsKey,
                Name = attribute.Name,
                Sequence = attribute.Sequence,
                Variable_Units = attribute.VariableUnits,
            });
        }

        public Attribute(EAV.Store.IStoreAttribute attribute)
        {
            Attribute_ID = attribute.AttributeID.GetValueOrDefault();
            Container_ID = attribute.ContainerID.GetValueOrDefault();
            Data_Name = attribute.DataName;
            Display_Text = attribute.DisplayText;
            Is_Key = attribute.IsKey;
            Name = attribute.Name;
            Sequence = attribute.Sequence;
            Variable_Units = attribute.VariableUnits;
        }
    }

    public partial class Unit
    {
        public static explicit operator EAV.Store.StoreUnit(Unit dbUnit)
        {
            if (dbUnit == null)
                return (null);

            return (new EAV.Store.StoreUnit()
            {
                UnitID = dbUnit.Unit_ID,
                SingularName = dbUnit.Singular_Name,
                SingularAbbreviation = dbUnit.Singular_Abbreviation,
                PluralName = dbUnit.Plural_Name,
                PluralAbbreviation = dbUnit.Plural_Abbreviation,
                Symbol = dbUnit.Symbol,
                DisplayText = dbUnit.Display_Text,
                Curated = dbUnit.Curated,
            });
        }

        public static explicit operator Unit(EAV.Store.StoreUnit aUnit)
        {
            if (aUnit == null)
                return (null);

            return (new Unit()
            {
                Unit_ID = aUnit.UnitID.GetValueOrDefault(),
                Singular_Name = aUnit.SingularName,
                Singular_Abbreviation = aUnit.SingularAbbreviation,
                Plural_Name = aUnit.PluralName,
                Plural_Abbreviation = aUnit.PluralAbbreviation,
                Symbol = aUnit.Symbol,
                Display_Text = aUnit.DisplayText,
                Curated = aUnit.Curated,
            });
        }

        public Unit(EAV.Store.IStoreUnit aUnit)
        {
            Unit_ID = aUnit.UnitID.GetValueOrDefault();
            Singular_Name = aUnit.SingularName;
            Singular_Abbreviation = aUnit.SingularAbbreviation;
            Plural_Name = aUnit.PluralName;
            Plural_Abbreviation = aUnit.PluralAbbreviation;
            Symbol = aUnit.Symbol;
            Display_Text = aUnit.DisplayText;
            Curated = aUnit.Curated;
        }
    }

    public partial class Subject
    {
        public static explicit operator EAV.Store.StoreSubject(Subject dbSubject)
        {
            if (dbSubject == null)
                return (null);

            return (new EAV.Store.StoreSubject()
            {
                ContextID = dbSubject.Context_ID,
                Identifier = dbSubject.Identifier,
                EntityID = dbSubject.Entity_ID,
                SubjectID = dbSubject.Subject_ID,
            });
        }

        public static explicit operator Subject(EAV.Store.StoreSubject aSubject)
        {
            if (aSubject == null)
                return (null);

            return (new Subject()
            {
                Context_ID = aSubject.ContextID.GetValueOrDefault(),
                Identifier = aSubject.Identifier,
                Entity_ID = aSubject.EntityID.GetValueOrDefault(),
                Subject_ID = aSubject.SubjectID.GetValueOrDefault(),
            });
        }

        public Subject(EAV.Store.IStoreSubject aSubject)
        {
            Context_ID = aSubject.ContextID.GetValueOrDefault();
            Identifier = aSubject.Identifier;
            Entity_ID = aSubject.EntityID.GetValueOrDefault();
            Subject_ID = aSubject.SubjectID.GetValueOrDefault();
        }
    }

    public partial class Instance
    {
        public static explicit operator EAV.Store.StoreInstance(Instance dbInstance)
        {
            if (dbInstance == null)
                return (null);

            return (new EAV.Store.StoreInstance()
            {
                ContainerID = dbInstance.Container_ID,
                InstanceID = dbInstance.Instance_ID,
                ParentInstanceID = dbInstance.Parent_Instance_ID,
                SubjectID = dbInstance.Subject_ID,
            });
        }

        public static explicit operator Instance(EAV.Store.StoreInstance anInstance)
        {
            if (anInstance == null)
                return (null);

            return (new Instance()
            {
                Container_ID = anInstance.ContainerID.GetValueOrDefault(),
                Instance_ID = anInstance.InstanceID.GetValueOrDefault(),
                Parent_Instance_ID = anInstance.ParentInstanceID.GetValueOrDefault(),
                Subject_ID = anInstance.SubjectID.GetValueOrDefault(),
            });
        }

        public Instance(EAV.Store.IStoreInstance anInstance)
        {
            Container_ID = anInstance.ContainerID.GetValueOrDefault();
            Instance_ID = anInstance.InstanceID.GetValueOrDefault();
            Parent_Instance_ID = anInstance.ParentInstanceID;
            Subject_ID = anInstance.SubjectID.GetValueOrDefault();
        }
    }

    public partial class Value
    {
        public static explicit operator EAV.Store.StoreValue(Value dbValue)
        {
            if (dbValue == null)
                return (null);

            return (new EAV.Store.StoreValue()
            {
                AttributeID = dbValue.Attribute_ID,
                InstanceID = dbValue.Instance_ID,
                RawValue = dbValue.Raw_Value,
                UnitID = dbValue.Unit_ID,
            });
        }

        public static explicit operator Value(EAV.Store.StoreValue value)
        {
            if (value == null)
                return (null);

            return (new Value()
            {
                Attribute_ID = value.AttributeID.GetValueOrDefault(),
                Instance_ID = value.InstanceID.GetValueOrDefault(),
                Raw_Value = value.RawValue,
                Unit_ID = value.UnitID,
            });
        }

        public Value() { }

        public Value(EAV.Store.IStoreValue value)
        {
            Attribute_ID = value.AttributeID.GetValueOrDefault();
            Instance_ID = value.InstanceID.GetValueOrDefault();
            Raw_Value = value.RawValue;
            Unit_ID = value.UnitID;
        }
    }
}