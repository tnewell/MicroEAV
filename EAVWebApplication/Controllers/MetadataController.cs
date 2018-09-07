using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;

using EAV.Model;
using EAVModelLibrary;
using EAVModelClient;

using EAVWebApplication.Models.Metadata;


namespace EAVWebApplication.Controllers
{
    public class MetadataController : Controller
    {
        private ModelClient eavClient = new ModelClient(ConfigurationManager.AppSettings["EAVServiceUrl"]);
        private EAV.Model.IModelObjectFactory objectFactory;

        public MetadataController()
        {
            objectFactory = new EAVModelLibrary.ModelObjectFactory();
        }

        protected bool UpdateRequested { get { return (Boolean.TryParse(Request["update"] ?? Boolean.FalseString, out Boolean x) ? x : false); } }

        protected int ID { get { return (Int32.TryParse(Request["id"] ?? "0", out int x) ? x : 0); } }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                eavClient.Dispose();
            }

            base.Dispose(disposing);
        }

        private JsonResult BuildResult(string dialogTitle, string dialogURL, string updateURL, params object[] contextItems)
        {
            return (Json(new
            {
                dialogTitle = dialogTitle,
                dialogURL = dialogURL,
                updateURL = updateURL,
                contextList = contextItems,
                errors = ModelState.Keys.ToDictionary(key => key, val => ModelState[val].Errors.FirstOrDefault()),
            }
            ));
        }

        private IModelContainer FindContainer(IEnumerable<IModelContainer> containers, int id)
        {
            if (containers == null || !containers.Any())
            {
                return (null);
            }

            return (containers.SingleOrDefault(it => it.ContainerID == id) ?? FindContainer(containers.SelectMany(it => it.ChildContainers), id));
        }

        [HttpGet]
        public ActionResult Index()
        {
            MetadataModel metadata = new MetadataModel();

            // Add any existing contexts
            foreach (var item in eavClient.LoadContexts())
            {
                metadata.Contexts.Add(item);
            }

            // Force validation so that UI lights up
            TryValidateModel(metadata);

            TempData["Metadata"] = metadata;

            return View("Index", metadata);
        }

        [HttpGet]
        public ActionResult PostRedirectGetTarget(string view)
        {
            MetadataModel metadata = TempData["Metadata"] as MetadataModel;

            TempData["Metadata"] = metadata;

            return(View(view, metadata));
        }

        [HttpPost]
        public ActionResult SaveMetadata(MetadataModel postedModel)
        {
            MetadataModel metadata = TempData["Metadata"] as MetadataModel;

            foreach (IModelContext context in metadata.Contexts)
            {
                eavClient.SaveContext(context);

                foreach (IModelRootContainer container in context.Containers)
                {
                    eavClient.SaveMetadata(container);
                }
            }

            TempData["Metadata"] = metadata;

            return (RedirectToAction("PostRedirectGetTarget", new { view = "Index" }));
        }

        #region Context
        [HttpGet]
        public ActionResult ContextEditorDialog()
        {
            MetadataModel metadata = TempData["Metadata"] as MetadataModel;

            ContextViewModel contextModel = metadata.DialogStack.Pop() as ContextViewModel;

            TempData["Metadata"] = metadata;

            return PartialView("ContextEditorDialog", contextModel);
        }

        [HttpPost]
        public ActionResult AddContext(MetadataModel postedModel)
        {
            MetadataModel metadata = TempData["Metadata"] as MetadataModel;

            // Create a blank to work with and use that as our dialog metadata
            IModelContext eavContext = objectFactory.Create<IModelContext>();

            eavContext.ContextID = metadata.NextContextID;

            metadata.Contexts.Add(eavContext);
            metadata.SelectedContextID = eavContext.ContextID.Value;

            metadata.DialogStack.Push(new ContextViewModel(eavContext));

            TempData["Metadata"] = metadata;

            return (BuildResult("Context Editor", Url.Content("~/Metadata/ContextEditorDialog"), Url.Content("~/Metadata/UpdateContext"), null));
        }

        [HttpPost]
        public ActionResult EditContext(MetadataModel postedModel)
        {
            MetadataModel metadata = TempData["Metadata"] as MetadataModel;

            metadata.SelectedContextID = postedModel.SelectedContextID;

            IModelContext eavContext = metadata.CurrentContext;

            // TODO: Check state after loading containers, verify that Modified doesn't go away if set
            if (eavContext.ObjectState != ObjectState.Deleted && eavContext.ObjectState != ObjectState.New && !eavContext.Containers.Any())
            {
                eavClient.LoadRootContainers(eavContext);
            }

            metadata.DialogStack.Push(new ContextViewModel(eavContext) { Existing = true });

            TempData["Metadata"] = metadata;

            return (BuildResult("Context Editor", Url.Content("~/Metadata/ContextEditorDialog"), Url.Content("~/Metadata/UpdateContext"), null));
        }

        [HttpPost]
        public ActionResult UpdateContext(ContextViewModel postedModel)
        {
            MetadataModel metadata = TempData["Metadata"] as MetadataModel;

            IModelContext eavContext = metadata.CurrentContext;

            if (UpdateRequested)
            {
                eavContext.Name = postedModel.Name;
                eavContext.DataName = postedModel.DataName;
                eavContext.DisplayText = postedModel.DisplayText;

                foreach (ContainerViewModel containerModel in postedModel.Containers)
                {
                    eavContext.Containers.Single(it => it.ContainerID == containerModel.ID).Sequence = containerModel.Sequence;
                }
            }
            else if (eavContext.ObjectState == ObjectState.New && (String.IsNullOrWhiteSpace(eavContext.Name) || (String.IsNullOrWhiteSpace(eavContext.DataName))) && !eavContext.Containers.Any())
            {
                metadata.Contexts.Remove(eavContext);
            }

            TempData["Metadata"] = metadata;

            return (BuildResult(null, null, null, metadata.Contexts.Select(it => new { Value = it.ContextID, Text = it.Name })));
        }
        #endregion

        #region Container
        [HttpGet]
        public ActionResult ContainerEditorDialog()
        {
            MetadataModel metadata = TempData["Metadata"] as MetadataModel;

            ContainerViewModel containerModel = metadata.DialogStack.Pop() as ContainerViewModel;

            TempData["Metadata"] = metadata;

            return PartialView("ContainerEditorDialog", containerModel);
        }

        [HttpPost]
        public ActionResult AddRootContainer(ContextViewModel postedModel)
        {
            MetadataModel metadata = TempData["Metadata"] as MetadataModel;

            postedModel.FixupContainerOrder();
            metadata.DialogStack.Push(postedModel);

            IModelRootContainer eavContainer = objectFactory.Create<EAV.Model.IModelRootContainer>();

            eavContainer.ContainerID = metadata.NextContainerID;
            eavContainer.Context = metadata.CurrentContext;
            eavContainer.Sequence = metadata.CurrentContext.Containers.Max(it => it.Sequence) + 1;

            metadata.DialogStack.Push(new ContainerViewModel(eavContainer));

            TempData["Metadata"] = metadata;

            return (BuildResult("Container Editor", Url.Content("~/Metadata/ContainerEditorDialog"), Url.Content("~/Metadata/UpdateRootContainer"), null));
        }

        [HttpPost]
        public ActionResult EditRootContainer(ContextViewModel postedModel)
        {
            MetadataModel metadata = TempData["Metadata"] as MetadataModel;

            postedModel.FixupContainerOrder();
            metadata.DialogStack.Push(postedModel);

            IModelRootContainer eavContainer = FindContainer(metadata.CurrentContext.Containers, ID) as IModelRootContainer;

            // TODO: Check state after loading metadata, verify that Modified doesn't go away if set
            if (eavContainer.ObjectState != ObjectState.Deleted && eavContainer.ObjectState != ObjectState.New && !eavContainer.ChildContainers.Any() && !eavContainer.Attributes.Any())
            {
                eavClient.LoadMetadata(eavContainer);
            }

            metadata.DialogStack.Push(new ContainerViewModel(eavContainer) { Existing = true });

            TempData["Metadata"] = metadata;

            return (BuildResult("Container Editor", Url.Content("~/Metadata/ContainerEditorDialog"), Url.Content("~/Metadata/UpdateRootContainer"), null));
        }

        [HttpPost]
        public ActionResult DeleteRootContainer(ContextViewModel postedModel)
        {
            MetadataModel metadata = TempData["Metadata"] as MetadataModel;

            metadata.DialogStack.Push(postedModel);

            IModelRootContainer eavContainer = FindContainer(metadata.CurrentContext.Containers, ID) as IModelRootContainer;

            if (eavContainer.ObjectState != ObjectState.New)
                eavContainer.MarkDeleted();
            else
                metadata.CurrentContext.Containers.Remove(eavContainer);

            TempData["Metadata"] = metadata;

            return (BuildResult("Context Editor", Url.Content("~/Metadata/ContextEditorDialog"), Url.Content("~/Metadata/UpdateContext"), null));
        }

        [HttpPost]
        public ActionResult UpdateRootContainer(ContainerViewModel postedModel)
        {
            MetadataModel metadata = TempData["Metadata"] as MetadataModel;

            IModelRootContainer eavContainer = FindContainer(metadata.CurrentContext.Containers, postedModel.ID) as IModelRootContainer;

            if (UpdateRequested)
            {
                eavContainer.Name = postedModel.Name;
                eavContainer.DataName = postedModel.DataName;
                eavContainer.DisplayText = postedModel.DisplayText;
                eavContainer.IsRepeating = postedModel.IsRepeating;

                foreach (AttributeViewModel attributeModel in postedModel.Attributes)
                {
                    eavContainer.Attributes.Single(it => it.AttributeID == attributeModel.ID).Sequence = attributeModel.Sequence;
                }

                foreach (ContainerViewModel containerModel in postedModel.ChildContainers)
                {
                    eavContainer.ChildContainers.Single(it => it.ContainerID == containerModel.ID).Sequence = containerModel.Sequence;
                }
            }
            else if (eavContainer.ObjectState == ObjectState.New && (String.IsNullOrWhiteSpace(eavContainer.Name) || (String.IsNullOrWhiteSpace(eavContainer.DataName))) && !eavContainer.ChildContainers.Any())
            {
                eavContainer.Context.Containers.Remove(eavContainer);
            }

            TempData["Metadata"] = metadata;

            return (BuildResult("Context Editor", Url.Content("~/Metadata/ContextEditorDialog"), Url.Content("~/Metadata/UpdateContext"), null));
        }

        [HttpPost]
        public ActionResult AddChildContainer(ContainerViewModel postedModel)
        {
            MetadataModel metadata = TempData["Metadata"] as MetadataModel;

            postedModel.FixupContainerOrder();
            postedModel.FixupAttributeOrder();
            metadata.DialogStack.Push(postedModel);

            IModelContainer eavParentContainer = FindContainer(metadata.CurrentContext.Containers, postedModel.ID);
            IModelChildContainer eavChildContainer = objectFactory.Create<EAV.Model.IModelChildContainer>();

            eavChildContainer.ContainerID = metadata.NextContainerID;
            eavChildContainer.ParentContainer = eavParentContainer;
            eavChildContainer.Sequence = eavParentContainer.ChildContainers.Max(it => it.Sequence) + 1;

            metadata.DialogStack.Push(new ContainerViewModel(eavChildContainer));

            TempData["Metadata"] = metadata;

            return (BuildResult("Container Editor", Url.Content("~/Metadata/ContainerEditorDialog"), Url.Content("~/Metadata/UpdateChildContainer"), null));
        }

        [HttpPost]
        public ActionResult EditChildContainer(ContainerViewModel postedModel)
        {
            MetadataModel metadata = TempData["Metadata"] as MetadataModel;

            postedModel.FixupContainerOrder();
            postedModel.FixupAttributeOrder();
            metadata.DialogStack.Push(postedModel);

            IModelContainer eavContainer = FindContainer(metadata.CurrentContext.Containers, ID);

            metadata.DialogStack.Push(new ContainerViewModel(eavContainer) { Existing = true });

            TempData["Metadata"] = metadata;

            return (BuildResult("Container Editor", Url.Content("~/Metadata/ContainerEditorDialog"), Url.Content("~/Metadata/UpdateChildContainer"), null));
        }

        [HttpPost]
        public ActionResult DeleteChildContainer(ContainerViewModel postedModel)
        {
            MetadataModel metadata = TempData["Metadata"] as MetadataModel;

            metadata.DialogStack.Push(postedModel);

            IModelChildContainer eavContainer = FindContainer(metadata.CurrentContext.Containers, ID) as IModelChildContainer;

            if (eavContainer.ObjectState != ObjectState.New)
                eavContainer.MarkDeleted();
            else
                eavContainer.ParentContainer.ChildContainers.Remove(eavContainer);

            TempData["Metadata"] = metadata;

            if (eavContainer.ParentContainer is IModelRootContainer)
                return (BuildResult("Container Editor", Url.Content("~/Metadata/ContainerEditorDialog"), Url.Content("~/Metadata/UpdateRootContainer"), null));
            else
                return (BuildResult("Container Editor", Url.Content("~/Metadata/ContainerEditorDialog"), Url.Content("~/Metadata/UpdateChildContainer"), null));
        }

        [HttpPost]
        public ActionResult UpdateChildContainer(ContainerViewModel postedModel)
        {
            MetadataModel metadata = TempData["Metadata"] as MetadataModel;

            IModelChildContainer eavContainer = FindContainer(metadata.CurrentContext.Containers, postedModel.ID) as IModelChildContainer;

            if (UpdateRequested)
            {
                eavContainer.Name = postedModel.Name;
                eavContainer.DataName = postedModel.DataName;
                eavContainer.DisplayText = postedModel.DisplayText;
                eavContainer.IsRepeating = postedModel.IsRepeating;

                foreach (AttributeViewModel attributeModel in postedModel.Attributes)
                {
                    eavContainer.Attributes.Single(it => it.AttributeID == attributeModel.ID).Sequence = attributeModel.Sequence;
                }

                foreach (ContainerViewModel containerModel in postedModel.ChildContainers)
                {
                    eavContainer.ChildContainers.Single(it => it.ContainerID == containerModel.ID).Sequence = containerModel.Sequence;
                }
            }
            else if (eavContainer.ObjectState == ObjectState.New && (String.IsNullOrWhiteSpace(eavContainer.Name) || (String.IsNullOrWhiteSpace(eavContainer.DataName))) && !eavContainer.ChildContainers.Any())
            {
                eavContainer.ParentContainer.ChildContainers.Remove(eavContainer);
            }

            TempData["Metadata"] = metadata;

            if (eavContainer.ParentContainer is IModelRootContainer)
                return (BuildResult("Container Editor", Url.Content("~/Metadata/ContainerEditorDialog"), Url.Content("~/Metadata/UpdateRootContainer"), null));
            else
                return (BuildResult("Container Editor", Url.Content("~/Metadata/ContainerEditorDialog"), Url.Content("~/Metadata/UpdateChildContainer"), null));
        }
        #endregion

        #region Attribute
        [HttpGet]
        public ActionResult AttributeEditorDialog()
        {
            MetadataModel metadata = TempData["Metadata"] as MetadataModel;

            AttributeViewModel attributeModel = metadata.DialogStack.Pop() as AttributeViewModel;

            TempData["Metadata"] = metadata;

            return PartialView("AttributeEditorDialog", attributeModel);
        }

        [HttpPost]
        public ActionResult AddAttribute(ContainerViewModel postedModel)
        {
            MetadataModel metadata = TempData["Metadata"] as MetadataModel;

            metadata.DialogStack.Push(postedModel);

            IModelContainer eavContainer = FindContainer(metadata.CurrentContext.Containers, postedModel.ID);
            IModelAttribute eavAttribute = objectFactory.Create<EAV.Model.IModelAttribute>();

            eavAttribute.AttributeID = metadata.NextAttributeID;
            eavAttribute.Container = eavContainer;
            eavAttribute.Sequence = eavContainer.Attributes.Max(it => it.Sequence) + 1;

            metadata.DialogStack.Push(new AttributeViewModel(eavAttribute));

            TempData["Metadata"] = metadata;

            return (BuildResult("Attribute Editor", Url.Content("~/Metadata/AttributeEditorDialog"), Url.Content("~/Metadata/UpdateAttribute"), null));
        }

        [HttpPost]
        public ActionResult EditAttribute(ContainerViewModel postedModel)
        {
            MetadataModel metadata = TempData["Metadata"] as MetadataModel;

            metadata.DialogStack.Push(postedModel);

            IModelContainer eavContainer = FindContainer(metadata.CurrentContext.Containers, postedModel.ID);
            IModelAttribute eavAttribute = eavContainer.Attributes.Single(it => it.AttributeID == ID);

            metadata.DialogStack.Push(new AttributeViewModel(eavAttribute) { Existing = true });

            TempData["Metadata"] = metadata;

            return (BuildResult("Attribute Editor", Url.Content("~/Metadata/AttributeEditorDialog"), Url.Content("~/Metadata/UpdateAttribute"), null));
        }

        [HttpPost]
        public ActionResult DeleteAttribute(ContainerViewModel postedModel)
        {
            MetadataModel metadata = TempData["Metadata"] as MetadataModel;

            metadata.DialogStack.Push(postedModel);

            IModelContainer eavContainer = FindContainer(metadata.CurrentContext.Containers, postedModel.ID);
            IModelAttribute eavAttribute = eavContainer.Attributes.Single(it => it.AttributeID == ID);

            if (eavAttribute.ObjectState != ObjectState.New)
                eavAttribute.MarkDeleted();
            else
                eavContainer.Attributes.Remove(eavAttribute);

            TempData["Metadata"] = metadata;

            if (eavContainer is IModelRootContainer)
                return (BuildResult("Container Editor", Url.Content("~/Metadata/ContainerEditorDialog"), Url.Content("~/Metadata/UpdateRootContainer"), null));
            else
                return (BuildResult("Container Editor", Url.Content("~/Metadata/ContainerEditorDialog"), Url.Content("~/Metadata/UpdateChildContainer"), null));
        }

        [HttpPost]
        public ActionResult UpdateAttribute(AttributeViewModel postedModel)
        {
            MetadataModel metadata = TempData["Metadata"] as MetadataModel;

            IModelContainer eavContainer = FindContainer(metadata.CurrentContext.Containers, postedModel.ContainerID);
            IModelAttribute eavAttribute = eavContainer.Attributes.Single(it => it.AttributeID == postedModel.ID);

            if (UpdateRequested)
            {
                eavAttribute.Name = postedModel.Name;
                eavAttribute.DataName = postedModel.DataName;
                eavAttribute.DisplayText = postedModel.DisplayText;
                eavAttribute.DataType = postedModel.DataType;
                eavAttribute.IsKey = postedModel.IsKey;
            }
            else if (eavAttribute.ObjectState == ObjectState.New && (String.IsNullOrWhiteSpace(eavAttribute.Name) || (String.IsNullOrWhiteSpace(eavAttribute.DataName))))
            {
                eavContainer.Attributes.Remove(eavAttribute);
            }

            TempData["Metadata"] = metadata;

            if (eavContainer is IModelRootContainer)
                return (BuildResult("Container Editor", Url.Content("~/Metadata/ContainerEditorDialog"), Url.Content("~/Metadata/UpdateRootContainer"), null));
            else
                return (BuildResult("Container Editor", Url.Content("~/Metadata/ContainerEditorDialog"), Url.Content("~/Metadata/UpdateChildContainer"), null));
        }
        #endregion
    }
}