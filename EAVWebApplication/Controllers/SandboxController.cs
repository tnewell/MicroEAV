using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using EAVModelClient;

using EAVWebApplication.Models.Data;


namespace EAVWebApplication.Controllers
{
    public class SandboxController : Controller
    {
        private ModelClient client = new ModelClient(ConfigurationManager.AppSettings["EAVServiceUrl"]);

        [HttpGet]
        public ActionResult Index()
        {
            DataSelectionViewModel model = TempData["ValueData"] as DataSelectionViewModel ?? new Models.Data.DataSelectionViewModel();

            TempData["ValueData"] = model;

            return View(model);
        }

        [HttpPost]
        public ActionResult SubmitValues(DataSelectionViewModel postedModel)
        {
            DataSelectionViewModel model = TempData["ValueData"] as DataSelectionViewModel;

            TempData["ValueData"] = model;

            return View("Index", model);
        }
    }
}