using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;


namespace EAVClient
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
        private void LoadAttributes(HttpClient client, Framework.EAVContainer container)
        {
            HttpResponseMessage response = client.GetAsync(String.Format("api/meta/containers/{0}/attributes", container.ContainerID)).Result;
            if (response.IsSuccessStatusCode)
            {
                var attributes = response.Content.ReadAsAsync<IEnumerable<Framework.EAVAttribute>>().Result;

                foreach (Framework.EAVAttribute attribute in attributes)
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

        private void LoadChildContainers(HttpClient client, Framework.EAVContainer parentContainer)
        {
            HttpResponseMessage response = client.GetAsync(String.Format("api/meta/containers/{0}/containers", parentContainer.ContainerID)).Result;
            if (response.IsSuccessStatusCode)
            {
                var childContainers = response.Content.ReadAsAsync<IEnumerable<Framework.EAVChildContainer>>().Result;

                foreach (Framework.EAVChildContainer childContainer in childContainers)
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

        private void LoadRootContainers(HttpClient client, Framework.EAVContext context)
        {
            HttpResponseMessage response = client.GetAsync(String.Format("api/meta/contexts/{0}/containers", context.ContextID)).Result;
            if (response.IsSuccessStatusCode)
            {
                var rootContainers = response.Content.ReadAsAsync<IEnumerable<Framework.EAVRootContainer>>().Result;

                foreach (Framework.EAVRootContainer rootContainer in rootContainers)
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

        private void LoadValues(HttpClient client, Framework.EAVInstance instance)
        {
            HttpResponseMessage response = client.GetAsync(String.Format("api/data/instances/{0}/values", instance.InstanceID)).Result;
            if (response.IsSuccessStatusCode)
            {
                var values = response.Content.ReadAsAsync<IEnumerable<Framework.EAVValue>>().Result;

                foreach (Framework.EAVValue value in values)
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

        private void LoadChildInstances(HttpClient client, Framework.EAVInstance parentInstance)
        {
            HttpResponseMessage response = client.GetAsync(String.Format("api/data/instances/{0}/instances", parentInstance.InstanceID)).Result;
            if (response.IsSuccessStatusCode)
            {
                var childInstances = response.Content.ReadAsAsync<IEnumerable<Framework.EAVChildInstance>>().Result;

                foreach (Framework.EAVChildInstance childInstance in childInstances)
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

        private void LoadRootInstances(HttpClient client, Framework.EAVSubject subject, Framework.EAVRootContainer rootContainer)
        {
            HttpResponseMessage response = client.GetAsync(String.Format("api/data/subjects/{0}/instances?container={1}", subject.SubjectID, rootContainer != null ? rootContainer.ContainerID : null)).Result;
            if (response.IsSuccessStatusCode)
            {
                var rootInstances = response.Content.ReadAsAsync<IEnumerable<Framework.EAVRootInstance>>().Result;

                foreach (Framework.EAVRootInstance rootInstance in rootInstances)
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

        public IEnumerable<Framework.EAVContext> LoadContexts(HttpClient client)
        {
            HttpResponseMessage response = client.GetAsync("api/meta/contexts").Result;
            if (response.IsSuccessStatusCode)
            {
                return (response.Content.ReadAsAsync<IEnumerable<Framework.EAVContext>>().Result);
            }
            else
            {
                throw (new ApplicationException("Attempt to get contexts failed."));
            }
        }

        public void LoadMetadata(HttpClient client, Framework.EAVContext context)
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

        public void LoadSubjects(HttpClient client, Framework.EAVContext context)
        {
            HttpResponseMessage response = client.GetAsync(String.Format("api/data/contexts/{0}/subjects", context.ContextID)).Result;
            if (response.IsSuccessStatusCode)
            {
                var subjects = response.Content.ReadAsAsync<IEnumerable<Framework.EAVSubject>>().Result;

                foreach (Framework.EAVSubject subject in subjects)
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

        public void LoadData(HttpClient client, Framework.EAVSubject subject, Framework.EAVRootContainer rootContainer = null)
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
        private void SaveAttribute(HttpClient client, Framework.EAVAttribute attribute)
        {
            HttpResponseMessage response;

            if (attribute.ObjectState == Framework.ObjectState.New)
            {
                response = client.PostAsJsonAsync<EAV.Model.IEAVAttribute>(String.Format("api/meta/containers/{0}/attributes", attribute.Container.ContainerID), attribute).Result;
                if (response.IsSuccessStatusCode)
                {
                    Framework.EAVAttribute newAttribute = response.Content.ReadAsAsync<Framework.EAVAttribute>().Result;

                    attribute.MarkCreated(newAttribute);
                }
                else
                {
                    throw (new ApplicationException("Attempt to create attribute failed."));
                }
            }
            else if (attribute.ObjectState == Framework.ObjectState.Modified)
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

            if (attribute.ObjectState == Framework.ObjectState.Deleted)
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

        private void SaveChildContainer(HttpClient client, Framework.EAVChildContainer container)
        {
            HttpResponseMessage response;

            if (container.ObjectState == Framework.ObjectState.New)
            {
                response = client.PostAsJsonAsync<EAV.Model.IEAVContainer>(String.Format("api/meta/containers/{0}/containers", container.ParentContainer.ContainerID), container).Result;
                if (response.IsSuccessStatusCode)
                {
                    Framework.EAVContainer newContainer = response.Content.ReadAsAsync<Framework.EAVChildContainer>().Result;

                    container.MarkCreated(newContainer);
                }
                else
                {
                    throw (new ApplicationException("Attempt to create child container failed."));
                }
            }
            else if (container.ObjectState == Framework.ObjectState.Modified)
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

            foreach (Framework.EAVAttribute attribute in container.Attributes)
            {
                SaveAttribute(client, attribute);
            }

            foreach (Framework.EAVChildContainer childContainer in container.ChildContainers)
            {
                SaveChildContainer(client, childContainer);
            }

            if (container.ObjectState == Framework.ObjectState.Deleted)
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

        private void SaveRootContainer(HttpClient client, Framework.EAVRootContainer container)
        {
            HttpResponseMessage response;

            if (container.ObjectState == Framework.ObjectState.New)
            {
                response = client.PostAsJsonAsync<EAV.Model.IEAVContainer>(String.Format("api/meta/contexts/{0}/containers", container.Context.ContextID), container).Result;
                if (response.IsSuccessStatusCode)
                {
                    Framework.EAVContainer newContainer = response.Content.ReadAsAsync<Framework.EAVRootContainer>().Result;

                    container.MarkCreated(newContainer);
                }
                else
                {
                    throw (new ApplicationException("Attempt to create root container failed."));
                }
            }
            else if (container.ObjectState == Framework.ObjectState.Modified)
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

            foreach (Framework.EAVAttribute attribute in container.Attributes)
            {
                SaveAttribute(client, attribute);
            }

            foreach (Framework.EAVChildContainer childContainer in container.ChildContainers)
            {
                SaveChildContainer(client, childContainer);
            }

            if (container.ObjectState == Framework.ObjectState.Deleted)
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

        public void SaveMetadata(HttpClient client, Framework.EAVContext context)
        {
            HttpResponseMessage response;

            if (context.ObjectState == Framework.ObjectState.New)
            {
                response = client.PostAsJsonAsync<EAV.Model.IEAVContext>("api/meta/contexts", context).Result;
                if (response.IsSuccessStatusCode)
                {
                    Framework.EAVContext newContext = response.Content.ReadAsAsync<Framework.EAVContext>().Result;

                    context.MarkCreated(newContext);
                }
                else
                {
                    throw (new ApplicationException("Attempt to create context failed."));
                }
            }
            else if (context.ObjectState == Framework.ObjectState.Modified)
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

            foreach (Framework.EAVRootContainer rootContainer in context.Containers)
            {
                SaveRootContainer(client, rootContainer);
            }

            if (context.ObjectState == Framework.ObjectState.Deleted)
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

        public void SaveData(HttpClient client, Framework.EAVSubject subject, Framework.EAVRootContainer container = null)
        {
        }
        #endregion
    }
}

