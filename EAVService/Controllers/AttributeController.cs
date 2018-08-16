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
    [RoutePrefix("api/meta/attributes")]
    public class AttributeController : BaseEAVController
    {
        private EAV.Store.IEAVAttributeClient attributeClient = new EAVStoreClient.EAVAttributeClient();
        private EAV.Store.IEAVUnitClient unitClient = new EAVStoreClient.EAVUnitClient();

        [HttpGet]
        [Route("{id}", Name = "RetrieveAttribute")]
        public IHttpActionResult RetrieveAttribute(int id)
        {
            try
            {
                return (Ok<EAV.Model.IEAVAttribute>(attributeClient.RetrieveAttribute(id)));
            }
            catch (Exception ex)
            {
                return (InternalServerError(ex));
            }
        }

        [HttpPatch]
        [Route("", Name = "UpdateAttribute")]
        public IHttpActionResult UpdateAttribute(EAV.Model.IEAVAttribute attribute)
        {
            try
            {
                attributeClient.UpdateAttribute(attribute);

                return (Ok());
            }
            catch (Exception ex)
            {
                return (InternalServerError(ex));
            }
        }

        [HttpDelete]
        [Route("{id}", Name = "DeleteAttribute")]
        public IHttpActionResult DeleteAttribute(int id)
        {
            try
            {
                attributeClient.DeleteAttribute(id);

                return (Ok());
            }
            catch (Exception ex)
            {
                return (InternalServerError(ex));
            }
        }

        [HttpGet]
        [Route("{id}/units", Name = "RetrieveAttributeUnits")]
        public IHttpActionResult RetrieveAttributeUnits(int id)
        {
            try
            {
                return (Ok<IEnumerable<EAV.Model.IEAVUnit>>(attributeClient.RetrieveAttributeUnits(id)));
            }
            catch (Exception ex)
            {
                return (InternalServerError(ex));
            }
        }

        [HttpPatch]
        [Route("{id}/units", Name = "UpdateAttributeUnits")]
        public IHttpActionResult UpdatettributeUnits(int id, IEnumerable<EAV.Model.IEAVUnit> units)
        {
            try
            {
                attributeClient.UpdateAttributeUnits(id, units);

                return (Ok());
            }
            catch (Exception ex)
            {
                return (InternalServerError(ex));
            }
        }
    }
}
