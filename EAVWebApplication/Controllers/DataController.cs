using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

using EAV.Model;
using EAVModelClient;
using EAVModelLibrary;
using EAVWebApplication.Models.Data;


namespace EAVWebApplication.Controllers
{
    public class DataController : Controller
    {
        private static readonly string TempDataModelKey = "DataViewModel";

        private ModelClient eavClient = new ModelClient(ConfigurationManager.AppSettings["EAVServiceUrl"]);
        private EAV.Model.IModelObjectFactory factory = new EAVModelLibrary.ModelObjectFactory();

        private int NextUnitID
        {
            get { int? id = (int?)Session["NextUnitID"]; NextUnitID = id.GetValueOrDefault(-1) - 1; return (id.GetValueOrDefault(-1)); }
            set { Session["NextUnitID"] = value; }
        }

        public DataController()
        {
        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
        }

        private void TrimViewModel(ViewModelContainer container)
        {
            Debug.WriteLine("");
            Debug.WriteLine("------------------------------------------------------------------------------------------------");
            Debug.WriteLine("-- TrimViewModel");
            Debug.WriteLine("------------------------------------------------------------------------------------------------");
            Debug.WriteLine("");
            Debug.Indent();

            Debug.WriteLine($"Analyzing {container.Instances.Count} instance(s) in container [{container.ContainerID}] '{container.DisplayText}'...");
            Debug.WriteLine("");

            foreach (ViewModelInstance instance in container.Instances)
            {
                var emptyValues = instance.Values.Where(it => it.IsEmpty).ToList();

                Debug.WriteLine($"{emptyValues.Count} of {instance.Values.Count} value(s) found for removal in instance [{instance.InstanceID}].");

                Debug.Indent();
                while (emptyValues.Any())
                {
                    Debug.WriteLine($"Removing value for attribute [{emptyValues.First().AttributeID}] from instance [{instance.InstanceID}].");

                    instance.Values.Remove(emptyValues.First());
                    emptyValues.Remove(emptyValues.First());
                }
                Debug.Unindent();

                Debug.WriteLine($"{instance.ChildContainers.Count} child container(s) found for analysis in instance [{instance.InstanceID}].");

                foreach (ViewModelContainer childContainer in instance.ChildContainers)
                {
                    TrimViewModel(childContainer);
                }
            }

            var emptyInstances = container.Instances.GroupJoin(container.Instances.SelectMany(it => it.ChildContainers).SelectMany(it => it.Instances), left => left.InstanceID, right => right.ParentInstanceID, (left, right) => new { Parent = left, Children = right }).Where(it => !it.Children.Any()).Select(it => it.Parent).Where(it => it.IsEmpty).ToList();

            Debug.WriteLine("");
            Debug.WriteLine($"{emptyInstances.Count} of {container.Instances.Count} instance(s) found for removal in container [{container.ContainerID}] '{container.DisplayText}'.");

            Debug.Indent();
            while (emptyInstances.Any())
            {
                Debug.WriteLine($"Removing instance [{emptyInstances.First().InstanceID}] from container [{emptyInstances.First().ContainerID}].");
                container.Instances.Remove(emptyInstances.First());
                emptyInstances.Remove(emptyInstances.First());
            }
            Debug.Unindent();

            Debug.WriteLine("");
            Debug.Unindent();
        }

