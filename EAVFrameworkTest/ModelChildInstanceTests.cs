using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using EAV.Model;


namespace EAVModelTest
{
    public partial class EAVModelTestHarness
    {
        [TestMethod]
        public void ChildInstanceCreate()
        {
            ModelChildInstance aChildInstance = new ModelChildInstance();

            Assert.AreEqual(ObjectState.New, aChildInstance.ObjectState, "Object state should be 'New' on creation.");

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
            ModelChildInstance aChildInstance = new ModelChildInstance();

            Assert.AreEqual(ObjectState.New, aChildInstance.ObjectState, "Object state should be 'New' on creation.");

            aChildInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aChildInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        public void ChildInstanceStateTransitionNewToUnmodifiedWithValidID()
        {
            ModelChildInstance aChildInstance = new ModelChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aChildInstance.ObjectState, "Object state should be 'New' on creation.");

            aChildInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aChildInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChildInstanceStateTransitionNewToDeleted()
        {
            ModelChildInstance aChildInstance = new ModelChildInstance();

            Assert.AreEqual(ObjectState.New, aChildInstance.ObjectState, "Object state should be 'New' on creation.");

            aChildInstance.MarkDeleted();
        }

        [TestMethod]
        public void ChildInstanceStateTransitionUnmodifiedToDeleted()
        {
            ModelChildInstance aChildInstance = new ModelChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aChildInstance.ObjectState, "Object state should be 'New' on creation.");

            aChildInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aChildInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aChildInstance.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aChildInstance.ObjectState, "Object state failed to transition to 'Deleted'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChildInstanceStateTransitionDeletedToUnmodified()
        {
            ModelChildInstance aChildInstance = new ModelChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aChildInstance.ObjectState, "Object state should be 'New' on creation.");

            aChildInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aChildInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aChildInstance.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aChildInstance.ObjectState, "Object state failed to transition to 'Deleted'.");

            aChildInstance.MarkUnmodified();
        }
        #endregion

        #region ID Property
        [TestMethod]
        public void ChildInstanceSetIDWhenNew()
        {
            ModelChildInstance aChildInstance = new ModelChildInstance();

            Assert.AreEqual(ObjectState.New, aChildInstance.ObjectState, "Object state should be 'New' on creation.");

            int id = rng.Next();
            aChildInstance.InstanceID = id;

            Assert.AreEqual(id, aChildInstance.InstanceID, "Property 'InstanceID' not properly set.");
            Assert.AreEqual(ObjectState.New, aChildInstance.ObjectState, "Object state after setting property 'InstanceID' should be 'New'.");
        }

