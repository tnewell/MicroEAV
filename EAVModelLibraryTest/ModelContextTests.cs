using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace EAVModelTest
{
    public partial class EAVModelTestHarness
    {
        [TestMethod]
        public void ContextCreate()
        {
            EAV.Model.IModelContext aContext = factory.Create<EAV.Model.IModelContext>();

            Assert.AreEqual(EAV.Model.ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

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
            EAV.Model.IModelContext aContext = factory.Create<EAV.Model.IModelContext>();

            Assert.AreEqual(EAV.Model.ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        public void ContextStateTransitionNewToUnmodifiedWithValidID()
        {
            EAVModelLibrary.ModelContext aContext = new EAVModelLibrary.ModelContext() { ContextID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ContextStateTransitionNewToDeleted()
        {
            EAV.Model.IModelContext aContext = factory.Create<EAV.Model.IModelContext>();

            Assert.AreEqual(EAV.Model.ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            aContext.MarkDeleted();
        }

        [TestMethod]
        public void ContextStateTransitionUnmodifiedToDeleted()
        {
            EAVModelLibrary.ModelContext aContext = new EAVModelLibrary.ModelContext() { ContextID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aContext.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, aContext.ObjectState, "Object state failed to transition to 'Deleted'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ContextStateTransitionDeletedToUnmodified()
        {
            EAVModelLibrary.ModelContext aContext = new EAVModelLibrary.ModelContext() { ContextID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aContext.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, aContext.ObjectState, "Object state failed to transition to 'Deleted'.");

            aContext.MarkUnmodified();
        }
        #endregion

        #region ID Property
        [TestMethod]
        public void ContextSetIDWhenNew()
        {
            EAV.Model.IModelContext aContext = factory.Create<EAV.Model.IModelContext>();

            Assert.AreEqual(EAV.Model.ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            int id = rng.Next();
            aContext.ContextID = id;

            Assert.AreEqual(id, aContext.ContextID, "Property 'ContextID' not properly set.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aContext.ObjectState, "Object state after setting property 'ContextID' should be 'New'.");
        }

        [TestMethod]
        public void ContextSetIDBeforeUnmodified()
        {
            EAV.Model.IModelContext aContext = factory.Create<EAV.Model.IModelContext>();

            Assert.AreEqual(EAV.Model.ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            int id = rng.Next();
            aContext.ContextID = id;

            Assert.AreEqual(id, aContext.ContextID, "Property 'ContextID' not properly set.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aContext.ObjectState, "Object state after setting property 'ContextID' should be 'New'.");

            aContext.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ContextSetIDAfterUnmodified()
        {
            EAV.Model.IModelContext aContext = factory.Create<EAV.Model.IModelContext>();

            Assert.AreEqual(EAV.Model.ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            aContext.MarkUnmodified();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ContextSetIDWhenDeleted()
        {
            EAV.Model.IModelContext aContext = factory.Create<EAV.Model.IModelContext>();

            Assert.AreEqual(EAV.Model.ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            int id = rng.Next();
            aContext.ContextID = id;

            Assert.AreEqual(id, aContext.ContextID, "Property 'ContextID' not properly set.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aContext.ObjectState, "Object state after setting property 'ContextID' should be 'New'.");

            aContext.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aContext.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");

            id = rng.Next();
            aContext.ContextID = id;
        }
        #endregion

        #region Primitive Properties
        #region Name
        [TestMethod]
        public void ContextSetNameWhenNew()
        {
            EAV.Model.IModelContext aContext = factory.Create<EAV.Model.IModelContext>();

            Assert.AreEqual(EAV.Model.ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            string value = Guid.NewGuid().ToString();
            aContext.Name = value;

            Assert.AreEqual(value, aContext.Name, "Property 'Name' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aContext.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ContextSetNameWhenUnmodified()
        {
            EAVModelLibrary.ModelContext aContext = new EAVModelLibrary.ModelContext() { ContextID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            aContext.Name = value;

            Assert.AreEqual(value, aContext.Name, "Property 'Name' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aContext.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void ContextSetNameWhenModified()
        {
            EAVModelLibrary.ModelContext aContext = new EAVModelLibrary.ModelContext() { ContextID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            aContext.Name = value;

            Assert.AreEqual(value, aContext.Name, "Property 'Name' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aContext.ObjectState, "Object state failed to transition to 'Modified'.");

            value = Guid.NewGuid().ToString();
            aContext.Name = value;

            Assert.AreEqual(value, aContext.Name, "Property 'Name' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aContext.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ContextSetNameWhenDeleted()
        {
            EAVModelLibrary.ModelContext aContext = new EAVModelLibrary.ModelContext() { ContextID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aContext.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, aContext.ObjectState, "Object state failed to transition to 'Deleted'.");

            aContext.Name = Guid.NewGuid().ToString();
        }
        #endregion

        #region DataName
        [TestMethod]
        public void ContextSetDataNameWhenNew()
        {
            EAV.Model.IModelContext aContext = factory.Create<EAV.Model.IModelContext>();

            Assert.AreEqual(EAV.Model.ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            string value = Guid.NewGuid().ToString();
            aContext.DataName = value;

            Assert.AreEqual(value, aContext.DataName, "Property 'DataName' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aContext.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ContextSetDataNameWhenUnmodified()
        {
            EAVModelLibrary.ModelContext aContext = new EAVModelLibrary.ModelContext() { ContextID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            aContext.DataName = value;

            Assert.AreEqual(value, aContext.DataName, "Property 'DataName' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aContext.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void ContextSetDataNameWhenModified()
        {
            EAVModelLibrary.ModelContext aContext = new EAVModelLibrary.ModelContext() { ContextID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            aContext.DataName = value;

            Assert.AreEqual(value, aContext.DataName, "Property 'DataName' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aContext.ObjectState, "Object state failed to transition to 'Modified'.");

            value = Guid.NewGuid().ToString();
            aContext.DataName = value;

            Assert.AreEqual(value, aContext.DataName, "Property 'DataName' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aContext.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ContextSetDataNameWhenDeleted()
        {
            EAVModelLibrary.ModelContext aContext = new EAVModelLibrary.ModelContext() { ContextID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aContext.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, aContext.ObjectState, "Object state failed to transition to 'Deleted'.");

            aContext.DataName = Guid.NewGuid().ToString();
        }
        #endregion
        
        #region DisplayText
        [TestMethod]
        public void ContextSetDisplayTextWhenNew()
        {
            EAV.Model.IModelContext aContext = factory.Create<EAV.Model.IModelContext>();

            Assert.AreEqual(EAV.Model.ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            string value = Guid.NewGuid().ToString();
            aContext.DisplayText = value;

            Assert.AreEqual(value, aContext.DisplayText, "Property 'DisplayText' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aContext.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ContextSetDisplayTextWhenUnmodified()
        {
            EAVModelLibrary.ModelContext aContext = new EAVModelLibrary.ModelContext() { ContextID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            aContext.DisplayText = value;

            Assert.AreEqual(value, aContext.DisplayText, "Property 'DisplayText' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aContext.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void ContextSetDisplayTextWhenModified()
        {
            EAVModelLibrary.ModelContext aContext = new EAVModelLibrary.ModelContext() { ContextID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            aContext.DisplayText = value;

            Assert.AreEqual(value, aContext.DisplayText, "Property 'DisplayText' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aContext.ObjectState, "Object state failed to transition to 'Modified'.");

            value = Guid.NewGuid().ToString();
            aContext.DisplayText = value;

            Assert.AreEqual(value, aContext.DisplayText, "Property 'DisplayText' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aContext.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ContextSetDisplayTextWhenDeleted()
        {
            EAVModelLibrary.ModelContext aContext = new EAVModelLibrary.ModelContext() { ContextID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aContext.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, aContext.ObjectState, "Object state failed to transition to 'Deleted'.");

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
            EAVModelLibrary.ModelContext aContext = new EAVModelLibrary.ModelContext() { ContextID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelRootContainer value = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };
            aContext.Containers.Add(value);

            Assert.IsTrue(aContext.Containers.Contains(value), "Property 'Containers' was not updated properly.");
            Assert.AreEqual(aContext, value.Context, "Property 'Context' was not set properly.");
            Assert.AreEqual(aContext.ContextID, value.ContextID, "Property 'ContextID' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aContext.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ContextAddToContainersWhenUnmodified()
        {
            EAVModelLibrary.ModelContext aContext = new EAVModelLibrary.ModelContext() { ContextID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVModelLibrary.ModelRootContainer value = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };
            aContext.Containers.Add(value);

            Assert.IsTrue(aContext.Containers.Contains(value), "Property 'Containers' was not updated properly.");
            Assert.AreEqual(aContext, value.Context, "Property 'Context' was not set properly.");
            Assert.AreEqual(aContext.ContextID, value.ContextID, "Property 'ContextID' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aContext.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void ContextAddToContainersWhenModified()
        {
            EAVModelLibrary.ModelContext aContext = new EAVModelLibrary.ModelContext() { ContextID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVModelLibrary.ModelRootContainer value = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };
            aContext.Containers.Add(value);

            Assert.IsTrue(aContext.Containers.Contains(value), "Property 'Containers' was not updated properly.");
            Assert.AreEqual(aContext, value.Context, "Property 'Context' was not set properly.");
            Assert.AreEqual(aContext.ContextID, value.ContextID, "Property 'ContextID' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aContext.ObjectState, "Object state failed to transition to 'Modified'.");

            value = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };
            aContext.Containers.Add(value);

            Assert.IsTrue(aContext.Containers.Contains(value), "Property 'Containers' was not updated properly.");
            Assert.AreEqual(aContext, value.Context, "Property 'Context' was not set properly.");
            Assert.AreEqual(aContext.ContextID, value.ContextID, "Property 'ContextID' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aContext.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ContextAddToContainersWhenDeleted()
        {
            EAVModelLibrary.ModelContext aContext = new EAVModelLibrary.ModelContext() { ContextID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aContext.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, aContext.ObjectState, "Object state failed to transition to 'Deleted'.");

            aContext.Containers.Add(new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() });
        }

        [TestMethod]
        public void ContextRemoveFromContainersWhenNew()
        {
            EAVModelLibrary.ModelContext aContext = new EAVModelLibrary.ModelContext() { ContextID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelRootContainer value = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };
            aContext.Containers.Add(value);
            value = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };
            aContext.Containers.Add(value);

            Assert.IsTrue(aContext.Containers.Contains(value), "Property 'Containers' was not updated properly.");
            Assert.AreEqual(aContext, value.Context, "Property 'Context' was not set properly.");
            Assert.AreEqual(aContext.ContextID, value.ContextID, "Property 'ContextID' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aContext.ObjectState, "Object state should remain 'New' when property set.");

            aContext.Containers.Remove(value);

            Assert.IsFalse(aContext.Containers.Contains(value), "Property 'Containers' was not updated properly.");
            Assert.IsNull(value.Context, "Property 'Context' was not set properly.");
            Assert.IsNull(value.ContextID, "Property 'ContextID' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aContext.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ContextRemoveFromContainersWhenUnmodified()
        {
            EAVModelLibrary.ModelContext aContext = new EAVModelLibrary.ModelContext() { ContextID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelRootContainer value = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };
            aContext.Containers.Add(value);
            value = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };
            aContext.Containers.Add(value);

            Assert.IsTrue(aContext.Containers.Contains(value), "Property 'Containers' was not updated properly.");
            Assert.AreEqual(aContext, value.Context, "Property 'Context' was not set properly.");
            Assert.AreEqual(aContext.ContextID, value.ContextID, "Property 'ContextID' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aContext.ObjectState, "Object state should remain 'New' when property set.");

            aContext.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aContext.Containers.Remove(value);

            Assert.IsFalse(aContext.Containers.Contains(value), "Property 'Containers' was not updated properly.");
            Assert.IsNull(value.Context, "Property 'Context' was not set properly.");
            Assert.IsNull(value.ContextID, "Property 'ContextID' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aContext.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void ContextRemoveFromContainersWhenModified()
        {
            EAVModelLibrary.ModelContext aContext = new EAVModelLibrary.ModelContext() { ContextID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVModelLibrary.ModelRootContainer value = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };
            aContext.Containers.Add(value);
            value = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };
            aContext.Containers.Add(value);

            Assert.IsTrue(aContext.Containers.Contains(value), "Property 'Containers' was not updated properly.");
            Assert.AreEqual(aContext, value.Context, "Property 'Context' was not set properly.");
            Assert.AreEqual(aContext.ContextID, value.ContextID, "Property 'ContextID' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aContext.ObjectState, "Object state failed to transition to 'Modified'.");

            aContext.Containers.Remove(value);

            Assert.IsFalse(aContext.Containers.Contains(value), "Property 'Containers' was not updated properly.");
            Assert.IsNull(value.Context, "Property 'Context' was not set properly.");
            Assert.IsNull(value.ContextID, "Property 'ContextID' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aContext.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ContextRemoveFromContainersWhenDeleted()
        {
            EAVModelLibrary.ModelContext aContext = new EAVModelLibrary.ModelContext() { ContextID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelRootContainer value = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };
            aContext.Containers.Add(value);

            aContext.MarkUnmodified();
            value.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");
            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, value.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aContext.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, aContext.ObjectState, "Object state failed to transition to 'Deleted'.");
            Assert.AreEqual(EAV.Model.ObjectState.Deleted, value.ObjectState, "Object state failed to transition to 'Deleted'.");

            aContext.Containers.Remove(value);
        }

        [TestMethod]
        public void ContextClearContainersWhenNew()
        {
            EAVModelLibrary.ModelContext aContext = new EAVModelLibrary.ModelContext() { ContextID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelRootContainer value = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };
            aContext.Containers.Add(value);

            Assert.IsTrue(aContext.Containers.Contains(value), "Property 'Containers' was not updated properly.");
            Assert.AreEqual(aContext, value.Context, "Property 'Context' was not set properly.");
            Assert.AreEqual(aContext.ContextID, value.ContextID, "Property 'ContextID' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aContext.ObjectState, "Object state should remain 'New' when property set.");

            aContext.Containers.Clear();

            Assert.IsFalse(aContext.Containers.Contains(value), "Property 'Containers' was not updated properly.");
            Assert.IsNull(value.Context, "Property 'Context' was not set properly.");
            Assert.IsNull(value.ContextID, "Property 'ContextID' was not set properly.");
            Assert.IsFalse(aContext.Containers.Any(), "Property 'Containers' was not updated properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aContext.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ContextClearContainersWhenUnmodified()
        {
            EAVModelLibrary.ModelContext aContext = new EAVModelLibrary.ModelContext() { ContextID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelRootContainer value = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };
            aContext.Containers.Add(value);

            Assert.IsTrue(aContext.Containers.Contains(value), "Property 'Containers' was not updated properly.");
            Assert.AreEqual(aContext, value.Context, "Property 'Context' was not set properly.");
            Assert.AreEqual(aContext.ContextID, value.ContextID, "Property 'ContextID' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aContext.ObjectState, "Object state should remain 'New' when property set.");

            aContext.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aContext.Containers.Clear();

            Assert.IsFalse(aContext.Containers.Contains(value), "Property 'Containers' was not updated properly.");
            Assert.IsNull(value.Context, "Property 'Context' was not set properly.");
            Assert.IsNull(value.ContextID, "Property 'ContextID' was not set properly.");
            Assert.IsFalse(aContext.Containers.Any(), "Property 'Containers' was not updated properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aContext.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void ContextClearContainersWhenModified()
        {
            EAVModelLibrary.ModelContext aContext = new EAVModelLibrary.ModelContext() { ContextID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVModelLibrary.ModelRootContainer value = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };
            aContext.Containers.Add(value);

            Assert.IsTrue(aContext.Containers.Contains(value), "Property 'Containers' was not updated properly.");
            Assert.AreEqual(aContext, value.Context, "Property 'Context' was not set properly.");
            Assert.AreEqual(aContext.ContextID, value.ContextID, "Property 'ContextID' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aContext.ObjectState, "Object state failed to transition to 'Modified'.");

            aContext.Containers.Clear();

            Assert.IsFalse(aContext.Containers.Contains(value), "Property 'Containers' was not updated properly.");
            Assert.IsNull(value.Context, "Property 'Context' was not set properly.");
            Assert.IsNull(value.ContextID, "Property 'ContextID' was not set properly.");
            Assert.IsFalse(aContext.Containers.Any(), "Property 'Containers' was not updated properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aContext.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ContextClearContainersWhenDeleted()
        {
            EAVModelLibrary.ModelContext aContext = new EAVModelLibrary.ModelContext() { ContextID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelRootContainer value = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };
            aContext.Containers.Add(value);

            aContext.MarkUnmodified();
            value.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");
            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, value.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aContext.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, aContext.ObjectState, "Object state failed to transition to 'Deleted'.");
            Assert.AreEqual(EAV.Model.ObjectState.Deleted, value.ObjectState, "Object state failed to transition to 'Deleted'.");

            aContext.Containers.Clear();
        }
        #endregion

        #region Subjects
        [TestMethod]
        public void ContextAddToSubjectsWhenNew()
        {
            EAVModelLibrary.ModelContext aContext = new EAVModelLibrary.ModelContext() { ContextID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelSubject value = new EAVModelLibrary.ModelSubject() { SubjectID = rng.Next() };
            aContext.Subjects.Add(value);

            Assert.IsTrue(aContext.Subjects.Contains(value), "Property 'Subjects' was not updated properly.");
            Assert.AreEqual(aContext, value.Context, "Property 'Context' was not set properly.");
            Assert.AreEqual(aContext.ContextID, value.ContextID, "Property 'ContextID' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aContext.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ContextAddToSubjectsWhenUnmodified()
        {
            EAVModelLibrary.ModelContext aContext = new EAVModelLibrary.ModelContext() { ContextID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVModelLibrary.ModelSubject value = new EAVModelLibrary.ModelSubject() { SubjectID = rng.Next() };
            aContext.Subjects.Add(value);

            Assert.IsTrue(aContext.Subjects.Contains(value), "Property 'Subjects' was not updated properly.");
            Assert.AreEqual(aContext, value.Context, "Property 'Context' was not set properly.");
            Assert.AreEqual(aContext.ContextID, value.ContextID, "Property 'ContextID' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aContext.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void ContextAddToSubjectsWhenModified()
        {
            EAVModelLibrary.ModelContext aContext = new EAVModelLibrary.ModelContext() { ContextID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVModelLibrary.ModelSubject value = new EAVModelLibrary.ModelSubject() { SubjectID = rng.Next() };
            aContext.Subjects.Add(value);

            Assert.IsTrue(aContext.Subjects.Contains(value), "Property 'Subjects' was not updated properly.");
            Assert.AreEqual(aContext, value.Context, "Property 'Context' was not set properly.");
            Assert.AreEqual(aContext.ContextID, value.ContextID, "Property 'ContextID' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aContext.ObjectState, "Object state failed to transition to 'Modified'.");

            value = new EAVModelLibrary.ModelSubject() { SubjectID = rng.Next() };
            aContext.Subjects.Add(value);

            Assert.IsTrue(aContext.Subjects.Contains(value), "Property 'Subjects' was not updated properly.");
            Assert.AreEqual(aContext, value.Context, "Property 'Context' was not set properly.");
            Assert.AreEqual(aContext.ContextID, value.ContextID, "Property 'ContextID' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aContext.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ContextAddToSubjectsWhenDeleted()
        {
            EAVModelLibrary.ModelContext aContext = new EAVModelLibrary.ModelContext() { ContextID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aContext.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, aContext.ObjectState, "Object state failed to transition to 'Deleted'.");

            aContext.Subjects.Add(new EAVModelLibrary.ModelSubject() { SubjectID = rng.Next() });
        }

        [TestMethod]
        public void ContextRemoveFromSubjectsWhenNew()
        {
            EAVModelLibrary.ModelContext aContext = new EAVModelLibrary.ModelContext() { ContextID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelSubject value = new EAVModelLibrary.ModelSubject() { SubjectID = rng.Next() };
            aContext.Subjects.Add(value);
            value = new EAVModelLibrary.ModelSubject() { SubjectID = rng.Next() };
            aContext.Subjects.Add(value);

            Assert.IsTrue(aContext.Subjects.Contains(value), "Property 'Subjects' was not updated properly.");
            Assert.AreEqual(aContext, value.Context, "Property 'Context' was not set properly.");
            Assert.AreEqual(aContext.ContextID, value.ContextID, "Property 'ContextID' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aContext.ObjectState, "Object state should remain 'New' when property set.");

            aContext.Subjects.Remove(value);

            Assert.IsFalse(aContext.Subjects.Contains(value), "Property 'Subjects' was not updated properly.");
            Assert.IsNull(value.Context, "Property 'Context' was not set properly.");
            Assert.IsNull(value.ContextID, "Property 'ContextID' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aContext.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ContextRemoveFromSubjectsWhenUnmodified()
        {
            EAVModelLibrary.ModelContext aContext = new EAVModelLibrary.ModelContext() { ContextID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelSubject value = new EAVModelLibrary.ModelSubject() { SubjectID = rng.Next() };
            aContext.Subjects.Add(value);
            value = new EAVModelLibrary.ModelSubject() { SubjectID = rng.Next() };
            aContext.Subjects.Add(value);

            Assert.IsTrue(aContext.Subjects.Contains(value), "Property 'Subjects' was not updated properly.");
            Assert.AreEqual(aContext, value.Context, "Property 'Context' was not set properly.");
            Assert.AreEqual(aContext.ContextID, value.ContextID, "Property 'ContextID' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aContext.ObjectState, "Object state should remain 'New' when property set.");

            aContext.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aContext.Subjects.Remove(value);

            Assert.IsFalse(aContext.Subjects.Contains(value), "Property 'Subjects' was not updated properly.");
            Assert.IsNull(value.Context, "Property 'Context' was not set properly.");
            Assert.IsNull(value.ContextID, "Property 'ContextID' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aContext.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void ContextRemoveFromSubjectsWhenModified()
        {
            EAVModelLibrary.ModelContext aContext = new EAVModelLibrary.ModelContext() { ContextID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelSubject value = new EAVModelLibrary.ModelSubject() { SubjectID = rng.Next() };
            aContext.Subjects.Add(value);
            value = new EAVModelLibrary.ModelSubject() { SubjectID = rng.Next() };
            aContext.Subjects.Add(value);

            Assert.IsTrue(aContext.Subjects.Contains(value), "Property 'Subjects' was not updated properly.");
            Assert.AreEqual(aContext, value.Context, "Property 'Context' was not set properly.");
            Assert.AreEqual(aContext.ContextID, value.ContextID, "Property 'ContextID' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aContext.ObjectState, "Object state should remain 'New' when property set.");

            aContext.MarkUnmodified();

            aContext.Subjects.Remove(value);

            Assert.IsFalse(aContext.Subjects.Contains(value), "Property 'Containers' was not updated properly.");
            Assert.IsNull(value.Context, "Property 'Context' was not set properly.");
            Assert.IsNull(value.ContextID, "Property 'ContextID' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aContext.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ContextRemoveFromSubjectsWhenDeleted()
        {
            EAVModelLibrary.ModelContext aContext = new EAVModelLibrary.ModelContext() { ContextID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelSubject value = new EAVModelLibrary.ModelSubject() { SubjectID = rng.Next() };
            aContext.Subjects.Add(value);

            aContext.MarkUnmodified();
            value.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");
            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, value.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aContext.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, aContext.ObjectState, "Object state failed to transition to 'Deleted'.");
            Assert.AreEqual(EAV.Model.ObjectState.Deleted, value.ObjectState, "Object state failed to transition to 'Deleted'.");

            aContext.Subjects.Remove(value);
        }

        [TestMethod]
        public void ContextClearSubjectsWhenNew()
        {
            EAVModelLibrary.ModelContext aContext = new EAVModelLibrary.ModelContext() { ContextID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelSubject value = new EAVModelLibrary.ModelSubject() { SubjectID = rng.Next() };
            aContext.Subjects.Add(value);

            Assert.IsTrue(aContext.Subjects.Contains(value), "Property 'Subjects' was not updated properly.");
            Assert.AreEqual(aContext, value.Context, "Property 'Context' was not set properly.");
            Assert.AreEqual(aContext.ContextID, value.ContextID, "Property 'ContextID' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aContext.ObjectState, "Object state should remain 'New' when property set.");

            aContext.Subjects.Clear();

            Assert.IsFalse(aContext.Subjects.Contains(value), "Property 'Subjects' was not updated properly.");
            Assert.IsNull(value.Context, "Property 'Context' was not set properly.");
            Assert.IsNull(value.ContextID, "Property 'ContextID' was not set properly.");
            Assert.IsFalse(aContext.Subjects.Any(), "Property 'Subjects' was not updated properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aContext.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ContextClearSubjectsWhenUnmodified()
        {
            EAVModelLibrary.ModelContext aContext = new EAVModelLibrary.ModelContext() { ContextID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelSubject value = new EAVModelLibrary.ModelSubject() { SubjectID = rng.Next() };
            aContext.Subjects.Add(value);

            Assert.IsTrue(aContext.Subjects.Contains(value), "Property 'Subjects' was not updated properly.");
            Assert.AreEqual(aContext, value.Context, "Property 'Context' was not set properly.");
            Assert.AreEqual(aContext.ContextID, value.ContextID, "Property 'ContextID' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aContext.ObjectState, "Object state should remain 'New' when property set.");

            aContext.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aContext.Subjects.Clear();

            Assert.IsFalse(aContext.Subjects.Contains(value), "Property 'Subjects' was not updated properly.");
            Assert.IsNull(value.Context, "Property 'Context' was not set properly.");
            Assert.IsNull(value.ContextID, "Property 'ContextID' was not set properly.");
            Assert.IsFalse(aContext.Subjects.Any(), "Property 'Subjects' was not updated properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aContext.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void ContextClearSubjectsWhenModified()
        {
            EAVModelLibrary.ModelContext aContext = new EAVModelLibrary.ModelContext() { ContextID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelSubject value = new EAVModelLibrary.ModelSubject() { SubjectID = rng.Next() };
            aContext.Subjects.Add(value);

            Assert.IsTrue(aContext.Subjects.Contains(value), "Property 'Subjects' was not updated properly.");
            Assert.AreEqual(aContext, value.Context, "Property 'Context' was not set properly.");
            Assert.AreEqual(aContext.ContextID, value.ContextID, "Property 'ContextID' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aContext.ObjectState, "Object state should remain 'New' when property set.");

            aContext.MarkUnmodified();

            aContext.Subjects.Clear();

            Assert.IsFalse(aContext.Subjects.Contains(value), "Property 'Containers' was not updated properly.");
            Assert.IsNull(value.Context, "Property 'Context' was not set properly.");
            Assert.IsNull(value.ContextID, "Property 'ContextID' was not set properly.");
            Assert.IsFalse(aContext.Subjects.Any(), "Property 'Subjects' was not updated properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aContext.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ContextClearSubjectsWhenDeleted()
        {
            EAVModelLibrary.ModelContext aContext = new EAVModelLibrary.ModelContext() { ContextID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelSubject value = new EAVModelLibrary.ModelSubject() { SubjectID = rng.Next() };
            aContext.Subjects.Add(value);

            aContext.MarkUnmodified();
            value.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");
            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, value.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aContext.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, aContext.ObjectState, "Object state failed to transition to 'Deleted'.");
            Assert.AreEqual(EAV.Model.ObjectState.Deleted, value.ObjectState, "Object state failed to transition to 'Deleted'.");

            aContext.Subjects.Clear();
        }
        #endregion
        #endregion
    }
}
