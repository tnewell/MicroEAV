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
using System.Web.Http;


namespace EAVWebService.Controllers
{
    [RoutePrefix("api/metadata/contexts")]
    public class ContextController : BaseEAVController
    {
        private EAV.Store.Clients.IContextStoreClient contextClient = clientFactory.Create<EAV.Store.Clients.IContextStoreClient>();
        private EAV.Store.Clients.IContainerStoreClient containerClient = clientFactory.Create<EAV.Store.Clients.IContainerStoreClient>();
        private EAV.Store.Clients.ISubjectStoreClient subjectClient = clientFactory.Create<EAV.Store.Clients.ISubjectStoreClient>();

        public int? EntityID
        {
            get
            {
                return (Int32.TryParse(QueryItem("entity"), out int value) ? (int?) value : null);
            }
        }

        [HttpGet]
        [Route("", Name = "RetrieveContexts")]
        public IHttpActionResult RetrieveContexts()
        {
            try
            {
                return (Ok<IEnumerable<EAV.Store.IStoreContext>>(contextClient.RetrieveContexts()));
            }
            catch (Exception ex)
            {
                return (InternalServerError(ex));
            }
        }

        [HttpGet]
        [Route("{id:int}", Name = "RetrieveContext")]
        public IHttpActionResult RetrieveContext(int id)
        {
            try
            {
                return (Ok<EAV.Store.IStoreContext>(contextClient.RetrieveContext(id)));
            }
            catch (Exception ex)
            {
                return (InternalServerError(ex));
            }
        }

        [HttpGet]
        [Route("{name}", Name = "RetrieveContextByName")]
        public IHttpActionResult RetrieveContext(string name)
        {
            try
            {
                return (Ok<EAV.Store.IStoreContext>(contextClient.RetrieveContext(name)));
            }
            catch (Exception ex)
            {
                return (InternalServerError(ex));
            }
        }

        [HttpPost]
        [Route("", Name = "CreateContext")]
        public IHttpActionResult CreateContext(EAV.Store.IStoreContext context)
        {
            try
            {
                return (Ok<EAV.Store.IStoreContext>(contextClient.CreateContext(context)));
            }
            catch (Exception ex)
            {
                return (InternalServerError(ex));
            }
        }

        [HttpPatch]
        [Route("", Name = "UpdateContext")]
        public IHttpActionResult UpdateContext(EAV.Store.IStoreContext context)
        {
            try
            {
                contextClient.UpdateContext(context);

                return (Ok());
            }
            catch (Exception ex)
            {
                return (InternalServerError(ex));
            }
        }

        [HttpDelete]
        [Route("{id}", Name = "DeleteContext")]
        public IHttpActionResult DeleteContext(int id)
        {
            try
            {
                contextClient.DeleteContext(id);

                return (Ok());
            }
            catch (Exception ex)
            {
                return (InternalServerError(ex));
            }
        }

        [HttpGet]
        [Route("{id}/containers", Name = "RetrieveRootContainers")]
        public IHttpActionResult RetrieveRootContainers(int id)
        {
            try
            {
                return (Ok<IEnumerable<EAV.Store.IStoreContainer>>(containerClient.RetrieveRootContainers(id)));
            }
            catch (Exception ex)
            {
                return (InternalServerError(ex));
            }
        }

        [HttpPost]
        [Route("{id}/containers", Name = "CreateRootContainer")]
        public IHttpActionResult CreateRootContainer(int id, EAV.Store.IStoreContainer container)
        {
            try
            {
                return (Ok<EAV.Store.IStoreContainer>(containerClient.CreateRootContainer(container, id)));
            }
            catch (Exception ex)
            {
                return (InternalServerError(ex));
            }
        }

        [HttpGet]
        [Route("{id}/subjects", Name = "RetrieveContextSubjects")]
        public IHttpActionResult RetrieveContextSubjects(int id)
        {
            try
            {
                return (Ok<IEnumerable<EAV.Store.IStoreSubject>>(subjectClient.RetrieveSubjects(id, EntityID)));
            }
            catch (Exception ex)
            {
                return (InternalServerError(ex));
            }
        }

        [HttpPost]
        [Route("{id}/subjects", Name = "CreateContextSubject")]
        public IHttpActionResult CreateContextSubject(int id, EAV.Store.IStoreSubject subject)
        {
            try
            {
                return (Ok<EAV.Store.IStoreSubject>(subjectClient.CreateSubject(subject, id, EntityID.GetValueOrDefault())));
            }
            catch (Exception ex)
            {
                return (InternalServerError(ex));
            }
        }
    }
}
