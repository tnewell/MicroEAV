using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web.Http;
using System.Xml;
using System.Xml.Serialization;
using EAV.Store;
using EAVStoreLibrary;
using Newtonsoft.Json;

namespace EAVWebService
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
            config.Formatters.JsonFormatter.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;

            //config.Formatters.XmlFormatter.SetSerializer<EAV.Model.StoreContext>(new EAVContextXmlSerializer(typeof(StoreContext)));

            // Web API routes
            config.MapHttpAttributeRoutes();
        }
    }
}