        private void BindToModelValue(EAV.Model.IModelAttribute attribute, EAV.Model.IModelInstance instance, EAV.Model.IModelValue value, ViewModelAttributeValue viewValue)
        {
            Debug.WriteLine("");
            Debug.WriteLine("------------------------------------------------------------------------------------------------");
            Debug.WriteLine("-- BindToModelValue");
            Debug.WriteLine("------------------------------------------------------------------------------------------------");
            Debug.WriteLine("");
            Debug.Indent();

            if (value == null && viewValue == null)
                return;

            if (value != null && viewValue == null)
            {
                Debug.WriteLine($"DELETE: {value.ObjectState} value '{value.RawValue}' for attribute [{attribute.AttributeID}] '{attribute.Name}' in instance [{instance.InstanceID}].");

                if (value.ObjectState == ObjectState.New)
                {
                    instance.Values.Remove(value);
                }
                else
                {
                    value.MarkDeleted();
                }

                return;
            }
            else if (value == null && viewValue != null)
            {
                Debug.WriteLine($"INSERT: value '{viewValue.Value}' for attribute [{attribute.AttributeID}] '{attribute.Name}' in instance [{instance.InstanceID}].");

                value = new ModelValue() { Attribute = attribute, Instance = instance, Unit = attribute.VariableUnits.HasValue ? new ModelUnit() { UnitID = NextUnitID } : null };
            }

            if (value.RawValue != viewValue.Value)
            {
                Debug.WriteLine($"Updating old value '{value.RawValue}' to new value '{viewValue.Value}'.");
                value.RawValue = viewValue.Value;
            }

            Debug.WriteLine($"VariableUnits property has value '{attribute.VariableUnits}'.");

            if (attribute.VariableUnits.HasValue)
            {
                if (attribute.VariableUnits.Value)
                {
                    if (value.Unit.ObjectState == ObjectState.New && !String.Equals(value.Unit.DisplayText, viewValue.UnitText, StringComparison.InvariantCulture))
                    {
                        value.Unit.DisplayText = viewValue.UnitText;
                        Debug.WriteLine($"Property DisplayText assigned value: '{viewValue.UnitText}'.");
                    }
                }
                else
                {
                    if (value.UnitID != viewValue.UnitID)
                    {
                        value.Unit = attribute.Units.SingleOrDefault(it => it.UnitID == viewValue.UnitID);
                        Debug.WriteLineIf(value.Unit != null, String.Format("Property Unit assigned value: [{0}] '{1}'.", value.Unit != null ? value.Unit.UnitID : 0, value.Unit != null ? value.Unit.DisplayText : null));
                        Debug.WriteLineIf(value.Unit == null, String.Format("Property Unit assigned null value."));
                    }
                }
            }

            Debug.WriteLine("");
            Debug.Unindent();
        }

        private void BindToModelInstance(EAV.Model.IModelContainer container, EAV.Model.IModelSubject subject, EAV.Model.IModelInstance parentInstance, EAV.Model.IModelInstance instance, ViewModelInstance viewParentInstance, ViewModelInstance viewInstance)
        {
            Debug.WriteLine("");
            Debug.WriteLine("------------------------------------------------------------------------------------------------");
            Debug.WriteLine("-- BindToModelInstance");
            Debug.WriteLine("------------------------------------------------------------------------------------------------");
            Debug.WriteLine("");
            Debug.Indent();

            if (instance == null && viewInstance == null)
                return;

            if (instance != null && viewInstance == null)
            {
                Debug.Write($"DELETE: {instance.ObjectState} instance [{instance.InstanceID}] for container [{container.ContainerID}] '{container.Name}'");

                if (instance.ObjectState == ObjectState.New)
                {
                    if (parentInstance != null)
                    {
                        Debug.WriteLine($" in parent instance [{parentInstance.InstanceID}].");
                        parentInstance.ChildInstances.Remove(instance as IModelChildInstance);
                    }
                    else
                    {
                        Debug.WriteLine($" in subject [{subject.SubjectID}].");
                        subject.Instances.Remove(instance as IModelRootInstance);
                    }
                }
                else
                {
                    Debug.WriteLine(".");
                    instance.MarkDeleted();
                }

                return;
            }
            else if (instance == null && viewInstance != null)
            {
                Debug.Write($"INSERT: instance [{viewInstance.InstanceID}] for container [{container.ContainerID}] '{container.Name}'");

                if (parentInstance != null)
                {
                    Debug.WriteLine($" in parent instance [{parentInstance.InstanceID}].");
                    instance = new ModelChildInstance() { Container = container, InstanceID = viewInstance.InstanceID, ParentInstance = parentInstance };
                }
                else
                {
                    Debug.WriteLine($" in subject [{subject.SubjectID}].");
                    instance = new ModelRootInstance() { Container = container, InstanceID = viewInstance.InstanceID, Subject = subject };
                }
            }

            Debug.WriteLine("Binding values to attributes...");

            foreach (ModelAttribute attribute in container.Attributes)
            {
                BindToModelValue(attribute, instance, instance.Values.SingleOrDefault(it => it.AttributeID == attribute.AttributeID), viewInstance.Values.SingleOrDefault(it => it.AttributeID == attribute.AttributeID));
            }

            Debug.WriteLine("Binding child instances to child containers...");

            foreach (IModelContainer childContainer in container.ChildContainers)
            {
                var dataInstanceSubset = childContainer.Instances.Where(it => it.ParentInstanceID == instance.InstanceID);
                var viewInstanceSubset = viewInstance.ChildContainers.Where(it => it.ContainerID == childContainer.ContainerID).SelectMany(it => it.Instances).Where(it => it.ParentInstanceID == viewInstance.InstanceID);

                var dataInstances = dataInstanceSubset.GroupJoin(viewInstanceSubset, left => left.InstanceID, right => right.InstanceID, (left, right) => new { ModelInstance = left, ViewInstance = right.FirstOrDefault() }).ToArray();
                var viewInstances = viewInstanceSubset.GroupJoin(dataInstanceSubset, left => left.InstanceID, right => right.InstanceID, (left, right) => new { ModelInstance = right.FirstOrDefault(), ViewInstance = left }).ToArray();
                var instancePairs = dataInstances.Union(viewInstances);

                Debug.WriteLine($"{instancePairs.Count()} instance pair(s) found for binding.");
                Debug.Indent();
                Debug.WriteLine($"{instancePairs.Count(it => it.ModelInstance == null && it.ViewInstance != null)} for insert.");
                Debug.WriteLine($"{instancePairs.Count(it => it.ModelInstance != null && it.ViewInstance != null)} for update.");
                Debug.WriteLine($"{instancePairs.Count(it => it.ModelInstance != null && it.ViewInstance == null)} for delete.");
                Debug.Unindent();

                foreach (var item in instancePairs)
                {
                    BindToModelInstance(childContainer, subject, instance, item.ModelInstance, viewInstance, item.ViewInstance);
                }
            }

            Debug.WriteLine("");
            Debug.Unindent();
        }