        [TestMethod]
        public void ChildInstanceSetIDBeforeUnmodified()
        {
            ModelChildInstance aChildInstance = new ModelChildInstance();

            Assert.AreEqual(ObjectState.New, aChildInstance.ObjectState, "Object state should be 'New' on creation.");

            int id = rng.Next();
            aChildInstance.InstanceID = id;

            Assert.AreEqual(id, aChildInstance.InstanceID, "Property 'InstanceID' not properly set.");
            Assert.AreEqual(ObjectState.New, aChildInstance.ObjectState, "Object state after setting property 'InstanceID' should be 'New'.");

            aChildInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aChildInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChildInstanceSetIDAfterUnmodified()
        {
            ModelChildInstance aChildInstance = new ModelChildInstance();

            Assert.AreEqual(ObjectState.New, aChildInstance.ObjectState, "Object state should be 'New' on creation.");

            aChildInstance.MarkUnmodified();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChildInstanceSetIDWhenDeleted()
        {
            ModelChildInstance aChildInstance = new ModelChildInstance();

            Assert.AreEqual(ObjectState.New, aChildInstance.ObjectState, "Object state should be 'New' on creation.");

            int id = rng.Next();
            aChildInstance.InstanceID = id;

            Assert.AreEqual(id, aChildInstance.InstanceID, "Property 'InstanceID' not properly set.");
            Assert.AreEqual(ObjectState.New, aChildInstance.ObjectState, "Object state after setting property 'InstanceID' should be 'New'.");

            aChildInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aChildInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aChildInstance.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aChildInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

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
            ModelChildInstance aChildInstance = new ModelChildInstance();

            Assert.AreEqual(ObjectState.New, aChildInstance.ObjectState, "Object state should be 'New' on creation.");

            ModelChildContainer value = new ModelChildContainer() { ContainerID = rng.Next() };
            aChildInstance.Container = value;

            Assert.AreEqual(value, aChildInstance.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(value.ContainerID, aChildInstance.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.AreEqual(ObjectState.New, aChildInstance.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ChildInstanceSetContainerWhenUnmodified()
        {
            ModelChildInstance aChildInstance = new ModelChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aChildInstance.ObjectState, "Object state should be 'New' on creation.");

            aChildInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aChildInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            ModelChildContainer value = new ModelChildContainer() { ContainerID = rng.Next() };
            aChildInstance.Container = value;

            Assert.AreEqual(value, aChildInstance.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(value.ContainerID, aChildInstance.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.AreEqual(ObjectState.Modified, aChildInstance.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void ChildInstanceSetContainerWhenModified()
        {
            ModelChildInstance aChildInstance = new ModelChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aChildInstance.ObjectState, "Object state should be 'New' on creation.");

            aChildInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aChildInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            ModelChildContainer value = new ModelChildContainer() { ContainerID = rng.Next() };
            aChildInstance.Container = value;

            Assert.AreEqual(value, aChildInstance.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(value.ContainerID, aChildInstance.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.AreEqual(ObjectState.Modified, aChildInstance.ObjectState, "Object state failed to transition to 'Modified'.");

            value = new ModelChildContainer() { ContainerID = rng.Next() };
            aChildInstance.Container = value;

            Assert.AreEqual(value, aChildInstance.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(value.ContainerID, aChildInstance.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.AreEqual(ObjectState.Modified, aChildInstance.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChildInstanceSetContainerWhenDeleted()
        {
            ModelChildInstance aChildInstance = new ModelChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aChildInstance.ObjectState, "Object state should be 'New' on creation.");

            aChildInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aChildInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aChildInstance.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aChildInstance.ObjectState, "Object state failed to transition to 'Deleted'.");

            aChildInstance.Container = new ModelChildContainer() { ContainerID = rng.Next() };
        }
        #endregion

        #region Subject
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChildInstanceSetSubjectWhenNew()
        {
            ModelChildInstance aChildInstance = new ModelChildInstance();

            Assert.AreEqual(ObjectState.New, aChildInstance.ObjectState, "Object state should be 'New' on creation.");

            aChildInstance.Subject = new ModelSubject() { SubjectID = rng.Next() };
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChildInstanceSetSubjectWhenUnmodified()
        {
            ModelChildInstance aChildInstance = new ModelChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aChildInstance.ObjectState, "Object state should be 'New' on creation.");

            aChildInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aChildInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aChildInstance.Subject = new ModelSubject() { SubjectID = rng.Next() };
        }

        // Set When Modified - N/A

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChildInstanceSetSubjectWhenDeleted()
        {
            ModelChildInstance aChildInstance = new ModelChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aChildInstance.ObjectState, "Object state should be 'New' on creation.");

            aChildInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aChildInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aChildInstance.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aChildInstance.ObjectState, "Object state failed to transition to 'Deleted'.");

            aChildInstance.Subject = new ModelSubject() { SubjectID = rng.Next() };
        }
        #endregion

        #region ParentInstance
        [TestMethod]
        public void ChildInstanceSetParentInstanceWhenNew()
        {
            ModelChildInstance aChildInstance = new ModelChildInstance();

            Assert.AreEqual(ObjectState.New, aChildInstance.ObjectState, "Object state should be 'New' on creation.");

            ModelChildInstance value = new ModelChildInstance() { InstanceID = rng.Next() };
            aChildInstance.ParentInstance = value;

            Assert.AreEqual(value, aChildInstance.ParentInstance, "Property 'ParentInstance' was not set properly.");
            Assert.AreEqual(value.InstanceID, aChildInstance.ParentInstanceID, "Property 'ParentInstanceID' was not reported properly.");
            Assert.AreEqual(ObjectState.New, aChildInstance.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ChildInstanceSetParentInstanceWhenUnmodified()
        {
            ModelChildInstance aChildInstance = new ModelChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aChildInstance.ObjectState, "Object state should be 'New' on creation.");

            aChildInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aChildInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            ModelChildInstance value = new ModelChildInstance() { InstanceID = rng.Next() };
            aChildInstance.ParentInstance = value;

            Assert.AreEqual(value, aChildInstance.ParentInstance, "Property 'ParentInstance' was not set properly.");
            Assert.AreEqual(value.InstanceID, aChildInstance.ParentInstanceID, "Property 'ParentInstanceID' was not reported properly.");
            Assert.AreEqual(ObjectState.Modified, aChildInstance.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void ChildInstanceSetParentInstanceWhenModified()
        {
            ModelChildInstance aChildInstance = new ModelChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aChildInstance.ObjectState, "Object state should be 'New' on creation.");

            aChildInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aChildInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            ModelChildInstance value = new ModelChildInstance() { InstanceID = rng.Next() };
            aChildInstance.ParentInstance = value;

            Assert.AreEqual(value, aChildInstance.ParentInstance, "Property 'ParentInstance' was not set properly.");
            Assert.AreEqual(value.InstanceID, aChildInstance.ParentInstanceID, "Property 'ParentInstanceID' was not reported properly.");
            Assert.AreEqual(ObjectState.Modified, aChildInstance.ObjectState, "Object state failed to transition to 'Modified'.");

            value = new ModelChildInstance() { InstanceID = rng.Next() };
            aChildInstance.ParentInstance = value;

            Assert.AreEqual(value, aChildInstance.ParentInstance, "Property 'ParentInstance' was not set properly.");
            Assert.AreEqual(value.InstanceID, aChildInstance.ParentInstanceID, "Property 'ParentInstanceID' was not reported properly.");
            Assert.AreEqual(ObjectState.Modified, aChildInstance.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChildInstanceSetParentInstanceWhenDeleted()
        {
            ModelChildInstance aChildInstance = new ModelChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aChildInstance.ObjectState, "Object state should be 'New' on creation.");

            aChildInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aChildInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aChildInstance.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aChildInstance.ObjectState, "Object state failed to transition to 'Deleted'.");

            aChildInstance.ParentInstance = new ModelChildInstance() { InstanceID = rng.Next() };
        }
        #endregion
        #endregion

        #region Collection Properties
        #region ChildInstances
        [TestMethod]
        public void ChildInstanceAddToChildInstancesWhenNew()
        {
            ModelChildInstance aParentInstance = new ModelChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aParentInstance.ObjectState, "Object state should be 'New' on creation.");

            ModelChildInstance value = new ModelChildInstance() { InstanceID = rng.Next() };
            aParentInstance.ChildInstances.Add(value);

            Assert.IsTrue(aParentInstance.ChildInstances.Contains(value), "Property 'ChildInstances' was not updated properly.");
            Assert.AreEqual(aParentInstance, value.ParentInstance, "Property 'ParentInstance' was not set properly.");
            Assert.AreEqual(aParentInstance.InstanceID, value.ParentInstanceID, "Property 'ParentInstanceID' was not reported properly.");
            Assert.AreEqual(ObjectState.New, aParentInstance.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ChildInstanceAddToChildInstancesWhenUnmodified()
        {
            ModelChildInstance aParentInstance = new ModelChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aParentInstance.ObjectState, "Object state should be 'New' on creation.");

            aParentInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aParentInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            ModelChildInstance value = new ModelChildInstance() { InstanceID = rng.Next() };
            aParentInstance.ChildInstances.Add(value);

            Assert.IsTrue(aParentInstance.ChildInstances.Contains(value), "Property 'ChildInstances' was not updated properly.");
            Assert.AreEqual(aParentInstance, value.ParentInstance, "Property 'ParentInstance' was not set properly.");
            Assert.AreEqual(aParentInstance.InstanceID, value.ParentInstanceID, "Property 'ParentInstanceID' was not reported properly.");
            Assert.AreEqual(ObjectState.Modified, aParentInstance.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void ChildInstanceAddToChildInstancesWhenModified()
        {
            ModelChildInstance aParentInstance = new ModelChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aParentInstance.ObjectState, "Object state should be 'New' on creation.");

            aParentInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aParentInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            ModelChildInstance value = new ModelChildInstance() { InstanceID = rng.Next() };
            aParentInstance.ChildInstances.Add(value);

            Assert.IsTrue(aParentInstance.ChildInstances.Contains(value), "Property 'ChildInstances' was not updated properly.");
            Assert.AreEqual(aParentInstance, value.ParentInstance, "Property 'ParentInstance' was not set properly.");
            Assert.AreEqual(aParentInstance.InstanceID, value.ParentInstanceID, "Property 'ParentInstanceID' was not reported properly.");
            Assert.AreEqual(ObjectState.Modified, aParentInstance.ObjectState, "Object state failed to transition to 'Modified'.");

            value = new ModelChildInstance() { InstanceID = rng.Next() };
            aParentInstance.ChildInstances.Add(value);

            Assert.IsTrue(aParentInstance.ChildInstances.Contains(value), "Property 'ChildInstances' was not updated properly.");
            Assert.AreEqual(aParentInstance, value.ParentInstance, "Property 'ParentInstance' was not set properly.");
            Assert.AreEqual(aParentInstance.InstanceID, value.ParentInstanceID, "Property 'ParentInstanceID' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aParentInstance.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ChildInstanceAddToChildInstancesWhenDeleted()
        {
            ModelChildInstance aParentInstance = new ModelChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aParentInstance.ObjectState, "Object state should be 'New' on creation.");

            aParentInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aParentInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aParentInstance.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aParentInstance.ObjectState, "Object state failed to transition to 'Deleted'.");

            aParentInstance.ChildInstances.Add(new ModelChildInstance() { InstanceID = rng.Next() });
        }

        [TestMethod]
        public void ChildInstanceRemoveFromChildInstancesWhenNew()
        {
            ModelChildInstance aParentInstance = new ModelChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aParentInstance.ObjectState, "Object state should be 'New' on creation.");

            ModelChildInstance value = new ModelChildInstance() { InstanceID = rng.Next() };
            aParentInstance.ChildInstances.Add(value);
            value = new ModelChildInstance() { InstanceID = rng.Next() };
            aParentInstance.ChildInstances.Add(value);

            Assert.IsTrue(aParentInstance.ChildInstances.Contains(value), "Property 'ChildInstances' was not updated properly.");
            Assert.AreEqual(aParentInstance, value.ParentInstance, "Property 'ParentInstance' was not set properly.");
            Assert.AreEqual(aParentInstance.InstanceID, value.ParentInstanceID, "Property 'ParentInstanceID' was not reported properly.");
            Assert.AreEqual(ObjectState.New, aParentInstance.ObjectState, "Object state should remain 'New' when property set.");

            aParentInstance.ChildInstances.Remove(value);

            Assert.IsFalse(aParentInstance.ChildInstances.Contains(value), "Property 'ChildInstances' was not updated properly.");
            Assert.IsNull(value.ParentInstance, "Property 'ParentInstance' was not set properly.");
            Assert.IsNull(value.ParentInstanceID, "Property 'ParentInstanceID' was not reported properly.");
            Assert.AreEqual(ObjectState.New, aParentInstance.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ChildInstanceRemoveFromChildInstancesWhenUnmodified()
        {
            ModelChildInstance aParentInstance = new ModelChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aParentInstance.ObjectState, "Object state should be 'New' on creation.");

            ModelChildInstance value = new ModelChildInstance() { InstanceID = rng.Next() };
            aParentInstance.ChildInstances.Add(value);
            value = new ModelChildInstance() { InstanceID = rng.Next() };
            aParentInstance.ChildInstances.Add(value);

            Assert.IsTrue(aParentInstance.ChildInstances.Contains(value), "Property 'ChildInstances' was not updated properly.");
            Assert.AreEqual(aParentInstance, value.ParentInstance, "Property 'ParentInstance' was not set properly.");
            Assert.AreEqual(aParentInstance.InstanceID, value.ParentInstanceID, "Property 'ParentInstanceID' was not reported properly.");
            Assert.AreEqual(ObjectState.New, aParentInstance.ObjectState, "Object state should remain 'New' when property set.");

            aParentInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aParentInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aParentInstance.ChildInstances.Remove(value);

            Assert.IsFalse(aParentInstance.ChildInstances.Contains(value), "Property 'ChildInstances' was not updated properly.");
            Assert.IsNull(value.ParentInstance, "Property 'ParentInstance' was not set properly.");
            Assert.IsNull(value.ParentInstanceID, "Property 'ParentInstanceID' was not reported properly.");
            Assert.AreEqual(ObjectState.Modified, aParentInstance.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void ChildInstanceRemoveFromChildInstancesWhenModified()
        {
            ModelChildInstance aParentInstance = new ModelChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aParentInstance.ObjectState, "Object state should be 'New' on creation.");

            aParentInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aParentInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            ModelChildInstance value = new ModelChildInstance() { InstanceID = rng.Next() };
            aParentInstance.ChildInstances.Add(value);
            value = new ModelChildInstance() { InstanceID = rng.Next() };
            aParentInstance.ChildInstances.Add(value);

            Assert.IsTrue(aParentInstance.ChildInstances.Contains(value), "Property 'ChildInstances' was not updated properly.");
            Assert.AreEqual(aParentInstance, value.ParentInstance, "Property 'ParentInstance' was not set properly.");
            Assert.AreEqual(aParentInstance.InstanceID, value.ParentInstanceID, "Property 'ParentInstanceID' was not reported properly.");
            Assert.AreEqual(ObjectState.Modified, aParentInstance.ObjectState, "Object state failed to transition to 'Modified'.");

            aParentInstance.ChildInstances.Remove(value);

            Assert.IsFalse(aParentInstance.ChildInstances.Contains(value), "Property 'ChildInstances' was not updated properly.");
            Assert.IsNull(value.ParentInstance, "Property 'ParentInstance' was not set properly.");
            Assert.IsNull(value.ParentInstanceID, "Property 'ParentInstanceID' was not reported properly.");
            Assert.AreEqual(ObjectState.Modified, aParentInstance.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ChildInstanceRemoveFromChildInstancesWhenDeleted()
        {
            ModelChildInstance aParentInstance = new ModelChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aParentInstance.ObjectState, "Object state should be 'New' on creation.");

            ModelChildInstance value = new ModelChildInstance() { InstanceID = rng.Next() };
            aParentInstance.ChildInstances.Add(value);

            Assert.IsTrue(aParentInstance.ChildInstances.Contains(value), "Property 'ChildInstances' was not updated properly.");
            Assert.AreEqual(aParentInstance, value.ParentInstance, "Property 'ParentInstance' was not set properly.");
            Assert.AreEqual(aParentInstance.InstanceID, value.ParentInstanceID, "Property 'ParentInstanceID' was not reported properly.");
            Assert.AreEqual(ObjectState.New, aParentInstance.ObjectState, "Object state should remain 'New' when property set.");

            aParentInstance.MarkUnmodified();
            value.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aParentInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");
            Assert.AreEqual(ObjectState.Unmodified, value.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aParentInstance.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aParentInstance.ObjectState, "Object state failed to transition to 'Deleted'.");
            Assert.AreEqual(ObjectState.Deleted, value.ObjectState, "Object state failed to transition to 'Deleted'.");

            aParentInstance.ChildInstances.Remove(value);
        }

        [TestMethod]
        public void ChildInstanceClearChildInstancesWhenNew()
        {
            ModelChildInstance aParentInstance = new ModelChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aParentInstance.ObjectState, "Object state should be 'New' on creation.");

            ModelChildInstance value = new ModelChildInstance() { InstanceID = rng.Next() };
            aParentInstance.ChildInstances.Add(value);
            value = new ModelChildInstance() { InstanceID = rng.Next() };
            aParentInstance.ChildInstances.Add(value);

            Assert.IsTrue(aParentInstance.ChildInstances.Contains(value), "Property 'ChildInstances' was not updated properly.");
            Assert.AreEqual(aParentInstance, value.ParentInstance, "Property 'ParentInstance' was not set properly.");
            Assert.AreEqual(aParentInstance.InstanceID, value.ParentInstanceID, "Property 'ParentInstanceID' was not reported properly.");
            Assert.AreEqual(ObjectState.New, aParentInstance.ObjectState, "Object state should remain 'New' when property set.");

            aParentInstance.ChildInstances.Clear();

            Assert.IsFalse(aParentInstance.ChildInstances.Contains(value), "Property 'ChildInstances' was not updated properly.");
            Assert.IsNull(value.ParentInstance, "Property 'ParentInstance' was not set properly.");
            Assert.IsNull(value.ParentInstanceID, "Property 'ParentInstanceID' was not reported properly.");
            Assert.IsFalse(aParentInstance.ChildInstances.Any(), "Property 'ChildInstances' was not updated properly.");
            Assert.AreEqual(ObjectState.New, aParentInstance.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ChildInstanceClearChildInstancesWhenUnmodified()
        {
            ModelChildInstance aParentInstance = new ModelChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aParentInstance.ObjectState, "Object state should be 'New' on creation.");

            ModelChildInstance value = new ModelChildInstance() { InstanceID = rng.Next() };
            aParentInstance.ChildInstances.Add(value);
            value = new ModelChildInstance() { InstanceID = rng.Next() };
            aParentInstance.ChildInstances.Add(value);

            Assert.IsTrue(aParentInstance.ChildInstances.Contains(value), "Property 'ChildInstances' was not updated properly.");
            Assert.AreEqual(aParentInstance, value.ParentInstance, "Property 'ParentInstance' was not set properly.");
            Assert.AreEqual(aParentInstance.InstanceID, value.ParentInstanceID, "Property 'ParentInstanceID' was not reported properly.");
            Assert.AreEqual(ObjectState.New, aParentInstance.ObjectState, "Object state should remain 'New' when property set.");

            aParentInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aParentInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aParentInstance.ChildInstances.Clear();

            Assert.IsFalse(aParentInstance.ChildInstances.Contains(value), "Property 'ChildInstances' was not updated properly.");
            Assert.IsNull(value.ParentInstance, "Property 'ParentInstance' was not set properly.");
            Assert.IsNull(value.ParentInstanceID, "Property 'ParentInstanceID' was not reported properly.");
            Assert.IsFalse(aParentInstance.ChildInstances.Any(), "Property 'ChildInstances' was not updated properly.");
            Assert.AreEqual(ObjectState.Modified, aParentInstance.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void ChildInstanceClearChildInstancesWhenModified()
        {
            ModelChildInstance aParentInstance = new ModelChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aParentInstance.ObjectState, "Object state should be 'New' on creation.");

            aParentInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aParentInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            ModelChildInstance value = new ModelChildInstance() { InstanceID = rng.Next() };
            aParentInstance.ChildInstances.Add(value);
            value = new ModelChildInstance() { InstanceID = rng.Next() };
            aParentInstance.ChildInstances.Add(value);

            Assert.IsTrue(aParentInstance.ChildInstances.Contains(value), "Property 'ChildInstances' was not updated properly.");
            Assert.AreEqual(aParentInstance, value.ParentInstance, "Property 'ParentInstance' was not set properly.");
            Assert.AreEqual(aParentInstance.InstanceID, value.ParentInstanceID, "Property 'ParentInstanceID' was not reported properly.");
            Assert.AreEqual(ObjectState.Modified, aParentInstance.ObjectState, "Object state failed to transition to 'Modified'.");

            aParentInstance.ChildInstances.Clear();

            Assert.IsFalse(aParentInstance.ChildInstances.Contains(value), "Property 'ChildInstances' was not updated properly.");
            Assert.IsNull(value.ParentInstance, "Property 'ParentInstance' was not set properly.");
            Assert.IsNull(value.ParentInstanceID, "Property 'ParentInstanceID' was not reported properly.");
            Assert.IsFalse(aParentInstance.ChildInstances.Any(), "Property 'ChildInstances' was not updated properly.");
            Assert.AreEqual(ObjectState.Modified, aParentInstance.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ChildInstanceClearChildInstancesWhenDeleted()
        {
            ModelChildInstance aParentInstance = new ModelChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aParentInstance.ObjectState, "Object state should be 'New' on creation.");

            ModelChildInstance value = new ModelChildInstance() { InstanceID = rng.Next() };
            aParentInstance.ChildInstances.Add(value);

            Assert.IsTrue(aParentInstance.ChildInstances.Contains(value), "Property 'ChildInstances' was not updated properly.");
            Assert.AreEqual(aParentInstance, value.ParentInstance, "Property 'ParentInstance' was not set properly.");
            Assert.AreEqual(aParentInstance.InstanceID, value.ParentInstanceID, "Property 'ParentInstanceID' was not reported properly.");
            Assert.AreEqual(ObjectState.New, aParentInstance.ObjectState, "Object state should remain 'New' when property set.");

            aParentInstance.MarkUnmodified();
            value.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aParentInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");
            Assert.AreEqual(ObjectState.Unmodified, value.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aParentInstance.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aParentInstance.ObjectState, "Object state failed to transition to 'Deleted'.");
            Assert.AreEqual(ObjectState.Deleted, value.ObjectState, "Object state failed to transition to 'Deleted'.");

            aParentInstance.ChildInstances.Clear();
        }
        #endregion

        #region Values
        [TestMethod]
        public void ChildInstanceAddToValuesWhenNew()
        {
            ModelChildInstance anInstance = new ModelChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anInstance.ObjectState, "Object state should be 'New' on creation.");

            ModelValue value = new ModelValue();
            anInstance.Values.Add(value);

            Assert.IsTrue(anInstance.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.AreEqual(anInstance, value.Instance, "Property 'Instance' was not set properly.");
            Assert.AreEqual(anInstance.InstanceID, value.InstanceID, "Property 'InstanceID' was not set properly.");
            Assert.AreEqual(ObjectState.New, anInstance.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ChildInstanceAddToValuesWhenUnmodified()
        {
            ModelChildInstance anInstance = new ModelChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anInstance.ObjectState, "Object state should be 'New' on creation.");

            anInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            ModelValue value = new ModelValue();
            anInstance.Values.Add(value);

            Assert.IsTrue(anInstance.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.AreEqual(anInstance, value.Instance, "Property 'Instance' was not set properly.");
            Assert.AreEqual(anInstance.InstanceID, value.InstanceID, "Property 'InstanceID' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, anInstance.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void ChildInstanceAddToValuesWhenModified()
        {
            ModelChildInstance anInstance = new ModelChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anInstance.ObjectState, "Object state should be 'New' on creation.");

            anInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            ModelValue value = new ModelValue();
            anInstance.Values.Add(value);

            Assert.IsTrue(anInstance.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.AreEqual(anInstance, value.Instance, "Property 'Instance' was not set properly.");
            Assert.AreEqual(anInstance.InstanceID, value.InstanceID, "Property 'InstanceID' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, anInstance.ObjectState, "Object state failed to transition to 'Modified'.");

            value = new ModelValue();
            anInstance.Values.Add(value);

            Assert.IsTrue(anInstance.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.AreEqual(anInstance, value.Instance, "Property 'Instance' was not set properly.");
            Assert.AreEqual(anInstance.InstanceID, value.InstanceID, "Property 'InstanceID' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, anInstance.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ChildInstanceAddToValuesWhenDeleted()
        {
            ModelChildInstance anInstance = new ModelChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anInstance.ObjectState, "Object state should be 'New' on creation.");

            anInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anInstance.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, anInstance.ObjectState, "Object state failed to transition to 'Deleted'.");

            anInstance.Values.Add(new ModelValue());
        }

        [TestMethod]
        public void ChildInstanceRemoveFromValuesWhenNew()
        {
            ModelChildInstance anInstance = new ModelChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anInstance.ObjectState, "Object state should be 'New' on creation.");

            ModelValue value = new ModelValue();
            anInstance.Values.Add(value);
            value = new ModelValue();
            anInstance.Values.Add(value);

            Assert.IsTrue(anInstance.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.AreEqual(anInstance, value.Instance, "Property 'Instance' was not set properly.");
            Assert.AreEqual(anInstance.InstanceID, value.InstanceID, "Property 'InstanceID' was not reported properly.");
            Assert.AreEqual(ObjectState.New, anInstance.ObjectState, "Object state should remain 'New' when property set.");

            anInstance.Values.Remove(value);

            Assert.IsFalse(anInstance.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.IsNull(value.Instance, "Property 'Instance' was not set properly.");
            Assert.IsNull(value.InstanceID, "Property 'InstanceID' was not reported properly.");
            Assert.AreEqual(ObjectState.New, anInstance.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ChildInstanceRemoveFromValuesWhenUnmodified()
        {
            ModelChildInstance anInstance = new ModelChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anInstance.ObjectState, "Object state should be 'New' on creation.");

            ModelValue value = new ModelValue();
            anInstance.Values.Add(value);
            value = new ModelValue();
            anInstance.Values.Add(value);

            Assert.IsTrue(anInstance.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.AreEqual(anInstance, value.Instance, "Property 'Instance' was not set properly.");
            Assert.AreEqual(anInstance.InstanceID, value.InstanceID, "Property 'InstanceID' was not reported properly.");
            Assert.AreEqual(ObjectState.New, anInstance.ObjectState, "Object state should remain 'New' when property set.");

            anInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anInstance.Values.Remove(value);

            Assert.IsFalse(anInstance.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.IsNull(value.Instance, "Property 'Instance' was not set properly.");
            Assert.IsNull(value.InstanceID, "Property 'InstanceID' was not reported properly.");
            Assert.AreEqual(ObjectState.Modified, anInstance.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void ChildInstanceRemoveFromValuesWhenModified()
        {
            ModelChildInstance anInstance = new ModelChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anInstance.ObjectState, "Object state should be 'New' on creation.");

            anInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            ModelValue value = new ModelValue();
            anInstance.Values.Add(value);
            value = new ModelValue();
            anInstance.Values.Add(value);

            Assert.IsTrue(anInstance.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.AreEqual(anInstance, value.Instance, "Property 'Instance' was not set properly.");
            Assert.AreEqual(anInstance.InstanceID, value.InstanceID, "Property 'InstanceID' was not reported properly.");
            Assert.AreEqual(ObjectState.Modified, anInstance.ObjectState, "Object state failed to transition to 'Modified'.");

            anInstance.Values.Remove(value);

            Assert.IsFalse(anInstance.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.IsNull(value.Instance, "Property 'Instance' was not set properly.");
            Assert.IsNull(value.InstanceID, "Property 'InstanceID' was not reported properly.");
            Assert.AreEqual(ObjectState.Modified, anInstance.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ChildInstanceRemoveFromValuesWhenDeleted()
        {
            ModelChildInstance anInstance = new ModelChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anInstance.ObjectState, "Object state should be 'New' on creation.");

            ModelValue value = new ModelValue() { Attribute = new ModelAttribute() { AttributeID = rng.Next() } };
            anInstance.Values.Add(value);

            Assert.IsTrue(anInstance.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.AreEqual(anInstance, value.Instance, "Property 'Instance' was not set properly.");
            Assert.AreEqual(anInstance.InstanceID, value.InstanceID, "Property 'InstanceID' was not reported properly.");
            Assert.AreEqual(ObjectState.New, anInstance.ObjectState, "Object state should remain 'New' when property set.");

            anInstance.MarkUnmodified();
            value.Attribute.MarkUnmodified();
            value.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");
            Assert.AreEqual(ObjectState.Unmodified, value.Attribute.ObjectState, "Object state failed to transition to 'Unmodified'.");
            Assert.AreEqual(ObjectState.Unmodified, value.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anInstance.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, anInstance.ObjectState, "Object state failed to transition to 'Deleted'.");
            Assert.AreEqual(ObjectState.Deleted, value.ObjectState, "Object state failed to transition to 'Deleted'.");

            anInstance.Values.Remove(value);
        }

        [TestMethod]
        public void ChildInstanceClearValuesWhenNew()
        {
            ModelChildInstance anInstance = new ModelChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anInstance.ObjectState, "Object state should be 'New' on creation.");

            ModelValue value = new ModelValue();
            anInstance.Values.Add(value);
            value = new ModelValue();
            anInstance.Values.Add(value);

            Assert.IsTrue(anInstance.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.AreEqual(anInstance, value.Instance, "Property 'Instance' was not set properly.");
            Assert.AreEqual(anInstance.InstanceID, value.InstanceID, "Property 'InstanceID' was not reported properly.");
            Assert.AreEqual(ObjectState.New, anInstance.ObjectState, "Object state should remain 'New' when property set.");

            anInstance.Values.Clear();

            Assert.IsFalse(anInstance.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.IsNull(value.Instance, "Property 'Instance' was not set properly.");
            Assert.IsNull(value.InstanceID, "Property 'InstanceID' was not reported properly.");
            Assert.IsFalse(anInstance.Values.Any(), "Property 'Values' was not updated properly.");
            Assert.AreEqual(ObjectState.New, anInstance.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ChildInstanceClearValuesWhenUnmodified()
        {
            ModelChildInstance anInstance = new ModelChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anInstance.ObjectState, "Object state should be 'New' on creation.");

            ModelValue value = new ModelValue();
            anInstance.Values.Add(value);
            value = new ModelValue();
            anInstance.Values.Add(value);

            Assert.IsTrue(anInstance.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.AreEqual(anInstance, value.Instance, "Property 'Instance' was not set properly.");
            Assert.AreEqual(anInstance.InstanceID, value.InstanceID, "Property 'InstanceID' was not reported properly.");
            Assert.AreEqual(ObjectState.New, anInstance.ObjectState, "Object state should remain 'New' when property set.");

            anInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anInstance.Values.Clear();

            Assert.IsFalse(anInstance.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.IsNull(value.Instance, "Property 'Instance' was not set properly.");
            Assert.IsNull(value.InstanceID, "Property 'InstanceID' was not reported properly.");
            Assert.IsFalse(anInstance.Values.Any(), "Property 'Values' was not updated properly.");
            Assert.AreEqual(ObjectState.Modified, anInstance.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void ChildInstanceClearValuesWhenModified()
        {
            ModelChildInstance anInstance = new ModelChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anInstance.ObjectState, "Object state should be 'New' on creation.");

            anInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            ModelValue value = new ModelValue();
            anInstance.Values.Add(value);
            value = new ModelValue();
            anInstance.Values.Add(value);

            Assert.IsTrue(anInstance.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.AreEqual(anInstance, value.Instance, "Property 'Instance' was not set properly.");
            Assert.AreEqual(anInstance.InstanceID, value.InstanceID, "Property 'InstanceID' was not reported properly.");
            Assert.AreEqual(ObjectState.Modified, anInstance.ObjectState, "Object state failed to transition to 'Modified'.");

            anInstance.Values.Clear();

            Assert.IsFalse(anInstance.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.IsNull(value.Instance, "Property 'Instance' was not set properly.");
            Assert.IsNull(value.InstanceID, "Property 'InstanceID' was not reported properly.");
            Assert.IsFalse(anInstance.Values.Any(), "Property 'Values' was not updated properly.");
            Assert.AreEqual(ObjectState.Modified, anInstance.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ChildInstanceClearValuesWhenDeleted()
        {
            ModelChildInstance anInstance = new ModelChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anInstance.ObjectState, "Object state should be 'New' on creation.");

            ModelValue value = new ModelValue() { Attribute = new ModelAttribute() { AttributeID = rng.Next() } };
            anInstance.Values.Add(value);

            Assert.IsTrue(anInstance.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.AreEqual(anInstance, value.Instance, "Property 'Instance' was not set properly.");
            Assert.AreEqual(anInstance.InstanceID, value.InstanceID, "Property 'InstanceID' was not reported properly.");
            Assert.AreEqual(ObjectState.New, anInstance.ObjectState, "Object state should remain 'New' when property set.");

            anInstance.MarkUnmodified();
            value.Attribute.MarkUnmodified();
            value.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");
            Assert.AreEqual(ObjectState.Unmodified, value.Attribute.ObjectState, "Object state failed to transition to 'Unmodified'.");
            Assert.AreEqual(ObjectState.Unmodified, value.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anInstance.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, anInstance.ObjectState, "Object state failed to transition to 'Deleted'.");
            Assert.AreEqual(ObjectState.Deleted, value.ObjectState, "Object state failed to transition to 'Deleted'.");

            anInstance.Values.Clear();
        }
        #endregion
        #endregion
    }
}
