﻿using System;
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

            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new IStoreContextJsonConverter());
            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new IStoreContainerJsonConverter());
            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new IStoreAttributeJsonConverter());
            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new IStoreUnitJsonConverter());
            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new IStoreEntityJsonConverter());
            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new IStoreSubjectJsonConverter());
            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new IStoreInstanceJsonConverter());
            config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new IStoreValueJsonConverter());

            //config.Formatters.XmlFormatter.SetSerializer<EAV.Model.StoreContext>(new EAVContextXmlSerializer(typeof(StoreContext)));

            // Web API routes
            config.MapHttpAttributeRoutes();
        }
    }

    public class IStoreContextJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(IStoreContext));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (objectType == typeof(IStoreContext))
                return (serializer.Deserialize<StoreContext>(reader));
            else
                return (null);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is IStoreContext)
                serializer.Serialize(writer, new StoreContext((IStoreContext)value), typeof(IStoreContext));
            else
                serializer.Serialize(writer, value);
        }
    }

    public class IStoreContainerJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(IStoreContainer));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (objectType == typeof(IStoreContainer))
                return (serializer.Deserialize<StoreContainer>(reader));
            else
                return (null);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is IStoreContainer)
                serializer.Serialize(writer, new StoreContainer((IStoreContainer)value), typeof(IStoreContainer));
            else
                serializer.Serialize(writer, value);
        }
    }

    public class IStoreAttributeJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(IStoreAttribute));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (objectType == typeof(IStoreAttribute))
                return (serializer.Deserialize<StoreAttribute>(reader));
            else
                return (null);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is IStoreAttribute)
                serializer.Serialize(writer, new StoreAttribute((IStoreAttribute)value), typeof(IStoreAttribute));
            else
                serializer.Serialize(writer, value);
        }
    }

    public class IStoreUnitJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(IStoreUnit));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (objectType == typeof(IStoreUnit))
                return (serializer.Deserialize<StoreUnit>(reader));
            else
                return (null);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is IStoreUnit)
                serializer.Serialize(writer, new StoreUnit((IStoreUnit)value), typeof(IStoreUnit));
            else
                serializer.Serialize(writer, value);
        }
    }

    public class IStoreEntityJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(IStoreEntity));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (objectType == typeof(IStoreEntity))
                return (serializer.Deserialize<StoreEntity>(reader));
            else
                return (null);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is IStoreEntity)
                serializer.Serialize(writer, new StoreEntity((IStoreEntity)value), typeof(IStoreEntity));
            else
                serializer.Serialize(writer, value);
        }
    }

    public class IStoreSubjectJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(IStoreSubject));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (objectType == typeof(IStoreSubject))
                return (serializer.Deserialize<StoreSubject>(reader));
            else
                return (null);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is IStoreSubject)
                serializer.Serialize(writer, new StoreSubject((IStoreSubject)value), typeof(IStoreSubject));
            else
                serializer.Serialize(writer, value);
        }
    }

    public class IStoreInstanceJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(IStoreInstance));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (objectType == typeof(IStoreInstance))
                return (serializer.Deserialize<StoreInstance>(reader));
            else
                return (null);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is IStoreInstance)
                serializer.Serialize(writer, new StoreInstance((IStoreInstance)value), typeof(IStoreInstance));
            else
                serializer.Serialize(writer, value);
        }
    }

    public class IStoreValueJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(IStoreValue));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (objectType == typeof(IStoreValue))
                return (serializer.Deserialize<StoreValue>(reader));
            else
                return (null);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is IStoreValue)
                serializer.Serialize(writer, new StoreValue((IStoreValue)value), typeof(IStoreValue));
            else
                serializer.Serialize(writer, value);
        }
    }
}