        private void BindToDataModel(DataViewModel currentDataViewModel, ViewModelContainer postedViewContainer)
        {
            Debug.WriteLine("");
            Debug.WriteLine("------------------------------------------------------------------------------------------------");
            Debug.WriteLine("-- BindToDataModel");
            Debug.WriteLine("------------------------------------------------------------------------------------------------");
            Debug.WriteLine("");
            Debug.Indent();

            Debug.WriteLine($"Display Mode = {currentDataViewModel.CurrentViewContainer.DisplayMode}.");

            if (currentDataViewModel.CurrentViewContainer.DisplayMode != DisplayMode.Running)
            {
                Debug.WriteLine($"Swapping out instance [{postedViewContainer.SelectedInstance.InstanceID}].");
                currentDataViewModel.CurrentViewContainer.Instances.Remove(currentDataViewModel.CurrentViewContainer.Instances.Single(it => it.InstanceID == postedViewContainer.SelectedInstance.InstanceID));
                currentDataViewModel.CurrentViewContainer.Instances.Add(postedViewContainer.SelectedInstance);
            }
            else
            {
                currentDataViewModel.CurrentViewContainer = postedViewContainer;
            }

            TrimViewModel(currentDataViewModel.CurrentViewContainer);

            var dataInstances = currentDataViewModel.CurrentSubject.Instances.GroupJoin(currentDataViewModel.CurrentViewContainer.Instances, left => left.InstanceID, right => right.InstanceID, (left, right) => new { ModelInstance = left, ViewInstance = right.FirstOrDefault() });
            var viewInstances = currentDataViewModel.CurrentViewContainer.Instances.GroupJoin(currentDataViewModel.CurrentSubject.Instances, left => left.InstanceID, right => right.InstanceID, (left, right) => new { ModelInstance = right.FirstOrDefault(), ViewInstance = left });
            var instancePairs = dataInstances.Union(viewInstances);

            Debug.WriteLine($"{instancePairs.Count()} instance pair(s) found for binding.");
            Debug.Indent();
            Debug.WriteLine($"{instancePairs.Count(it => it.ModelInstance == null && it.ViewInstance != null)} for insert.");
            Debug.WriteLine($"{instancePairs.Count(it => it.ModelInstance != null && it.ViewInstance != null)} for update.");
            Debug.WriteLine($"{instancePairs.Count(it => it.ModelInstance != null && it.ViewInstance == null)} for delete.");
            Debug.Unindent();

            foreach (var item in instancePairs)
            {
                BindToModelInstance(currentDataViewModel.CurrentContainer, currentDataViewModel.CurrentSubject, null, item.ModelInstance, null, item.ViewInstance);
            }

            Debug.WriteLine("");
            Debug.Unindent();
        }

