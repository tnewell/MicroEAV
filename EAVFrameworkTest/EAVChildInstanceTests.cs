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
        public void ChildInstanceCreate()
        {
            EAVChildInstance aChildInstance = new EAVChildInstance();

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
            EAVChildInstance aChildInstance = new EAVChildInstance();

            Assert.AreEqual(ObjectState.New, aChildInstance.ObjectState, "Object state should be 'New' on creation.");

            aChildInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aChildInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        public void ChildInstanceStateTransitionNewToUnmodifiedWithValidID()
        {
            EAVChildInstance aChildInstance = new EAVChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aChildInstance.ObjectState, "Object state should be 'New' on creation.");

            aChildInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aChildInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChildInstanceStateTransitionNewToDeleted()
        {
            EAVChildInstance aChildInstance = new EAVChildInstance();

            Assert.AreEqual(ObjectState.New, aChildInstance.ObjectState, "Object state should be 'New' on creation.");

            aChildInstance.MarkDeleted();
        }

        [TestMethod]
        public void ChildInstanceStateTransitionUnmodifiedToDeleted()
        {
            EAVChildInstance aChildInstance = new EAVChildInstance() { InstanceID = rng.Next() };

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
            EAVChildInstance aChildInstance = new EAVChildInstance() { InstanceID = rng.Next() };

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
            EAVChildInstance aChildInstance = new EAVChildInstance();

            Assert.AreEqual(ObjectState.New, aChildInstance.ObjectState, "Object state should be 'New' on creation.");

            int id = rng.Next();
            aChildInstance.InstanceID = id;

            Assert.AreEqual(id, aChildInstance.InstanceID, "Property 'InstanceID' not properly set.");
            Assert.AreEqual(ObjectState.New, aChildInstance.ObjectState, "Object state after setting property 'InstanceID' should be 'New'.");
        }

        [TestMethod]
        public void ChildInstanceSetIDBeforeUnmodified()
        {
            EAVChildInstance aChildInstance = new EAVChildInstance();

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
            EAVChildInstance aChildInstance = new EAVChildInstance();

            Assert.AreEqual(ObjectState.New, aChildInstance.ObjectState, "Object state should be 'New' on creation.");

            aChildInstance.MarkUnmodified();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChildInstanceSetIDWhenDeleted()
        {
            EAVChildInstance aChildInstance = new EAVChildInstance();

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
            EAVChildInstance aChildInstance = new EAVChildInstance();

            Assert.AreEqual(ObjectState.New, aChildInstance.ObjectState, "Object state should be 'New' on creation.");

            EAVChildContainer value = new EAVChildContainer() { ContainerID = rng.Next() };
            aChildInstance.Container = value;

            Assert.AreEqual(value, aChildInstance.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(value.ContainerID, aChildInstance.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.AreEqual(ObjectState.New, aChildInstance.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ChildInstanceSetContainerWhenUnmodified()
        {
            EAVChildInstance aChildInstance = new EAVChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aChildInstance.ObjectState, "Object state should be 'New' on creation.");

            aChildInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aChildInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVChildContainer value = new EAVChildContainer() { ContainerID = rng.Next() };
            aChildInstance.Container = value;

            Assert.AreEqual(value, aChildInstance.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(value.ContainerID, aChildInstance.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.AreEqual(ObjectState.Modified, aChildInstance.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void ChildInstanceSetContainerWhenModified()
        {
            EAVChildInstance aChildInstance = new EAVChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aChildInstance.ObjectState, "Object state should be 'New' on creation.");

            aChildInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aChildInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVChildContainer value = new EAVChildContainer() { ContainerID = rng.Next() };
            aChildInstance.Container = value;

            Assert.AreEqual(value, aChildInstance.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(value.ContainerID, aChildInstance.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.AreEqual(ObjectState.Modified, aChildInstance.ObjectState, "Object state failed to transition to 'Modified'.");

            value = new EAVChildContainer() { ContainerID = rng.Next() };
            aChildInstance.Container = value;

            Assert.AreEqual(value, aChildInstance.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(value.ContainerID, aChildInstance.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.AreEqual(ObjectState.Modified, aChildInstance.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChildInstanceSetContainerWhenDeleted()
        {
            EAVChildInstance aChildInstance = new EAVChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aChildInstance.ObjectState, "Object state should be 'New' on creation.");

            aChildInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aChildInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aChildInstance.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aChildInstance.ObjectState, "Object state failed to transition to 'Deleted'.");

            aChildInstance.Container = new EAVChildContainer() { ContainerID = rng.Next() };
        }
        #endregion

        #region Subject
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChildInstanceSetSubjectWhenNew()
        {
            EAVChildInstance aChildInstance = new EAVChildInstance();

            Assert.AreEqual(ObjectState.New, aChildInstance.ObjectState, "Object state should be 'New' on creation.");

            aChildInstance.Subject = new EAVSubject() { SubjectID = rng.Next() };
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChildInstanceSetSubjectWhenUnmodified()
        {
            EAVChildInstance aChildInstance = new EAVChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aChildInstance.ObjectState, "Object state should be 'New' on creation.");

            aChildInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aChildInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aChildInstance.Subject = new EAVSubject() { SubjectID = rng.Next() };
        }

        // Set When Modified - N/A

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChildInstanceSetSubjectWhenDeleted()
        {
            EAVChildInstance aChildInstance = new EAVChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aChildInstance.ObjectState, "Object state should be 'New' on creation.");

            aChildInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aChildInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aChildInstance.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aChildInstance.ObjectState, "Object state failed to transition to 'Deleted'.");

            aChildInstance.Subject = new EAVSubject() { SubjectID = rng.Next() };
        }
        #endregion

        #region ParentInstance
        [TestMethod]
        public void ChildInstanceSetParentInstanceWhenNew()
        {
            EAVChildInstance aChildInstance = new EAVChildInstance();

            Assert.AreEqual(ObjectState.New, aChildInstance.ObjectState, "Object state should be 'New' on creation.");

            EAVChildInstance value = new EAVChildInstance() { InstanceID = rng.Next() };
            aChildInstance.ParentInstance = value;

            Assert.AreEqual(value, aChildInstance.ParentInstance, "Property 'ParentInstance' was not set properly.");
            Assert.AreEqual(value.InstanceID, aChildInstance.ParentInstanceID, "Property 'ParentInstanceID' was not reported properly.");
            Assert.AreEqual(ObjectState.New, aChildInstance.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ChildInstanceSetParentInstanceWhenUnmodified()
        {
            EAVChildInstance aChildInstance = new EAVChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aChildInstance.ObjectState, "Object state should be 'New' on creation.");

            aChildInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aChildInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVChildInstance value = new EAVChildInstance() { InstanceID = rng.Next() };
            aChildInstance.ParentInstance = value;

            Assert.AreEqual(value, aChildInstance.ParentInstance, "Property 'ParentInstance' was not set properly.");
            Assert.AreEqual(value.InstanceID, aChildInstance.ParentInstanceID, "Property 'ParentInstanceID' was not reported properly.");
            Assert.AreEqual(ObjectState.Modified, aChildInstance.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void ChildInstanceSetParentInstanceWhenModified()
        {
            EAVChildInstance aChildInstance = new EAVChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aChildInstance.ObjectState, "Object state should be 'New' on creation.");

            aChildInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aChildInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVChildInstance value = new EAVChildInstance() { InstanceID = rng.Next() };
            aChildInstance.ParentInstance = value;

            Assert.AreEqual(value, aChildInstance.ParentInstance, "Property 'ParentInstance' was not set properly.");
            Assert.AreEqual(value.InstanceID, aChildInstance.ParentInstanceID, "Property 'ParentInstanceID' was not reported properly.");
            Assert.AreEqual(ObjectState.Modified, aChildInstance.ObjectState, "Object state failed to transition to 'Modified'.");

            value = new EAVChildInstance() { InstanceID = rng.Next() };
            aChildInstance.ParentInstance = value;

            Assert.AreEqual(value, aChildInstance.ParentInstance, "Property 'ParentInstance' was not set properly.");
            Assert.AreEqual(value.InstanceID, aChildInstance.ParentInstanceID, "Property 'ParentInstanceID' was not reported properly.");
            Assert.AreEqual(ObjectState.Modified, aChildInstance.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChildInstanceSetParentInstanceWhenDeleted()
        {
            EAVChildInstance aChildInstance = new EAVChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aChildInstance.ObjectState, "Object state should be 'New' on creation.");

            aChildInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aChildInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aChildInstance.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aChildInstance.ObjectState, "Object state failed to transition to 'Deleted'.");

            aChildInstance.ParentInstance = new EAVChildInstance() { InstanceID = rng.Next() };
        }
        #endregion
        #endregion

        #region Collection Properties
        #region ChildInstances
        [TestMethod]
        public void ChildInstanceAddToChildInstancesWhenNew()
        {
            EAVChildInstance aParentInstance = new EAVChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aParentInstance.ObjectState, "Object state should be 'New' on creation.");

            EAVChildInstance value = new EAVChildInstance() { InstanceID = rng.Next() };
            aParentInstance.ChildInstances.Add(value);

            Assert.IsTrue(aParentInstance.ChildInstances.Contains(value), "Property 'ChildInstances' was not updated properly.");
            Assert.AreEqual(aParentInstance, value.ParentInstance, "Property 'ParentInstance' was not set properly.");
            Assert.AreEqual(aParentInstance.InstanceID, value.ParentInstanceID, "Property 'ParentInstanceID' was not reported properly.");
            Assert.AreEqual(ObjectState.New, aParentInstance.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ChildInstanceAddToChildInstancesWhenUnmodified()
        {
            EAVChildInstance aParentInstance = new EAVChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aParentInstance.ObjectState, "Object state should be 'New' on creation.");

            aParentInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aParentInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVChildInstance value = new EAVChildInstance() { InstanceID = rng.Next() };
            aParentInstance.ChildInstances.Add(value);

            Assert.IsTrue(aParentInstance.ChildInstances.Contains(value), "Property 'ChildInstances' was not updated properly.");
            Assert.AreEqual(aParentInstance, value.ParentInstance, "Property 'ParentInstance' was not set properly.");
            Assert.AreEqual(aParentInstance.InstanceID, value.ParentInstanceID, "Property 'ParentInstanceID' was not reported properly.");
            Assert.AreEqual(ObjectState.Modified, aParentInstance.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void ChildInstanceAddToChildInstancesWhenModified()
        {
            EAVChildInstance aParentInstance = new EAVChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aParentInstance.ObjectState, "Object state should be 'New' on creation.");

            aParentInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aParentInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVChildInstance value = new EAVChildInstance() { InstanceID = rng.Next() };
            aParentInstance.ChildInstances.Add(value);

            Assert.IsTrue(aParentInstance.ChildInstances.Contains(value), "Property 'ChildInstances' was not updated properly.");
            Assert.AreEqual(aParentInstance, value.ParentInstance, "Property 'ParentInstance' was not set properly.");
            Assert.AreEqual(aParentInstance.InstanceID, value.ParentInstanceID, "Property 'ParentInstanceID' was not reported properly.");
            Assert.AreEqual(ObjectState.Modified, aParentInstance.ObjectState, "Object state failed to transition to 'Modified'.");

            value = new EAVChildInstance() { InstanceID = rng.Next() };
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
            EAVChildInstance aParentInstance = new EAVChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aParentInstance.ObjectState, "Object state should be 'New' on creation.");

            aParentInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aParentInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aParentInstance.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aParentInstance.ObjectState, "Object state failed to transition to 'Deleted'.");

            aParentInstance.ChildInstances.Add(new EAVChildInstance() { InstanceID = rng.Next() });
        }

        [TestMethod]
        public void ChildInstanceRemoveFromChildInstancesWhenNew()
        {
            EAVChildInstance aParentInstance = new EAVChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aParentInstance.ObjectState, "Object state should be 'New' on creation.");

            EAVChildInstance value = new EAVChildInstance() { InstanceID = rng.Next() };
            aParentInstance.ChildInstances.Add(value);
            value = new EAVChildInstance() { InstanceID = rng.Next() };
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
            EAVChildInstance aParentInstance = new EAVChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aParentInstance.ObjectState, "Object state should be 'New' on creation.");

            EAVChildInstance value = new EAVChildInstance() { InstanceID = rng.Next() };
            aParentInstance.ChildInstances.Add(value);
            value = new EAVChildInstance() { InstanceID = rng.Next() };
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
            EAVChildInstance aParentInstance = new EAVChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aParentInstance.ObjectState, "Object state should be 'New' on creation.");

            aParentInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aParentInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVChildInstance value = new EAVChildInstance() { InstanceID = rng.Next() };
            aParentInstance.ChildInstances.Add(value);
            value = new EAVChildInstance() { InstanceID = rng.Next() };
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
            EAVChildInstance aParentInstance = new EAVChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aParentInstance.ObjectState, "Object state should be 'New' on creation.");

            EAVChildInstance value = new EAVChildInstance() { InstanceID = rng.Next() };
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
            EAVChildInstance aParentInstance = new EAVChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aParentInstance.ObjectState, "Object state should be 'New' on creation.");

            EAVChildInstance value = new EAVChildInstance() { InstanceID = rng.Next() };
            aParentInstance.ChildInstances.Add(value);
            value = new EAVChildInstance() { InstanceID = rng.Next() };
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
            EAVChildInstance aParentInstance = new EAVChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aParentInstance.ObjectState, "Object state should be 'New' on creation.");

            EAVChildInstance value = new EAVChildInstance() { InstanceID = rng.Next() };
            aParentInstance.ChildInstances.Add(value);
            value = new EAVChildInstance() { InstanceID = rng.Next() };
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
            EAVChildInstance aParentInstance = new EAVChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aParentInstance.ObjectState, "Object state should be 'New' on creation.");

            aParentInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aParentInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVChildInstance value = new EAVChildInstance() { InstanceID = rng.Next() };
            aParentInstance.ChildInstances.Add(value);
            value = new EAVChildInstance() { InstanceID = rng.Next() };
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
            EAVChildInstance aParentInstance = new EAVChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aParentInstance.ObjectState, "Object state should be 'New' on creation.");

            EAVChildInstance value = new EAVChildInstance() { InstanceID = rng.Next() };
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
            EAVChildInstance anInstance = new EAVChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anInstance.ObjectState, "Object state should be 'New' on creation.");

            EAVValue value = new EAVValue();
            anInstance.Values.Add(value);

            Assert.IsTrue(anInstance.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.AreEqual(anInstance, value.Instance, "Property 'Instance' was not set properly.");
            Assert.AreEqual(anInstance.InstanceID, value.InstanceID, "Property 'InstanceID' was not set properly.");
            Assert.AreEqual(ObjectState.New, anInstance.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ChildInstanceAddToValuesWhenUnmodified()
        {
            EAVChildInstance anInstance = new EAVChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anInstance.ObjectState, "Object state should be 'New' on creation.");

            anInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVValue value = new EAVValue();
            anInstance.Values.Add(value);

            Assert.IsTrue(anInstance.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.AreEqual(anInstance, value.Instance, "Property 'Instance' was not set properly.");
            Assert.AreEqual(anInstance.InstanceID, value.InstanceID, "Property 'InstanceID' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, anInstance.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void ChildInstanceAddToValuesWhenModified()
        {
            EAVChildInstance anInstance = new EAVChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anInstance.ObjectState, "Object state should be 'New' on creation.");

            anInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVValue value = new EAVValue();
            anInstance.Values.Add(value);

            Assert.IsTrue(anInstance.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.AreEqual(anInstance, value.Instance, "Property 'Instance' was not set properly.");
            Assert.AreEqual(anInstance.InstanceID, value.InstanceID, "Property 'InstanceID' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, anInstance.ObjectState, "Object state failed to transition to 'Modified'.");

            value = new EAVValue();
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
            EAVChildInstance anInstance = new EAVChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anInstance.ObjectState, "Object state should be 'New' on creation.");

            anInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anInstance.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, anInstance.ObjectState, "Object state failed to transition to 'Deleted'.");

            anInstance.Values.Add(new EAVValue());
        }

        [TestMethod]
        public void ChildInstanceRemoveFromValuesWhenNew()
        {
            EAVChildInstance anInstance = new EAVChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anInstance.ObjectState, "Object state should be 'New' on creation.");

            EAVValue value = new EAVValue();
            anInstance.Values.Add(value);
            value = new EAVValue();
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
            EAVChildInstance anInstance = new EAVChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anInstance.ObjectState, "Object state should be 'New' on creation.");

            EAVValue value = new EAVValue();
            anInstance.Values.Add(value);
            value = new EAVValue();
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
            EAVChildInstance anInstance = new EAVChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anInstance.ObjectState, "Object state should be 'New' on creation.");

            anInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVValue value = new EAVValue();
            anInstance.Values.Add(value);
            value = new EAVValue();
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
            EAVChildInstance anInstance = new EAVChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anInstance.ObjectState, "Object state should be 'New' on creation.");

            EAVValue value = new EAVValue() { Attribute = new EAVAttribute() { AttributeID = rng.Next() } };
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
            EAVChildInstance anInstance = new EAVChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anInstance.ObjectState, "Object state should be 'New' on creation.");

            EAVValue value = new EAVValue();
            anInstance.Values.Add(value);
            value = new EAVValue();
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
            EAVChildInstance anInstance = new EAVChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anInstance.ObjectState, "Object state should be 'New' on creation.");

            EAVValue value = new EAVValue();
            anInstance.Values.Add(value);
            value = new EAVValue();
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
            EAVChildInstance anInstance = new EAVChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anInstance.ObjectState, "Object state should be 'New' on creation.");

            anInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVValue value = new EAVValue();
            anInstance.Values.Add(value);
            value = new EAVValue();
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
            EAVChildInstance anInstance = new EAVChildInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anInstance.ObjectState, "Object state should be 'New' on creation.");

            EAVValue value = new EAVValue() { Attribute = new EAVAttribute() { AttributeID = rng.Next() } };
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
