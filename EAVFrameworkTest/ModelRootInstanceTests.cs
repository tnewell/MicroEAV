using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using EAV.Model;


namespace EAVModelTest
{
    public partial class EAVModelTestHarness
    {
        [TestMethod]
        public void RootInstanceCreate()
        {
            ModelRootInstance aRootInstance = new ModelRootInstance();

            Assert.AreEqual(ObjectState.New, aRootInstance.ObjectState, "Object state should be 'New' on creation.");

            Assert.IsNull(aRootInstance.InstanceID, "Property 'InstanceID' should be null on creation.");

            Assert.IsNull(aRootInstance.Subject, "Property 'Subject' should be null on creation.");
            Assert.IsNull(aRootInstance.Container, "Property 'Container' should be null on creation.");
            Assert.IsNull(aRootInstance.ParentInstance, "Property 'ParentInstance' should be null on creation.");

            Assert.IsNotNull(aRootInstance.ChildInstances, "Property 'ChildInstances' should not be null on creation.");
            Assert.IsFalse(aRootInstance.ChildInstances.Any(), "Property 'ChildInstances' should be empty on creation.");

            Assert.IsNotNull(aRootInstance.Values, "Property 'Values' should not be null on creation.");
            Assert.IsFalse(aRootInstance.Values.Any(), "Property 'Values' should be empty on creation.");
        }

        #region State Transitions
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RootInstanceStateTransitionNewToUnmodifiedWithNullID()
        {
            ModelRootInstance aRootInstance = new ModelRootInstance();

            Assert.AreEqual(ObjectState.New, aRootInstance.ObjectState, "Object state should be 'New' on creation.");

            aRootInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aRootInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        public void RootInstanceStateTransitionNewToUnmodifiedWithValidID()
        {
            ModelRootInstance aRootInstance = new ModelRootInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aRootInstance.ObjectState, "Object state should be 'New' on creation.");

            aRootInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aRootInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RootInstanceStateTransitionNewToDeleted()
        {
            ModelRootInstance aRootInstance = new ModelRootInstance();

            Assert.AreEqual(ObjectState.New, aRootInstance.ObjectState, "Object state should be 'New' on creation.");

            aRootInstance.MarkDeleted();
        }

        [TestMethod]
        public void RootInstanceStateTransitionUnmodifiedToDeleted()
        {
            ModelRootInstance aRootInstance = new ModelRootInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aRootInstance.ObjectState, "Object state should be 'New' on creation.");

            aRootInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aRootInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aRootInstance.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aRootInstance.ObjectState, "Object state failed to transition to 'Deleted'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RootInstanceStateTransitionDeletedToUnmodified()
        {
            ModelRootInstance aRootInstance = new ModelRootInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aRootInstance.ObjectState, "Object state should be 'New' on creation.");

            aRootInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aRootInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aRootInstance.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aRootInstance.ObjectState, "Object state failed to transition to 'Deleted'.");

            aRootInstance.MarkUnmodified();
        }
        #endregion

        #region ID Property
        [TestMethod]
        public void RootInstanceSetIDWhenNew()
        {
            ModelRootInstance aRootInstance = new ModelRootInstance();

            Assert.AreEqual(ObjectState.New, aRootInstance.ObjectState, "Object state should be 'New' on creation.");

            int id = rng.Next();
            aRootInstance.InstanceID = id;

            Assert.AreEqual(id, aRootInstance.InstanceID, "Property 'InstanceID' not properly set.");
            Assert.AreEqual(ObjectState.New, aRootInstance.ObjectState, "Object state after setting property 'InstanceID' should be 'New'.");
        }

