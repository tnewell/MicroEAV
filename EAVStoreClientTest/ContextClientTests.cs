using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace EAVStoreClientTestHarness
{
    public partial class EAVStoreClientTestHarness
    {
        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Retrieve")]
        [TestCategory("Context")]
        public void RetrieveAllContexts()
        {
            EAV.Store.Clients.IContextStoreClient client = factory.Create<EAV.Store.Clients.IContextStoreClient>();

            int nDbContexts = this.DbContext.Contexts.Count();
            int nClientContexts = client.RetrieveContexts().Count();

            Assert.AreEqual(nDbContexts, nClientContexts, "The number of contexts retrieved by the client does not match the number in the database.");
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Retrieve")]
        [TestCategory("Context")]
        public void RetrieveNonExistentContext()
        {
            EAV.Store.Clients.IContextStoreClient client = factory.Create<EAV.Store.Clients.IContextStoreClient>();

            var context = client.RetrieveContext(-1);

            Assert.IsNull(context, "Unexpected context object retrieved.");
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Retrieve")]
        [TestCategory("Context")]
        public void RetrieveNonExistentContextByName()
        {
            EAV.Store.Clients.IContextStoreClient client = factory.Create<EAV.Store.Clients.IContextStoreClient>();

            var context = client.RetrieveContext("No Such Context");

            Assert.IsNull(context, "Unexpected context object retrieved.");
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Retrieve")]
        [TestCategory("Context")]
        public void RetrieveRandomContext()
        {
            var dbContext = SelectRandomItem(this.DbContext.Contexts);

            if (dbContext != null)
            {
                EAV.Store.Clients.IContextStoreClient client = factory.Create<EAV.Store.Clients.IContextStoreClient>();

                var context = client.RetrieveContext(dbContext.Context_ID);

                Assert.IsNotNull(context, "Failed to retrieve context {0}.", dbContext.Context_ID);
                Assert.AreEqual(dbContext.Context_ID, context.ContextID, "Context ID values do not match.");
            }
            else
            {
                Assert.Inconclusive("No contexts were found in the database.");
            }
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Retrieve")]
        [TestCategory("Context")]
        public void RetrieveRandomContextByName()
        {
            var dbContext = SelectRandomItem(this.DbContext.Contexts);

            if (dbContext != null)
            {
                EAV.Store.Clients.IContextStoreClient client = factory.Create<EAV.Store.Clients.IContextStoreClient>();

                var context = client.RetrieveContext(dbContext.Name);

                Assert.IsNotNull(context, "Failed to retrieve context {0}.", dbContext.Name);
                Assert.AreEqual(dbContext.Name, context.Name, "Context Name values do not match.");
            }
            else
            {
                Assert.Inconclusive("No contexts were found in the database.");
            }
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Create")]
        [TestCategory("Context")]
        public void CreateContext()
        {
            EAV.Store.Clients.IContextStoreClient client = factory.Create<EAV.Store.Clients.IContextStoreClient>();
            string contextName = Guid.NewGuid().ToString();

            EAV.Store.IStoreContext context = client.CreateContext(new EAVStoreLibrary.StoreContext()
            {
                Name = contextName,
                DataName = contextName.ToUpper(),
                DisplayText = contextName + ":",
            });

            Assert.IsNotNull(context, "Failed to create context with name '{0}'", contextName);

            ResetDatabaseContext();

            var dbContext = this.DbContext.Contexts.SingleOrDefault(it => it.Context_ID == context.ContextID);

            Assert.IsNotNull(dbContext, String.Format("Failed to retrieve context ID {0} from the database.", context.ContextID));

            Assert.AreEqual(context.Name, dbContext.Name, "Property 'Name' was not created correctly.");
            Assert.AreEqual(context.DataName, dbContext.Data_Name, "Property 'DataName' was not created correctly.");
            Assert.AreEqual(context.DisplayText, dbContext.Display_Text, "Property 'DisplayText' was not created correctly.");
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Create")]
        [TestCategory("Context")]
        [ExpectedException(typeof(System.Data.Entity.Infrastructure.DbUpdateException))]
        public void CreateDuplicateContext_Name()
        {
            EAV.Store.Clients.IContextStoreClient client = factory.Create<EAV.Store.Clients.IContextStoreClient>();
            string contextName = Guid.NewGuid().ToString();

            EAV.Store.IStoreContext context = client.CreateContext(new EAVStoreLibrary.StoreContext()
            {
                Name = contextName,
                DataName = contextName.ToUpper(),
                DisplayText = contextName + ":",
            });

            Assert.IsNotNull(context, "Failed to create context with name '{0}'", contextName);

            client.CreateContext(new EAVStoreLibrary.StoreContext()
            {
                Name = contextName,
                DataName = contextName.ToUpper() + "1",
                DisplayText = contextName + ":",
            });

            Assert.Fail("Failed to throw exception creating context with duplicate name.");
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Create")]
        [TestCategory("Context")]
        [ExpectedException(typeof(System.Data.Entity.Infrastructure.DbUpdateException))]
        public void CreateDuplicateContext_Data_Name()
        {
            EAV.Store.Clients.IContextStoreClient client = factory.Create<EAV.Store.Clients.IContextStoreClient>();
            string contextName = Guid.NewGuid().ToString();

            EAV.Store.IStoreContext context = client.CreateContext(new EAVStoreLibrary.StoreContext()
            {
                Name = contextName,
                DataName = contextName.ToUpper(),
                DisplayText = contextName + ":",
            });

            Assert.IsNotNull(context, "Failed to create context with name '{0}'", contextName);

            client.CreateContext(new EAVStoreLibrary.StoreContext()
            {
                Name = contextName + "1",
                DataName = contextName.ToUpper(),
                DisplayText = contextName + ":",
            });

            Assert.Fail("Failed to throw exception creating context with duplicate data name.");
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Update")]
        [TestCategory("Context")]
        public void UpdateContext()
        {
            var dbContext = SelectRandomItem(this.DbContext.Contexts);
            string oldName = dbContext.Name;
            string oldDataName = dbContext.Data_Name;
            string oldDisplayText = dbContext.Display_Text;

            EAV.Store.Clients.IContextStoreClient client = factory.Create<EAV.Store.Clients.IContextStoreClient>();

            var context = (EAVStoreLibrary.StoreContext)dbContext;

            context.Name = oldName.Flip();
            context.DataName = oldDataName.Flip();
            context.DisplayText = oldDisplayText.Flip();

            client.UpdateContext(context);

            ResetDatabaseContext();

            dbContext = this.dbContext.Contexts.Single(it => it.Context_ID == context.ContextID);

            Assert.AreEqual(context.Name, dbContext.Name);
            Assert.AreNotEqual(oldName, dbContext.Name);
            Assert.AreEqual(context.DataName, dbContext.Data_Name);
            Assert.AreNotEqual(oldDataName, dbContext.Data_Name);
            Assert.AreEqual(context.DisplayText, dbContext.Display_Text);
            Assert.AreNotEqual(oldDisplayText, dbContext.Display_Text);
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Delete")]
        [TestCategory("Context")]
        public void DeleteContext()
        {
            EAV.Store.Clients.IContextStoreClient client = factory.Create<EAV.Store.Clients.IContextStoreClient>();
            EAVStoreClient.Context dbContextIn = CreateContext(Guid.NewGuid().ToString());

            client.DeleteContext(dbContextIn.Context_ID);

            EAVStoreClient.Context dbContextOut = this.DbContext.Contexts.SingleOrDefault(it => it.Context_ID == dbContextIn.Context_ID);

            Assert.IsNull(dbContextOut, "Failed to delete context ID {0} from the database.", dbContextIn.Context_ID);
        }
    }
}
