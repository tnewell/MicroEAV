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
using System.Web.Http;


namespace EAVWebService.Controllers
{
    [RoutePrefix("api/data/values")]
    public class ValueController : BaseEAVController
    {
        private EAV.Store.Clients.IValueStoreClient valueClient = factory.Create<EAV.Store.Clients.IValueStoreClient>();

        [HttpGet]
        [Route("~/api/data/instances/{instance}/values/{attribute}", Name = "RetrieveValue")]
        public IHttpActionResult RetrieveValue(int instance, int attribute)
        {
            try
            {
                return (Ok<EAV.Store.IStoreValue>(valueClient.RetrieveValue(attribute, instance)));
            }
            catch (Exception ex)
            {
                return (InternalServerError(ex));
            }
        }

        [HttpPatch]
        [Route("", Name = "UpdateValue")]
        public IHttpActionResult UpdateValue(EAV.Store.IStoreValue value)
        {
            try
            {
                valueClient.UpdateValue(value);

                return (Ok());
            }
            catch (Exception ex)
            {
                return (InternalServerError(ex));
            }
        }

        [HttpDelete]
        [Route("~/api/data/instances/{instance}/values/{attribute}", Name = "DeleteValue")]
        public IHttpActionResult DeleteValue(int instance, int attribute)
        {
            try
            {
                valueClient.DeleteValue(attribute, instance);

                return (Ok());
            }
            catch (Exception ex)
            {
                return (InternalServerError(ex));
            }
        }
    }
}
