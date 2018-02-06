using System;
using System.Linq;


namespace EAVStoreClient
{
    public partial class MicroEAVContext
    {
        public Data_Type LookupDataType(EAV.Model.EAVDataType dataType)
        {
            return (Data_Type.Single(it => it.Name == dataType.ToString()));
        }
    }

    public partial class Entity
    {
        public static explicit operator EAV.Model.BaseEAVEntity(Entity dbEntity)
        {
            if (dbEntity == null)
                return (null);

            return (new EAV.Model.BaseEAVEntity()
            {
                EntityID = dbEntity.Entity_ID,
                Descriptor = dbEntity.Descriptor,
            });
        }

        public static explicit operator Entity(EAV.Model.BaseEAVEntity entity)
        {
            if (entity == null)
                return (null);

            return (new Entity()
            {
                Entity_ID = entity.EntityID.GetValueOrDefault(),
                Descriptor = entity.Descriptor,
            });
        }

        public Entity(EAV.Model.IEAVEntity entity)
        {
            Entity_ID = entity.EntityID.GetValueOrDefault();
            Descriptor = entity.Descriptor;
        }
    }

    public partial class Context
    {
        public static explicit operator EAV.Model.BaseEAVContext(Context dbContext)
        {
            if (dbContext == null)
                return (null);

            return (new EAV.Model.BaseEAVContext()
            {
                ContextID = dbContext.Context_ID,
                DataName = dbContext.Data_Name,
                DisplayText = dbContext.Display_Text,
                Name = dbContext.Name,
            });
        }

        public static explicit operator Context(EAV.Model.BaseEAVContext context)
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

        public Context(EAV.Model.IEAVContext context)
        {
            Context_ID = context.ContextID.GetValueOrDefault();
            Data_Name = context.DataName;
            Display_Text = context.DisplayText;
            Name = context.Name;
        }
    }

    public partial class Container
    {
        public static explicit operator EAV.Model.BaseEAVContainer(Container dbContainer)
        {
            if (dbContainer == null)
                return (null);

            return (new EAV.Model.BaseEAVContainer()
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

        public static explicit operator Container(EAV.Model.BaseEAVContainer container)
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

        public Container(EAV.Model.IEAVContainer container)
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
        public EAV.Model.EAVDataType Value
        {
            get
            {
                return ((EAV.Model.EAVDataType)Enum.Parse(typeof(EAV.Model.EAVDataType), Name));
            }
            set
            {
                Name = value.ToString();
            }
        }
    }

    public partial class Attribute
    {
        public static explicit operator EAV.Model.BaseEAVAttribute(Attribute dbAttribute)
        {
            if (dbAttribute == null)
                return (null);

            return (new EAV.Model.BaseEAVAttribute()
            {
                AttributeID = dbAttribute.Attribute_ID,
                ContainerID = dbAttribute.Container_ID,
                DataName = dbAttribute.Data_Name,
                DataType = dbAttribute.Data_Type.Value,
                DisplayText = dbAttribute.Display_Text,
                IsKey = dbAttribute.Is_Key,
                Name = dbAttribute.Name,
                Sequence = dbAttribute.Sequence,
            });
        }

        public static explicit operator Attribute(EAV.Model.BaseEAVAttribute attribute)
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
            });
        }

        public Attribute(EAV.Model.IEAVAttribute attribute)
        {
            Attribute_ID = attribute.AttributeID.GetValueOrDefault();
            Container_ID = attribute.ContainerID.GetValueOrDefault();
            Data_Name = attribute.DataName;
            Display_Text = attribute.DisplayText;
            Is_Key = attribute.IsKey;
            Name = attribute.Name;
            Sequence = attribute.Sequence;
        }
    }

    public partial class Subject
    {
        public static explicit operator EAV.Model.BaseEAVSubject(Subject dbSubject)
        {
            if (dbSubject == null)
                return (null);

            return (new EAV.Model.BaseEAVSubject()
            {
                ContextID = dbSubject.Context_ID,
                Identifier = dbSubject.Identifier,
                EntityID = dbSubject.Entity_ID,
                SubjectID = dbSubject.Subject_ID,
            });
        }

        public static explicit operator Subject(EAV.Model.BaseEAVSubject subject)
        {
            if (subject == null)
                return (null);

            return (new Subject()
            {
                Context_ID = subject.ContextID.GetValueOrDefault(),
                Identifier = subject.Identifier,
                Entity_ID = subject.EntityID.GetValueOrDefault(),
                Subject_ID = subject.SubjectID.GetValueOrDefault(),
            });
        }

        public Subject(EAV.Model.IEAVSubject subject)
        {
            Context_ID = subject.ContextID.GetValueOrDefault();
            Identifier = subject.Identifier;
            Entity_ID = subject.EntityID.GetValueOrDefault();
            Subject_ID = subject.SubjectID.GetValueOrDefault();
        }
    }

    public partial class Instance
    {
        public static explicit operator EAV.Model.BaseEAVInstance(Instance dbInstance)
        {
            if (dbInstance == null)
                return (null);

            return (new EAV.Model.BaseEAVInstance()
            {
                ContainerID = dbInstance.Container_ID,
                InstanceID = dbInstance.Instance_ID,
                ParentInstanceID = dbInstance.Parent_Instance_ID,
                SubjectID = dbInstance.Subject_ID,
            });
        }

        public static explicit operator Instance(EAV.Model.BaseEAVInstance instance)
        {
            if (instance == null)
                return (null);

            return (new Instance()
            {
                Container_ID = instance.ContainerID.GetValueOrDefault(),
                Instance_ID = instance.InstanceID.GetValueOrDefault(),
                Parent_Instance_ID = instance.ParentInstanceID.GetValueOrDefault(),
                Subject_ID = instance.SubjectID.GetValueOrDefault(),
            });
        }

        public Instance(EAV.Model.IEAVInstance instance)
        {
            Container_ID = instance.ContainerID.GetValueOrDefault();
            Instance_ID = instance.InstanceID.GetValueOrDefault();
            Parent_Instance_ID = instance.ParentInstanceID;
            Subject_ID = instance.SubjectID.GetValueOrDefault();
        }
    }

    public partial class Value
    {
        public static explicit operator EAV.Model.BaseEAVValue(Value dbValue)
        {
            if (dbValue == null)
                return (null);

            return (new EAV.Model.BaseEAVValue()
            {
                AttributeID = dbValue.Attribute_ID,
                InstanceID = dbValue.Instance_ID,
                RawValue = dbValue.Raw_Value,
                Units = dbValue.Units,
            });
        }

        public static explicit operator Value(EAV.Model.BaseEAVValue value)
        {
            if (value == null)
                return (null);

            return (new Value()
            {
                Attribute_ID = value.AttributeID.GetValueOrDefault(),
                Instance_ID = value.InstanceID.GetValueOrDefault(),
                Raw_Value = value.RawValue,
                Units = value.Units,
            });
        }

        public Value() { }

        public Value(EAV.Model.IEAVValue value)
        {
            Attribute_ID = value.AttributeID.GetValueOrDefault();
            Instance_ID = value.InstanceID.GetValueOrDefault();
            Raw_Value = value.RawValue;
            Units = value.Units;
        }
    }
}