﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

using EAVFramework.Model;


namespace EAVServiceClient
{
    public static class HttpClientExtensions
    {
        public static async Task<HttpResponseMessage> PatchAsync<T>(this HttpClient client, string requestUri, T value, MediaTypeFormatter formatter)
        {
            return (await client.SendAsync(new HttpRequestMessage(new HttpMethod("PATCH"), requestUri) { Content = new ObjectContent<T>(value, formatter) }));
        }

        public static async Task<HttpResponseMessage> PatchAsJsonAsync<T>(this HttpClient client, string requestUri, T value)
        {
            return (await PatchAsync<T>(client, requestUri, value, new JsonMediaTypeFormatter()));
        }

        public static async Task<HttpResponseMessage> PatchAsXmlAsync<T>(this HttpClient client, string requestUri, T value)
        {
            return (await PatchAsync<T>(client, requestUri, value, new XmlMediaTypeFormatter()));
        }
    }

    public class EAVClient
    {
        public EAVClient()
        {
        }

        #region Load
        private void LoadAttributes(HttpClient client, EAVContainer container)
        {
            HttpResponseMessage response = client.GetAsync(String.Format("api/meta/containers/{0}/attributes", container.ContainerID)).Result;
            if (response.IsSuccessStatusCode)
            {
                var attributes = response.Content.ReadAsAsync<IEnumerable<EAVAttribute>>().Result;

                foreach (EAVAttribute attribute in attributes)
                {
                    attribute.MarkCreated();

                    container.Attributes.Add(attribute);
                }
            }
            else
            {
                throw (new ApplicationException("Attempt to get attributes failed."));
            }
        }

        private void LoadChildContainers(HttpClient client, EAVContainer parentContainer)
        {
            HttpResponseMessage response = client.GetAsync(String.Format("api/meta/containers/{0}/containers", parentContainer.ContainerID)).Result;
            if (response.IsSuccessStatusCode)
            {
                var childContainers = response.Content.ReadAsAsync<IEnumerable<EAVChildContainer>>().Result;

                foreach (EAVChildContainer childContainer in childContainers)
                {
                    childContainer.MarkCreated();

                    LoadAttributes(client, childContainer);
                    LoadChildContainers(client, childContainer);

                    parentContainer.ChildContainers.Add(childContainer);
                }
            }
            else
            {
                throw (new ApplicationException("Attempt to get root containers failed."));
            }
        }

        private void LoadRootContainers(HttpClient client, EAVContext context)
        {
            HttpResponseMessage response = client.GetAsync(String.Format("api/meta/contexts/{0}/containers", context.ContextID)).Result;
            if (response.IsSuccessStatusCode)
            {
                var rootContainers = response.Content.ReadAsAsync<IEnumerable<EAVRootContainer>>().Result;

                foreach (EAVRootContainer rootContainer in rootContainers)
                {
                    rootContainer.MarkCreated();

                    LoadAttributes(client, rootContainer);
                    LoadChildContainers(client, rootContainer);

                    context.Containers.Add(rootContainer);
                }
            }
            else
            {
                throw (new ApplicationException("Attempt to get root containers failed."));
            }
        }

        private void LoadValues(HttpClient client, EAVInstance instance)
        {
            HttpResponseMessage response = client.GetAsync(String.Format("api/data/instances/{0}/values", instance.InstanceID)).Result;
            if (response.IsSuccessStatusCode)
            {
                var values = response.Content.ReadAsAsync<IEnumerable<EAVValue>>().Result;

                foreach (EAVValue value in values)
                {
                    value.MarkCreated();

                    instance.Values.Add(value);
                }
            }
            else
            {
                throw (new ApplicationException("Attempt to get values failed."));
            }
        }

        private void LoadChildInstances(HttpClient client, EAVInstance parentInstance)
        {
            HttpResponseMessage response = client.GetAsync(String.Format("api/data/instances/{0}/instances", parentInstance.InstanceID)).Result;
            if (response.IsSuccessStatusCode)
            {
                var childInstances = response.Content.ReadAsAsync<IEnumerable<EAVChildInstance>>().Result;

                foreach (EAVChildInstance childInstance in childInstances)
                {
                    childInstance.MarkCreated();

                    LoadValues(client, childInstance);
                    LoadChildInstances(client, childInstance);

                    parentInstance.ChildInstances.Add(childInstance);
                }
            }
            else
            {
                throw (new ApplicationException("Attempt to get child instances failed."));
            }
        }

