﻿// MicroEAV
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
    [RoutePrefix("api/data/subjects")]
    public class SubjectController : BaseEAVController
    {
        private EAV.Store.IEAVSubjectClient subjectClient = new EAVStoreClient.EAVSubjectClient();
        private EAV.Store.IEAVInstanceClient instanceClient = new EAVStoreClient.EAVInstanceClient();

        public int? ContainerID
        {
            get
            {
                return (Int32.TryParse(QueryItem("container"), out int value) ? (int?)value : null);
            }
        }

        [HttpGet]
        [Route("{id}", Name = "RetrieveSubject")]
        public IHttpActionResult RetrieveSubject(int id)
        {
            try
            {
                return (Ok<EAV.Model.IEAVSubject>(subjectClient.RetrieveSubject(id)));
            }
            catch (Exception ex)
            {
                return (InternalServerError(ex));
            }
        }

        [HttpPatch]
        [Route("", Name = "UpdateSubject")]
        public IHttpActionResult UpdateSubject(EAV.Model.IEAVSubject subject)
        {
            try
            {
                subjectClient.UpdateSubject(subject);

                return (Ok());
            }
            catch (Exception ex)
            {
                return (InternalServerError(ex));
            }
        }

        [HttpDelete]
        [Route("{id}", Name = "DeleteSubject")]
        public IHttpActionResult DeleteSubject(int id)
        {
            try
            {
                subjectClient.DeleteSubject(id);

                return (Ok());
            }
            catch (Exception ex)
            {
                return (InternalServerError(ex));
            }
        }

        [HttpGet]
        [Route("{id}/instances", Name = "RetrieveRootInstances")]
        public IHttpActionResult RetrieveRootInstances(int id)
        {
            try
            {
                return (Ok<IEnumerable<EAV.Model.IEAVInstance>>(instanceClient.RetrieveRootInstances(ContainerID, id)));
            }
            catch (Exception ex)
            {
                return (InternalServerError(ex));
            }
        }

        [HttpPost]
        [Route("{id}/instances", Name = "CreateRootInstance")]
        public IHttpActionResult CreateRootInstance(int id, EAV.Model.IEAVInstance instance)
        {
            try
            {
                return (Ok<EAV.Model.IEAVInstance>(instanceClient.CreateRootInstance(instance, ContainerID.GetValueOrDefault(), id)));
            }
            catch (Exception ex)
            {
                return (InternalServerError(ex));
            }
        }
    }
}
