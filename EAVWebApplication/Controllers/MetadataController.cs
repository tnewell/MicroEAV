using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

using EAVServiceClient;
using EAVFramework.Model;
using Newtonsoft.Json;
using System.Net;
using System.Diagnostics;
using System.Configuration;

namespace EAVWebApplication.Controllers
{
    public class MetadataController : Controller
    {
        private HttpClient client = new HttpClient() { BaseAddress = new Uri(ConfigurationManager.AppSettings["EAVServiceUrl"]) };
        private EAVClient eavClient = new EAVClient();

        public MetadataController()
        {
        }

        protected bool UpdateRequested { get { return (Boolean.TryParse(Request["update"] ?? Boolean.FalseString, out Boolean x) ? x : false); } }

        protected int ID { get { return (Int32.TryParse(Request["id"] ?? "0", out int x) ? x : 0); } }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                client.Dispose();
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

        private EAVContainer FindContainer(IEnumerable<EAVContainer> containers, int id)
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
            foreach (var item in eavClient.LoadContexts(client))
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

            foreach (EAVContext context in metadata.Contexts)
            {
                eavClient.SaveMetadata(client, context);
            }

            TempData["Metadata"] = metadata;

            return (RedirectToAction("PostRedirectGetTarget", new { view = "Index" }));
        }

        #region Context
        [HttpGet]
        public ActionResult ContextEditorDialog()
        {
            MetadataModel metadata = TempData["Metadata"] as MetadataModel;

            ContextModel context = metadata.DialogStack.Pop() as ContextModel;

            context.InitializeContainers(metadata.CurrentContext);

            TempData["Metadata"] = metadata;

            return PartialView("ContextEditorDialog", context);
        }

        [HttpPost]
        public ActionResult AddContext(MetadataModel postedModel)
        {
            MetadataModel metadata = TempData["Metadata"] as MetadataModel;

            // Create a blank to work with and use that as our dialog metadata
            EAVContext context = new EAVContext() { ContextID = metadata.NextContextID };

            metadata.Contexts.Add(context);
            metadata.SelectedContextID = context.ContextID.Value;

            metadata.DialogStack.Push((ContextModel) context);

            TempData["Metadata"] = metadata;

            return (BuildResult("Container Editor", Url.Content("~/Metadata/ContextEditorDialog"), Url.Content("~/Metadata/UpdateContext"), null));
        }

        [HttpPost]
        public ActionResult EditContext(MetadataModel postedModel)
        {
            MetadataModel metadata = TempData["Metadata"] as MetadataModel;

            metadata.SelectedContextID = postedModel.SelectedContextID;

            EAVContext context = metadata.CurrentContext;

            // TODO: Check state after loading containers, verify that Modified doesn't go away if set
            if (context.ObjectState != ObjectState.Deleted && context.ObjectState != ObjectState.New && !context.Containers.Any())
            {
                eavClient.LoadRootContainers(client, context);
            }

            metadata.DialogStack.Push(new ContextModel(context) { Existing = true });

            TempData["Metadata"] = metadata;

            return (BuildResult("Context Editor", Url.Content("~/Metadata/ContextEditorDialog"), Url.Content("~/Metadata/UpdateContext"), null));
        }

