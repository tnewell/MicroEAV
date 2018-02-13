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
    [RoutePrefix("api/data/instances")]
    public class InstanceController : BaseEAVController
    {
        private EAV.Store.IEAVInstanceClient instanceClient = new EAVStoreClient.EAVInstanceClient();
        private EAV.Store.IEAVValueClient valueClient = new EAVStoreClient.EAVValueClient();

        public int? ContainerID
        {
            get
            {
                int value = 0;
                return (Int32.TryParse(QueryItem("container"), out value) ? (int?)value : null);
            }
        }
        public int? AttributeID
        {
            get
            {
                int value = 0;
                return (Int32.TryParse(QueryItem("attribute"), out value) ? (int?)value : null);
            }
        }

        [HttpGet]
        [Route("{id}", Name = "RetrieveInstance")]
        public IHttpActionResult RetrieveInstance(int id)
        {
            try
            {
                return (Ok<EAV.Model.IEAVInstance>(instanceClient.RetrieveInstance(id)));
            }
            catch (Exception ex)
            {
                return (InternalServerError(ex));
            }
        }

        [HttpPatch]
        [Route("", Name = "UpdateInstance")]
        public IHttpActionResult UpdateInstance(EAV.Model.IEAVInstance instance)
        {
            try
            {
                instanceClient.UpdateInstance(instance);

                return (Ok());
            }
            catch (Exception ex)
            {
                return (InternalServerError(ex));
            }
        }

        [HttpDelete]
        [Route("{id}", Name = "DeleteInstance")]
        public IHttpActionResult DeleteInstance(int id)
        {
            try
            {
                instanceClient.DeleteInstance(id);

                return (Ok());
            }
            catch (Exception ex)
            {
                return (InternalServerError(ex));
            }
        }

        [HttpGet]
        [Route("{id}/instances", Name = "RetrieveChildInstances")]
        public IHttpActionResult RetrieveChildInstances(int id)
        {
            try
            {
                return (Ok<IEnumerable<EAV.Model.IEAVInstance>>(instanceClient.RetrieveChildInstances(ContainerID, id)));
            }
            catch (Exception ex)
            {
                return (InternalServerError(ex));
            }
        }

        [HttpPost]
        [Route("{id}/instances", Name = "CreateChildInstance")]
        public IHttpActionResult CreateChildInstance(int id, EAV.Model.IEAVInstance instance)
        {
            try
            {
                return (Ok<EAV.Model.IEAVInstance>(instanceClient.CreateChildInstance(instance, ContainerID.GetValueOrDefault(), id)));
            }
            catch (Exception ex)
            {
                return (InternalServerError(ex));
            }
        }

        [HttpGet]
        [Route("{id}/values", Name = "RetrieveValues")]
        public IHttpActionResult RetrieveValues(int id)
        {
            try
            {
                return (Ok<IEnumerable<EAV.Model.IEAVValue>>(valueClient.RetrieveValues(AttributeID, id)));
            }
            catch (Exception ex)
            {
                return (InternalServerError(ex));
            }
        }

        [HttpPost]
        [Route("{id}/values", Name = "CreateValue")]
        public IHttpActionResult CreateValue(int id, EAV.Model.IEAVValue value)
        {
            try
            {
                return (Ok<EAV.Model.IEAVValue>(valueClient.CreateValue(value, id, AttributeID.GetValueOrDefault())));
            }
            catch (Exception ex)
            {
                return (InternalServerError(ex));
            }
        }
    }
}
