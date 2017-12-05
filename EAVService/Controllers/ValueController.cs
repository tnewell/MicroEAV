using System;
using System.Web.Http;


namespace EAVService.Controllers
{
    [RoutePrefix("api/data/values")]
    public class ValueController : BaseEAVController
    {
        private EAV.Store.IEAVValueClient valueClient = new EAVStoreClient.EAVValueClient();

        [HttpGet]
        [Route("~/api/data/instances/{instance}/values/{attribute}", Name = "RetrieveValue")]
        public IHttpActionResult RetrieveValue(int instance, int attribute)
        {
            try
            {
                return (Ok<EAV.Model.IEAVValue>(valueClient.RetrieveValue(attribute, instance)));
            }
            catch (Exception ex)
            {
                return (InternalServerError(ex));
            }
        }

        [HttpPatch]
        [Route("", Name = "UpdateValue")]
        public IHttpActionResult UpdateValue(EAV.Model.IEAVValue value)
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
