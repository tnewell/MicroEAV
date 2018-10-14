﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
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

        private void BindToModelValue(EAV.Model.IModelAttribute attribute, EAV.Model.IModelInstance instance, EAV.Model.IModelValue value, ViewModelAttributeValue viewValue)
        {
            if (value == null && viewValue == null)
                return;

            if (value != null && viewValue == null)
            {
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
                value = new ModelValue() { Attribute = attribute, Instance = instance, Unit = attribute.VariableUnits.HasValue ? new ModelUnit() { UnitID = NextUnitID } : null };
            }

            value.RawValue = viewValue.Value;

            if (attribute.VariableUnits.HasValue)
            {
                if (attribute.VariableUnits.Value)
                {
                    if (value.Unit.ObjectState == ObjectState.New && !String.Equals(value.Unit.DisplayText, viewValue.UnitText, StringComparison.InvariantCulture))
                    {
                        value.Unit.DisplayText = viewValue.UnitText;
                    }
                }
                else
                {
                    if (value.UnitID != viewValue.UnitID)
                    {
                        value.Unit = attribute.Units.SingleOrDefault(it => it.UnitID == viewValue.UnitID);
                    }
                }
            }
        }

        private void BindToModelInstance(EAV.Model.IModelContainer container, EAV.Model.IModelSubject subject, EAV.Model.IModelInstance parentInstance, EAV.Model.IModelInstance instance, ViewModelInstance viewInstance)
        {
            if (instance == null && viewInstance == null)
                return;

            if (instance != null && viewInstance == null)
            {
                if (instance.ObjectState == ObjectState.New)
                {
                    if (parentInstance != null)
                    {
                        parentInstance.ChildInstances.Remove(instance as IModelChildInstance);
                    }
                    else
                    {
                        subject.Instances.Remove(instance as IModelRootInstance);
                    }
                }
                else
                {
                    instance.MarkDeleted();
                }

                return;
            }
            else if (instance == null && viewInstance != null)
            {
                if (parentInstance != null)
                {
                    instance = new ModelChildInstance() { Container = container, InstanceID = viewInstance.InstanceID, ParentInstance = parentInstance /*, Subject = subject */ };
                }
                else
                {
                    instance = new ModelRootInstance() { Container = container, InstanceID = viewInstance.InstanceID, Subject = subject };
                }
            }

            foreach (ModelAttribute attribute in container.Attributes)
            {
                BindToModelValue(attribute, instance, instance.Values.SingleOrDefault(it => it.AttributeID == attribute.AttributeID), viewInstance.Values.SingleOrDefault(it => it.AttributeID == attribute.AttributeID));
            }

            foreach (ModelContainer childContainer in container.ChildContainers)
            {
                ViewModelContainer childViewContainer = viewInstance.ChildContainers.Single(it => it.ContainerID == childContainer.ContainerID);
                var dataInstances = childContainer.Instances.GroupJoin(childViewContainer.Instances, left => left.InstanceID, right => right.InstanceID, (left, right) => new { ModelInstance = left, ViewInstance = right.FirstOrDefault() }).ToArray();
                var viewInstances = childViewContainer.Instances.GroupJoin(childContainer.Instances, left => left.InstanceID, right => right.InstanceID, (left, right) => new { ModelInstance = right.FirstOrDefault(), ViewInstance = left }).ToArray();

                foreach (var item in dataInstances.Union(viewInstances))
                {
                    BindToModelInstance(childContainer, subject, instance, item.ModelInstance, item.ViewInstance);
                }
            }
        }

        private void TrimViewModel(ViewModelContainer container)
        {
            foreach (ViewModelInstance instance in container.Instances)
            {
                var emptyValues = instance.Values.Where(it => it.IsEmpty).ToList();

                while (emptyValues.Any())
                {
                    instance.Values.Remove(emptyValues.First());
                    emptyValues.Remove(emptyValues.First());
                }

                foreach (ViewModelContainer childContainer in instance.ChildContainers)
                {
                    TrimViewModel(childContainer);
                }
            }

            var emptyInstances = container.Instances.GroupJoin(container.Instances.SelectMany(it => it.ChildContainers).SelectMany(it => it.Instances), left => left.InstanceID, right => right.ParentInstanceID, (left, right) => new { Parent = left, Children = right }).Where(it => !it.Children.Any()).Select(it => it.Parent).Where(it => it.IsEmpty).ToList();

            while (emptyInstances.Any())
            {
                container.Instances.Remove(emptyInstances.First());
                emptyInstances.Remove(emptyInstances.First());
            }
        }

        private void BindToDataModel(DataViewModel currentDataViewModel, ViewModelContainer postedViewContainer)
        {
            if (postedViewContainer.DisplayMode != DisplayMode.Running)
            {
                postedViewContainer.Instances.Add(postedViewContainer.SelectedInstance);
            }

            TrimViewModel(postedViewContainer);

            var dataInstances = currentDataViewModel.CurrentSubject.Instances.GroupJoin(postedViewContainer.Instances, left => left.InstanceID, right => right.InstanceID, (left, right) => new { ModelInstance = left, ViewInstance = right.FirstOrDefault() });
            var viewInstances = postedViewContainer.Instances.GroupJoin(currentDataViewModel.CurrentSubject.Instances, left => left.InstanceID, right => right.InstanceID, (left, right) => new { ModelInstance = right.FirstOrDefault(), ViewInstance = left });

            foreach (var item in dataInstances.Union(viewInstances))
            {
                BindToModelInstance(currentDataViewModel.CurrentContainer, currentDataViewModel.CurrentSubject, null, item.ModelInstance, item.ViewInstance);
            }
        }

        private void TrimDataModel(IModelContainer container)
        {
            foreach (IModelContainer childContainer in container.ChildContainers)
            {
                TrimDataModel(childContainer);
            }

            foreach (IModelInstance instance in container.Instances)
            {
                var deletedValues = instance.Values.Where(it => it.ObjectState == ObjectState.Deleted).ToList();

                while (deletedValues.Any())
                {
                    instance.Values.Remove(deletedValues.First());
                    deletedValues.Remove(deletedValues.First());
                }

                if (instance.ObjectState != ObjectState.Deleted)
                    instance.MarkUnmodified();
            }

            var deletedInstances = !container.ChildContainers.Any() ? container.Instances.Where(it => it.ObjectState == ObjectState.Deleted).ToList() : container.Instances.GroupJoin(container.ChildContainers.SelectMany(it => it.Instances), left => left.InstanceID, right => right.ParentInstanceID, (left, right) => new { Parent = left, Children = right }).Where(it => !it.Children.Any()).Select(it => it.Parent).Where(it => it.ObjectState == ObjectState.Deleted).ToList();

            while (deletedInstances.Any())
            {
                container.Instances.Remove(deletedInstances.First());
                deletedInstances.Remove(deletedInstances.First());
            }

            container.MarkUnmodified();
        }

        [HttpGet]
        public ActionResult Index()
        {
            DataViewModel currentViewModel = TempData[TempDataModelKey] as DataViewModel ?? new DataViewModel();

            if (!currentViewModel.Contexts.Any())
            {
                foreach (var item in eavClient.LoadContexts())
                {
                    currentViewModel.Contexts.Add(item);
                }
            }

            TempData[TempDataModelKey] = currentViewModel;

            return View("Index", currentViewModel);
        }

        [HttpGet]
        public ActionResult PostRedirectGetTarget(string view)
        {
            DataViewModel currentViewModel = TempData[TempDataModelKey] as DataViewModel;

            TempData[TempDataModelKey] = currentViewModel;

            return (View(view, currentViewModel.CurrentViewContainer));
        }

        [HttpGet]
        [Route("{contextID}", Name = "SelectContext")]
        public ActionResult SelectContext(int contextID)
        {
            DataViewModel currentViewModel = TempData[TempDataModelKey] as DataViewModel;

            currentViewModel.SelectedContextID = contextID;

            IEnumerable<object> containers = Enumerable.Empty<object>();
            IEnumerable<object> subjects = Enumerable.Empty<object>();

            if (currentViewModel.CurrentContext != null)
            {
                eavClient.LoadRootContainers(currentViewModel.CurrentContext);
                containers = currentViewModel.CurrentContext.Containers.Select(it => new { Text = it.Name, Value = it.ContainerID });

                eavClient.LoadSubjects(currentViewModel.CurrentContext);
                subjects = currentViewModel.CurrentContext.Subjects.Select(it => new { Text = it.Identifier, Value = it.SubjectID });
            }

            TempData[TempDataModelKey] = currentViewModel;

            return (new JsonResult() { Data = new { Containers = containers, Subjects = subjects }, JsonRequestBehavior = JsonRequestBehavior.AllowGet });
        }

        [HttpPost]
        public ActionResult EditForm(DataViewModel postedModel)
        {
            DataViewModel currentViewModel = TempData[TempDataModelKey] as DataViewModel;

            // User's current choices
            currentViewModel.SelectedContainerID = postedModel.SelectedContainerID;
            currentViewModel.SelectedSubjectID = postedModel.SelectedSubjectID;

            if (currentViewModel.CurrentContainer != null)
            {
                eavClient.LoadMetadata(currentViewModel.CurrentContainer);

                if (currentViewModel.CurrentSubject != null)
                {
                    eavClient.LoadRootInstances(currentViewModel.CurrentSubject, currentViewModel.CurrentContainer);

                    foreach (IModelRootInstance instance in currentViewModel.CurrentSubject.Instances)
                    {
                        eavClient.LoadData(instance);
                    }
                }

                // Refresh the view object
                currentViewModel.RegenerateViewContainer();
            }

            currentViewModel.CurrentViewContainer.DisplayMode = DisplayMode.Recurring;
            currentViewModel.CurrentViewContainer.Enabled = currentViewModel.CurrentSubject != null;

            if (currentViewModel.CurrentViewContainer.DisplayMode != DisplayMode.Running)
            {
                currentViewModel.CurrentViewContainer.SelectedInstanceID = currentViewModel.CurrentViewContainer.Instances.Max(it => it.InstanceID.GetValueOrDefault());
                currentViewModel.CurrentViewContainer.SelectedInstance = currentViewModel.CurrentViewContainer.Instances.SingleOrDefault(it => it.InstanceID == currentViewModel.CurrentViewContainer.SelectedInstanceID);
            }

            TempData[TempDataModelKey] = currentViewModel;

            return (RedirectToAction("PostRedirectGetTarget", new { view = currentViewModel.CurrentViewContainer.DisplayMode == DisplayMode.Singleton ? "DisplaySingletonContainer" : (currentViewModel.CurrentViewContainer.DisplayMode == DisplayMode.Recurring ? "DisplayRecurringContainer" : "DisplayRunningContainer") }));
        }

        [HttpPost]
        public ActionResult SaveForm(ViewModelContainer postedViewContainer)
        {
            DataViewModel currentViewModel = TempData[TempDataModelKey] as DataViewModel;

            // Reconcile changes
            BindToDataModel(currentViewModel, postedViewContainer);

            // Save changes
            foreach (IModelRootInstance instance in currentViewModel.CurrentSubject.Instances)
            {
                eavClient.SaveData(instance);
            }

            // Get rid of deleted items
            TrimDataModel(currentViewModel.CurrentContainer);

            // Refresh the view object
            currentViewModel.RegenerateViewContainer();

            currentViewModel.CurrentViewContainer.DisplayMode = postedViewContainer.DisplayMode;
            currentViewModel.CurrentViewContainer.Enabled = postedViewContainer.Enabled;

            if (currentViewModel.CurrentViewContainer.DisplayMode != DisplayMode.Running)
            {
                currentViewModel.CurrentViewContainer.SelectedInstanceID = currentViewModel.CurrentViewContainer.Instances.Max(it => it.InstanceID.GetValueOrDefault());
                currentViewModel.CurrentViewContainer.SelectedInstance = currentViewModel.CurrentViewContainer.Instances.SingleOrDefault(it => it.InstanceID == currentViewModel.CurrentViewContainer.SelectedInstanceID);
            }

            TempData[TempDataModelKey] = currentViewModel;

            return (RedirectToAction("PostRedirectGetTarget", new { view = currentViewModel.CurrentViewContainer.DisplayMode == DisplayMode.Singleton ? "DisplaySingletonContainer" : (currentViewModel.CurrentViewContainer.DisplayMode == DisplayMode.Recurring ? "DisplayRecurringContainer" : "DisplayRunningContainer") }));
        }

        [HttpPost]
        public ActionResult RetrieveInstance(ViewModelContainer postedViewContainer)
        {
            DataViewModel currentViewModel = TempData[TempDataModelKey] as DataViewModel;

            // This will keep ASP from holding on to any values when we run the new
            // instance through our partial view.
            ModelState.Clear();

            // Make sure we keep the posted version to capture any changes
            currentViewModel.CurrentViewContainer.Instances.Remove(currentViewModel.CurrentViewContainer.Instances.Single(it => it.InstanceID == postedViewContainer.SelectedInstance.InstanceID));
            currentViewModel.CurrentViewContainer.Instances.Add(postedViewContainer.SelectedInstance);

            // Now switch instances
            currentViewModel.CurrentViewContainer.SelectedInstanceID = postedViewContainer.SelectedInstanceID;
            currentViewModel.CurrentViewContainer.SelectedInstance = currentViewModel.CurrentViewContainer.Instances.SingleOrDefault(it => it.InstanceID == currentViewModel.CurrentViewContainer.SelectedInstanceID);

            TempData[TempDataModelKey] = currentViewModel;

            return (PartialView("SingletonInstance", currentViewModel.CurrentViewContainer));
        }
    }
 }