        [HttpPost]
        public ActionResult UpdateContext(ContextModel postedModel)
        {
            MetadataModel metadata = TempData["Metadata"] as MetadataModel;

            EAVContext context = metadata.CurrentContext;

            if (UpdateRequested)
            {
                context.Name = postedModel.Name;
                context.DataName = postedModel.DataName;
                context.DisplayText = postedModel.DisplayText;
            }
            else if (context.ObjectState == ObjectState.New && (String.IsNullOrWhiteSpace(context.Name) || (String.IsNullOrWhiteSpace(context.DataName))) && !context.Containers.Any())
            {
                metadata.Contexts.Remove(context);
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

            ContainerModel container = metadata.DialogStack.Pop() as ContainerModel;
            EAVContainer parentContainer = FindContainer(metadata.CurrentContext.Containers, container.ID);

            container.InitializeContainers(parentContainer);
            container.InitializeAttributes(parentContainer);

            TempData["Metadata"] = metadata;

            return PartialView("ContainerEditorDialog", container);
        }

        [HttpPost]
        public ActionResult AddRootContainer(ContextModel postedModel)
        {
            MetadataModel metadata = TempData["Metadata"] as MetadataModel;

            metadata.DialogStack.Push(postedModel);

            // Create a blank to work with and use that as our dialog metadata
            EAVRootContainer container = new EAVRootContainer() { ContainerID = metadata.NextContainerID, Context = metadata.CurrentContext };

            metadata.DialogStack.Push((ContainerModel) container);

            TempData["Metadata"] = metadata;

            return (BuildResult("Container Editor", Url.Content("~/Metadata/ContainerEditorDialog"), Url.Content("~/Metadata/UpdateRootContainer"), null));
        }

        [HttpPost]
        public ActionResult EditRootContainer(ContextModel postedModel)
        {
            MetadataModel metadata = TempData["Metadata"] as MetadataModel;

            metadata.DialogStack.Push(postedModel);

            EAVContainer container = FindContainer(metadata.CurrentContext.Containers, ID);

            metadata.DialogStack.Push(new ContainerModel(container) { Existing = true });

            TempData["Metadata"] = metadata;

            return (BuildResult("Container Editor", Url.Content("~/Metadata/ContainerEditorDialog"), Url.Content("~/Metadata/UpdateRootContainer"), null));
        }

        [HttpPost]
        public ActionResult DeleteRootContainer(ContextModel postedModel)
        {
            MetadataModel metadata = TempData["Metadata"] as MetadataModel;

            metadata.DialogStack.Push(postedModel);

            EAVRootContainer container = FindContainer(metadata.CurrentContext.Containers, ID) as EAVRootContainer;

            if (container.ObjectState != ObjectState.New)
                container.MarkDeleted();
            else
                metadata.CurrentContext.Containers.Remove(container);

            TempData["Metadata"] = metadata;

            return (BuildResult("Context Editor", Url.Content("~/Metadata/ContextEditorDialog"), Url.Content("~/Metadata/UpdateContext"), null));
        }

        [HttpPost]
        public ActionResult UpdateRootContainer(ContainerModel postedModel)
        {
            MetadataModel metadata = TempData["Metadata"] as MetadataModel;

            EAVRootContainer container = FindContainer(metadata.CurrentContext.Containers, postedModel.ID) as EAVRootContainer;

            if (UpdateRequested)
            {
                container.Name = postedModel.Name;
                container.DataName = postedModel.DataName;
                container.DisplayText = postedModel.DisplayText;
                container.IsRepeating = postedModel.IsRepeating;
            }
            else if (container.ObjectState == ObjectState.New && (String.IsNullOrWhiteSpace(container.Name) || (String.IsNullOrWhiteSpace(container.DataName))) && !container.ChildContainers.Any())
            {
                container.Context.Containers.Remove(container);
            }

            TempData["Metadata"] = metadata;

            return (BuildResult("Context Editor", Url.Content("~/Metadata/ContextEditorDialog"), Url.Content("~/Metadata/UpdateContext"), null));
        }

        [HttpPost]
        public ActionResult AddChildContainer(ContainerModel postedModel)
        {
            MetadataModel metadata = TempData["Metadata"] as MetadataModel;

            metadata.DialogStack.Push(postedModel);

            EAVContainer parentContainer = FindContainer(metadata.CurrentContext.Containers, postedModel.ID);
            EAVChildContainer childContainer = new EAVChildContainer() { ContainerID = metadata.NextContainerID, ParentContainer = parentContainer };

            metadata.DialogStack.Push((ContainerModel)childContainer);

            TempData["Metadata"] = metadata;

            return (BuildResult("Container Editor", Url.Content("~/Metadata/ContainerEditorDialog"), Url.Content("~/Metadata/UpdateChildContainer"), null));
        }

        [HttpPost]
        public ActionResult EditChildContainer(ContainerModel postedModel)
        {
            MetadataModel metadata = TempData["Metadata"] as MetadataModel;

            metadata.DialogStack.Push(postedModel);

            EAVContainer container = FindContainer(metadata.CurrentContext.Containers, ID);

            metadata.DialogStack.Push(new ContainerModel(container) { Existing = true });

            TempData["Metadata"] = metadata;

            return (BuildResult("Container Editor", Url.Content("~/Metadata/ContainerEditorDialog"), Url.Content("~/Metadata/UpdateChildContainer"), null));
        }

        [HttpPost]
        public ActionResult DeleteChildContainer(ContainerModel postedModel)
        {
            MetadataModel metadata = TempData["Metadata"] as MetadataModel;

            metadata.DialogStack.Push(postedModel);

            EAVChildContainer container = FindContainer(metadata.CurrentContext.Containers, ID) as EAVChildContainer;

            if (container.ObjectState != ObjectState.New)
                container.MarkDeleted();
            else
                container.ParentContainer.ChildContainers.Remove(container);

            TempData["Metadata"] = metadata;

            if (container.ParentContainer is EAVRootContainer)
                return (BuildResult("Container Editor", Url.Content("~/Metadata/ContainerEditorDialog"), Url.Content("~/Metadata/UpdateRootContainer"), null));
            else
                return (BuildResult("Container Editor", Url.Content("~/Metadata/ContainerEditorDialog"), Url.Content("~/Metadata/UpdateChildContainer"), null));
        }

        [HttpPost]
        public ActionResult UpdateChildContainer(ContainerModel postedModel)
        {
            MetadataModel metadata = TempData["Metadata"] as MetadataModel;

            EAVChildContainer container = FindContainer(metadata.CurrentContext.Containers, postedModel.ID) as EAVChildContainer;

            if (UpdateRequested)
            {
                container.Name = postedModel.Name;
                container.DataName = postedModel.DataName;
                container.DisplayText = postedModel.DisplayText;
                container.IsRepeating = postedModel.IsRepeating;
            }
            else if (container.ObjectState == ObjectState.New && (String.IsNullOrWhiteSpace(container.Name) || (String.IsNullOrWhiteSpace(container.DataName))) && !container.ChildContainers.Any())
            {
                container.ParentContainer.ChildContainers.Remove(container);
            }

            TempData["Metadata"] = metadata;

            if (container.ParentContainer is EAVRootContainer)
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

            AttributeModel attribute = metadata.DialogStack.Pop() as AttributeModel;

            TempData["Metadata"] = metadata;

            return PartialView("AttributeEditorDialog", attribute);
        }

        [HttpPost]
        public ActionResult AddAttribute(ContainerModel postedModel)
        {
            MetadataModel metadata = TempData["Metadata"] as MetadataModel;

            metadata.DialogStack.Push(postedModel);

            // Create a blank to work with and use that as our dialog metadata
            EAVAttribute attribute = new EAVAttribute() { AttributeID = metadata.NextAttributeID, Container = FindContainer(metadata.CurrentContext.Containers, postedModel.ID) };

            metadata.DialogStack.Push((AttributeModel)attribute);

            TempData["Metadata"] = metadata;

            return (BuildResult("Attribute Editor", Url.Content("~/Metadata/AttributeEditorDialog"), Url.Content("~/Metadata/UpdateAttribute"), null));
        }

        [HttpPost]
        public ActionResult EditAttribute(ContainerModel postedModel)
        {
            MetadataModel metadata = TempData["Metadata"] as MetadataModel;

            metadata.DialogStack.Push(postedModel);

            EAVContainer container = FindContainer(metadata.CurrentContext.Containers, postedModel.ID);
            EAVAttribute attribute = container.Attributes.Single(it => it.AttributeID == ID);

            metadata.DialogStack.Push(new AttributeModel(attribute) { Existing = true });

            TempData["Metadata"] = metadata;

            return (BuildResult("Attribute Editor", Url.Content("~/Metadata/AttributeEditorDialog"), Url.Content("~/Metadata/UpdateAttribute"), null));
        }

        [HttpPost]
        public ActionResult DeleteAttribute(ContainerModel postedModel)
        {
            MetadataModel metadata = TempData["Metadata"] as MetadataModel;

            metadata.DialogStack.Push(postedModel);

            EAVContainer container = FindContainer(metadata.CurrentContext.Containers, postedModel.ID);
            EAVAttribute attribute = container.Attributes.Single(it => it.AttributeID == ID);

            if (attribute.ObjectState != ObjectState.New)
                attribute.MarkDeleted();
            else
                container.Attributes.Remove(attribute);

            TempData["Metadata"] = metadata;

            if (container is EAVRootContainer)
                return (BuildResult("Container Editor", Url.Content("~/Metadata/ContainerEditorDialog"), Url.Content("~/Metadata/UpdateRootContainer"), null));
            else
                return (BuildResult("Container Editor", Url.Content("~/Metadata/ContainerEditorDialog"), Url.Content("~/Metadata/UpdateChildContainer"), null));
        }

        [HttpPost]
        public ActionResult UpdateAttribute(AttributeModel postedModel)
        {
            MetadataModel metadata = TempData["Metadata"] as MetadataModel;

            EAVContainer container = FindContainer(metadata.CurrentContext.Containers, postedModel.ContainerID);
            EAVAttribute attribute = container.Attributes.Single(it => it.AttributeID == postedModel.ID);

            if (UpdateRequested)
            {
                attribute.Name = postedModel.Name;
                attribute.DataName = postedModel.DataName;
                attribute.DisplayText = postedModel.DisplayText;
                attribute.DataType = postedModel.DataType;
                attribute.IsKey = postedModel.IsKey;
            }
            else if (attribute.ObjectState == ObjectState.New && (String.IsNullOrWhiteSpace(attribute.Name) || (String.IsNullOrWhiteSpace(attribute.DataName))))
            {
                container.Attributes.Remove(attribute);
            }

            TempData["Metadata"] = metadata;

            if (container is EAVRootContainer)
                return (BuildResult("Container Editor", Url.Content("~/Metadata/ContainerEditorDialog"), Url.Content("~/Metadata/UpdateRootContainer"), null));
            else
                return (BuildResult("Container Editor", Url.Content("~/Metadata/ContainerEditorDialog"), Url.Content("~/Metadata/UpdateChildContainer"), null));
        }
        #endregion
    }
}