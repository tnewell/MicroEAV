using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using EAVFramework.Model;
using System.Linq;

namespace EAVFrameworkTest
{
    public partial class EAVFrameworkTestHarness
    {
        #region Object State Tests
        [TestMethod]
        public void CreateEntity()
        {
            EAVEntity aEntity = new EAVEntity();

            Assert.IsTrue(aEntity.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            Assert.IsNull(aEntity.EntityID, "Property 'EntityID' is not 'null' on creation.");

            Assert.IsNull(aEntity.Descriptor, "Property 'Descriptor' is not 'null' on creation.");

            Assert.IsNotNull(aEntity.Subjects, "Property 'Subjects' is null on creation.");
            Assert.IsTrue(aEntity.Subjects.Count == 0, "Property 'Subjects' is not empty on creation.");
        }

        [TestMethod]
        public void SetEntityUnmodifiedFromNew()
        {
            EAVEntity aEntity = new EAVEntity();

            Assert.IsTrue(aEntity.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aEntity.MarkUnmodified();

            Assert.IsTrue(aEntity.ObjectState == ObjectState.Unmodified, "Object state not properly set to 'Unmodified'.");
        }

        [TestMethod]
        public void SetEntityModifiedFromNew()
        {
            EAVEntity aEntity = new EAVEntity();

            Assert.IsTrue(aEntity.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            string descriptor = Guid.NewGuid().ToString();
            aEntity.Descriptor = descriptor;

            Assert.AreEqual(descriptor, aEntity.Descriptor, "Property 'Descriptor' not properly set.");

            Assert.IsTrue(aEntity.ObjectState == ObjectState.New, "Setting property in 'New' state should not alter object state.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SetEntityDeletedFromNew()
        {
            EAVEntity aEntity = new EAVEntity();

            Assert.IsTrue(aEntity.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aEntity.MarkDeleted();

            Assert.IsTrue(aEntity.ObjectState == ObjectState.New, "Setting object state to 'Deleted' from 'New' state should not alter object state.");
        }

        [TestMethod]
        public void SetEntityModifiedFromUnmodified()
        {
            EAVEntity aEntity = new EAVEntity();

            Assert.IsTrue(aEntity.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aEntity.MarkUnmodified();

            Assert.IsTrue(aEntity.ObjectState == ObjectState.Unmodified, "Object state not properly set to 'Unmodified'.");

            string descriptor = Guid.NewGuid().ToString();
            aEntity.Descriptor = descriptor;

            Assert.AreEqual(descriptor, aEntity.Descriptor, "Property 'Descriptor' not properly set.");
            Assert.IsTrue(aEntity.ObjectState == ObjectState.Modified, "Setting property should alter object state to 'Modified'.");
        }

        [TestMethod]
        public void SetEntityDeletedFromUnmodified()
        {
            EAVEntity aEntity = new EAVEntity();

            Assert.IsTrue(aEntity.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aEntity.MarkUnmodified();

            Assert.IsTrue(aEntity.ObjectState == ObjectState.Unmodified, "Object state not properly set to 'Unmodified'.");

            aEntity.MarkDeleted();

            Assert.IsTrue(aEntity.ObjectState == ObjectState.Deleted, "Object state not properly set to 'Deleted'.");
        }

        [TestMethod]
        public void SetEntityUnmodifiedFromModified()
        {
            EAVEntity aEntity = new EAVEntity();

            Assert.IsTrue(aEntity.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aEntity.MarkUnmodified();

            Assert.IsTrue(aEntity.ObjectState == ObjectState.Unmodified, "Object state not properly set to 'Unmodified'.");

            string descriptor = Guid.NewGuid().ToString();
            aEntity.Descriptor = descriptor;

            Assert.AreEqual(descriptor, aEntity.Descriptor, "Property 'Descriptor' not properly set.");
            Assert.IsTrue(aEntity.ObjectState == ObjectState.Modified, "Setting property should alter object state to 'Modified'.");

            aEntity.MarkUnmodified();

            Assert.IsTrue(aEntity.ObjectState == ObjectState.Unmodified, "Object state not properly set to 'Unmodified'.");
        }

        [TestMethod]
        public void SetEntityDeletedFromModified()
        {
            EAVEntity aEntity = new EAVEntity();

            Assert.IsTrue(aEntity.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aEntity.MarkUnmodified();

            Assert.IsTrue(aEntity.ObjectState == ObjectState.Unmodified, "Object state not properly set to 'Unmodified'.");

            string descriptor = Guid.NewGuid().ToString();
            aEntity.Descriptor = descriptor;

            Assert.AreEqual(descriptor, aEntity.Descriptor, "Property 'Descriptor' not properly set.");
            Assert.IsTrue(aEntity.ObjectState == ObjectState.Modified, "Setting property should alter object state to 'Modified'.");

            aEntity.MarkDeleted();

            Assert.IsTrue(aEntity.ObjectState == ObjectState.Deleted, "Object state not properly set to 'Deleted'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SetEntityUnmodifiedFromDeleted()
        {
            EAVEntity aEntity = new EAVEntity();

            Assert.IsTrue(aEntity.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aEntity.MarkUnmodified();

            Assert.IsTrue(aEntity.ObjectState == ObjectState.Unmodified, "Object state not properly set to 'Unmodified'.");

            aEntity.MarkDeleted();

            Assert.IsTrue(aEntity.ObjectState == ObjectState.Deleted, "Object state not properly set to 'Deleted'.");

            aEntity.MarkUnmodified();
        }

        [TestMethod]
        public void SetEntityModifiedFromDeleted()
        {
            EAVEntity aEntity = new EAVEntity();

            Assert.IsTrue(aEntity.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aEntity.MarkUnmodified();

            Assert.IsTrue(aEntity.ObjectState == ObjectState.Unmodified, "Object state not properly set to 'Unmodified'.");

            aEntity.MarkDeleted();

            Assert.IsTrue(aEntity.ObjectState == ObjectState.Deleted, "Object state not properly set to 'Deleted'.");

            aEntity.Descriptor = Guid.NewGuid().ToString();

            Assert.IsTrue(aEntity.ObjectState == ObjectState.Deleted, "Setting property should not alter object state from 'Deleted'.");
        }
        #endregion

        #region Entity ID Property Tests
        [TestMethod]
        public void TestEntityIDForNewEntity()
        {
            EAVEntity aEntity = new EAVEntity();

            Assert.IsTrue(aEntity.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            int id = rng.Next();
            aEntity.EntityID = id;

            Assert.AreEqual(id, aEntity.EntityID, "Property 'EntityID' was not set correctly.");
            Assert.IsTrue(aEntity.ObjectState == ObjectState.New, "Setting property in 'New' state should not alter object state.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestEntityIDForUnmodifiedEntity()
        {
            EAVEntity aEntity = new EAVEntity();

            Assert.IsTrue(aEntity.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aEntity.MarkUnmodified();

            Assert.IsTrue(aEntity.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            int id = rng.Next();
            aEntity.EntityID = id;

            Assert.AreEqual(id, aEntity.EntityID, "Property 'EntityID' was not set correctly.");
            Assert.IsTrue(aEntity.ObjectState == ObjectState.Unmodified, "Setting null property 'EntityID' in 'Unmodified' state should not alter object state.");

            aEntity.EntityID = id;
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestEntityIDForModifiedEntity()
        {
            EAVEntity aEntity = new EAVEntity();

            int id = rng.Next();
            aEntity.EntityID = id;

            Assert.AreEqual(id, aEntity.EntityID, "Property 'EntityID' was not set correctly.");

            aEntity.MarkUnmodified();

            Assert.IsTrue(aEntity.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            string descriptor = Guid.NewGuid().ToString();
            aEntity.Descriptor = descriptor;

            Assert.IsTrue(aEntity.ObjectState == ObjectState.Modified, "Object was not correctly marked 'Modified'.");

            aEntity.EntityID = id;
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestEntityIDForDeletedEntity()
        {
            EAVEntity aEntity = new EAVEntity();

            aEntity.MarkUnmodified();

            Assert.IsTrue(aEntity.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            aEntity.MarkDeleted();

            Assert.IsTrue(aEntity.ObjectState == ObjectState.Deleted, "Object was not correctly marked 'Deleted'.");

            aEntity.EntityID = rng.Next();
        }
        #endregion

        #region Descriptor Property Tests
        [TestMethod]
        public void TestDescriptorForNewEntity()
        {
            EAVEntity aEntity = new EAVEntity();

            Assert.IsTrue(aEntity.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            string descriptor = Guid.NewGuid().ToString();
            aEntity.Descriptor = descriptor;

            Assert.AreEqual(descriptor, aEntity.Descriptor, "Property 'Descriptor' was not set correctly.");
            Assert.IsTrue(aEntity.ObjectState == ObjectState.New, "Setting property in 'New' state should not alter object state.");
        }

        [TestMethod]
        public void TestDescriptorForUnmodifiedEntity()
        {
            EAVEntity aEntity = new EAVEntity();

            Assert.IsTrue(aEntity.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aEntity.MarkUnmodified();

            Assert.IsTrue(aEntity.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            string descriptor = Guid.NewGuid().ToString();
            aEntity.Descriptor = descriptor;

            Assert.AreEqual(descriptor, aEntity.Descriptor, "Property 'Descriptor' was not set correctly.");
            Assert.IsTrue(aEntity.ObjectState == ObjectState.Modified, "Setting property in 'Unmodified' state should alter state to 'Modified'.");
        }

        [TestMethod]
        public void TestDescriptorForModifiedEntity()
        {
            EAVEntity aEntity = new EAVEntity();

            Assert.IsTrue(aEntity.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aEntity.MarkUnmodified();

            Assert.IsTrue(aEntity.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            string descriptor = Guid.NewGuid().ToString();
            aEntity.Descriptor = descriptor;

            Assert.AreEqual(descriptor, aEntity.Descriptor, "Property 'Descriptor' was not set correctly.");
            Assert.IsTrue(aEntity.ObjectState == ObjectState.Modified, "Setting property in 'Unmodified' state should alter state to 'Modified'.");

            string oldDescriptor = descriptor;
            descriptor = Guid.NewGuid().ToString();

            Assert.AreNotEqual(oldDescriptor, descriptor, "New value selected for descriptor is the same as the old value.");

            aEntity.Descriptor = descriptor;

            Assert.AreEqual(descriptor, aEntity.Descriptor, "Property 'Descriptor' was not set correctly.");
            Assert.IsTrue(aEntity.ObjectState == ObjectState.Modified, "Setting property in 'Modified' state should not alter object state.");
        }

        [TestMethod]
        public void TestDescriptorForDeletedEntity()
        {
            EAVEntity aEntity = new EAVEntity();

            Assert.IsTrue(aEntity.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aEntity.MarkUnmodified();

            Assert.IsTrue(aEntity.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            aEntity.MarkDeleted();

            Assert.IsTrue(aEntity.ObjectState == ObjectState.Deleted, "Object was not correctly marked 'Deleted'.");

            aEntity.Descriptor = Guid.NewGuid().ToString();

            Assert.IsNull(aEntity.Descriptor, "Setting property in 'Deleted' state should not alter property value.");
            Assert.IsTrue(aEntity.ObjectState == ObjectState.Deleted, "Setting property in 'Deleted' state should not alter object state.");
        }
        #endregion

        #region Subjects Property Tests
        [TestMethod]
        public void TestSubjectForNewEntity()
        {
            EAVEntity aEntity = new EAVEntity();

            Assert.IsTrue(aEntity.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aEntity.Subjects.Add(new EAVSubject());
            aEntity.Subjects.Add(new EAVSubject());

            Assert.IsTrue(aEntity.Subjects.Count == 2, "Property 'Subjects' was not set correctly.");
            Assert.IsTrue(aEntity.ObjectState == ObjectState.New, "Modifying property in 'New' state should not alter object state.");

            aEntity.Subjects.Remove(aEntity.Subjects.First());

            Assert.IsTrue(aEntity.ObjectState == ObjectState.New, "Modifying property in 'New' state should not alter object state.");

            aEntity.Subjects.Clear();

            Assert.IsTrue(aEntity.ObjectState == ObjectState.New, "Modifying property in 'New' state should not alter object state.");
        }

        [TestMethod]
        public void TestSubjectForUnmodifiedEntity()
        {
            EAVEntity aEntity = new EAVEntity();

            Assert.IsTrue(aEntity.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aEntity.MarkUnmodified();

            Assert.IsTrue(aEntity.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            aEntity.Subjects.Add(new EAVSubject());
            aEntity.Subjects.Add(new EAVSubject());

            Assert.IsTrue(aEntity.Subjects.Count == 2, "Property 'Subjects' was not set correctly.");
            Assert.IsTrue(aEntity.ObjectState == ObjectState.Modified, "Modifying property in 'Unmodified' state should alter object state to 'Modified'.");

            aEntity.MarkUnmodified();

            Assert.IsTrue(aEntity.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            aEntity.Subjects.Remove(aEntity.Subjects.First());

            Assert.IsTrue(aEntity.ObjectState == ObjectState.Modified, "Modifying property in 'Unmodified' state should alter object state to 'Modified'.");

            aEntity.MarkUnmodified();

            Assert.IsTrue(aEntity.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            aEntity.Subjects.Clear();

            Assert.IsTrue(aEntity.ObjectState == ObjectState.Modified, "Modifying property in 'Unmodified' state should alter object state to 'Modified'.");
        }

        [TestMethod]
        public void TestSubjectForModifiedEntity()
        {
            EAVEntity aEntity = new EAVEntity();

            Assert.IsTrue(aEntity.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aEntity.MarkUnmodified();

            Assert.IsTrue(aEntity.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            aEntity.Descriptor = Guid.NewGuid().ToString();

            Assert.IsTrue(aEntity.ObjectState == ObjectState.Modified, "Object was not correctly marked 'Modified'.");

            aEntity.Subjects.Add(new EAVSubject());
            aEntity.Subjects.Add(new EAVSubject());

            Assert.IsTrue(aEntity.Subjects.Count == 2, "Property 'Subjects' was not set correctly.");
            Assert.IsTrue(aEntity.ObjectState == ObjectState.Modified, "Modifying property in 'Modified' state should not alter object state.");

            aEntity.Subjects.Remove(aEntity.Subjects.First());

            Assert.IsTrue(aEntity.ObjectState == ObjectState.Modified, "Modifying property in 'Modified' state should not alter object state.");

            aEntity.Subjects.Clear();

            Assert.IsTrue(aEntity.ObjectState == ObjectState.Modified, "Modifying property in 'Modified' state should not alter object state.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestSubjectForDeletedEntityWithAdd()
        {
            EAVEntity aEntity = new EAVEntity();

            Assert.IsTrue(aEntity.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aEntity.MarkUnmodified();

            Assert.IsTrue(aEntity.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            aEntity.MarkDeleted();

            Assert.IsTrue(aEntity.ObjectState == ObjectState.Deleted, "Object was not correctly marked 'Deleted'.");

            aEntity.Subjects.Add(new EAVSubject());
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestSubjectForDeletedEntityWithRemove()
        {
            EAVEntity aEntity = new EAVEntity();

            Assert.IsTrue(aEntity.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aEntity.Subjects.Add(new EAVSubject());
            aEntity.Subjects.Add(new EAVSubject());

            aEntity.MarkUnmodified();

            foreach (EAVSubject subject in aEntity.Subjects)
                subject.MarkUnmodified();

            Assert.IsTrue(aEntity.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            aEntity.MarkDeleted();

            Assert.IsTrue(aEntity.ObjectState == ObjectState.Deleted, "Object was not correctly marked 'Deleted'.");

            aEntity.Subjects.Remove(aEntity.Subjects.First());
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestSubjectForDeletedEntityWithClear()
        {
            EAVEntity aEntity = new EAVEntity();

            Assert.IsTrue(aEntity.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aEntity.Subjects.Add(new EAVSubject());

            aEntity.MarkUnmodified();

            foreach (EAVSubject subject in aEntity.Subjects)
                subject.MarkUnmodified();

            Assert.IsTrue(aEntity.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            aEntity.MarkDeleted();

            Assert.IsTrue(aEntity.ObjectState == ObjectState.Deleted, "Object was not correctly marked 'Deleted'.");

            aEntity.Subjects.Clear();
        }

        [TestMethod]
        public void TestSubjectInteractionWithEntity()
        {
            EAVEntity aEntity = new EAVEntity();

            int id = rng.Next();
            aEntity.EntityID = id;

            EAVSubject aSubject = new EAVSubject();
            aEntity.Subjects.Add(aSubject);

            Assert.IsTrue(aEntity.Subjects.Count == 1, "Collection property 'Subjects' is empty.");
            Assert.IsTrue(aEntity.Subjects.Single() == aSubject, "Collection property 'Subjects' not modified properly.");

            Assert.IsNotNull(aSubject.Entity, "Property 'Entity' is null.");
            Assert.AreEqual(aEntity, aSubject.Entity, "Property 'Entity' not set properly.");

            Assert.IsNotNull(aSubject.EntityID, "Propert 'EntityID' is null.");
            Assert.AreEqual(id, aSubject.EntityID, "Property 'EntityID' not set properly.");
        }
        #endregion
    }
}
