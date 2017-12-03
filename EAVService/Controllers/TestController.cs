using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml.Linq;

namespace EAVService.Controllers
{
    public class Registration
    {
        public object ODM { get; set; }
        public object ClinicalData { get; set; }
    }

    [RoutePrefix("api/registrations")]
    public class TestController : ApiController
    {
        [HttpGet]
        [Route("", Name = "RetrieveRegistrations")]
        public IHttpActionResult RetrieveRegistration()
        {
            return (Ok("PING!"));
        }

        [HttpPost]
        [Route("", Name = "CreateRegistration")]
        public IHttpActionResult CreateRegistration([FromBody] XElement data)
        {
            return (Ok("PING!"));
        }
    }
}
