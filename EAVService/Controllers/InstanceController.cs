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

        // TODO: DELETE

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
                return (Ok<EAV.Model.IEAVInstance>(instanceClient.CreateChildInstance(instance, ContainerID.Value, id)));
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
