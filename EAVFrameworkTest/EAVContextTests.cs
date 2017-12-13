using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using EAVFramework.Model;
using System.Linq;

namespace EAVFrameworkTest
{
    public partial class EAVFrameworkTestHarness
    {
        #region Object State Tests
        [TestMethod]
        public void CreateContext()
        {
            EAVContext aContext = new EAVContext();

            Assert.IsTrue(aContext.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            Assert.IsNull(aContext.ContextID, "Property 'ContextID' is not 'null' on creation.");

            Assert.IsNull(aContext.Name, "Property 'Name' is not 'null' on creation.");
            Assert.IsNull(aContext.DataName, "Property 'DataName' is not 'null' on creation.");
            Assert.IsNull(aContext.DisplayText, "Property 'DisplayText' is not 'null' on creation.");

            Assert.IsNotNull(aContext.Containers, "Property 'Containers' is null on creation.");
            Assert.IsTrue(aContext.Containers.Count == 0, "Property 'Containers' is not empty on creation.");

            Assert.IsNotNull(aContext.Subjects, "Property 'Subjects' is null on creation.");
            Assert.IsTrue(aContext.Subjects.Count == 0, "Property 'Subjects' is not empty on creation.");
        }

        [TestMethod]
        public void SetContextUnmodifiedFromNew()
        {
            EAVContext aContext = new EAVContext();

            Assert.IsTrue(aContext.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.IsTrue(aContext.ObjectState == ObjectState.Unmodified, "Object state not properly set to 'Unmodified'.");
        }

        [TestMethod]
        public void SetContextModifiedFromNew()
        {
            EAVContext aContext = new EAVContext();

            Assert.IsTrue(aContext.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            string name = Guid.NewGuid().ToString();
            aContext.Name = name;

            Assert.AreEqual(name, aContext.Name, "Property 'Name' not properly set.");

            Assert.IsTrue(aContext.ObjectState == ObjectState.New, "Setting property in 'New' state should not alter object state.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SetContextDeletedFromNew()
        {
            EAVContext aContext = new EAVContext();

            Assert.IsTrue(aContext.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContext.MarkDeleted();

            Assert.IsTrue(aContext.ObjectState == ObjectState.New, "Setting object state to 'Deleted' from 'New' state should not alter object state.");
        }

        [TestMethod]
        public void SetContextModifiedFromUnmodified()
        {
            EAVContext aContext = new EAVContext();

            Assert.IsTrue(aContext.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.IsTrue(aContext.ObjectState == ObjectState.Unmodified, "Object state not properly set to 'Unmodified'.");

            string name = Guid.NewGuid().ToString();
            aContext.Name = name;

            Assert.AreEqual(name, aContext.Name, "Property 'Name' not properly set.");
            Assert.IsTrue(aContext.ObjectState == ObjectState.Modified, "Setting property should alter object state to 'Modified'.");
        }

        [TestMethod]
        public void SetContextDeletedFromUnmodified()
        {
            EAVContext aContext = new EAVContext();

            Assert.IsTrue(aContext.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.IsTrue(aContext.ObjectState == ObjectState.Unmodified, "Object state not properly set to 'Unmodified'.");

            aContext.MarkDeleted();

            Assert.IsTrue(aContext.ObjectState == ObjectState.Deleted, "Object state not properly set to 'Deleted'.");
        }

        [TestMethod]
        public void SetContextUnmodifiedFromModified()
        {
            EAVContext aContext = new EAVContext();

            Assert.IsTrue(aContext.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.IsTrue(aContext.ObjectState == ObjectState.Unmodified, "Object state not properly set to 'Unmodified'.");

            string name = Guid.NewGuid().ToString();
            aContext.Name = name;

            Assert.AreEqual(name, aContext.Name, "Property 'Name' not properly set.");
            Assert.IsTrue(aContext.ObjectState == ObjectState.Modified, "Setting property should alter object state to 'Modified'.");

            aContext.MarkUnmodified();

            Assert.IsTrue(aContext.ObjectState == ObjectState.Unmodified, "Object state not properly set to 'Unmodified'.");
        }

        [TestMethod]
        public void SetContextDeletedFromModified()
        {
            EAVContext aContext = new EAVContext();

            Assert.IsTrue(aContext.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.IsTrue(aContext.ObjectState == ObjectState.Unmodified, "Object state not properly set to 'Unmodified'.");

            string name = Guid.NewGuid().ToString();
            aContext.Name = name;

            Assert.AreEqual(name, aContext.Name, "Property 'Name' not properly set.");
            Assert.IsTrue(aContext.ObjectState == ObjectState.Modified, "Setting property should alter object state to 'Modified'.");

            aContext.MarkDeleted();

            Assert.IsTrue(aContext.ObjectState == ObjectState.Deleted, "Object state not properly set to 'Deleted'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SetContextUnmodifiedFromDeleted()
        {
            EAVContext aContext = new EAVContext();

            Assert.IsTrue(aContext.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.IsTrue(aContext.ObjectState == ObjectState.Unmodified, "Object state not properly set to 'Unmodified'.");

            aContext.MarkDeleted();

            Assert.IsTrue(aContext.ObjectState == ObjectState.Deleted, "Object state not properly set to 'Deleted'.");

            aContext.MarkUnmodified();
        }

        [TestMethod]
        public void SetContextModifiedFromDeleted()
        {
            EAVContext aContext = new EAVContext();

            Assert.IsTrue(aContext.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.IsTrue(aContext.ObjectState == ObjectState.Unmodified, "Object state not properly set to 'Unmodified'.");

            aContext.MarkDeleted();

            Assert.IsTrue(aContext.ObjectState == ObjectState.Deleted, "Object state not properly set to 'Deleted'.");

            aContext.Name = Guid.NewGuid().ToString();

            Assert.IsTrue(aContext.ObjectState == ObjectState.Deleted, "Setting property should not alter object state from 'Deleted'.");
        }
        #endregion

        #region Context ID Property Tests
        [TestMethod]
        public void TestContextIDForNewContext()
        {
            EAVContext aContext = new EAVContext();

            Assert.IsTrue(aContext.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            int id = rng.Next();
            aContext.ContextID = id;

            Assert.AreEqual(id, aContext.ContextID, "Property 'ContextID' was not set correctly.");
            Assert.IsTrue(aContext.ObjectState == ObjectState.New, "Setting property in 'New' state should not alter object state.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestContextIDForUnmodifiedContext()
        {
            EAVContext aContext = new EAVContext();

            Assert.IsTrue(aContext.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.IsTrue(aContext.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            int id = rng.Next();
            aContext.ContextID = id;

            Assert.AreEqual(id, aContext.ContextID, "Property 'ContextID' was not set correctly.");
            Assert.IsTrue(aContext.ObjectState == ObjectState.Unmodified, "Setting null property 'ContextID' in 'Unmodified' state should not alter object state.");

            aContext.ContextID = id;
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestContextIDForModifiedContext()
        {
            EAVContext aContext = new EAVContext();

            int id = rng.Next();
            aContext.ContextID = id;

            Assert.AreEqual(id, aContext.ContextID, "Property 'ContextID' was not set correctly.");

            aContext.MarkUnmodified();

            Assert.IsTrue(aContext.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            string name = Guid.NewGuid().ToString();
            aContext.Name = name;

            Assert.IsTrue(aContext.ObjectState == ObjectState.Modified, "Object was not correctly marked 'Modified'.");

            aContext.ContextID = id;
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestContextIDForDeletedContext()
        {
            EAVContext aContext = new EAVContext();

            aContext.MarkUnmodified();

            Assert.IsTrue(aContext.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            aContext.MarkDeleted();

            Assert.IsTrue(aContext.ObjectState == ObjectState.Deleted, "Object was not correctly marked 'Deleted'.");

            aContext.ContextID = rng.Next();
        }
        #endregion

        #region Name Property Tests
        [TestMethod]
        public void TestNameForNewContext()
        {
            EAVContext aContext = new EAVContext();

            Assert.IsTrue(aContext.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            string name = Guid.NewGuid().ToString();
            aContext.Name = name;

            Assert.AreEqual(name, aContext.Name, "Property 'Name' was not set correctly.");
            Assert.IsTrue(aContext.ObjectState == ObjectState.New, "Setting property in 'New' state should not alter object state.");
        }

        [TestMethod]
        public void TestNameForUnmodifiedContext()
        {
            EAVContext aContext = new EAVContext();

            Assert.IsTrue(aContext.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.IsTrue(aContext.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            string name = Guid.NewGuid().ToString();
            aContext.Name = name;

            Assert.AreEqual(name, aContext.Name, "Property 'Name' was not set correctly.");
            Assert.IsTrue(aContext.ObjectState == ObjectState.Modified, "Setting property in 'Unmodified' state should alter state to 'Modified'.");
        }

        [TestMethod]
        public void TestNameForModifiedContext()
        {
            EAVContext aContext = new EAVContext();

            Assert.IsTrue(aContext.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.IsTrue(aContext.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            string name = Guid.NewGuid().ToString();
            aContext.Name = name;

            Assert.AreEqual(name, aContext.Name, "Property 'Name' was not set correctly.");
            Assert.IsTrue(aContext.ObjectState == ObjectState.Modified, "Setting property in 'Unmodified' state should alter state to 'Modified'.");

            string oldName = name;
            name = Guid.NewGuid().ToString();

            Assert.AreNotEqual(oldName, name, "New value selected for name is the same as the old value.");

            aContext.Name = name;

            Assert.AreEqual(name, aContext.Name, "Property 'Name' was not set correctly.");
            Assert.IsTrue(aContext.ObjectState == ObjectState.Modified, "Setting property in 'Modified' state should not alter object state.");
        }

        [TestMethod]
        public void TestNameForDeletedContext()
        {
            EAVContext aContext = new EAVContext();

            Assert.IsTrue(aContext.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.IsTrue(aContext.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            aContext.MarkDeleted();

            Assert.IsTrue(aContext.ObjectState == ObjectState.Deleted, "Object was not correctly marked 'Deleted'.");

            aContext.Name = Guid.NewGuid().ToString();

            Assert.IsNull(aContext.Name, "Setting property in 'Deleted' state should not alter property value.");
            Assert.IsTrue(aContext.ObjectState == ObjectState.Deleted, "Setting property in 'Deleted' state should not alter object state.");
        }
        #endregion

        #region DataName Property Tests
        [TestMethod]
        public void TestDataNameForNewContext()
        {
            EAVContext aContext = new EAVContext();

            Assert.IsTrue(aContext.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            string dataName = Guid.NewGuid().ToString();
            aContext.DataName = dataName;

            Assert.AreEqual(dataName, aContext.DataName, "Property 'DataName' was not set correctly.");
            Assert.IsTrue(aContext.ObjectState == ObjectState.New, "Setting property in 'New' state should not alter object state.");
        }

        [TestMethod]
        public void TestDataNameForUnmodifiedContext()
        {
            EAVContext aContext = new EAVContext();

            Assert.IsTrue(aContext.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.IsTrue(aContext.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            string dataName = Guid.NewGuid().ToString();
            aContext.DataName = dataName;

            Assert.AreEqual(dataName, aContext.DataName, "Property 'DataName' was not set correctly.");
            Assert.IsTrue(aContext.ObjectState == ObjectState.Modified, "Setting property in 'Unmodified' state should alter state to 'Modified'.");
        }

        [TestMethod]
        public void TestDataNameForModifiedContext()
        {
            EAVContext aContext = new EAVContext();

            Assert.IsTrue(aContext.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.IsTrue(aContext.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            string dataName = Guid.NewGuid().ToString();
            aContext.DataName = dataName;

            Assert.AreEqual(dataName, aContext.DataName, "Property 'DataName' was not set correctly.");
            Assert.IsTrue(aContext.ObjectState == ObjectState.Modified, "Setting property in 'Unmodified' state should alter state to 'Modified'.");

            string oldDataName = dataName;
            dataName = Guid.NewGuid().ToString();

            Assert.AreNotEqual(oldDataName, dataName, "New value selected for name is the same as the old value.");

            aContext.DataName = dataName;

            Assert.AreEqual(dataName, aContext.DataName, "Property 'DataName' was not set correctly.");
            Assert.IsTrue(aContext.ObjectState == ObjectState.Modified, "Setting property in 'Modified' state should not alter object state.");
        }

        [TestMethod]
        public void TestDataNameForDeletedContext()
        {
            EAVContext aContext = new EAVContext();

            Assert.IsTrue(aContext.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.IsTrue(aContext.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            aContext.MarkDeleted();

            Assert.IsTrue(aContext.ObjectState == ObjectState.Deleted, "Object was not correctly marked 'Deleted'.");

            aContext.Name = Guid.NewGuid().ToString();

            Assert.IsNull(aContext.DataName, "Setting property in 'Deleted' state should not alter property value.");
            Assert.IsTrue(aContext.ObjectState == ObjectState.Deleted, "Setting property in 'Deleted' state should not alter object state.");
        }
        #endregion

        #region DisplayText Property Tests
        [TestMethod]
        public void TestDisplayTextForNewContext()
        {
            EAVContext aContext = new EAVContext();

            Assert.IsTrue(aContext.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            string displayText = Guid.NewGuid().ToString();
            aContext.DisplayText = displayText;

            Assert.AreEqual(displayText, aContext.DisplayText, "Property 'DisplayText' was not set correctly.");
            Assert.IsTrue(aContext.ObjectState == ObjectState.New, "Setting property in 'New' state should not alter object state.");
        }

        [TestMethod]
        public void TestDisplayTextForUnmodifiedContext()
        {
            EAVContext aContext = new EAVContext();

            Assert.IsTrue(aContext.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.IsTrue(aContext.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            string displayText = Guid.NewGuid().ToString();
            aContext.DisplayText = displayText;

            Assert.AreEqual(displayText, aContext.DisplayText, "Property 'DisplayText' was not set correctly.");
            Assert.IsTrue(aContext.ObjectState == ObjectState.Modified, "Setting property in 'Unmodified' state should alter state to 'Modified'.");
        }

        [TestMethod]
        public void TestDisplayTextForModifiedContext()
        {
            EAVContext aContext = new EAVContext();

            Assert.IsTrue(aContext.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.IsTrue(aContext.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            string displayText = Guid.NewGuid().ToString();
            aContext.DisplayText = displayText;

            Assert.AreEqual(displayText, aContext.DisplayText, "Property 'DisplayText' was not set correctly.");
            Assert.IsTrue(aContext.ObjectState == ObjectState.Modified, "Setting property in 'Unmodified' state should alter state to 'Modified'.");

            string oldDisplayText = displayText;
            displayText = Guid.NewGuid().ToString();

            Assert.AreNotEqual(oldDisplayText, displayText, "New value selected for name is the same as the old value.");

            aContext.DisplayText = displayText;

            Assert.AreEqual(displayText, aContext.DisplayText, "Property 'DisplayText' was not set correctly.");
            Assert.IsTrue(aContext.ObjectState == ObjectState.Modified, "Setting property in 'Modified' state should not alter object state.");
        }

        [TestMethod]
        public void TestDisplayTextForDeletedContext()
        {
            EAVContext aContext = new EAVContext();

            Assert.IsTrue(aContext.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.IsTrue(aContext.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            aContext.MarkDeleted();

            Assert.IsTrue(aContext.ObjectState == ObjectState.Deleted, "Object was not correctly marked 'Deleted'.");

            aContext.Name = Guid.NewGuid().ToString();

            Assert.IsNull(aContext.DisplayText, "Setting property in 'Deleted' state should not alter property value.");
            Assert.IsTrue(aContext.ObjectState == ObjectState.Deleted, "Setting property in 'Deleted' state should not alter object state.");
        }
        #endregion

        #region Subjects Property Tests
        [TestMethod]
        public void TestSubjectForNewContext()
        {
            EAVContext aContext = new EAVContext();

            Assert.IsTrue(aContext.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContext.Subjects.Add(new EAVSubject());
            aContext.Subjects.Add(new EAVSubject());

            Assert.IsTrue(aContext.Subjects.Count == 2, "Property 'Subjects' was not set correctly.");
            Assert.IsTrue(aContext.ObjectState == ObjectState.New, "Modifying property in 'New' state should not alter object state.");

            aContext.Subjects.Remove(aContext.Subjects.First());

            Assert.IsTrue(aContext.ObjectState == ObjectState.New, "Modifying property in 'New' state should not alter object state.");

            aContext.Subjects.Clear();

            Assert.IsTrue(aContext.ObjectState == ObjectState.New, "Modifying property in 'New' state should not alter object state.");
        }

        [TestMethod]
        public void TestSubjectForUnmodifiedContext()
        {
            EAVContext aContext = new EAVContext();

            Assert.IsTrue(aContext.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.IsTrue(aContext.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            aContext.Subjects.Add(new EAVSubject());
            aContext.Subjects.Add(new EAVSubject());

            Assert.IsTrue(aContext.Subjects.Count == 2, "Property 'Subjects' was not set correctly.");
            Assert.IsTrue(aContext.ObjectState == ObjectState.Modified, "Modifying property in 'Unmodified' state should alter object state to 'Modified'.");

            aContext.MarkUnmodified();

            Assert.IsTrue(aContext.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            aContext.Subjects.Remove(aContext.Subjects.First());

            Assert.IsTrue(aContext.ObjectState == ObjectState.Modified, "Modifying property in 'Unmodified' state should alter object state to 'Modified'.");

            aContext.MarkUnmodified();

            Assert.IsTrue(aContext.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            aContext.Subjects.Clear();

            Assert.IsTrue(aContext.ObjectState == ObjectState.Modified, "Modifying property in 'Unmodified' state should alter object state to 'Modified'.");
        }

        [TestMethod]
        public void TestSubjectForModifiedContext()
        {
            EAVContext aContext = new EAVContext();

            Assert.IsTrue(aContext.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.IsTrue(aContext.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            aContext.Name = Guid.NewGuid().ToString();

            Assert.IsTrue(aContext.ObjectState == ObjectState.Modified, "Object was not correctly marked 'Modified'.");

            aContext.Subjects.Add(new EAVSubject());
            aContext.Subjects.Add(new EAVSubject());

            Assert.IsTrue(aContext.Subjects.Count == 2, "Property 'Subjects' was not set correctly.");
            Assert.IsTrue(aContext.ObjectState == ObjectState.Modified, "Modifying property in 'Modified' state should not alter object state.");

            aContext.Subjects.Remove(aContext.Subjects.First());

            Assert.IsTrue(aContext.ObjectState == ObjectState.Modified, "Modifying property in 'Modified' state should not alter object state.");

            aContext.Subjects.Clear();

            Assert.IsTrue(aContext.ObjectState == ObjectState.Modified, "Modifying property in 'Modified' state should not alter object state.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestSubjectForDeletedContextWithAdd()
        {
            EAVContext aContext = new EAVContext();

            Assert.IsTrue(aContext.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.IsTrue(aContext.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            aContext.MarkDeleted();

            Assert.IsTrue(aContext.ObjectState == ObjectState.Deleted, "Object was not correctly marked 'Deleted'.");

            aContext.Subjects.Add(new EAVSubject());
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestSubjectForDeletedContextWithRemove()
        {
            EAVContext aContext = new EAVContext();

            Assert.IsTrue(aContext.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContext.Subjects.Add(new EAVSubject());
            aContext.Subjects.Add(new EAVSubject());

            aContext.MarkUnmodified();

            foreach (EAVSubject subject in aContext.Subjects)
                subject.MarkUnmodified();

            Assert.IsTrue(aContext.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            aContext.MarkDeleted();

            Assert.IsTrue(aContext.ObjectState == ObjectState.Deleted, "Object was not correctly marked 'Deleted'.");

            aContext.Subjects.Remove(aContext.Subjects.First());
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestSubjectForDeletedContextWithClear()
        {
            EAVContext aContext = new EAVContext();

            Assert.IsTrue(aContext.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContext.Subjects.Add(new EAVSubject());

            aContext.MarkUnmodified();

            foreach (EAVSubject subject in aContext.Subjects)
                subject.MarkUnmodified();

            Assert.IsTrue(aContext.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            aContext.MarkDeleted();

            Assert.IsTrue(aContext.ObjectState == ObjectState.Deleted, "Object was not correctly marked 'Deleted'.");

            aContext.Subjects.Clear();
        }

        [TestMethod]
        public void TestSubjectInteractionWithContext()
        {
            EAVContext aContext = new EAVContext();

            int id = rng.Next();
            aContext.ContextID = id;

            EAVSubject aSubject = new EAVSubject();
            aContext.Subjects.Add(aSubject);

            Assert.IsTrue(aContext.Subjects.Count == 1, "Collection property 'Subjects' is empty.");
            Assert.IsTrue(aContext.Subjects.Single() == aSubject, "Collection property 'Subjects' not modified properly.");

            Assert.IsNotNull(aSubject.Context, "Property 'Context' is null.");
            Assert.AreEqual(aContext, aSubject.Context, "Property 'Context' not set properly.");

            Assert.IsNotNull(aSubject.ContextID, "Propert 'ContextID' is null.");
            Assert.AreEqual(id, aSubject.ContextID, "Property 'ContextID' not set properly.");
        }
        #endregion

        #region Containers Property Tests
        [TestMethod]
        public void TestRootContainerForNewContext()
        {
            EAVContext aContext = new EAVContext();

            Assert.IsTrue(aContext.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContext.Containers.Add(new EAVRootContainer());
            aContext.Containers.Add(new EAVRootContainer());

            Assert.IsTrue(aContext.Containers.Count == 2, "Property 'Containers' was not set correctly.");
            Assert.IsTrue(aContext.ObjectState == ObjectState.New, "Modifying property in 'New' state should not alter object state.");

            aContext.Containers.Remove(aContext.Containers.First());

            Assert.IsTrue(aContext.ObjectState == ObjectState.New, "Modifying property in 'New' state should not alter object state.");

            aContext.Containers.Clear();

            Assert.IsTrue(aContext.ObjectState == ObjectState.New, "Modifying property in 'New' state should not alter object state.");
        }

        [TestMethod]
        public void TestRootContainerForUnmodifiedContext()
        {
            EAVContext aContext = new EAVContext();

            Assert.IsTrue(aContext.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.IsTrue(aContext.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            aContext.Containers.Add(new EAVRootContainer());
            aContext.Containers.Add(new EAVRootContainer());

            Assert.IsTrue(aContext.Containers.Count == 2, "Property 'Containers' was not set correctly.");
            Assert.IsTrue(aContext.ObjectState == ObjectState.Modified, "Modifying property in 'Unmodified' state should alter object state to 'Modified'.");

            aContext.MarkUnmodified();

            Assert.IsTrue(aContext.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            aContext.Containers.Remove(aContext.Containers.First());

            Assert.IsTrue(aContext.ObjectState == ObjectState.Modified, "Modifying property in 'Unmodified' state should alter object state to 'Modified'.");

            aContext.MarkUnmodified();

            Assert.IsTrue(aContext.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            aContext.Containers.Clear();

            Assert.IsTrue(aContext.ObjectState == ObjectState.Modified, "Modifying property in 'Unmodified' state should alter object state to 'Modified'.");
        }

        [TestMethod]
        public void TestRootContainerForModifiedContext()
        {
            EAVContext aContext = new EAVContext();

            Assert.IsTrue(aContext.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.IsTrue(aContext.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            aContext.Name = Guid.NewGuid().ToString();

            Assert.IsTrue(aContext.ObjectState == ObjectState.Modified, "Object was not correctly marked 'Modified'.");

            aContext.Containers.Add(new EAVRootContainer());
            aContext.Containers.Add(new EAVRootContainer());

            Assert.IsTrue(aContext.Containers.Count == 2, "Property 'Containers' was not set correctly.");
            Assert.IsTrue(aContext.ObjectState == ObjectState.Modified, "Modifying property in 'Modified' state should not alter object state.");

            aContext.Containers.Remove(aContext.Containers.First());

            Assert.IsTrue(aContext.ObjectState == ObjectState.Modified, "Modifying property in 'Modified' state should not alter object state.");

            aContext.Containers.Clear();

            Assert.IsTrue(aContext.ObjectState == ObjectState.Modified, "Modifying property in 'Modified' state should not alter object state.");
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestRootContainerForDeletedContextWithAdd()
        {
            EAVContext aContext = new EAVContext();

            Assert.IsTrue(aContext.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContext.MarkUnmodified();

            Assert.IsTrue(aContext.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            aContext.MarkDeleted();

            Assert.IsTrue(aContext.ObjectState == ObjectState.Deleted, "Object was not correctly marked 'Deleted'.");

            aContext.Containers.Add(new EAVRootContainer());
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestRootContainerForDeletedContextWithRemove()
        {
            EAVContext aContext = new EAVContext();

            Assert.IsTrue(aContext.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContext.Containers.Add(new EAVRootContainer());
            aContext.Containers.Add(new EAVRootContainer());

            aContext.MarkUnmodified();

            foreach (EAVContainer container in aContext.Containers)
                container.MarkUnmodified();

            Assert.IsTrue(aContext.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            aContext.MarkDeleted();

            Assert.IsTrue(aContext.ObjectState == ObjectState.Deleted, "Object was not correctly marked 'Deleted'.");

            aContext.Containers.Remove(aContext.Containers.First());
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void TestRootContainerForDeletedContextWithClear()
        {
            EAVContext aContext = new EAVContext();

            Assert.IsTrue(aContext.ObjectState == ObjectState.New, "Object state is not 'New' on creation.");

            aContext.Containers.Add(new EAVRootContainer());

            aContext.MarkUnmodified();

            foreach (EAVContainer container in aContext.Containers)
                container.MarkUnmodified();

            Assert.IsTrue(aContext.ObjectState == ObjectState.Unmodified, "Object was not correctly marked 'Unmodified'.");

            aContext.MarkDeleted();

            Assert.IsTrue(aContext.ObjectState == ObjectState.Deleted, "Object was not correctly marked 'Deleted'.");

            aContext.Containers.Clear();
        }

        [TestMethod]
        public void TestRootContainerInteractionWithContext()
        {
            EAVContext aContext = new EAVContext();

            int id = rng.Next();
            aContext.ContextID = id;

            EAVRootContainer aRootContainer = new EAVRootContainer();
            aContext.Containers.Add(aRootContainer);

            Assert.IsTrue(aContext.Containers.Count == 1, "Collection property 'Containers' is empty.");
            Assert.IsTrue(aContext.Containers.Single() == aRootContainer, "Collection property 'Containers' not modified properly.");

            Assert.IsNotNull(aRootContainer.Context, "Property 'Context' is null.");
            Assert.AreEqual(aContext, aRootContainer.Context, "Property 'Context' not set properly.");

            Assert.IsNotNull(aRootContainer.ContextID, "Propert 'ContextID' is null.");
            Assert.AreEqual(id, aRootContainer.ContextID, "Property 'ContextID' not set properly.");
        }
        #endregion
    }
}
