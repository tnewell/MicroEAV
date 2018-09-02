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
    public partial class ContextStoreClient : EAV.Store.Clients.IContextStoreClient
    {
        public IEnumerable<EAV.Store.IStoreContext> RetrieveContexts()
        {
            using (EAVStoreClient.MicroEAVContext ctx = new MicroEAVContext())
            {
                return (ctx.Contexts.AsEnumerable().Select(it => (EAVStoreLibrary.StoreContext)it).ToList());
            }
        }

        public EAV.Store.IStoreContext RetrieveContext(int contextID)
        {
            using (EAVStoreClient.MicroEAVContext ctx = new MicroEAVContext())
            {
                return ((EAVStoreLibrary.StoreContext)ctx.Contexts.SingleOrDefault(it => it.Context_ID == contextID));
            }
        }

        public EAV.Store.IStoreContext RetrieveContext(string name)
        {
            using (EAVStoreClient.MicroEAVContext ctx = new MicroEAVContext())
            {
                return ((EAVStoreLibrary.StoreContext)ctx.Contexts.SingleOrDefault(it => it.Name == name));
            }
        }

        public EAV.Store.IStoreContext CreateContext(EAV.Store.IStoreContext context)
        {
            if (context == null)
                return (null);

            if (string.IsNullOrWhiteSpace(context.Name))
                throw (new InvalidOperationException("Property 'Name' for parameter 'context' may not be null or empty."));

            using (EAVStoreClient.MicroEAVContext ctx = new MicroEAVContext())
            {
                Context dbContext = new Context(context);

                ctx.Contexts.Add(dbContext);

                ctx.SaveChanges();

                return ((EAVStoreLibrary.StoreContext)dbContext);
            }
        }

        public void UpdateContext(EAV.Store.IStoreContext context)
        {
            using (EAVStoreClient.MicroEAVContext ctx = new MicroEAVContext())
            {
                EAVStoreClient.Context dbContext = ctx.Contexts.SingleOrDefault(it => it.Context_ID == context.ContextID);

                if (dbContext != null)
                {
                    if (dbContext.Name != context.Name)
                        dbContext.Name = context.Name;

                    if (dbContext.Data_Name != context.DataName)
                        dbContext.Data_Name = context.DataName;

                    if (dbContext.Display_Text != context.DisplayText)
                        dbContext.Display_Text = context.DisplayText;

                    ctx.SaveChanges();
                }
                else
                    throw (new Exception(String.Format("Unable to retrieve context ID {0}.", context.ContextID)));
            }
        }

        public void DeleteContext(int contextID)
        {
            using (EAVStoreClient.MicroEAVContext ctx = new MicroEAVContext())
            {
                var dbContext = ctx.Contexts.SingleOrDefault(it => it.Context_ID == contextID);

                if (dbContext != null)
                {
                    ctx.Contexts.Remove(dbContext);

                    ctx.SaveChanges();
                }
            }
        }
    }
}
