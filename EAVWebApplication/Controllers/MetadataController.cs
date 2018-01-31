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

namespace EAVWebApplication.Controllers
{
    public class MetadataController : Controller
    {
        HttpClient client = new HttpClient() { BaseAddress = new Uri("http://localhost:10240/") };
        EAVClient eavClient = new EAVClient();
        MetadataModel model;

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

        private JsonResult BuildResult(string dialogTitle, string dialogURL, string updateURL)
        {
            return (Json(new
            {
                dialogTitle = dialogTitle,
                dialogURL = dialogURL,
                updateURL = updateURL,
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
            model = new MetadataModel();

            // Add any existing contexts
            foreach (var item in eavClient.LoadContexts(client))
            {
                model.Contexts.Add(item);
            }

            // Force validation so that UI lights up
            TryValidateModel(model);

            TempData["MetadataModel"] = model;

            return View("Index", model);
        }

        [HttpGet]
        public ActionResult PostRedirectGetTarget(string view)
        {
            model = TempData["MetadataModel"] as MetadataModel;

            TempData["MetadataModel"] = model;

            return(View(view, model));
        }

        [HttpPost]
        public ActionResult SaveMetadata(MetadataModel postedModel)
        {
            model = TempData["MetadataModel"] as MetadataModel;

            EAVContext emptyContext = model.Contexts.SingleOrDefault(it => it.ContextID.GetValueOrDefault() == 0 && (String.IsNullOrWhiteSpace(it.Name) || String.IsNullOrWhiteSpace(it.DataName)));

            model.Contexts.Remove(emptyContext);

            foreach (EAVContext context in model.Contexts)
            {
                eavClient.SaveMetadata(client, context);
            }

            model.Contexts.Add(emptyContext ?? new EAVContext());

            TempData["MetadataModel"] = model;

            return (RedirectToAction("PostRedirectGetTarget", new { view = "Index" }));
        }

        #region Context
        [HttpGet]
        public ActionResult ContextEditorDialog()
        {
            model = TempData["MetadataModel"] as MetadataModel;

            ContextModel context = model.TheStack.Pop() as ContextModel;

            context.InitializeContainers(model.CurrentContext);

            TempData["MetadataModel"] = model;

            return PartialView("ContextEditorDialog", context);
        }

        [HttpPost]
        public ActionResult AddContext(MetadataModel postedModel)
        {
            model = TempData["MetadataModel"] as MetadataModel;

            // Create a blank to work with and use that as our dialog model
            EAVContext context = new EAVContext() { ContextID = model.NextContextID };

            model.Contexts.Add(context);
            model.SelectedContextID = context.ContextID.Value;

            model.TheStack.Push((ContextModel) context);

            TempData["MetadataModel"] = model;

            return (BuildResult("Container Editor", Url.Content("~/Metadata/ContextEditorDialog"), Url.Content("~/Metadata/UpdateContext")));
        }

        [HttpPost]
        public ActionResult EditContext(MetadataModel postedModel)
        {
            model = TempData["MetadataModel"] as MetadataModel;

            model.SelectedContextID = postedModel.SelectedContextID;

            EAVContext context = model.CurrentContext;

            // TODO: Check state after loading containers, verify that Modified doesn't go away if set
            if (context.ObjectState != ObjectState.Deleted && context.ObjectState != ObjectState.New && !context.Containers.Any())
            {
                eavClient.LoadRootContainers(client, context);
            }

            model.TheStack.Push((ContextModel)context);

            TempData["MetadataModel"] = model;

            return (BuildResult("Context Editor", Url.Content("~/Metadata/ContextEditorDialog"), Url.Content("~/Metadata/UpdateContext")));
        }

        [HttpPost]
        public ActionResult UpdateContext(ContextModel postedModel)
        {
            model = TempData["MetadataModel"] as MetadataModel;

            EAVContext context = model.CurrentContext;

            if (UpdateRequested)
            {
                context.Name = postedModel.Name;
                context.DataName = postedModel.DataName;
                context.DisplayText = postedModel.DisplayText;
            }
            else if (context.ObjectState == ObjectState.New && (String.IsNullOrWhiteSpace(context.Name) || (String.IsNullOrWhiteSpace(context.DataName))) && !context.Containers.Any())
            {
                model.Contexts.Remove(context);
            }

            TempData["MetadataModel"] = model;

            return (BuildResult(null, null, null));
        }
        #endregion

        #region Container
        [HttpGet]
        public ActionResult ContainerEditorDialog()
        {
            model = TempData["MetadataModel"] as MetadataModel;

            ContainerModel container = model.TheStack.Pop() as ContainerModel;

            container.InitializeContainers(FindContainer(model.CurrentContext.Containers, container.ID));

            TempData["MetadataModel"] = model;

            return PartialView("ContainerEditorDialog", container);
        }

        [HttpPost]
        public ActionResult AddRootContainer(ContextModel postedModel)
        {
            model = TempData["MetadataModel"] as MetadataModel;

            model.TheStack.Push(postedModel);

            // Create a blank to work with and use that as our dialog model
            EAVRootContainer container = new EAVRootContainer() { ContainerID = model.NextContainerID, Context = model.CurrentContext };

            model.TheStack.Push((ContainerModel) container);

            TempData["MetadataModel"] = model;

            return (BuildResult("Container Editor", Url.Content("~/Metadata/ContainerEditorDialog"), Url.Content("~/Metadata/UpdateRootContainer")));
        }

        [HttpPost]
        public ActionResult EditRootContainer(ContextModel postedModel)
        {
            model = TempData["MetadataModel"] as MetadataModel;

            model.TheStack.Push(postedModel);

            EAVContainer container = FindContainer(model.CurrentContext.Containers, ID);

            model.TheStack.Push((ContainerModel)container);

            TempData["MetadataModel"] = model;

            return (BuildResult("Container Editor", Url.Content("~/Metadata/ContainerEditorDialog"), Url.Content("~/Metadata/UpdateRootContainer")));
        }

        [HttpPost]
        public ActionResult UpdateRootContainer(ContainerModel postedModel)
        {
            model = TempData["MetadataModel"] as MetadataModel;

            EAVRootContainer container = FindContainer(model.CurrentContext.Containers, postedModel.ID) as EAVRootContainer;

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

            TempData["MetadataModel"] = model;

            return (BuildResult("Context Editor", Url.Content("~/Metadata/ContextEditorDialog"), Url.Content("~/Metadata/UpdateContext")));
        }

        [HttpPost]
        public ActionResult AddChildContainer(ContainerModel postedModel)
        {
            model = TempData["MetadataModel"] as MetadataModel;

            model.TheStack.Push(postedModel);

            EAVContainer parentContainer = FindContainer(model.CurrentContext.Containers, postedModel.ID);
            EAVChildContainer childContainer = new EAVChildContainer() { ContainerID = model.NextContainerID, ParentContainer = parentContainer };

            model.TheStack.Push((ContainerModel)childContainer);

            TempData["MetadataModel"] = model;

            return (BuildResult("Container Editor", Url.Content("~/Metadata/ContainerEditorDialog"), Url.Content("~/Metadata/UpdateChildContainer")));
        }

        [HttpPost]
        public ActionResult EditChildContainer(ContainerModel postedModel)
        {
            model = TempData["MetadataModel"] as MetadataModel;

            model.TheStack.Push(postedModel);

            EAVContainer container = FindContainer(model.CurrentContext.Containers, ID);

            model.TheStack.Push((ContainerModel)container);

            TempData["MetadataModel"] = model;

            return (BuildResult("Container Editor", Url.Content("~/Metadata/ContainerEditorDialog"), Url.Content("~/Metadata/UpdateChildContainer")));
        }

        [HttpPost]
        public ActionResult UpdateChildContainer(ContainerModel postedModel)
        {
            model = TempData["MetadataModel"] as MetadataModel;

            EAVChildContainer container = FindContainer(model.CurrentContext.Containers, postedModel.ID) as EAVChildContainer;

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

            TempData["MetadataModel"] = model;

            if (container.ParentContainer is EAVRootContainer)
                return (BuildResult("Container Editor", Url.Content("~/Metadata/ContainerEditorDialog"), Url.Content("~/Metadata/UpdateRootContainer")));
            else
                return (BuildResult("Container Editor", Url.Content("~/Metadata/ContainerEditorDialog"), Url.Content("~/Metadata/UpdateChildContainer")));
        }
        #endregion
    }
}