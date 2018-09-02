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
    public partial class EntityStoreClient : EAV.Store.Clients.IEntityStoreClient
    {
        public IEnumerable<EAV.Store.IStoreEntity> RetrieveEntities()
        {
            using (EAVStoreClient.MicroEAVContext ctx = new MicroEAVContext())
            {
                return (ctx.Entities.AsEnumerable().Select(it => (EAVStoreLibrary.StoreEntity)it).ToList());
            }
        }

        public EAV.Store.IStoreEntity RetrieveEntity(int entityID)
        {
            using (EAVStoreClient.MicroEAVContext ctx = new MicroEAVContext())
            {
                return ((EAVStoreLibrary.StoreEntity)ctx.Entities.SingleOrDefault(it => it.Entity_ID == entityID));
            }
        }

        public EAV.Store.IStoreEntity CreateEntity(EAV.Store.IStoreEntity anEntity)
        {
            if (anEntity == null)
                return (null);

            if (string.IsNullOrWhiteSpace(anEntity.Descriptor))
                throw (new InvalidOperationException("Property 'Name' for parameter 'entity' may not be null or empty."));

            using (EAVStoreClient.MicroEAVContext ctx = new MicroEAVContext())
            {
                Entity dbEntity = new Entity(anEntity);

                ctx.Entities.Add(dbEntity);

                ctx.SaveChanges();

                return ((EAVStoreLibrary.StoreEntity)dbEntity);
            }
        }

        public void UpdateEntity(EAV.Store.IStoreEntity anEntity)
        {
            using (EAVStoreClient.MicroEAVContext ctx = new MicroEAVContext())
            {
                EAVStoreClient.Entity dbEntity = ctx.Entities.SingleOrDefault(it => it.Entity_ID == anEntity.EntityID);

                if (dbEntity != null)
                {
                    if (dbEntity.Descriptor != anEntity.Descriptor)
                        dbEntity.Descriptor = anEntity.Descriptor;

                    ctx.SaveChanges();
                }
                else
                    throw (new Exception(String.Format("Unable to retrieve entity ID {0}.", anEntity.EntityID)));
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