        private void LoadRootInstances(HttpClient client, EAVSubject subject, EAVRootContainer rootContainer)
        {
            HttpResponseMessage response = client.GetAsync(String.Format("api/data/subjects/{0}/instances?container={1}", subject.SubjectID, rootContainer != null ? rootContainer.ContainerID : null)).Result;
            if (response.IsSuccessStatusCode)
            {
                var rootInstances = response.Content.ReadAsAsync<IEnumerable<EAVRootInstance>>().Result;

                foreach (EAVRootInstance rootInstance in rootInstances)
                {
                    rootInstance.MarkCreated();

                    LoadValues(client, rootInstance);
                    LoadChildInstances(client, rootInstance);

                    subject.Instances.Add(rootInstance);
                }
            }
            else
            {
                throw (new ApplicationException("Attempt to get root instances failed."));
            }
        }

        public IEnumerable<EAVContext> LoadContexts(HttpClient client)
        {
            HttpResponseMessage response = client.GetAsync("api/meta/contexts").Result;
            if (response.IsSuccessStatusCode)
            {
                return (response.Content.ReadAsAsync<IEnumerable<EAVContext>>().Result);
            }
            else
            {
                throw (new ApplicationException("Attempt to get contexts failed."));
            }
        }

        public void LoadMetadata(HttpClient client, EAVContext context)
        {
            try
            {
                LoadRootContainers(client, context);
            }
            catch (Exception ex)
            {
                throw (new ApplicationException("Attempt to get metadata failed.", ex));
            }
        }

        public void LoadSubjects(HttpClient client, EAVContext context)
        {
            HttpResponseMessage response = client.GetAsync(String.Format("api/data/contexts/{0}/subjects", context.ContextID)).Result;
            if (response.IsSuccessStatusCode)
            {
                var subjects = response.Content.ReadAsAsync<IEnumerable<EAVSubject>>().Result;

                foreach (EAVSubject subject in subjects)
                {
                    subject.MarkCreated();

                    context.Subjects.Add(subject);
                }
            }
            else
            {
                throw (new ApplicationException("Attempt to get subjects failed."));
            }
        }

        public void LoadData(HttpClient client, EAVSubject subject, EAVRootContainer rootContainer = null)
        {
            try
            {
                LoadRootInstances(client, subject, rootContainer);
            }
            catch (Exception ex)
            {
                throw (new ApplicationException("Attempt to load data failed.", ex));
            }
        }
        #endregion

        #region Save
        private void SaveAttribute(HttpClient client, EAVAttribute attribute)
        {
            HttpResponseMessage response;

            if (attribute.ObjectState == ObjectState.New)
            {
                response = client.PostAsJsonAsync<EAV.Model.IEAVAttribute>(String.Format("api/meta/containers/{0}/attributes", attribute.Container.ContainerID), attribute).Result;
                if (response.IsSuccessStatusCode)
                {
                    EAVAttribute newAttribute = response.Content.ReadAsAsync<EAVAttribute>().Result;

                    attribute.MarkCreated(newAttribute);
                }
                else
                {
                    throw (new ApplicationException("Attempt to create attribute failed."));
                }
            }
            else if (attribute.ObjectState == ObjectState.Modified)
            {
                response = client.PatchAsJsonAsync<EAV.Model.IEAVAttribute>("api/meta/attributes", attribute).Result;
                if (response.IsSuccessStatusCode)
                {
                    attribute.MarkCreated();
                }
                else
                {
                    throw (new ApplicationException("Attempt to update attribute failed."));
                }
            }

            if (attribute.ObjectState == ObjectState.Deleted)
            {
                response = client.DeleteAsync(String.Format("api/meta/attributes/{0}", attribute.AttributeID)).Result;
                if (response.IsSuccessStatusCode)
                {
                }
                else
                {
                    throw (new ApplicationException("Attempt to delete attribute failed."));
                }
            }
        }

