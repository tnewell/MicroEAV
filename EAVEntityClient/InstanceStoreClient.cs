using System;
using System.Collections.Generic;
using System.Linq;


namespace EAVStoreClient
{
    public partial class EAVInstanceClient : EAV.Store.IEAVInstanceClient
    {
        public IEnumerable<EAV.Model.IEAVInstance> RetrieveRootInstances(int? containerID, int? subjectID)
        {
            using (EAVStoreClient.MicroEAVContext ctx = new MicroEAVContext())
            {
                if (containerID != null && subjectID != null)
                {
                    return (ctx.Instances.Where(it => it.Container_ID == containerID && it.Subject_ID == subjectID && it.Parent_Instance_ID == null).AsEnumerable().Select(it => (EAV.Model.BaseEAVInstance)it).ToList());
                }
                else if (containerID != null && subjectID == null)
                {
                    return (ctx.Instances.Where(it => it.Container_ID == containerID && it.Parent_Instance_ID == null).AsEnumerable().Select(it => (EAV.Model.BaseEAVInstance)it).ToList());
                }
                else if (containerID == null && subjectID != null)
                {
                    return (ctx.Instances.Where(it => it.Subject_ID == subjectID && it.Parent_Instance_ID == null).AsEnumerable().Select(it => (EAV.Model.BaseEAVInstance)it).ToList());
                }
                else
                {
                    return (ctx.Instances.Where(it => it.Parent_Instance_ID == null).AsEnumerable().Select(it => (EAV.Model.BaseEAVInstance)it).ToList());
                }
            }
        }

        public IEnumerable<EAV.Model.IEAVInstance> RetrieveChildInstances(int? containerID, int? parentInstanceID)
        {
            using (EAVStoreClient.MicroEAVContext ctx = new MicroEAVContext())
            {
                if (containerID != null && parentInstanceID != null)
                {
                    return (ctx.Instances.Where(it => it.Container_ID == containerID && it.Parent_Instance_ID == parentInstanceID).AsEnumerable().Select(it => (EAV.Model.BaseEAVInstance)it).ToList());
                }
                else if (containerID != null && parentInstanceID == null)
                {
                    return (ctx.Instances.Where(it => it.Container_ID == containerID && it.Parent_Instance_ID != null).AsEnumerable().Select(it => (EAV.Model.BaseEAVInstance)it).ToList());
                }
                else if (containerID == null && parentInstanceID != null)
                {
                    return (ctx.Instances.Where(it => it.Parent_Instance_ID == parentInstanceID).AsEnumerable().Select(it => (EAV.Model.BaseEAVInstance)it).ToList());
                }
                else
                {
                    return (ctx.Instances.Where(it => it.Parent_Instance_ID != null).AsEnumerable().Select(it => (EAV.Model.BaseEAVInstance)it).ToList());
                }
            }
        }

        public EAV.Model.IEAVInstance RetrieveInstance(int instanceID)
        {
            using (EAVStoreClient.MicroEAVContext ctx = new MicroEAVContext())
            {
                return ((EAV.Model.BaseEAVInstance)ctx.Instances.SingleOrDefault(it => it.Instance_ID == instanceID));
            }
        }

        public EAV.Model.IEAVInstance CreateRootInstance(EAV.Model.IEAVInstance instance, int containerID, int subjectID)
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

                return ((EAV.Model.BaseEAVInstance)dbInstance);
            }
        }

        public EAV.Model.IEAVInstance CreateChildInstance(EAV.Model.IEAVInstance instance, int containerID, int parentInstanceID)
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

                return ((EAV.Model.BaseEAVInstance)dbInstance);
            }
        }

        public void UpdateInstance(EAV.Model.IEAVInstance instance)
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