        private void TrimDataModel(IModelContainer container)
        {
            Debug.WriteLine("");
            Debug.WriteLine("------------------------------------------------------------------------------------------------");
            Debug.WriteLine("-- TrimDataModel");
            Debug.WriteLine("------------------------------------------------------------------------------------------------");
            Debug.WriteLine("");
            Debug.Indent();

            Debug.WriteLine($"Analyzing {container.Instances.Count} instance(s) in container [{container.ContainerID}] '{container.DisplayText}'.");

            foreach (IModelInstance instance in container.Instances)
            {
                var deletedValues = instance.Values.Where(it => it.ObjectState == ObjectState.Deleted).ToList();

                Debug.WriteLine($"{deletedValues.Count} of {instance.Values.Count} value(s) found for removal in instance [{instance.InstanceID}].");

                while (deletedValues.Any())
                {
                    instance.Values.Remove(deletedValues.First());
                    deletedValues.Remove(deletedValues.First());
                }

                if (instance.ObjectState != ObjectState.Deleted)
                    instance.MarkUnmodified();
            }

            Debug.WriteLine($"{container.ChildContainers.Count} child container(s) found for analysis in container [{container.ContainerID}] '{container.Name}'.");

            foreach (IModelContainer childContainer in container.ChildContainers)
            {
                TrimDataModel(childContainer);
            }

            var deletedInstances = container.Instances.GroupJoin(container.ChildContainers.SelectMany(it => it.Instances), left => left.InstanceID, right => right.ParentInstanceID, (left, right) => new { Parent = left, Children = right }).Where(it => !it.Children.Any()).Select(it => it.Parent).Where(it => it.ObjectState == ObjectState.Deleted).ToList();

            Debug.WriteLine($"{deletedInstances.Count} of {container.Instances.Count} instance(s) found for removal in container [{container.ContainerID}] '{container.Name}'.");

            while (deletedInstances.Any())
            {
                container.Instances.Remove(deletedInstances.First());
                deletedInstances.Remove(deletedInstances.First());
            }

            container.MarkUnmodified();

            Debug.WriteLine("");
            Debug.Unindent();
        }

        [HttpGet]
        public ActionResult Index()
        {
            Debug.WriteLine("");
            Debug.WriteLine("------------------------------------------------------------------------------------------------");
            Debug.WriteLine("-- Index");
            Debug.WriteLine("------------------------------------------------------------------------------------------------");
            Debug.WriteLine("");
            Debug.Indent();

            DataViewModel currentViewModel = TempData[TempDataModelKey] as DataViewModel ?? new DataViewModel();

            if (!currentViewModel.Contexts.Any())
            {
                Debug.Write("Loading contexts... ");
                foreach (var item in eavClient.LoadContexts())
                {
                    currentViewModel.Contexts.Add(item);
                }
                Debug.WriteLine($"{currentViewModel.Contexts.Count} context(s) found.");
            }

            TempData[TempDataModelKey] = currentViewModel;

            Debug.WriteLine("");
            Debug.Unindent();

            return View("Index", currentViewModel);
        }

        [HttpGet]
        public ActionResult PostRedirectGetTarget(string view)
        {
            Debug.WriteLine("");
            Debug.WriteLine("------------------------------------------------------------------------------------------------");
            Debug.WriteLine("-- PostRedirectGetTarget");
            Debug.WriteLine("------------------------------------------------------------------------------------------------");
            Debug.WriteLine("");
            Debug.Indent();

            DataViewModel currentViewModel = TempData[TempDataModelKey] as DataViewModel;

            Debug.WriteLine($"Redirecting to '{view}'.");

            TempData[TempDataModelKey] = currentViewModel;

            Debug.WriteLine("");
            Debug.Unindent();

            return (View(view, currentViewModel.CurrentViewContainer));
        }

        [HttpGet]
        [Route("{contextID}", Name = "SelectContext")]
        public ActionResult SelectContext(int contextID)
        {
            Debug.WriteLine("");
            Debug.WriteLine("------------------------------------------------------------------------------------------------");
            Debug.WriteLine("-- SelectContext");
            Debug.WriteLine("------------------------------------------------------------------------------------------------");
            Debug.WriteLine("");
            Debug.Indent();

            DataViewModel currentViewModel = TempData[TempDataModelKey] as DataViewModel;

            Debug.WriteLine($"Context [{contextID}] selected.");

            currentViewModel.SelectedContextID = contextID;

            IEnumerable<object> containers = Enumerable.Empty<object>();
            IEnumerable<object> subjects = Enumerable.Empty<object>();

            if (currentViewModel.CurrentContext != null)
            {
                Debug.Write("Loading root containers... ");
                eavClient.LoadRootContainers(currentViewModel.CurrentContext);
                containers = currentViewModel.CurrentContext.Containers.Select(it => new { Text = it.Name, Value = it.ContainerID });
                Debug.WriteLine($"{containers.Count()} root container(s) found.");

                Debug.Write("Loading subjects... ");
                eavClient.LoadSubjects(currentViewModel.CurrentContext);
                subjects = currentViewModel.CurrentContext.Subjects.Select(it => new { Text = it.Identifier, Value = it.SubjectID });
                Debug.WriteLine($"{subjects.Count()} subject(s) found.");
            }

            TempData[TempDataModelKey] = currentViewModel;

            Debug.WriteLine("");
            Debug.Unindent();

            return (new JsonResult() { Data = new { Containers = containers, Subjects = subjects }, JsonRequestBehavior = JsonRequestBehavior.AllowGet });
        }

