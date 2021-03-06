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
using System.Linq;
using System.Net.Http;
using System.Web.Http;


namespace EAVWebService.Controllers
{
    public class BaseEAVController : ApiController
    {
        protected static readonly EAV.Store.Clients.IStoreClientFactory factory = new EAVStoreClient.StoreClientFactory();

        protected string QueryItem(string name)
        {
            var queryItems = this.Request.GetQueryNameValuePairs();

            if (queryItems == null || !queryItems.Any())
                return (null);

            return(queryItems.Where(it => String.Equals(it.Key, name, StringComparison.InvariantCultureIgnoreCase)).Select(it => it.Value).LastOrDefault());
        }
    }
}
