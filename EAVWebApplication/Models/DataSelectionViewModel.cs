using System;
using System.Collections.Generic;
using System.Linq;

using EAV.Model;


namespace EAVWebApplication.Models.Data
{
    public class DataSelectionViewModel
    {
        public DataSelectionViewModel()
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

        public ViewModelRootContainer CurrentViewContainer { get; set; }

        public void RegenerateViewContainer()
        {
            if (CurrentContainer != null)
            {
                CurrentViewContainer = ViewModelRootContainer.Create(CurrentContainer, CurrentSubject);
                CurrentViewContainer.Enabled = CurrentSubject != null;
            }
        }
    }

    public enum DisplayMode { Running, Recurring, Singleton }

    public partial class ViewModelRootContainer : ViewModelContainer
    {
        public static ViewModelRootContainer Create(IModelRootContainer container, IModelSubject subject)
        {
            ViewModelRootContainer viewContainer = new ViewModelRootContainer() { ContainerID = container.ContainerID, DisplayText = container.DisplayText, IsRepeating = container.IsRepeating };
            int nextInstanceID = -1;

            foreach (IModelInstance instance in container.Instances)
            {
                ViewModelInstance viewInstance = ViewModelInstance.Create(container, subject, instance, null, ref nextInstanceID);

                viewContainer.Instances.Add(viewInstance);
            }

            if (container.IsRepeating || !viewContainer.Instances.Any())
            {
                ViewModelInstance viewInstance = ViewModelInstance.Create(container, subject, null, null, ref nextInstanceID);

                viewContainer.Instances.Add(viewInstance);
            }

            return (viewContainer);
        }

        public int SelectedInstanceID { get; set; }
        public ViewModelInstance CurrentInstance { get; set; }

        public DisplayMode DisplayMode { get; set; }

        public new int NextInstanceID()
        {
            var childContainers = Instances.SelectMany(it => it.ChildContainers);

            return (Math.Min(Instances.Any() ? Instances.Min(it => it.InstanceID.GetValueOrDefault()) : 0, childContainers.Any() ? childContainers.Min(it => it.NextInstanceID()) : 0) - 1);
        }
    }

    public partial class ViewModelContainer
    {
        public static ViewModelContainer Create(IModelChildContainer container, IModelSubject subject, ViewModelInstance parentInstance, ref int nextInstanceID)
        {
            ViewModelContainer viewContainer = new ViewModelContainer() { ContainerID = container.ContainerID, ParentContainerID = container.ParentContainerID, DisplayText = container.DisplayText, IsRepeating = container.IsRepeating };

            foreach (IModelInstance instance in container.Instances.Where(it => it.ParentInstanceID == parentInstance.InstanceID))
            {
                ViewModelInstance viewInstance = ViewModelInstance.Create(container, subject, instance, parentInstance, ref nextInstanceID);

                viewContainer.Instances.Add(viewInstance);
            }

            if (container.IsRepeating || !viewContainer.Instances.Any())
            {
                ViewModelInstance viewInstance = ViewModelInstance.Create(container, subject, null, parentInstance, ref nextInstanceID);

                viewContainer.Instances.Add(viewInstance);
            }

            return (viewContainer);
        }

        public ViewModelContainer()
        {
            Instances = new List<ViewModelInstance>();
        }

        public int? ContainerID { get; set; }
        public int? ParentContainerID { get; set; }
        public int Sequence { get; set; }
        public string DisplayText { get; set; }
        public bool IsRepeating { get; set; }
        public bool Enabled { get; set; }

        public IList<ViewModelInstance> Instances { get; set; }

        protected internal int NextInstanceID()
        {
            var childContainers = Instances.SelectMany(it => it.ChildContainers);

            return (Math.Min(Instances.Any() ? Instances.Min(it => it.InstanceID.GetValueOrDefault()) : 0, childContainers.Any() ? childContainers.Min(it => it.NextInstanceID()) : 0) - 1);
        }

        public void Trim()
        {
            foreach (ViewModelInstance instance in Instances)
            {
                instance.Trim();

                foreach (ViewModelContainer childContainer in instance.ChildContainers)
                {
                    childContainer.Trim();
                }
            }

            var emptyInstances = Instances.GroupJoin(Instances.SelectMany(it => it.ChildContainers).SelectMany(it => it.Instances), left => left.InstanceID, right => right.ParentInstanceID, (left, right) => new { Parent = left, Children = right }).Where(it => !it.Children.Any()).Select(it => it.Parent).Where(it => it.IsEmpty).ToList();

            while (emptyInstances.Any())
            {
                Instances.Remove(emptyInstances.First());
                emptyInstances.Remove(emptyInstances.First());
            }
        }
    }

    public partial class ViewModelInstance
    {
        public static ViewModelInstance Create(IModelContainer container, IModelSubject subject, IModelInstance instance, ViewModelInstance parentInstance, ref int nextInstanceID)
        {
            ViewModelInstance viewInstance = new ViewModelInstance() { ObjectState = instance != null ? instance.ObjectState : ObjectState.New, ContainerID = container.ContainerID, SubjectID = subject != null ? subject.SubjectID : 0, InstanceID = instance != null ? instance.InstanceID : nextInstanceID--, ParentInstanceID = parentInstance != null ? parentInstance.InstanceID : null };

            foreach (EAV.Model.IModelAttribute attribute in container.Attributes.OrderBy(it => it.Sequence))
            {
                viewInstance.Values.Add(ViewModelAttributeValue.Create(attribute, instance != null ? instance.Values.SingleOrDefault(it => it.AttributeID == attribute.AttributeID) : null));
            }

            foreach (IModelChildContainer childContainer in container.ChildContainers.OrderBy(it => it.Sequence))
            {
                viewInstance.ChildContainers.Add(ViewModelContainer.Create(childContainer, subject, viewInstance, ref nextInstanceID));
            }

            return (viewInstance);
        }

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

        public void Trim()
        {
            var emptyValues = Values.Where(it => it.IsEmpty).ToList();

            while (emptyValues.Any())
            {
                Values.Remove(emptyValues.First());
                emptyValues.Remove(emptyValues.First());
            }
        }

        public bool IsEmpty { get { return (Values.All(it => it.IsEmpty)); } }
    }

    public partial class ViewModelAttributeValue
    {
        public static ViewModelAttributeValue Create(IModelAttribute attribute, IModelValue value)
        {
            ViewModelAttributeValue viewAttributeValue = new ViewModelAttributeValue() { ObjectState = value != null ? value.ObjectState : ObjectState.New, AttributeID = attribute.AttributeID, DataType = attribute.DataType, DisplayText = attribute.DisplayText, IsKey = attribute.IsKey, VariableUnits = attribute.VariableUnits };

            foreach (EAV.Model.IModelUnit unit in attribute.Units)
            {
                viewAttributeValue.Units.Add(ViewModelUnit.Create(unit));
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
        public static ViewModelUnit Create(IModelUnit unit)
        {
            return(new ViewModelUnit() { UnitID = unit.UnitID.Value, DisplayText = unit.DisplayText });
        }

        public int UnitID { get; set; }
        public string DisplayText { get; set; }
    }
}
