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
using System.Diagnostics;
using System.Linq;
using System.Net.Http;

//using EAVModelLibrary;


namespace EAVModelClient
{
    public class ModelClient : EAV.Model.Clients.IModelClient, IDisposable
    {
        private HttpClient client;

        public ModelClient()
        {
        }

        public ModelClient(Uri uri)
        {
            client = new HttpClient() { BaseAddress = uri };
        }

        public ModelClient(string address)
        {
            client = new HttpClient() { BaseAddress = new Uri(address) };
        }

        public void Dispose()
        {
            if (client != null)
                client.Dispose();
        }

        #region Load Helpers
        private void LoadAttributeUnits(EAV.Model.IModelAttribute attribute)
        {
            bool attributeWasUnmodified = attribute.ObjectState == EAV.Model.ObjectState.Unmodified;

            HttpResponseMessage response = client.GetAsync(String.Format("api/metadata/attributes/{0}/units", attribute.AttributeID)).Result;
            if (response.IsSuccessStatusCode)
            {
                attribute.Units.Clear();

                var units = response.Content.ReadAsAsync<IEnumerable<EAVModelLibrary.ModelUnit>>().Result;
                foreach (EAVModelLibrary.ModelUnit unit in units)
                {
                    unit.MarkUnmodified();

                    attribute.Units.Add(unit);

                    unit.MarkUnmodified();
                }

                if (attributeWasUnmodified)
                    attribute.MarkUnmodified();
            }
            else
            {
                throw (new ApplicationException("Attempt to get attribute units failed."));
            }
        }

        private void LoadAttributes(EAV.Model.IModelContainer container)
        {
            bool containerWasUnmodified = container.ObjectState == EAV.Model.ObjectState.Unmodified;

            HttpResponseMessage response = client.GetAsync(String.Format("api/metadata/containers/{0}/attributes", container.ContainerID.GetValueOrDefault())).Result;
            if (response.IsSuccessStatusCode)
            {
                container.Attributes.Clear();

                var attributes = response.Content.ReadAsAsync<IEnumerable<EAVModelLibrary.ModelAttribute>>().Result;
                foreach (EAVModelLibrary.ModelAttribute attribute in attributes)
                {
                    attribute.MarkUnmodified();

                    LoadAttributeUnits(attribute);

                    container.Attributes.Add(attribute);

                    attribute.MarkUnmodified();
                }

                if (containerWasUnmodified)
                    container.MarkUnmodified();
            }
            else
            {
                throw (new ApplicationException("Attempt to get attributes failed."));
            }
        }

        private void LoadChildContainers(EAV.Model.IModelContainer parentContainer)
        {
            bool parentContainerWasUnmodified = parentContainer.ObjectState == EAV.Model.ObjectState.Unmodified;

            HttpResponseMessage response = client.GetAsync(String.Format("api/metadata/containers/{0}/containers", parentContainer.ContainerID.GetValueOrDefault())).Result;
            if (response.IsSuccessStatusCode)
            {
                parentContainer.ChildContainers.Clear();

                var childContainers = response.Content.ReadAsAsync<IEnumerable<EAVModelLibrary.ModelChildContainer>>().Result;
                foreach (EAVModelLibrary.ModelChildContainer childContainer in childContainers)
                {
                    childContainer.MarkUnmodified();

                    LoadAttributes(childContainer);
                    LoadChildContainers(childContainer);

                    parentContainer.ChildContainers.Add(childContainer);

                    childContainer.MarkUnmodified();
                }

                if (parentContainerWasUnmodified)
                    parentContainer.MarkUnmodified();
            }
            else
            {
                throw (new ApplicationException("Attempt to get root containers failed."));
            }
        }

