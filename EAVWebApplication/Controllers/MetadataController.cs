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
                return (null);

            return (containers.SingleOrDefault(it => it.ContainerID == id) ?? FindContainer(containers.SelectMany(it => it.ChildContainers), id));
        }

        [HttpGet]
        public ActionResult Index()
        {
            model = new MetadataModel();

            // Add a blank
            model.Contexts.Add(new EAVContext());

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

            var dialogModel = model.TheStack.Pop();

            TempData["MetadataModel"] = model;

            return PartialView("ContextEditorDialog", dialogModel);
        }

        [HttpPost]
        public ActionResult AddContext(MetadataModel postedModel)
        {
            model = TempData["MetadataModel"] as MetadataModel;

            // TODO: Maybe add new context here?

            model.TheStack.Push(new ContextModel());

            TempData["MetadataModel"] = model;

            return (BuildResult("Container Editor", Url.Content("~/Metadata/ContextEditorDialog"), Url.Content("~/Metadata/UpdateContext")));
        }

        [HttpPost]
        public ActionResult EditContext(MetadataModel postedModel)
        {
            model = TempData["MetadataModel"] as MetadataModel;

            model.SelectedContextID = postedModel.SelectedContextID;

            EAVContext context = model.Contexts.Single(it => it.ContextID.GetValueOrDefault() == model.SelectedContextID);

            if (!context.Containers.Any())
                eavClient.LoadRootContainers(client, context);

            model.TheStack.Push(new ContextModel()
            {
                ID = context.ContextID.GetValueOrDefault(),
                Name = context.Name,
                DataName = context.DataName,
                DisplayText = context.DisplayText,
            });

            TempData["MetadataModel"] = model;

            return (BuildResult("Context Editor", Url.Content("~/Metadata/ContextEditorDialog"), Url.Content("~/Metadata/UpdateContext")));
        }

        [HttpPost]
        public ActionResult UpdateContext(ContextModel postedModel)
        {
            model = TempData["MetadataModel"] as MetadataModel;

            // See if we should be doing an update
            if (Boolean.Parse(Request["update"]))
            {
                EAVContext context = model.Contexts.Single(it => it.ContextID.GetValueOrDefault() == postedModel.ID);

                context.Name = postedModel.Name;
                context.DataName = postedModel.DataName;
                context.DisplayText = postedModel.DisplayText;
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

            var dialogModel = model.TheStack.Pop();

            TempData["MetadataModel"] = model;

            return PartialView("ContainerEditorDialog", dialogModel);
        }

        [HttpPost]
        public ActionResult AddRootContainer(ContextModel postedModel)
        {
            model = TempData["MetadataModel"] as MetadataModel;

            model.TheStack.Push(postedModel);

            EAVRootContainer container = new EAVRootContainer() { ContainerID = model.NextContainerID, Context = model.CurrentContext };

            model.TheStack.Push(new ContainerModel() { ID = container.ContainerID.Value });

            TempData["MetadataModel"] = model;

            return (BuildResult("Container Editor", Url.Content("~/Metadata/ContainerEditorDialog"), Url.Content("~/Metadata/UpdateRootContainer")));
        }

        [HttpPost]
        public ActionResult UpdateRootContainer(ContainerModel postedModel)
        {
            model = TempData["MetadataModel"] as MetadataModel;

            EAVContainer container = FindContainer(model.CurrentContext.Containers, postedModel.ID);

            // See if we should be doing an update
            if (Boolean.Parse(Request["update"]))
            {
                container.Name = postedModel.Name;
                container.DataName = postedModel.DataName;
                container.DisplayText = postedModel.DisplayText;
                container.IsRepeating = postedModel.IsRepeating;
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

            model.TheStack.Push(new ContainerModel() { ID = childContainer.ContainerID.Value, ParentID = childContainer.ParentContainerID.Value });

            TempData["MetadataModel"] = model;

            return (BuildResult("Container Editor", Url.Content("~/Metadata/ContainerEditorDialog"), Url.Content("~/Metadata/UpdateChildContainer")));
        }

        [HttpPost]
        public ActionResult UpdateChildContainer(ContainerModel postedModel)
        {
            model = TempData["MetadataModel"] as MetadataModel;

            EAVContainer container = FindContainer(model.CurrentContext.Containers, postedModel.ID);

            // See if we should be doing an update
            if (Boolean.Parse(Request["update"]))
            {
                container.Name = postedModel.Name;
                container.DataName = postedModel.DataName;
                container.DisplayText = postedModel.DisplayText;
                container.IsRepeating = postedModel.IsRepeating;
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