        [TestMethod]
        public void RootInstanceSetIDBeforeUnmodified()
        {
            ModelRootInstance aRootInstance = new ModelRootInstance();

            Assert.AreEqual(ObjectState.New, aRootInstance.ObjectState, "Object state should be 'New' on creation.");

            int id = rng.Next();
            aRootInstance.InstanceID = id;

            Assert.AreEqual(id, aRootInstance.InstanceID, "Property 'InstanceID' not properly set.");
            Assert.AreEqual(ObjectState.New, aRootInstance.ObjectState, "Object state after setting property 'InstanceID' should be 'New'.");

            aRootInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aRootInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RootInstanceSetIDAfterUnmodified()
        {
            ModelRootInstance aRootInstance = new ModelRootInstance();

            Assert.AreEqual(ObjectState.New, aRootInstance.ObjectState, "Object state should be 'New' on creation.");

            aRootInstance.MarkUnmodified();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RootInstanceSetIDWhenDeleted()
        {
            ModelRootInstance aRootInstance = new ModelRootInstance();

            Assert.AreEqual(ObjectState.New, aRootInstance.ObjectState, "Object state should be 'New' on creation.");

            int id = rng.Next();
            aRootInstance.InstanceID = id;

            Assert.AreEqual(id, aRootInstance.InstanceID, "Property 'InstanceID' not properly set.");
            Assert.AreEqual(ObjectState.New, aRootInstance.ObjectState, "Object state after setting property 'InstanceID' should be 'New'.");

            aRootInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aRootInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aRootInstance.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aRootInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            id = rng.Next();
            aRootInstance.InstanceID = id;
        }
        #endregion

        #region Primitive Properties
        // N/A
        #endregion

        #region Object Properties
        #region Container
        [TestMethod]
        public void RootInstanceSetContainerWhenNew()
        {
            ModelRootInstance aRootInstance = new ModelRootInstance();

            Assert.AreEqual(ObjectState.New, aRootInstance.ObjectState, "Object state should be 'New' on creation.");

            ModelRootContainer value = new ModelRootContainer() { ContainerID = rng.Next() };
            aRootInstance.Container = value;

            Assert.AreEqual(value, aRootInstance.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(value.ContainerID, aRootInstance.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.IsTrue(value.Instances.Contains(aRootInstance), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(ObjectState.New, aRootInstance.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void RootInstanceSetContainerWhenUnmodified()
        {
            ModelRootInstance aRootInstance = new ModelRootInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aRootInstance.ObjectState, "Object state should be 'New' on creation.");

            aRootInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aRootInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            ModelRootContainer value = new ModelRootContainer() { ContainerID = rng.Next() };
            aRootInstance.Container = value;

            Assert.AreEqual(value, aRootInstance.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(value.ContainerID, aRootInstance.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.IsTrue(value.Instances.Contains(aRootInstance), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(ObjectState.Modified, aRootInstance.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void RootInstanceSetContainerWhenModified()
        {
            ModelRootInstance aRootInstance = new ModelRootInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aRootInstance.ObjectState, "Object state should be 'New' on creation.");

            aRootInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aRootInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            ModelRootContainer value = new ModelRootContainer() { ContainerID = rng.Next() };
            aRootInstance.Container = value;

            Assert.AreEqual(value, aRootInstance.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(value.ContainerID, aRootInstance.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.IsTrue(value.Instances.Contains(aRootInstance), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(ObjectState.Modified, aRootInstance.ObjectState, "Object state failed to transition to 'Modified'.");

            value = new ModelRootContainer() { ContainerID = rng.Next() };
            aRootInstance.Container = value;

            Assert.AreEqual(value, aRootInstance.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(value.ContainerID, aRootInstance.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.IsTrue(value.Instances.Contains(aRootInstance), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(ObjectState.Modified, aRootInstance.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RootInstanceSetContainerWhenDeleted()
        {
            ModelRootInstance aRootInstance = new ModelRootInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aRootInstance.ObjectState, "Object state should be 'New' on creation.");

            aRootInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aRootInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aRootInstance.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aRootInstance.ObjectState, "Object state failed to transition to 'Deleted'.");

            aRootInstance.Container = new ModelRootContainer() { ContainerID = rng.Next() };
        }
        #endregion

        #region Subject
        [TestMethod]
        public void RootInstanceSetSubjectWhenNew()
        {
            ModelRootInstance aRootInstance = new ModelRootInstance();

            Assert.AreEqual(ObjectState.New, aRootInstance.ObjectState, "Object state should be 'New' on creation.");

            ModelSubject value = new ModelSubject() { SubjectID = rng.Next() };
            aRootInstance.Subject = value;

            Assert.AreEqual(value, aRootInstance.Subject, "Property 'Subject' was not set properly.");
            Assert.AreEqual(value.SubjectID, aRootInstance.SubjectID, "Property 'SubjectID' was not reported properly.");
            Assert.IsTrue(value.Instances.Contains(aRootInstance), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(ObjectState.New, aRootInstance.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void RootInstanceSetSubjectWhenUnmodified()
        {
            ModelRootInstance aRootInstance = new ModelRootInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aRootInstance.ObjectState, "Object state should be 'New' on creation.");

            aRootInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aRootInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            ModelSubject value = new ModelSubject() { SubjectID = rng.Next() };
            aRootInstance.Subject = value;

            Assert.AreEqual(value, aRootInstance.Subject, "Property 'Subject' was not set properly.");
            Assert.AreEqual(value.SubjectID, aRootInstance.SubjectID, "Property 'SubjectID' was not reported properly.");
            Assert.IsTrue(value.Instances.Contains(aRootInstance), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(ObjectState.Modified, aRootInstance.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void RootInstanceSetSubjectWhenModified()
        {
            ModelRootInstance aRootInstance = new ModelRootInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aRootInstance.ObjectState, "Object state should be 'New' on creation.");

            aRootInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aRootInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            ModelSubject value = new ModelSubject() { SubjectID = rng.Next() };
            aRootInstance.Subject = value;

            Assert.AreEqual(value, aRootInstance.Subject, "Property 'Subject' was not set properly.");
            Assert.AreEqual(value.SubjectID, aRootInstance.SubjectID, "Property 'SubjectID' was not reported properly.");
            Assert.IsTrue(value.Instances.Contains(aRootInstance), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(ObjectState.Modified, aRootInstance.ObjectState, "Object state failed to transition to 'Modified'.");

            value = new ModelSubject() { SubjectID = rng.Next() };
            aRootInstance.Subject = value;

            Assert.AreEqual(value, aRootInstance.Subject, "Property 'Subject' was not set properly.");
            Assert.AreEqual(value.SubjectID, aRootInstance.SubjectID, "Property 'SubjectID' was not reported properly.");
            Assert.IsTrue(value.Instances.Contains(aRootInstance), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(ObjectState.Modified, aRootInstance.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RootInstanceSetSubjectWhenDeleted()
        {
            ModelRootInstance aRootInstance = new ModelRootInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aRootInstance.ObjectState, "Object state should be 'New' on creation.");

            aRootInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aRootInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aRootInstance.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aRootInstance.ObjectState, "Object state failed to transition to 'Deleted'.");

            aRootInstance.Subject = new ModelSubject() { SubjectID = rng.Next() };
        }
        #endregion

        #region ParentInstance
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RootInstanceSetParentInstanceWithObjectWhenNew()
        {
            ModelRootInstance aRootInstance = new ModelRootInstance();

            Assert.AreEqual(ObjectState.New, aRootInstance.ObjectState, "Object state should be 'New' on creation.");

            aRootInstance.ParentInstance = new ModelRootInstance() { InstanceID = rng.Next() };
        }

        [TestMethod]
        public void RootInstanceSetParentInstanceWithNullWhenNew()
        {
            ModelRootInstance aRootInstance = new ModelRootInstance();

            Assert.AreEqual(ObjectState.New, aRootInstance.ObjectState, "Object state should be 'New' on creation.");

            aRootInstance.ParentInstance = null;

            Assert.IsNull(aRootInstance.ParentInstance, "Property 'ParentInstance' was not set properly.");
            Assert.IsNull(aRootInstance.ParentInstanceID, "Property 'ParentInstanceID' was not reported properly.");
            Assert.AreEqual(ObjectState.New, aRootInstance.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RootInstanceSetParentInstanceWithObjectWhenUnmodified()
        {
            ModelRootInstance aRootInstance = new ModelRootInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aRootInstance.ObjectState, "Object state should be 'New' on creation.");

            aRootInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aRootInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aRootInstance.ParentInstance = new ModelRootInstance() { InstanceID = rng.Next() };
        }

        [TestMethod]
        public void RootInstanceSetParentInstanceWithNullWhenUnmodified()
        {
            ModelRootInstance aRootInstance = new ModelRootInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aRootInstance.ObjectState, "Object state should be 'New' on creation.");

            aRootInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aRootInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aRootInstance.ParentInstance = null;

            Assert.IsNull(aRootInstance.ParentInstance, "Property 'ParentInstance' was not set properly.");
            Assert.IsNull(aRootInstance.ParentInstanceID, "Property 'ParentInstanceID' was not reported properly.");
            Assert.AreEqual(ObjectState.Unmodified, aRootInstance.ObjectState, "Object state should remain 'Unmodified' when property set.");
        }

        // Set When Modified - N/A

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RootInstanceSetParentInstanceWithObjectWhenDeleted()
        {
            ModelRootInstance aRootInstance = new ModelRootInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aRootInstance.ObjectState, "Object state should be 'New' on creation.");

            aRootInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aRootInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aRootInstance.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aRootInstance.ObjectState, "Object state failed to transition to 'Deleted'.");

            aRootInstance.ParentInstance = new ModelRootInstance() { InstanceID = rng.Next() };
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RootInstanceSetParentInstanceWithNullWhenDeleted()
        {
            ModelRootInstance aRootInstance = new ModelRootInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aRootInstance.ObjectState, "Object state should be 'New' on creation.");

            aRootInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aRootInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aRootInstance.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aRootInstance.ObjectState, "Object state failed to transition to 'Deleted'.");

            aRootInstance.ParentInstance = null;
        }
        #endregion
        #endregion

        #region Collection Properties
        #region ChildInstances
        [TestMethod]
        public void RootInstanceAddToChildInstancesWhenNew()
        {
            ModelRootInstance aParentInstance = new ModelRootInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aParentInstance.ObjectState, "Object state should be 'New' on creation.");

            ModelChildInstance value = new ModelChildInstance() { InstanceID = rng.Next() };
            aParentInstance.ChildInstances.Add(value);

            Assert.IsTrue(aParentInstance.ChildInstances.Contains(value), "Property 'ChildInstances' was not updated properly.");
            Assert.AreEqual(aParentInstance, value.ParentInstance, "Property 'ParentInstance' was not set properly.");
            Assert.AreEqual(aParentInstance.InstanceID, value.ParentInstanceID, "Property 'ParentInstanceID' was not reported properly.");
            Assert.AreEqual(ObjectState.New, aParentInstance.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void RootInstanceAddToChildInstancesWhenUnmodified()
        {
            ModelRootInstance aParentInstance = new ModelRootInstance() { InstanceID = rng.Next() };

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
        public void RootInstanceAddToChildInstancesWhenModified()
        {
            ModelRootInstance aParentInstance = new ModelRootInstance() { InstanceID = rng.Next() };

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
        public void RootInstanceAddToChildInstancesWhenDeleted()
        {
            ModelRootInstance aParentInstance = new ModelRootInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aParentInstance.ObjectState, "Object state should be 'New' on creation.");

            aParentInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aParentInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aParentInstance.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aParentInstance.ObjectState, "Object state failed to transition to 'Deleted'.");

            aParentInstance.ChildInstances.Add(new ModelChildInstance() { InstanceID = rng.Next() });
        }

        [TestMethod]
        public void RootInstanceRemoveFromChildInstancesWhenNew()
        {
            ModelRootInstance aParentInstance = new ModelRootInstance() { InstanceID = rng.Next() };

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
        public void RootInstanceRemoveFromChildInstancesWhenUnmodified()
        {
            ModelRootInstance aParentInstance = new ModelRootInstance() { InstanceID = rng.Next() };

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
        public void RootInstanceRemoveFromChildInstancesWhenModified()
        {
            ModelRootInstance aParentInstance = new ModelRootInstance() { InstanceID = rng.Next() };

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
        public void RootInstanceRemoveFromChildInstancesWhenDeleted()
        {
            ModelRootInstance aParentInstance = new ModelRootInstance() { InstanceID = rng.Next() };

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
        public void RootInstanceClearChildInstancesWhenNew()
        {
            ModelRootInstance aParentInstance = new ModelRootInstance() { InstanceID = rng.Next() };

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
        public void RootInstanceClearChildInstancesWhenUnmodified()
        {
            ModelRootInstance aParentInstance = new ModelRootInstance() { InstanceID = rng.Next() };

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
        public void RootInstanceClearChildInstancesWhenModified()
        {
            ModelRootInstance aParentInstance = new ModelRootInstance() { InstanceID = rng.Next() };

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
        public void RootInstanceClearChildInstancesWhenDeleted()
        {
            ModelRootInstance aParentInstance = new ModelRootInstance() { InstanceID = rng.Next() };

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
        public void RootInstanceAddToValuesWhenNew()
        {
            ModelRootInstance anInstance = new ModelRootInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anInstance.ObjectState, "Object state should be 'New' on creation.");

            ModelValue value = new ModelValue();
            anInstance.Values.Add(value);

            Assert.IsTrue(anInstance.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.AreEqual(anInstance, value.Instance, "Property 'Instance' was not set properly.");
            Assert.AreEqual(anInstance.InstanceID, value.InstanceID, "Property 'InstanceID' was not set properly.");
            Assert.AreEqual(ObjectState.New, anInstance.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void RootInstanceAddToValuesWhenUnmodified()
        {
            ModelRootInstance anInstance = new ModelRootInstance() { InstanceID = rng.Next() };

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
        public void RootInstanceAddToValuesWhenModified()
        {
            ModelRootInstance anInstance = new ModelRootInstance() { InstanceID = rng.Next() };

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
        public void RootInstanceAddToValuesWhenDeleted()
        {
            ModelRootInstance anInstance = new ModelRootInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anInstance.ObjectState, "Object state should be 'New' on creation.");

            anInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anInstance.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, anInstance.ObjectState, "Object state failed to transition to 'Deleted'.");

            anInstance.Values.Add(new ModelValue());
        }

        [TestMethod]
        public void RootInstanceRemoveFromValuesWhenNew()
        {
            ModelRootInstance anInstance = new ModelRootInstance() { InstanceID = rng.Next() };

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
        public void RootInstanceRemoveFromValuesWhenUnmodified()
        {
            ModelRootInstance anInstance = new ModelRootInstance() { InstanceID = rng.Next() };

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
        public void RootInstanceRemoveFromValuesWhenModified()
        {
            ModelRootInstance anInstance = new ModelRootInstance() { InstanceID = rng.Next() };

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
        public void RootInstanceRemoveFromValuesWhenDeleted()
        {
            ModelRootInstance anInstance = new ModelRootInstance() { InstanceID = rng.Next() };

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
        public void RootInstanceClearValuesWhenNew()
        {
            ModelRootInstance anInstance = new ModelRootInstance() { InstanceID = rng.Next() };

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
        public void RootInstanceClearValuesWhenUnmodified()
        {
            ModelRootInstance anInstance = new ModelRootInstance() { InstanceID = rng.Next() };

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
        public void RootInstanceClearValuesWhenModified()
        {
            ModelRootInstance anInstance = new ModelRootInstance() { InstanceID = rng.Next() };

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
        public void RootInstanceClearValuesWhenDeleted()
        {
            ModelRootInstance anInstance = new ModelRootInstance() { InstanceID = rng.Next() };

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