        private void LoadValueUnit(EAV.Model.IModelValue value)
        {
            bool valueWasUnmodified = value.ObjectState == EAV.Model.ObjectState.Unmodified;

            HttpResponseMessage response = client.GetAsync(String.Format("api/metadata/units/{0}", value.UnitID)).Result;
            if (response.IsSuccessStatusCode)
            {
                var unit = response.Content.ReadAsAsync<EAVModelLibrary.ModelUnit>().Result;

                unit.MarkUnmodified();

                value.Unit = unit;

                unit.MarkUnmodified();

                if (valueWasUnmodified)
                    value.MarkUnmodified();
            }
            else
            {
                throw (new ApplicationException("Attempt to get unit failed."));
            }
        }

        private void LoadValues(EAV.Model.IModelInstance instance, IEnumerable<EAV.Model.IModelAttribute> attributes)
        {
            bool instanceWasUnmodified = instance.ObjectState == EAV.Model.ObjectState.Unmodified;
            Dictionary<int?, bool> attributeWasUnmodified = attributes.ToDictionary(key => key.AttributeID, val => val.ObjectState == EAV.Model.ObjectState.Unmodified);

            HttpResponseMessage response = client.GetAsync(String.Format("api/data/instances/{0}/values", instance.InstanceID.GetValueOrDefault())).Result;
            if (response.IsSuccessStatusCode)
            {
                instance.Values.Clear();

                var values = response.Content.ReadAsAsync<IEnumerable<EAVModelLibrary.ModelValue>>().Result;
                foreach (EAVModelLibrary.ModelValue value in values)
                {
                    //value.MarkUnmodified();

                    value.Attribute = attributes.Single(it => it.AttributeID == value.AttributeID);

                    if (value.UnitID != null)
                        LoadValueUnit(value);

                    instance.Values.Add(value);

                    value.MarkUnmodified();
                }

                if (instanceWasUnmodified)
                    instance.MarkUnmodified();

                foreach (var attribute in attributes)
                {
                    if (attributeWasUnmodified[attribute.AttributeID])
                        attribute.MarkUnmodified();
                }
            }
            else
            {
                throw (new ApplicationException("Attempt to get values failed."));
            }
        }

        private void LoadChildInstances(EAV.Model.IModelInstance parentInstance, IEnumerable<EAV.Model.IModelContainer> containers)
        {
            bool ifParentInstanceWasUnmodified = parentInstance.ObjectState == EAV.Model.ObjectState.Unmodified;
            Dictionary<int?, bool> containerWasUnmodified = containers.ToDictionary(key => key.ContainerID, val => val.ObjectState == EAV.Model.ObjectState.Unmodified);

            HttpResponseMessage response = client.GetAsync(String.Format("api/data/instances/{0}/instances", parentInstance.InstanceID.GetValueOrDefault())).Result;
            if (response.IsSuccessStatusCode)
            {
                parentInstance.ChildInstances.Clear();

                var childInstances = response.Content.ReadAsAsync<IEnumerable<EAVModelLibrary.ModelChildInstance>>().Result;
                foreach (EAVModelLibrary.ModelChildInstance childInstance in childInstances)
                {
                    childInstance.MarkUnmodified();

                    EAV.Model.IModelContainer container = containers.Single(it => it.ContainerID == childInstance.ContainerID);

                    childInstance.Container = container;

                    if (container.Attributes.Any())
                        LoadValues(childInstance, container.Attributes);

                    if (container.ChildContainers.Any())
                        LoadChildInstances(childInstance, container.ChildContainers);

                    parentInstance.ChildInstances.Add(childInstance);

                    childInstance.MarkUnmodified();
                }

                if (ifParentInstanceWasUnmodified)
                    parentInstance.MarkUnmodified();

                foreach (var container in containers)
                {
                    if (containerWasUnmodified[container.ContainerID])
                        container.MarkUnmodified();
                }
            }
            else
            {
                throw (new ApplicationException("Attempt to get child instances failed."));
            }
        }

