﻿using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using EAVModelLibrary;


namespace EAVModelTest
{
    public partial class EAVModelTestHarness
    {
        [TestMethod]
        public void SubjectCreate()
        {
            EAV.Model.IModelSubject aSubject = factory.Create<EAV.Model.IModelSubject>();

            Assert.AreEqual(EAV.Model.ObjectState.New, aSubject.ObjectState, "Object state should be 'New' on creation.");

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
            EAV.Model.IModelSubject aSubject = factory.Create<EAV.Model.IModelSubject>();

            Assert.AreEqual(EAV.Model.ObjectState.New, aSubject.ObjectState, "Object state should be 'New' on creation.");

            aSubject.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aSubject.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        public void SubjectStateTransitionNewToUnmodifiedWithValidID()
        {
            ModelSubject aSubject = new EAVModelLibrary.ModelSubject() { SubjectID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aSubject.ObjectState, "Object state should be 'New' on creation.");

            aSubject.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aSubject.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SubjectStateTransitionNewToDeleted()
        {
            EAV.Model.IModelSubject aSubject = factory.Create<EAV.Model.IModelSubject>();

            Assert.AreEqual(EAV.Model.ObjectState.New, aSubject.ObjectState, "Object state should be 'New' on creation.");

            aSubject.MarkDeleted();
        }

        [TestMethod]
        public void SubjectStateTransitionUnmodifiedToDeleted()
        {
            ModelSubject aSubject = new EAVModelLibrary.ModelSubject() { SubjectID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aSubject.ObjectState, "Object state should be 'New' on creation.");

            aSubject.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aSubject.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aSubject.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, aSubject.ObjectState, "Object state failed to transition to 'Deleted'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SubjectStateTransitionDeletedToUnmodified()
        {
            ModelSubject aSubject = new EAVModelLibrary.ModelSubject() { SubjectID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aSubject.ObjectState, "Object state should be 'New' on creation.");

            aSubject.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aSubject.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aSubject.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, aSubject.ObjectState, "Object state failed to transition to 'Deleted'.");

            aSubject.MarkUnmodified();
        }
        #endregion

        #region ID Property
        [TestMethod]
        public void SubjectSetIDWhenNew()
        {
            EAV.Model.IModelSubject aSubject = factory.Create<EAV.Model.IModelSubject>();

            Assert.AreEqual(EAV.Model.ObjectState.New, aSubject.ObjectState, "Object state should be 'New' on creation.");

            int id = rng.Next();
            aSubject.SubjectID = id;

            Assert.AreEqual(id, aSubject.SubjectID, "Property 'SubjectID' not properly set.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aSubject.ObjectState, "Object state after setting property 'SubjectID' should be 'New'.");
        }

        [TestMethod]
        public void SubjectSetIDBeforeUnmodified()
        {
            EAV.Model.IModelSubject aSubject = factory.Create<EAV.Model.IModelSubject>();

            Assert.AreEqual(EAV.Model.ObjectState.New, aSubject.ObjectState, "Object state should be 'New' on creation.");

            int id = rng.Next();
            aSubject.SubjectID = id;

            Assert.AreEqual(id, aSubject.SubjectID, "Property 'SubjectID' not properly set.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aSubject.ObjectState, "Object state after setting property 'SubjectID' should be 'New'.");

            aSubject.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aSubject.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SubjectSetIDAfterUnmodified()
        {
            EAV.Model.IModelSubject aSubject = factory.Create<EAV.Model.IModelSubject>();

            Assert.AreEqual(EAV.Model.ObjectState.New, aSubject.ObjectState, "Object state should be 'New' on creation.");

            aSubject.MarkUnmodified();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SubjectSetIDWhenDeleted()
        {
            EAV.Model.IModelSubject aSubject = factory.Create<EAV.Model.IModelSubject>();

            Assert.AreEqual(EAV.Model.ObjectState.New, aSubject.ObjectState, "Object state should be 'New' on creation.");

            int id = rng.Next();
            aSubject.SubjectID = id;

            Assert.AreEqual(id, aSubject.SubjectID, "Property 'SubjectID' not properly set.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aSubject.ObjectState, "Object state after setting property 'SubjectID' should be 'New'.");

            aSubject.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aSubject.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aSubject.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, aSubject.ObjectState, "Object state failed to transition to 'Unmodified'.");

            id = rng.Next();
            aSubject.SubjectID = id;
        }
        #endregion

        #region Primitive Properties
        #region Identifier
        [TestMethod]
        public void SubjectSetIdentifierWhenNew()
        {
            EAV.Model.IModelSubject aSubject = factory.Create<EAV.Model.IModelSubject>();

            Assert.AreEqual(EAV.Model.ObjectState.New, aSubject.ObjectState, "Object state should be 'New' on creation.");

            string value = Guid.NewGuid().ToString();
            aSubject.Identifier = value;

            Assert.AreEqual(value, aSubject.Identifier, "Property 'Identifier' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aSubject.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void SubjectSetIdentifierWhenUnmodified()
        {
            ModelSubject aSubject = new EAVModelLibrary.ModelSubject() { SubjectID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aSubject.ObjectState, "Object state should be 'New' on creation.");

            aSubject.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aSubject.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            aSubject.Identifier = value;

            Assert.AreEqual(value, aSubject.Identifier, "Property 'Identifier' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aSubject.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void SubjectSetIdentifierWhenModified()
        {
            ModelSubject aSubject = new EAVModelLibrary.ModelSubject() { SubjectID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aSubject.ObjectState, "Object state should be 'New' on creation.");

            aSubject.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aSubject.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            aSubject.Identifier = value;

            Assert.AreEqual(value, aSubject.Identifier, "Property 'Identifier' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aSubject.ObjectState, "Object state failed to transition to 'Modified'.");

            value = Guid.NewGuid().ToString();
            aSubject.Identifier = value;

            Assert.AreEqual(value, aSubject.Identifier, "Property 'Identifier' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aSubject.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SubjectSetIdentifierWhenDeleted()
        {
            ModelSubject aSubject = new EAVModelLibrary.ModelSubject() { SubjectID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aSubject.ObjectState, "Object state should be 'New' on creation.");

            aSubject.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aSubject.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aSubject.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, aSubject.ObjectState, "Object state failed to transition to 'Deleted'.");

            aSubject.Identifier = Guid.NewGuid().ToString();
        }
        #endregion
        #endregion

        #region Object Properties
        #region Entity
        [TestMethod]
        public void SubjectSetEntityWhenNew()
        {
            EAV.Model.IModelSubject aSubject = factory.Create<EAV.Model.IModelSubject>();

            Assert.AreEqual(EAV.Model.ObjectState.New, aSubject.ObjectState, "Object state should be 'New' on creation.");

            ModelEntity value = new EAVModelLibrary.ModelEntity() { EntityID = rng.Next() };
            aSubject.Entity = value;

            Assert.AreEqual(value, aSubject.Entity, "Property 'Entity' was not set properly.");
            Assert.AreEqual(value.EntityID, aSubject.EntityID, "Property 'EntityID' was not reported properly.");
            Assert.IsTrue(value.Subjects.Contains(aSubject), "Property 'Subjects' was not updated properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aSubject.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void SubjectSetEntityWhenUnmodified()
        {
            ModelSubject aSubject = new EAVModelLibrary.ModelSubject() { SubjectID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aSubject.ObjectState, "Object state should be 'New' on creation.");

            aSubject.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aSubject.ObjectState, "Object state failed to transition to 'Unmodified'.");

            ModelEntity value = new EAVModelLibrary.ModelEntity() { EntityID = rng.Next() };
            aSubject.Entity = value;

            Assert.AreEqual(value, aSubject.Entity, "Property 'Entity' was not set properly.");
            Assert.AreEqual(value.EntityID, aSubject.EntityID, "Property 'EntityID' was not reported properly.");
            Assert.IsTrue(value.Subjects.Contains(aSubject), "Property 'Subjects' was not updated properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aSubject.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void SubjectSetEntityWhenModified()
        {
            ModelSubject aSubject = new EAVModelLibrary.ModelSubject() { SubjectID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aSubject.ObjectState, "Object state should be 'New' on creation.");

            aSubject.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aSubject.ObjectState, "Object state failed to transition to 'Unmodified'.");

            ModelEntity value = new EAVModelLibrary.ModelEntity() { EntityID = rng.Next() };
            aSubject.Entity = value;

            Assert.AreEqual(value, aSubject.Entity, "Property 'Entity' was not set properly.");
            Assert.AreEqual(value.EntityID, aSubject.EntityID, "Property 'EntityID' was not reported properly.");
            Assert.IsTrue(value.Subjects.Contains(aSubject), "Property 'Subjects' was not updated properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aSubject.ObjectState, "Object state failed to transition to 'Modified'.");

            value = new EAVModelLibrary.ModelEntity() { EntityID = rng.Next() };
            aSubject.Entity = value;

            Assert.AreEqual(value, aSubject.Entity, "Property 'Entity' was not set properly.");
            Assert.AreEqual(value.EntityID, aSubject.EntityID, "Property 'EntityID' was not reported properly.");
            Assert.IsTrue(value.Subjects.Contains(aSubject), "Property 'Subjects' was not updated properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aSubject.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SubjectSetEntityWhenDeleted()
        {
            EAV.Model.IModelSubject aSubject = factory.Create<EAV.Model.IModelSubject>();

            Assert.AreEqual(EAV.Model.ObjectState.New, aSubject.ObjectState, "Object state should be 'New' on creation.");

            aSubject.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aSubject.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aSubject.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, aSubject.ObjectState, "Object state failed to transition to 'Deleted'.");

            aSubject.Entity = new EAVModelLibrary.ModelEntity() { EntityID = rng.Next() };
        }
        #endregion

        #region Context
        [TestMethod]
        public void SubjectSetContextWhenNew()
        {
            EAV.Model.IModelSubject aSubject = factory.Create<EAV.Model.IModelSubject>();

            Assert.AreEqual(EAV.Model.ObjectState.New, aSubject.ObjectState, "Object state should be 'New' on creation.");

            ModelContext value = new EAVModelLibrary.ModelContext() { ContextID = rng.Next() };
            aSubject.Context = value;

            Assert.AreEqual(value, aSubject.Context, "Property 'Context' was not set properly.");
            Assert.AreEqual(value.ContextID, aSubject.ContextID, "Property 'ContextID' was not reported properly.");
            Assert.IsTrue(value.Subjects.Contains(aSubject), "Property 'Subjects' was not updated properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aSubject.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void SubjectSetContextWhenUnmodified()
        {
            ModelSubject aSubject = new EAVModelLibrary.ModelSubject() { SubjectID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aSubject.ObjectState, "Object state should be 'New' on creation.");

            aSubject.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aSubject.ObjectState, "Object state failed to transition to 'Unmodified'.");

            ModelContext value = new EAVModelLibrary.ModelContext() { ContextID = rng.Next() };
            aSubject.Context = value;

            Assert.AreEqual(value, aSubject.Context, "Property 'Context' was not set properly.");
            Assert.AreEqual(value.ContextID, aSubject.ContextID, "Property 'ContextID' was not reported properly.");
            Assert.IsTrue(value.Subjects.Contains(aSubject), "Property 'Subjects' was not updated properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aSubject.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void SubjectSetContextWhenModified()
        {
            ModelSubject aSubject = new EAVModelLibrary.ModelSubject() { SubjectID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aSubject.ObjectState, "Object state should be 'New' on creation.");

            aSubject.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aSubject.ObjectState, "Object state failed to transition to 'Unmodified'.");

            ModelContext value = new EAVModelLibrary.ModelContext() { ContextID = rng.Next() };
            aSubject.Context = value;

            Assert.AreEqual(value, aSubject.Context, "Property 'Context' was not set properly.");
            Assert.AreEqual(value.ContextID, aSubject.ContextID, "Property 'ContextID' was not reported properly.");
            Assert.IsTrue(value.Subjects.Contains(aSubject), "Property 'Subjects' was not updated properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aSubject.ObjectState, "Object state failed to transition to 'Modified'.");

            value = new EAVModelLibrary.ModelContext() { ContextID = rng.Next() };
            aSubject.Context = value;

            Assert.AreEqual(value, aSubject.Context, "Property 'Context' was not set properly.");
            Assert.AreEqual(value.ContextID, aSubject.ContextID, "Property 'ContextID' was not reported properly.");
            Assert.IsTrue(value.Subjects.Contains(aSubject), "Property 'Subjects' was not updated properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aSubject.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SubjectSetContextWhenDeleted()
        {
            EAV.Model.IModelSubject aSubject = factory.Create<EAV.Model.IModelSubject>();

            Assert.AreEqual(EAV.Model.ObjectState.New, aSubject.ObjectState, "Object state should be 'New' on creation.");

            aSubject.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aSubject.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aSubject.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, aSubject.ObjectState, "Object state failed to transition to 'Deleted'.");

            aSubject.Context = new EAVModelLibrary.ModelContext() { ContextID = rng.Next() };
        }
        #endregion
        #endregion

        #region Collection Properties
        #region Instances
        [TestMethod]
        public void SubjectAddToInstancesWhenNew()
        {
            ModelSubject aSubject = new EAVModelLibrary.ModelSubject() { SubjectID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aSubject.ObjectState, "Object state should be 'New' on creation.");

            ModelRootInstance value = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() };
            aSubject.Instances.Add(value);

            Assert.IsTrue(aSubject.Instances.Contains(value), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(aSubject, value.Subject, "Property 'Subject' was not set properly.");
            Assert.AreEqual(aSubject.SubjectID, value.SubjectID, "Property 'SubjectID' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aSubject.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void SubjectAddToInstancesWhenUnmodified()
        {
            ModelSubject aSubject = new EAVModelLibrary.ModelSubject() { SubjectID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aSubject.ObjectState, "Object state should be 'New' on creation.");

            aSubject.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aSubject.ObjectState, "Object state failed to transition to 'Unmodified'.");

            ModelRootInstance value = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() };
            aSubject.Instances.Add(value);

            Assert.IsTrue(aSubject.Instances.Contains(value), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(aSubject, value.Subject, "Property 'Subject' was not set properly.");
            Assert.AreEqual(aSubject.SubjectID, value.SubjectID, "Property 'SubjectID' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aSubject.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void SubjectAddToInstancesWhenModified()
        {
            ModelSubject aSubject = new EAVModelLibrary.ModelSubject() { SubjectID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aSubject.ObjectState, "Object state should be 'New' on creation.");

            aSubject.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aSubject.ObjectState, "Object state failed to transition to 'Unmodified'.");

            ModelRootInstance value = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() };
            aSubject.Instances.Add(value);

            Assert.IsTrue(aSubject.Instances.Contains(value), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(aSubject, value.Subject, "Property 'Subject' was not set properly.");
            Assert.AreEqual(aSubject.SubjectID, value.SubjectID, "Property 'SubjectID' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aSubject.ObjectState, "Object state failed to transition to 'Modified'.");

            value = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() };
            aSubject.Instances.Add(value);

            Assert.IsTrue(aSubject.Instances.Contains(value), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(aSubject, value.Subject, "Property 'Subject' was not set properly.");
            Assert.AreEqual(aSubject.SubjectID, value.SubjectID, "Property 'SubjectID' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aSubject.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void SubjectAddToInstancesWhenDeleted()
        {
            ModelSubject aSubject = new EAVModelLibrary.ModelSubject() { SubjectID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aSubject.ObjectState, "Object state should be 'New' on creation.");

            aSubject.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aSubject.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aSubject.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, aSubject.ObjectState, "Object state failed to transition to 'Deleted'.");

            aSubject.Instances.Add(new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() });
        }

        [TestMethod]
        public void SubjectRemoveFromInstancesWhenNew()
        {
            ModelSubject aSubject = new EAVModelLibrary.ModelSubject() { SubjectID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aSubject.ObjectState, "Object state should be 'New' on creation.");

            ModelRootInstance value = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() };
            aSubject.Instances.Add(value);
            value = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() };
            aSubject.Instances.Add(value);

            Assert.IsTrue(aSubject.Instances.Contains(value), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(aSubject, value.Subject, "Property 'Subject' was not set properly.");
            Assert.AreEqual(aSubject.SubjectID, value.SubjectID, "Property 'SubjectID' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aSubject.ObjectState, "Object state should remain 'New' when property set.");

            aSubject.Instances.Remove(value);

            Assert.IsFalse(aSubject.Instances.Contains(value), "Property 'Instances' was not updated properly.");
            Assert.IsNull(value.Subject, "Property 'Subject' was not set properly.");
            Assert.IsNull(value.SubjectID, "Property 'SubjectID' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aSubject.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void SubjectRemoveFromInstancesWhenUnmodified()
        {
            ModelSubject aSubject = new EAVModelLibrary.ModelSubject() { SubjectID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aSubject.ObjectState, "Object state should be 'New' on creation.");

            ModelRootInstance value = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() };
            aSubject.Instances.Add(value);
            value = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() };
            aSubject.Instances.Add(value);

            Assert.IsTrue(aSubject.Instances.Contains(value), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(aSubject, value.Subject, "Property 'Subject' was not set properly.");
            Assert.AreEqual(aSubject.SubjectID, value.SubjectID, "Property 'SubjectID' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aSubject.ObjectState, "Object state should remain 'New' when property set.");

            aSubject.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aSubject.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aSubject.Instances.Remove(value);

            Assert.IsFalse(aSubject.Instances.Contains(value), "Property 'Instances' was not updated properly.");
            Assert.IsNull(value.Subject, "Property 'Subject' was not set properly.");
            Assert.IsNull(value.SubjectID, "Property 'SubjectID' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aSubject.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void SubjectRemoveFromInstancesWhenModified()
        {
            ModelSubject aSubject = new EAVModelLibrary.ModelSubject() { SubjectID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aSubject.ObjectState, "Object state should be 'New' on creation.");

            aSubject.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aSubject.ObjectState, "Object state failed to transition to 'Unmodified'.");

            ModelRootInstance value = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() };
            aSubject.Instances.Add(value);
            value = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() };
            aSubject.Instances.Add(value);

            Assert.IsTrue(aSubject.Instances.Contains(value), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(aSubject, value.Subject, "Property 'Subject' was not set properly.");
            Assert.AreEqual(aSubject.SubjectID, value.SubjectID, "Property 'SubjectID' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aSubject.ObjectState, "Object state failed to transition to 'Modified'.");

            aSubject.Instances.Remove(value);

            Assert.IsFalse(aSubject.Instances.Contains(value), "Property 'Instances' was not updated properly.");
            Assert.IsNull(value.Subject, "Property 'Subject' was not set properly.");
            Assert.IsNull(value.SubjectID, "Property 'SubjectID' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aSubject.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void SubjectRemoveFromInstancesWhenDeleted()
        {
            ModelSubject aSubject = new EAVModelLibrary.ModelSubject() { SubjectID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aSubject.ObjectState, "Object state should be 'New' on creation.");

            ModelRootInstance value = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() };
            aSubject.Instances.Add(value);

            aSubject.MarkUnmodified();
            value.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aSubject.ObjectState, "Object state failed to transition to 'Unmodified'.");
            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, value.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aSubject.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, aSubject.ObjectState, "Object state failed to transition to 'Deleted'.");
            Assert.AreEqual(EAV.Model.ObjectState.Deleted, value.ObjectState, "Object state failed to transition to 'Deleted'.");

            aSubject.Instances.Remove(value);
        }

        [TestMethod]
        public void SubjectClearInstancesWhenNew()
        {
            ModelSubject aSubject = new EAVModelLibrary.ModelSubject() { SubjectID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aSubject.ObjectState, "Object state should be 'New' on creation.");

            ModelRootInstance value = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() };
            aSubject.Instances.Add(value);
            value = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() };
            aSubject.Instances.Add(value);

            Assert.IsTrue(aSubject.Instances.Contains(value), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(aSubject, value.Subject, "Property 'Subject' was not set properly.");
            Assert.AreEqual(aSubject.SubjectID, value.SubjectID, "Property 'SubjectID' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aSubject.ObjectState, "Object state should remain 'New' when property set.");

            aSubject.Instances.Clear();

            Assert.IsFalse(aSubject.Instances.Contains(value), "Property 'Instances' was not updated properly.");
            Assert.IsNull(value.Subject, "Property 'Subject' was not set properly.");
            Assert.IsNull(value.SubjectID, "Property 'SubjectID' was not set properly.");
            Assert.IsFalse(aSubject.Instances.Any(), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aSubject.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void SubjectClearInstancesWhenUnmodified()
        {
            ModelSubject aSubject = new EAVModelLibrary.ModelSubject() { SubjectID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aSubject.ObjectState, "Object state should be 'New' on creation.");

            ModelRootInstance value = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() };
            aSubject.Instances.Add(value);
            value = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() };
            aSubject.Instances.Add(value);

            Assert.IsTrue(aSubject.Instances.Contains(value), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(aSubject, value.Subject, "Property 'Subject' was not set properly.");
            Assert.AreEqual(aSubject.SubjectID, value.SubjectID, "Property 'SubjectID' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aSubject.ObjectState, "Object state should remain 'New' when property set.");

            aSubject.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aSubject.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aSubject.Instances.Clear();

            Assert.IsFalse(aSubject.Instances.Contains(value), "Property 'Instances' was not updated properly.");
            Assert.IsNull(value.Subject, "Property 'Subject' was not set properly.");
            Assert.IsNull(value.SubjectID, "Property 'SubjectID' was not set properly.");
            Assert.IsFalse(aSubject.Instances.Any(), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aSubject.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void SubjectClearInstancesWhenModified()
        {
            ModelSubject aSubject = new EAVModelLibrary.ModelSubject() { SubjectID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aSubject.ObjectState, "Object state should be 'New' on creation.");

            aSubject.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aSubject.ObjectState, "Object state failed to transition to 'Unmodified'.");

            ModelRootInstance value = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() };
            aSubject.Instances.Add(value);
            value = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() };
            aSubject.Instances.Add(value);

            Assert.IsTrue(aSubject.Instances.Contains(value), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(aSubject, value.Subject, "Property 'Subject' was not set properly.");
            Assert.AreEqual(aSubject.SubjectID, value.SubjectID, "Property 'SubjectID' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aSubject.ObjectState, "Object state failed to transition to 'Modified'.");

            aSubject.Instances.Clear();

            Assert.IsFalse(aSubject.Instances.Contains(value), "Property 'Instances' was not updated properly.");
            Assert.IsNull(value.Subject, "Property 'Subject' was not set properly.");
            Assert.IsNull(value.SubjectID, "Property 'SubjectID' was not set properly.");
            Assert.IsFalse(aSubject.Instances.Any(), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aSubject.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void SubjectClearInstancesWhenDeleted()
        {
            ModelSubject aSubject = new EAVModelLibrary.ModelSubject() { SubjectID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aSubject.ObjectState, "Object state should be 'New' on creation.");

            ModelRootInstance value = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() };
            aSubject.Instances.Add(value);

            aSubject.MarkUnmodified();
            value.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aSubject.ObjectState, "Object state failed to transition to 'Unmodified'.");
            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, value.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aSubject.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, aSubject.ObjectState, "Object state failed to transition to 'Deleted'.");
            Assert.AreEqual(EAV.Model.ObjectState.Deleted, value.ObjectState, "Object state failed to transition to 'Deleted'.");

            aSubject.Instances.Clear();
        }
        #endregion
        #endregion
    }
}
