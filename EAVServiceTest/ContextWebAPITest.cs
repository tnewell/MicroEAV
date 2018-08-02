using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace EAVServiceTest
{
    public partial class EAVServiceTestHarness
    {
        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Retrieve")]
        [TestCategory("Context")]
        public void RetrieveAllContexts()
        {
            int nDbContexts = this.DbContext.Contexts.Count();

            HttpResponseMessage response = WebClient.GetAsync(String.Format("api/meta/contexts")).Result;
            if (response.IsSuccessStatusCode)
            {
                var contexts = response.Content.ReadAsAsync<IEnumerable<EAV.Model.BaseEAVContext>>().Result;
                int nClientContexts = contexts.Count();

                Assert.AreEqual(nDbContexts, nClientContexts, "The number of contexts retrieved by the client does not match the number in the database.");
            }
            else
            {
                Assert.Fail(response.Content.ReadAsStringAsync().Result);
            }
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Retrieve")]
        [TestCategory("Context")]
        public void RetrieveNonExistentContext()
        {
            HttpResponseMessage response = WebClient.GetAsync(String.Format("api/meta/contexts/{0}", -1)).Result;
            if (response.IsSuccessStatusCode)
            {
                var context = response.Content.ReadAsAsync<EAV.Model.BaseEAVContext>().Result;

                Assert.IsNull(context, "Unexpected context object retrieved.");
            }
            else
            {
                Assert.Fail(response.Content.ReadAsStringAsync().Result);
            }
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Retrieve")]
        [TestCategory("Context")]
        public void RetrieveNonExistentContextByName()
        {
            HttpResponseMessage response = WebClient.GetAsync(String.Format("api/meta/contexts/{0}", "No Such Context")).Result;
            if (response.IsSuccessStatusCode)
            {
                var context = response.Content.ReadAsAsync<EAV.Model.BaseEAVContext>().Result;

                Assert.IsNull(context, "Unexpected context object retrieved.");
            }
            else
            {
                Assert.Fail(response.Content.ReadAsStringAsync().Result);
            }
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
                HttpResponseMessage response = WebClient.GetAsync(String.Format("api/meta/contexts/{0}", dbContext.Context_ID)).Result;
                if (response.IsSuccessStatusCode)
                {
                    var context = response.Content.ReadAsAsync<EAV.Model.BaseEAVContext>().Result;

                    Assert.IsNotNull(context, "Failed to retrieve context {0}.", dbContext.Context_ID);
                    Assert.AreEqual(dbContext.Context_ID, context.ContextID, "Context ID values do not match.");
                }
                else
                {
                    Assert.Fail(response.Content.ReadAsStringAsync().Result);
                }
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
                HttpResponseMessage response = WebClient.GetAsync(String.Format("api/meta/contexts/{0}", dbContext.Name)).Result;
                if (response.IsSuccessStatusCode)
                {
                    var context = response.Content.ReadAsAsync<EAV.Model.BaseEAVContext>().Result;

                    Assert.IsNotNull(context, "Failed to retrieve context {0}.", dbContext.Name);
                    Assert.AreEqual(dbContext.Name, context.Name, "Context Name values do not match.");
                }
                else
                {
                    Assert.Fail(response.Content.ReadAsStringAsync().Result);
                }
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
            string contextName = Guid.NewGuid().ToString();

            HttpResponseMessage response = WebClient.PostAsJsonAsync<EAV.Model.BaseEAVContext>("api/meta/contexts", new EAV.Model.BaseEAVContext() { Name = contextName, DataName = contextName.ToUpper(), DisplayText = contextName + ":" }).Result;
            if (response.IsSuccessStatusCode)
            {
                var context = response.Content.ReadAsAsync<EAV.Model.BaseEAVContext>().Result;

                Assert.IsNotNull(context, "Failed to create context with name '{0}'", contextName);

                var dbContext = this.DbContext.Contexts.SingleOrDefault(it => it.Context_ID == context.ContextID);

                Assert.IsNotNull(dbContext, String.Format("Failed to retrieve context ID {0} from the database.", context.ContextID));

                Assert.AreEqual(context.Name, dbContext.Name, "Property 'Name' was not created correctly.");
                Assert.AreEqual(context.DataName, dbContext.Data_Name, "Property 'DataName' was not created correctly.");
                Assert.AreEqual(context.DisplayText, dbContext.Display_Text, "Property 'DisplayText' was not created correctly.");
            }
            else
            {
                Assert.Fail(response.Content.ReadAsStringAsync().Result);
            }
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

            var context = (EAV.Model.BaseEAVContext)dbContext;

            context.Name = oldName.Flip();
            context.DataName = oldDataName.Flip();
            context.DisplayText = oldDisplayText.Flip();

            HttpResponseMessage response = WebClient.PatchAsJsonAsync<EAV.Model.BaseEAVContext>("api/meta/contexts", context).Result;
            if (response.IsSuccessStatusCode)
            {
                ResetDatabaseContext();

                dbContext = this.dbContext.Contexts.Single(it => it.Context_ID == context.ContextID);

                Assert.AreEqual(context.Name, dbContext.Name);
                Assert.AreNotEqual(oldName, dbContext.Name);
                Assert.AreEqual(context.DataName, dbContext.Data_Name);
                Assert.AreNotEqual(oldDataName, dbContext.Data_Name);
                Assert.AreEqual(context.DisplayText, dbContext.Display_Text);
                Assert.AreNotEqual(oldDisplayText, dbContext.Display_Text);
            }
            else
            {
                Assert.Fail(response.Content.ReadAsStringAsync().Result);
            }
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Delete")]
        [TestCategory("Context")]
        public void DeleteContext()
        {
            EAVStoreClient.Context dbContextIn = CreateContext(Guid.NewGuid().ToString());

            HttpResponseMessage response = WebClient.DeleteAsync(String.Format("api/meta/contexts/{0}", dbContextIn.Context_ID)).Result;
            if (response.IsSuccessStatusCode)
            {
                ResetDatabaseContext();

                EAVStoreClient.Context dbContextOut = this.DbContext.Contexts.SingleOrDefault(it => it.Context_ID == dbContextIn.Context_ID);

                Assert.IsNull(dbContextOut, "Failed to delete context ID {0} from the database.", dbContextIn.Context_ID);
            }
            else
            {
                Assert.Fail(response.Content.ReadAsStringAsync().Result);
            }
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Retrieve")]
        [TestCategory("Container")]
        public void RetrieveRootContainers()
        {
            var dbContext = SelectRandomItem(this.DbContext.Contexts);
            int nDbRootContainers = this.DbContext.Containers.Where(it => it.Context_ID == dbContext.Context_ID && it.Parent_Container_ID == null).Count();

            HttpResponseMessage response = WebClient.GetAsync(String.Format("api/meta/contexts/{0}/containers", dbContext.Context_ID)).Result;
            if (response.IsSuccessStatusCode)
            {
                var containers = response.Content.ReadAsAsync<IEnumerable<EAV.Model.BaseEAVContainer>>().Result;
                int nClientContainers = containers.Count();

                Assert.AreEqual(nDbRootContainers, nClientContainers, "The number of containers retrieved by the client does not match the number in the database.");
            }
            else
            {
                Assert.Fail(response.Content.ReadAsStringAsync().Result);
            }
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Create")]
        [TestCategory("Container")]
        public void CreateRootContainer()
        {
            var dbContext = SelectRandomItem(this.DbContext.Contexts);
            string containerName = Guid.NewGuid().ToString();

            HttpResponseMessage response = WebClient.PostAsJsonAsync<EAV.Model.BaseEAVContainer>(String.Format("api/meta/contexts/{0}/containers", dbContext.Context_ID), new EAV.Model.BaseEAVContainer() { Name = containerName, DataName = containerName.ToUpper(), DisplayText = containerName + ":", IsRepeating = true }).Result;
            if (response.IsSuccessStatusCode)
            {
                var container = response.Content.ReadAsAsync<EAV.Model.BaseEAVContainer>().Result;

                Assert.IsNotNull(container, "Failed to create container with name '{0}'", containerName);

                var dbContainer = this.DbContext.Containers.SingleOrDefault(it => it.Container_ID == container.ContainerID);

                Assert.IsNotNull(dbContainer, String.Format("Failed to retrieve container ID {0} from the database.", container.ContainerID));

                Assert.AreEqual(container.Name, dbContainer.Name, "Property 'Name' was not created correctly.");
                Assert.AreEqual(container.DataName, dbContainer.Data_Name, "Property 'DataName' was not created correctly.");
                Assert.AreEqual(container.DisplayText, dbContainer.Display_Text, "Property 'DisplayText' was not created correctly.");
                Assert.AreEqual(container.IsRepeating, dbContainer.Is_Repeating, "Property 'IsRepeating' was not created correctly.");
                Assert.AreEqual(container.Sequence, dbContainer.Sequence, "Property 'Sequence' was not created correctly.");
            }
            else
            {
                Assert.Fail(response.Content.ReadAsStringAsync().Result);
            }
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Retrieve")]
        [TestCategory("Subject")]
        public void RetrieveSubjects()
        {
            Assert.Fail("Test needed.");
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Create")]
        [TestCategory("Subject")]
        public void CreateContextSubject()
        {
            Assert.Fail("Test needed.");
        }
    }
}
