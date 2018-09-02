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
    public partial class InstanceStoreClient : EAV.Store.Clients.IInstanceStoreClient
    {
        public IEnumerable<EAV.Store.IStoreInstance> RetrieveRootInstances(int? containerID, int? subjectID)
        {
            using (EAVStoreClient.MicroEAVContext ctx = new MicroEAVContext())
            {
                if (containerID != null && subjectID != null)
                {
                    return (ctx.Instances.Where(it => it.Container_ID == containerID && it.Subject_ID == subjectID && it.Parent_Instance_ID == null).AsEnumerable().Select(it => (EAV.Store.StoreInstance)it).ToList());
                }
                else if (containerID != null && subjectID == null)
                {
                    return (ctx.Instances.Where(it => it.Container_ID == containerID && it.Parent_Instance_ID == null).AsEnumerable().Select(it => (EAV.Store.StoreInstance)it).ToList());
                }
                else if (containerID == null && subjectID != null)
                {
                    return (ctx.Instances.Where(it => it.Subject_ID == subjectID && it.Parent_Instance_ID == null).AsEnumerable().Select(it => (EAV.Store.StoreInstance)it).ToList());
                }
                else
                {
                    return (ctx.Instances.Where(it => it.Parent_Instance_ID == null).AsEnumerable().Select(it => (EAV.Store.StoreInstance)it).ToList());
                }
            }
        }

        public IEnumerable<EAV.Store.IStoreInstance> RetrieveChildInstances(int? containerID, int? parentInstanceID)
        {
            using (EAVStoreClient.MicroEAVContext ctx = new MicroEAVContext())
            {
                if (containerID != null && parentInstanceID != null)
                {
                    return (ctx.Instances.Where(it => it.Container_ID == containerID && it.Parent_Instance_ID == parentInstanceID).AsEnumerable().Select(it => (EAV.Store.StoreInstance)it).ToList());
                }
                else if (containerID != null && parentInstanceID == null)
                {
                    return (ctx.Instances.Where(it => it.Container_ID == containerID && it.Parent_Instance_ID != null).AsEnumerable().Select(it => (EAV.Store.StoreInstance)it).ToList());
                }
                else if (containerID == null && parentInstanceID != null)
                {
                    return (ctx.Instances.Where(it => it.Parent_Instance_ID == parentInstanceID).AsEnumerable().Select(it => (EAV.Store.StoreInstance)it).ToList());
                }
                else
                {
                    return (ctx.Instances.Where(it => it.Parent_Instance_ID != null).AsEnumerable().Select(it => (EAV.Store.StoreInstance)it).ToList());
                }
            }
        }

        public EAV.Store.IStoreInstance RetrieveInstance(int instanceID)
        {
            using (EAVStoreClient.MicroEAVContext ctx = new MicroEAVContext())
            {
                return ((EAV.Store.StoreInstance)ctx.Instances.SingleOrDefault(it => it.Instance_ID == instanceID));
            }
        }

        public EAV.Store.IStoreInstance CreateRootInstance(EAV.Store.IStoreInstance instance, int containerID, int subjectID)
        {
            if (instance == null)
                return (null);

            using (EAVStoreClient.MicroEAVContext ctx = new MicroEAVContext())
            {
                Instance dbInstance = new Instance(instance);

                dbInstance.Subject_ID = subjectID;
                dbInstance.Container_ID = containerID;

                ctx.Instances.Add(dbInstance);

                ctx.SaveChanges();

                return ((EAV.Store.StoreInstance)dbInstance);
            }
        }

        public EAV.Store.IStoreInstance CreateChildInstance(EAV.Store.IStoreInstance instance, int containerID, int parentInstanceID)
        {
            if (instance == null)
                return (null);

            using (EAVStoreClient.MicroEAVContext ctx = new MicroEAVContext())
            {
                Instance dbInstance = new Instance(instance);
                Instance dbParentInstance = ctx.Instances.SingleOrDefault(it => it.Instance_ID == parentInstanceID);

                // TODO: Error if dbParentInstance is null

                dbInstance.Subject_ID = dbParentInstance.Subject_ID;
                dbInstance.Container_ID = containerID;
                dbInstance.Parent_Instance_ID = parentInstanceID;

                ctx.Instances.Add(dbInstance);

                ctx.SaveChanges();

                return ((EAV.Store.StoreInstance)dbInstance);
            }
        }

        public void UpdateInstance(EAV.Store.IStoreInstance instance)
        {
            using (EAVStoreClient.MicroEAVContext ctx = new MicroEAVContext())
            {
                EAVStoreClient.Instance dbInstance = ctx.Instances.SingleOrDefault(it => it.Instance_ID == instance.InstanceID);

                if (dbInstance != null)
                {
                    // Nothing to do at the moment, but including skeleton for completness and possible future expansion.
                }
                else
                    throw (new Exception(String.Format("Unable to retrieve instance ID {0}.", instance.InstanceID)));
            }
        }

        public void DeleteInstance(int instanceID)
        {
            using (EAVStoreClient.MicroEAVContext ctx = new MicroEAVContext())
            {
                var dbInstance = ctx.Instances.SingleOrDefault(it => it.Instance_ID == instanceID);

                if (dbInstance != null)
                {
                    ctx.Instances.Remove(dbInstance);

                    ctx.SaveChanges();
                }
            }
        }
    }
}
