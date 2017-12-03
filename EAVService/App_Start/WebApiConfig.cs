using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web.Http;
using System.Xml;
using System.Xml.Serialization;
using EAV.Model;
using Newtonsoft.Json;

namespace EAVService
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Serialize;
            config.Formatters.JsonFormatter.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;

            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new IEAVContextJsonConverter());
            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new IEAVContainerJsonConverter());
            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new IEAVAttributeJsonConverter());
            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new IEAVEntityJsonConverter());
            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new IEAVSubjectJsonConverter());
            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new IEAVInstanceJsonConverter());
            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new IEAVValueJsonConverter());

            //config.Formatters.XmlFormatter.SetSerializer<EAV.Model.IEAVContext>(new EAVContextXmlSerializer(typeof(BaseEAVContext)));

            // Web API routes
            config.MapHttpAttributeRoutes();

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);
        }
    }

    public class IEAVContextJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(IEAVContext));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (objectType == typeof(IEAVContext))
                return (serializer.Deserialize<BaseEAVContext>(reader));
            else
                return (null);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is IEAVContext)
                serializer.Serialize(writer, new BaseEAVContext((IEAVContext)value), typeof(IEAVContext));
            else
                serializer.Serialize(writer, value);
        }
    }

    public class IEAVContainerJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(IEAVContainer));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (objectType == typeof(IEAVContainer))
                return (serializer.Deserialize<BaseEAVContainer>(reader));
            else
                return (null);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is IEAVContainer)
                serializer.Serialize(writer, new BaseEAVContainer((IEAVContainer)value), typeof(IEAVContainer));
            else
                serializer.Serialize(writer, value);
        }
    }

    public class IEAVAttributeJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(IEAVAttribute));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (objectType == typeof(IEAVAttribute))
                return (serializer.Deserialize<BaseEAVAttribute>(reader));
            else
                return (null);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is IEAVAttribute)
                serializer.Serialize(writer, new BaseEAVAttribute((IEAVAttribute)value), typeof(IEAVAttribute));
            else
                serializer.Serialize(writer, value);
        }
    }

    public class IEAVEntityJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(IEAVEntity));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (objectType == typeof(IEAVEntity))
                return (serializer.Deserialize<BaseEAVEntity>(reader));
            else
                return (null);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is IEAVEntity)
                serializer.Serialize(writer, new BaseEAVEntity((IEAVEntity)value), typeof(IEAVEntity));
            else
                serializer.Serialize(writer, value);
        }
    }

    public class IEAVSubjectJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(IEAVSubject));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (objectType == typeof(IEAVSubject))
                return (serializer.Deserialize<BaseEAVSubject>(reader));
            else
                return (null);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is IEAVSubject)
                serializer.Serialize(writer, new BaseEAVSubject((IEAVSubject) value), typeof(IEAVSubject));
            else
                serializer.Serialize(writer, value);
        }
    }

    public class IEAVInstanceJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(IEAVInstance));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (objectType == typeof(IEAVInstance))
                return (serializer.Deserialize<BaseEAVInstance>(reader));
            else
                return (null);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is IEAVInstance)
                serializer.Serialize(writer, new BaseEAVInstance((IEAVInstance)value), typeof(IEAVInstance));
            else
                serializer.Serialize(writer, value);
        }
    }

    public class IEAVValueJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(IEAVValue));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (objectType == typeof(IEAVValue))
                return (serializer.Deserialize<BaseEAVValue>(reader));
            else
                return (null);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is IEAVValue)
                serializer.Serialize(writer, new BaseEAVValue((IEAVValue)value), typeof(IEAVValue));
            else
                serializer.Serialize(writer, value);
        }
    }
}
