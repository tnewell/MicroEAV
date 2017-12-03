using System;
using System.Web.Http;


namespace EAVService.Controllers
{
    [RoutePrefix("api/meta/attributes")]
    public class AttributeController : BaseEAVController
    {
        private EAV.Store.IEAVAttributeClient attributeClient = new EAVStoreClient.EAVAttributeClient();

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
    }
}
