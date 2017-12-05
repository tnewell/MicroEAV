using System;
using System.Collections.Generic;
using System.Web.Http;


namespace EAVService.Controllers
{
    [RoutePrefix("api/data/subjects")]
    public class SubjectController : BaseEAVController
    {
        private EAV.Store.IEAVSubjectClient subjectClient = new EAVStoreClient.EAVSubjectClient();
        private EAV.Store.IEAVInstanceClient instanceClient = new EAVStoreClient.EAVInstanceClient();

        public int? ContainerID
        {
            get
            {
                return (Int32.TryParse(QueryItem("container"), out int value) ? (int?)value : null);
            }
        }

        [HttpGet]
        [Route("{id}", Name = "RetrieveSubject")]
        public IHttpActionResult RetrieveSubject(int id)
        {
            try
            {
                return (Ok<EAV.Model.IEAVSubject>(subjectClient.RetrieveSubject(id)));
            }
            catch (Exception ex)
            {
                return (InternalServerError(ex));
            }
        }

        [HttpPatch]
        [Route("", Name = "UpdateSubject")]
        public IHttpActionResult UpdateSubject(EAV.Model.IEAVSubject subject)
        {
            try
            {
                subjectClient.UpdateSubject(subject);

                return (Ok());
            }
            catch (Exception ex)
            {
                return (InternalServerError(ex));
            }
        }

        [HttpDelete]
        [Route("{id}", Name = "DeleteSubject")]
        public IHttpActionResult DeleteSubject(int id)
        {
            try
            {
                subjectClient.DeleteSubject(id);

                return (Ok());
            }
            catch (Exception ex)
            {
                return (InternalServerError(ex));
            }
        }

        [HttpGet]
        [Route("{id}/instances", Name = "RetrieveRootInstances")]
        public IHttpActionResult RetrieveRootInstances(int id)
        {
            try
            {
                return (Ok<IEnumerable<EAV.Model.IEAVInstance>>(instanceClient.RetrieveRootInstances(ContainerID, id)));
            }
            catch (Exception ex)
            {
                return (InternalServerError(ex));
            }
        }

        [HttpPost]
        [Route("{id}/instances", Name = "CreateRootInstance")]
        public IHttpActionResult CreateRootInstance(int id, EAV.Model.IEAVInstance instance)
        {
            try
            {
                return (Ok<EAV.Model.IEAVInstance>(instanceClient.CreateRootInstance(instance, ContainerID.Value, id)));
            }
            catch (Exception ex)
            {
                return (InternalServerError(ex));
            }
        }
    }
}
