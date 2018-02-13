// MicroEAV
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
