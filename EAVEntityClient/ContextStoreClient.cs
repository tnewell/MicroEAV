using System;
using System.Collections.Generic;
using System.Linq;


namespace EAVStoreClient
{
    public partial class EAVContextClient : EAV.Store.IEAVContextClient
    {
        public IEnumerable<EAV.Model.IEAVContext> RetrieveContexts()
        {
            using (EAVStoreClient.MicroEAVContext ctx = new MicroEAVContext())
            {
                return (ctx.Contexts.AsEnumerable().Select(it => (EAV.Model.BaseEAVContext)it).ToList());
            }
        }

        public EAV.Model.IEAVContext RetrieveContext(int contextID)
        {
            using (EAVStoreClient.MicroEAVContext ctx = new MicroEAVContext())
            {
                return ((EAV.Model.BaseEAVContext)ctx.Contexts.SingleOrDefault(it => it.Context_ID == contextID));
            }
        }

        public EAV.Model.IEAVContext RetrieveContext(string name)
        {
            using (EAVStoreClient.MicroEAVContext ctx = new MicroEAVContext())
            {
                return ((EAV.Model.BaseEAVContext)ctx.Contexts.SingleOrDefault(it => it.Name == name));
            }
        }

        public EAV.Model.IEAVContext CreateContext(EAV.Model.IEAVContext context)
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

                return ((EAV.Model.BaseEAVContext)dbContext);
            }
        }

        public void UpdateContext(EAV.Model.IEAVContext context)
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
