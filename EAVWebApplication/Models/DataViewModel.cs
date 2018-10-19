using System;
using System.Collections.Generic;
using System.Linq;

using EAV.Model;


namespace EAVWebApplication.Models.Data
{
    public class DataViewModel
    {
        public DataViewModel()
        {
            contexts = new List<IModelContext>();
        }

        private List<IModelContext> contexts;
        public ICollection<IModelContext> Contexts { get { return (contexts); } set { contexts.Clear(); contexts.AddRange(value); } }

        public int SelectedContextID { get; set; }
        public IModelContext CurrentContext { get { return (contexts.SingleOrDefault(it => it.ContextID == SelectedContextID)); } }

        public int SelectedContainerID { get; set; }
        public IModelRootContainer CurrentContainer { get { return (CurrentContext != null ? CurrentContext.Containers.SingleOrDefault(it => it.ContainerID == SelectedContainerID) : null); } }

        public int SelectedSubjectID { get; set; }
        public IModelSubject CurrentSubject { get { return (CurrentContext != null ? CurrentContext.Subjects.SingleOrDefault(it => it.SubjectID == SelectedSubjectID) : null); } }

        public ViewModelContainer CurrentViewContainer { get; set; }

        private int nextInstanceID = -1;
        private int NextInstanceID(ViewModelContainer container)
        {
            if (container == null)
                return (-1);

            var instances = container.Instances;
            var childContainers = instances.SelectMany(it => it.ChildContainers);

            return (Math.Min(instances.Any() ? instances.Min(it => it.InstanceID.GetValueOrDefault()) : 0, childContainers.Any() ? childContainers.Min(it => NextInstanceID(it)) : 0) - 1);
        }

        private ViewModelAttributeValue CreateViewAttributeValue(EAV.Model.IModelAttribute attribute, EAV.Model.IModelValue value)
        {
            ViewModelAttributeValue viewAttributeValue = new ViewModelAttributeValue() { ObjectState = value != null ? value.ObjectState : ObjectState.New, AttributeID = attribute.AttributeID, DataType = attribute.DataType, DisplayText = attribute.DisplayText, IsKey = attribute.IsKey, VariableUnits = attribute.VariableUnits };

            foreach (EAV.Model.IModelUnit unit in attribute.Units)
            {
                viewAttributeValue.Units.Add(new ViewModelUnit() { UnitID = unit.UnitID.Value, DisplayText = unit.DisplayText });
            }

            if (value != null)
            {
                viewAttributeValue.Value = value.RawValue;

                if (value.Unit != null)
                {
                    viewAttributeValue.UnitID = value.Unit.UnitID;
                    viewAttributeValue.UnitText = value.Unit.DisplayText;
                }
            }
            else if (!attribute.VariableUnits.GetValueOrDefault(true) && attribute.Units.Count == 1)
            {
                viewAttributeValue.UnitID = attribute.Units.First().UnitID;
                viewAttributeValue.UnitText = attribute.Units.First().DisplayText;
            }

            return (viewAttributeValue);
        }

        private ViewModelInstance CreateViewInstance(IModelContainer container, IModelSubject subject, IModelInstance instance, ViewModelInstance parentInstance)
        {
            ViewModelInstance viewInstance = new ViewModelInstance() { ObjectState = instance != null ? instance.ObjectState : ObjectState.New, ContainerID = container.ContainerID, SubjectID = subject != null ? subject.SubjectID : 0, InstanceID = instance != null ? instance.InstanceID : nextInstanceID--, ParentInstanceID = parentInstance != null ? parentInstance.InstanceID : null };

            foreach (EAV.Model.IModelAttribute attribute in container.Attributes.OrderBy(it => it.Sequence))
            {
                viewInstance.Values.Add(CreateViewAttributeValue(attribute, instance != null ? instance.Values.SingleOrDefault(it => it.AttributeID == attribute.AttributeID) : null));
            }

            foreach (IModelContainer childContainer in container.ChildContainers.OrderBy(it => it.Sequence))
            {
                viewInstance.ChildContainers.Add(CreateViewContainer(childContainer, subject, viewInstance));
            }

            return (viewInstance);
        }

