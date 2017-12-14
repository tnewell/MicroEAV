using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using EAV.Model;

using EAVFramework.Model;


namespace EAVFrameworkTest
{
    public partial class EAVFrameworkTestHarness
    {
        #region Object State Tests
        [TestMethod]
        public void CreateSubject()
        {
            EAVSubject aSubject = new EAVSubject();

            Assert.IsTrue(aSubject.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            Assert.IsNull(aSubject.SubjectID, "Property 'SubjectID' is not 'null' on creation.");

            Assert.IsNull(aSubject.Identifier, "Property 'Identifier' is not 'null' on creation.");

            Assert.IsNull(aSubject.Context, "Property 'Context' is not 'null' on creation.");

            Assert.IsNull(aSubject.Entity, "Property 'Entity' is not 'null' on creation.");

            Assert.IsNotNull(aSubject.Instances, "Property 'Instances' is null on creation.");
            Assert.IsTrue(aSubject.Instances.Count == 0, "Property 'Instances' is not empty on creation.");
        }

        [TestMethod]
        public void SetSubjectUnmodifiedFromNew()
        {
            EAVSubject aSubject = new EAVSubject();

            Assert.IsTrue(aSubject.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aSubject.MarkUnmodified();

            Assert.IsTrue(aSubject.ObjectState == ObjectState.Unmodified, "Object state not properly set to 'Unmodified'.");
        }

        [TestMethod]
        public void SetSubjectModifiedFromNew()
        {
            EAVSubject aSubject = new EAVSubject();

            Assert.IsTrue(aSubject.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            string identifier = Guid.NewGuid().ToString();
            aSubject.Identifier = identifier;

            Assert.AreEqual(identifier, aSubject.Identifier, "Property 'Identifier' not properly set.");

            Assert.IsTrue(aSubject.ObjectState == ObjectState.New, "Setting property in 'New' state should not alter object state.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SetSubjectDeletedFromNew()
        {
            EAVSubject aSubject = new EAVSubject();

            Assert.IsTrue(aSubject.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aSubject.MarkDeleted();

            Assert.IsTrue(aSubject.ObjectState == ObjectState.New, "Setting object state to 'Deleted' from 'New' state should not alter object state.");
        }

        [TestMethod]
        public void SetSubjectModifiedFromUnmodified()
        {
            EAVSubject aSubject = new EAVSubject();

            Assert.IsTrue(aSubject.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aSubject.MarkUnmodified();

            Assert.IsTrue(aSubject.ObjectState == ObjectState.Unmodified, "Object state not properly set to 'Unmodified'.");

            string identifier = Guid.NewGuid().ToString();
            aSubject.Identifier = identifier;

            Assert.AreEqual(identifier, aSubject.Identifier, "Property 'Identifier' not properly set.");
            Assert.IsTrue(aSubject.ObjectState == ObjectState.Modified, "Setting property should alter object state to 'Modified'.");
        }

        [TestMethod]
        public void SetSubjectDeletedFromUnmodified()
        {
            EAVSubject aSubject = new EAVSubject();

            Assert.IsTrue(aSubject.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aSubject.MarkUnmodified();

            Assert.IsTrue(aSubject.ObjectState == ObjectState.Unmodified, "Object state not properly set to 'Unmodified'.");

            aSubject.MarkDeleted();

            Assert.IsTrue(aSubject.ObjectState == ObjectState.Deleted, "Object state not properly set to 'Deleted'.");
        }

        [TestMethod]
        public void SetSubjectUnmodifiedFromModified()
        {
            EAVSubject aSubject = new EAVSubject();

            Assert.IsTrue(aSubject.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aSubject.MarkUnmodified();

            Assert.IsTrue(aSubject.ObjectState == ObjectState.Unmodified, "Object state not properly set to 'Unmodified'.");

            string identifier = Guid.NewGuid().ToString();
            aSubject.Identifier = identifier;

            Assert.AreEqual(identifier, aSubject.Identifier, "Property 'Identifier' not properly set.");
            Assert.IsTrue(aSubject.ObjectState == ObjectState.Modified, "Setting property should alter object state to 'Modified'.");

            aSubject.MarkUnmodified();

            Assert.IsTrue(aSubject.ObjectState == ObjectState.Unmodified, "Object state not properly set to 'Unmodified'.");
        }

        [TestMethod]
        public void SetSubjectDeletedFromModified()
        {
            EAVSubject aSubject = new EAVSubject();

            Assert.IsTrue(aSubject.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aSubject.MarkUnmodified();

            Assert.IsTrue(aSubject.ObjectState == ObjectState.Unmodified, "Object state not properly set to 'Unmodified'.");

            string identifier = Guid.NewGuid().ToString();
            aSubject.Identifier = identifier;

            Assert.AreEqual(identifier, aSubject.Identifier, "Property 'Identifier' not properly set.");
            Assert.IsTrue(aSubject.ObjectState == ObjectState.Modified, "Setting property should alter object state to 'Modified'.");

            aSubject.MarkDeleted();

            Assert.IsTrue(aSubject.ObjectState == ObjectState.Deleted, "Object state not properly set to 'Deleted'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SetSubjectUnmodifiedFromDeleted()
        {
            EAVSubject aSubject = new EAVSubject();

            Assert.IsTrue(aSubject.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aSubject.MarkUnmodified();

            Assert.IsTrue(aSubject.ObjectState == ObjectState.Unmodified, "Object state not properly set to 'Unmodified'.");

            aSubject.MarkDeleted();

            Assert.IsTrue(aSubject.ObjectState == ObjectState.Deleted, "Object state not properly set to 'Deleted'.");

            aSubject.MarkUnmodified();
        }

        [TestMethod]
        public void SetSubjectModifiedFromDeleted()
        {
            EAVSubject aSubject = new EAVSubject();

            Assert.IsTrue(aSubject.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aSubject.MarkUnmodified();

            Assert.IsTrue(aSubject.ObjectState == ObjectState.Unmodified, "Object state not properly set to 'Unmodified'.");

            aSubject.MarkDeleted();

            Assert.IsTrue(aSubject.ObjectState == ObjectState.Deleted, "Object state not properly set to 'Deleted'.");

            aSubject.Identifier = Guid.NewGuid().ToString();

            Assert.IsTrue(aSubject.ObjectState == ObjectState.Deleted, "Setting property should not alter object state from 'Deleted'.");
        }
        #endregion

        #region SubjectID Property Tests
        [TestMethod]
        public void TestSubjectIDForNewSubject()
        {
            EAVSubject aSubject = new EAVSubject();

            Assert.IsTrue(aSubject.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            int id = rng.Next();
            aSubject.SubjectID = id;

            Assert.AreEqual(id, aSubject.SubjectID, "Property 'SubjectID' was not set correctly.");
            Assert.IsTrue(aSubject.ObjectState == ObjectState.New, "Setting property in 'New' state should not alter object state.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestSubjectIDForUnmodifiedSubject()
        {
            EAVSubject aSubject = new EAVSubject();

            Assert.IsTrue(aSubject.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aSubject.MarkUnmodified();

            Assert.IsTrue(aSubject.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            int id = rng.Next();
            aSubject.SubjectID = id;

            Assert.AreEqual(id, aSubject.SubjectID, "Property 'SubjectID' was not set correctly.");
            Assert.IsTrue(aSubject.ObjectState == ObjectState.Unmodified, "Setting null property 'SubjectID' in 'Unmodified' state should not alter object state.");

            aSubject.SubjectID = id;
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestSubjectIDForModifiedSubject()
        {
            EAVSubject aSubject = new EAVSubject();

            int id = rng.Next();
            aSubject.SubjectID = id;

            Assert.AreEqual(id, aSubject.SubjectID, "Property 'SubjectID' was not set correctly.");

            aSubject.MarkUnmodified();

            Assert.IsTrue(aSubject.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            string identifier = Guid.NewGuid().ToString();
            aSubject.Identifier = identifier;

            Assert.IsTrue(aSubject.ObjectState == ObjectState.Modified, "Object was not correctly marked 'Modified'.");

            aSubject.SubjectID = id;
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestSubjectIDForDeletedSubject()
        {
            EAVSubject aSubject = new EAVSubject();

            aSubject.MarkUnmodified();

            Assert.IsTrue(aSubject.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            aSubject.MarkDeleted();

            Assert.IsTrue(aSubject.ObjectState == ObjectState.Deleted, "Object was not correctly marked 'Deleted'.");

            aSubject.SubjectID = rng.Next();
        }
        #endregion

        #region Identifier Property Tests
        [TestMethod]
        public void TestIdentifierForNewSubject()
        {
            EAVSubject aSubject = new EAVSubject();

            Assert.IsTrue(aSubject.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            string identifier = Guid.NewGuid().ToString();
            aSubject.Identifier = identifier;

            Assert.AreEqual(identifier, aSubject.Identifier, "Property 'Identifier' was not set correctly.");
            Assert.IsTrue(aSubject.ObjectState == ObjectState.New, "Setting property in 'New' state should not alter object state.");
        }

        [TestMethod]
        public void TestIdentifierForUnmodifiedSubject()
        {
            EAVSubject aSubject = new EAVSubject();

            Assert.IsTrue(aSubject.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aSubject.MarkUnmodified();

            Assert.IsTrue(aSubject.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            string identifier = Guid.NewGuid().ToString();
            aSubject.Identifier = identifier;

            Assert.AreEqual(identifier, aSubject.Identifier, "Property 'Identifier' was not set correctly.");
            Assert.IsTrue(aSubject.ObjectState == ObjectState.Modified, "Setting property in 'Unmodified' state should alter state to 'Modified'.");
        }

        [TestMethod]
        public void TestIdentifierForModifiedSubject()
        {
            EAVSubject aSubject = new EAVSubject();

            Assert.IsTrue(aSubject.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aSubject.MarkUnmodified();

            Assert.IsTrue(aSubject.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            string identifier = Guid.NewGuid().ToString();
            aSubject.Identifier = identifier;

            Assert.AreEqual(identifier, aSubject.Identifier, "Property 'Identifier' was not set correctly.");
            Assert.IsTrue(aSubject.ObjectState == ObjectState.Modified, "Setting property in 'Unmodified' state should alter state to 'Modified'.");

            string oldIdentifier = identifier;
            identifier = Guid.NewGuid().ToString();

            Assert.AreNotEqual(oldIdentifier, identifier, "New value selected for identifier is the same as the old value.");

            aSubject.Identifier = identifier;

            Assert.AreEqual(identifier, aSubject.Identifier, "Property 'Identifier' was not set correctly.");
            Assert.IsTrue(aSubject.ObjectState == ObjectState.Modified, "Setting property in 'Modified' state should not alter object state.");
        }

        [TestMethod]
        public void TestIdentifierForDeletedSubject()
        {
            EAVSubject aSubject = new EAVSubject();

            Assert.IsTrue(aSubject.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aSubject.MarkUnmodified();

            Assert.IsTrue(aSubject.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            aSubject.MarkDeleted();

            Assert.IsTrue(aSubject.ObjectState == ObjectState.Deleted, "Object was not correctly marked 'Deleted'.");

            aSubject.Identifier = Guid.NewGuid().ToString();

            Assert.IsNull(aSubject.Identifier, "Setting property in 'Deleted' state should not alter property value.");
            Assert.IsTrue(aSubject.ObjectState == ObjectState.Deleted, "Setting property in 'Deleted' state should not alter object state.");
        }
        #endregion

        #region Context Property Tests
        [TestMethod]
        public void TestContextForNewSubject()
        {
            EAVSubject aSubject = new EAVSubject();

            Assert.IsTrue(aSubject.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            EAVContext aContext = new EAVContext();
            aSubject.Context = aContext;

            Assert.AreEqual(aContext, aSubject.Context, "Property 'Context' was not set correctly.");
            Assert.IsTrue(aSubject.ObjectState == ObjectState.New, "Setting property in 'New' state should not alter object state.");
        }

        [TestMethod]
        public void TestContextForUnmodifiedSubject()
        {
            EAVSubject aSubject = new EAVSubject();

            Assert.IsTrue(aSubject.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aSubject.MarkUnmodified();

            Assert.IsTrue(aSubject.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            EAVContext aContext = new EAVContext();
            aSubject.Context = aContext;

            Assert.AreEqual(aContext, aSubject.Context, "Property 'Context' was not set correctly.");
            Assert.IsTrue(aSubject.ObjectState == ObjectState.Modified, "Setting property in 'Unmodified' state should alter state to 'Modified'.");
        }

        [TestMethod]
        public void TestContextForModifiedSubject()
        {
            EAVSubject aSubject = new EAVSubject();

            Assert.IsTrue(aSubject.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aSubject.MarkUnmodified();

            Assert.IsTrue(aSubject.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            EAVContext aContext = new EAVContext();
            aSubject.Context = aContext;

            Assert.AreEqual(aContext, aSubject.Context, "Property 'Context' was not set correctly.");
            Assert.IsTrue(aSubject.ObjectState == ObjectState.Modified, "Setting property in 'Unmodified' state should alter state to 'Modified'.");

            aContext = new EAVContext();
            aSubject.Context = aContext;

            Assert.AreEqual(aContext, aSubject.Context, "Property 'Context' was not set correctly.");
            Assert.IsTrue(aSubject.ObjectState == ObjectState.Modified, "Setting property in 'Modified' state should not alter object state.");
        }

        [TestMethod]
        public void TestContextForDeletedSubject()
        {
            EAVSubject aSubject = new EAVSubject();

            Assert.IsTrue(aSubject.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aSubject.MarkUnmodified();

            Assert.IsTrue(aSubject.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            aSubject.MarkDeleted();

            Assert.IsTrue(aSubject.ObjectState == ObjectState.Deleted, "Object was not correctly marked 'Deleted'.");

            EAVContext aContext = new EAVContext();
            aSubject.Context = aContext;

            Assert.AreNotEqual(aContext, aSubject.Context, "Setting property in 'Deleted' state should not alter property value.");
            Assert.IsTrue(aSubject.ObjectState == ObjectState.Deleted, "Setting property in 'Deleted' state should not alter object state.");
        }

        [TestMethod]
        public void TestContextInteractionWithSubject()
        {
            int subjectID = rng.Next();
            EAVSubject aSubject = new EAVSubject() { SubjectID = subjectID };
            int contextID = rng.Next();
            EAVContext aContext = new EAVContext() { ContextID = contextID };

            Assert.AreEqual(subjectID, aSubject.SubjectID, "Property 'SubjectID' was not set properly.");
            Assert.AreEqual(contextID, aContext.ContextID, "Property 'ContextID' was not set properly.");

            Assert.IsNull(aSubject.Context, "Property 'Context' should be null.");
            Assert.IsTrue(aContext.Subjects.Count == 0, "Collection property 'Subjects' of EAVContext object should be empty.");

            aSubject.Context = aContext;

            Assert.AreEqual(aContext, aSubject.Context, "Property 'Context' was not set properly.");
            Assert.AreEqual(contextID, aSubject.Context.ContextID, "Property 'ContextID' was not set properly.");

            Assert.IsTrue(aContext.Subjects.Count == 1, "Collection property 'Subjects' was not set properly.");
            Assert.AreEqual(subjectID, aContext.Subjects.First().SubjectID, "Property 'SubjectID' was not set properly.");

            subjectID = rng.Next();
            aSubject = new EAVSubject() { SubjectID = subjectID };
            contextID = rng.Next();
            aContext = new EAVContext() { ContextID = contextID };

            aContext.Subjects.Add(aSubject);

            Assert.AreEqual(aContext, aSubject.Context, "Property 'Context' was not set properly.");
            Assert.AreEqual(contextID, aSubject.Context.ContextID, "Property 'ContextID' was not set properly.");

            Assert.IsTrue(aContext.Subjects.Count == 1, "Collection property 'Subjects' was not set properly.");
            Assert.AreEqual(subjectID, aContext.Subjects.First().SubjectID, "Property 'SubjectID' was not set properly.");
        }
        #endregion

        #region Entity Property Tests
        [TestMethod]
        public void TestEntityForNewSubject()
        {
            EAVSubject aSubject = new EAVSubject();

            Assert.IsTrue(aSubject.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            EAVEntity aEntity = new EAVEntity();
            aSubject.Entity = aEntity;

            Assert.AreEqual(aEntity, aSubject.Entity, "Property 'Entity' was not set correctly.");
            Assert.IsTrue(aSubject.ObjectState == ObjectState.New, "Setting property in 'New' state should not alter object state.");
        }

        [TestMethod]
        public void TestEntityForUnmodifiedSubject()
        {
            EAVSubject aSubject = new EAVSubject();

            Assert.IsTrue(aSubject.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aSubject.MarkUnmodified();

            Assert.IsTrue(aSubject.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            EAVEntity aEntity = new EAVEntity();
            aSubject.Entity = aEntity;

            Assert.AreEqual(aEntity, aSubject.Entity, "Property 'Entity' was not set correctly.");
            Assert.IsTrue(aSubject.ObjectState == ObjectState.Modified, "Setting property in 'Unmodified' state should alter state to 'Modified'.");
        }

        [TestMethod]
        public void TestEntityForModifiedSubject()
        {
            EAVSubject aSubject = new EAVSubject();

            Assert.IsTrue(aSubject.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aSubject.MarkUnmodified();

            Assert.IsTrue(aSubject.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            EAVEntity aEntity = new EAVEntity();
            aSubject.Entity = aEntity;

            Assert.AreEqual(aEntity, aSubject.Entity, "Property 'Entity' was not set correctly.");
            Assert.IsTrue(aSubject.ObjectState == ObjectState.Modified, "Setting property in 'Unmodified' state should alter state to 'Modified'.");

            aEntity = new EAVEntity();
            aSubject.Entity = aEntity;

            Assert.AreEqual(aEntity, aSubject.Entity, "Property 'Entity' was not set correctly.");
            Assert.IsTrue(aSubject.ObjectState == ObjectState.Modified, "Setting property in 'Modified' state should not alter object state.");
        }

        [TestMethod]
        public void TestEntityForDeletedSubject()
        {
            EAVSubject aSubject = new EAVSubject();

            Assert.IsTrue(aSubject.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aSubject.MarkUnmodified();

            Assert.IsTrue(aSubject.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            aSubject.MarkDeleted();

            Assert.IsTrue(aSubject.ObjectState == ObjectState.Deleted, "Object was not correctly marked 'Deleted'.");

            EAVEntity aEntity = new EAVEntity();
            aSubject.Entity = aEntity;

            Assert.AreNotEqual(aEntity, aSubject.Entity, "Setting property in 'Deleted' state should not alter property value.");
            Assert.IsTrue(aSubject.ObjectState == ObjectState.Deleted, "Setting property in 'Deleted' state should not alter object state.");
        }

        [TestMethod]
        public void TestEntityInteractionWithSubject()
        {
            int subjectID = rng.Next();
            EAVSubject aSubject = new EAVSubject() { SubjectID = subjectID };
            int entityID = rng.Next();
            EAVEntity aEntity = new EAVEntity() { EntityID = entityID };

            Assert.AreEqual(subjectID, aSubject.SubjectID, "Property 'SubjectID' was not set properly.");
            Assert.AreEqual(entityID, aEntity.EntityID, "Property 'EntityID' was not set properly.");

            Assert.IsNull(aSubject.Entity, "Property 'Entity' should be null.");
            Assert.IsTrue(aEntity.Subjects.Count == 0, "Collection property 'Subjects' of EAVEntity object should be empty.");

            aSubject.Entity = aEntity;

            Assert.AreEqual(aEntity, aSubject.Entity, "Property 'Entity' was not set properly.");
            Assert.AreEqual(entityID, aSubject.Entity.EntityID, "Property 'EntityID' was not set properly.");

            Assert.IsTrue(aEntity.Subjects.Count == 1, "Collection property 'Subjects' was not set properly.");
            Assert.AreEqual(subjectID, aEntity.Subjects.First().SubjectID, "Property 'SubjectID' was not set properly.");

            subjectID = rng.Next();
            aSubject = new EAVSubject() { SubjectID = subjectID };
            entityID = rng.Next();
            aEntity = new EAVEntity() { EntityID = entityID };

            aEntity.Subjects.Add(aSubject);

            Assert.AreEqual(aEntity, aSubject.Entity, "Property 'Entity' was not set properly.");
            Assert.AreEqual(entityID, aSubject.Entity.EntityID, "Property 'EntityID' was not set properly.");

            Assert.IsTrue(aEntity.Subjects.Count == 1, "Collection property 'Subjects' was not set properly.");
            Assert.AreEqual(subjectID, aEntity.Subjects.First().SubjectID, "Property 'SubjectID' was not set properly.");
        }
        #endregion

        #region Instances Property Tests
        [TestMethod]
        public void TestInstancesForNewSubject()
        {
            EAVSubject aSubject = new EAVSubject();

            Assert.IsTrue(aSubject.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aSubject.Instances.Add(new EAVRootInstance());
            aSubject.Instances.Add(new EAVRootInstance());

            Assert.IsTrue(aSubject.Instances.Count == 2, "Property 'Instances' was not set correctly.");
            Assert.IsTrue(aSubject.ObjectState == ObjectState.New, "Modifying property in 'New' state should not alter object state.");

            aSubject.Instances.Remove(aSubject.Instances.First());

            Assert.IsTrue(aSubject.ObjectState == ObjectState.New, "Modifying property in 'New' state should not alter object state.");

            aSubject.Instances.Clear();

            Assert.IsTrue(aSubject.ObjectState == ObjectState.New, "Modifying property in 'New' state should not alter object state.");
        }

        [TestMethod]
        public void TestInstancesForUnmodifiedSubject()
        {
            EAVSubject aSubject = new EAVSubject();

            Assert.IsTrue(aSubject.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aSubject.MarkUnmodified();

            Assert.IsTrue(aSubject.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            aSubject.Instances.Add(new EAVRootInstance());
            aSubject.Instances.Add(new EAVRootInstance());

            Assert.IsTrue(aSubject.Instances.Count == 2, "Property 'Instances' was not set correctly.");
            Assert.IsTrue(aSubject.ObjectState == ObjectState.Modified, "Modifying property in 'Unmodified' state should alter object state to 'Modified'.");

            aSubject.MarkUnmodified();

            Assert.IsTrue(aSubject.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            aSubject.Instances.Remove(aSubject.Instances.First());

            Assert.IsTrue(aSubject.ObjectState == ObjectState.Modified, "Modifying property in 'Unmodified' state should alter object state to 'Modified'.");

            aSubject.MarkUnmodified();

            Assert.IsTrue(aSubject.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            aSubject.Instances.Clear();

            Assert.IsTrue(aSubject.ObjectState == ObjectState.Modified, "Modifying property in 'Unmodified' state should alter object state to 'Modified'.");
        }

        [TestMethod]
        public void TestInstancesForModifiedSubject()
        {
            EAVSubject aSubject = new EAVSubject();

            Assert.IsTrue(aSubject.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aSubject.MarkUnmodified();

            Assert.IsTrue(aSubject.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            aSubject.Identifier = Guid.NewGuid().ToString();

            Assert.IsTrue(aSubject.ObjectState == ObjectState.Modified, "Object was not correctly marked 'Modified'.");

            aSubject.Instances.Add(new EAVRootInstance());
            aSubject.Instances.Add(new EAVRootInstance());

            Assert.IsTrue(aSubject.Instances.Count == 2, "Property 'Instances' was not set correctly.");
            Assert.IsTrue(aSubject.ObjectState == ObjectState.Modified, "Modifying property in 'Modified' state should not alter object state.");

            aSubject.Instances.Remove(aSubject.Instances.First());

            Assert.IsTrue(aSubject.ObjectState == ObjectState.Modified, "Modifying property in 'Modified' state should not alter object state.");

            aSubject.Instances.Clear();

            Assert.IsTrue(aSubject.ObjectState == ObjectState.Modified, "Modifying property in 'Modified' state should not alter object state.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestInstancesForDeletedSubjectWithAdd()
        {
            EAVSubject aSubject = new EAVSubject();

            Assert.IsTrue(aSubject.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aSubject.MarkUnmodified();

            Assert.IsTrue(aSubject.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            aSubject.MarkDeleted();

            Assert.IsTrue(aSubject.ObjectState == ObjectState.Deleted, "Object was not correctly marked 'Deleted'.");

            aSubject.Instances.Add(new EAVRootInstance());
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestInstancesForDeletedSubjectWithRemove()
        {
            EAVSubject aSubject = new EAVSubject();

            Assert.IsTrue(aSubject.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aSubject.Instances.Add(new EAVRootInstance());
            aSubject.Instances.Add(new EAVRootInstance());

            aSubject.MarkUnmodified();

            foreach (EAVRootInstance childSubject in aSubject.Instances)
            {
                childSubject.MarkUnmodified();
            }

            Assert.IsTrue(aSubject.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            aSubject.MarkDeleted();

            Assert.IsTrue(aSubject.ObjectState == ObjectState.Deleted, "Object was not correctly marked 'Deleted'.");

            aSubject.Instances.Remove(aSubject.Instances.First());
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestInstancesForDeletedSubjectWithClear()
        {
            EAVSubject aSubject = new EAVSubject();

            Assert.IsTrue(aSubject.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aSubject.Instances.Add(new EAVRootInstance());

            aSubject.MarkUnmodified();

            foreach (EAVRootInstance childSubject in aSubject.Instances)
            {
                childSubject.MarkUnmodified();
            }

            Assert.IsTrue(aSubject.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            aSubject.MarkDeleted();

            Assert.IsTrue(aSubject.ObjectState == ObjectState.Deleted, "Object was not correctly marked 'Deleted'.");

            aSubject.Instances.Clear();
        }

        [TestMethod]
        public void TestInstancesInteractionWithSubject()
        {
            EAVSubject aSubject = new EAVSubject();

            int id = rng.Next();
            aSubject.SubjectID = id;

            EAVRootInstance anInstance = new EAVRootInstance();
            aSubject.Instances.Add(anInstance);

            Assert.IsTrue(aSubject.Instances.Count == 1, "Collection property 'Instances' is empty.");
            Assert.IsTrue(aSubject.Instances.Single() == anInstance, "Collection property 'Instances' not modified properly.");

            Assert.IsNotNull(anInstance.Subject, "Property 'Subject' is null.");
            Assert.AreEqual(aSubject, anInstance.Subject, "Property 'Subject' not set properly.");

            Assert.IsNotNull(anInstance.SubjectID, "Propert 'SubjectID' is null.");
            Assert.AreEqual(id, anInstance.SubjectID, "Property 'SubjectID' not set properly.");
        }
        #endregion
    }
}
