using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace EAVStoreClientTestHarness
{
    public partial class EAVStoreClientTestHarness
    {
        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Retrieve")]
        [TestCategory("Subject")]
        public void RetrieveAllSubjects()
        {
            EAVStoreClient.EAVSubjectClient client = new EAVStoreClient.EAVSubjectClient();

            int nDbSubjects = this.DbContext.Subjects.Count();
            int nClientSubjects = client.RetrieveSubjects(null, null).Count();

            Assert.AreEqual(nDbSubjects, nClientSubjects, "The number of subjects retrieved by the client does not match the number in the database.");
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Retrieve")]
        [TestCategory("Subject")]
        public void RetrieveNonExistentSubject()
        {
            EAVStoreClient.EAVSubjectClient client = new EAVStoreClient.EAVSubjectClient();

            var subject = client.RetrieveSubject(-1);

            Assert.IsNull(subject, "Unexpected subject object retrieved.");
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Retrieve")]
        [TestCategory("Subject")]
        public void RetrieveRandomSubject()
        {
            var dbSubject = SelectRandomItem(this.DbContext.Subjects);

            if (dbSubject != null)
            {
                EAVStoreClient.EAVSubjectClient client = new EAVStoreClient.EAVSubjectClient();

                var subject = client.RetrieveSubject(dbSubject.Subject_ID);

                Assert.IsNotNull(subject, "Failed to retrieve subject {0}.", dbSubject.Subject_ID);
                Assert.AreEqual(dbSubject.Subject_ID, subject.SubjectID, "Subject ID values do not match.");
            }
            else
            {
                Assert.Inconclusive("No subjects were found in the database.");
            }
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Create")]
        [TestCategory("Subject")]
        public void CreateSubject()
        {
            EAVStoreClient.EAVSubjectClient client = new EAVStoreClient.EAVSubjectClient();
            int entityID = SelectRandomItem(this.DbContext.Entities).Entity_ID;
            int contextID = SelectRandomItem(this.DbContext.Contexts).Context_ID;
            string subjectDescriptor = Guid.NewGuid().ToString();

            EAV.Model.IEAVSubject subject = client.CreateSubject(new EAV.Model.BaseEAVSubject()
            {
                Identifier = subjectDescriptor,
            }, contextID, entityID);

            Assert.IsNotNull(subject, "Failed to create subject with descriptor '{0}' for context ID {1} and entity ID {2}.", subjectDescriptor, contextID, entityID);

            ResetDatabaseContext();

            var dbSubject = this.DbContext.Subjects.SingleOrDefault(it => it.Subject_ID == subject.SubjectID);

            Assert.IsNotNull(dbSubject, String.Format("Failed to retrieve subject ID {0} from the database.", subject.SubjectID));

            Assert.AreEqual(subject.Identifier, dbSubject.Identifier, "Property 'Identifier' was not created correctly.");
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Update")]
        [TestCategory("Subject")]
        public void UpdateSubject()
        {
            var dbSubject = SelectRandomItem(this.DbContext.Subjects);
            string oldIdentifier = dbSubject.Identifier;

            EAVStoreClient.EAVSubjectClient client = new EAVStoreClient.EAVSubjectClient();

            var subject = (EAV.Model.BaseEAVSubject)dbSubject;

            subject.Identifier = oldIdentifier.Flip();

            client.UpdateSubject(subject);

            ResetDatabaseContext();

            dbSubject = this.dbContext.Subjects.Single(it => it.Subject_ID == subject.SubjectID);

            Assert.AreEqual(subject.Identifier, dbSubject.Identifier);
            Assert.AreNotEqual(oldIdentifier, dbSubject.Identifier);
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Delete")]
        [TestCategory("Subject")]
        public void DeleteSubject()
        {
            EAVStoreClient.EAVSubjectClient client = new EAVStoreClient.EAVSubjectClient();

            EAVStoreClient.Entity dbEntity = SelectRandomItem(this.DbContext.Entities);
            EAVStoreClient.Context dbContext = SelectRandomItem(this.DbContext.Contexts);
            EAVStoreClient.Subject dbSubjectIn = CreateSubject(dbContext.Context_ID, dbEntity.Entity_ID, Guid.NewGuid().ToString());

            client.DeleteSubject(dbSubjectIn.Subject_ID);

            EAVStoreClient.Subject dbSubjectOut = this.DbContext.Subjects.SingleOrDefault(it => it.Subject_ID == dbSubjectIn.Subject_ID);

            Assert.IsNull(dbSubjectOut, "Failed to delete subject ID {0} from the database.", dbSubjectIn.Subject_ID);
        }
    }
}