        private void LoadSubjectEntity(EAV.Model.IModelSubject subject)
        {
            bool subjectWasUnmodified = subject.ObjectState == EAV.Model.ObjectState.Unmodified;

            HttpResponseMessage response = client.GetAsync(String.Format("api/entities/{0}", subject.EntityID)).Result;
            if (response.IsSuccessStatusCode)
            {
                var entity = response.Content.ReadAsAsync<EAVModelLibrary.ModelEntity>().Result;

                entity.MarkUnmodified();

                subject.Entity = entity;

                entity.MarkUnmodified();

                if (subjectWasUnmodified)
                    subject.MarkUnmodified();
            }
            else
            {
                throw (new ApplicationException("Attempt to get subject entity failed."));
            }
        }

        private void LoadSubjectContext(EAV.Model.IModelSubject subject)
        {
            bool subjectWasUnmodified = subject.ObjectState == EAV.Model.ObjectState.Unmodified;

            HttpResponseMessage response = client.GetAsync(String.Format("api/metadata/contexts/{0}", subject.ContextID)).Result;
            if (response.IsSuccessStatusCode)
            {
                var context = response.Content.ReadAsAsync<EAVModelLibrary.ModelContext>().Result;

                context.MarkUnmodified();

                subject.Context = context;

                context.MarkUnmodified();

                if (subjectWasUnmodified)
                    subject.MarkUnmodified();
            }
            else
            {
                throw (new ApplicationException("Attempt to get subject context failed."));
            }
        }
        #endregion

        #region Save Helpers
        private void SaveAttributeUnits(int attributeID, IEnumerable<EAV.Model.IModelUnit> units)
        {
            HttpResponseMessage response;

            response = client.PatchAsJsonAsync<IEnumerable<EAV.Store.IStoreUnit>>(String.Format("api/metadata/attributes/{0}/units", attributeID), units).Result;
            if (response.IsSuccessStatusCode)
            {
                foreach (var unit in units)
                    unit.MarkUnmodified();
            }
            else
            {
                throw (new ApplicationException("Attempt to update attribute units failed."));
            }
        }

