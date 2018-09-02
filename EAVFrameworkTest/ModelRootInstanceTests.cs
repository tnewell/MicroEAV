using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace EAVModelTest
{
    public partial class EAVModelTestHarness
    {
        [TestMethod]
        public void RootInstanceCreate()
        {
            EAVModelLibrary.ModelRootInstance aRootInstance = new EAVModelLibrary.ModelRootInstance();

            Assert.AreEqual(EAV.Model.ObjectState.New, aRootInstance.ObjectState, "Object state should be 'New' on creation.");

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
            EAVModelLibrary.ModelRootInstance aRootInstance = new EAVModelLibrary.ModelRootInstance();

            Assert.AreEqual(EAV.Model.ObjectState.New, aRootInstance.ObjectState, "Object state should be 'New' on creation.");

            aRootInstance.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aRootInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        public void RootInstanceStateTransitionNewToUnmodifiedWithValidID()
        {
            EAVModelLibrary.ModelRootInstance aRootInstance = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aRootInstance.ObjectState, "Object state should be 'New' on creation.");

            aRootInstance.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aRootInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RootInstanceStateTransitionNewToDeleted()
        {
            EAVModelLibrary.ModelRootInstance aRootInstance = new EAVModelLibrary.ModelRootInstance();

            Assert.AreEqual(EAV.Model.ObjectState.New, aRootInstance.ObjectState, "Object state should be 'New' on creation.");

            aRootInstance.MarkDeleted();
        }

        [TestMethod]
        public void RootInstanceStateTransitionUnmodifiedToDeleted()
        {
            EAVModelLibrary.ModelRootInstance aRootInstance = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aRootInstance.ObjectState, "Object state should be 'New' on creation.");

            aRootInstance.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aRootInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aRootInstance.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, aRootInstance.ObjectState, "Object state failed to transition to 'Deleted'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RootInstanceStateTransitionDeletedToUnmodified()
        {
            EAVModelLibrary.ModelRootInstance aRootInstance = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aRootInstance.ObjectState, "Object state should be 'New' on creation.");

            aRootInstance.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aRootInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aRootInstance.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, aRootInstance.ObjectState, "Object state failed to transition to 'Deleted'.");

            aRootInstance.MarkUnmodified();
        }
        #endregion

        #region ID Property
        [TestMethod]
        public void RootInstanceSetIDWhenNew()
        {
            EAVModelLibrary.ModelRootInstance aRootInstance = new EAVModelLibrary.ModelRootInstance();

            Assert.AreEqual(EAV.Model.ObjectState.New, aRootInstance.ObjectState, "Object state should be 'New' on creation.");

            int id = rng.Next();
            aRootInstance.InstanceID = id;

            Assert.AreEqual(id, aRootInstance.InstanceID, "Property 'InstanceID' not properly set.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aRootInstance.ObjectState, "Object state after setting property 'InstanceID' should be 'New'.");
        }

        [TestMethod]
        public void RootInstanceSetIDBeforeUnmodified()
        {
            EAVModelLibrary.ModelRootInstance aRootInstance = new EAVModelLibrary.ModelRootInstance();

            Assert.AreEqual(EAV.Model.ObjectState.New, aRootInstance.ObjectState, "Object state should be 'New' on creation.");

            int id = rng.Next();
            aRootInstance.InstanceID = id;

            Assert.AreEqual(id, aRootInstance.InstanceID, "Property 'InstanceID' not properly set.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aRootInstance.ObjectState, "Object state after setting property 'InstanceID' should be 'New'.");

            aRootInstance.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aRootInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RootInstanceSetIDAfterUnmodified()
        {
            EAVModelLibrary.ModelRootInstance aRootInstance = new EAVModelLibrary.ModelRootInstance();

            Assert.AreEqual(EAV.Model.ObjectState.New, aRootInstance.ObjectState, "Object state should be 'New' on creation.");

            aRootInstance.MarkUnmodified();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RootInstanceSetIDWhenDeleted()
        {
            EAVModelLibrary.ModelRootInstance aRootInstance = new EAVModelLibrary.ModelRootInstance();

            Assert.AreEqual(EAV.Model.ObjectState.New, aRootInstance.ObjectState, "Object state should be 'New' on creation.");

            int id = rng.Next();
            aRootInstance.InstanceID = id;

            Assert.AreEqual(id, aRootInstance.InstanceID, "Property 'InstanceID' not properly set.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aRootInstance.ObjectState, "Object state after setting property 'InstanceID' should be 'New'.");

            aRootInstance.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aRootInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aRootInstance.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, aRootInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

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
            EAVModelLibrary.ModelRootInstance aRootInstance = new EAVModelLibrary.ModelRootInstance();

            Assert.AreEqual(EAV.Model.ObjectState.New, aRootInstance.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelRootContainer value = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };
            aRootInstance.Container = value;

            Assert.AreEqual(value, aRootInstance.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(value.ContainerID, aRootInstance.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.IsTrue(value.Instances.Contains(aRootInstance), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aRootInstance.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void RootInstanceSetContainerWhenUnmodified()
        {
            EAVModelLibrary.ModelRootInstance aRootInstance = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aRootInstance.ObjectState, "Object state should be 'New' on creation.");

            aRootInstance.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aRootInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVModelLibrary.ModelRootContainer value = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };
            aRootInstance.Container = value;

            Assert.AreEqual(value, aRootInstance.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(value.ContainerID, aRootInstance.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.IsTrue(value.Instances.Contains(aRootInstance), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aRootInstance.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void RootInstanceSetContainerWhenModified()
        {
            EAVModelLibrary.ModelRootInstance aRootInstance = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aRootInstance.ObjectState, "Object state should be 'New' on creation.");

            aRootInstance.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aRootInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVModelLibrary.ModelRootContainer value = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };
            aRootInstance.Container = value;

            Assert.AreEqual(value, aRootInstance.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(value.ContainerID, aRootInstance.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.IsTrue(value.Instances.Contains(aRootInstance), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aRootInstance.ObjectState, "Object state failed to transition to 'Modified'.");

            value = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };
            aRootInstance.Container = value;

            Assert.AreEqual(value, aRootInstance.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(value.ContainerID, aRootInstance.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.IsTrue(value.Instances.Contains(aRootInstance), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aRootInstance.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RootInstanceSetContainerWhenDeleted()
        {
            EAVModelLibrary.ModelRootInstance aRootInstance = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aRootInstance.ObjectState, "Object state should be 'New' on creation.");

            aRootInstance.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aRootInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aRootInstance.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, aRootInstance.ObjectState, "Object state failed to transition to 'Deleted'.");

            aRootInstance.Container = new EAVModelLibrary.ModelRootContainer() { ContainerID = rng.Next() };
        }
        #endregion

        #region Subject
        [TestMethod]
        public void RootInstanceSetSubjectWhenNew()
        {
            EAVModelLibrary.ModelRootInstance aRootInstance = new EAVModelLibrary.ModelRootInstance();

            Assert.AreEqual(EAV.Model.ObjectState.New, aRootInstance.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelSubject value = new EAVModelLibrary.ModelSubject() { SubjectID = rng.Next() };
            aRootInstance.Subject = value;

            Assert.AreEqual(value, aRootInstance.Subject, "Property 'Subject' was not set properly.");
            Assert.AreEqual(value.SubjectID, aRootInstance.SubjectID, "Property 'SubjectID' was not reported properly.");
            Assert.IsTrue(value.Instances.Contains(aRootInstance), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aRootInstance.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void RootInstanceSetSubjectWhenUnmodified()
        {
            EAVModelLibrary.ModelRootInstance aRootInstance = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aRootInstance.ObjectState, "Object state should be 'New' on creation.");

            aRootInstance.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aRootInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVModelLibrary.ModelSubject value = new EAVModelLibrary.ModelSubject() { SubjectID = rng.Next() };
            aRootInstance.Subject = value;

            Assert.AreEqual(value, aRootInstance.Subject, "Property 'Subject' was not set properly.");
            Assert.AreEqual(value.SubjectID, aRootInstance.SubjectID, "Property 'SubjectID' was not reported properly.");
            Assert.IsTrue(value.Instances.Contains(aRootInstance), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aRootInstance.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void RootInstanceSetSubjectWhenModified()
        {
            EAVModelLibrary.ModelRootInstance aRootInstance = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aRootInstance.ObjectState, "Object state should be 'New' on creation.");

            aRootInstance.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aRootInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVModelLibrary.ModelSubject value = new EAVModelLibrary.ModelSubject() { SubjectID = rng.Next() };
            aRootInstance.Subject = value;

            Assert.AreEqual(value, aRootInstance.Subject, "Property 'Subject' was not set properly.");
            Assert.AreEqual(value.SubjectID, aRootInstance.SubjectID, "Property 'SubjectID' was not reported properly.");
            Assert.IsTrue(value.Instances.Contains(aRootInstance), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aRootInstance.ObjectState, "Object state failed to transition to 'Modified'.");

            value = new EAVModelLibrary.ModelSubject() { SubjectID = rng.Next() };
            aRootInstance.Subject = value;

            Assert.AreEqual(value, aRootInstance.Subject, "Property 'Subject' was not set properly.");
            Assert.AreEqual(value.SubjectID, aRootInstance.SubjectID, "Property 'SubjectID' was not reported properly.");
            Assert.IsTrue(value.Instances.Contains(aRootInstance), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Modified, aRootInstance.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RootInstanceSetSubjectWhenDeleted()
        {
            EAVModelLibrary.ModelRootInstance aRootInstance = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aRootInstance.ObjectState, "Object state should be 'New' on creation.");

            aRootInstance.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aRootInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aRootInstance.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, aRootInstance.ObjectState, "Object state failed to transition to 'Deleted'.");

            aRootInstance.Subject = new EAVModelLibrary.ModelSubject() { SubjectID = rng.Next() };
        }
        #endregion

        #region ParentInstance
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RootInstanceSetParentInstanceWithObjectWhenNew()
        {
            EAVModelLibrary.ModelRootInstance aRootInstance = new EAVModelLibrary.ModelRootInstance();

            Assert.AreEqual(EAV.Model.ObjectState.New, aRootInstance.ObjectState, "Object state should be 'New' on creation.");

            aRootInstance.ParentInstance = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() };
        }

        [TestMethod]
        public void RootInstanceSetParentInstanceWithNullWhenNew()
        {
            EAVModelLibrary.ModelRootInstance aRootInstance = new EAVModelLibrary.ModelRootInstance();

            Assert.AreEqual(EAV.Model.ObjectState.New, aRootInstance.ObjectState, "Object state should be 'New' on creation.");

            aRootInstance.ParentInstance = null;

            Assert.IsNull(aRootInstance.ParentInstance, "Property 'ParentInstance' was not set properly.");
            Assert.IsNull(aRootInstance.ParentInstanceID, "Property 'ParentInstanceID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aRootInstance.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RootInstanceSetParentInstanceWithObjectWhenUnmodified()
        {
            EAVModelLibrary.ModelRootInstance aRootInstance = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aRootInstance.ObjectState, "Object state should be 'New' on creation.");

            aRootInstance.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aRootInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aRootInstance.ParentInstance = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() };
        }

        [TestMethod]
        public void RootInstanceSetParentInstanceWithNullWhenUnmodified()
        {
            EAVModelLibrary.ModelRootInstance aRootInstance = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aRootInstance.ObjectState, "Object state should be 'New' on creation.");

            aRootInstance.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aRootInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aRootInstance.ParentInstance = null;

            Assert.IsNull(aRootInstance.ParentInstance, "Property 'ParentInstance' was not set properly.");
            Assert.IsNull(aRootInstance.ParentInstanceID, "Property 'ParentInstanceID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aRootInstance.ObjectState, "Object state should remain 'Unmodified' when property set.");
        }

        // Set When Modified - N/A

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RootInstanceSetParentInstanceWithObjectWhenDeleted()
        {
            EAVModelLibrary.ModelRootInstance aRootInstance = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aRootInstance.ObjectState, "Object state should be 'New' on creation.");

            aRootInstance.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aRootInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aRootInstance.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, aRootInstance.ObjectState, "Object state failed to transition to 'Deleted'.");

            aRootInstance.ParentInstance = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() };
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RootInstanceSetParentInstanceWithNullWhenDeleted()
        {
            EAVModelLibrary.ModelRootInstance aRootInstance = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aRootInstance.ObjectState, "Object state should be 'New' on creation.");

            aRootInstance.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aRootInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aRootInstance.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, aRootInstance.ObjectState, "Object state failed to transition to 'Deleted'.");

            aRootInstance.ParentInstance = null;
        }
        #endregion
        #endregion

        #region Collection Properties
        #region ChildInstances
        [TestMethod]
        public void RootInstanceAddToChildInstancesWhenNew()
        {
            EAVModelLibrary.ModelRootInstance aParentInstance = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aParentInstance.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelChildInstance value = new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() };
            aParentInstance.ChildInstances.Add(value);

            Assert.IsTrue(aParentInstance.ChildInstances.Contains(value), "Property 'ChildInstances' was not updated properly.");
            Assert.AreEqual(aParentInstance, value.ParentInstance, "Property 'ParentInstance' was not set properly.");
            Assert.AreEqual(aParentInstance.InstanceID, value.ParentInstanceID, "Property 'ParentInstanceID' was not reported properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, aParentInstance.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void RootInstanceAddToChildInstancesWhenUnmodified()
        {
            EAVModelLibrary.ModelRootInstance aParentInstance = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() };

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
        public void RootInstanceAddToChildInstancesWhenModified()
        {
            EAVModelLibrary.ModelRootInstance aParentInstance = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() };

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
        public void RootInstanceAddToChildInstancesWhenDeleted()
        {
            EAVModelLibrary.ModelRootInstance aParentInstance = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, aParentInstance.ObjectState, "Object state should be 'New' on creation.");

            aParentInstance.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, aParentInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aParentInstance.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, aParentInstance.ObjectState, "Object state failed to transition to 'Deleted'.");

            aParentInstance.ChildInstances.Add(new EAVModelLibrary.ModelChildInstance() { InstanceID = rng.Next() });
        }

        [TestMethod]
        public void RootInstanceRemoveFromChildInstancesWhenNew()
        {
            EAVModelLibrary.ModelRootInstance aParentInstance = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() };

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
        public void RootInstanceRemoveFromChildInstancesWhenUnmodified()
        {
            EAVModelLibrary.ModelRootInstance aParentInstance = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() };

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
        public void RootInstanceRemoveFromChildInstancesWhenModified()
        {
            EAVModelLibrary.ModelRootInstance aParentInstance = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() };

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
        public void RootInstanceRemoveFromChildInstancesWhenDeleted()
        {
            EAVModelLibrary.ModelRootInstance aParentInstance = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() };

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
        public void RootInstanceClearChildInstancesWhenNew()
        {
            EAVModelLibrary.ModelRootInstance aParentInstance = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() };

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
        public void RootInstanceClearChildInstancesWhenUnmodified()
        {
            EAVModelLibrary.ModelRootInstance aParentInstance = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() };

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
        public void RootInstanceClearChildInstancesWhenModified()
        {
            EAVModelLibrary.ModelRootInstance aParentInstance = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() };

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
        public void RootInstanceClearChildInstancesWhenDeleted()
        {
            EAVModelLibrary.ModelRootInstance aParentInstance = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() };

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
        public void RootInstanceAddToValuesWhenNew()
        {
            EAVModelLibrary.ModelRootInstance anInstance = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, anInstance.ObjectState, "Object state should be 'New' on creation.");

            EAVModelLibrary.ModelValue value = new EAVModelLibrary.ModelValue();
            anInstance.Values.Add(value);

            Assert.IsTrue(anInstance.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.AreEqual(anInstance, value.Instance, "Property 'Instance' was not set properly.");
            Assert.AreEqual(anInstance.InstanceID, value.InstanceID, "Property 'InstanceID' was not set properly.");
            Assert.AreEqual(EAV.Model.ObjectState.New, anInstance.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void RootInstanceAddToValuesWhenUnmodified()
        {
            EAVModelLibrary.ModelRootInstance anInstance = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() };

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
        public void RootInstanceAddToValuesWhenModified()
        {
            EAVModelLibrary.ModelRootInstance anInstance = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() };

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
        public void RootInstanceAddToValuesWhenDeleted()
        {
            EAVModelLibrary.ModelRootInstance anInstance = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(EAV.Model.ObjectState.New, anInstance.ObjectState, "Object state should be 'New' on creation.");

            anInstance.MarkUnmodified();

            Assert.AreEqual(EAV.Model.ObjectState.Unmodified, anInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anInstance.MarkDeleted();

            Assert.AreEqual(EAV.Model.ObjectState.Deleted, anInstance.ObjectState, "Object state failed to transition to 'Deleted'.");

            anInstance.Values.Add(new EAVModelLibrary.ModelValue());
        }

        [TestMethod]
        public void RootInstanceRemoveFromValuesWhenNew()
        {
            EAVModelLibrary.ModelRootInstance anInstance = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() };

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
        public void RootInstanceRemoveFromValuesWhenUnmodified()
        {
            EAVModelLibrary.ModelRootInstance anInstance = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() };

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
        public void RootInstanceRemoveFromValuesWhenModified()
        {
            EAVModelLibrary.ModelRootInstance anInstance = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() };

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
        public void RootInstanceRemoveFromValuesWhenDeleted()
        {
            EAVModelLibrary.ModelRootInstance anInstance = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() };

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
        public void RootInstanceClearValuesWhenNew()
        {
            EAVModelLibrary.ModelRootInstance anInstance = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() };

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
        public void RootInstanceClearValuesWhenUnmodified()
        {
            EAVModelLibrary.ModelRootInstance anInstance = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() };

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
        public void RootInstanceClearValuesWhenModified()
        {
            EAVModelLibrary.ModelRootInstance anInstance = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() };

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
        public void RootInstanceClearValuesWhenDeleted()
        {
            EAVModelLibrary.ModelRootInstance anInstance = new EAVModelLibrary.ModelRootInstance() { InstanceID = rng.Next() };

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
