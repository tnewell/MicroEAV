using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using EAVFramework.Model;
using System.Linq;


namespace EAVFrameworkTest
{
    public partial class EAVFrameworkTestHarness
    {
        [TestMethod]
        public void EntityCreate()
        {
            EAVEntity anEntity = new EAVEntity();

            Assert.AreEqual(ObjectState.New, anEntity.ObjectState, "Object state should be 'New' on creation.");

            Assert.IsNull(anEntity.EntityID, "Property 'EntityID' should be null on creation.");

            Assert.IsNull(anEntity.Descriptor, "Property 'Descriptor' should be null on creation.");

            Assert.IsNotNull(anEntity.Subjects, "Property 'Subjects' should not be null on creation.");
            Assert.IsFalse(anEntity.Subjects.Any(), "Property 'Subjects' should be empty on creation.");
        }

        #region State Transitions
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void EntityStateTransitionNewToUnmodifiedWithNullID()
        {
            EAVEntity anEntity = new EAVEntity();

            Assert.AreEqual(ObjectState.New, anEntity.ObjectState, "Object state should be 'New' on creation.");

            anEntity.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anEntity.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        public void EntityStateTransitionNewToUnmodifiedWithValidID()
        {
            EAVEntity anEntity = new EAVEntity() { EntityID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anEntity.ObjectState, "Object state should be 'New' on creation.");

            anEntity.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anEntity.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void EntityStateTransitionNewToDeleted()
        {
            EAVEntity anEntity = new EAVEntity();

            Assert.AreEqual(ObjectState.New, anEntity.ObjectState, "Object state should be 'New' on creation.");

            anEntity.MarkDeleted();
        }

        [TestMethod]
        public void EntityStateTransitionUnmodifiedToDeleted()
        {
            EAVEntity anEntity = new EAVEntity() { EntityID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anEntity.ObjectState, "Object state should be 'New' on creation.");

            anEntity.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anEntity.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anEntity.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, anEntity.ObjectState, "Object state failed to transition to 'Deleted'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void EntityStateTransitionDeletedToUnmodified()
        {
            EAVEntity anEntity = new EAVEntity() { EntityID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anEntity.ObjectState, "Object state should be 'New' on creation.");

            anEntity.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anEntity.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anEntity.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, anEntity.ObjectState, "Object state failed to transition to 'Deleted'.");

            anEntity.MarkUnmodified();
        }
        #endregion

        #region ID Property
        [TestMethod]
        public void EntitySetIDWhenNew()
        {
            EAVEntity anEntity = new EAVEntity();

            Assert.AreEqual(ObjectState.New, anEntity.ObjectState, "Object state should be 'New' on creation.");

            int id = rng.Next();
            anEntity.EntityID = id;

            Assert.AreEqual(id, anEntity.EntityID, "Property 'EntityID' not properly set.");
            Assert.AreEqual(ObjectState.New, anEntity.ObjectState, "Object state after setting property 'EntityID' should be 'New'.");
        }

        [TestMethod]
        public void EntitySetIDBeforeUnmodified()
        {
            EAVEntity anEntity = new EAVEntity();

            Assert.AreEqual(ObjectState.New, anEntity.ObjectState, "Object state should be 'New' on creation.");

            int id = rng.Next();
            anEntity.EntityID = id;

            Assert.AreEqual(id, anEntity.EntityID, "Property 'EntityID' not properly set.");
            Assert.AreEqual(ObjectState.New, anEntity.ObjectState, "Object state after setting property 'EntityID' should be 'New'.");

            anEntity.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anEntity.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void EntitySetIDAfterUnmodified()
        {
            EAVEntity anEntity = new EAVEntity();

            Assert.AreEqual(ObjectState.New, anEntity.ObjectState, "Object state should be 'New' on creation.");

            anEntity.MarkUnmodified();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void EntitySetIDWhenDeleted()
        {
            EAVEntity anEntity = new EAVEntity();

            Assert.AreEqual(ObjectState.New, anEntity.ObjectState, "Object state should be 'New' on creation.");

            int id = rng.Next();
            anEntity.EntityID = id;

            Assert.AreEqual(id, anEntity.EntityID, "Property 'EntityID' not properly set.");
            Assert.AreEqual(ObjectState.New, anEntity.ObjectState, "Object state after setting property 'EntityID' should be 'New'.");

            anEntity.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anEntity.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anEntity.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, anEntity.ObjectState, "Object state failed to transition to 'Unmodified'.");

            id = rng.Next();
            anEntity.EntityID = id;
        }
        #endregion

        #region Primitive Properties
        #region Descriptor
        [TestMethod]
        public void EntitySetDescriptorWhenNew()
        {
            EAVEntity anEntity = new EAVEntity();

            Assert.AreEqual(ObjectState.New, anEntity.ObjectState, "Object state should be 'New' on creation.");

            string value = Guid.NewGuid().ToString();
            anEntity.Descriptor = value;

            Assert.AreEqual(value, anEntity.Descriptor, "Property 'Descriptor' was not set properly.");
            Assert.AreEqual(ObjectState.New, anEntity.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void EntitySetDescriptorWhenUnmodified()
        {
            EAVEntity anEntity = new EAVEntity() { EntityID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anEntity.ObjectState, "Object state should be 'New' on creation.");

            anEntity.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anEntity.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            anEntity.Descriptor = value;

            Assert.AreEqual(value, anEntity.Descriptor, "Property 'Descriptor' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, anEntity.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void EntitySetDescriptorWhenModified()
        {
            EAVEntity anEntity = new EAVEntity() { EntityID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anEntity.ObjectState, "Object state should be 'New' on creation.");

            anEntity.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anEntity.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            anEntity.Descriptor = value;

            Assert.AreEqual(value, anEntity.Descriptor, "Property 'Descriptor' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, anEntity.ObjectState, "Object state failed to transition to 'Modified'.");

            value = Guid.NewGuid().ToString();
            anEntity.Descriptor = value;

            Assert.AreEqual(value, anEntity.Descriptor, "Property 'Descriptor' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, anEntity.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void EntitySetDescriptorWhenDeleted()
        {
            EAVEntity anEntity = new EAVEntity() { EntityID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anEntity.ObjectState, "Object state should be 'New' on creation.");

            anEntity.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anEntity.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anEntity.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, anEntity.ObjectState, "Object state failed to transition to 'Deleted'.");

            anEntity.Descriptor = Guid.NewGuid().ToString();
        }
        #endregion
        #endregion

        #region Object Properties
        // N/A
        #endregion

        #region Collection Properties

        #region Subjects
        [TestMethod]
        public void EntityAddToSubjectsWhenNew()
        {
            EAVEntity anEntity = new EAVEntity() { EntityID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anEntity.ObjectState, "Object state should be 'New' on creation.");

            EAVSubject value = new EAVSubject() { SubjectID = rng.Next() };
            anEntity.Subjects.Add(value);

            Assert.IsTrue(anEntity.Subjects.Contains(value), "Property 'Subjects' was not updated properly.");
            Assert.AreEqual(anEntity, value.Entity, "Property 'Entity' was not set properly.");
            Assert.AreEqual(anEntity.EntityID, value.EntityID, "Property 'EntityID' was not set properly.");
            Assert.AreEqual(ObjectState.New, anEntity.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void EntityAddToSubjectsWhenUnmodified()
        {
            EAVEntity anEntity = new EAVEntity() { EntityID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anEntity.ObjectState, "Object state should be 'New' on creation.");

            anEntity.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anEntity.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVSubject value = new EAVSubject() { SubjectID = rng.Next() };
            anEntity.Subjects.Add(value);

            Assert.IsTrue(anEntity.Subjects.Contains(value), "Property 'Subjects' was not updated properly.");
            Assert.AreEqual(anEntity, value.Entity, "Property 'Entity' was not set properly.");
            Assert.AreEqual(anEntity.EntityID, value.EntityID, "Property 'EntityID' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, anEntity.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void EntityAddToSubjectsWhenModified()
        {
            EAVEntity anEntity = new EAVEntity() { EntityID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anEntity.ObjectState, "Object state should be 'New' on creation.");

            anEntity.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anEntity.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVSubject value = new EAVSubject() { SubjectID = rng.Next() };
            anEntity.Subjects.Add(value);

            Assert.IsTrue(anEntity.Subjects.Contains(value), "Property 'Subjects' was not updated properly.");
            Assert.AreEqual(anEntity, value.Entity, "Property 'Entity' was not set properly.");
            Assert.AreEqual(anEntity.EntityID, value.EntityID, "Property 'EntityID' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, anEntity.ObjectState, "Object state failed to transition to 'Modified'.");

            value = new EAVSubject() { SubjectID = rng.Next() };
            anEntity.Subjects.Add(value);

            Assert.IsTrue(anEntity.Subjects.Contains(value), "Property 'Subjects' was not updated properly.");
            Assert.AreEqual(anEntity, value.Entity, "Property 'Entity' was not set properly.");
            Assert.AreEqual(anEntity.EntityID, value.EntityID, "Property 'EntityID' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, anEntity.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void EntityAddToSubjectsWhenDeleted()
        {
            EAVEntity anEntity = new EAVEntity() { EntityID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anEntity.ObjectState, "Object state should be 'New' on creation.");

            anEntity.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anEntity.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anEntity.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, anEntity.ObjectState, "Object state failed to transition to 'Deleted'.");

            anEntity.Subjects.Add(new EAVSubject() { SubjectID = rng.Next() });
        }
        #endregion
        #endregion
    }
}
