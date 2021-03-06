﻿using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace EAVModelTest
{
    public partial class EAVModelTestHarness
    {
        [TestMethod]
        public void ChildInstanceCreate()
        {
            EAV.Model.IModelChildInstance aChildInstance = factory.Create<EAV.Model.IModelChildInstance>();

            Assert.AreEqual(EAV.Model.ObjectState.New, aChildInstance.ObjectState, "Object state should be 'New' on creation.");

            Assert.IsNull(aChildInstance.InstanceID, "Property 'InstanceID' should be null on creation.");

            Assert.IsNull(aChildInstance.Subject, "Property 'Subject' should be null on creation.");
            Assert.IsNull(aChildInstance.Container, "Property 'Container' should be null on creation.");
            Assert.IsNull(aChildInstance.ParentInstance, "Property 'ParentInstance' should be null on creation.");

            Assert.IsNotNull(aChildInstance.ChildInstances, "Property 'ChildInstances' should not be null on creation.");
            Assert.IsFalse(aChildInstance.ChildInstances.Any(), "Property 'ChildInstances' should be empty on creation.");

            Assert.IsNotNull(aChildInstance.Values, "Property 'Values' should not be null on creation.");
            Assert.IsFalse(aChildInstance.Values.Any(), "Property 'Values' should be empty on creation.");
        }

        #region State Transitions
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChildInstanceStateTransitionNewToUnmodifiedWithNullID()
        {
            EAV.Model.IModelChildInstance aChildInstance = factory.Create<EAV.Model.IModelChildInstance>();

            Assert.AreEqual(EAV.Model.ObjectState.New, aChildInstance.ObjectState, "Object state should be 'New' on creation.");

            aChildInstance.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aChildInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        public void ChildInstanceStateTransitionNewToUnmodifiedWithValidID()
        {
            EAVModelLibrary.ModelChildInstance aChildInstance = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aChildInstance.ObjectState, "Object state should be 'New' on creation.");

            aChildInstance.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aChildInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChildInstanceStateTransitionNewToDeleted()
        {
            EAV.Model.IModelChildInstance aChildInstance = factory.Create<EAV.Model.IModelChildInstance>();

            Assert.AreEqual(EAV.Model.ObjectState.New, aChildInstance.ObjectState, "Object state should be 'New' on creation.");

            aChildInstance.MarkDeleted();
        }

        [TestMethod]
        public void ChildInstanceStateTransitionUnmodifiedToDeleted()
        {
            EAVModelLibrary.ModelChildInstance aChildInstance = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aChildInstance.ObjectState, "Object state should be 'New' on creation.");

            aChildInstance.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aChildInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aChildInstance.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, aChildInstance.ObjectState, "Object state failed to transition to 'Deleted'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChildInstanceStateTransitionDeletedToUnmodified()
        {
            EAVModelLibrary.ModelChildInstance aChildInstance = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aChildInstance.ObjectState, "Object state should be 'New' on creation.");

            aChildInstance.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aChildInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aChildInstance.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, aChildInstance.ObjectState, "Object state failed to transition to 'Deleted'.");

            aChildInstance.MarkUnmodified();
        }
        #endregion

        #region ID Property
        [TestMethod]
        public void ChildInstanceSetIDWhenNew()
        {
            EAV.Model.IModelChildInstance aChildInstance = factory.Create<EAV.Model.IModelChildInstance>();

            Assert.AreEqual(EAV.Model.ObjectState.New, aChildInstance.ObjectState, "Object state should be 'New' on creation.");

            int id = rng.Next();
            aChildInstance.InstanceID = id;

            Assert.AreEqual(id, aChildInstance.InstanceID, "Property 'InstanceID' not properly set.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aChildInstance.ObjectState, "Object state after setting property 'InstanceID' should be 'New'.");
        }

        [TestMethod]
        public void ChildInstanceSetIDBeforeUnmodified()
        {
            EAV.Model.IModelChildInstance aChildInstance = factory.Create<EAV.Model.IModelChildInstance>();

            Assert.AreEqual(EAV.Model.ObjectState.New, aChildInstance.ObjectState, "Object state should be 'New' on creation.");

            int id = rng.Next();
            aChildInstance.InstanceID = id;

            Assert.AreEqual(id, aChildInstance.InstanceID, "Property 'InstanceID' not properly set.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aChildInstance.ObjectState, "Object state after setting property 'InstanceID' should be 'New'.");

            aChildInstance.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aChildInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChildInstanceSetIDAfterUnmodified()
        {
            EAV.Model.IModelChildInstance aChildInstance = factory.Create<EAV.Model.IModelChildInstance>();

            Assert.AreEqual(EAV.Model.ObjectState.New, aChildInstance.ObjectState, "Object state should be 'New' on creation.");

            aChildInstance.MarkUnmodified();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChildInstanceSetIDWhenDeleted()
        {
            EAV.Model.IModelChildInstance aChildInstance = factory.Create<EAV.Model.IModelChildInstance>();

            Assert.AreEqual(EAV.Model.ObjectState.New, aChildInstance.ObjectState, "Object state should be 'New' on creation.");

            int id = rng.Next();
            aChildInstance.InstanceID = id;

            Assert.AreEqual(id, aChildInstance.InstanceID, "Property 'InstanceID' not properly set.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aChildInstance.ObjectState, "Object state after setting property 'InstanceID' should be 'New'.");

            aChildInstance.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aChildInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aChildInstance.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, aChildInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            id = rng.Next();
            aChildInstance.InstanceID = id;
        }
        #endregion

        #region Primitive Properties
        // N/A
        #endregion

        #region Object Properties
        #region Container
        [TestMethod]
        public void ChildInstanceSetContainerWhenNew()
        {
            EAV.Model.IModelChildInstance aChildInstance = factory.Create<EAV.Model.IModelChildInstance>();

            Assert.AreEqual(EAV.Model.ObjectState.New, aChildInstance.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelChildContainer value = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };
            aChildInstance.Container = value;

            Assert.AreEqual(value, aChildInstance.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(value.ContainerID, aChildInstance.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aChildInstance.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ChildInstanceSetContainerWhenUnmodified()
        {
            EAVModelLibrary.ModelChildInstance aChildInstance = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aChildInstance.ObjectState, "Object state should be 'New' on creation.");

            aChildInstance.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aChildInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVModelLibrary.ModelChildContainer value = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };
            aChildInstance.Container = value;

            Assert.AreEqual(value, aChildInstance.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(value.ContainerID, aChildInstance.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aChildInstance.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void ChildInstanceSetContainerWhenModified()
        {
            EAVModelLibrary.ModelChildInstance aChildInstance = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aChildInstance.ObjectState, "Object state should be 'New' on creation.");

            aChildInstance.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aChildInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVModelLibrary.ModelChildContainer value = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };
            aChildInstance.Container = value;

            Assert.AreEqual(value, aChildInstance.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(value.ContainerID, aChildInstance.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aChildInstance.ObjectState, "Object state failed to transition to 'Modified'.");

            value = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };
            aChildInstance.Container = value;

            Assert.AreEqual(value, aChildInstance.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(value.ContainerID, aChildInstance.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aChildInstance.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChildInstanceSetContainerWhenDeleted()
        {
            EAVModelLibrary.ModelChildInstance aChildInstance = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aChildInstance.ObjectState, "Object state should be 'New' on creation.");

            aChildInstance.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aChildInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aChildInstance.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, aChildInstance.ObjectState, "Object state failed to transition to 'Deleted'.");

            aChildInstance.Container = new EAVModelLibrary.ModelChildContainer() { ContainerID = rng.Next() };
        }
        #endregion

        #region Subject
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChildInstanceSetSubjectWhenNew()
        {
            EAV.Model.IModelChildInstance aChildInstance = factory.Create<EAV.Model.IModelChildInstance>();

            Assert.AreEqual(EAV.Model.ObjectState.New, aChildInstance.ObjectState, "Object state should be 'New' on creation.");

            aChildInstance.Subject = new EAVModelLibrary.ModelSubject() { SubjectID = rng.Next() };
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChildInstanceSetSubjectWhenUnmodified()
        {
            EAVModelLibrary.ModelChildInstance aChildInstance = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aChildInstance.ObjectState, "Object state should be 'New' on creation.");

            aChildInstance.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aChildInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aChildInstance.Subject = new EAVModelLibrary.ModelSubject() { SubjectID = rng.Next() };
        }

        // Set When Modified - N/A

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChildInstanceSetSubjectWhenDeleted()
        {
            EAVModelLibrary.ModelChildInstance aChildInstance = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aChildInstance.ObjectState, "Object state should be 'New' on creation.");

            aChildInstance.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aChildInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aChildInstance.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, aChildInstance.ObjectState, "Object state failed to transition to 'Deleted'.");

            aChildInstance.Subject = new EAVModelLibrary.ModelSubject() { SubjectID = rng.Next() };
        }
        #endregion

        #region ParentInstance
        [TestMethod]
        public void ChildInstanceSetParentInstanceWhenNew()
        {
            EAV.Model.IModelChildInstance aChildInstance = factory.Create<EAV.Model.IModelChildInstance>();

            Assert.AreEqual(EAV.Model.ObjectState.New, aChildInstance.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelChildInstance value = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };
            aChildInstance.ParentInstance = value;

            Assert.AreEqual(value, aChildInstance.ParentInstance, "Property 'ParentInstance' was not set properly.");
            Assert.AreEqual(value.InstanceID, aChildInstance.ParentInstanceID, "Property 'ParentInstanceID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aChildInstance.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ChildInstanceSetParentInstanceWhenUnmodified()
        {
            EAVModelLibrary.ModelChildInstance aChildInstance = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aChildInstance.ObjectState, "Object state should be 'New' on creation.");

            aChildInstance.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aChildInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVModelLibrary.ModelChildInstance value = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };
            aChildInstance.ParentInstance = value;

            Assert.AreEqual(value, aChildInstance.ParentInstance, "Property 'ParentInstance' was not set properly.");
            Assert.AreEqual(value.InstanceID, aChildInstance.ParentInstanceID, "Property 'ParentInstanceID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aChildInstance.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void ChildInstanceSetParentInstanceWhenModified()
        {
            EAVModelLibrary.ModelChildInstance aChildInstance = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aChildInstance.ObjectState, "Object state should be 'New' on creation.");

            aChildInstance.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aChildInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVModelLibrary.ModelChildInstance value = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };
            aChildInstance.ParentInstance = value;

            Assert.AreEqual(value, aChildInstance.ParentInstance, "Property 'ParentInstance' was not set properly.");
            Assert.AreEqual(value.InstanceID, aChildInstance.ParentInstanceID, "Property 'ParentInstanceID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aChildInstance.ObjectState, "Object state failed to transition to 'Modified'.");

            value = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };
            aChildInstance.ParentInstance = value;

            Assert.AreEqual(value, aChildInstance.ParentInstance, "Property 'ParentInstance' was not set properly.");
            Assert.AreEqual(value.InstanceID, aChildInstance.ParentInstanceID, "Property 'ParentInstanceID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aChildInstance.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChildInstanceSetParentInstanceWhenDeleted()
        {
            EAVModelLibrary.ModelChildInstance aChildInstance = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aChildInstance.ObjectState, "Object state should be 'New' on creation.");

            aChildInstance.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aChildInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aChildInstance.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, aChildInstance.ObjectState, "Object state failed to transition to 'Deleted'.");

            aChildInstance.ParentInstance = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };
        }
        #endregion
        #endregion

        #region Collection Properties
        #region ChildInstances
        [TestMethod]
        public void ChildInstanceAddToChildInstancesWhenNew()
        {
            EAVModelLibrary.ModelChildInstance aParentInstance = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aParentInstance.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelChildInstance value = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };
            aParentInstance.ChildInstances.Add(value);

            Assert.IsTrue(aParentInstance.ChildInstances.Contains(value), "Property 'ChildInstances' was not updated properly.");
            Assert.AreEqual(aParentInstance, value.ParentInstance, "Property 'ParentInstance' was not set properly.");
            Assert.AreEqual(aParentInstance.InstanceID, value.ParentInstanceID, "Property 'ParentInstanceID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aParentInstance.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ChildInstanceAddToChildInstancesWhenUnmodified()
        {
            EAVModelLibrary.ModelChildInstance aParentInstance = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aParentInstance.ObjectState, "Object state should be 'New' on creation.");

            aParentInstance.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aParentInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVModelLibrary.ModelChildInstance value = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };
            aParentInstance.ChildInstances.Add(value);

            Assert.IsTrue(aParentInstance.ChildInstances.Contains(value), "Property 'ChildInstances' was not updated properly.");
            Assert.AreEqual(aParentInstance, value.ParentInstance, "Property 'ParentInstance' was not set properly.");
            Assert.AreEqual(aParentInstance.InstanceID, value.ParentInstanceID, "Property 'ParentInstanceID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aParentInstance.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void ChildInstanceAddToChildInstancesWhenModified()
        {
            EAVModelLibrary.ModelChildInstance aParentInstance = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aParentInstance.ObjectState, "Object state should be 'New' on creation.");

            aParentInstance.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aParentInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVModelLibrary.ModelChildInstance value = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };
            aParentInstance.ChildInstances.Add(value);

            Assert.IsTrue(aParentInstance.ChildInstances.Contains(value), "Property 'ChildInstances' was not updated properly.");
            Assert.AreEqual(aParentInstance, value.ParentInstance, "Property 'ParentInstance' was not set properly.");
            Assert.AreEqual(aParentInstance.InstanceID, value.ParentInstanceID, "Property 'ParentInstanceID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aParentInstance.ObjectState, "Object state failed to transition to 'Modified'.");

            value = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };
            aParentInstance.ChildInstances.Add(value);

            Assert.IsTrue(aParentInstance.ChildInstances.Contains(value), "Property 'ChildInstances' was not updated properly.");
            Assert.AreEqual(aParentInstance, value.ParentInstance, "Property 'ParentInstance' was not set properly.");
            Assert.AreEqual(aParentInstance.InstanceID, value.ParentInstanceID, "Property 'ParentInstanceID' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aParentInstance.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ChildInstanceAddToChildInstancesWhenDeleted()
        {
            EAVModelLibrary.ModelChildInstance aParentInstance = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aParentInstance.ObjectState, "Object state should be 'New' on creation.");

            aParentInstance.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aParentInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aParentInstance.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, aParentInstance.ObjectState, "Object state failed to transition to 'Deleted'.");

            aParentInstance.ChildInstances.Add(new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() });
        }

        [TestMethod]
        public void ChildInstanceRemoveFromChildInstancesWhenNew()
        {
            EAVModelLibrary.ModelChildInstance aParentInstance = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aParentInstance.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelChildInstance value = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };
            aParentInstance.ChildInstances.Add(value);
            value = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };
            aParentInstance.ChildInstances.Add(value);

            Assert.IsTrue(aParentInstance.ChildInstances.Contains(value), "Property 'ChildInstances' was not updated properly.");
            Assert.AreEqual(aParentInstance, value.ParentInstance, "Property 'ParentInstance' was not set properly.");
            Assert.AreEqual(aParentInstance.InstanceID, value.ParentInstanceID, "Property 'ParentInstanceID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aParentInstance.ObjectState, "Object state should remain 'New' when property set.");

            aParentInstance.ChildInstances.Remove(value);

            Assert.IsFalse(aParentInstance.ChildInstances.Contains(value), "Property 'ChildInstances' was not updated properly.");
            Assert.IsNull(value.ParentInstance, "Property 'ParentInstance' was not set properly.");
            Assert.IsNull(value.ParentInstanceID, "Property 'ParentInstanceID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aParentInstance.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ChildInstanceRemoveFromChildInstancesWhenUnmodified()
        {
            EAVModelLibrary.ModelChildInstance aParentInstance = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aParentInstance.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelChildInstance value = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };
            aParentInstance.ChildInstances.Add(value);
            value = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };
            aParentInstance.ChildInstances.Add(value);

            Assert.IsTrue(aParentInstance.ChildInstances.Contains(value), "Property 'ChildInstances' was not updated properly.");
            Assert.AreEqual(aParentInstance, value.ParentInstance, "Property 'ParentInstance' was not set properly.");
            Assert.AreEqual(aParentInstance.InstanceID, value.ParentInstanceID, "Property 'ParentInstanceID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aParentInstance.ObjectState, "Object state should remain 'New' when property set.");

            aParentInstance.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aParentInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aParentInstance.ChildInstances.Remove(value);

            Assert.IsFalse(aParentInstance.ChildInstances.Contains(value), "Property 'ChildInstances' was not updated properly.");
            Assert.IsNull(value.ParentInstance, "Property 'ParentInstance' was not set properly.");
            Assert.IsNull(value.ParentInstanceID, "Property 'ParentInstanceID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aParentInstance.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void ChildInstanceRemoveFromChildInstancesWhenModified()
        {
            EAVModelLibrary.ModelChildInstance aParentInstance = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aParentInstance.ObjectState, "Object state should be 'New' on creation.");

            aParentInstance.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aParentInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVModelLibrary.ModelChildInstance value = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };
            aParentInstance.ChildInstances.Add(value);
            value = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };
            aParentInstance.ChildInstances.Add(value);

            Assert.IsTrue(aParentInstance.ChildInstances.Contains(value), "Property 'ChildInstances' was not updated properly.");
            Assert.AreEqual(aParentInstance, value.ParentInstance, "Property 'ParentInstance' was not set properly.");
            Assert.AreEqual(aParentInstance.InstanceID, value.ParentInstanceID, "Property 'ParentInstanceID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aParentInstance.ObjectState, "Object state failed to transition to 'Modified'.");

            aParentInstance.ChildInstances.Remove(value);

            Assert.IsFalse(aParentInstance.ChildInstances.Contains(value), "Property 'ChildInstances' was not updated properly.");
            Assert.IsNull(value.ParentInstance, "Property 'ParentInstance' was not set properly.");
            Assert.IsNull(value.ParentInstanceID, "Property 'ParentInstanceID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aParentInstance.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ChildInstanceRemoveFromChildInstancesWhenDeleted()
        {
            EAVModelLibrary.ModelChildInstance aParentInstance = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aParentInstance.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelChildInstance value = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };
            aParentInstance.ChildInstances.Add(value);

            Assert.IsTrue(aParentInstance.ChildInstances.Contains(value), "Property 'ChildInstances' was not updated properly.");
            Assert.AreEqual(aParentInstance, value.ParentInstance, "Property 'ParentInstance' was not set properly.");
            Assert.AreEqual(aParentInstance.InstanceID, value.ParentInstanceID, "Property 'ParentInstanceID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aParentInstance.ObjectState, "Object state should remain 'New' when property set.");

            aParentInstance.MarkUnmodified();
            value.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aParentInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");
            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, value.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aParentInstance.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, aParentInstance.ObjectState, "Object state failed to transition to 'Deleted'.");
            Assert.AreEqual(EAV.Model.ObjectState.Deleted, value.ObjectState, "Object state failed to transition to 'Deleted'.");

            aParentInstance.ChildInstances.Remove(value);
        }

        [TestMethod]
        public void ChildInstanceClearChildInstancesWhenNew()
        {
            EAVModelLibrary.ModelChildInstance aParentInstance = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aParentInstance.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelChildInstance value = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };
            aParentInstance.ChildInstances.Add(value);
            value = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };
            aParentInstance.ChildInstances.Add(value);

            Assert.IsTrue(aParentInstance.ChildInstances.Contains(value), "Property 'ChildInstances' was not updated properly.");
            Assert.AreEqual(aParentInstance, value.ParentInstance, "Property 'ParentInstance' was not set properly.");
            Assert.AreEqual(aParentInstance.InstanceID, value.ParentInstanceID, "Property 'ParentInstanceID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aParentInstance.ObjectState, "Object state should remain 'New' when property set.");

            aParentInstance.ChildInstances.Clear();

            Assert.IsFalse(aParentInstance.ChildInstances.Contains(value), "Property 'ChildInstances' was not updated properly.");
            Assert.IsNull(value.ParentInstance, "Property 'ParentInstance' was not set properly.");
            Assert.IsNull(value.ParentInstanceID, "Property 'ParentInstanceID' was not reported properly.");
            Assert.IsFalse(aParentInstance.ChildInstances.Any(), "Property 'ChildInstances' was not updated properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aParentInstance.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ChildInstanceClearChildInstancesWhenUnmodified()
        {
            EAVModelLibrary.ModelChildInstance aParentInstance = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aParentInstance.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelChildInstance value = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };
            aParentInstance.ChildInstances.Add(value);
            value = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };
            aParentInstance.ChildInstances.Add(value);

            Assert.IsTrue(aParentInstance.ChildInstances.Contains(value), "Property 'ChildInstances' was not updated properly.");
            Assert.AreEqual(aParentInstance, value.ParentInstance, "Property 'ParentInstance' was not set properly.");
            Assert.AreEqual(aParentInstance.InstanceID, value.ParentInstanceID, "Property 'ParentInstanceID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aParentInstance.ObjectState, "Object state should remain 'New' when property set.");

            aParentInstance.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aParentInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aParentInstance.ChildInstances.Clear();

            Assert.IsFalse(aParentInstance.ChildInstances.Contains(value), "Property 'ChildInstances' was not updated properly.");
            Assert.IsNull(value.ParentInstance, "Property 'ParentInstance' was not set properly.");
            Assert.IsNull(value.ParentInstanceID, "Property 'ParentInstanceID' was not reported properly.");
            Assert.IsFalse(aParentInstance.ChildInstances.Any(), "Property 'ChildInstances' was not updated properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aParentInstance.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void ChildInstanceClearChildInstancesWhenModified()
        {
            EAVModelLibrary.ModelChildInstance aParentInstance = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aParentInstance.ObjectState, "Object state should be 'New' on creation.");

            aParentInstance.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aParentInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVModelLibrary.ModelChildInstance value = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };
            aParentInstance.ChildInstances.Add(value);
            value = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };
            aParentInstance.ChildInstances.Add(value);

            Assert.IsTrue(aParentInstance.ChildInstances.Contains(value), "Property 'ChildInstances' was not updated properly.");
            Assert.AreEqual(aParentInstance, value.ParentInstance, "Property 'ParentInstance' was not set properly.");
            Assert.AreEqual(aParentInstance.InstanceID, value.ParentInstanceID, "Property 'ParentInstanceID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aParentInstance.ObjectState, "Object state failed to transition to 'Modified'.");

            aParentInstance.ChildInstances.Clear();

            Assert.IsFalse(aParentInstance.ChildInstances.Contains(value), "Property 'ChildInstances' was not updated properly.");
            Assert.IsNull(value.ParentInstance, "Property 'ParentInstance' was not set properly.");
            Assert.IsNull(value.ParentInstanceID, "Property 'ParentInstanceID' was not reported properly.");
            Assert.IsFalse(aParentInstance.ChildInstances.Any(), "Property 'ChildInstances' was not updated properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aParentInstance.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ChildInstanceClearChildInstancesWhenDeleted()
        {
            EAVModelLibrary.ModelChildInstance aParentInstance = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aParentInstance.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelChildInstance value = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };
            aParentInstance.ChildInstances.Add(value);

            Assert.IsTrue(aParentInstance.ChildInstances.Contains(value), "Property 'ChildInstances' was not updated properly.");
            Assert.AreEqual(aParentInstance, value.ParentInstance, "Property 'ParentInstance' was not set properly.");
            Assert.AreEqual(aParentInstance.InstanceID, value.ParentInstanceID, "Property 'ParentInstanceID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aParentInstance.ObjectState, "Object state should remain 'New' when property set.");

            aParentInstance.MarkUnmodified();
            value.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aParentInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");
            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, value.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aParentInstance.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, aParentInstance.ObjectState, "Object state failed to transition to 'Deleted'.");
            Assert.AreEqual(EAV.Model.ObjectState.Deleted, value.ObjectState, "Object state failed to transition to 'Deleted'.");

            aParentInstance.ChildInstances.Clear();
        }
        #endregion

        #region Values
        [TestMethod]
        public void ChildInstanceAddToValuesWhenNew()
        {
            EAVModelLibrary.ModelChildInstance anInstance = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, anInstance.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelValue value = new EAVModelLibrary.ModelValue();
            anInstance.Values.Add(value);

            Assert.IsTrue(anInstance.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.AreEqual(anInstance, value.Instance, "Property 'Instance' was not set properly.");
            Assert.AreEqual(anInstance.InstanceID, value.InstanceID, "Property 'InstanceID' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, anInstance.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ChildInstanceAddToValuesWhenUnmodified()
        {
            EAVModelLibrary.ModelChildInstance anInstance = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, anInstance.ObjectState, "Object state should be 'New' on creation.");

            anInstance.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, anInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVModelLibrary.ModelValue value = new EAVModelLibrary.ModelValue();
            anInstance.Values.Add(value);

            Assert.IsTrue(anInstance.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.AreEqual(anInstance, value.Instance, "Property 'Instance' was not set properly.");
            Assert.AreEqual(anInstance.InstanceID, value.InstanceID, "Property 'InstanceID' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, anInstance.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void ChildInstanceAddToValuesWhenModified()
        {
            EAVModelLibrary.ModelChildInstance anInstance = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, anInstance.ObjectState, "Object state should be 'New' on creation.");

            anInstance.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, anInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVModelLibrary.ModelValue value = new EAVModelLibrary.ModelValue();
            anInstance.Values.Add(value);

            Assert.IsTrue(anInstance.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.AreEqual(anInstance, value.Instance, "Property 'Instance' was not set properly.");
            Assert.AreEqual(anInstance.InstanceID, value.InstanceID, "Property 'InstanceID' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, anInstance.ObjectState, "Object state failed to transition to 'Modified'.");

            value = new EAVModelLibrary.ModelValue();
            anInstance.Values.Add(value);

            Assert.IsTrue(anInstance.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.AreEqual(anInstance, value.Instance, "Property 'Instance' was not set properly.");
            Assert.AreEqual(anInstance.InstanceID, value.InstanceID, "Property 'InstanceID' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, anInstance.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ChildInstanceAddToValuesWhenDeleted()
        {
            EAVModelLibrary.ModelChildInstance anInstance = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, anInstance.ObjectState, "Object state should be 'New' on creation.");

            anInstance.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, anInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anInstance.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, anInstance.ObjectState, "Object state failed to transition to 'Deleted'.");

            anInstance.Values.Add(new EAVModelLibrary.ModelValue());
        }

        [TestMethod]
        public void ChildInstanceRemoveFromValuesWhenNew()
        {
            EAVModelLibrary.ModelChildInstance anInstance = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, anInstance.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelValue value = new EAVModelLibrary.ModelValue();
            anInstance.Values.Add(value);
            value = new EAVModelLibrary.ModelValue();
            anInstance.Values.Add(value);

            Assert.IsTrue(anInstance.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.AreEqual(anInstance, value.Instance, "Property 'Instance' was not set properly.");
            Assert.AreEqual(anInstance.InstanceID, value.InstanceID, "Property 'InstanceID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, anInstance.ObjectState, "Object state should remain 'New' when property set.");

            anInstance.Values.Remove(value);

            Assert.IsFalse(anInstance.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.IsNull(value.Instance, "Property 'Instance' was not set properly.");
            Assert.IsNull(value.InstanceID, "Property 'InstanceID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, anInstance.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ChildInstanceRemoveFromValuesWhenUnmodified()
        {
            EAVModelLibrary.ModelChildInstance anInstance = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, anInstance.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelValue value = new EAVModelLibrary.ModelValue();
            anInstance.Values.Add(value);
            value = new EAVModelLibrary.ModelValue();
            anInstance.Values.Add(value);

            Assert.IsTrue(anInstance.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.AreEqual(anInstance, value.Instance, "Property 'Instance' was not set properly.");
            Assert.AreEqual(anInstance.InstanceID, value.InstanceID, "Property 'InstanceID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, anInstance.ObjectState, "Object state should remain 'New' when property set.");

            anInstance.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, anInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anInstance.Values.Remove(value);

            Assert.IsFalse(anInstance.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.IsNull(value.Instance, "Property 'Instance' was not set properly.");
            Assert.IsNull(value.InstanceID, "Property 'InstanceID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, anInstance.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void ChildInstanceRemoveFromValuesWhenModified()
        {
            EAVModelLibrary.ModelChildInstance anInstance = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, anInstance.ObjectState, "Object state should be 'New' on creation.");

            anInstance.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, anInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVModelLibrary.ModelValue value = new EAVModelLibrary.ModelValue();
            anInstance.Values.Add(value);
            value = new EAVModelLibrary.ModelValue();
            anInstance.Values.Add(value);

            Assert.IsTrue(anInstance.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.AreEqual(anInstance, value.Instance, "Property 'Instance' was not set properly.");
            Assert.AreEqual(anInstance.InstanceID, value.InstanceID, "Property 'InstanceID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, anInstance.ObjectState, "Object state failed to transition to 'Modified'.");

            anInstance.Values.Remove(value);

            Assert.IsFalse(anInstance.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.IsNull(value.Instance, "Property 'Instance' was not set properly.");
            Assert.IsNull(value.InstanceID, "Property 'InstanceID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, anInstance.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ChildInstanceRemoveFromValuesWhenDeleted()
        {
            EAVModelLibrary.ModelChildInstance anInstance = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, anInstance.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelValue value = new EAVModelLibrary.ModelValue() { Attribute = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() } };
            anInstance.Values.Add(value);

            Assert.IsTrue(anInstance.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.AreEqual(anInstance, value.Instance, "Property 'Instance' was not set properly.");
            Assert.AreEqual(anInstance.InstanceID, value.InstanceID, "Property 'InstanceID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, anInstance.ObjectState, "Object state should remain 'New' when property set.");

            anInstance.MarkUnmodified();
            value.Attribute.MarkUnmodified();
            value.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, anInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");
            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, value.Attribute.ObjectState, "Object state failed to transition to 'Unmodified'.");
            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, value.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anInstance.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, anInstance.ObjectState, "Object state failed to transition to 'Deleted'.");
            Assert.AreEqual(EAV.Model.ObjectState.Deleted, value.ObjectState, "Object state failed to transition to 'Deleted'.");

            anInstance.Values.Remove(value);
        }

        [TestMethod]
        public void ChildInstanceClearValuesWhenNew()
        {
            EAVModelLibrary.ModelChildInstance anInstance = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, anInstance.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelValue value = new EAVModelLibrary.ModelValue();
            anInstance.Values.Add(value);
            value = new EAVModelLibrary.ModelValue();
            anInstance.Values.Add(value);

            Assert.IsTrue(anInstance.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.AreEqual(anInstance, value.Instance, "Property 'Instance' was not set properly.");
            Assert.AreEqual(anInstance.InstanceID, value.InstanceID, "Property 'InstanceID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, anInstance.ObjectState, "Object state should remain 'New' when property set.");

            anInstance.Values.Clear();

            Assert.IsFalse(anInstance.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.IsNull(value.Instance, "Property 'Instance' was not set properly.");
            Assert.IsNull(value.InstanceID, "Property 'InstanceID' was not reported properly.");
            Assert.IsFalse(anInstance.Values.Any(), "Property 'Values' was not updated properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, anInstance.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ChildInstanceClearValuesWhenUnmodified()
        {
            EAVModelLibrary.ModelChildInstance anInstance = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, anInstance.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelValue value = new EAVModelLibrary.ModelValue();
            anInstance.Values.Add(value);
            value = new EAVModelLibrary.ModelValue();
            anInstance.Values.Add(value);

            Assert.IsTrue(anInstance.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.AreEqual(anInstance, value.Instance, "Property 'Instance' was not set properly.");
            Assert.AreEqual(anInstance.InstanceID, value.InstanceID, "Property 'InstanceID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, anInstance.ObjectState, "Object state should remain 'New' when property set.");

            anInstance.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, anInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anInstance.Values.Clear();

            Assert.IsFalse(anInstance.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.IsNull(value.Instance, "Property 'Instance' was not set properly.");
            Assert.IsNull(value.InstanceID, "Property 'InstanceID' was not reported properly.");
            Assert.IsFalse(anInstance.Values.Any(), "Property 'Values' was not updated properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, anInstance.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void ChildInstanceClearValuesWhenModified()
        {
            EAVModelLibrary.ModelChildInstance anInstance = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, anInstance.ObjectState, "Object state should be 'New' on creation.");

            anInstance.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, anInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVModelLibrary.ModelValue value = new EAVModelLibrary.ModelValue();
            anInstance.Values.Add(value);
            value = new EAVModelLibrary.ModelValue();
            anInstance.Values.Add(value);

            Assert.IsTrue(anInstance.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.AreEqual(anInstance, value.Instance, "Property 'Instance' was not set properly.");
            Assert.AreEqual(anInstance.InstanceID, value.InstanceID, "Property 'InstanceID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, anInstance.ObjectState, "Object state failed to transition to 'Modified'.");

            anInstance.Values.Clear();

            Assert.IsFalse(anInstance.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.IsNull(value.Instance, "Property 'Instance' was not set properly.");
            Assert.IsNull(value.InstanceID, "Property 'InstanceID' was not reported properly.");
            Assert.IsFalse(anInstance.Values.Any(), "Property 'Values' was not updated properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, anInstance.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ChildInstanceClearValuesWhenDeleted()
        {
            EAVModelLibrary.ModelChildInstance anInstance = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, anInstance.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelValue value = new EAVModelLibrary.ModelValue() { Attribute = new EAVModelLibrary.ModelAttribute() { AttributeID = rng.Next() } };
            anInstance.Values.Add(value);

            Assert.IsTrue(anInstance.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.AreEqual(anInstance, value.Instance, "Property 'Instance' was not set properly.");
            Assert.AreEqual(anInstance.InstanceID, value.InstanceID, "Property 'InstanceID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, anInstance.ObjectState, "Object state should remain 'New' when property set.");

            anInstance.MarkUnmodified();
            value.Attribute.MarkUnmodified();
            value.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, anInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");
            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, value.Attribute.ObjectState, "Object state failed to transition to 'Unmodified'.");
            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, value.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anInstance.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, anInstance.ObjectState, "Object state failed to transition to 'Deleted'.");
            Assert.AreEqual(EAV.Model.ObjectState.Deleted, value.ObjectState, "Object state failed to transition to 'Deleted'.");

            anInstance.Values.Clear();
        }
        #endregion
        #endregion
    }
}