        private void SaveChildContainer(HttpClient client, EAVChildContainer container)
        {
            HttpResponseMessage response;

            if (container.ObjectState == ObjectState.New)
            {
                response = client.PostAsJsonAsync<EAV.Model.IEAVContainer>(String.Format("api/meta/containers/{0}/containers", container.ParentContainer.ContainerID), container).Result;
                if (response.IsSuccessStatusCode)
                {
                    EAVContainer newContainer = response.Content.ReadAsAsync<EAVChildContainer>().Result;

                    container.MarkCreated(newContainer);
                }
                else
                {
                    throw (new ApplicationException("Attempt to create child container failed."));
                }
            }
            else if (container.ObjectState == ObjectState.Modified)
            {
                response = client.PatchAsJsonAsync<EAV.Model.IEAVContainer>("api/meta/containers", container).Result;
                if (response.IsSuccessStatusCode)
                {
                    container.MarkCreated();
                }
                else
                {
                    throw (new ApplicationException("Attempt to update child container failed."));
                }
            }

            foreach (EAVAttribute attribute in container.Attributes)
            {
                SaveAttribute(client, attribute);
            }

            foreach (EAVChildContainer childContainer in container.ChildContainers)
            {
                SaveChildContainer(client, childContainer);
            }

            if (container.ObjectState == ObjectState.Deleted)
            {
                response = client.DeleteAsync(String.Format("api/meta/containers/{0}", container.ContainerID)).Result;
                if (response.IsSuccessStatusCode)
                {
                }
                else
                {
                    throw (new ApplicationException("Attempt to delete child container failed."));
                }
            }
        }

        private void SaveRootContainer(HttpClient client, EAVRootContainer container)
        {
            HttpResponseMessage response;

            if (container.ObjectState == ObjectState.New)
            {
                response = client.PostAsJsonAsync<EAV.Model.IEAVContainer>(String.Format("api/meta/contexts/{0}/containers", container.Context.ContextID), container).Result;
                if (response.IsSuccessStatusCode)
                {
                    EAVContainer newContainer = response.Content.ReadAsAsync<EAVRootContainer>().Result;

                    container.MarkCreated(newContainer);
                }
                else
                {
                    throw (new ApplicationException("Attempt to create root container failed."));
                }
            }
            else if (container.ObjectState == ObjectState.Modified)
            {
                response = client.PatchAsJsonAsync<EAV.Model.IEAVContainer>("api/meta/containers", container).Result;
                if (response.IsSuccessStatusCode)
                {
                    container.MarkCreated();
                }
                else
                {
                    throw (new ApplicationException("Attempt to update root container failed."));
                }
            }

            foreach (EAVAttribute attribute in container.Attributes)
            {
                SaveAttribute(client, attribute);
            }

            foreach (EAVChildContainer childContainer in container.ChildContainers)
            {
                SaveChildContainer(client, childContainer);
            }

            if (container.ObjectState == ObjectState.Deleted)
            {
                response = client.DeleteAsync(String.Format("api/meta/containers/{0}", container.ContainerID)).Result;
                if (response.IsSuccessStatusCode)
                {
                }
                else
                {
                    throw (new ApplicationException("Attempt to delete root container failed."));
                }
            }
        }

        public void SaveMetadata(HttpClient client, EAVContext context)
        {
            HttpResponseMessage response;

            if (context.ObjectState == ObjectState.New)
            {
                response = client.PostAsJsonAsync<EAV.Model.IEAVContext>("api/meta/contexts", context).Result;
                if (response.IsSuccessStatusCode)
                {
                    EAVContext newContext = response.Content.ReadAsAsync<EAVContext>().Result;

                    context.MarkCreated(newContext);
                }
                else
                {
                    throw (new ApplicationException("Attempt to create context failed."));
                }
            }
            else if (context.ObjectState == ObjectState.Modified)
            {
                response = client.PatchAsJsonAsync<EAV.Model.IEAVContext>("api/meta/contexts", context).Result;
                if (response.IsSuccessStatusCode)
                {
                    context.MarkCreated();
                }
                else
                {
                    throw (new ApplicationException("Attempt to update context failed."));
                }
            }

            foreach (EAVRootContainer rootContainer in context.Containers)
            {
                SaveRootContainer(client, rootContainer);
            }

            if (context.ObjectState == ObjectState.Deleted)
            {
                response = client.DeleteAsync(String.Format("api/meta/contexts/{0}", context.ContextID)).Result;
                if (response.IsSuccessStatusCode)
                {
                }
                else
                {
                    throw (new ApplicationException("Attempt to delete context failed."));
                }
            }
        }

        public void SaveData(HttpClient client, EAVSubject subject, EAVRootContainer container = null)
        {
        }
        #endregion
    }
}

