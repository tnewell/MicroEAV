using System;
using System.Collections.Generic;
using System.Linq;
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
                    attribute.MarkUnmodified();

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
                    childContainer.MarkUnmodified();

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
                    rootContainer.MarkUnmodified();

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

        private void LoadValues(HttpClient client, EAVInstance instance)
        {
            HttpResponseMessage response = client.GetAsync(String.Format("api/data/instances/{0}/values", instance.InstanceID)).Result;
            if (response.IsSuccessStatusCode)
            {
                var values = response.Content.ReadAsAsync<IEnumerable<EAVValue>>().Result;

                foreach (EAVValue value in values)
                {
                    value.MarkUnmodified();

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
                    childInstance.MarkUnmodified();

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
                    rootInstance.MarkUnmodified();

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

        private void LoadEntity(HttpClient client, EAVSubject subject)
        {
            HttpResponseMessage response = client.GetAsync(String.Format("api/data/entities/{0}", subject.EntityID)).Result;
            if (response.IsSuccessStatusCode)
            {
                var entity = response.Content.ReadAsAsync<EAVEntity>().Result;

                entity.MarkUnmodified();

                subject.Entity = entity;
            }
            else
            {
                throw (new ApplicationException("Attempt to get entity failed."));
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

        public void LoadSubjects(HttpClient client, EAVContext context)
        {
            HttpResponseMessage response = client.GetAsync(String.Format("api/data/contexts/{0}/subjects", context.ContextID)).Result;
            if (response.IsSuccessStatusCode)
            {
                var subjects = response.Content.ReadAsAsync<IEnumerable<EAVSubject>>().Result;

                foreach (EAVSubject subject in subjects)
                {
                    LoadEntity(client, subject);

                    subject.MarkUnmodified();

                    context.Subjects.Add(subject);
                }
            }
            else
            {
                throw (new ApplicationException("Attempt to get subjects failed."));
            }
        }

        public IEnumerable<EAVEntity> LoadEntities(HttpClient client)
        {
            HttpResponseMessage response = client.GetAsync(String.Format("api/data/entities")).Result;
            if (response.IsSuccessStatusCode)
            {
                return(response.Content.ReadAsAsync<IEnumerable<EAVEntity>>().Result);
            }
            else
            {
                throw (new ApplicationException("Attempt to get subjects failed."));
            }
        }
        #endregion

        #region Save
        private void SaveAttribute(HttpClient client, EAVAttribute attribute)
        {
            HttpResponseMessage response;

            if (attribute.ObjectState == ObjectState.New)
            {
                response = client.PostAsJsonAsync<EAV.Model.IEAVAttribute>(String.Format("api/meta/containers/{0}/attributes", attribute.ContainerID), attribute).Result;
                if (response.IsSuccessStatusCode)
                {
                    EAVAttribute newAttribute = response.Content.ReadAsAsync<EAVAttribute>().Result;

                    attribute.AttributeID = newAttribute.AttributeID;
                    attribute.MarkUnmodified();
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
                    attribute.MarkUnmodified();
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
                response = client.PostAsJsonAsync<EAV.Model.IEAVContainer>(String.Format("api/meta/containers/{0}/containers", container.ParentContainerID), container).Result;
                if (response.IsSuccessStatusCode)
                {
                    EAVContainer newContainer = response.Content.ReadAsAsync<EAVChildContainer>().Result;

                    container.ContainerID = newContainer.ContainerID;
                    container.MarkUnmodified();
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
                    container.MarkUnmodified();
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
                response = client.PostAsJsonAsync<EAV.Model.IEAVContainer>(String.Format("api/meta/contexts/{0}/containers", container.ContextID), container).Result;
                if (response.IsSuccessStatusCode)
                {
                    EAVContainer newContainer = response.Content.ReadAsAsync<EAVRootContainer>().Result;

                    container.ContainerID = newContainer.ContainerID;
                    container.MarkUnmodified();
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
                    container.MarkUnmodified();
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

                    context.ContextID = newContext.ContextID;
                    context.MarkUnmodified();
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
                    context.MarkUnmodified();
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

        private void SaveValue(HttpClient client, EAVValue value)
        {
            HttpResponseMessage response;

            if (value.ObjectState == ObjectState.New)
            {
                response = client.PostAsJsonAsync<EAV.Model.IEAVValue>(String.Format("api/data/instances/{0}/values?attribute={1}", value.InstanceID, value.AttributeID), value).Result;
                if (response.IsSuccessStatusCode)
                {
                    EAVValue newValue = response.Content.ReadAsAsync<EAVValue>().Result;

                    value.MarkUnmodified();
                }
                else
                {
                    throw (new ApplicationException("Attempt to create value failed."));
                }
            }
            else if (value.ObjectState == ObjectState.Modified)
            {
                response = client.PatchAsJsonAsync<EAV.Model.IEAVValue>("api/data/values", value).Result;
                if (response.IsSuccessStatusCode)
                {
                    value.MarkUnmodified();
                }
                else
                {
                    throw (new ApplicationException("Attempt to update value failed."));
                }
            }

            if (value.ObjectState == ObjectState.Deleted)
            {
                response = client.DeleteAsync(String.Format("api/data/instances/{0}/values/{1}", value.InstanceID, value.AttributeID)).Result;
                if (response.IsSuccessStatusCode)
                {
                }
                else
                {
                    throw (new ApplicationException("Attempt to delete value failed."));
                }
            }
        }

        private void SaveChildInstance(HttpClient client, EAVChildInstance instance)
        {
            HttpResponseMessage response;

            if (instance.ObjectState == ObjectState.New)
            {
                response = client.PostAsJsonAsync<EAV.Model.IEAVInstance>(String.Format("api/data/instances/{0}/instances?container={1}", instance.ParentInstanceID, instance.ContainerID), instance).Result;
                if (response.IsSuccessStatusCode)
                {
                    EAVInstance newInstance = response.Content.ReadAsAsync<EAVChildInstance>().Result;

                    instance.InstanceID = newInstance.InstanceID;
                    instance.MarkUnmodified();
                }
                else
                {
                    throw (new ApplicationException("Attempt to create child instance failed."));
                }
            }
            else if (instance.ObjectState == ObjectState.Modified)
            {
                response = client.PatchAsJsonAsync<EAV.Model.IEAVInstance>("api/data/instances", instance).Result;
                if (response.IsSuccessStatusCode)
                {
                    instance.MarkUnmodified();
                }
                else
                {
                    throw (new ApplicationException("Attempt to update child instance failed."));
                }
            }

            foreach (EAVValue value in instance.Values)
            {
                SaveValue(client, value);
            }

            foreach (EAVChildInstance childInstance in instance.ChildInstances)
            {
                SaveChildInstance(client, childInstance);
            }

            if (instance.ObjectState == ObjectState.Deleted)
            {
                response = client.DeleteAsync(String.Format("api/data/instances/{0}", instance.InstanceID)).Result;
                if (response.IsSuccessStatusCode)
                {
                }
                else
                {
                    throw (new ApplicationException("Attempt to delete child instance failed."));
                }
            }
        }

        private void SaveRootInstance(HttpClient client, EAVRootInstance instance)
        {
            HttpResponseMessage response;

            if (instance.ObjectState == ObjectState.New)
            {
                response = client.PostAsJsonAsync<EAV.Model.IEAVInstance>(String.Format("api/data/subjects/{0}/instances?container={1}", instance.SubjectID, instance.ContainerID), instance).Result;
                if (response.IsSuccessStatusCode)
                {
                    EAVInstance newInstance = response.Content.ReadAsAsync<EAVRootInstance>().Result;

                    instance.InstanceID = newInstance.InstanceID;
                    instance.MarkUnmodified();
                }
                else
                {
                    throw (new ApplicationException("Attempt to create root instance failed."));
                }
            }
            else if (instance.ObjectState == ObjectState.Modified)
            {
                response = client.PatchAsJsonAsync<EAV.Model.IEAVInstance>("api/data/instances", instance).Result;
                if (response.IsSuccessStatusCode)
                {
                    instance.MarkUnmodified();
                }
                else
                {
                    throw (new ApplicationException("Attempt to update root instance failed."));
                }
            }

            foreach (EAVValue value in instance.Values)
            {
                SaveValue(client, value);
            }

            foreach (EAVChildInstance childInstance in instance.ChildInstances)
            {
                SaveChildInstance(client, childInstance);
            }

            if (instance.ObjectState == ObjectState.Deleted)
            {
                response = client.DeleteAsync(String.Format("api/data/instances/{0}", instance.InstanceID)).Result;
                if (response.IsSuccessStatusCode)
                {
                }
                else
                {
                    throw (new ApplicationException("Attempt to delete root instance failed."));
                }
            }
        }

        public void SaveData(HttpClient client, EAVSubject subject)
        {
            HttpResponseMessage response;

            if (subject.ObjectState == ObjectState.New)
            {
                response = client.PostAsJsonAsync<EAV.Model.IEAVSubject>(String.Format("api/data/entities/{0}/subjects", subject.EntityID), subject).Result;
                if (response.IsSuccessStatusCode)
                {
                    EAVSubject newSubject = response.Content.ReadAsAsync<EAVSubject>().Result;

                    subject.SubjectID = newSubject.SubjectID;
                    subject.MarkUnmodified();
                }
                else
                {
                    throw (new ApplicationException("Attempt to create subject failed."));
                }
            }
            else if (subject.ObjectState == ObjectState.Modified)
            {
                response = client.PatchAsJsonAsync<EAV.Model.IEAVSubject>("api/data/subjects", subject).Result;
                if (response.IsSuccessStatusCode)
                {
                    subject.MarkUnmodified();
                }
                else
                {
                    throw (new ApplicationException("Attempt to update subject failed."));
                }
            }

            foreach (EAVRootInstance rootInstance in subject.Instances)
            {
                SaveRootInstance(client, rootInstance);
            }

            if (subject.ObjectState == ObjectState.Deleted)
            {
                response = client.DeleteAsync(String.Format("api/data/subjects/{0}", subject.SubjectID)).Result;
                if (response.IsSuccessStatusCode)
                {
                }
                else
                {
                    throw (new ApplicationException("Attempt to delete subject failed."));
                }
            }
        }

        private void SaveEntity(HttpClient client, EAVEntity entity)
        {
            HttpResponseMessage response;

            if (entity.ObjectState == ObjectState.New)
            {
                response = client.PostAsJsonAsync<EAV.Model.IEAVEntity>(String.Format("api/data/entities"), entity).Result;
                if (response.IsSuccessStatusCode)
                {
                    EAVEntity newEntity = response.Content.ReadAsAsync<EAVEntity>().Result;

                    entity.MarkUnmodified();
                }
                else
                {
                    throw (new ApplicationException("Attempt to create entity failed."));
                }
            }
            else if (entity.ObjectState == ObjectState.Modified)
            {
                response = client.PatchAsJsonAsync<EAV.Model.IEAVEntity>("api/data/entities", entity).Result;
                if (response.IsSuccessStatusCode)
                {
                    entity.MarkUnmodified();
                }
                else
                {
                    throw (new ApplicationException("Attempt to update entity failed."));
                }
            }

            if (entity.ObjectState == ObjectState.Deleted)
            {
                response = client.DeleteAsync(String.Format("api/data/entities/{1}", entity.EntityID)).Result;
                if (response.IsSuccessStatusCode)
                {
                }
                else
                {
                    throw (new ApplicationException("Attempt to delete entity failed."));
                }
            }
        }
        #endregion
    }
}

