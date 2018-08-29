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
            ModelEntity anEntity = new ModelEntity();

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
            ModelEntity anEntity = new ModelEntity();

            Assert.AreEqual(ObjectState.New, anEntity.ObjectState, "Object state should be 'New' on creation.");

            anEntity.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anEntity.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        public void EntityStateTransitionNewToUnmodifiedWithValidID()
        {
            ModelEntity anEntity = new ModelEntity() { EntityID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anEntity.ObjectState, "Object state should be 'New' on creation.");

            anEntity.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anEntity.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void EntityStateTransitionNewToDeleted()
        {
            ModelEntity anEntity = new ModelEntity();

            Assert.AreEqual(ObjectState.New, anEntity.ObjectState, "Object state should be 'New' on creation.");

            anEntity.MarkDeleted();
        }

        [TestMethod]
        public void EntityStateTransitionUnmodifiedToDeleted()
        {
            ModelEntity anEntity = new ModelEntity() { EntityID = rng.Next() };

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
            ModelEntity anEntity = new ModelEntity() { EntityID = rng.Next() };

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
            ModelEntity anEntity = new ModelEntity();

            Assert.AreEqual(ObjectState.New, anEntity.ObjectState, "Object state should be 'New' on creation.");

            int id = rng.Next();
            anEntity.EntityID = id;

            Assert.AreEqual(id, anEntity.EntityID, "Property 'EntityID' not properly set.");
            Assert.AreEqual(ObjectState.New, anEntity.ObjectState, "Object state after setting property 'EntityID' should be 'New'.");
        }

        [TestMethod]
        public void EntitySetIDBeforeUnmodified()
        {
            ModelEntity anEntity = new ModelEntity();

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
            ModelEntity anEntity = new ModelEntity();

            Assert.AreEqual(ObjectState.New, anEntity.ObjectState, "Object state should be 'New' on creation.");

            anEntity.MarkUnmodified();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void EntitySetIDWhenDeleted()
        {
            ModelEntity anEntity = new ModelEntity();

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
            ModelEntity anEntity = new ModelEntity();

            Assert.AreEqual(ObjectState.New, anEntity.ObjectState, "Object state should be 'New' on creation.");

            string value = Guid.NewGuid().ToString();
            anEntity.Descriptor = value;

            Assert.AreEqual(value, anEntity.Descriptor, "Property 'Descriptor' was not set properly.");
            Assert.AreEqual(ObjectState.New, anEntity.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void EntitySetDescriptorWhenUnmodified()
        {
            ModelEntity anEntity = new ModelEntity() { EntityID = rng.Next() };

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
            ModelEntity anEntity = new ModelEntity() { EntityID = rng.Next() };

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
            ModelEntity anEntity = new ModelEntity() { EntityID = rng.Next() };

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
            ModelEntity anEntity = new ModelEntity() { EntityID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anEntity.ObjectState, "Object state should be 'New' on creation.");

            ModelSubject value = new ModelSubject() { SubjectID = rng.Next() };
            anEntity.Subjects.Add(value);

            Assert.IsTrue(anEntity.Subjects.Contains(value), "Property 'Subjects' was not updated properly.");
            Assert.AreEqual(anEntity, value.Entity, "Property 'Entity' was not set properly.");
            Assert.AreEqual(anEntity.EntityID, value.EntityID, "Property 'EntityID' was not set properly.");
            Assert.AreEqual(ObjectState.New, anEntity.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void EntityAddToSubjectsWhenUnmodified()
        {
            ModelEntity anEntity = new ModelEntity() { EntityID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anEntity.ObjectState, "Object state should be 'New' on creation.");

            anEntity.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anEntity.ObjectState, "Object state failed to transition to 'Unmodified'.");

            ModelSubject value = new ModelSubject() { SubjectID = rng.Next() };
            anEntity.Subjects.Add(value);

            Assert.IsTrue(anEntity.Subjects.Contains(value), "Property 'Subjects' was not updated properly.");
            Assert.AreEqual(anEntity, value.Entity, "Property 'Entity' was not set properly.");
            Assert.AreEqual(anEntity.EntityID, value.EntityID, "Property 'EntityID' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, anEntity.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void EntityAddToSubjectsWhenModified()
        {
            ModelEntity anEntity = new ModelEntity() { EntityID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anEntity.ObjectState, "Object state should be 'New' on creation.");

            anEntity.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anEntity.ObjectState, "Object state failed to transition to 'Unmodified'.");

            ModelSubject value = new ModelSubject() { SubjectID = rng.Next() };
            anEntity.Subjects.Add(value);

            Assert.IsTrue(anEntity.Subjects.Contains(value), "Property 'Subjects' was not updated properly.");
            Assert.AreEqual(anEntity, value.Entity, "Property 'Entity' was not set properly.");
            Assert.AreEqual(anEntity.EntityID, value.EntityID, "Property 'EntityID' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, anEntity.ObjectState, "Object state failed to transition to 'Modified'.");

            value = new ModelSubject() { SubjectID = rng.Next() };
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
            ModelEntity anEntity = new ModelEntity() { EntityID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anEntity.ObjectState, "Object state should be 'New' on creation.");

            anEntity.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anEntity.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anEntity.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, anEntity.ObjectState, "Object state failed to transition to 'Deleted'.");

            anEntity.Subjects.Add(new ModelSubject() { SubjectID = rng.Next() });
        }

        [TestMethod]
        public void EntityRemoveFromSubjectsWhenNew()
        {
            ModelEntity anEntity = new ModelEntity() { EntityID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anEntity.ObjectState, "Object state should be 'New' on creation.");

            ModelSubject value = new ModelSubject() { SubjectID = rng.Next() };
            anEntity.Subjects.Add(value);
            value = new ModelSubject() { SubjectID = rng.Next() };
            anEntity.Subjects.Add(value);

            Assert.IsTrue(anEntity.Subjects.Contains(value), "Property 'Subjects' was not updated properly.");
            Assert.AreEqual(anEntity, value.Entity, "Property 'Entity' was not set properly.");
            Assert.AreEqual(anEntity.EntityID, value.EntityID, "Property 'EntityID' was not reported properly.");
            Assert.AreEqual(ObjectState.New, anEntity.ObjectState, "Object state should remain 'New' when property set.");

            anEntity.Subjects.Remove(value);

            Assert.IsFalse(anEntity.Subjects.Contains(value), "Property 'Subjects' was not updated properly.");
            Assert.IsNull(value.Entity, "Property 'Entity' was not set properly.");
            Assert.IsNull(value.EntityID, "Property 'EntityID' was not reported properly.");
            Assert.AreEqual(ObjectState.New, anEntity.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void EntityRemoveFromSubjectsWhenUnmodified()
        {
            ModelEntity anEntity = new ModelEntity() { EntityID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anEntity.ObjectState, "Object state should be 'New' on creation.");

            ModelSubject value = new ModelSubject() { SubjectID = rng.Next() };
            anEntity.Subjects.Add(value);
            value = new ModelSubject() { SubjectID = rng.Next() };
            anEntity.Subjects.Add(value);

            Assert.IsTrue(anEntity.Subjects.Contains(value), "Property 'Subjects' was not updated properly.");
            Assert.AreEqual(anEntity, value.Entity, "Property 'Entity' was not set properly.");
            Assert.AreEqual(anEntity.EntityID, value.EntityID, "Property 'EntityID' was not reported properly.");
            Assert.AreEqual(ObjectState.New, anEntity.ObjectState, "Object state should remain 'New' when property set.");

            anEntity.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anEntity.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anEntity.Subjects.Remove(value);

            Assert.IsFalse(anEntity.Subjects.Contains(value), "Property 'Subjects' was not updated properly.");
            Assert.IsNull(value.Entity, "Property 'Entity' was not set properly.");
            Assert.IsNull(value.EntityID, "Property 'EntityID' was not reported properly.");
            Assert.AreEqual(ObjectState.Modified, anEntity.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void EntityRemoveFromSubjectsWhenModified()
        {
            ModelEntity anEntity = new ModelEntity() { EntityID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anEntity.ObjectState, "Object state should be 'New' on creation.");

            anEntity.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anEntity.ObjectState, "Object state failed to transition to 'Unmodified'.");

            ModelSubject value = new ModelSubject() { SubjectID = rng.Next() };
            anEntity.Subjects.Add(value);
            value = new ModelSubject() { SubjectID = rng.Next() };
            anEntity.Subjects.Add(value);

            Assert.IsTrue(anEntity.Subjects.Contains(value), "Property 'Subjects' was not updated properly.");
            Assert.AreEqual(anEntity, value.Entity, "Property 'Entity' was not set properly.");
            Assert.AreEqual(anEntity.EntityID, value.EntityID, "Property 'EntityID' was not reported properly.");
            Assert.AreEqual(ObjectState.Modified, anEntity.ObjectState, "Object state failed to transition to 'Modified'.");

            anEntity.Subjects.Remove(value);

            Assert.IsFalse(anEntity.Subjects.Contains(value), "Property 'Subjects' was not updated properly.");
            Assert.IsNull(value.Entity, "Property 'Entity' was not set properly.");
            Assert.IsNull(value.EntityID, "Property 'EntityID' was not reported properly.");
            Assert.AreEqual(ObjectState.Modified, anEntity.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void EntityRemoveFromSubjectsWhenDeleted()
        {
            ModelEntity anEntity = new ModelEntity() { EntityID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anEntity.ObjectState, "Object state should be 'New' on creation.");

            ModelSubject value = new ModelSubject() { SubjectID = rng.Next() };
            anEntity.Subjects.Add(value);

            Assert.IsTrue(anEntity.Subjects.Contains(value), "Property 'Subjects' was not updated properly.");
            Assert.AreEqual(anEntity, value.Entity, "Property 'Entity' was not set properly.");
            Assert.AreEqual(anEntity.EntityID, value.EntityID, "Property 'EntityID' was not reported properly.");
            Assert.AreEqual(ObjectState.New, anEntity.ObjectState, "Object state should remain 'New' when property set.");

            anEntity.MarkUnmodified();
            value.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anEntity.ObjectState, "Object state failed to transition to 'Unmodified'.");
            Assert.AreEqual(ObjectState.Unmodified, value.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anEntity.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, anEntity.ObjectState, "Object state failed to transition to 'Deleted'.");
            Assert.AreEqual(ObjectState.Deleted, value.ObjectState, "Object state failed to transition to 'Deleted'.");

            anEntity.Subjects.Remove(value);
        }

        [TestMethod]
        public void EntityClearSubjectsWhenNew()
        {
            ModelEntity anEntity = new ModelEntity() { EntityID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anEntity.ObjectState, "Object state should be 'New' on creation.");

            ModelSubject value = new ModelSubject() { SubjectID = rng.Next() };
            anEntity.Subjects.Add(value);
            value = new ModelSubject() { SubjectID = rng.Next() };
            anEntity.Subjects.Add(value);

            Assert.IsTrue(anEntity.Subjects.Contains(value), "Property 'Subjects' was not updated properly.");
            Assert.AreEqual(anEntity, value.Entity, "Property 'Entity' was not set properly.");
            Assert.AreEqual(anEntity.EntityID, value.EntityID, "Property 'EntityID' was not reported properly.");
            Assert.AreEqual(ObjectState.New, anEntity.ObjectState, "Object state should remain 'New' when property set.");

            anEntity.Subjects.Clear();

            Assert.IsFalse(anEntity.Subjects.Contains(value), "Property 'Subjects' was not updated properly.");
            Assert.IsNull(value.Entity, "Property 'Entity' was not set properly.");
            Assert.IsNull(value.EntityID, "Property 'EntityID' was not reported properly.");
            Assert.AreEqual(ObjectState.New, anEntity.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void EntityClearSubjectsWhenUnmodified()
        {
            ModelEntity anEntity = new ModelEntity() { EntityID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anEntity.ObjectState, "Object state should be 'New' on creation.");

            ModelSubject value = new ModelSubject() { SubjectID = rng.Next() };
            anEntity.Subjects.Add(value);
            value = new ModelSubject() { SubjectID = rng.Next() };
            anEntity.Subjects.Add(value);

            Assert.IsTrue(anEntity.Subjects.Contains(value), "Property 'Subjects' was not updated properly.");
            Assert.AreEqual(anEntity, value.Entity, "Property 'Entity' was not set properly.");
            Assert.AreEqual(anEntity.EntityID, value.EntityID, "Property 'EntityID' was not reported properly.");
            Assert.AreEqual(ObjectState.New, anEntity.ObjectState, "Object state should remain 'New' when property set.");

            anEntity.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anEntity.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anEntity.Subjects.Clear();

            Assert.IsFalse(anEntity.Subjects.Contains(value), "Property 'Subjects' was not updated properly.");
            Assert.IsNull(value.Entity, "Property 'Entity' was not set properly.");
            Assert.IsNull(value.EntityID, "Property 'EntityID' was not reported properly.");
            Assert.AreEqual(ObjectState.Modified, anEntity.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void EntityClearSubjectsWhenModified()
        {
            ModelEntity anEntity = new ModelEntity() { EntityID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anEntity.ObjectState, "Object state should be 'New' on creation.");

            anEntity.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anEntity.ObjectState, "Object state failed to transition to 'Unmodified'.");

            ModelSubject value = new ModelSubject() { SubjectID = rng.Next() };
            anEntity.Subjects.Add(value);
            value = new ModelSubject() { SubjectID = rng.Next() };
            anEntity.Subjects.Add(value);

            Assert.IsTrue(anEntity.Subjects.Contains(value), "Property 'Subjects' was not updated properly.");
            Assert.AreEqual(anEntity, value.Entity, "Property 'Entity' was not set properly.");
            Assert.AreEqual(anEntity.EntityID, value.EntityID, "Property 'EntityID' was not reported properly.");
            Assert.AreEqual(ObjectState.Modified, anEntity.ObjectState, "Object state failed to transition to 'Modified'.");

            anEntity.Subjects.Clear();

            Assert.IsFalse(anEntity.Subjects.Contains(value), "Property 'Subjects' was not updated properly.");
            Assert.IsNull(value.Entity, "Property 'Entity' was not set properly.");
            Assert.IsNull(value.EntityID, "Property 'EntityID' was not reported properly.");
            Assert.AreEqual(ObjectState.Modified, anEntity.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void EntityClearSubjectsWhenDeleted()
        {
            ModelEntity anEntity = new ModelEntity() { EntityID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anEntity.ObjectState, "Object state should be 'New' on creation.");

            ModelSubject value = new ModelSubject() { SubjectID = rng.Next() };
            anEntity.Subjects.Add(value);

            Assert.IsTrue(anEntity.Subjects.Contains(value), "Property 'Subjects' was not updated properly.");
            Assert.AreEqual(anEntity, value.Entity, "Property 'Entity' was not set properly.");
            Assert.AreEqual(anEntity.EntityID, value.EntityID, "Property 'EntityID' was not reported properly.");
            Assert.AreEqual(ObjectState.New, anEntity.ObjectState, "Object state should remain 'New' when property set.");

            anEntity.MarkUnmodified();
            value.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anEntity.ObjectState, "Object state failed to transition to 'Unmodified'.");
            Assert.AreEqual(ObjectState.Unmodified, value.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anEntity.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, anEntity.ObjectState, "Object state failed to transition to 'Deleted'.");
            Assert.AreEqual(ObjectState.Deleted, value.ObjectState, "Object state failed to transition to 'Deleted'.");

            anEntity.Subjects.Clear();
        }
        #endregion
        #endregion
    }
}
