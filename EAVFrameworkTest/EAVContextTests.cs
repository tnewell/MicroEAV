using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using EAVFramework.Model;
using System.Linq;

namespace EAVFrameworkTest
{
    public partial class EAVFrameworkTestHarness
    {
        [TestMethod]
        public void ContextCreate()
        {
            EAVContext aContext = new EAVContext();

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
            EAVContext aContext = new EAVContext();

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        public void ContextStateTransitionNewToUnmodifiedWithValidID()
        {
            EAVContext aContext = new EAVContext() { ContextID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ContextStateTransitionNewToDeleted()
        {
            EAVContext aContext = new EAVContext();

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            aContext.MarkDeleted();
        }

        [TestMethod]
        public void ContextStateTransitionUnmodifiedToDeleted()
        {
            EAVContext aContext = new EAVContext() { ContextID = rng.Next() };

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
            EAVContext aContext = new EAVContext() { ContextID = rng.Next() };

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
            EAVContext aContext = new EAVContext();

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            int id = rng.Next();
            aContext.ContextID = id;

            Assert.AreEqual(id, aContext.ContextID, "Property 'ContextID' not properly set.");
            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state after setting property 'ContextID' should be 'New'.");
        }

        [TestMethod]
        public void ContextSetIDBeforeUnmodified()
        {
            EAVContext aContext = new EAVContext();

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
            EAVContext aContext = new EAVContext();

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            aContext.MarkUnmodified();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ContextSetIDWhenDeleted()
        {
            EAVContext aContext = new EAVContext();

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
            EAVContext aContext = new EAVContext();

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            string value = Guid.NewGuid().ToString();
            aContext.Name = value;

            Assert.AreEqual(value, aContext.Name, "Property 'Name' was not set properly.");
            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ContextSetNameWhenUnmodified()
        {
            EAVContext aContext = new EAVContext() { ContextID = rng.Next() };

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
            EAVContext aContext = new EAVContext() { ContextID = rng.Next() };

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
            EAVContext aContext = new EAVContext() { ContextID = rng.Next() };

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
            EAVContext aContext = new EAVContext();

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            string value = Guid.NewGuid().ToString();
            aContext.DataName = value;

            Assert.AreEqual(value, aContext.DataName, "Property 'DataName' was not set properly.");
            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ContextSetDataNameWhenUnmodified()
        {
            EAVContext aContext = new EAVContext() { ContextID = rng.Next() };

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
            EAVContext aContext = new EAVContext() { ContextID = rng.Next() };

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
            EAVContext aContext = new EAVContext() { ContextID = rng.Next() };

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
            EAVContext aContext = new EAVContext();

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            string value = Guid.NewGuid().ToString();
            aContext.DisplayText = value;

            Assert.AreEqual(value, aContext.DisplayText, "Property 'DisplayText' was not set properly.");
            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ContextSetDisplayTextWhenUnmodified()
        {
            EAVContext aContext = new EAVContext() { ContextID = rng.Next() };

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
            EAVContext aContext = new EAVContext() { ContextID = rng.Next() };

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
            EAVContext aContext = new EAVContext() { ContextID = rng.Next() };

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
        public void ContextSetContainersWhenNew()
        {
            EAVContext aContext = new EAVContext() { ContextID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            EAVRootContainer value = new EAVRootContainer() { ContainerID = rng.Next() };
            aContext.Containers.Add(value);

            Assert.IsTrue(aContext.Containers.Contains(value), "Property 'Containers' was not updated properly.");
            Assert.AreEqual(aContext, value.Context, "Property 'Context' was not set properly.");
            Assert.AreEqual(aContext.ContextID, value.ContextID, "Property 'ContextID' was not set properly.");
            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ContextSetContainersWhenUnmodified()
        {
            EAVContext aContext = new EAVContext() { ContextID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVRootContainer value = new EAVRootContainer() { ContainerID = rng.Next() };
            aContext.Containers.Add(value);

            Assert.IsTrue(aContext.Containers.Contains(value), "Property 'Containers' was not updated properly.");
            Assert.AreEqual(aContext, value.Context, "Property 'Context' was not set properly.");
            Assert.AreEqual(aContext.ContextID, value.ContextID, "Property 'ContextID' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aContext.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void ContextSetContainersWhenModified()
        {
            EAVContext aContext = new EAVContext() { ContextID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVRootContainer value = new EAVRootContainer() { ContainerID = rng.Next() };
            aContext.Containers.Add(value);

            Assert.IsTrue(aContext.Containers.Contains(value), "Property 'Containers' was not updated properly.");
            Assert.AreEqual(aContext, value.Context, "Property 'Context' was not set properly.");
            Assert.AreEqual(aContext.ContextID, value.ContextID, "Property 'ContextID' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aContext.ObjectState, "Object state failed to transition to 'Modified'.");

            value = new EAVRootContainer() { ContainerID = rng.Next() };
            aContext.Containers.Add(value);

            Assert.IsTrue(aContext.Containers.Contains(value), "Property 'Containers' was not updated properly.");
            Assert.AreEqual(aContext, value.Context, "Property 'Context' was not set properly.");
            Assert.AreEqual(aContext.ContextID, value.ContextID, "Property 'ContextID' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aContext.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ContextSetContainersWhenDeleted()
        {
            EAVContext aContext = new EAVContext() { ContextID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aContext.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aContext.ObjectState, "Object state failed to transition to 'Deleted'.");

            aContext.Containers.Add(new EAVRootContainer() { ContainerID = rng.Next() });
        }
        #endregion

        #region Subjects
        [TestMethod]
        public void ContextSetSubjectsWhenNew()
        {
            EAVContext aContext = new EAVContext() { ContextID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            EAVSubject value = new EAVSubject() { SubjectID = rng.Next() };
            aContext.Subjects.Add(value);

            Assert.IsTrue(aContext.Subjects.Contains(value), "Property 'Subjects' was not updated properly.");
            Assert.AreEqual(aContext, value.Context, "Property 'Context' was not set properly.");
            Assert.AreEqual(aContext.ContextID, value.ContextID, "Property 'ContextID' was not set properly.");
            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ContextSetSubjectsWhenUnmodified()
        {
            EAVContext aContext = new EAVContext() { ContextID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVSubject value = new EAVSubject() { SubjectID = rng.Next() };
            aContext.Subjects.Add(value);

            Assert.IsTrue(aContext.Subjects.Contains(value), "Property 'Subjects' was not updated properly.");
            Assert.AreEqual(aContext, value.Context, "Property 'Context' was not set properly.");
            Assert.AreEqual(aContext.ContextID, value.ContextID, "Property 'ContextID' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aContext.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void ContextSetSubjectsWhenModified()
        {
            EAVContext aContext = new EAVContext() { ContextID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVSubject value = new EAVSubject() { SubjectID = rng.Next() };
            aContext.Subjects.Add(value);

            Assert.IsTrue(aContext.Subjects.Contains(value), "Property 'Subjects' was not updated properly.");
            Assert.AreEqual(aContext, value.Context, "Property 'Context' was not set properly.");
            Assert.AreEqual(aContext.ContextID, value.ContextID, "Property 'ContextID' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aContext.ObjectState, "Object state failed to transition to 'Modified'.");

            value = new EAVSubject() { SubjectID = rng.Next() };
            aContext.Subjects.Add(value);

            Assert.IsTrue(aContext.Subjects.Contains(value), "Property 'Subjects' was not updated properly.");
            Assert.AreEqual(aContext, value.Context, "Property 'Context' was not set properly.");
            Assert.AreEqual(aContext.ContextID, value.ContextID, "Property 'ContextID' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aContext.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void ContextSetSubjectsWhenDeleted()
        {
            EAVContext aContext = new EAVContext() { ContextID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aContext.ObjectState, "Object state should be 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aContext.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aContext.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aContext.ObjectState, "Object state failed to transition to 'Deleted'.");

            aContext.Subjects.Add(new EAVSubject() { SubjectID = rng.Next() });
        }
        #endregion
        #endregion
    }
}
