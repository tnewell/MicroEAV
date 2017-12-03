using System;
using System.Collections.Generic;
using System.Linq;


namespace EAVStoreClient
{
    public partial class EAVContainerClient : EAV.Store.IEAVContainerClient
    {
        public IEnumerable<EAV.Model.IEAVContainer> RetrieveRootContainers(int? contextID)
        {
            using (EAVStoreClient.MicroEAVContext ctx = new MicroEAVContext())
            {
                if (contextID != null)
                {
                    return (ctx.Containers.Where(it => it.Context_ID == contextID && it.Parent_Container_ID == null).AsEnumerable().Select(it => (EAV.Model.BaseEAVContainer)it).ToList());
                }
                else
                {
                    return (ctx.Containers.Where(it => it.Parent_Container_ID == null).AsEnumerable().Select(it => (EAV.Model.BaseEAVContainer)it).ToList());
                }
            }
        }

        public IEnumerable<EAV.Model.IEAVContainer> RetrieveChildContainers(int? parentContainerID)
        {
            using (EAVStoreClient.MicroEAVContext ctx = new MicroEAVContext())
            {
                if (parentContainerID != null)
                {
                    return (ctx.Containers.Where(it => it.Parent_Container_ID == parentContainerID).AsEnumerable().Select(it => (EAV.Model.BaseEAVContainer)it).ToList());
                }
                else
                {
                    return (ctx.Containers.Where(it => it.Parent_Container_ID != null).AsEnumerable().Select(it => (EAV.Model.BaseEAVContainer)it).ToList());
                }
            }
        }

        public EAV.Model.IEAVContainer RetrieveContainer(int containerID)
        {
            using (EAVStoreClient.MicroEAVContext ctx = new MicroEAVContext())
            {
                return ((EAV.Model.BaseEAVContainer)ctx.Containers.SingleOrDefault(it => it.Container_ID == containerID));
            }
        }

        public EAV.Model.IEAVContainer CreateRootContainer(EAV.Model.IEAVContainer container, int contextID)
        {
            if (container == null)
                return (null);

            using (EAVStoreClient.MicroEAVContext ctx = new MicroEAVContext())
            {
                Container dbContainer = new Container(container);

                dbContainer.Context_ID = contextID;

                ctx.Containers.Add(dbContainer);

                ctx.SaveChanges();

                return ((EAV.Model.BaseEAVContainer)dbContainer);
            }
        }

        public EAV.Model.IEAVContainer CreateChildContainer(EAV.Model.IEAVContainer container, int parentContainerID)
        {
            if (container == null)
                return (null);

            using (EAVStoreClient.MicroEAVContext ctx = new MicroEAVContext())
            {
                Container dbContainer = new Container(container);
                Container dbParentContainer = ctx.Containers.SingleOrDefault(it => it.Container_ID == parentContainerID);

                if (dbContainer == null)
                    throw (new InvalidOperationException("Unable to locate parent container."));

                dbContainer.Context_ID = dbParentContainer.Context_ID;
                dbContainer.Parent_Container_ID = parentContainerID;

                ctx.Containers.Add(dbContainer);

                ctx.SaveChanges();

                return ((EAV.Model.BaseEAVContainer)dbContainer);
            }
        }

        public void UpdateContainer(EAV.Model.IEAVContainer container)
        {
            using (EAVStoreClient.MicroEAVContext ctx = new MicroEAVContext())
            {
                EAVStoreClient.Container dbContainer = ctx.Containers.SingleOrDefault(it => it.Container_ID == container.ContainerID);

                if (dbContainer != null)
                {
                    if (dbContainer.Name != container.Name)
                        dbContainer.Name = container.Name;

                    if (dbContainer.Data_Name != container.DataName)
                        dbContainer.Data_Name = container.DataName;

                    if (dbContainer.Display_Text != container.DisplayText)
                        dbContainer.Display_Text = container.DisplayText;

                    if (dbContainer.Is_Repeating != container.IsRepeating)
                        dbContainer.Is_Repeating = container.IsRepeating;

                    ctx.SaveChanges();
                }
                else
                    throw (new Exception(String.Format("Unable to retrieve container ID {0}.", container.ContainerID)));
            }
        }

        public void DeleteContainer(int containerID)
        {
            using (EAVStoreClient.MicroEAVContext ctx = new MicroEAVContext())
            {
                var dbContainer = ctx.Containers.SingleOrDefault(it => it.Container_ID == containerID);

                if (dbContainer != null)
                {
                    ctx.Containers.Remove(dbContainer);

                    ctx.SaveChanges();
                }
            }
        }
    }
}
