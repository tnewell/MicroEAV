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
    [RoutePrefix("api/meta/containers")]
    public class ContainerController : BaseEAVController
    {
        private EAV.Store.IEAVContainerClient containerClient = new EAVStoreClient.EAVContainerClient();
        private EAV.Store.IEAVAttributeClient attributeClient = new EAVStoreClient.EAVAttributeClient();

        [HttpGet]
        [Route("{id}", Name = "RetrieveContainer")]
        public IHttpActionResult RetrieveContainer(int id)
        {
            try
            {
                return (Ok<EAV.Model.IEAVContainer>(containerClient.RetrieveContainer(id)));
            }
            catch (Exception ex)
            {
                return (InternalServerError(ex));
            }
        }

        [HttpPatch]
        [Route("", Name = "UpdateContainer")]
        public IHttpActionResult UpdateContainer(EAV.Model.IEAVContainer container)
        {
            try
            {
                containerClient.UpdateContainer(container);

                return (Ok());
            }
            catch (Exception ex)
            {
                return (InternalServerError(ex));
            }
        }

        [HttpDelete]
        [Route("{id}", Name = "DeleteContainer")]
        public IHttpActionResult DeleteContainer(int id)
        {
            try
            {
                containerClient.DeleteContainer(id);

                return (Ok());
            }
            catch (Exception ex)
            {
                return (InternalServerError(ex));
            }
        }

        [HttpGet]
        [Route("{id}/containers", Name = "RetrieveChildContainers")]
        public IHttpActionResult RetrieveChildContainers(int id)
        {
            try
            {
                return (Ok<IEnumerable<EAV.Model.IEAVContainer>>(containerClient.RetrieveChildContainers(id)));
            }
            catch (Exception ex)
            {
                return (InternalServerError(ex));
            }
        }

        [HttpPost]
        [Route("{id}/containers", Name = "CreateChildContainer")]
        public IHttpActionResult CreateChildContainer(int id, EAV.Model.IEAVContainer container)
        {
            try
            {
                return (Ok<EAV.Model.IEAVContainer>(containerClient.CreateChildContainer(container, id)));
            }
            catch (Exception ex)
            {
                return (InternalServerError(ex));
            }
        }

        [HttpGet]
        [Route("{id}/attributes", Name = "RetrieveAttributes")]
        public IHttpActionResult RetrieveAttributes(int id)
        {
            try
            {
                return (Ok<IEnumerable<EAV.Model.IEAVAttribute>>(attributeClient.RetrieveAttributes(id)));
            }
            catch (Exception ex)
            {
                return (InternalServerError(ex));
            }
        }

        [HttpPost]
        [Route("{id}/attributes", Name = "CreateAttribute")]
        public IHttpActionResult CreateAttribute(int id, EAV.Model.IEAVAttribute attribute)
        {
            try
            {
                return (Ok<EAV.Model.IEAVAttribute>(attributeClient.CreateAttribute(attribute, id)));
            }
            catch (Exception ex)
            {
                return (InternalServerError(ex));
            }
        }
    }
}