        private ViewModelContainer CreateViewContainer(EAV.Model.IModelContainer container, IModelSubject subject, ViewModelInstance parentInstance)
        {
            if (parentInstance == null)
                nextInstanceID = NextInstanceID(CurrentViewContainer);

            ViewModelContainer viewContainer = new ViewModelContainer() { ContainerID = container.ContainerID, ParentContainerID = container.ParentContainerID, DisplayText = container.DisplayText, IsRepeating = container.IsRepeating };

            foreach (IModelInstance instance in container.Instances.Where(it => parentInstance == null || it.ParentInstanceID == parentInstance.InstanceID))
            {
                ViewModelInstance viewInstance = CreateViewInstance(container, subject, instance, parentInstance);

                viewContainer.Instances.Add(viewInstance);
            }

            if (container.IsRepeating || !viewContainer.Instances.Any())
            {
                ViewModelInstance viewInstance = CreateViewInstance(container, subject, null, parentInstance);

                viewContainer.Instances.Add(viewInstance);
            }

            return (viewContainer);
        }

        public void RegenerateViewContainer()
        {
            CurrentViewContainer = CreateViewContainer(CurrentContainer, CurrentSubject, null);
        }
    }

    public enum DisplayMode { Running, Recurring, Singleton }

    public partial class ViewModelContainer
    {
        public ViewModelContainer()
        {
            Instances = new List<ViewModelInstance>();
        }

        public int? ContainerID { get; set; }
        public int? ParentContainerID { get; set; }
        public int Sequence { get; set; }
        public string DisplayText { get; set; }
        public bool IsRepeating { get; set; }
        public DisplayMode DisplayMode { get; set; }
        public bool Enabled { get; set; }

        public IList<ViewModelInstance> Instances { get; set; }
        public int SelectedInstanceID { get; set; }
        public ViewModelInstance SelectedInstance { get; set; }
    }

    public partial class ViewModelInstance
    {
        public ViewModelInstance()
        {
            ChildContainers = new List<ViewModelContainer>();
            Values = new List<ViewModelAttributeValue>();
        }

        public ObjectState ObjectState { get; set; }
        public int? ContainerID { get; set; }
        public int? SubjectID { get; set; }
        public int? InstanceID { get; set; }
        public int? ParentInstanceID { get; set; }
        public IList<ViewModelContainer> ChildContainers { get; set; }
        public IList<ViewModelAttributeValue> Values { get; set; }

        public bool IsEmpty { get { return (Values.All(it => it.IsEmpty)); } }
    }

    public partial class ViewModelAttributeValue
    {
        public sealed class BooleanListItem
        {
            public BooleanListItem(string text, bool value) { Text = text; Value = value; }

            public string Text { get; private set; }
            public bool Value { get; private set; }
        }

        public static readonly IReadOnlyCollection<BooleanListItem> YesNoList;
        public static readonly IReadOnlyCollection<BooleanListItem> TrueFalseList;

        static ViewModelAttributeValue()
        {
            YesNoList = new List<BooleanListItem>() { new BooleanListItem("Yes", true), new BooleanListItem("No", false) };
            TrueFalseList = new List<BooleanListItem>() { new BooleanListItem("True", true), new BooleanListItem("False", false) };
        }

        public ViewModelAttributeValue()
        {
            Units = new List<ViewModelUnit>();
        }

        public ObjectState ObjectState { get; set; }
        public int? AttributeID { get; set; }
        public EAV.EAVDataType DataType { get; set; }
        public int Sequence { get; set; }
        public string DisplayText { get; set; }
        public bool IsKey { get; set; }
        public bool? VariableUnits { get; set; }
        public IList<ViewModelUnit> Units { get; set; }

        public string Value { get; set; }
        public int? UnitID { get; set; }
        public string UnitText { get; set; }

        public bool IsEmpty { get { return (String.IsNullOrWhiteSpace(Value)); } }
    }

    public partial class ViewModelUnit
    {
        public int UnitID { get; set; }
        public string DisplayText { get; set; }
    }
}
