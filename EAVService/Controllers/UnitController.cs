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
    [RoutePrefix("api/metadata/units")]
    public class UnitController : BaseEAVController
    {
        private EAV.Store.IEAVUnitClient unitClient = new EAVStoreClient.EAVUnitClient();

        [HttpGet]
        [Route("", Name = "RetrieveUnits")]
        public IHttpActionResult RetrieveUnits()
        {
            try
            {
                return (Ok<IEnumerable<EAV.Model.IEAVUnit>>(unitClient.RetrieveUnits()));
            }
            catch (Exception ex)
            {
                return (InternalServerError(ex));
            }
        }

        [HttpGet]
        [Route("{id}", Name = "RetrieveUnit")]
        public IHttpActionResult RetrieveUnit(int id)
        {
            try
            {
                return (Ok<EAV.Model.IEAVUnit>(unitClient.RetrieveUnit(id)));
            }
            catch (Exception ex)
            {
                return (InternalServerError(ex));
            }
        }

        [HttpPost]
        [Route("", Name = "CreateUnit")]
        public IHttpActionResult CreateUnit(EAV.Model.IEAVUnit unit)
        {
            try
            {
                return (Ok<EAV.Model.IEAVUnit>(unitClient.CreateUnit(unit)));
            }
            catch (Exception ex)
            {
                return (InternalServerError(ex));
            }
        }

        [HttpPatch]
        [Route("", Name = "UpdateUnit")]
        public IHttpActionResult UpdateUnit(EAV.Model.IEAVUnit unit)
        {
            try
            {
                unitClient.UpdateUnit(unit);

                return (Ok());
            }
            catch (Exception ex)
            {
                return (InternalServerError(ex));
            }
        }

        [HttpDelete]
        [Route("{id}", Name = "DeleteUnit")]
        public IHttpActionResult DeleteUnit(int id)
        {
            try
            {
                unitClient.DeleteUnit(id);

                return (Ok());
            }
            catch (Exception ex)
            {
                return (InternalServerError(ex));
            }
        }
    }
}
