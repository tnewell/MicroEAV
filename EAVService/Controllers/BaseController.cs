using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using EAV.Model;
using Newtonsoft.Json.Converters;


namespace EAVService.Controllers
{
    public class BaseEAVController : ApiController
    {
        protected string QueryItem(string name)
        {
            var queryItems = this.Request.GetQueryNameValuePairs();

            if (queryItems == null || !queryItems.Any())
                return (null);

            return(queryItems.Where(it => String.Equals(it.Key, name, StringComparison.InvariantCultureIgnoreCase)).Select(it => it.Value).LastOrDefault());
        }
    }
}
