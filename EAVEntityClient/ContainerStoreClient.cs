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
    public partial class EAVContainerClient : EAV.Store.IStoreContainerClient
    {
        public IEnumerable<EAV.Store.IStoreContainer> RetrieveRootContainers(int? contextID)
        {
            using (EAVStoreClient.MicroEAVContext ctx = new MicroEAVContext())
            {
                if (contextID != null)
                {
                    return (ctx.Containers.Where(it => it.Context_ID == contextID && it.Parent_Container_ID == null).AsEnumerable().Select(it => (EAV.Store.StoreContainer)it).ToList());
                }
                else
                {
                    return (ctx.Containers.Where(it => it.Parent_Container_ID == null).AsEnumerable().Select(it => (EAV.Store.StoreContainer)it).ToList());
                }
            }
        }

        public IEnumerable<EAV.Store.IStoreContainer> RetrieveChildContainers(int? parentContainerID)
        {
            using (EAVStoreClient.MicroEAVContext ctx = new MicroEAVContext())
            {
                if (parentContainerID != null)
                {
                    return (ctx.Containers.Where(it => it.Parent_Container_ID == parentContainerID).AsEnumerable().Select(it => (EAV.Store.StoreContainer)it).ToList());
                }
                else
                {
                    return (ctx.Containers.Where(it => it.Parent_Container_ID != null).AsEnumerable().Select(it => (EAV.Store.StoreContainer)it).ToList());
                }
            }
        }

        public EAV.Store.IStoreContainer RetrieveContainer(int containerID)
        {
            using (EAVStoreClient.MicroEAVContext ctx = new MicroEAVContext())
            {
                return ((EAV.Store.StoreContainer)ctx.Containers.SingleOrDefault(it => it.Container_ID == containerID));
            }
        }

        public EAV.Store.IStoreContainer CreateRootContainer(EAV.Store.IStoreContainer container, int contextID)
        {
            if (container == null)
                return (null);

            using (EAVStoreClient.MicroEAVContext ctx = new MicroEAVContext())
            {
                Container dbContainer = new Container(container);

                dbContainer.Context_ID = contextID;

                ctx.Containers.Add(dbContainer);

                ctx.SaveChanges();

                return ((EAV.Store.StoreContainer)dbContainer);
            }
        }

        public EAV.Store.IStoreContainer CreateChildContainer(EAV.Store.IStoreContainer container, int parentContainerID)
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

                return ((EAV.Store.StoreContainer)dbContainer);
            }
        }

        public void UpdateContainer(EAV.Store.IStoreContainer container)
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

                    if (dbContainer.Sequence != container.Sequence)
                        dbContainer.Sequence = container.Sequence;

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
