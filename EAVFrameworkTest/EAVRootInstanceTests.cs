using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using EAV.Model;

using EAVFramework.Model;


namespace EAVFrameworkTest
{
    public partial class EAVFrameworkTestHarness
    {
        #region Object State Tests
        [TestMethod]
        public void CreateRootInstance()
        {
            EAVRootInstance anInstance = new EAVRootInstance();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            Assert.IsNull(anInstance.InstanceID, "Property 'InstanceID' is not 'null' on creation.");

            Assert.IsNull(anInstance.Subject, "Property 'Subject' is not 'null' on creation.");

            Assert.IsNull(anInstance.Container, "Property 'Container' is not 'null' on creation.");

            Assert.IsNull(anInstance.ParentInstance, "Property 'ParentInstance' is not 'null' on creation.");

            Assert.IsNotNull(anInstance.ChildInstances, "Property 'ChildInstances' is null on creation.");
            Assert.IsTrue(anInstance.ChildInstances.Count == 0, "Property 'ChildInstances' is not empty on creation.");

            Assert.IsNotNull(anInstance.Values, "Property 'Values' is null on creation.");
            Assert.IsTrue(anInstance.Values.Count == 0, "Property 'Values' is not empty on creation.");
        }

        [TestMethod]
        public void SetRootInstanceUnmodifiedFromNew()
        {
            EAVRootInstance anInstance = new EAVRootInstance();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            anInstance.MarkUnmodified();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.Unmodified, "Object state not properly set to 'Unmodified'.");
        }

        [TestMethod]
        public void SetRootInstanceModifiedFromNew()
        {
            EAVRootInstance anInstance = new EAVRootInstance();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            int id = rng.Next();
            anInstance.InstanceID = id;

            Assert.AreEqual(id, anInstance.InstanceID, "Property 'InstanceID' not properly set.");

            Assert.IsTrue(anInstance.ObjectState == ObjectState.New, "Setting property in 'New' state should not alter object state.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SetRootInstanceDeletedFromNew()
        {
            EAVRootInstance anInstance = new EAVRootInstance();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            anInstance.MarkDeleted();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.New, "Setting object state to 'Deleted' from 'New' state should not alter object state.");
        }

        [TestMethod]
        public void SetRootInstanceModifiedFromUnmodified()
        {
            EAVRootInstance anInstance = new EAVRootInstance();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            anInstance.MarkUnmodified();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.Unmodified, "Object state not properly set to 'Unmodified'.");

            EAVSubject aSubject = new EAVSubject();
            anInstance.Subject = aSubject;

            Assert.AreEqual(aSubject, anInstance.Subject, "Property 'Subject' not properly set.");
            Assert.IsTrue(anInstance.ObjectState == ObjectState.Modified, "Setting property should alter object state to 'Modified'.");
        }

        [TestMethod]
        public void SetRootInstanceDeletedFromUnmodified()
        {
            EAVRootInstance anInstance = new EAVRootInstance();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            anInstance.MarkUnmodified();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.Unmodified, "Object state not properly set to 'Unmodified'.");

