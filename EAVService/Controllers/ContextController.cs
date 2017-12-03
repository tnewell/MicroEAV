using System;
using System.Collections.Generic;
using System.Web.Http;


namespace EAVService.Controllers
{
    [RoutePrefix("api/meta/contexts")]
    public class ContextController : BaseEAVController
    {
        private EAV.Store.IEAVContextClient contextClient = new EAVStoreClient.EAVContextClient();
        private EAV.Store.IEAVContainerClient containerClient = new EAVStoreClient.EAVContainerClient();

        [HttpGet]
        [Route("", Name = "RetrieveContexts")]
        public IHttpActionResult RetrieveContexts()
        {
            try
            {
                return (Ok<IEnumerable<EAV.Model.IEAVContext>>(contextClient.RetrieveContexts()));
            }
            catch (Exception ex)
            {
                return (InternalServerError(ex));
            }
        }

        [HttpGet]
        [Route("{id:int}", Name = "RetrieveContext")]
        public IHttpActionResult RetrieveContext(int id)
        {
            try
            {
                return (Ok<EAV.Model.IEAVContext>(contextClient.RetrieveContext(id)));
            }
            catch (Exception ex)
            {
                return (InternalServerError(ex));
            }
        }

        [HttpGet]
        [Route("{name}", Name = "RetrieveContextByName")]
        public IHttpActionResult RetrieveContext(string name)
        {
            try
            {
                return (Ok<EAV.Model.IEAVContext>(contextClient.RetrieveContext(name)));
            }
            catch (Exception ex)
            {
                return (InternalServerError(ex));
            }
        }

        [HttpPost]
        [Route("", Name = "CreateContext")]
        public IHttpActionResult CreateContext(EAV.Model.IEAVContext context)
        {
            try
            {
                return (Ok<EAV.Model.IEAVContext>(contextClient.CreateContext(context)));
            }
            catch (Exception ex)
            {
                return (InternalServerError(ex));
            }
        }

        [HttpPatch]
        [Route("", Name = "UpdateContext")]
        public IHttpActionResult UpdateContext(EAV.Model.IEAVContext context)
        {
            try
            {
                contextClient.UpdateContext(context);

                return (Ok());
            }
            catch (Exception ex)
            {
                return (InternalServerError(ex));
            }
        }

        [HttpDelete]
        [Route("{id}", Name = "DeleteContext")]
        public IHttpActionResult DeleteContext(int id)
        {
            try
            {
                contextClient.DeleteContext(id);

                return (Ok());
            }
            catch (Exception ex)
            {
                return (InternalServerError(ex));
            }
        }

        [HttpGet]
        [Route("{id}/containers", Name = "RetrieveRootContainers")]
        public IHttpActionResult RetrieveRootContainers(int id)
        {
            try
            {
                return (Ok<IEnumerable<EAV.Model.IEAVContainer>>(containerClient.RetrieveRootContainers(id)));
            }
            catch (Exception ex)
            {
                return (InternalServerError(ex));
            }
        }

        [HttpPost]
        [Route("{id}/containers", Name = "CreateRootContainer")]
        public IHttpActionResult CreateRootContainer(int id, EAV.Model.IEAVContainer container)
        {
            try
            {
                var x = containerClient.CreateRootContainer(container, id);

                return (Ok<EAV.Model.IEAVContainer>(x));
            }
            catch (Exception ex)
            {
                return (InternalServerError(ex));
            }
        }
    }
}
