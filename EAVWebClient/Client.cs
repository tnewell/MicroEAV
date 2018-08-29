// MicroEAV
//
// Copyright(C) 2017  Tim Newell

// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with this program.If not, see<http://www.gnu.org/licenses/>.

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
            // ConfigureAwait is used here to avoid deadlocks.
            // https://blog.stephencleary.com/2012/07/dont-block-on-async-code.html
            return (await client.SendAsync(new HttpRequestMessage(new HttpMethod("PATCH"), requestUri) { Content = new ObjectContent<T>(value, formatter) }).ConfigureAwait(false));
        }

        public static async Task<HttpResponseMessage> PatchAsJsonAsync<T>(this HttpClient client, string requestUri, T value)
        {
            // ConfigureAwait is used here to avoid deadlocks.
            // https://blog.stephencleary.com/2012/07/dont-block-on-async-code.html
            return (await PatchAsync<T>(client, requestUri, value, new JsonMediaTypeFormatter()).ConfigureAwait(false));
        }

        public static async Task<HttpResponseMessage> PatchAsXmlAsync<T>(this HttpClient client, string requestUri, T value)
        {
            // ConfigureAwait is used here to avoid deadlocks.
            // https://blog.stephencleary.com/2012/07/dont-block-on-async-code.html
            return (await PatchAsync<T>(client, requestUri, value, new XmlMediaTypeFormatter()).ConfigureAwait(false));
        }
    }

    public interface IEAVDataClient
    {
        IEnumerable<IModelUnit> LoadUnits();

        void SaveUnit(IModelUnit unit);

        IEnumerable<IModelEntity> LoadEntities();

        void SaveEntity(IModelEntity entity);

        IEnumerable<IModelContext> LoadContexts();

        void SaveContext(IModelContext context);

        void LoadSubjects(IModelContext context);

        void LoadSubjects(IModelEntity entity);

        void SaveSubject(IModelSubject subject);

        void LoadRootContainers(IModelContext context);

        void LoadMetadata(IModelRootContainer container);

        void SaveMetadata(IModelRootContainer container);

        void LoadRootInstances(IModelSubject subject, IModelRootContainer container);

        void LoadData(IModelRootInstance instance);

        void SaveData(IModelRootInstance instance);
    }

    public class EAVDataClient : IEAVDataClient, IDisposable
    {
        private HttpClient client;

        public EAVDataClient()
        {
        }

        public EAVDataClient(Uri uri)
        {
            client = new HttpClient() { BaseAddress = uri };
        }

        public EAVDataClient(string address)
        {
            client = new HttpClient() { BaseAddress = new Uri(address) };
        }

        public void Dispose()
        {
            if (client != null)
                client.Dispose();
        }

        #region Load Helpers
        private void LoadAttributeUnits(IModelAttribute attribute)
        {
            HttpResponseMessage response = client.GetAsync(String.Format("api/meta/attribute/{0}/units", attribute.AttributeID)).Result;
            if (response.IsSuccessStatusCode)
            {
                var units = response.Content.ReadAsAsync<IEnumerable<ModelUnit>>().Result;

                attribute.Units.Clear();

                foreach (ModelUnit unit in units)
                {
                    unit.MarkUnmodified();

                    attribute.Units.Add(unit);
                }
            }
            else
            {
                throw (new ApplicationException("Attempt to get units failed."));
            }
        }

        private void LoadAttributes(IModelContainer container)
        {
            HttpResponseMessage response = client.GetAsync(String.Format("api/meta/containers/{0}/attributes", container.ContainerID.GetValueOrDefault())).Result;
            if (response.IsSuccessStatusCode)
            {
                var attributes = response.Content.ReadAsAsync<IEnumerable<ModelAttribute>>().Result;

                container.Attributes.Clear();

                foreach (ModelAttribute attribute in attributes)
                {
                    attribute.MarkUnmodified();

                    LoadAttributeUnits(attribute);

                    container.Attributes.Add(attribute);
                }
            }
            else
            {
                throw (new ApplicationException("Attempt to get attributes failed."));
            }
        }

        private void LoadChildContainers(IModelContainer parentContainer)
        {
            HttpResponseMessage response = client.GetAsync(String.Format("api/meta/containers/{0}/containers", parentContainer.ContainerID.GetValueOrDefault())).Result;
            if (response.IsSuccessStatusCode)
            {
                var childContainers = response.Content.ReadAsAsync<IEnumerable<ModelChildContainer>>().Result;

                parentContainer.ChildContainers.Clear();

                foreach (ModelChildContainer childContainer in childContainers)
                {
                    childContainer.MarkUnmodified();

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

        private void LoadValueUnit(IModelValue value)
        {
            HttpResponseMessage response = client.GetAsync(String.Format("api/meta/units/{0}", value.UnitID)).Result;
            if (response.IsSuccessStatusCode)
            {
                value.Unit = response.Content.ReadAsAsync<ModelUnit>().Result;
            }
            else
            {
                throw (new ApplicationException("Attempt to get unit failed."));
            }
        }

        private void LoadValues(IModelInstance instance)
        {
            HttpResponseMessage response = client.GetAsync(String.Format("api/data/instances/{0}/values", instance.InstanceID.GetValueOrDefault())).Result;
            if (response.IsSuccessStatusCode)
            {
                var values = response.Content.ReadAsAsync<IEnumerable<ModelValue>>().Result;

                instance.Values.Clear();

                foreach (ModelValue value in values)
                {
                    value.MarkUnmodified();

                    if (value.UnitID != null)
                        LoadValueUnit(value);

                    instance.Values.Add(value);
                }
            }
            else
            {
                throw (new ApplicationException("Attempt to get values failed."));
            }
        }

        private void LoadChildInstances(IModelInstance parentInstance)
        {
            HttpResponseMessage response = client.GetAsync(String.Format("api/data/instances/{0}/instances", parentInstance.InstanceID.GetValueOrDefault())).Result;
            if (response.IsSuccessStatusCode)
            {
                var childInstances = response.Content.ReadAsAsync<IEnumerable<ModelChildInstance>>().Result;

                parentInstance.ChildInstances.Clear();

                foreach (ModelChildInstance childInstance in childInstances)
                {
                    childInstance.MarkUnmodified();

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
        #endregion

        #region Save Helpers
        private void SaveAttributeUnits(int attributeID, IEnumerable<IModelUnit> units)
        {
            HttpResponseMessage response;

            response = client.PatchAsJsonAsync<IEnumerable<EAV.Store.IStoreUnit>>(String.Format("api/meta/attributes/{0}/units", attributeID), units).Result;
            if (response.IsSuccessStatusCode)
            {
                // Nothing to do
            }
            else
            {
                throw (new ApplicationException("Attempt to update attribute units failed."));
            }
        }

        private void SaveAttribute(IModelAttribute attribute)
        {
            HttpResponseMessage response;

            if (attribute.ObjectState == ObjectState.New)
            {
                response = client.PostAsJsonAsync<EAV.Store.IStoreAttribute>(String.Format("api/meta/containers/{0}/attributes", attribute.ContainerID.GetValueOrDefault()), attribute).Result;
                if (response.IsSuccessStatusCode)
                {
                    ModelAttribute newAttribute = response.Content.ReadAsAsync<ModelAttribute>().Result;

                    attribute.AttributeID = newAttribute.AttributeID;

                    if (!attribute.VariableUnits.GetValueOrDefault(true))
                        SaveAttributeUnits(attribute.AttributeID.Value, attribute.Units);

                    attribute.MarkUnmodified();
                }
                else
                {
                    throw (new ApplicationException("Attempt to create attribute failed."));
                }
            }
            else if (attribute.ObjectState == ObjectState.Modified)
            {
                response = client.PatchAsJsonAsync<EAV.Store.IStoreAttribute>("api/meta/attributes", attribute).Result;
                if (response.IsSuccessStatusCode)
                {
                    if (!attribute.VariableUnits.GetValueOrDefault(true))
                        SaveAttributeUnits(attribute.AttributeID.Value, attribute.Units);

                    attribute.MarkUnmodified();
                }
                else
                {
                    throw (new ApplicationException("Attempt to update attribute failed."));
                }
            }

            if (attribute.ObjectState == ObjectState.Deleted)
            {
                response = client.DeleteAsync(String.Format("api/meta/attributes/{0}", attribute.AttributeID.GetValueOrDefault())).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Nothing to do
                }
                else
                {
                    throw (new ApplicationException("Attempt to delete attribute failed."));
                }
            }
        }

        private void SaveChildContainer(IModelChildContainer container)
        {
            HttpResponseMessage response;

            if (container.ObjectState == ObjectState.New)
            {
                response = client.PostAsJsonAsync<EAV.Store.IStoreContainer>(String.Format("api/meta/containers/{0}/containers", container.ParentContainerID.GetValueOrDefault()), container).Result;
                if (response.IsSuccessStatusCode)
                {
                    ModelContainer newContainer = response.Content.ReadAsAsync<ModelChildContainer>().Result;

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
                response = client.PatchAsJsonAsync<EAV.Store.IStoreContainer>("api/meta/containers", container).Result;
                if (response.IsSuccessStatusCode)
                {
                    container.MarkUnmodified();
                }
                else
                {
                    throw (new ApplicationException("Attempt to update child container failed."));
                }
            }

            foreach (ModelAttribute attribute in container.Attributes)
            {
                SaveAttribute(attribute);
            }

            foreach (ModelChildContainer childContainer in container.ChildContainers)
            {
                SaveChildContainer(childContainer);
            }

            if (container.ObjectState == ObjectState.Deleted)
            {
                response = client.DeleteAsync(String.Format("api/meta/containers/{0}", container.ContainerID.GetValueOrDefault())).Result;
                if (response.IsSuccessStatusCode)
                {
                }
                else
                {
                    throw (new ApplicationException("Attempt to delete child container failed."));
                }
            }
        }

        private void SaveValueUnit(IModelUnit unit)
        {
            HttpResponseMessage response;

            if (unit.ObjectState == ObjectState.New)
            {
                response = client.PostAsJsonAsync<EAV.Store.IStoreUnit>("api/meta/units", unit).Result;
                if (response.IsSuccessStatusCode)
                {
                    ModelUnit newUnit = response.Content.ReadAsAsync<ModelUnit>().Result;

                    unit.UnitID = newUnit.UnitID;

                    unit.MarkUnmodified();
                }
                else
                {
                    throw (new ApplicationException("Attempt to create unit failed."));
                }
            }
            else if (unit.ObjectState == ObjectState.Modified)
            {
                response = client.PatchAsJsonAsync<EAV.Store.IStoreUnit>("api/meta/units", unit).Result;
                if (response.IsSuccessStatusCode)
                {
                    unit.MarkUnmodified();
                }
                else
                {
                    throw (new ApplicationException("Attempt to update unit failed."));
                }
            }

            if (unit.ObjectState == ObjectState.Deleted)
            {
                response = client.DeleteAsync(String.Format("api/meta/units/{0}", unit.UnitID.GetValueOrDefault())).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Nothing to do
                }
                else
                {
                    throw (new ApplicationException("Attempt to delete unit failed."));
                }
            }
        }

        private void SaveValue(IModelValue value)
        {
            HttpResponseMessage response;

            if (value.Unit != null)
                SaveValueUnit(value.Unit);

            if (value.ObjectState == ObjectState.New)
            {
                response = client.PostAsJsonAsync<EAV.Store.IStoreValue>(String.Format("api/data/instances/{0}/values?attribute={1}", value.InstanceID.GetValueOrDefault(), value.AttributeID), value).Result;
                if (response.IsSuccessStatusCode)
                {
                    ModelValue newValue = response.Content.ReadAsAsync<ModelValue>().Result;

                    value.MarkUnmodified();
                }
                else
                {
                    throw (new ApplicationException("Attempt to create value failed."));
                }
            }
            else if (value.ObjectState == ObjectState.Modified)
            {
                response = client.PatchAsJsonAsync<EAV.Store.IStoreValue>("api/data/values", value).Result;
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
                response = client.DeleteAsync(String.Format("api/data/instances/{0}/values/{1}", value.InstanceID.GetValueOrDefault(), value.AttributeID)).Result;
                if (response.IsSuccessStatusCode)
                {
                }
                else
                {
                    throw (new ApplicationException("Attempt to delete value failed."));
                }
            }
        }

        private void SaveChildInstance(IModelChildInstance instance)
        {
            HttpResponseMessage response;

            if (instance.ObjectState == ObjectState.New)
            {
                response = client.PostAsJsonAsync<EAV.Store.IStoreInstance>(String.Format("api/data/instances/{0}/instances?container={1}", instance.ParentInstanceID.GetValueOrDefault(), instance.ContainerID), instance).Result;
                if (response.IsSuccessStatusCode)
                {
                    ModelInstance newInstance = response.Content.ReadAsAsync<ModelChildInstance>().Result;

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
                response = client.PatchAsJsonAsync<EAV.Store.IStoreInstance>("api/data/instances", instance).Result;
                if (response.IsSuccessStatusCode)
                {
                    instance.MarkUnmodified();
                }
                else
                {
                    throw (new ApplicationException("Attempt to update child instance failed."));
                }
            }

            foreach (ModelValue value in instance.Values)
            {
                SaveValue(value);
            }

            foreach (ModelChildInstance childInstance in instance.ChildInstances)
            {
                SaveChildInstance(childInstance);
            }

            if (instance.ObjectState == ObjectState.Deleted)
            {
                response = client.DeleteAsync(String.Format("api/data/instances/{0}", instance.InstanceID.GetValueOrDefault())).Result;
                if (response.IsSuccessStatusCode)
                {
                }
                else
                {
                    throw (new ApplicationException("Attempt to delete child instance failed."));
                }
            }
        }
        #endregion

        public IEnumerable<IModelUnit> LoadUnits()
        {
            HttpResponseMessage response = client.GetAsync(String.Format("api/metadata/units")).Result;
            if (response.IsSuccessStatusCode)
            {
                var units = response.Content.ReadAsAsync<IEnumerable<ModelUnit>>().Result;

                foreach (var unit in units)
                    unit.MarkUnmodified();

                return (units);
            }
            else
            {
                throw (new ApplicationException("Attempt to get units failed."));
            }
        }

        public void SaveUnit(IModelUnit unit)
        {
            HttpResponseMessage response;

            if (unit.ObjectState == ObjectState.New)
            {
                response = client.PostAsJsonAsync<EAV.Store.IStoreUnit>(String.Format("api/metadata/units"), unit).Result;
                if (response.IsSuccessStatusCode)
                {
                    ModelUnit newUnit = response.Content.ReadAsAsync<ModelUnit>().Result;

                    unit.UnitID = newUnit.UnitID;

                    unit.MarkUnmodified();
                }
                else
                {
                    throw (new ApplicationException("Attempt to create unit failed."));
                }
            }
            else if (unit.ObjectState == ObjectState.Modified)
            {
                response = client.PatchAsJsonAsync<EAV.Store.IStoreUnit>("api/metadata/units", unit).Result;
                if (response.IsSuccessStatusCode)
                {
                    unit.MarkUnmodified();
                }
                else
                {
                    throw (new ApplicationException("Attempt to update unit failed."));
                }
            }

            if (unit.ObjectState == ObjectState.Deleted)
            {
                response = client.DeleteAsync(String.Format("api/metadata/units/{0}", unit.UnitID.GetValueOrDefault())).Result;
                if (response.IsSuccessStatusCode)
                {
                }
                else
                {
                    throw (new ApplicationException("Attempt to delete unit failed."));
                }
            }
        }

        public IEnumerable<IModelEntity> LoadEntities()
        {
            HttpResponseMessage response = client.GetAsync(String.Format("api/data/entities")).Result;
            if (response.IsSuccessStatusCode)
            {
                var entities = response.Content.ReadAsAsync<IEnumerable<ModelEntity>>().Result;

                foreach (var entity in entities)
                    entity.MarkUnmodified();

                return (entities);
            }
            else
            {
                throw (new ApplicationException("Attempt to get entities failed."));
            }
        }

        public void SaveEntity(IModelEntity entity)
        {
            HttpResponseMessage response;

            if (entity.ObjectState == ObjectState.New)
            {
                response = client.PostAsJsonAsync<EAV.Store.IStoreEntity>(String.Format("api/data/entities"), entity).Result;
                if (response.IsSuccessStatusCode)
                {
                    ModelEntity newEntity = response.Content.ReadAsAsync<ModelEntity>().Result;

                    entity.EntityID = newEntity.EntityID;

                    entity.MarkUnmodified();
                }
                else
                {
                    throw (new ApplicationException("Attempt to create entity failed."));
                }
            }
            else if (entity.ObjectState == ObjectState.Modified)
            {
                response = client.PatchAsJsonAsync<EAV.Store.IStoreEntity>("api/data/entities", entity).Result;
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
                response = client.DeleteAsync(String.Format("api/data/entities/{0}", entity.EntityID.GetValueOrDefault())).Result;
                if (response.IsSuccessStatusCode)
                {
                }
                else
                {
                    throw (new ApplicationException("Attempt to delete entity failed."));
                }
            }
        }

        public IEnumerable<IModelContext> LoadContexts()
        {
            HttpResponseMessage response = client.GetAsync("api/meta/contexts").Result;
            if (response.IsSuccessStatusCode)
            {
                var contexts = response.Content.ReadAsAsync<IEnumerable<ModelContext>>().Result;

                foreach (var context in contexts)
                    context.MarkUnmodified();

                return (contexts);
            }
            else
            {
                throw (new ApplicationException("Attempt to get contexts failed."));
            }
        }

        public void SaveContext(IModelContext context)
        {
            HttpResponseMessage response;

            if (context.ObjectState == ObjectState.New)
            {
                response = client.PostAsJsonAsync<EAV.Store.IStoreContext>(String.Format("api/metadata/contexts"), context).Result;
                if (response.IsSuccessStatusCode)
                {
                    ModelContext newContext = response.Content.ReadAsAsync<ModelContext>().Result;

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
                response = client.PatchAsJsonAsync<EAV.Store.IStoreContext>("api/metadata/contexts", context).Result;
                if (response.IsSuccessStatusCode)
                {
                    context.MarkUnmodified();
                }
                else
                {
                    throw (new ApplicationException("Attempt to update context failed."));
                }
            }

            if (context.ObjectState == ObjectState.Deleted)
            {
                response = client.DeleteAsync(String.Format("api/metadata/contexts/{0}", context.ContextID.GetValueOrDefault())).Result;
                if (response.IsSuccessStatusCode)
                {
                }
                else
                {
                    throw (new ApplicationException("Attempt to delete context failed."));
                }
            }
        }

        public void LoadSubjects(IModelContext context)
        {
            HttpResponseMessage response = client.GetAsync(String.Format("api/meta/contexts/{0}/subjects", context.ContextID.GetValueOrDefault())).Result;
            if (response.IsSuccessStatusCode)
            {
                var subjects = response.Content.ReadAsAsync<IEnumerable<ModelSubject>>().Result;

                context.Subjects.Clear();

                foreach (ModelSubject subject in subjects)
                {
                    //LoadEntity(client, subject);

                    subject.MarkUnmodified();

                    context.Subjects.Add(subject);
                }
            }
            else
            {
                throw (new ApplicationException("Attempt to get subjects failed."));
            }
        }

        public void LoadSubjects(IModelEntity entity)
        {
            HttpResponseMessage response = client.GetAsync(String.Format("api/data/entities/{0}/subjects", entity.EntityID)).Result;
            if (response.IsSuccessStatusCode)
            {
                var subjects = response.Content.ReadAsAsync<IEnumerable<ModelSubject>>().Result;

                entity.Subjects.Clear();

                foreach (ModelSubject subject in subjects)
                {
                    //LoadContext(client, subject);

                    subject.MarkUnmodified();

                    entity.Subjects.Add(subject);
                }
            }
            else
            {
                throw (new ApplicationException("Attempt to get subjects failed."));
            }
        }

        public void SaveSubject(IModelSubject subject)
        {
            HttpResponseMessage response;

            if (subject.ObjectState == ObjectState.New)
            {
                response = client.PostAsJsonAsync<EAV.Store.IStoreSubject>(String.Format("api/metadata/subjects"), subject).Result;
                if (response.IsSuccessStatusCode)
                {
                    ModelSubject newSubject = response.Content.ReadAsAsync<ModelSubject>().Result;

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
                response = client.PatchAsJsonAsync<EAV.Store.IStoreSubject>("api/metadata/subjects", subject).Result;
                if (response.IsSuccessStatusCode)
                {
                    subject.MarkUnmodified();
                }
                else
                {
                    throw (new ApplicationException("Attempt to update subject failed."));
                }
            }

            if (subject.ObjectState == ObjectState.Deleted)
            {
                response = client.DeleteAsync(String.Format("api/metadata/subjects/{0}", subject.SubjectID.GetValueOrDefault())).Result;
                if (response.IsSuccessStatusCode)
                {
                }
                else
                {
                    throw (new ApplicationException("Attempt to delete subject failed."));
                }
            }
        }

        public void LoadRootContainers(IModelContext context)
        {
            HttpResponseMessage response = client.GetAsync(String.Format("api/meta/contexts/{0}/containers", context.ContextID.GetValueOrDefault())).Result;
            if (response.IsSuccessStatusCode)
            {
                var rootContainers = response.Content.ReadAsAsync<IEnumerable<ModelRootContainer>>().Result;

                context.Containers.Clear();

                foreach (ModelRootContainer rootContainer in rootContainers)
                {
                    rootContainer.MarkUnmodified();

                    context.Containers.Add(rootContainer);
                }
            }
            else
            {
                throw (new ApplicationException("Attempt to get root containers failed."));
            }
        }

        public void LoadMetadata(IModelRootContainer container)
        {
            try
            {
                LoadAttributes(container);
                LoadChildContainers(container);
            }
            catch (Exception ex)
            {
                throw (new ApplicationException("Attempt to get metadata failed.", ex));
            }
        }

        public void SaveMetadata(IModelRootContainer container)
        {
            HttpResponseMessage response;

            if (container.Context.ObjectState != ObjectState.Unmodified)
                SaveContext(container.Context);

            if (container.ObjectState == ObjectState.New)
            {
                response = client.PostAsJsonAsync<EAV.Store.IStoreContainer>(String.Format("api/meta/contexts/{0}/containers", container.ContextID.GetValueOrDefault()), container).Result;
                if (response.IsSuccessStatusCode)
                {
                    ModelContainer newContainer = response.Content.ReadAsAsync<ModelRootContainer>().Result;

                    container.ContainerID = newContainer.ContainerID;

                    container.MarkUnmodified();
                }
                else
                {
                    throw (new ApplicationException("Attempt to create metadata failed."));
                }
            }
            else if (container.ObjectState == ObjectState.Modified)
            {
                response = client.PatchAsJsonAsync<EAV.Store.IStoreContainer>("api/meta/containers", container).Result;
                if (response.IsSuccessStatusCode)
                {
                    container.MarkUnmodified();
                }
                else
                {
                    throw (new ApplicationException("Attempt to update metadata failed."));
                }
            }

            foreach (ModelAttribute attribute in container.Attributes)
            {
                SaveAttribute(attribute);
            }

            foreach (ModelChildContainer childContainer in container.ChildContainers)
            {
                SaveChildContainer(childContainer);
            }

            if (container.ObjectState == ObjectState.Deleted)
            {
                response = client.DeleteAsync(String.Format("api/meta/containers/{0}", container.ContainerID.GetValueOrDefault())).Result;
                if (response.IsSuccessStatusCode)
                {
                }
                else
                {
                    throw (new ApplicationException("Attempt to delete metadata failed."));
                }
            }
        }

        public void LoadRootInstances(IModelSubject subject, IModelRootContainer container)
        {
            HttpResponseMessage response = client.GetAsync(String.Format("api/data/subjects/{0}/instances?container={1}", subject.SubjectID.GetValueOrDefault(), container != null ? container.ContainerID : null)).Result;
            if (response.IsSuccessStatusCode)
            {
                var rootInstances = response.Content.ReadAsAsync<IEnumerable<ModelRootInstance>>().Result;

                subject.Instances.Clear();

                foreach (ModelRootInstance rootInstance in rootInstances)
                {
                    rootInstance.MarkUnmodified();

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

        public void LoadData(IModelRootInstance instance)
        {
            HttpResponseMessage response = client.GetAsync(String.Format("api/data/instances/{0}/instances", instance.InstanceID.GetValueOrDefault())).Result;
            if (response.IsSuccessStatusCode)
            {
                var childInstances = response.Content.ReadAsAsync<IEnumerable<ModelChildInstance>>().Result;

                instance.ChildInstances.Clear();

                foreach (ModelChildInstance childInstance in childInstances)
                {
                    childInstance.MarkUnmodified();

                    LoadValues(childInstance);
                    LoadChildInstances(childInstance);

                    instance.ChildInstances.Add(childInstance);
                }
            }
            else
            {
                throw (new ApplicationException("Attempt to load data failed."));
            }
        }

        public void SaveData(IModelRootInstance instance)
        {
            HttpResponseMessage response;

            if (instance.ObjectState == ObjectState.New)
            {
                response = client.PostAsJsonAsync<EAV.Store.IStoreInstance>(String.Format("api/data/subjects/{0}/instances?container={1}", instance.SubjectID.GetValueOrDefault(), instance.ContainerID), instance).Result;
                if (response.IsSuccessStatusCode)
                {
                    ModelInstance newInstance = response.Content.ReadAsAsync<ModelRootInstance>().Result;

                    instance.InstanceID = newInstance.InstanceID;
                    instance.MarkUnmodified();
                }
                else
                {
                    throw (new ApplicationException("Attempt to create data failed."));
                }
            }
            else if (instance.ObjectState == ObjectState.Modified)
            {
                response = client.PatchAsJsonAsync<EAV.Store.IStoreInstance>("api/data/instances", instance).Result;
                if (response.IsSuccessStatusCode)
                {
                    instance.MarkUnmodified();
                }
                else
                {
                    throw (new ApplicationException("Attempt to update data failed."));
                }
            }

            foreach (ModelValue value in instance.Values)
            {
                SaveValue(value);
            }

            foreach (ModelChildInstance childInstance in instance.ChildInstances)
            {
                SaveChildInstance(childInstance);
            }

            if (instance.ObjectState == ObjectState.Deleted)
            {
                response = client.DeleteAsync(String.Format("api/data/instances/{0}", instance.InstanceID.GetValueOrDefault())).Result;
                if (response.IsSuccessStatusCode)
                {
                }
                else
                {
                    throw (new ApplicationException("Attempt to delete data failed."));
                }
            }
        }
    }
}

