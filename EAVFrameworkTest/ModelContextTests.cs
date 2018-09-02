using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using EAV.Model;
using System.Linq;

namespace EAVModelTest
{
    public partial class EAVModelTestHarness
    {
        [TestMethod]
        public void ContextCreate()
        {
            ModelContext aContext = new ModelContext();

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            Assert.IsNull(aContext.ContextID, "Property 'ContextID' should be null on creation.");

            Assert.IsNull(aContext.Name, "Property 'Name' should be null on creation.");
            Assert.IsNull(aContext.DataName, "Property 'DataName' should be null on creation.");
            Assert.IsNull(aContext.DisplayText, "Property 'DisplayText' should be null on creation.");

            Assert.IsNotNull(aContext.Containers, "Property 'Containers' should not be null on creation.");
            Assert.IsFalse(aContext.Containers.Any(), "Property 'Containers' should be empty on creation.");

            Assert.IsNotNull(aContext.Subjects, "Property 'Subjects' should not be null on creation.");
            Assert.IsFalse(aContext.Subjects.Any(), "Property 'Subjects' should be empty on creation.");
        }

        #region State Transitions
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ContextStateTransitionNewToUnmodifiedWithNullID()
        {
            ModelContext aContext = new ModelContext();

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        public void ContextStateTransitionNewToUnmodifiedWithValidID()
        {
            ModelContext aContext = new ModelContext() { ContextID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ContextStateTransitionNewToDeleted()
        {
            ModelContext aContext = new ModelContext();

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            aContext.MarkDeleted();
        }

        [TestMethod]
        public void ContextStateTransitionUnmodifiedToDeleted()
        {
            ModelContext aContext = new ModelContext() { ContextID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aContext.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aContext.ObjectState, "Object state failed to transition to 'Deleted'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ContextStateTransitionDeletedToUnmodified()
        {
            ModelContext aContext = new ModelContext() { ContextID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aContext.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aContext.ObjectState, "Object state failed to transition to 'Deleted'.");

            aContext.MarkUnmodified();
        }
        #endregion

        #region ID Property
        [TestMethod]
        public void ContextSetIDWhenNew()
        {
            ModelContext aContext = new ModelContext();

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            int id = rng.Next();
            aContext.ContextID = id;

            Assert.AreEqual(id, aContext.ContextID, "Property 'ContextID' not properly set.");
            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state after setting property 'ContextID' should be 'New'.");
        }

        [TestMethod]
        public void ContextSetIDBeforeUnmodified()
        {
            ModelContext aContext = new ModelContext();

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            int id = rng.Next();
            aContext.ContextID = id;

            Assert.AreEqual(id, aContext.ContextID, "Property 'ContextID' not properly set.");
            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state after setting property 'ContextID' should be 'New'.");

            aContext.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ContextSetIDAfterUnmodified()
        {
            ModelContext aContext = new ModelContext();

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            aContext.MarkUnmodified();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ContextSetIDWhenDeleted()
        {
            ModelContext aContext = new ModelContext();

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            int id = rng.Next();
            aContext.ContextID = id;

            Assert.AreEqual(id, aContext.ContextID, "Property 'ContextID' not properly set.");
            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state after setting property 'ContextID' should be 'New'.");

            aContext.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aContext.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");

            id = rng.Next();
            aContext.ContextID = id;
        }
        #endregion

        #region Primitive Properties
        #region Name
        [TestMethod]
        public void ContextSetNameWhenNew()
        {
            ModelContext aContext = new ModelContext();

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            string value = Guid.NewGuid().ToString();
            aContext.Name = value;

            Assert.AreEqual(value, aContext.Name, "Property 'Name' was not set properly.");
            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ContextSetNameWhenUnmodified()
        {
            ModelContext aContext = new ModelContext() { ContextID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            aContext.Name = value;

            Assert.AreEqual(value, aContext.Name, "Property 'Name' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aContext.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void ContextSetNameWhenModified()
        {
            ModelContext aContext = new ModelContext() { ContextID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            aContext.Name = value;

            Assert.AreEqual(value, aContext.Name, "Property 'Name' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aContext.ObjectState, "Object state failed to transition to 'Modified'.");

            value = Guid.NewGuid().ToString();
            aContext.Name = value;

            Assert.AreEqual(value, aContext.Name, "Property 'Name' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aContext.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ContextSetNameWhenDeleted()
        {
            ModelContext aContext = new ModelContext() { ContextID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aContext.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aContext.ObjectState, "Object state failed to transition to 'Deleted'.");

            aContext.Name = Guid.NewGuid().ToString();
        }
        #endregion

        #region DataName
        [TestMethod]
        public void ContextSetDataNameWhenNew()
        {
            ModelContext aContext = new ModelContext();

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            string value = Guid.NewGuid().ToString();
            aContext.DataName = value;

            Assert.AreEqual(value, aContext.DataName, "Property 'DataName' was not set properly.");
            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ContextSetDataNameWhenUnmodified()
        {
            ModelContext aContext = new ModelContext() { ContextID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            aContext.DataName = value;

            Assert.AreEqual(value, aContext.DataName, "Property 'DataName' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aContext.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void ContextSetDataNameWhenModified()
        {
            ModelContext aContext = new ModelContext() { ContextID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            aContext.DataName = value;

            Assert.AreEqual(value, aContext.DataName, "Property 'DataName' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aContext.ObjectState, "Object state failed to transition to 'Modified'.");

            value = Guid.NewGuid().ToString();
            aContext.DataName = value;

            Assert.AreEqual(value, aContext.DataName, "Property 'DataName' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aContext.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ContextSetDataNameWhenDeleted()
        {
            ModelContext aContext = new ModelContext() { ContextID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aContext.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aContext.ObjectState, "Object state failed to transition to 'Deleted'.");

            aContext.DataName = Guid.NewGuid().ToString();
        }
        #endregion
        
        #region DisplayText
        [TestMethod]
        public void ContextSetDisplayTextWhenNew()
        {
            ModelContext aContext = new ModelContext();

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            string value = Guid.NewGuid().ToString();
            aContext.DisplayText = value;

            Assert.AreEqual(value, aContext.DisplayText, "Property 'DisplayText' was not set properly.");
            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ContextSetDisplayTextWhenUnmodified()
        {
            ModelContext aContext = new ModelContext() { ContextID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            aContext.DisplayText = value;

            Assert.AreEqual(value, aContext.DisplayText, "Property 'DisplayText' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aContext.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void ContextSetDisplayTextWhenModified()
        {
            ModelContext aContext = new ModelContext() { ContextID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            aContext.DisplayText = value;

            Assert.AreEqual(value, aContext.DisplayText, "Property 'DisplayText' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aContext.ObjectState, "Object state failed to transition to 'Modified'.");

            value = Guid.NewGuid().ToString();
            aContext.DisplayText = value;

            Assert.AreEqual(value, aContext.DisplayText, "Property 'DisplayText' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aContext.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ContextSetDisplayTextWhenDeleted()
        {
            ModelContext aContext = new ModelContext() { ContextID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aContext.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aContext.ObjectState, "Object state failed to transition to 'Deleted'.");

            aContext.DisplayText = Guid.NewGuid().ToString();
        }
        #endregion
        #endregion

        #region Object Properties
        // N/A
        #endregion

        #region Collection Properties
        #region Containers
        [TestMethod]
        public void ContextAddToContainersWhenNew()
        {
            ModelContext aContext = new ModelContext() { ContextID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            ModelRootContainer value = new ModelRootContainer() { ContainerID = rng.Next() };
            aContext.Containers.Add(value);

            Assert.IsTrue(aContext.Containers.Contains(value), "Property 'Containers' was not updated properly.");
            Assert.AreEqual(aContext, value.Context, "Property 'Context' was not set properly.");
            Assert.AreEqual(aContext.ContextID, value.ContextID, "Property 'ContextID' was not set properly.");
            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ContextAddToContainersWhenUnmodified()
        {
            ModelContext aContext = new ModelContext() { ContextID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");

            ModelRootContainer value = new ModelRootContainer() { ContainerID = rng.Next() };
            aContext.Containers.Add(value);

            Assert.IsTrue(aContext.Containers.Contains(value), "Property 'Containers' was not updated properly.");
            Assert.AreEqual(aContext, value.Context, "Property 'Context' was not set properly.");
            Assert.AreEqual(aContext.ContextID, value.ContextID, "Property 'ContextID' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aContext.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void ContextAddToContainersWhenModified()
        {
            ModelContext aContext = new ModelContext() { ContextID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");

            ModelRootContainer value = new ModelRootContainer() { ContainerID = rng.Next() };
            aContext.Containers.Add(value);

            Assert.IsTrue(aContext.Containers.Contains(value), "Property 'Containers' was not updated properly.");
            Assert.AreEqual(aContext, value.Context, "Property 'Context' was not set properly.");
            Assert.AreEqual(aContext.ContextID, value.ContextID, "Property 'ContextID' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aContext.ObjectState, "Object state failed to transition to 'Modified'.");

            value = new ModelRootContainer() { ContainerID = rng.Next() };
            aContext.Containers.Add(value);

            Assert.IsTrue(aContext.Containers.Contains(value), "Property 'Containers' was not updated properly.");
            Assert.AreEqual(aContext, value.Context, "Property 'Context' was not set properly.");
            Assert.AreEqual(aContext.ContextID, value.ContextID, "Property 'ContextID' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aContext.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ContextAddToContainersWhenDeleted()
        {
            ModelContext aContext = new ModelContext() { ContextID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aContext.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aContext.ObjectState, "Object state failed to transition to 'Deleted'.");

            aContext.Containers.Add(new ModelRootContainer() { ContainerID = rng.Next() });
        }

        [TestMethod]
        public void ContextRemoveFromContainersWhenNew()
        {
            ModelContext aContext = new ModelContext() { ContextID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            ModelRootContainer value = new ModelRootContainer() { ContainerID = rng.Next() };
            aContext.Containers.Add(value);
            value = new ModelRootContainer() { ContainerID = rng.Next() };
            aContext.Containers.Add(value);

            Assert.IsTrue(aContext.Containers.Contains(value), "Property 'Containers' was not updated properly.");
            Assert.AreEqual(aContext, value.Context, "Property 'Context' was not set properly.");
            Assert.AreEqual(aContext.ContextID, value.ContextID, "Property 'ContextID' was not set properly.");
            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should remain 'New' when property set.");

            aContext.Containers.Remove(value);

            Assert.IsFalse(aContext.Containers.Contains(value), "Property 'Containers' was not updated properly.");
            Assert.IsNull(value.Context, "Property 'Context' was not set properly.");
            Assert.IsNull(value.ContextID, "Property 'ContextID' was not set properly.");
            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ContextRemoveFromContainersWhenUnmodified()
        {
            ModelContext aContext = new ModelContext() { ContextID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            ModelRootContainer value = new ModelRootContainer() { ContainerID = rng.Next() };
            aContext.Containers.Add(value);
            value = new ModelRootContainer() { ContainerID = rng.Next() };
            aContext.Containers.Add(value);

            Assert.IsTrue(aContext.Containers.Contains(value), "Property 'Containers' was not updated properly.");
            Assert.AreEqual(aContext, value.Context, "Property 'Context' was not set properly.");
            Assert.AreEqual(aContext.ContextID, value.ContextID, "Property 'ContextID' was not set properly.");
            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should remain 'New' when property set.");

            aContext.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aContext.Containers.Remove(value);

            Assert.IsFalse(aContext.Containers.Contains(value), "Property 'Containers' was not updated properly.");
            Assert.IsNull(value.Context, "Property 'Context' was not set properly.");
            Assert.IsNull(value.ContextID, "Property 'ContextID' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aContext.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void ContextRemoveFromContainersWhenModified()
        {
            ModelContext aContext = new ModelContext() { ContextID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");

            ModelRootContainer value = new ModelRootContainer() { ContainerID = rng.Next() };
            aContext.Containers.Add(value);
            value = new ModelRootContainer() { ContainerID = rng.Next() };
            aContext.Containers.Add(value);

            Assert.IsTrue(aContext.Containers.Contains(value), "Property 'Containers' was not updated properly.");
            Assert.AreEqual(aContext, value.Context, "Property 'Context' was not set properly.");
            Assert.AreEqual(aContext.ContextID, value.ContextID, "Property 'ContextID' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aContext.ObjectState, "Object state failed to transition to 'Modified'.");

            aContext.Containers.Remove(value);

            Assert.IsFalse(aContext.Containers.Contains(value), "Property 'Containers' was not updated properly.");
            Assert.IsNull(value.Context, "Property 'Context' was not set properly.");
            Assert.IsNull(value.ContextID, "Property 'ContextID' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aContext.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ContextRemoveFromContainersWhenDeleted()
        {
            ModelContext aContext = new ModelContext() { ContextID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            ModelRootContainer value = new ModelRootContainer() { ContainerID = rng.Next() };
            aContext.Containers.Add(value);

            aContext.MarkUnmodified();
            value.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");
            Assert.AreEqual(ObjectState.Unmodified, value.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aContext.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aContext.ObjectState, "Object state failed to transition to 'Deleted'.");
            Assert.AreEqual(ObjectState.Deleted, value.ObjectState, "Object state failed to transition to 'Deleted'.");

            aContext.Containers.Remove(value);
        }

        [TestMethod]
        public void ContextClearContainersWhenNew()
        {
            ModelContext aContext = new ModelContext() { ContextID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            ModelRootContainer value = new ModelRootContainer() { ContainerID = rng.Next() };
            aContext.Containers.Add(value);

            Assert.IsTrue(aContext.Containers.Contains(value), "Property 'Containers' was not updated properly.");
            Assert.AreEqual(aContext, value.Context, "Property 'Context' was not set properly.");
            Assert.AreEqual(aContext.ContextID, value.ContextID, "Property 'ContextID' was not set properly.");
            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should remain 'New' when property set.");

            aContext.Containers.Clear();

            Assert.IsFalse(aContext.Containers.Contains(value), "Property 'Containers' was not updated properly.");
            Assert.IsNull(value.Context, "Property 'Context' was not set properly.");
            Assert.IsNull(value.ContextID, "Property 'ContextID' was not set properly.");
            Assert.IsFalse(aContext.Containers.Any(), "Property 'Containers' was not updated properly.");
            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ContextClearContainersWhenUnmodified()
        {
            ModelContext aContext = new ModelContext() { ContextID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            ModelRootContainer value = new ModelRootContainer() { ContainerID = rng.Next() };
            aContext.Containers.Add(value);

            Assert.IsTrue(aContext.Containers.Contains(value), "Property 'Containers' was not updated properly.");
            Assert.AreEqual(aContext, value.Context, "Property 'Context' was not set properly.");
            Assert.AreEqual(aContext.ContextID, value.ContextID, "Property 'ContextID' was not set properly.");
            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should remain 'New' when property set.");

            aContext.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aContext.Containers.Clear();

            Assert.IsFalse(aContext.Containers.Contains(value), "Property 'Containers' was not updated properly.");
            Assert.IsNull(value.Context, "Property 'Context' was not set properly.");
            Assert.IsNull(value.ContextID, "Property 'ContextID' was not set properly.");
            Assert.IsFalse(aContext.Containers.Any(), "Property 'Containers' was not updated properly.");
            Assert.AreEqual(ObjectState.Modified, aContext.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void ContextClearContainersWhenModified()
        {
            ModelContext aContext = new ModelContext() { ContextID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");

            ModelRootContainer value = new ModelRootContainer() { ContainerID = rng.Next() };
            aContext.Containers.Add(value);

            Assert.IsTrue(aContext.Containers.Contains(value), "Property 'Containers' was not updated properly.");
            Assert.AreEqual(aContext, value.Context, "Property 'Context' was not set properly.");
            Assert.AreEqual(aContext.ContextID, value.ContextID, "Property 'ContextID' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aContext.ObjectState, "Object state failed to transition to 'Modified'.");

            aContext.Containers.Clear();

            Assert.IsFalse(aContext.Containers.Contains(value), "Property 'Containers' was not updated properly.");
            Assert.IsNull(value.Context, "Property 'Context' was not set properly.");
            Assert.IsNull(value.ContextID, "Property 'ContextID' was not set properly.");
            Assert.IsFalse(aContext.Containers.Any(), "Property 'Containers' was not updated properly.");
            Assert.AreEqual(ObjectState.Modified, aContext.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ContextClearContainersWhenDeleted()
        {
            ModelContext aContext = new ModelContext() { ContextID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            ModelRootContainer value = new ModelRootContainer() { ContainerID = rng.Next() };
            aContext.Containers.Add(value);

            aContext.MarkUnmodified();
            value.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");
            Assert.AreEqual(ObjectState.Unmodified, value.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aContext.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aContext.ObjectState, "Object state failed to transition to 'Deleted'.");
            Assert.AreEqual(ObjectState.Deleted, value.ObjectState, "Object state failed to transition to 'Deleted'.");

            aContext.Containers.Clear();
        }
        #endregion

        #region Subjects
        [TestMethod]
        public void ContextAddToSubjectsWhenNew()
        {
            ModelContext aContext = new ModelContext() { ContextID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            ModelSubject value = new ModelSubject() { SubjectID = rng.Next() };
            aContext.Subjects.Add(value);

            Assert.IsTrue(aContext.Subjects.Contains(value), "Property 'Subjects' was not updated properly.");
            Assert.AreEqual(aContext, value.Context, "Property 'Context' was not set properly.");
            Assert.AreEqual(aContext.ContextID, value.ContextID, "Property 'ContextID' was not set properly.");
            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ContextAddToSubjectsWhenUnmodified()
        {
            ModelContext aContext = new ModelContext() { ContextID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");

            ModelSubject value = new ModelSubject() { SubjectID = rng.Next() };
            aContext.Subjects.Add(value);

            Assert.IsTrue(aContext.Subjects.Contains(value), "Property 'Subjects' was not updated properly.");
            Assert.AreEqual(aContext, value.Context, "Property 'Context' was not set properly.");
            Assert.AreEqual(aContext.ContextID, value.ContextID, "Property 'ContextID' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aContext.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void ContextAddToSubjectsWhenModified()
        {
            ModelContext aContext = new ModelContext() { ContextID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");

            ModelSubject value = new ModelSubject() { SubjectID = rng.Next() };
            aContext.Subjects.Add(value);

            Assert.IsTrue(aContext.Subjects.Contains(value), "Property 'Subjects' was not updated properly.");
            Assert.AreEqual(aContext, value.Context, "Property 'Context' was not set properly.");
            Assert.AreEqual(aContext.ContextID, value.ContextID, "Property 'ContextID' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aContext.ObjectState, "Object state failed to transition to 'Modified'.");

            value = new ModelSubject() { SubjectID = rng.Next() };
            aContext.Subjects.Add(value);

            Assert.IsTrue(aContext.Subjects.Contains(value), "Property 'Subjects' was not updated properly.");
            Assert.AreEqual(aContext, value.Context, "Property 'Context' was not set properly.");
            Assert.AreEqual(aContext.ContextID, value.ContextID, "Property 'ContextID' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aContext.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ContextAddToSubjectsWhenDeleted()
        {
            ModelContext aContext = new ModelContext() { ContextID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aContext.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aContext.ObjectState, "Object state failed to transition to 'Deleted'.");

            aContext.Subjects.Add(new ModelSubject() { SubjectID = rng.Next() });
        }

        [TestMethod]
        public void ContextRemoveFromSubjectsWhenNew()
        {
            ModelContext aContext = new ModelContext() { ContextID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            ModelSubject value = new ModelSubject() { SubjectID = rng.Next() };
            aContext.Subjects.Add(value);
            value = new ModelSubject() { SubjectID = rng.Next() };
            aContext.Subjects.Add(value);

            Assert.IsTrue(aContext.Subjects.Contains(value), "Property 'Subjects' was not updated properly.");
            Assert.AreEqual(aContext, value.Context, "Property 'Context' was not set properly.");
            Assert.AreEqual(aContext.ContextID, value.ContextID, "Property 'ContextID' was not set properly.");
            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should remain 'New' when property set.");

            aContext.Subjects.Remove(value);

            Assert.IsFalse(aContext.Subjects.Contains(value), "Property 'Subjects' was not updated properly.");
            Assert.IsNull(value.Context, "Property 'Context' was not set properly.");
            Assert.IsNull(value.ContextID, "Property 'ContextID' was not set properly.");
            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ContextRemoveFromSubjectsWhenUnmodified()
        {
            ModelContext aContext = new ModelContext() { ContextID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            ModelSubject value = new ModelSubject() { SubjectID = rng.Next() };
            aContext.Subjects.Add(value);
            value = new ModelSubject() { SubjectID = rng.Next() };
            aContext.Subjects.Add(value);

            Assert.IsTrue(aContext.Subjects.Contains(value), "Property 'Subjects' was not updated properly.");
            Assert.AreEqual(aContext, value.Context, "Property 'Context' was not set properly.");
            Assert.AreEqual(aContext.ContextID, value.ContextID, "Property 'ContextID' was not set properly.");
            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should remain 'New' when property set.");

            aContext.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aContext.Subjects.Remove(value);

            Assert.IsFalse(aContext.Subjects.Contains(value), "Property 'Subjects' was not updated properly.");
            Assert.IsNull(value.Context, "Property 'Context' was not set properly.");
            Assert.IsNull(value.ContextID, "Property 'ContextID' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aContext.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void ContextRemoveFromSubjectsWhenModified()
        {
            ModelContext aContext = new ModelContext() { ContextID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            ModelSubject value = new ModelSubject() { SubjectID = rng.Next() };
            aContext.Subjects.Add(value);
            value = new ModelSubject() { SubjectID = rng.Next() };
            aContext.Subjects.Add(value);

            Assert.IsTrue(aContext.Subjects.Contains(value), "Property 'Subjects' was not updated properly.");
            Assert.AreEqual(aContext, value.Context, "Property 'Context' was not set properly.");
            Assert.AreEqual(aContext.ContextID, value.ContextID, "Property 'ContextID' was not set properly.");
            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should remain 'New' when property set.");

            aContext.MarkUnmodified();

            aContext.Subjects.Remove(value);

            Assert.IsFalse(aContext.Subjects.Contains(value), "Property 'Containers' was not updated properly.");
            Assert.IsNull(value.Context, "Property 'Context' was not set properly.");
            Assert.IsNull(value.ContextID, "Property 'ContextID' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aContext.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ContextRemoveFromSubjectsWhenDeleted()
        {
            ModelContext aContext = new ModelContext() { ContextID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            ModelSubject value = new ModelSubject() { SubjectID = rng.Next() };
            aContext.Subjects.Add(value);

            aContext.MarkUnmodified();
            value.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");
            Assert.AreEqual(ObjectState.Unmodified, value.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aContext.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aContext.ObjectState, "Object state failed to transition to 'Deleted'.");
            Assert.AreEqual(ObjectState.Deleted, value.ObjectState, "Object state failed to transition to 'Deleted'.");

            aContext.Subjects.Remove(value);
        }

        [TestMethod]
        public void ContextClearSubjectsWhenNew()
        {
            ModelContext aContext = new ModelContext() { ContextID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            ModelSubject value = new ModelSubject() { SubjectID = rng.Next() };
            aContext.Subjects.Add(value);

            Assert.IsTrue(aContext.Subjects.Contains(value), "Property 'Subjects' was not updated properly.");
            Assert.AreEqual(aContext, value.Context, "Property 'Context' was not set properly.");
            Assert.AreEqual(aContext.ContextID, value.ContextID, "Property 'ContextID' was not set properly.");
            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should remain 'New' when property set.");

            aContext.Subjects.Clear();

            Assert.IsFalse(aContext.Subjects.Contains(value), "Property 'Subjects' was not updated properly.");
            Assert.IsNull(value.Context, "Property 'Context' was not set properly.");
            Assert.IsNull(value.ContextID, "Property 'ContextID' was not set properly.");
            Assert.IsFalse(aContext.Subjects.Any(), "Property 'Subjects' was not updated properly.");
            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ContextClearSubjectsWhenUnmodified()
        {
            ModelContext aContext = new ModelContext() { ContextID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            ModelSubject value = new ModelSubject() { SubjectID = rng.Next() };
            aContext.Subjects.Add(value);

            Assert.IsTrue(aContext.Subjects.Contains(value), "Property 'Subjects' was not updated properly.");
            Assert.AreEqual(aContext, value.Context, "Property 'Context' was not set properly.");
            Assert.AreEqual(aContext.ContextID, value.ContextID, "Property 'ContextID' was not set properly.");
            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should remain 'New' when property set.");

            aContext.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aContext.Subjects.Clear();

            Assert.IsFalse(aContext.Subjects.Contains(value), "Property 'Subjects' was not updated properly.");
            Assert.IsNull(value.Context, "Property 'Context' was not set properly.");
            Assert.IsNull(value.ContextID, "Property 'ContextID' was not set properly.");
            Assert.IsFalse(aContext.Subjects.Any(), "Property 'Subjects' was not updated properly.");
            Assert.AreEqual(ObjectState.Modified, aContext.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void ContextClearSubjectsWhenModified()
        {
            ModelContext aContext = new ModelContext() { ContextID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            ModelSubject value = new ModelSubject() { SubjectID = rng.Next() };
            aContext.Subjects.Add(value);

            Assert.IsTrue(aContext.Subjects.Contains(value), "Property 'Subjects' was not updated properly.");
            Assert.AreEqual(aContext, value.Context, "Property 'Context' was not set properly.");
            Assert.AreEqual(aContext.ContextID, value.ContextID, "Property 'ContextID' was not set properly.");
            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should remain 'New' when property set.");

            aContext.MarkUnmodified();

            aContext.Subjects.Clear();

            Assert.IsFalse(aContext.Subjects.Contains(value), "Property 'Containers' was not updated properly.");
            Assert.IsNull(value.Context, "Property 'Context' was not set properly.");
            Assert.IsNull(value.ContextID, "Property 'ContextID' was not set properly.");
            Assert.IsFalse(aContext.Subjects.Any(), "Property 'Subjects' was not updated properly.");
            Assert.AreEqual(ObjectState.Modified, aContext.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ContextClearSubjectsWhenDeleted()
        {
            ModelContext aContext = new ModelContext() { ContextID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            ModelSubject value = new ModelSubject() { SubjectID = rng.Next() };
            aContext.Subjects.Add(value);

            aContext.MarkUnmodified();
            value.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");
            Assert.AreEqual(ObjectState.Unmodified, value.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aContext.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aContext.ObjectState, "Object state failed to transition to 'Deleted'.");
            Assert.AreEqual(ObjectState.Deleted, value.ObjectState, "Object state failed to transition to 'Deleted'.");

            aContext.Subjects.Clear();
        }
        #endregion
        #endregion
    }
}
