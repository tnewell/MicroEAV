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
            DataViewModel model = TempData["ValueData"] as DataViewModel ?? new Models.Data.DataViewModel();

            TempData["ValueData"] = model;

            return View(model);
        }

        [HttpPost]
        public ActionResult SubmitValues(DataViewModel postedModel)
        {
            DataViewModel model = TempData["ValueData"] as DataViewModel;

            TempData["ValueData"] = model;

            return View("Index", model);
        }
    }
}