        private void SaveAttribute(EAV.Model.IModelAttribute attribute)
        {
            HttpResponseMessage response;

            if (attribute.ObjectState == EAV.Model.ObjectState.New)
            {
                response = client.PostAsJsonAsync<EAV.Store.IStoreAttribute>(String.Format("api/metadata/containers/{0}/attributes", attribute.ContainerID.GetValueOrDefault()), attribute).Result;
                if (response.IsSuccessStatusCode)
                {
                    EAVModelLibrary.ModelAttribute newAttribute = response.Content.ReadAsAsync<EAVModelLibrary.ModelAttribute>().Result;

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
            else if (attribute.ObjectState == EAV.Model.ObjectState.Modified)
            {
                response = client.PatchAsJsonAsync<EAV.Store.IStoreAttribute>("api/metadata/attributes", attribute).Result;
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

            if (attribute.ObjectState == EAV.Model.ObjectState.Deleted)
            {
                response = client.DeleteAsync(String.Format("api/metadata/attributes/{0}", attribute.AttributeID.GetValueOrDefault())).Result;
                if (response.IsSuccessStatusCode)
                {
                }
                else
                {
                    throw (new ApplicationException("Attempt to delete attribute failed."));
                }
            }
        }

        private void SaveChildContainer(EAV.Model.IModelChildContainer container)
        {
            HttpResponseMessage response;

            if (container.ObjectState == EAV.Model.ObjectState.New)
            {
                response = client.PostAsJsonAsync<EAV.Store.IStoreContainer>(String.Format("api/metadata/containers/{0}/containers", container.ParentContainerID.GetValueOrDefault()), container).Result;
                if (response.IsSuccessStatusCode)
                {
                    EAVModelLibrary.ModelContainer newContainer = response.Content.ReadAsAsync<EAVModelLibrary.ModelChildContainer>().Result;

                    container.ContainerID = newContainer.ContainerID;

                    container.MarkUnmodified();
                }
                else
                {
                    throw (new ApplicationException("Attempt to create child container failed."));
                }
            }
            else if (container.ObjectState == EAV.Model.ObjectState.Modified)
            {
                response = client.PatchAsJsonAsync<EAV.Store.IStoreContainer>("api/metadata/containers", container).Result;
                if (response.IsSuccessStatusCode)
                {
                }
                else
                {
                    throw (new ApplicationException("Attempt to update child container failed."));
                }
            }

            foreach (EAVModelLibrary.ModelAttribute attribute in container.Attributes)
            {
                SaveAttribute(attribute);
            }

            foreach (EAVModelLibrary.ModelChildContainer childContainer in container.ChildContainers)
            {
                SaveChildContainer(childContainer);
            }

            if (container.ObjectState == EAV.Model.ObjectState.Deleted)
            {
                response = client.DeleteAsync(String.Format("api/metadata/containers/{0}", container.ContainerID.GetValueOrDefault())).Result;
                if (response.IsSuccessStatusCode)
                {
                }
                else
                {
                    throw (new ApplicationException("Attempt to delete child container failed."));
                }
            }
        }

        private void SaveValueUnit(EAV.Model.IModelUnit unit)
        {
            HttpResponseMessage response;

            if (unit.ObjectState == EAV.Model.ObjectState.New)
            {
                response = client.PostAsJsonAsync<EAV.Store.IStoreUnit>("api/metadata/units", unit).Result;
                if (response.IsSuccessStatusCode)
                {
                    EAVModelLibrary.ModelUnit newUnit = response.Content.ReadAsAsync<EAVModelLibrary.ModelUnit>().Result;

                    unit.UnitID = newUnit.UnitID;

                    unit.MarkUnmodified();
                }
                else
                {
                    throw (new ApplicationException("Attempt to create unit failed."));
                }
            }
            else if (unit.ObjectState == EAV.Model.ObjectState.Modified)
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

            if (unit.ObjectState == EAV.Model.ObjectState.Deleted)
            {
                response = client.DeleteAsync(String.Format("api/metadata/units/{0}", unit.UnitID.GetValueOrDefault())).Result;
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

        private void SaveValue(EAV.Model.IModelValue value)
        {
            HttpResponseMessage response;

            if (value.Unit != null)
            {
                SaveValueUnit(value.Unit);
                value.Unit = value.Unit;
            }

            if (value.ObjectState == EAV.Model.ObjectState.New)
            {
                response = client.PostAsJsonAsync<EAV.Store.IStoreValue>(String.Format("api/data/instances/{0}/values?attribute={1}", value.InstanceID.GetValueOrDefault(), value.AttributeID), value).Result;
                if (response.IsSuccessStatusCode)
                {
                    EAVModelLibrary.ModelValue newValue = response.Content.ReadAsAsync<EAVModelLibrary.ModelValue>().Result;

                    value.MarkUnmodified();
                }
                else
                {
                    throw (new ApplicationException("Attempt to create value failed."));
                }
            }
            else if (value.ObjectState == EAV.Model.ObjectState.Modified)
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

            if (value.ObjectState == EAV.Model.ObjectState.Deleted)
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

        private void SaveChildInstance(EAV.Model.IModelChildInstance instance)
        {
            HttpResponseMessage response;

            if (instance.ObjectState == EAV.Model.ObjectState.New)
            {
                response = client.PostAsJsonAsync<EAV.Store.IStoreInstance>(String.Format("api/data/instances/{0}/instances?container={1}", instance.ParentInstanceID.GetValueOrDefault(), instance.ContainerID), instance).Result;
                if (response.IsSuccessStatusCode)
                {
                    EAVModelLibrary.ModelInstance newInstance = response.Content.ReadAsAsync<EAVModelLibrary.ModelChildInstance>().Result;

                    instance.InstanceID = newInstance.InstanceID;

                    instance.MarkUnmodified();
                }
                else
                {
                    throw (new ApplicationException("Attempt to create child instance failed."));
                }
            }
            else if (instance.ObjectState == EAV.Model.ObjectState.Modified)
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

            foreach (EAVModelLibrary.ModelValue value in instance.Values)
            {
                SaveValue(value);
            }

            foreach (EAVModelLibrary.ModelChildInstance childInstance in instance.ChildInstances)
            {
                SaveChildInstance(childInstance);
            }

            if (instance.ObjectState == EAV.Model.ObjectState.Deleted)
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

        public IEnumerable<EAV.Model.IModelUnit> LoadUnits()
        {
            HttpResponseMessage response = client.GetAsync(String.Format("api/metadata/units")).Result;
            if (response.IsSuccessStatusCode)
            {
                var units = response.Content.ReadAsAsync<IEnumerable<EAVModelLibrary.ModelUnit>>().Result;

                foreach (var unit in units)
                    unit.MarkUnmodified();

                return (units);
            }
            else
            {
                throw (new ApplicationException("Attempt to get units failed."));
            }
        }

        public void SaveUnit(EAV.Model.IModelUnit unit)
        {
            HttpResponseMessage response;

            if (unit.ObjectState == EAV.Model.ObjectState.New)
            {
                response = client.PostAsJsonAsync<EAV.Store.IStoreUnit>(String.Format("api/metadata/units"), unit).Result;
                if (response.IsSuccessStatusCode)
                {
                    EAVModelLibrary.ModelUnit newUnit = response.Content.ReadAsAsync<EAVModelLibrary.ModelUnit>().Result;

                    unit.UnitID = newUnit.UnitID;

                    unit.MarkUnmodified();
                }
                else
                {
                    throw (new ApplicationException("Attempt to create unit failed."));
                }
            }
            else if (unit.ObjectState == EAV.Model.ObjectState.Modified)
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

            if (unit.ObjectState == EAV.Model.ObjectState.Deleted)
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

        public IEnumerable<EAV.Model.IModelEntity> LoadEntities()
        {
            HttpResponseMessage response = client.GetAsync(String.Format("api/entities")).Result;
            if (response.IsSuccessStatusCode)
            {
                var entities = response.Content.ReadAsAsync<IEnumerable<EAVModelLibrary.ModelEntity>>().Result;

                foreach (var entity in entities)
                    entity.MarkUnmodified();

                return (entities);
            }
            else
            {
                throw (new ApplicationException("Attempt to get entities failed."));
            }
        }

        public void SaveEntity(EAV.Model.IModelEntity entity)
        {
            HttpResponseMessage response;

            if (entity.ObjectState == EAV.Model.ObjectState.New)
            {
                response = client.PostAsJsonAsync<EAV.Store.IStoreEntity>(String.Format("api/entities"), entity).Result;
                if (response.IsSuccessStatusCode)
                {
                    EAVModelLibrary.ModelEntity newEntity = response.Content.ReadAsAsync<EAVModelLibrary.ModelEntity>().Result;

                    entity.EntityID = newEntity.EntityID;

                    entity.MarkUnmodified();
                }
                else
                {
                    throw (new ApplicationException("Attempt to create entity failed."));
                }
            }
            else if (entity.ObjectState == EAV.Model.ObjectState.Modified)
            {
                response = client.PatchAsJsonAsync<EAV.Store.IStoreEntity>("api/entities", entity).Result;
                if (response.IsSuccessStatusCode)
                {
                    entity.MarkUnmodified();
                }
                else
                {
                    throw (new ApplicationException("Attempt to update entity failed."));
                }
            }

            if (entity.ObjectState == EAV.Model.ObjectState.Deleted)
            {
                response = client.DeleteAsync(String.Format("api/entities/{0}", entity.EntityID.GetValueOrDefault())).Result;
                if (response.IsSuccessStatusCode)
                {
                }
                else
                {
                    throw (new ApplicationException("Attempt to delete entity failed."));
                }
            }
        }

        public IEnumerable<EAV.Model.IModelContext> LoadContexts()
        {
            HttpResponseMessage response = client.GetAsync("api/metadata/contexts").Result;
            if (response.IsSuccessStatusCode)
            {
                var contexts = response.Content.ReadAsAsync<IEnumerable<EAVModelLibrary.ModelContext>>().Result;

                foreach (var context in contexts)
                    context.MarkUnmodified();

                return (contexts);
            }
            else
            {
                throw (new ApplicationException("Attempt to get contexts failed."));
            }
        }

        public void SaveContext(EAV.Model.IModelContext context)
        {
            HttpResponseMessage response;

            if (context.ObjectState == EAV.Model.ObjectState.New)
            {
                response = client.PostAsJsonAsync<EAV.Store.IStoreContext>(String.Format("api/metadata/contexts"), context).Result;
                if (response.IsSuccessStatusCode)
                {
                    EAVModelLibrary.ModelContext newContext = response.Content.ReadAsAsync<EAVModelLibrary.ModelContext>().Result;

                    context.ContextID = newContext.ContextID;

                    context.MarkUnmodified();
                }
                else
                {
                    throw (new ApplicationException("Attempt to create context failed."));
                }
            }
            else if (context.ObjectState == EAV.Model.ObjectState.Modified)
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

            if (context.ObjectState == EAV.Model.ObjectState.Deleted)
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

        public void LoadSubjects(EAV.Model.IModelContext context)
        {
            bool contextWasUnmodified = context.ObjectState == EAV.Model.ObjectState.Unmodified;

            HttpResponseMessage response = client.GetAsync(String.Format("api/metadata/contexts/{0}/subjects", context.ContextID.GetValueOrDefault())).Result;
            if (response.IsSuccessStatusCode)
            {
                context.Subjects.Clear();

                var subjects = response.Content.ReadAsAsync<IEnumerable<EAVModelLibrary.ModelSubject>>().Result;
                foreach (EAVModelLibrary.ModelSubject subject in subjects)
                {
                    subject.MarkUnmodified();

                    LoadSubjectEntity(subject);

                    context.Subjects.Add(subject);

                    subject.MarkUnmodified();
                }

                if (contextWasUnmodified)
                    context.MarkUnmodified();
            }
            else
            {
                throw (new ApplicationException("Attempt to get subjects failed."));
            }
        }

        public void LoadSubjects(EAV.Model.IModelEntity entity)
        {
            bool entityWasUnmodified = entity.ObjectState == EAV.Model.ObjectState.Unmodified;

            HttpResponseMessage response = client.GetAsync(String.Format("api/entities/{0}/subjects", entity.EntityID)).Result;
            if (response.IsSuccessStatusCode)
            {
                entity.Subjects.Clear();

                var subjects = response.Content.ReadAsAsync<IEnumerable<EAVModelLibrary.ModelSubject>>().Result;
                foreach (EAVModelLibrary.ModelSubject subject in subjects)
                {
                    subject.MarkUnmodified();

                    LoadSubjectContext(subject);

                    entity.Subjects.Add(subject);

                    subject.MarkUnmodified();
                }

                if (entityWasUnmodified)
                    entity.MarkUnmodified();
            }
            else
            {
                throw (new ApplicationException("Attempt to get subjects failed."));
            }
        }

        public void SaveSubject(EAV.Model.IModelSubject subject)
        {
            HttpResponseMessage response;

            if (subject.ObjectState == EAV.Model.ObjectState.New)
            {
                response = client.PostAsJsonAsync<EAV.Store.IStoreSubject>(String.Format("api/metadata/subjects"), subject).Result;
                if (response.IsSuccessStatusCode)
                {
                    EAVModelLibrary.ModelSubject newSubject = response.Content.ReadAsAsync<EAVModelLibrary.ModelSubject>().Result;

                    subject.SubjectID = newSubject.SubjectID;

                    subject.MarkUnmodified();
                }
                else
                {
                    throw (new ApplicationException("Attempt to create subject failed."));
                }
            }
            else if (subject.ObjectState == EAV.Model.ObjectState.Modified)
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

            if (subject.ObjectState == EAV.Model.ObjectState.Deleted)
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

        public void LoadRootContainers(EAV.Model.IModelContext context)
        {
            bool contextWasUnmodified = context.ObjectState == EAV.Model.ObjectState.Unmodified;

            HttpResponseMessage response = client.GetAsync(String.Format("api/metadata/contexts/{0}/containers", context.ContextID.GetValueOrDefault())).Result;
            if (response.IsSuccessStatusCode)
            {
                context.Containers.Clear();

                var rootContainers = response.Content.ReadAsAsync<IEnumerable<EAVModelLibrary.ModelRootContainer>>().Result;
                foreach (EAVModelLibrary.ModelRootContainer rootContainer in rootContainers)
                {
                    rootContainer.MarkUnmodified();

                    context.Containers.Add(rootContainer);

                    rootContainer.MarkUnmodified();
                }

                if (contextWasUnmodified)
                    context.MarkUnmodified();
            }
            else
            {
                throw (new ApplicationException("Attempt to get root containers failed."));
            }
        }

        public void LoadMetadata(EAV.Model.IModelRootContainer container)
        {
            try
            {
                bool containerWasUnmodified = container.ObjectState == EAV.Model.ObjectState.Unmodified;

                LoadAttributes(container);
                LoadChildContainers(container);

                if (containerWasUnmodified)
                    container.MarkUnmodified();
            }
            catch (Exception ex)
            {
                throw (new ApplicationException("Attempt to get metadata failed.", ex));
            }
        }

        public void SaveMetadata(EAV.Model.IModelRootContainer container)
        {
            HttpResponseMessage response;

            if (container.Context.ObjectState != EAV.Model.ObjectState.Unmodified)
                SaveContext(container.Context);

            if (container.ObjectState == EAV.Model.ObjectState.New)
            {
                response = client.PostAsJsonAsync<EAV.Store.IStoreContainer>(String.Format("api/metadata/contexts/{0}/containers", container.ContextID.GetValueOrDefault()), container).Result;
                if (response.IsSuccessStatusCode)
                {
                    EAVModelLibrary.ModelContainer newContainer = response.Content.ReadAsAsync<EAVModelLibrary.ModelRootContainer>().Result;

                    container.ContainerID = newContainer.ContainerID;

                    container.MarkUnmodified();
                }
                else
                {
                    throw (new ApplicationException("Attempt to create metadata failed."));
                }
            }
            else if (container.ObjectState == EAV.Model.ObjectState.Modified)
            {
                response = client.PatchAsJsonAsync<EAV.Store.IStoreContainer>("api/metadata/containers", container).Result;
                if (response.IsSuccessStatusCode)
                {
                    container.MarkUnmodified();
                }
                else
                {
                    throw (new ApplicationException("Attempt to update metadata failed."));
                }
            }

            foreach (EAVModelLibrary.ModelAttribute attribute in container.Attributes)
            {
                SaveAttribute(attribute);
            }

            foreach (EAVModelLibrary.ModelChildContainer childContainer in container.ChildContainers)
            {
                SaveChildContainer(childContainer);
            }

            if (container.ObjectState == EAV.Model.ObjectState.Deleted)
            {
                response = client.DeleteAsync(String.Format("api/metadata/containers/{0}", container.ContainerID.GetValueOrDefault())).Result;
                if (response.IsSuccessStatusCode)
                {
                }
                else
                {
                    throw (new ApplicationException("Attempt to delete metadata failed."));
                }
            }
        }

        public void LoadRootInstances(EAV.Model.IModelSubject subject, EAV.Model.IModelRootContainer container)
        {
            bool subjectWasUnmodified = subject.ObjectState == EAV.Model.ObjectState.Unmodified;
            bool containerWasUnmodified = container.ObjectState == EAV.Model.ObjectState.Unmodified;

            HttpResponseMessage response = client.GetAsync(String.Format("api/data/subjects/{0}/instances?container={1}", subject.SubjectID.GetValueOrDefault(), container != null ? container.ContainerID : null)).Result;
            if (response.IsSuccessStatusCode)
            {
                subject.Instances.Clear();

                var rootInstances = response.Content.ReadAsAsync<IEnumerable<EAVModelLibrary.ModelRootInstance>>().Result;
                foreach (EAVModelLibrary.ModelRootInstance rootInstance in rootInstances)
                {
                    rootInstance.MarkUnmodified();

                    rootInstance.Container = container;

                    if (container.Attributes.Any())
                        LoadValues(rootInstance, container.Attributes);

                    subject.Instances.Add(rootInstance);

                    rootInstance.MarkUnmodified();
                }

                if (containerWasUnmodified)
                    container.MarkUnmodified();

                if (subjectWasUnmodified)
                    subject.MarkUnmodified();
            }
            else
            {
                throw (new ApplicationException("Attempt to get root instances failed."));
            }
        }

        public void LoadData(EAV.Model.IModelRootInstance instance)
        {
            bool instanceWasUnmodified = instance.ObjectState == EAV.Model.ObjectState.Unmodified;

            HttpResponseMessage response = client.GetAsync(String.Format("api/data/instances/{0}/instances", instance.InstanceID.GetValueOrDefault())).Result;
            if (response.IsSuccessStatusCode)
            {
                instance.ChildInstances.Clear();

                var childInstances = response.Content.ReadAsAsync<IEnumerable<EAVModelLibrary.ModelChildInstance>>().Result;
                foreach (EAVModelLibrary.ModelChildInstance childInstance in childInstances)
                {
                    childInstance.MarkUnmodified();

                    EAV.Model.IModelContainer container = instance.Container.ChildContainers.Single(it => it.ContainerID == childInstance.ContainerID);

                    childInstance.Container = container;

                    if (container.Attributes.Any())
                        LoadValues(childInstance, container.Attributes);

                    if (container.ChildContainers.Any())
                        LoadChildInstances(childInstance, container.ChildContainers);

                    instance.ChildInstances.Add(childInstance);

                    childInstance.MarkUnmodified();
                }

                if (instanceWasUnmodified)
                    instance.MarkUnmodified();
            }
            else
            {
                throw (new ApplicationException("Attempt to load data failed."));
            }
        }

        public void SaveData(EAV.Model.IModelRootInstance instance)
        {
            HttpResponseMessage response;

            if (instance.ObjectState == EAV.Model.ObjectState.New)
            {
                response = client.PostAsJsonAsync<EAV.Store.IStoreInstance>(String.Format("api/data/subjects/{0}/instances?container={1}", instance.SubjectID.GetValueOrDefault(), instance.ContainerID), instance).Result;
                if (response.IsSuccessStatusCode)
                {
                    EAVModelLibrary.ModelInstance newInstance = response.Content.ReadAsAsync<EAVModelLibrary.ModelRootInstance>().Result;

                    instance.InstanceID = newInstance.InstanceID;

                    instance.MarkUnmodified();
                }
                else
                {
                    throw (new ApplicationException("Attempt to create data failed."));
                }
            }
            else if (instance.ObjectState == EAV.Model.ObjectState.Modified)
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

            foreach (EAVModelLibrary.ModelValue value in instance.Values)
            {
                SaveValue(value);
            }

            foreach (EAVModelLibrary.ModelChildInstance childInstance in instance.ChildInstances)
            {
                SaveChildInstance(childInstance);
            }

            if (instance.ObjectState == EAV.Model.ObjectState.Deleted)
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
