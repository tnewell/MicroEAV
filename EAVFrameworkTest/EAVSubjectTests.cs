using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using EAV.Model;

using EAVFramework.Model;


namespace EAVFrameworkTest
{
    public partial class EAVFrameworkTestHarness
    {
        [TestMethod]
        public void SubjectCreate()
        {
            EAVSubject aSubject = new EAVSubject();

            Assert.AreEqual(ObjectState.New, aSubject.ObjectState, "Object state should be 'New' on creation.");

            Assert.IsNull(aSubject.SubjectID, "Property 'SubjectID' should be null on creation.");

            Assert.IsNull(aSubject.Identifier, "Property 'Identifier' should be null on creation.");

            Assert.IsNull(aSubject.Entity, "Property 'Entity' should be null on creation.");
            Assert.IsNull(aSubject.Context, "Property 'Context' should be null on creation.");

            Assert.IsNotNull(aSubject.Instances, "Property 'Instances' should not be null on creation.");
            Assert.IsFalse(aSubject.Instances.Any(), "Property 'Instances' should be empty on creation.");
        }

        #region State Transitions
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SubjectStateTransitionNewToUnmodifiedWithNullID()
        {
            EAVSubject aSubject = new EAVSubject();

            Assert.AreEqual(ObjectState.New, aSubject.ObjectState, "Object state should be 'New' on creation.");

            aSubject.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aSubject.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        public void SubjectStateTransitionNewToUnmodifiedWithValidID()
        {
            EAVSubject aSubject = new EAVSubject() { SubjectID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aSubject.ObjectState, "Object state should be 'New' on creation.");

            aSubject.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aSubject.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SubjectStateTransitionNewToDeleted()
        {
            EAVSubject aSubject = new EAVSubject();

            Assert.AreEqual(ObjectState.New, aSubject.ObjectState, "Object state should be 'New' on creation.");

            aSubject.MarkDeleted();
        }

        [TestMethod]
        public void SubjectStateTransitionUnmodifiedToDeleted()
        {
            EAVSubject aSubject = new EAVSubject() { SubjectID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aSubject.ObjectState, "Object state should be 'New' on creation.");

            aSubject.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aSubject.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aSubject.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aSubject.ObjectState, "Object state failed to transition to 'Deleted'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SubjectStateTransitionDeletedToUnmodified()
        {
            EAVSubject aSubject = new EAVSubject() { SubjectID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aSubject.ObjectState, "Object state should be 'New' on creation.");

            aSubject.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aSubject.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aSubject.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aSubject.ObjectState, "Object state failed to transition to 'Deleted'.");

            aSubject.MarkUnmodified();
        }
        #endregion

        #region ID Property
        [TestMethod]
        public void SubjectSetIDWhenNew()
        {
            EAVSubject aSubject = new EAVSubject();

            Assert.AreEqual(ObjectState.New, aSubject.ObjectState, "Object state should be 'New' on creation.");

            int id = rng.Next();
            aSubject.SubjectID = id;

            Assert.AreEqual(id, aSubject.SubjectID, "Property 'SubjectID' not properly set.");
            Assert.AreEqual(ObjectState.New, aSubject.ObjectState, "Object state after setting property 'SubjectID' should be 'New'.");
        }

        [TestMethod]
        public void SubjectSetIDBeforeUnmodified()
        {
            EAVSubject aSubject = new EAVSubject();

            Assert.AreEqual(ObjectState.New, aSubject.ObjectState, "Object state should be 'New' on creation.");

            int id = rng.Next();
            aSubject.SubjectID = id;

            Assert.AreEqual(id, aSubject.SubjectID, "Property 'SubjectID' not properly set.");
            Assert.AreEqual(ObjectState.New, aSubject.ObjectState, "Object state after setting property 'SubjectID' should be 'New'.");

            aSubject.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aSubject.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SubjectSetIDAfterUnmodified()
        {
            EAVSubject aSubject = new EAVSubject();

            Assert.AreEqual(ObjectState.New, aSubject.ObjectState, "Object state should be 'New' on creation.");

            aSubject.MarkUnmodified();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SubjectSetIDWhenDeleted()
        {
            EAVSubject aSubject = new EAVSubject();

            Assert.AreEqual(ObjectState.New, aSubject.ObjectState, "Object state should be 'New' on creation.");

            int id = rng.Next();
            aSubject.SubjectID = id;

            Assert.AreEqual(id, aSubject.SubjectID, "Property 'SubjectID' not properly set.");
            Assert.AreEqual(ObjectState.New, aSubject.ObjectState, "Object state after setting property 'SubjectID' should be 'New'.");

            aSubject.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aSubject.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aSubject.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aSubject.ObjectState, "Object state failed to transition to 'Unmodified'.");

            id = rng.Next();
            aSubject.SubjectID = id;
        }
        #endregion

        #region Primitive Properties
        #region Identifier
        [TestMethod]
        public void SubjectSetIdentifierWhenNew()
        {
            EAVSubject aSubject = new EAVSubject();

            Assert.AreEqual(ObjectState.New, aSubject.ObjectState, "Object state should be 'New' on creation.");

            string value = Guid.NewGuid().ToString();
            aSubject.Identifier = value;

            Assert.AreEqual(value, aSubject.Identifier, "Property 'Identifier' was not set properly.");
            Assert.AreEqual(ObjectState.New, aSubject.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void SubjectSetIdentifierWhenUnmodified()
        {
            EAVSubject aSubject = new EAVSubject() { SubjectID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aSubject.ObjectState, "Object state should be 'New' on creation.");

            aSubject.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aSubject.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            aSubject.Identifier = value;

            Assert.AreEqual(value, aSubject.Identifier, "Property 'Identifier' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aSubject.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void SubjectSetIdentifierWhenModified()
        {
            EAVSubject aSubject = new EAVSubject() { SubjectID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aSubject.ObjectState, "Object state should be 'New' on creation.");

            aSubject.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aSubject.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            aSubject.Identifier = value;

            Assert.AreEqual(value, aSubject.Identifier, "Property 'Identifier' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aSubject.ObjectState, "Object state failed to transition to 'Modified'.");

            value = Guid.NewGuid().ToString();
            aSubject.Identifier = value;

            Assert.AreEqual(value, aSubject.Identifier, "Property 'Identifier' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aSubject.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SubjectSetIdentifierWhenDeleted()
        {
            EAVSubject aSubject = new EAVSubject() { SubjectID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aSubject.ObjectState, "Object state should be 'New' on creation.");

            aSubject.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aSubject.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aSubject.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aSubject.ObjectState, "Object state failed to transition to 'Deleted'.");

            aSubject.Identifier = Guid.NewGuid().ToString();
        }
        #endregion
        #endregion

        #region Object Properties
        #region Entity
        [TestMethod]
        public void SubjectSetEntityWhenNew()
        {
            EAVSubject aSubject = new EAVSubject();

            Assert.AreEqual(ObjectState.New, aSubject.ObjectState, "Object state should be 'New' on creation.");

            EAVEntity value = new EAVEntity() { EntityID = rng.Next() };
            aSubject.Entity = value;

            Assert.AreEqual(value, aSubject.Entity, "Property 'Entity' was not set properly.");
            Assert.AreEqual(value.EntityID, aSubject.EntityID, "Property 'EntityID' was not reported properly.");
            Assert.IsTrue(value.Subjects.Contains(aSubject), "Property 'Subjects' was not updated properly.");
            Assert.AreEqual(ObjectState.New, aSubject.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void SubjectSetEntityWhenUnmodified()
        {
            EAVSubject aSubject = new EAVSubject() { SubjectID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aSubject.ObjectState, "Object state should be 'New' on creation.");

            aSubject.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aSubject.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVEntity value = new EAVEntity() { EntityID = rng.Next() };
            aSubject.Entity = value;

            Assert.AreEqual(value, aSubject.Entity, "Property 'Entity' was not set properly.");
            Assert.AreEqual(value.EntityID, aSubject.EntityID, "Property 'EntityID' was not reported properly.");
            Assert.IsTrue(value.Subjects.Contains(aSubject), "Property 'Subjects' was not updated properly.");
            Assert.AreEqual(ObjectState.Modified, aSubject.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void SubjectSetEntityWhenModified()
        {
            EAVSubject aSubject = new EAVSubject() { SubjectID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aSubject.ObjectState, "Object state should be 'New' on creation.");

            aSubject.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aSubject.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVEntity value = new EAVEntity() { EntityID = rng.Next() };
            aSubject.Entity = value;

            Assert.AreEqual(value, aSubject.Entity, "Property 'Entity' was not set properly.");
            Assert.AreEqual(value.EntityID, aSubject.EntityID, "Property 'EntityID' was not reported properly.");
            Assert.IsTrue(value.Subjects.Contains(aSubject), "Property 'Subjects' was not updated properly.");
            Assert.AreEqual(ObjectState.Modified, aSubject.ObjectState, "Object state failed to transition to 'Modified'.");

            value = new EAVEntity() { EntityID = rng.Next() };
            aSubject.Entity = value;

            Assert.AreEqual(value, aSubject.Entity, "Property 'Entity' was not set properly.");
            Assert.AreEqual(value.EntityID, aSubject.EntityID, "Property 'EntityID' was not reported properly.");
            Assert.IsTrue(value.Subjects.Contains(aSubject), "Property 'Subjects' was not updated properly.");
            Assert.AreEqual(ObjectState.Modified, aSubject.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SubjectSetEntityWhenDeleted()
        {
            EAVSubject aSubject = new EAVSubject();

            Assert.AreEqual(ObjectState.New, aSubject.ObjectState, "Object state should be 'New' on creation.");

            aSubject.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aSubject.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aSubject.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aSubject.ObjectState, "Object state failed to transition to 'Deleted'.");

            aSubject.Entity = new EAVEntity() { EntityID = rng.Next() };
        }
        #endregion

        #region Context
        [TestMethod]
        public void SubjectSetContextWhenNew()
        {
            EAVSubject aSubject = new EAVSubject();

            Assert.AreEqual(ObjectState.New, aSubject.ObjectState, "Object state should be 'New' on creation.");

            EAVContext value = new EAVContext() { ContextID = rng.Next() };
            aSubject.Context = value;

            Assert.AreEqual(value, aSubject.Context, "Property 'Context' was not set properly.");
            Assert.AreEqual(value.ContextID, aSubject.ContextID, "Property 'ContextID' was not reported properly.");
            Assert.IsTrue(value.Subjects.Contains(aSubject), "Property 'Subjects' was not updated properly.");
            Assert.AreEqual(ObjectState.New, aSubject.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void SubjectSetContextWhenUnmodified()
        {
            EAVSubject aSubject = new EAVSubject() { SubjectID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aSubject.ObjectState, "Object state should be 'New' on creation.");

            aSubject.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aSubject.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVContext value = new EAVContext() { ContextID = rng.Next() };
            aSubject.Context = value;

            Assert.AreEqual(value, aSubject.Context, "Property 'Context' was not set properly.");
            Assert.AreEqual(value.ContextID, aSubject.ContextID, "Property 'ContextID' was not reported properly.");
            Assert.IsTrue(value.Subjects.Contains(aSubject), "Property 'Subjects' was not updated properly.");
            Assert.AreEqual(ObjectState.Modified, aSubject.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void SubjectSetContextWhenModified()
        {
            EAVSubject aSubject = new EAVSubject() { SubjectID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aSubject.ObjectState, "Object state should be 'New' on creation.");

            aSubject.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aSubject.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVContext value = new EAVContext() { ContextID = rng.Next() };
            aSubject.Context = value;

            Assert.AreEqual(value, aSubject.Context, "Property 'Context' was not set properly.");
            Assert.AreEqual(value.ContextID, aSubject.ContextID, "Property 'ContextID' was not reported properly.");
            Assert.IsTrue(value.Subjects.Contains(aSubject), "Property 'Subjects' was not updated properly.");
            Assert.AreEqual(ObjectState.Modified, aSubject.ObjectState, "Object state failed to transition to 'Modified'.");

            value = new EAVContext() { ContextID = rng.Next() };
            aSubject.Context = value;

            Assert.AreEqual(value, aSubject.Context, "Property 'Context' was not set properly.");
            Assert.AreEqual(value.ContextID, aSubject.ContextID, "Property 'ContextID' was not reported properly.");
            Assert.IsTrue(value.Subjects.Contains(aSubject), "Property 'Subjects' was not updated properly.");
            Assert.AreEqual(ObjectState.Modified, aSubject.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SubjectSetContextWhenDeleted()
        {
            EAVSubject aSubject = new EAVSubject();

            Assert.AreEqual(ObjectState.New, aSubject.ObjectState, "Object state should be 'New' on creation.");

            aSubject.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aSubject.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aSubject.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aSubject.ObjectState, "Object state failed to transition to 'Deleted'.");

            aSubject.Context = new EAVContext() { ContextID = rng.Next() };
        }
        #endregion
        #endregion

        #region Collection Properties
        #region Instances
        [TestMethod]
        public void SubjectAddToInstancesWhenNew()
        {
            EAVSubject aSubject = new EAVSubject() { SubjectID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aSubject.ObjectState, "Object state should be 'New' on creation.");

            EAVRootInstance value = new EAVRootInstance() { InstanceID = rng.Next() };
            aSubject.Instances.Add(value);

            Assert.IsTrue(aSubject.Instances.Contains(value), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(aSubject, value.Subject, "Property 'Subject' was not set properly.");
            Assert.AreEqual(aSubject.SubjectID, value.SubjectID, "Property 'SubjectID' was not set properly.");
            Assert.AreEqual(ObjectState.New, aSubject.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void SubjectAddToInstancesWhenUnmodified()
        {
            EAVSubject aSubject = new EAVSubject() { SubjectID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aSubject.ObjectState, "Object state should be 'New' on creation.");

            aSubject.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aSubject.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVRootInstance value = new EAVRootInstance() { InstanceID = rng.Next() };
            aSubject.Instances.Add(value);

            Assert.IsTrue(aSubject.Instances.Contains(value), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(aSubject, value.Subject, "Property 'Subject' was not set properly.");
            Assert.AreEqual(aSubject.SubjectID, value.SubjectID, "Property 'SubjectID' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aSubject.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void SubjectAddToInstancesWhenModified()
        {
            EAVSubject aSubject = new EAVSubject() { SubjectID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aSubject.ObjectState, "Object state should be 'New' on creation.");

            aSubject.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aSubject.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVRootInstance value = new EAVRootInstance() { InstanceID = rng.Next() };
            aSubject.Instances.Add(value);

            Assert.IsTrue(aSubject.Instances.Contains(value), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(aSubject, value.Subject, "Property 'Subject' was not set properly.");
            Assert.AreEqual(aSubject.SubjectID, value.SubjectID, "Property 'SubjectID' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aSubject.ObjectState, "Object state failed to transition to 'Modified'.");

            value = new EAVRootInstance() { InstanceID = rng.Next() };
            aSubject.Instances.Add(value);

            Assert.IsTrue(aSubject.Instances.Contains(value), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(aSubject, value.Subject, "Property 'Subject' was not set properly.");
            Assert.AreEqual(aSubject.SubjectID, value.SubjectID, "Property 'SubjectID' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aSubject.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void SubjectAddToInstancesWhenDeleted()
        {
            EAVSubject aSubject = new EAVSubject() { SubjectID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aSubject.ObjectState, "Object state should be 'New' on creation.");

            aSubject.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aSubject.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aSubject.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aSubject.ObjectState, "Object state failed to transition to 'Deleted'.");

            aSubject.Instances.Add(new EAVRootInstance() { InstanceID = rng.Next() });
        }
        #endregion
        #endregion
    }
}
