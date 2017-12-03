using System;
using System.Collections.Generic;
using System.Web.Http;


namespace EAVService.Controllers
{
    [RoutePrefix("api/data/entities")]
    public class EntityController : BaseEAVController
    {
        private EAV.Store.IEAVEntityClient entityClient = new EAVStoreClient.EAVEntityClient();
        private EAV.Store.IEAVSubjectClient subjectClient = new EAVStoreClient.EAVSubjectClient();

        public int? ContextID
        {
            get
            {
                int value = 0;
                return (Int32.TryParse(QueryItem("context"), out value) ? (int?)value : null);
            }
        }

        [HttpGet]
        [Route("", Name = "RetrieveEntities")]
        public IHttpActionResult RetrieveEntities()
        {
            try
            {
                return (Ok<IEnumerable<EAV.Model.IEAVEntity>>(entityClient.RetrieveEntities()));
            }
            catch (Exception ex)
            {
                return (InternalServerError(ex));
            }
        }

        [HttpGet]
        [Route("{id}", Name = "RetrieveEntity")]
        public IHttpActionResult RetrieveEntity(int id)
        {
            try
            {
                return (Ok<EAV.Model.IEAVEntity>(entityClient.RetrieveEntity(id)));
            }
            catch (Exception ex)
            {
                return (InternalServerError(ex));
            }
        }

        [HttpPost]
        [Route("", Name = "CreateEntity")]
        public IHttpActionResult CreateEntity(EAV.Model.IEAVEntity entity)
        {
            try
            {
                return (Ok<EAV.Model.IEAVEntity>(entityClient.CreateEntity(entity)));
            }
            catch (Exception ex)
            {
                return (InternalServerError(ex));
            }
        }

        [HttpPatch]
        [Route("", Name = "UpdateEntity")]
        public IHttpActionResult UpdateEntity(EAV.Model.IEAVEntity entity)
        {
            try
            {
                entityClient.UpdateEntity(entity);

                return (Ok());
            }
            catch (Exception ex)
            {
                return (InternalServerError(ex));
            }
        }

        [HttpDelete]
        [Route("{id}", Name = "DeleteEntity")]
        public IHttpActionResult DeleteEntity(int id)
        {
            try
            {
                entityClient.DeleteEntity(id);

                return (Ok());
            }
            catch (Exception ex)
            {
                return (InternalServerError(ex));
            }
        }

        [HttpGet]
        [Route("{id}/subjects", Name = "RetrieveSubjects")]
        public IHttpActionResult RetrieveSubjects(int id)
        {
            try
            {
                return (Ok<IEnumerable<EAV.Model.IEAVSubject>>(subjectClient.RetrieveSubjects(ContextID, id)));
            }
            catch (Exception ex)
            {
                return (InternalServerError(ex));
            }
        }

        [HttpPost]
        [Route("{id}/subjects", Name = "CreateSubject")]
        public IHttpActionResult CreateSubject(int id, EAV.Model.IEAVSubject subject)
        {
            try
            {
                return (Ok<EAV.Model.IEAVSubject>(subjectClient.CreateSubject(subject, ContextID.Value, id)));
            }
            catch (Exception ex)
            {
                return (InternalServerError(ex));
            }
        }
    }
}
