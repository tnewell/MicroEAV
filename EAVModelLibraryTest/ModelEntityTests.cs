using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace EAVModelTest
{
    public partial class EAVModelTestHarness
    {
        [TestMethod]
        public void EntityCreate()
        {
            EAVModelLibrary.ModelEntity anEntity = new EAVModelLibrary.ModelEntity();

            Assert.AreEqual(EAV.Model.ObjectState.New, anEntity.ObjectState, "Object state should be 'New' on creation.");

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
            EAVModelLibrary.ModelEntity anEntity = new EAVModelLibrary.ModelEntity();

            Assert.AreEqual(EAV.Model.ObjectState.New, anEntity.ObjectState, "Object state should be 'New' on creation.");

            anEntity.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, anEntity.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        public void EntityStateTransitionNewToUnmodifiedWithValidID()
        {
            EAVModelLibrary.ModelEntity anEntity = new EAVModelLibrary.ModelEntity() { EntityID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, anEntity.ObjectState, "Object state should be 'New' on creation.");

            anEntity.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, anEntity.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void EntityStateTransitionNewToDeleted()
        {
            EAVModelLibrary.ModelEntity anEntity = new EAVModelLibrary.ModelEntity();

            Assert.AreEqual(EAV.Model.ObjectState.New, anEntity.ObjectState, "Object state should be 'New' on creation.");

            anEntity.MarkDeleted();
        }

        [TestMethod]
        public void EntityStateTransitionUnmodifiedToDeleted()
        {
            EAVModelLibrary.ModelEntity anEntity = new EAVModelLibrary.ModelEntity() { EntityID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, anEntity.ObjectState, "Object state should be 'New' on creation.");

            anEntity.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, anEntity.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anEntity.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, anEntity.ObjectState, "Object state failed to transition to 'Deleted'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void EntityStateTransitionDeletedToUnmodified()
        {
            EAVModelLibrary.ModelEntity anEntity = new EAVModelLibrary.ModelEntity() { EntityID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, anEntity.ObjectState, "Object state should be 'New' on creation.");

            anEntity.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, anEntity.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anEntity.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, anEntity.ObjectState, "Object state failed to transition to 'Deleted'.");

            anEntity.MarkUnmodified();
        }
        #endregion

        #region ID Property
        [TestMethod]
        public void EntitySetIDWhenNew()
        {
            EAVModelLibrary.ModelEntity anEntity = new EAVModelLibrary.ModelEntity();

            Assert.AreEqual(EAV.Model.ObjectState.New, anEntity.ObjectState, "Object state should be 'New' on creation.");

            int id = rng.Next();
            anEntity.EntityID = id;

            Assert.AreEqual(id, anEntity.EntityID, "Property 'EntityID' not properly set.");
            Assert.AreEqual(EAV.Model.ObjectState.New, anEntity.ObjectState, "Object state after setting property 'EntityID' should be 'New'.");
        }

        [TestMethod]
        public void EntitySetIDBeforeUnmodified()
        {
            EAVModelLibrary.ModelEntity anEntity = new EAVModelLibrary.ModelEntity();

            Assert.AreEqual(EAV.Model.ObjectState.New, anEntity.ObjectState, "Object state should be 'New' on creation.");

            int id = rng.Next();
            anEntity.EntityID = id;

            Assert.AreEqual(id, anEntity.EntityID, "Property 'EntityID' not properly set.");
            Assert.AreEqual(EAV.Model.ObjectState.New, anEntity.ObjectState, "Object state after setting property 'EntityID' should be 'New'.");

            anEntity.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, anEntity.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void EntitySetIDAfterUnmodified()
        {
            EAVModelLibrary.ModelEntity anEntity = new EAVModelLibrary.ModelEntity();

            Assert.AreEqual(EAV.Model.ObjectState.New, anEntity.ObjectState, "Object state should be 'New' on creation.");

            anEntity.MarkUnmodified();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void EntitySetIDWhenDeleted()
        {
            EAVModelLibrary.ModelEntity anEntity = new EAVModelLibrary.ModelEntity();

            Assert.AreEqual(EAV.Model.ObjectState.New, anEntity.ObjectState, "Object state should be 'New' on creation.");

            int id = rng.Next();
            anEntity.EntityID = id;

            Assert.AreEqual(id, anEntity.EntityID, "Property 'EntityID' not properly set.");
            Assert.AreEqual(EAV.Model.ObjectState.New, anEntity.ObjectState, "Object state after setting property 'EntityID' should be 'New'.");

            anEntity.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, anEntity.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anEntity.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, anEntity.ObjectState, "Object state failed to transition to 'Unmodified'.");

            id = rng.Next();
            anEntity.EntityID = id;
        }
        #endregion

        #region Primitive Properties
        #region Descriptor
        [TestMethod]
        public void EntitySetDescriptorWhenNew()
        {
            EAVModelLibrary.ModelEntity anEntity = new EAVModelLibrary.ModelEntity();

            Assert.AreEqual(EAV.Model.ObjectState.New, anEntity.ObjectState, "Object state should be 'New' on creation.");

            string value = Guid.NewGuid().ToString();
            anEntity.Descriptor = value;

            Assert.AreEqual(value, anEntity.Descriptor, "Property 'Descriptor' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, anEntity.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void EntitySetDescriptorWhenUnmodified()
        {
            EAVModelLibrary.ModelEntity anEntity = new EAVModelLibrary.ModelEntity() { EntityID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, anEntity.ObjectState, "Object state should be 'New' on creation.");

            anEntity.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, anEntity.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            anEntity.Descriptor = value;

            Assert.AreEqual(value, anEntity.Descriptor, "Property 'Descriptor' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, anEntity.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void EntitySetDescriptorWhenModified()
        {
            EAVModelLibrary.ModelEntity anEntity = new EAVModelLibrary.ModelEntity() { EntityID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, anEntity.ObjectState, "Object state should be 'New' on creation.");

            anEntity.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, anEntity.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            anEntity.Descriptor = value;

            Assert.AreEqual(value, anEntity.Descriptor, "Property 'Descriptor' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, anEntity.ObjectState, "Object state failed to transition to 'Modified'.");

            value = Guid.NewGuid().ToString();
            anEntity.Descriptor = value;

            Assert.AreEqual(value, anEntity.Descriptor, "Property 'Descriptor' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, anEntity.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void EntitySetDescriptorWhenDeleted()
        {
            EAVModelLibrary.ModelEntity anEntity = new EAVModelLibrary.ModelEntity() { EntityID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, anEntity.ObjectState, "Object state should be 'New' on creation.");

            anEntity.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, anEntity.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anEntity.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, anEntity.ObjectState, "Object state failed to transition to 'Deleted'.");

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
            EAVModelLibrary.ModelEntity anEntity = new EAVModelLibrary.ModelEntity() { EntityID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, anEntity.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelSubject value = new EAVModelLibrary.ModelSubject() { SubjectID = rng.Next() };
            anEntity.Subjects.Add(value);

            Assert.IsTrue(anEntity.Subjects.Contains(value), "Property 'Subjects' was not updated properly.");
            Assert.AreEqual(anEntity, value.Entity, "Property 'Entity' was not set properly.");
            Assert.AreEqual(anEntity.EntityID, value.EntityID, "Property 'EntityID' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, anEntity.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void EntityAddToSubjectsWhenUnmodified()
        {
            EAVModelLibrary.ModelEntity anEntity = new EAVModelLibrary.ModelEntity() { EntityID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, anEntity.ObjectState, "Object state should be 'New' on creation.");

            anEntity.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, anEntity.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVModelLibrary.ModelSubject value = new EAVModelLibrary.ModelSubject() { SubjectID = rng.Next() };
            anEntity.Subjects.Add(value);

            Assert.IsTrue(anEntity.Subjects.Contains(value), "Property 'Subjects' was not updated properly.");
            Assert.AreEqual(anEntity, value.Entity, "Property 'Entity' was not set properly.");
            Assert.AreEqual(anEntity.EntityID, value.EntityID, "Property 'EntityID' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, anEntity.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void EntityAddToSubjectsWhenModified()
        {
            EAVModelLibrary.ModelEntity anEntity = new EAVModelLibrary.ModelEntity() { EntityID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, anEntity.ObjectState, "Object state should be 'New' on creation.");

            anEntity.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, anEntity.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVModelLibrary.ModelSubject value = new EAVModelLibrary.ModelSubject() { SubjectID = rng.Next() };
            anEntity.Subjects.Add(value);

            Assert.IsTrue(anEntity.Subjects.Contains(value), "Property 'Subjects' was not updated properly.");
            Assert.AreEqual(anEntity, value.Entity, "Property 'Entity' was not set properly.");
            Assert.AreEqual(anEntity.EntityID, value.EntityID, "Property 'EntityID' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, anEntity.ObjectState, "Object state failed to transition to 'Modified'.");

            value = new EAVModelLibrary.ModelSubject() { SubjectID = rng.Next() };
            anEntity.Subjects.Add(value);

            Assert.IsTrue(anEntity.Subjects.Contains(value), "Property 'Subjects' was not updated properly.");
            Assert.AreEqual(anEntity, value.Entity, "Property 'Entity' was not set properly.");
            Assert.AreEqual(anEntity.EntityID, value.EntityID, "Property 'EntityID' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, anEntity.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void EntityAddToSubjectsWhenDeleted()
        {
            EAVModelLibrary.ModelEntity anEntity = new EAVModelLibrary.ModelEntity() { EntityID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, anEntity.ObjectState, "Object state should be 'New' on creation.");

            anEntity.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, anEntity.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anEntity.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, anEntity.ObjectState, "Object state failed to transition to 'Deleted'.");

            anEntity.Subjects.Add(new EAVModelLibrary.ModelSubject() { SubjectID = rng.Next() });
        }

        [TestMethod]
        public void EntityRemoveFromSubjectsWhenNew()
        {
            EAVModelLibrary.ModelEntity anEntity = new EAVModelLibrary.ModelEntity() { EntityID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, anEntity.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelSubject value = new EAVModelLibrary.ModelSubject() { SubjectID = rng.Next() };
            anEntity.Subjects.Add(value);
            value = new EAVModelLibrary.ModelSubject() { SubjectID = rng.Next() };
            anEntity.Subjects.Add(value);

            Assert.IsTrue(anEntity.Subjects.Contains(value), "Property 'Subjects' was not updated properly.");
            Assert.AreEqual(anEntity, value.Entity, "Property 'Entity' was not set properly.");
            Assert.AreEqual(anEntity.EntityID, value.EntityID, "Property 'EntityID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, anEntity.ObjectState, "Object state should remain 'New' when property set.");

            anEntity.Subjects.Remove(value);

            Assert.IsFalse(anEntity.Subjects.Contains(value), "Property 'Subjects' was not updated properly.");
            Assert.IsNull(value.Entity, "Property 'Entity' was not set properly.");
            Assert.IsNull(value.EntityID, "Property 'EntityID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, anEntity.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void EntityRemoveFromSubjectsWhenUnmodified()
        {
            EAVModelLibrary.ModelEntity anEntity = new EAVModelLibrary.ModelEntity() { EntityID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, anEntity.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelSubject value = new EAVModelLibrary.ModelSubject() { SubjectID = rng.Next() };
            anEntity.Subjects.Add(value);
            value = new EAVModelLibrary.ModelSubject() { SubjectID = rng.Next() };
            anEntity.Subjects.Add(value);

            Assert.IsTrue(anEntity.Subjects.Contains(value), "Property 'Subjects' was not updated properly.");
            Assert.AreEqual(anEntity, value.Entity, "Property 'Entity' was not set properly.");
            Assert.AreEqual(anEntity.EntityID, value.EntityID, "Property 'EntityID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, anEntity.ObjectState, "Object state should remain 'New' when property set.");

            anEntity.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, anEntity.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anEntity.Subjects.Remove(value);

            Assert.IsFalse(anEntity.Subjects.Contains(value), "Property 'Subjects' was not updated properly.");
            Assert.IsNull(value.Entity, "Property 'Entity' was not set properly.");
            Assert.IsNull(value.EntityID, "Property 'EntityID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, anEntity.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void EntityRemoveFromSubjectsWhenModified()
        {
            EAVModelLibrary.ModelEntity anEntity = new EAVModelLibrary.ModelEntity() { EntityID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, anEntity.ObjectState, "Object state should be 'New' on creation.");

            anEntity.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, anEntity.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVModelLibrary.ModelSubject value = new EAVModelLibrary.ModelSubject() { SubjectID = rng.Next() };
            anEntity.Subjects.Add(value);
            value = new EAVModelLibrary.ModelSubject() { SubjectID = rng.Next() };
            anEntity.Subjects.Add(value);

            Assert.IsTrue(anEntity.Subjects.Contains(value), "Property 'Subjects' was not updated properly.");
            Assert.AreEqual(anEntity, value.Entity, "Property 'Entity' was not set properly.");
            Assert.AreEqual(anEntity.EntityID, value.EntityID, "Property 'EntityID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, anEntity.ObjectState, "Object state failed to transition to 'Modified'.");

            anEntity.Subjects.Remove(value);

            Assert.IsFalse(anEntity.Subjects.Contains(value), "Property 'Subjects' was not updated properly.");
            Assert.IsNull(value.Entity, "Property 'Entity' was not set properly.");
            Assert.IsNull(value.EntityID, "Property 'EntityID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, anEntity.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void EntityRemoveFromSubjectsWhenDeleted()
        {
            EAVModelLibrary.ModelEntity anEntity = new EAVModelLibrary.ModelEntity() { EntityID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, anEntity.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelSubject value = new EAVModelLibrary.ModelSubject() { SubjectID = rng.Next() };
            anEntity.Subjects.Add(value);

            Assert.IsTrue(anEntity.Subjects.Contains(value), "Property 'Subjects' was not updated properly.");
            Assert.AreEqual(anEntity, value.Entity, "Property 'Entity' was not set properly.");
            Assert.AreEqual(anEntity.EntityID, value.EntityID, "Property 'EntityID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, anEntity.ObjectState, "Object state should remain 'New' when property set.");

            anEntity.MarkUnmodified();
            value.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, anEntity.ObjectState, "Object state failed to transition to 'Unmodified'.");
            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, value.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anEntity.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, anEntity.ObjectState, "Object state failed to transition to 'Deleted'.");
            Assert.AreEqual(EAV.Model.ObjectState.Deleted, value.ObjectState, "Object state failed to transition to 'Deleted'.");

            anEntity.Subjects.Remove(value);
        }

        [TestMethod]
        public void EntityClearSubjectsWhenNew()
        {
            EAVModelLibrary.ModelEntity anEntity = new EAVModelLibrary.ModelEntity() { EntityID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, anEntity.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelSubject value = new EAVModelLibrary.ModelSubject() { SubjectID = rng.Next() };
            anEntity.Subjects.Add(value);
            value = new EAVModelLibrary.ModelSubject() { SubjectID = rng.Next() };
            anEntity.Subjects.Add(value);

            Assert.IsTrue(anEntity.Subjects.Contains(value), "Property 'Subjects' was not updated properly.");
            Assert.AreEqual(anEntity, value.Entity, "Property 'Entity' was not set properly.");
            Assert.AreEqual(anEntity.EntityID, value.EntityID, "Property 'EntityID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, anEntity.ObjectState, "Object state should remain 'New' when property set.");

            anEntity.Subjects.Clear();

            Assert.IsFalse(anEntity.Subjects.Contains(value), "Property 'Subjects' was not updated properly.");
            Assert.IsNull(value.Entity, "Property 'Entity' was not set properly.");
            Assert.IsNull(value.EntityID, "Property 'EntityID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, anEntity.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void EntityClearSubjectsWhenUnmodified()
        {
            EAVModelLibrary.ModelEntity anEntity = new EAVModelLibrary.ModelEntity() { EntityID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, anEntity.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelSubject value = new EAVModelLibrary.ModelSubject() { SubjectID = rng.Next() };
            anEntity.Subjects.Add(value);
            value = new EAVModelLibrary.ModelSubject() { SubjectID = rng.Next() };
            anEntity.Subjects.Add(value);

            Assert.IsTrue(anEntity.Subjects.Contains(value), "Property 'Subjects' was not updated properly.");
            Assert.AreEqual(anEntity, value.Entity, "Property 'Entity' was not set properly.");
            Assert.AreEqual(anEntity.EntityID, value.EntityID, "Property 'EntityID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, anEntity.ObjectState, "Object state should remain 'New' when property set.");

            anEntity.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, anEntity.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anEntity.Subjects.Clear();

            Assert.IsFalse(anEntity.Subjects.Contains(value), "Property 'Subjects' was not updated properly.");
            Assert.IsNull(value.Entity, "Property 'Entity' was not set properly.");
            Assert.IsNull(value.EntityID, "Property 'EntityID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, anEntity.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void EntityClearSubjectsWhenModified()
        {
            EAVModelLibrary.ModelEntity anEntity = new EAVModelLibrary.ModelEntity() { EntityID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, anEntity.ObjectState, "Object state should be 'New' on creation.");

            anEntity.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, anEntity.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVModelLibrary.ModelSubject value = new EAVModelLibrary.ModelSubject() { SubjectID = rng.Next() };
            anEntity.Subjects.Add(value);
            value = new EAVModelLibrary.ModelSubject() { SubjectID = rng.Next() };
            anEntity.Subjects.Add(value);

            Assert.IsTrue(anEntity.Subjects.Contains(value), "Property 'Subjects' was not updated properly.");
            Assert.AreEqual(anEntity, value.Entity, "Property 'Entity' was not set properly.");
            Assert.AreEqual(anEntity.EntityID, value.EntityID, "Property 'EntityID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, anEntity.ObjectState, "Object state failed to transition to 'Modified'.");

            anEntity.Subjects.Clear();

            Assert.IsFalse(anEntity.Subjects.Contains(value), "Property 'Subjects' was not updated properly.");
            Assert.IsNull(value.Entity, "Property 'Entity' was not set properly.");
            Assert.IsNull(value.EntityID, "Property 'EntityID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, anEntity.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void EntityClearSubjectsWhenDeleted()
        {
            EAVModelLibrary.ModelEntity anEntity = new EAVModelLibrary.ModelEntity() { EntityID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, anEntity.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelSubject value = new EAVModelLibrary.ModelSubject() { SubjectID = rng.Next() };
            anEntity.Subjects.Add(value);

            Assert.IsTrue(anEntity.Subjects.Contains(value), "Property 'Subjects' was not updated properly.");
            Assert.AreEqual(anEntity, value.Entity, "Property 'Entity' was not set properly.");
            Assert.AreEqual(anEntity.EntityID, value.EntityID, "Property 'EntityID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, anEntity.ObjectState, "Object state should remain 'New' when property set.");

            anEntity.MarkUnmodified();
            value.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, anEntity.ObjectState, "Object state failed to transition to 'Unmodified'.");
            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, value.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anEntity.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, anEntity.ObjectState, "Object state failed to transition to 'Deleted'.");
            Assert.AreEqual(EAV.Model.ObjectState.Deleted, value.ObjectState, "Object state failed to transition to 'Deleted'.");

            anEntity.Subjects.Clear();
        }
        #endregion
        #endregion
    }
}
