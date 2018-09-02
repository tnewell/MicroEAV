using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

using EAV.Model;

using EAVModelClient;

using EAVWebApplication.Models.Data;


namespace EAVWebApplication.Controllers
{
    public class DataController : Controller
    {
        private static readonly string TempDataModelKey = "DataModel";

        private ModelClient eavClient = new ModelClient(ConfigurationManager.AppSettings["EAVServiceUrl"]);

        public ICollection<IModelContext> ContextMasterList
        {
            get
            {
                return (Session["Contexts"] as ICollection<IModelContext>);
            }
            set
            {
                Session["Contexts"] = value;
            }
        }

        public DataController()
        {
        }

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            ContextMasterList = new List<IModelContext>();
        }

        private void ReconcileInstance(IModelInstance original, IModelInstance modified)
        {
            if (original == null)
                throw (new ArgumentNullException("original", "Parameter 'original' must not be null."));

            if (modified != null)
            {
                var valueSet1 = original.Values.GroupJoin(modified.Values, org => org.AttributeID, mod => mod.AttributeID, (org, mod) => new { Original = org, Modified = mod.SingleOrDefault() });
                var valueSet2 = modified.Values.GroupJoin(original.Values, mod => mod.AttributeID, org => org.AttributeID, (mod, org) => new { Original = org.SingleOrDefault(), Modified = mod });

                foreach (var valuePair in valueSet1.Union(valueSet2)) // Update
                {
                    if (valuePair.Original != null && valuePair.Modified != null) // Update
                    {
                        valuePair.Original.RawValue = valuePair.Modified.RawValue;
                        valuePair.Original.Unit = valuePair.Modified.Unit;
                    }
                    else if (valuePair.Original == null && valuePair.Modified != null) // Insert
                    {
                        original.Values.Add(valuePair.Modified);
                    }
                    else if (valuePair.Original != null && valuePair.Modified == null) // Delete
                    {
                        valuePair.Original.MarkDeleted();
                    }
                }

                // TODO: Eventually may be able to reduce this to one statement
                var instanceSet1 = original.ChildInstances.GroupJoin(modified.ChildInstances, org => org.InstanceID, mod => mod.InstanceID, (org, mod) => new { Original = org, Modified = mod.SingleOrDefault() });
                var instanceSet2 = modified.ChildInstances.GroupJoin(original.ChildInstances, mod => mod.InstanceID, org => org.InstanceID, (mod, org) => new { Original = org.SingleOrDefault(), Modified = mod });

                foreach (var instancePair in instanceSet1.Union(instanceSet2))
                {
                    ReconcileInstance(instancePair.Original, instancePair.Modified.ParentInstance);
                }
            }
            else if (modified == null) // Delete
            {
                original.MarkDeleted();
            }
        }

        [HttpGet]
        public ActionResult Index()
        {
            DataModel data = new DataModel();

            // Add any existing contexts
            foreach (var item in eavClient.LoadContexts())
            {
                eavClient.LoadSubjects(item);

                ContextMasterList.Add(item);
            }

            data.Contexts = ContextMasterList;

            TempData[TempDataModelKey] = data;

            return View("Index", data);
        }

        [HttpGet]
        public ActionResult PostRedirectGetTarget(string view)
        {
            DataModel data = TempData[TempDataModelKey] as DataModel;

            TempData[TempDataModelKey] = data;

            return (View(view, data));
        }

        [HttpGet]
        [Route("{contextID}", Name="GetRootContainers")]
        public ActionResult GetRootContainers(int contextID)
        {
            DataModel data = TempData[TempDataModelKey] as DataModel;

            data.SelectedContextID = contextID;

            IEnumerable<object> containers = Enumerable.Empty<object>();

            if (data.CurrentContext != null)
            {
                eavClient.LoadRootContainers(data.CurrentContext);

                containers = data.CurrentContext.Containers.Select(it => new { Text = it.Name, Value = it.ContainerID });
            }

            TempData[TempDataModelKey] = data;

            return (new JsonResult() { Data = containers, JsonRequestBehavior = JsonRequestBehavior.AllowGet });
        }

        [HttpPost]
        public ActionResult EditForm(DataModel postedModel)
        {
            DataModel data = TempData[TempDataModelKey] as DataModel;

            data.SelectedContainerID = postedModel.SelectedContainerID;

            if (data.CurrentContainer != null)
            {
                eavClient.LoadMetadata(data.CurrentContainer);
            }

            // Temporarily select a subject and add a blank instance
            data.SelectedSubjectID = 1;

            data.CurrentInstance = new ViewRootInstance(ModelRootInstance.Create(data.CurrentContainer, data.CurrentSubject));

            TempData[TempDataModelKey] = data;

            return (RedirectToAction("PostRedirectGetTarget", new { view = "DisplayContainer" }));
        }

        [HttpPost]
        public ActionResult SaveForm(DataModel postedModel)
        {
            DataModel data = TempData[TempDataModelKey] as DataModel;

            //ReconcileInstance(data.CurrentInstance, postedModel.CurrentInstance);
            //ReconcileInstance(data.viewInstance, postedModel.CurrentInstance);

            TempData[TempDataModelKey] = data;

            return (RedirectToAction("PostRedirectGetTarget", new { view = "DisplayContainer" }));
        }
    }
}