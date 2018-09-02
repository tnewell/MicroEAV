using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace EAVServiceTest
{
    public partial class EAVWebServiceTestHarness
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
                var contexts = response.Content.ReadAsAsync<IEnumerable<EAVStoreLibrary.StoreContext>>().Result;
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
                var context = response.Content.ReadAsAsync<EAVStoreLibrary.StoreContext>().Result;

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
                var context = response.Content.ReadAsAsync<EAVStoreLibrary.StoreContext>().Result;

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
                    var context = response.Content.ReadAsAsync<EAVStoreLibrary.StoreContext>().Result;

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
                    var context = response.Content.ReadAsAsync<EAVStoreLibrary.StoreContext>().Result;

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

            HttpResponseMessage response = WebClient.PostAsJsonAsync<EAVStoreLibrary.StoreContext>("api/meta/contexts", new EAVStoreLibrary.StoreContext() { Name = contextName, DataName = contextName.ToUpper(), DisplayText = contextName + ":" }).Result;
            if (response.IsSuccessStatusCode)
            {
                var context = response.Content.ReadAsAsync<EAVStoreLibrary.StoreContext>().Result;

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

            var context = (EAVStoreLibrary.StoreContext)dbContext;

            context.Name = oldName.Flip();
            context.DataName = oldDataName.Flip();
            context.DisplayText = oldDisplayText.Flip();

            HttpResponseMessage response = WebClient.PatchAsJsonAsync<EAVStoreLibrary.StoreContext>("api/meta/contexts", context).Result;
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
                var containers = response.Content.ReadAsAsync<IEnumerable<EAVStoreLibrary.StoreContainer>>().Result;
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

            HttpResponseMessage response = WebClient.PostAsJsonAsync<EAVStoreLibrary.StoreContainer>(String.Format("api/meta/contexts/{0}/containers", dbContext.Context_ID), new EAVStoreLibrary.StoreContainer() { Name = containerName, DataName = containerName.ToUpper(), DisplayText = containerName + ":", IsRepeating = true }).Result;
            if (response.IsSuccessStatusCode)
            {
                var container = response.Content.ReadAsAsync<EAVStoreLibrary.StoreContainer>().Result;

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
        public void RetrieveContextSubjects()
        {
            var dbContext = SelectRandomItem(this.DbContext.Contexts);
            int nDbSubjects = this.DbContext.Subjects.Where(it => it.Context_ID == dbContext.Context_ID).Count();

            HttpResponseMessage response = WebClient.GetAsync(String.Format("api/meta/contexts/{0}/subjects", dbContext.Context_ID)).Result;
            if (response.IsSuccessStatusCode)
            {
                var subjects = response.Content.ReadAsAsync<IEnumerable<EAVStoreLibrary.StoreSubject>>().Result;
                int nClientEntities = subjects.Count();

                Assert.AreEqual(nDbSubjects, nClientEntities, "The number of subjects retrieved by the client does not match the number in the database.");
            }
            else
            {
                Assert.Fail(response.Content.ReadAsStringAsync().Result);
            }
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Create")]
        [TestCategory("Subject")]
        public void CreateContextSubject()
        {
            var dbContext = SelectRandomItem(this.DbContext.Contexts);
            var dbEntity = SelectRandomItem(this.DbContext.Entities);
            string subjectIdentifier = Guid.NewGuid().ToString();

            HttpResponseMessage response = WebClient.PostAsJsonAsync<EAVStoreLibrary.StoreSubject>(String.Format("api/meta/contexts/{0}/subjects?entity={1}", dbContext.Context_ID, dbEntity.Entity_ID), new EAVStoreLibrary.StoreSubject() { Identifier = subjectIdentifier }).Result;
            if (response.IsSuccessStatusCode)
            {
                var subject = response.Content.ReadAsAsync<EAVStoreLibrary.StoreSubject>().Result;

                Assert.IsNotNull(subject, "Failed to create subject with identifier '{0}'", subjectIdentifier);

                var dbSubject = this.DbContext.Subjects.SingleOrDefault(it => it.Subject_ID == subject.SubjectID);

                Assert.IsNotNull(dbSubject, String.Format("Failed to retrieve subject ID {0} from the database.", subject.SubjectID));

                Assert.AreEqual(subject.Identifier, dbSubject.Identifier, "Property 'Identifier' was not created correctly.");
            }
            else
            {
                Assert.Fail(response.Content.ReadAsStringAsync().Result);
            }
        }
    }
}