            anInstance.MarkDeleted();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.Deleted, "Object state not properly set to 'Deleted'.");
        }

        [TestMethod]
        public void SetRootInstanceUnmodifiedFromModified()
        {
            EAVRootInstance anInstance = new EAVRootInstance();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            anInstance.MarkUnmodified();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.Unmodified, "Object state not properly set to 'Unmodified'.");

            EAVSubject aSubject = new EAVSubject();
            anInstance.Subject = aSubject;

            Assert.AreEqual(aSubject, anInstance.Subject, "Property 'Subject' not properly set.");
            Assert.IsTrue(anInstance.ObjectState == ObjectState.Modified, "Setting property should alter object state to 'Modified'.");

            anInstance.MarkUnmodified();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.Unmodified, "Object state not properly set to 'Unmodified'.");
        }

        [TestMethod]
        public void SetRootInstanceDeletedFromModified()
        {
            EAVRootInstance anInstance = new EAVRootInstance();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            anInstance.MarkUnmodified();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.Unmodified, "Object state not properly set to 'Unmodified'.");

            EAVSubject aSubject = new EAVSubject();
            anInstance.Subject = aSubject;

            Assert.AreEqual(aSubject, anInstance.Subject, "Property 'Subject' not properly set.");
            Assert.IsTrue(anInstance.ObjectState == ObjectState.Modified, "Setting property should alter object state to 'Modified'.");

            anInstance.MarkDeleted();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.Deleted, "Object state not properly set to 'Deleted'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SetRootInstanceUnmodifiedFromDeleted()
        {
            EAVRootInstance anInstance = new EAVRootInstance();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            anInstance.MarkUnmodified();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.Unmodified, "Object state not properly set to 'Unmodified'.");

            anInstance.MarkDeleted();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.Deleted, "Object state not properly set to 'Deleted'.");

            anInstance.MarkUnmodified();
        }

        [TestMethod]
        public void SetRootInstanceModifiedFromDeleted()
        {
            EAVRootInstance anInstance = new EAVRootInstance();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            anInstance.MarkUnmodified();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.Unmodified, "Object state not properly set to 'Unmodified'.");

            anInstance.MarkDeleted();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.Deleted, "Object state not properly set to 'Deleted'.");

            anInstance.Subject = new EAVSubject();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.Deleted, "Setting property should not alter object state from 'Deleted'.");
        }
        #endregion

        #region RootInstanceID Property Tests
        [TestMethod]
        public void TestRootInstanceIDForNewRootInstance()
        {
            EAVRootInstance anInstance = new EAVRootInstance();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            int id = rng.Next();
            anInstance.InstanceID = id;

            Assert.AreEqual(id, anInstance.InstanceID, "Property 'InstanceID' was not set correctly.");
            Assert.IsTrue(anInstance.ObjectState == ObjectState.New, "Setting property in 'New' state should not alter object state.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestRootInstanceIDForUnmodifiedRootInstance()
        {
            EAVRootInstance anInstance = new EAVRootInstance();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            anInstance.MarkUnmodified();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            int id = rng.Next();
            anInstance.InstanceID = id;

            Assert.AreEqual(id, anInstance.InstanceID, "Property 'InstanceID' was not set correctly.");
            Assert.IsTrue(anInstance.ObjectState == ObjectState.Unmodified, "Setting null property 'InstanceID' in 'Unmodified' state should not alter object state.");

            anInstance.InstanceID = id;
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestRootInstanceIDForModifiedRootInstance()
        {
            EAVRootInstance anInstance = new EAVRootInstance();

            int id = rng.Next();
            anInstance.InstanceID = id;

            Assert.AreEqual(id, anInstance.InstanceID, "Property 'InstanceID' was not set correctly.");

            anInstance.MarkUnmodified();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            EAVSubject aSubject = new EAVSubject();
            anInstance.Subject = aSubject;

            Assert.IsTrue(anInstance.ObjectState == ObjectState.Modified, "Object was not correctly marked 'Modified'.");

            anInstance.InstanceID = id;
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestRootInstanceIDForDeletedRootInstance()
        {
            EAVRootInstance anInstance = new EAVRootInstance();

            anInstance.MarkUnmodified();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            anInstance.MarkDeleted();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.Deleted, "Object was not correctly marked 'Deleted'.");

            anInstance.InstanceID = rng.Next();
        }
        #endregion

        #region Subject Property Tests
        [TestMethod]
        public void TestSubjectForNewRootInstance()
        {
            EAVRootInstance anInstance = new EAVRootInstance();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            EAVSubject aSubject = new EAVSubject();
            anInstance.Subject = aSubject;

            Assert.AreEqual(aSubject, anInstance.Subject, "Property 'Subject' was not set correctly.");
            Assert.IsTrue(anInstance.ObjectState == ObjectState.New, "Setting property in 'New' state should not alter object state.");
        }

        [TestMethod]
        public void TestSubjectForUnmodifiedRootInstance()
        {
            EAVRootInstance anInstance = new EAVRootInstance();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            anInstance.MarkUnmodified();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            EAVSubject aSubject = new EAVSubject();
            anInstance.Subject = aSubject;

            Assert.AreEqual(aSubject, anInstance.Subject, "Property 'Subject' was not set correctly.");
            Assert.IsTrue(anInstance.ObjectState == ObjectState.Modified, "Setting property in 'Unmodified' state should alter state to 'Modified'.");
        }

        [TestMethod]
        public void TestSubjectForModifiedRootInstance()
        {
            EAVRootInstance anInstance = new EAVRootInstance();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            anInstance.MarkUnmodified();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            EAVSubject aSubject = new EAVSubject();
            anInstance.Subject = aSubject;

            Assert.AreEqual(aSubject, anInstance.Subject, "Property 'Subject' was not set correctly.");
            Assert.IsTrue(anInstance.ObjectState == ObjectState.Modified, "Setting property in 'Unmodified' state should alter state to 'Modified'.");

            aSubject = new EAVSubject();
            anInstance.Subject = aSubject;

            Assert.AreEqual(aSubject, anInstance.Subject, "Property 'Subject' was not set correctly.");
            Assert.IsTrue(anInstance.ObjectState == ObjectState.Modified, "Setting property in 'Modified' state should not alter object state.");
        }

        [TestMethod]
        public void TestSubjectForDeletedRootInstance()
        {
            EAVRootInstance anInstance = new EAVRootInstance();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            anInstance.MarkUnmodified();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            anInstance.MarkDeleted();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.Deleted, "Object was not correctly marked 'Deleted'.");

            EAVSubject aSubject = new EAVSubject();
            anInstance.Subject = aSubject;

            Assert.AreNotEqual(aSubject, anInstance.Subject, "Setting property in 'Deleted' state should not alter property value.");
            Assert.IsTrue(anInstance.ObjectState == ObjectState.Deleted, "Setting property in 'Deleted' state should not alter object state.");
        }

        [TestMethod]
        public void TestSubjectInteractionWithRootInstance()
        {
            int instanceID = rng.Next();
            EAVRootInstance anInstance = new EAVRootInstance() { InstanceID = instanceID };
            int subjectID = rng.Next();
            EAVSubject aSubject = new EAVSubject() { SubjectID = subjectID };

            Assert.AreEqual(instanceID, anInstance.InstanceID, "Property 'InstanceID' was not set properly.");
            Assert.AreEqual(subjectID, aSubject.SubjectID, "Property 'SubjectID' was not set properly.");

            Assert.IsNull(anInstance.Subject, "Property 'Subject' should be null.");
            Assert.IsTrue(aSubject.Instances.Count == 0, "Collection property 'Instances' of EAVSubject object should be empty.");

            anInstance.Subject = aSubject;

            Assert.AreEqual(aSubject, anInstance.Subject, "Property 'Subject' was not set properly.");
            Assert.AreEqual(subjectID, anInstance.Subject.SubjectID, "Property 'SubjectID' was not set properly.");

            Assert.IsTrue(aSubject.Instances.Count == 1, "Collection property 'Instances' was not set properly.");
            Assert.AreEqual(instanceID, aSubject.Instances.First().InstanceID, "Property 'InstanceID' was not set properly.");

            instanceID = rng.Next();
            anInstance = new EAVRootInstance() { InstanceID = instanceID };
            subjectID = rng.Next();
            aSubject = new EAVSubject() { SubjectID = subjectID };

            aSubject.Instances.Add(anInstance);

            Assert.AreEqual(aSubject, anInstance.Subject, "Property 'Subject' was not set properly.");
            Assert.AreEqual(subjectID, anInstance.Subject.SubjectID, "Property 'SubjectID' was not set properly.");

            Assert.IsTrue(aSubject.Instances.Count == 1, "Collection property 'Instances' was not set properly.");
            Assert.AreEqual(instanceID, aSubject.Instances.First().InstanceID, "Property 'InstanceID' was not set properly.");
        }
        #endregion

        #region Container Property Tests
        [TestMethod]
        public void TestContainerForNewRootInstance()
        {
            EAVRootInstance anInstance = new EAVRootInstance();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            EAVContainer aContainer = new EAVRootContainer();
            anInstance.Container = aContainer;

            Assert.AreEqual(aContainer, anInstance.Container, "Property 'Container' was not set correctly.");
            Assert.IsTrue(anInstance.ObjectState == ObjectState.New, "Setting property in 'New' state should not alter object state.");
        }

        [TestMethod]
        public void TestContainerForUnmodifiedRootInstance()
        {
            EAVRootInstance anInstance = new EAVRootInstance();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            anInstance.MarkUnmodified();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            EAVContainer aContainer = new EAVRootContainer();
            anInstance.Container = aContainer;

            Assert.AreEqual(aContainer, anInstance.Container, "Property 'Container' was not set correctly.");
            Assert.IsTrue(anInstance.ObjectState == ObjectState.Modified, "Setting property in 'Unmodified' state should alter state to 'Modified'.");
        }

        [TestMethod]
        public void TestContainerForModifiedRootInstance()
        {
            EAVRootInstance anInstance = new EAVRootInstance();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            anInstance.MarkUnmodified();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            EAVContainer aContainer = new EAVRootContainer();
            anInstance.Container = aContainer;

            Assert.AreEqual(aContainer, anInstance.Container, "Property 'Container' was not set correctly.");
            Assert.IsTrue(anInstance.ObjectState == ObjectState.Modified, "Setting property in 'Unmodified' state should alter state to 'Modified'.");

            aContainer = new EAVRootContainer();
            anInstance.Container = aContainer;

            Assert.AreEqual(aContainer, anInstance.Container, "Property 'Container' was not set correctly.");
            Assert.IsTrue(anInstance.ObjectState == ObjectState.Modified, "Setting property in 'Modified' state should not alter object state.");
        }

        [TestMethod]
        public void TestContainerForDeletedRootInstance()
        {
            EAVRootInstance anInstance = new EAVRootInstance();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            anInstance.MarkUnmodified();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            anInstance.MarkDeleted();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.Deleted, "Object was not correctly marked 'Deleted'.");

            EAVContainer aContainer = new EAVRootContainer();
            anInstance.Container = aContainer;

            Assert.AreNotEqual(aContainer, anInstance.Container, "Setting property in 'Deleted' state should not alter property value.");
            Assert.IsTrue(anInstance.ObjectState == ObjectState.Deleted, "Setting property in 'Deleted' state should not alter object state.");
        }

        [TestMethod]
        public void TestContainerInteractionWithRootInstance()
        {
            int instanceID = rng.Next();
            EAVRootInstance anInstance = new EAVRootInstance() { InstanceID = instanceID };
            int containerID = rng.Next();
            EAVContainer aContainer = new EAVRootContainer() { ContainerID = containerID };

            Assert.AreEqual(instanceID, anInstance.InstanceID, "Property 'InstanceID' was not set properly.");
            Assert.AreEqual(containerID, aContainer.ContainerID, "Property 'ContainerID' was not set properly.");

            Assert.IsNull(anInstance.Container, "Property 'Container' should be null.");
            Assert.IsTrue(aContainer.Instances.Count == 0, "Collection property 'Instances' of EAVContainer object should be empty.");

            anInstance.Container = aContainer;

            Assert.AreEqual(aContainer, anInstance.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(containerID, anInstance.Container.ContainerID, "Property 'ContainerID' was not set properly.");

            Assert.IsTrue(aContainer.Instances.Count == 1, "Collection property 'Instances' was not set properly.");
            Assert.AreEqual(instanceID, aContainer.Instances.First().InstanceID, "Property 'InstanceID' was not set properly.");

            instanceID = rng.Next();
            anInstance = new EAVRootInstance() { InstanceID = instanceID };
            containerID = rng.Next();
            aContainer = new EAVRootContainer() { ContainerID = containerID };

            aContainer.Instances.Add(anInstance);

            Assert.AreEqual(aContainer, anInstance.Container, "Property 'Container' was not set properly.");
            Assert.AreEqual(containerID, anInstance.Container.ContainerID, "Property 'ContainerID' was not set properly.");

            Assert.IsTrue(aContainer.Instances.Count == 1, "Collection property 'Instances' was not set properly.");
            Assert.AreEqual(instanceID, aContainer.Instances.First().InstanceID, "Property 'InstanceID' was not set properly.");
        }
        #endregion

        #region ParentInstance Property Tests
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestParentInstanceForNewRootInstance()
        {
            EAVRootInstance anInstance = new EAVRootInstance();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");
            Assert.IsNull(anInstance.ParentInstance, "Property 'ParentInstance' shoud be null on creation.");

            anInstance.ParentInstance = new EAVRootInstance();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestParentInstanceForUnmodifiedRootInstance()
        {
            EAVRootInstance anInstance = new EAVRootInstance();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            anInstance.MarkUnmodified();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            anInstance.ParentInstance = new EAVRootInstance();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestParentInstanceForModifiedRootInstance()
        {
            EAVRootInstance anInstance = new EAVRootInstance();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            anInstance.MarkUnmodified();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            anInstance.ParentInstance = new EAVRootInstance();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestParentInstanceForDeletedRootInstance()
        {
            EAVRootInstance anInstance = new EAVRootInstance();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            anInstance.MarkUnmodified();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            anInstance.MarkDeleted();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.Deleted, "Object was not correctly marked 'Deleted'.");

            anInstance.ParentInstance = new EAVRootInstance();
        }
        #endregion

        #region ChildInstances Property Tests
        [TestMethod]
        public void TestChildInstancesForNewRootInstance()
        {
            EAVRootInstance anInstance = new EAVRootInstance();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            anInstance.ChildInstances.Add(new EAVChildInstance());
            anInstance.ChildInstances.Add(new EAVChildInstance());

            Assert.IsTrue(anInstance.ChildInstances.Count == 2, "Property 'ChildInstances' was not set correctly.");
            Assert.IsTrue(anInstance.ObjectState == ObjectState.New, "Modifying property in 'New' state should not alter object state.");

            anInstance.ChildInstances.Remove(anInstance.ChildInstances.First());

            Assert.IsTrue(anInstance.ObjectState == ObjectState.New, "Modifying property in 'New' state should not alter object state.");

            anInstance.ChildInstances.Clear();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.New, "Modifying property in 'New' state should not alter object state.");
        }

        [TestMethod]
        public void TestChildInstancesForUnmodifiedRootInstance()
        {
            EAVRootInstance anInstance = new EAVRootInstance();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            anInstance.MarkUnmodified();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            anInstance.ChildInstances.Add(new EAVChildInstance());
            anInstance.ChildInstances.Add(new EAVChildInstance());

            Assert.IsTrue(anInstance.ChildInstances.Count == 2, "Property 'ChildInstances' was not set correctly.");
            Assert.IsTrue(anInstance.ObjectState == ObjectState.Modified, "Modifying property in 'Unmodified' state should alter object state to 'Modified'.");

            anInstance.MarkUnmodified();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            anInstance.ChildInstances.Remove(anInstance.ChildInstances.First());

            Assert.IsTrue(anInstance.ObjectState == ObjectState.Modified, "Modifying property in 'Unmodified' state should alter object state to 'Modified'.");

            anInstance.MarkUnmodified();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            anInstance.ChildInstances.Clear();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.Modified, "Modifying property in 'Unmodified' state should alter object state to 'Modified'.");
        }

        [TestMethod]
        public void TestChildInstancesForModifiedRootInstance()
        {
            EAVRootInstance anInstance = new EAVRootInstance();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            anInstance.MarkUnmodified();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            anInstance.Subject = new EAVSubject();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.Modified, "Object was not correctly marked 'Modified'.");

            anInstance.ChildInstances.Add(new EAVChildInstance());
            anInstance.ChildInstances.Add(new EAVChildInstance());

            Assert.IsTrue(anInstance.ChildInstances.Count == 2, "Property 'ChildInstances' was not set correctly.");
            Assert.IsTrue(anInstance.ObjectState == ObjectState.Modified, "Modifying property in 'Modified' state should not alter object state.");

            anInstance.ChildInstances.Remove(anInstance.ChildInstances.First());

            Assert.IsTrue(anInstance.ObjectState == ObjectState.Modified, "Modifying property in 'Modified' state should not alter object state.");

            anInstance.ChildInstances.Clear();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.Modified, "Modifying property in 'Modified' state should not alter object state.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestChildInstancesForDeletedRootInstanceWithAdd()
        {
            EAVRootInstance anInstance = new EAVRootInstance();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            anInstance.MarkUnmodified();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            anInstance.MarkDeleted();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.Deleted, "Object was not correctly marked 'Deleted'.");

            anInstance.ChildInstances.Add(new EAVChildInstance());
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestChildInstancesForDeletedRootInstanceWithRemove()
        {
            EAVRootInstance anInstance = new EAVRootInstance();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            anInstance.ChildInstances.Add(new EAVChildInstance());
            anInstance.ChildInstances.Add(new EAVChildInstance());

            anInstance.MarkUnmodified();

            foreach (EAVChildInstance childContainer in anInstance.ChildInstances)
            {
                childContainer.MarkUnmodified();
            }

            Assert.IsTrue(anInstance.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            anInstance.MarkDeleted();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.Deleted, "Object was not correctly marked 'Deleted'.");

            anInstance.ChildInstances.Remove(anInstance.ChildInstances.First());
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestChildInstancesForDeletedRootInstanceWithClear()
        {
            EAVRootInstance anInstance = new EAVRootInstance();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            anInstance.ChildInstances.Add(new EAVChildInstance());

            anInstance.MarkUnmodified();

            foreach (EAVChildInstance childContainer in anInstance.ChildInstances)
            {
                childContainer.MarkUnmodified();
            }

            Assert.IsTrue(anInstance.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            anInstance.MarkDeleted();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.Deleted, "Object was not correctly marked 'Deleted'.");

            anInstance.ChildInstances.Clear();
        }

        [TestMethod]
        public void TestChildInstancesInteractionWithRootInstance()
        {
            EAVRootInstance anInstance = new EAVRootInstance();

            int id = rng.Next();
            anInstance.InstanceID = id;

            EAVChildInstance aChildInstance = new EAVChildInstance();
            anInstance.ChildInstances.Add(aChildInstance);

            Assert.IsTrue(anInstance.ChildInstances.Count == 1, "Collection property 'ChildInstances' is empty.");
            Assert.IsTrue(anInstance.ChildInstances.Single() == aChildInstance, "Collection property 'ChildInstances' not modified properly.");

            Assert.IsNotNull(aChildInstance.ParentInstance, "Property 'ParentInstance' is null.");
            Assert.AreEqual(anInstance, aChildInstance.ParentInstance, "Property 'ParentInstance' not set properly.");

            Assert.IsNotNull(aChildInstance.ParentInstanceID, "Property 'ParentInstanceID' is null.");
            Assert.AreEqual(id, aChildInstance.ParentInstanceID, "Property 'ParentInstanceID' not set properly.");
        }
        #endregion

        #region Values Property Tests
        [TestMethod]
        public void TestValuesForNewRootInstance()
        {
            EAVRootInstance anInstance = new EAVRootInstance();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            anInstance.Values.Add(new EAVValue());
            anInstance.Values.Add(new EAVValue());

            Assert.IsTrue(anInstance.Values.Count == 2, "Property 'Values' was not set correctly.");
            Assert.IsTrue(anInstance.ObjectState == ObjectState.New, "Modifying property in 'New' state should not alter object state.");

            anInstance.Values.Remove(anInstance.Values.First());

            Assert.IsTrue(anInstance.ObjectState == ObjectState.New, "Modifying property in 'New' state should not alter object state.");

            anInstance.Values.Clear();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.New, "Modifying property in 'New' state should not alter object state.");
        }

        [TestMethod]
        public void TestValuesForUnmodifiedRootInstance()
        {
            EAVRootInstance anInstance = new EAVRootInstance();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            anInstance.MarkUnmodified();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            anInstance.Values.Add(new EAVValue());
            anInstance.Values.Add(new EAVValue());

            Assert.IsTrue(anInstance.Values.Count == 2, "Property 'Values' was not set correctly.");
            Assert.IsTrue(anInstance.ObjectState == ObjectState.Modified, "Modifying property in 'Unmodified' state should alter object state to 'Modified'.");

            anInstance.MarkUnmodified();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            anInstance.Values.Remove(anInstance.Values.First());

            Assert.IsTrue(anInstance.ObjectState == ObjectState.Modified, "Modifying property in 'Unmodified' state should alter object state to 'Modified'.");

            anInstance.MarkUnmodified();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            anInstance.Values.Clear();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.Modified, "Modifying property in 'Unmodified' state should alter object state to 'Modified'.");
        }

        [TestMethod]
        public void TestValuesForModifiedRootInstance()
        {
            EAVRootInstance anInstance = new EAVRootInstance();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            anInstance.MarkUnmodified();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            anInstance.Subject = new EAVSubject();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.Modified, "Object was not correctly marked 'Modified'.");

            anInstance.Values.Add(new EAVValue());
            anInstance.Values.Add(new EAVValue());

            Assert.IsTrue(anInstance.Values.Count == 2, "Property 'Values' was not set correctly.");
            Assert.IsTrue(anInstance.ObjectState == ObjectState.Modified, "Modifying property in 'Modified' state should not alter object state.");

            anInstance.Values.Remove(anInstance.Values.First());

            Assert.IsTrue(anInstance.ObjectState == ObjectState.Modified, "Modifying property in 'Modified' state should not alter object state.");

            anInstance.Values.Clear();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.Modified, "Modifying property in 'Modified' state should not alter object state.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestValuesForDeletedRootInstanceWithAdd()
        {
            EAVRootInstance anInstance = new EAVRootInstance();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            anInstance.MarkUnmodified();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            anInstance.MarkDeleted();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.Deleted, "Object was not correctly marked 'Deleted'.");

            anInstance.Values.Add(new EAVValue());
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestValuesForDeletedRootInstanceWithRemove()
        {
            EAVRootInstance anInstance = new EAVRootInstance();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            anInstance.Values.Add(new EAVValue());
            anInstance.Values.Add(new EAVValue());

            anInstance.MarkUnmodified();

            foreach (EAVValue aValue in anInstance.Values)
            {
                aValue.MarkUnmodified();
            }

            Assert.IsTrue(anInstance.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            anInstance.MarkDeleted();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.Deleted, "Object was not correctly marked 'Deleted'.");

            anInstance.Values.Remove(anInstance.Values.First());
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestValuesForDeletedRootInstanceWithClear()
        {
            EAVRootInstance anInstance = new EAVRootInstance();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            anInstance.Values.Add(new EAVValue());

            anInstance.MarkUnmodified();

            foreach (EAVValue aValue in anInstance.Values)
            {
                aValue.MarkUnmodified();
            }

            Assert.IsTrue(anInstance.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            anInstance.MarkDeleted();

            Assert.IsTrue(anInstance.ObjectState == ObjectState.Deleted, "Object was not correctly marked 'Deleted'.");

            anInstance.Values.Clear();
        }

        [TestMethod]
        public void TestValuesInteractionWithRootInstance()
        {
            EAVRootInstance anInstance = new EAVRootInstance();

            int id = rng.Next();
            anInstance.InstanceID = id;

            EAVValue anValue = new EAVValue();
            anInstance.Values.Add(anValue);

            Assert.IsTrue(anInstance.Values.Count == 1, "Collection property 'Values' is empty.");
            Assert.IsTrue(anInstance.Values.Single() == anValue, "Collection property 'Values' not modified properly.");

            Assert.IsNotNull(anValue.Instance, "Property 'Instance' is null.");
            Assert.AreEqual(anInstance, anValue.Instance, "Property 'Instance' not set properly.");

            Assert.IsNotNull(anValue.InstanceID, "Propert 'InstanceID' is null.");
            Assert.AreEqual(id, anValue.InstanceID, "Property 'InstanceID' not set properly.");
        }
        #endregion
    }
}
