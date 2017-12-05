using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;


namespace EAVWebClient
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

    public class EAVWebClient
    {
        private HttpClient client;

        public EAVWebClient()
        {
            client = new HttpClient();
        }

        public EAVWebClient(string baseAddress)
        {
            client = new HttpClient() { BaseAddress = new Uri(baseAddress) };
        }

        #region Load
        private void LoadAttributes(Model.EAVContainer container)
        {
            HttpResponseMessage response = client.GetAsync(String.Format("api/meta/containers/{0}/attributes", container.ContainerID)).Result;
            if (response.IsSuccessStatusCode)
            {
                var attributes = response.Content.ReadAsAsync<IEnumerable<Model.EAVAttribute>>().Result;

                foreach (Model.EAVAttribute attribute in attributes)
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

        private void LoadChildContainers(Model.EAVContainer parentContainer)
        {
            HttpResponseMessage response = client.GetAsync(String.Format("api/meta/containers/{0}/containers", parentContainer.ContainerID)).Result;
            if (response.IsSuccessStatusCode)
            {
                var childContainers = response.Content.ReadAsAsync<IEnumerable<Model.EAVChildContainer>>().Result;

                foreach (Model.EAVChildContainer childContainer in childContainers)
                {
                    childContainer.MarkCreated();

                    LoadAttributes(childContainer);
                    LoadChildContainers(childContainer);

                    parentContainer.ChildContainers.Add(childContainer);
                }
            }
            else
            {
                throw (new ApplicationException("Attempt to get root containers failed."));
            }
        }

        private void LoadRootContainers(Model.EAVContext context)
        {
            HttpResponseMessage response = client.GetAsync(String.Format("api/meta/contexts/{0}/containers", context.ContextID)).Result;
            if (response.IsSuccessStatusCode)
            {
                var rootContainers = response.Content.ReadAsAsync<IEnumerable<Model.EAVRootContainer>>().Result;

                foreach (Model.EAVRootContainer rootContainer in rootContainers)
                {
                    rootContainer.MarkCreated();

                    LoadAttributes(rootContainer);
                    LoadChildContainers(rootContainer);

                    context.Containers.Add(rootContainer);
                }
            }
            else
            {
                throw (new ApplicationException("Attempt to get root containers failed."));
            }
        }

        private void LoadValues(Model.EAVInstance instance)
        {
            HttpResponseMessage response = client.GetAsync(String.Format("api/data/instances/{0}/values", instance.InstanceID)).Result;
            if (response.IsSuccessStatusCode)
            {
                var values = response.Content.ReadAsAsync<IEnumerable<Model.EAVValue>>().Result;

                foreach (Model.EAVValue value in values)
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

        private void LoadChildInstances(Model.EAVInstance parentInstance)
        {
            HttpResponseMessage response = client.GetAsync(String.Format("api/data/instances/{0}/instances", parentInstance.InstanceID)).Result;
            if (response.IsSuccessStatusCode)
            {
                var childInstances = response.Content.ReadAsAsync<IEnumerable<Model.EAVChildInstance>>().Result;

                foreach (Model.EAVChildInstance childInstance in childInstances)
                {
                    childInstance.MarkCreated();

                    LoadValues(childInstance);
                    LoadChildInstances(childInstance);

                    parentInstance.ChildInstances.Add(childInstance);
                }
            }
            else
            {
                throw (new ApplicationException("Attempt to get child instances failed."));
            }
        }

        private void LoadRootInstances(Model.EAVSubject subject, Model.EAVRootContainer rootContainer)
        {
            HttpResponseMessage response = client.GetAsync(String.Format("api/data/subjects/{0}/instances?container={1}", subject.SubjectID, rootContainer != null ? rootContainer.ContainerID : null)).Result;
            if (response.IsSuccessStatusCode)
            {
                var rootInstances = response.Content.ReadAsAsync<IEnumerable<Model.EAVRootInstance>>().Result;

                foreach (Model.EAVRootInstance rootInstance in rootInstances)
                {
                    rootInstance.MarkCreated();

                    LoadValues(rootInstance);
                    LoadChildInstances(rootInstance);

                    subject.Instances.Add(rootInstance);
                }
            }
            else
            {
                throw (new ApplicationException("Attempt to get root instances failed."));
            }
        }

        public IEnumerable<Model.EAVContext> LoadContexts()
        {
            HttpResponseMessage response = client.GetAsync("api/meta/contexts").Result;
            if (response.IsSuccessStatusCode)
            {
                return (response.Content.ReadAsAsync<IEnumerable<Model.EAVContext>>().Result);
            }
            else
            {
                throw (new ApplicationException("Attempt to get contexts failed."));
            }
        }

        public void LoadMetadata(Model.EAVContext context)
        {
            try
            {
                LoadRootContainers(context);
            }
            catch (Exception ex)
            {
                throw (new ApplicationException("Attempt to get metadata failed.", ex));
            }
        }

        public void LoadSubjects(Model.EAVContext context)
        {
            HttpResponseMessage response = client.GetAsync(String.Format("api/data/contexts/{0}/subjects", context.ContextID)).Result;
            if (response.IsSuccessStatusCode)
            {
                var subjects = response.Content.ReadAsAsync<IEnumerable<Model.EAVSubject>>().Result;

                foreach (Model.EAVSubject subject in subjects)
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

        public void LoadData(Model.EAVSubject subject, Model.EAVRootContainer rootContainer = null)
        {
            try
            {
                LoadRootInstances(subject, rootContainer);
            }
            catch (Exception ex)
            {
                throw (new ApplicationException("Attempt to load data failed.", ex));
            }
        }
        #endregion

        #region Save
        private void SaveAttribute(Model.EAVAttribute attribute)
        {
            HttpResponseMessage response;

            if (attribute.ObjectState == Model.ObjectState.New)
            {
                response = client.PostAsJsonAsync<EAV.Model.IEAVAttribute>(String.Format("api/meta/containers/{0}/attributes", attribute.Container.ContainerID), attribute).Result;
                if (response.IsSuccessStatusCode)
                {
                    Model.EAVAttribute newAttribute = response.Content.ReadAsAsync<Model.EAVAttribute>().Result;

                    attribute.MarkCreated(newAttribute);
                }
                else
                {
                    throw (new ApplicationException("Attempt to create attribute failed."));
                }
            }
            else if (attribute.ObjectState == Model.ObjectState.Modified)
            {
                response = client.PatchAsJsonAsync<EAV.Model.IEAVAttribute>("api/meta/attributes", attribute).Result;
                if (response.IsSuccessStatusCode)
                {
                }
                else
                {
                    throw (new ApplicationException("Attempt to update attribute failed."));
                }
            }

            if (attribute.ObjectState == Model.ObjectState.Deleted)
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

        private void SaveChildContainer(Model.EAVChildContainer container)
        {
            HttpResponseMessage response;

            if (container.ObjectState == Model.ObjectState.New)
            {
                response = client.PostAsJsonAsync<EAV.Model.IEAVContainer>(String.Format("api/meta/containers/{0}/containers", container.ParentContainer.ContainerID), container).Result;
                if (response.IsSuccessStatusCode)
                {
                    Model.EAVContainer newContainer = response.Content.ReadAsAsync<Model.EAVChildContainer>().Result;

                    container.MarkCreated(newContainer);
                }
                else
                {
                    throw (new ApplicationException("Attempt to create child container failed."));
                }
            }
            else if (container.ObjectState == Model.ObjectState.Modified)
            {
                response = client.PatchAsJsonAsync<EAV.Model.IEAVContainer>("api/meta/containers", container).Result;
                if (response.IsSuccessStatusCode)
                {
                }
                else
                {
                    throw (new ApplicationException("Attempt to update child container failed."));
                }
            }

            foreach (Model.EAVAttribute attribute in container.Attributes)
            {
                SaveAttribute(attribute);
            }

            foreach (Model.EAVChildContainer childContainer in container.ChildContainers)
            {
                SaveChildContainer(childContainer);
            }

            if (container.ObjectState == Model.ObjectState.Deleted)
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

        private void SaveRootContainer(Model.EAVRootContainer container)
        {
            HttpResponseMessage response;

            if (container.ObjectState == Model.ObjectState.New)
            {
                response = client.PostAsJsonAsync<EAV.Model.IEAVContainer>(String.Format("api/meta/contexts/{0}/containers", container.Context.ContextID), container).Result;
                if (response.IsSuccessStatusCode)
                {
                    Model.EAVContainer newContainer = response.Content.ReadAsAsync<Model.EAVRootContainer>().Result;

                    container.MarkCreated(newContainer);
                }
                else
                {
                    throw (new ApplicationException("Attempt to create root container failed."));
                }
            }
            else if (container.ObjectState == Model.ObjectState.Modified)
            {
                response = client.PatchAsJsonAsync<EAV.Model.IEAVContainer>("api/meta/containers", container).Result;
                if (response.IsSuccessStatusCode)
                {
                }
                else
                {
                    throw (new ApplicationException("Attempt to update root container failed."));
                }
            }

            foreach (Model.EAVAttribute attribute in container.Attributes)
            {
                SaveAttribute(attribute);
            }

            foreach (Model.EAVChildContainer childContainer in container.ChildContainers)
            {
                SaveChildContainer(childContainer);
            }

            if (container.ObjectState == Model.ObjectState.Deleted)
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

        public void SaveMetadata(Model.EAVContext context)
        {
            HttpResponseMessage response;

            if (context.ObjectState == Model.ObjectState.New)
            {
                response = client.PostAsJsonAsync<EAV.Model.IEAVContext>("api/meta/contexts", context).Result;
                if (response.IsSuccessStatusCode)
                {
                    Model.EAVContext newContext = response.Content.ReadAsAsync<Model.EAVContext>().Result;

                    context.MarkCreated(newContext);
                }
                else
                {
                    throw (new ApplicationException("Attempt to create context failed."));
                }
            }
            else if (context.ObjectState == Model.ObjectState.Modified)
            {
                response = client.PatchAsJsonAsync<EAV.Model.IEAVContext>("api/meta/contexts", context).Result;
                if (response.IsSuccessStatusCode)
                {
                }
                else
                {
                    throw (new ApplicationException("Attempt to update context failed."));
                }
            }

            foreach (Model.EAVRootContainer rootContainer in context.Containers)
            {
                SaveRootContainer(rootContainer);
            }

            if (context.ObjectState == Model.ObjectState.Deleted)
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

        public void SaveData(Model.EAVSubject subject, Model.EAVRootContainer container = null)
        {
        }
        #endregion
    }
}

