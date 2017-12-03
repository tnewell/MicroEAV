using System;
using System.Collections.Generic;
using System.Linq;


namespace EAVStoreClient
{
    public partial class EAVEntityClient : EAV.Store.IEAVEntityClient
    {
        public IEnumerable<EAV.Model.IEAVEntity> RetrieveEntities()
        {
            using (EAVStoreClient.MicroEAVContext ctx = new MicroEAVContext())
            {
                return (ctx.Entities.AsEnumerable().Select(it => (EAV.Model.BaseEAVEntity)it).ToList());
            }
        }

        public EAV.Model.IEAVEntity RetrieveEntity(int entityID)
        {
            using (EAVStoreClient.MicroEAVContext ctx = new MicroEAVContext())
            {
                return ((EAV.Model.BaseEAVEntity)ctx.Entities.SingleOrDefault(it => it.Entity_ID == entityID));
            }
        }

        public EAV.Model.IEAVEntity CreateEntity(EAV.Model.IEAVEntity entity)
        {
            if (entity == null)
                return (null);

            if (string.IsNullOrWhiteSpace(entity.Descriptor))
                throw (new InvalidOperationException("Property 'Name' for parameter 'entity' may not be null or empty."));

            using (EAVStoreClient.MicroEAVContext ctx = new MicroEAVContext())
            {
                Entity dbEntity = new Entity(entity);

                ctx.Entities.Add(dbEntity);

                ctx.SaveChanges();

                return ((EAV.Model.BaseEAVEntity)dbEntity);
            }
        }

        public void UpdateEntity(EAV.Model.IEAVEntity entity)
        {
            using (EAVStoreClient.MicroEAVContext ctx = new MicroEAVContext())
            {
                EAVStoreClient.Entity dbEntity = ctx.Entities.SingleOrDefault(it => it.Entity_ID == entity.EntityID);

                if (dbEntity != null)
                {
                    if (dbEntity.Descriptor != entity.Descriptor)
                        dbEntity.Descriptor = entity.Descriptor;

                    ctx.SaveChanges();
                }
                else
                    throw (new Exception(String.Format("Unable to retrieve entity ID {0}.", entity.EntityID)));
            }
        }

        public void DeleteEntity(int entityID)
        {
            using (EAVStoreClient.MicroEAVContext ctx = new MicroEAVContext())
            {
                var dbEntity = ctx.Entities.SingleOrDefault(it => it.Entity_ID == entityID);

                if (dbEntity != null)
                {
                    ctx.Entities.Remove(dbEntity);

                    ctx.SaveChanges();
                }
            }
        }
    }
}
