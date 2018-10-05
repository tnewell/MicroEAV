using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

using EAV.Model;
using EAVModelLibrary;


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

        public int SelectedInstanceID { get; set; }
        public IModelRootInstance CurrentInstance { get { return (CurrentSubject != null ? CurrentSubject.Instances.SingleOrDefault(it => it.InstanceID == SelectedInstanceID) : null); } }

        public ViewModelContainer CurrentViewContainer { get; set; }

        private int NextInstanceID(ViewModelContainer container)
        {
            if (container == null)
                return (-1);

            return (Math.Min(container.Instances.Any() ? container.Instances.Min(it => it.InstanceID.GetValueOrDefault()) : 0, container.ChildContainers.Any() ? container.ChildContainers.Min(it => NextInstanceID(it)) : 0) - 1);
        }

        private ViewModelAttributeValue BindToViewAttributeValue(EAV.Model.IModelAttribute attribute, EAV.Model.IModelValue value)
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

        private ViewModelInstance BindToViewInstance(IModelContainer container, IModelInstance instance, ViewModelInstance parentInstance)
        {
            // TODO: Set Subject ID?
            ViewModelInstance viewContainerInstance = new ViewModelInstance() { ObjectState = instance != null ? instance.ObjectState : ObjectState.New, ContainerID = container.ContainerID, SubjectID = null, InstanceID = instance != null ? instance.InstanceID : NextInstanceID(CurrentViewContainer), ParentInstanceID = parentInstance != null ? parentInstance.InstanceID : null };

            foreach (EAV.Model.IModelAttribute attribute in container.Attributes)
            {
                viewContainerInstance.Values.Add(BindToViewAttributeValue(attribute, instance != null ? instance.Values.SingleOrDefault(it => it.AttributeID == attribute.AttributeID) : null));
            }

            return (viewContainerInstance);
        }

        private ViewModelContainer BindToViewContainer(EAV.Model.IModelContainer container, ViewModelInstance parentInstance)
        {
            ViewModelContainer viewContainer = new ViewModelContainer() { ContainerID = container.ContainerID, ParentContainerID = container.ParentContainerID, DisplayText = container.DisplayText, IsRepeating = container.IsRepeating };

            foreach (IModelInstance instance in container.Instances.Where(it => parentInstance == null || it.ParentInstanceID == parentInstance.InstanceID))
            {
                ViewModelInstance viewInstance = BindToViewInstance(container, instance, parentInstance);

                viewContainer.Instances.Add(viewInstance);

                foreach (IModelContainer childContainer in container.ChildContainers)
                {
                    viewContainer.ChildContainers.Add(BindToViewContainer(childContainer, viewInstance));
                }
            }

            if (container.IsRepeating || !viewContainer.Instances.Any())
            {
                ViewModelInstance viewInstance = BindToViewInstance(container, null, parentInstance);

                viewContainer.Instances.Add(viewInstance);

                foreach (IModelContainer childContainer in container.ChildContainers)
                {
                    viewContainer.ChildContainers.Add(BindToViewContainer(childContainer, viewInstance));
                }
            }

            return (viewContainer);
        }

        public void Refresh()
        {
            CurrentViewContainer = BindToViewContainer(CurrentContainer, null);
        }
    }

    public partial class ViewModelContainer
    {
        public ViewModelContainer()
        {
            ChildContainers = new List<ViewModelContainer>();
            Instances = new List<ViewModelInstance>();
        }

        public int? ContainerID { get; set; }
        public int? ParentContainerID { get; set; }
        public int Sequence { get; set; }
        public string DisplayText { get; set; }
        public bool IsRepeating { get; set; }
        public ICollection<ViewModelContainer> ChildContainers { get; set; }
        public ICollection<ViewModelInstance> Instances { get; set; }
    }

    public partial class ViewModelInstance
    {
        public ViewModelInstance()
        {
            Values = new List<ViewModelAttributeValue>();
        }

        public ObjectState ObjectState { get; set; }
        public int? ContainerID { get; set; }
        public int? SubjectID { get; set; }
        public int? InstanceID { get; set; }
        public int? ParentInstanceID { get; set; }
        public ICollection<ViewModelAttributeValue> Values { get; set; }

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
        public ICollection<ViewModelUnit> Units { get; set; }

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
