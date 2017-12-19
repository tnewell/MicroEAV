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
        public void RootInstanceCreate()
        {
            EAVRootInstance aRootInstance = new EAVRootInstance();

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
            EAVRootInstance aRootInstance = new EAVRootInstance();

            Assert.AreEqual(ObjectState.New, aRootInstance.ObjectState, "Object state should be 'New' on creation.");

            aRootInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aRootInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        public void RootInstanceStateTransitionNewToUnmodifiedWithValidID()
        {
            EAVRootInstance aRootInstance = new EAVRootInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aRootInstance.ObjectState, "Object state should be 'New' on creation.");

            aRootInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aRootInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RootInstanceStateTransitionNewToDeleted()
        {
            EAVRootInstance aRootInstance = new EAVRootInstance();

            Assert.AreEqual(ObjectState.New, aRootInstance.ObjectState, "Object state should be 'New' on creation.");

            aRootInstance.MarkDeleted();
        }

        [TestMethod]
        public void RootInstanceStateTransitionUnmodifiedToDeleted()
        {
            EAVRootInstance aRootInstance = new EAVRootInstance() { InstanceID = rng.Next() };

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
            EAVRootInstance aRootInstance = new EAVRootInstance() { InstanceID = rng.Next() };

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
            EAVRootInstance aRootInstance = new EAVRootInstance();

            Assert.AreEqual(ObjectState.New, aRootInstance.ObjectState, "Object state should be 'New' on creation.");

            int id = rng.Next();
            aRootInstance.InstanceID = id;

            Assert.AreEqual(id, aRootInstance.InstanceID, "Property 'InstanceID' not properly set.");
            Assert.AreEqual(ObjectState.New, aRootInstance.ObjectState, "Object state after setting property 'InstanceID' should be 'New'.");
        }

        [TestMethod]
        public void RootInstanceSetIDBeforeUnmodified()
        {
            EAVRootInstance aRootInstance = new EAVRootInstance();

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
            EAVRootInstance aRootInstance = new EAVRootInstance();

            Assert.AreEqual(ObjectState.New, aRootInstance.ObjectState, "Object state should be 'New' on creation.");

            aRootInstance.MarkUnmodified();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RootInstanceSetIDWhenDeleted()
        {
            EAVRootInstance aRootInstance = new EAVRootInstance();

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
            EAVRootInstance aRootInstance = new EAVRootInstance();

            Assert.AreEqual(ObjectState.New, aRootInstance.ObjectState, "Object state should be 'New' on creation.");

            EAVRootContainer value = new EAVRootContainer() { ContainerID = rng.Next() };
            aRootInstance.Container = value;

            Assert.AreEqual(value, aRootInstance.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(value.ContainerID, aRootInstance.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.IsTrue(value.Instances.Contains(aRootInstance), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(ObjectState.New, aRootInstance.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void RootInstanceSetContainerWhenUnmodified()
        {
            EAVRootInstance aRootInstance = new EAVRootInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aRootInstance.ObjectState, "Object state should be 'New' on creation.");

            aRootInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aRootInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVRootContainer value = new EAVRootContainer() { ContainerID = rng.Next() };
            aRootInstance.Container = value;

            Assert.AreEqual(value, aRootInstance.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(value.ContainerID, aRootInstance.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.IsTrue(value.Instances.Contains(aRootInstance), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(ObjectState.Modified, aRootInstance.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void RootInstanceSetContainerWhenModified()
        {
            EAVRootInstance aRootInstance = new EAVRootInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aRootInstance.ObjectState, "Object state should be 'New' on creation.");

            aRootInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aRootInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVRootContainer value = new EAVRootContainer() { ContainerID = rng.Next() };
            aRootInstance.Container = value;

            Assert.AreEqual(value, aRootInstance.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(value.ContainerID, aRootInstance.ContainerID, "Property 'ContainerID' was not reported properly.");
            Assert.IsTrue(value.Instances.Contains(aRootInstance), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(ObjectState.Modified, aRootInstance.ObjectState, "Object state failed to transition to 'Modified'.");

            value = new EAVRootContainer() { ContainerID = rng.Next() };
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
            EAVRootInstance aRootInstance = new EAVRootInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aRootInstance.ObjectState, "Object state should be 'New' on creation.");

            aRootInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aRootInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aRootInstance.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aRootInstance.ObjectState, "Object state failed to transition to 'Deleted'.");

            aRootInstance.Container = new EAVRootContainer() { ContainerID = rng.Next() };
        }
        #endregion

        #region Subject
        [TestMethod]
        public void RootInstanceSetSubjectWhenNew()
        {
            EAVRootInstance aRootInstance = new EAVRootInstance();

            Assert.AreEqual(ObjectState.New, aRootInstance.ObjectState, "Object state should be 'New' on creation.");

            EAVSubject value = new EAVSubject() { SubjectID = rng.Next() };
            aRootInstance.Subject = value;

            Assert.AreEqual(value, aRootInstance.Subject, "Property 'Subject' was not set properly.");
            Assert.AreEqual(value.SubjectID, aRootInstance.SubjectID, "Property 'SubjectID' was not reported properly.");
            Assert.IsTrue(value.Instances.Contains(aRootInstance), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(ObjectState.New, aRootInstance.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void RootInstanceSetSubjectWhenUnmodified()
        {
            EAVRootInstance aRootInstance = new EAVRootInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aRootInstance.ObjectState, "Object state should be 'New' on creation.");

            aRootInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aRootInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVSubject value = new EAVSubject() { SubjectID = rng.Next() };
            aRootInstance.Subject = value;

            Assert.AreEqual(value, aRootInstance.Subject, "Property 'Subject' was not set properly.");
            Assert.AreEqual(value.SubjectID, aRootInstance.SubjectID, "Property 'SubjectID' was not reported properly.");
            Assert.IsTrue(value.Instances.Contains(aRootInstance), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(ObjectState.Modified, aRootInstance.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void RootInstanceSetSubjectWhenModified()
        {
            EAVRootInstance aRootInstance = new EAVRootInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aRootInstance.ObjectState, "Object state should be 'New' on creation.");

            aRootInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aRootInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVSubject value = new EAVSubject() { SubjectID = rng.Next() };
            aRootInstance.Subject = value;

            Assert.AreEqual(value, aRootInstance.Subject, "Property 'Subject' was not set properly.");
            Assert.AreEqual(value.SubjectID, aRootInstance.SubjectID, "Property 'SubjectID' was not reported properly.");
            Assert.IsTrue(value.Instances.Contains(aRootInstance), "Property 'Instances' was not updated properly.");
            Assert.AreEqual(ObjectState.Modified, aRootInstance.ObjectState, "Object state failed to transition to 'Modified'.");

            value = new EAVSubject() { SubjectID = rng.Next() };
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
            EAVRootInstance aRootInstance = new EAVRootInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aRootInstance.ObjectState, "Object state should be 'New' on creation.");

            aRootInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aRootInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aRootInstance.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aRootInstance.ObjectState, "Object state failed to transition to 'Deleted'.");

            aRootInstance.Subject = new EAVSubject() { SubjectID = rng.Next() };
        }
        #endregion

        #region ParentInstance
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RootInstanceSetParentInstanceWithObjectWhenNew()
        {
            EAVRootInstance aRootInstance = new EAVRootInstance();

            Assert.AreEqual(ObjectState.New, aRootInstance.ObjectState, "Object state should be 'New' on creation.");

            aRootInstance.ParentInstance = new EAVRootInstance() { InstanceID = rng.Next() };
        }

        [TestMethod]
        public void RootInstanceSetParentInstanceWithNullWhenNew()
        {
            EAVRootInstance aRootInstance = new EAVRootInstance();

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
            EAVRootInstance aRootInstance = new EAVRootInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aRootInstance.ObjectState, "Object state should be 'New' on creation.");

            aRootInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aRootInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aRootInstance.ParentInstance = new EAVRootInstance() { InstanceID = rng.Next() };
        }

        [TestMethod]
        public void RootInstanceSetParentInstanceWithNullWhenUnmodified()
        {
            EAVRootInstance aRootInstance = new EAVRootInstance() { InstanceID = rng.Next() };

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
            EAVRootInstance aRootInstance = new EAVRootInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aRootInstance.ObjectState, "Object state should be 'New' on creation.");

            aRootInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aRootInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aRootInstance.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aRootInstance.ObjectState, "Object state failed to transition to 'Deleted'.");

            aRootInstance.ParentInstance = new EAVRootInstance() { InstanceID = rng.Next() };
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void RootInstanceSetParentInstanceWithNullWhenDeleted()
        {
            EAVRootInstance aRootInstance = new EAVRootInstance() { InstanceID = rng.Next() };

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
            EAVRootInstance aParentInstance = new EAVRootInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aParentInstance.ObjectState, "Object state should be 'New' on creation.");

            EAVChildInstance value = new EAVChildInstance() { InstanceID = rng.Next() };
            aParentInstance.ChildInstances.Add(value);

            Assert.IsTrue(aParentInstance.ChildInstances.Contains(value), "Property 'ChildInstances' was not updated properly.");
            Assert.AreEqual(aParentInstance, value.ParentInstance, "Property 'ParentInstance' was not set properly.");
            Assert.AreEqual(aParentInstance.InstanceID, value.ParentInstanceID, "Property 'ParentInstanceID' was not reported properly.");
            Assert.AreEqual(ObjectState.New, aParentInstance.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void RootInstanceAddToChildInstancesWhenUnmodified()
        {
            EAVRootInstance aParentInstance = new EAVRootInstance() { InstanceID = rng.Next() };

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
        public void RootInstanceAddToChildInstancesWhenModified()
        {
            EAVRootInstance aParentInstance = new EAVRootInstance() { InstanceID = rng.Next() };

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
        public void RootInstanceAddToChildInstancesWhenDeleted()
        {
            EAVRootInstance aParentInstance = new EAVRootInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aParentInstance.ObjectState, "Object state should be 'New' on creation.");

            aParentInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aParentInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aParentInstance.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aParentInstance.ObjectState, "Object state failed to transition to 'Deleted'.");

            aParentInstance.ChildInstances.Add(new EAVChildInstance() { InstanceID = rng.Next() });
        }
        #endregion

        #region Values
        [TestMethod]
        public void RootInstanceAddToValuesWhenNew()
        {
            EAVRootInstance anInstance = new EAVRootInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anInstance.ObjectState, "Object state should be 'New' on creation.");

            EAVValue value = new EAVValue();
            anInstance.Values.Add(value);

            Assert.IsTrue(anInstance.Values.Contains(value), "Property 'Values' was not updated properly.");
            Assert.AreEqual(anInstance, value.Instance, "Property 'Instance' was not set properly.");
            Assert.AreEqual(anInstance.InstanceID, value.InstanceID, "Property 'InstanceID' was not set properly.");
            Assert.AreEqual(ObjectState.New, anInstance.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void RootInstanceAddToValuesWhenUnmodified()
        {
            EAVRootInstance anInstance = new EAVRootInstance() { InstanceID = rng.Next() };

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
        public void RootInstanceAddToValuesWhenModified()
        {
            EAVRootInstance anInstance = new EAVRootInstance() { InstanceID = rng.Next() };

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
        public void RootInstanceAddToValuesWhenDeleted()
        {
            EAVRootInstance anInstance = new EAVRootInstance() { InstanceID = rng.Next() };

            Assert.AreEqual(ObjectState.New, anInstance.ObjectState, "Object state should be 'New' on creation.");

            anInstance.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, anInstance.ObjectState, "Object state failed to transition to 'Unmodified'.");

            anInstance.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, anInstance.ObjectState, "Object state failed to transition to 'Deleted'.");

            anInstance.Values.Add(new EAVValue());
        }
        #endregion
        #endregion
    }
}