        [HttpPost]
        public ActionResult EditForm(DataViewModel postedModel)
        {
            Debug.WriteLine("");
            Debug.WriteLine("------------------------------------------------------------------------------------------------");
            Debug.WriteLine("-- EditForm");
            Debug.WriteLine("------------------------------------------------------------------------------------------------");
            Debug.WriteLine("");
            Debug.Indent();

            DataViewModel currentViewModel = TempData[TempDataModelKey] as DataViewModel;

            Debug.WriteLine($"Root Container [{postedModel.SelectedContainerID}] selected.");
            Debug.WriteLine($"Subject [{postedModel.SelectedSubjectID}] selected.");

            // User's current choices
            currentViewModel.SelectedContainerID = postedModel.SelectedContainerID;
            currentViewModel.SelectedSubjectID = postedModel.SelectedSubjectID;

            if (currentViewModel.CurrentContainer != null)
            {
                Debug.WriteLine("Loading metadata...");
                eavClient.LoadMetadata(currentViewModel.CurrentContainer);

                if (currentViewModel.CurrentSubject != null)
                {
                    Debug.Write("Loading root instances... ");
                    eavClient.LoadRootInstances(currentViewModel.CurrentSubject, currentViewModel.CurrentContainer);
                    Debug.WriteLine($"{currentViewModel.CurrentSubject.Instances.Count} root instance(s) found");

                    Debug.WriteLineIf(currentViewModel.CurrentSubject.Instances.Any(), "Loading data...");
                    foreach (IModelRootInstance instance in currentViewModel.CurrentSubject.Instances)
                    {
                        eavClient.LoadData(instance);
                    }
                }

                // Refresh the view object
                Debug.WriteLine("Regenerating view model...");
                currentViewModel.RegenerateViewContainer();
            }

            currentViewModel.CurrentViewContainer.DisplayMode = DisplayMode.Recurring;
            Debug.WriteLine($"Display Mode = {currentViewModel.CurrentViewContainer.DisplayMode}");
            currentViewModel.CurrentViewContainer.Enabled = currentViewModel.CurrentSubject != null;
            Debug.WriteLine(String.Format("Container will {0}be enabled.", currentViewModel.CurrentViewContainer.Enabled ? "" : "not "));

            if (currentViewModel.CurrentViewContainer.DisplayMode != DisplayMode.Running)
            {
                currentViewModel.CurrentViewContainer.SelectedInstanceID = currentViewModel.CurrentViewContainer.Instances.Max(it => it.InstanceID.GetValueOrDefault());
                currentViewModel.CurrentViewContainer.SelectedInstance = currentViewModel.CurrentViewContainer.Instances.SingleOrDefault(it => it.InstanceID == currentViewModel.CurrentViewContainer.SelectedInstanceID);
                Debug.WriteLine($"Setting selected instance to [{currentViewModel.CurrentViewContainer.SelectedInstanceID}]");
            }

            TempData[TempDataModelKey] = currentViewModel;

            Debug.WriteLine("");
            Debug.Unindent();

            return (RedirectToAction("PostRedirectGetTarget", new { view = currentViewModel.CurrentViewContainer.DisplayMode == DisplayMode.Singleton ? "DisplaySingletonContainer" : (currentViewModel.CurrentViewContainer.DisplayMode == DisplayMode.Recurring ? "DisplayRecurringContainer" : "DisplayRunningContainer") }));
        }

