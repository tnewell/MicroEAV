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


namespace EAVService.Controllers
{
    [RoutePrefix("api/data/entities")]
    public class EntityController : BaseEAVController
    {
        private EAV.Store.Clients.IEntityStoreClient entityClient = new EAVStoreClient.EntityStoreClient();
        private EAV.Store.Clients.ISubjectStoreClient subjectClient = new EAVStoreClient.SubjectStoreClient();

        public int? ContextID
        {
            get
            {
                return (Int32.TryParse(QueryItem("context"), out int value) ? (int?)value : null);
            }
        }

        [HttpGet]
        [Route("", Name = "RetrieveEntities")]
        public IHttpActionResult RetrieveEntities()
        {
            try
            {
                return (Ok<IEnumerable<EAV.Store.IStoreEntity>>(entityClient.RetrieveEntities()));
            }
            catch (Exception ex)
            {
                return (InternalServerError(ex));
            }
        }

        [HttpGet]
        [Route("{id}", Name = "RetrieveEntity")]
        public IHttpActionResult RetrieveEntity(int id)
        {
            try
            {
                return (Ok<EAV.Store.IStoreEntity>(entityClient.RetrieveEntity(id)));
            }
            catch (Exception ex)
            {
                return (InternalServerError(ex));
            }
        }

        [HttpPost]
        [Route("", Name = "CreateEntity")]
        public IHttpActionResult CreateEntity(EAV.Store.IStoreEntity entity)
        {
            try
            {
                return (Ok<EAV.Store.IStoreEntity>(entityClient.CreateEntity(entity)));
            }
            catch (Exception ex)
            {
                return (InternalServerError(ex));
            }
        }

        [HttpPatch]
        [Route("", Name = "UpdateEntity")]
        public IHttpActionResult UpdateEntity(EAV.Store.IStoreEntity entity)
        {
            try
            {
                entityClient.UpdateEntity(entity);

                return (Ok());
            }
            catch (Exception ex)
            {
                return (InternalServerError(ex));
            }
        }

        [HttpDelete]
        [Route("{id}", Name = "DeleteEntity")]
        public IHttpActionResult DeleteEntity(int id)
        {
            try
            {
                entityClient.DeleteEntity(id);

                return (Ok());
            }
            catch (Exception ex)
            {
                return (InternalServerError(ex));
            }
        }

        [HttpGet]
        [Route("{id}/subjects", Name = "RetrieveEntitySubjects")]
        public IHttpActionResult RetrieveEntitySubjects(int id)
        {
            try
            {
                return (Ok<IEnumerable<EAV.Store.IStoreSubject>>(subjectClient.RetrieveSubjects(ContextID, id)));
            }
            catch (Exception ex)
            {
                return (InternalServerError(ex));
            }
        }

        [HttpPost]
        [Route("{id}/subjects", Name = "CreateEntitySubject")]
        public IHttpActionResult CreateEntitySubject(int id, EAV.Store.IStoreSubject subject)
        {
            try
            {
                return (Ok<EAV.Store.IStoreSubject>(subjectClient.CreateSubject(subject, ContextID.GetValueOrDefault(), id)));
            }
            catch (Exception ex)
            {
                return (InternalServerError(ex));
            }
        }
    }
}