        [HttpPost]
        public ActionResult SaveForm(ViewModelContainer postedViewContainer)
        {
            Debug.WriteLine("");
            Debug.WriteLine("------------------------------------------------------------------------------------------------");
            Debug.WriteLine("-- SaveForm");
            Debug.WriteLine("------------------------------------------------------------------------------------------------");
            Debug.WriteLine("");
            Debug.Indent();

            DataViewModel currentViewModel = TempData[TempDataModelKey] as DataViewModel;

            // Reconcile changes
            Debug.WriteLine("Binding to data model...");
            BindToDataModel(currentViewModel, postedViewContainer);

            // Save changes
            foreach (IModelRootInstance instance in currentViewModel.CurrentSubject.Instances)
            {
                Debug.Write($"Saving instance [{instance.InstanceID}]... ");
                eavClient.SaveData(instance);
                Debug.WriteLine($"ID is now [{instance.InstanceID}].");
            }

            // Get rid of deleted items
            Debug.WriteLine("Trimming data model...");
            TrimDataModel(currentViewModel.CurrentContainer);

            // Refresh the view object
            Debug.WriteLine("Regenerating view model...");
            currentViewModel.RegenerateViewContainer();

            currentViewModel.CurrentViewContainer.DisplayMode = postedViewContainer.DisplayMode;
            Debug.WriteLine($"Display Mode = {currentViewModel.CurrentViewContainer.DisplayMode}");
            currentViewModel.CurrentViewContainer.Enabled = postedViewContainer.Enabled;
            Debug.WriteLine(String.Format("Container will {0}be enabled.", currentViewModel.CurrentViewContainer.Enabled ? "" : "not "));

            if (currentViewModel.CurrentViewContainer.DisplayMode != DisplayMode.Running)
            {
                currentViewModel.CurrentViewContainer.SelectedInstanceID = currentViewModel.CurrentViewContainer.Instances.Max(it => it.InstanceID.GetValueOrDefault());
                currentViewModel.CurrentViewContainer.SelectedInstance = currentViewModel.CurrentViewContainer.Instances.SingleOrDefault(it => it.InstanceID == currentViewModel.CurrentViewContainer.SelectedInstanceID);
                Debug.WriteLine($"Setting selected instance to [{currentViewModel.CurrentViewContainer.SelectedInstanceID}]");
            }

            TempData[TempDataModelKey] = currentViewModel;

            Debug.WriteLine("");
            Debug.Unindent();

            return (RedirectToAction("PostRedirectGetTarget", new { view = currentViewModel.CurrentViewContainer.DisplayMode == DisplayMode.Singleton ? "DisplaySingletonContainer" : (currentViewModel.CurrentViewContainer.DisplayMode == DisplayMode.Recurring ? "DisplayRecurringContainer" : "DisplayRunningContainer") }));
        }

        [HttpPost]
        public ActionResult RetrieveInstance(ViewModelContainer postedViewContainer)
        {
            Debug.WriteLine("");
            Debug.WriteLine("------------------------------------------------------------------------------------------------");
            Debug.WriteLine("-- RetrieveInstance");
            Debug.WriteLine("------------------------------------------------------------------------------------------------");
            Debug.WriteLine("");
            Debug.Indent();

            DataViewModel currentViewModel = TempData[TempDataModelKey] as DataViewModel;

            Debug.WriteLine($"Selected instance is [{postedViewContainer.SelectedInstanceID}]");

            // This will keep ASP from holding on to any values when we run the new
            // instance through our partial view.
            ModelState.Clear();

            // Make sure we keep the posted version to capture any changes
            Debug.WriteLine($"Swapping out instance [{postedViewContainer.SelectedInstance.InstanceID}].");

            var a = currentViewModel.CurrentViewContainer.Instances.Single(it => it.InstanceID == postedViewContainer.SelectedInstance.InstanceID);
            var b = postedViewContainer.SelectedInstance;

            currentViewModel.CurrentViewContainer.Instances.Remove(currentViewModel.CurrentViewContainer.Instances.Single(it => it.InstanceID == postedViewContainer.SelectedInstance.InstanceID));
            currentViewModel.CurrentViewContainer.Instances.Add(postedViewContainer.SelectedInstance);

            // Now switch instances
            currentViewModel.CurrentViewContainer.SelectedInstanceID = postedViewContainer.SelectedInstanceID;
            currentViewModel.CurrentViewContainer.SelectedInstance = currentViewModel.CurrentViewContainer.Instances.SingleOrDefault(it => it.InstanceID == currentViewModel.CurrentViewContainer.SelectedInstanceID);
            Debug.WriteLine($"Updating selected instance to [{currentViewModel.CurrentViewContainer.SelectedInstanceID}]");

            TempData[TempDataModelKey] = currentViewModel;

            Debug.WriteLine("");
            Debug.Unindent();

            return (PartialView("SingletonInstance", currentViewModel.CurrentViewContainer));
        }
    }
 }