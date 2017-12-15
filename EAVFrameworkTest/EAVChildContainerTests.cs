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
        public void ChildContainerCreate()
        {
            EAVChildContainer aChildContainer = new EAVChildContainer();

            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            Assert.IsNull(aChildContainer.ContainerID, "Property 'ContainerID' should be null on creation.");

            Assert.IsNull(aChildContainer.Name, "Property 'Name' should be null on creation.");
            Assert.IsNull(aChildContainer.DataName, "Property 'DataName' should be null on creation.");
            Assert.IsNull(aChildContainer.DisplayText, "Property 'DisplayText' should be null on creation.");
            Assert.IsFalse(aChildContainer.IsRepeating, "Property 'IsRepeating' should be false on creation.");

            Assert.IsNull(aChildContainer.Context, "Property 'Context' should be null on creation.");
            Assert.IsNull(aChildContainer.ParentContainer, "Property 'ParentContainer' should be null on creation.");

            Assert.IsNotNull(aChildContainer.ChildContainers, "Property 'ChildContainers' should not be null on creation.");
            Assert.IsFalse(aChildContainer.ChildContainers.Any(), "Property 'ChildContainers' should be empty on creation.");

            Assert.IsNotNull(aChildContainer.Attributes, "Property 'Attributes' should not be null on creation.");
            Assert.IsFalse(aChildContainer.Attributes.Any(), "Property 'Attributes' should be empty on creation.");
        }

        #region State Transitions
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChildContainerStateTransitionNewToUnmodifiedWithNullID()
        {
            EAVChildContainer aChildContainer = new EAVChildContainer();

            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            aChildContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aChildContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        public void ChildContainerStateTransitionNewToUnmodifiedWithValidID()
        {
            EAVChildContainer aChildContainer = new EAVChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            aChildContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aChildContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChildContainerStateTransitionNewToDeleted()
        {
            EAVChildContainer aChildContainer = new EAVChildContainer();

            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            aChildContainer.MarkDeleted();
        }

        [TestMethod]
        public void ChildContainerStateTransitionUnmodifiedToDeleted()
        {
            EAVChildContainer aChildContainer = new EAVChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            aChildContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aChildContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aChildContainer.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aChildContainer.ObjectState, "Object state failed to transition to 'Deleted'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChildContainerStateTransitionDeletedToUnmodified()
        {
            EAVChildContainer aChildContainer = new EAVChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            aChildContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aChildContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aChildContainer.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aChildContainer.ObjectState, "Object state failed to transition to 'Deleted'.");

            aChildContainer.MarkUnmodified();
        }
        #endregion

        #region ID Property
        [TestMethod]
        public void ChildContainerSetIDWhenNew()
        {
            EAVChildContainer aChildContainer = new EAVChildContainer();

            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            int id = rng.Next();
            aChildContainer.ContainerID = id;

            Assert.AreEqual(id, aChildContainer.ContainerID, "Property 'ContainerID' not properly set.");
            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state after setting property 'ContainerID' should be 'New'.");
        }

        [TestMethod]
        public void ChildContainerSetIDBeforeUnmodified()
        {
            EAVChildContainer aChildContainer = new EAVChildContainer();

            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            int id = rng.Next();
            aChildContainer.ContainerID = id;

            Assert.AreEqual(id, aChildContainer.ContainerID, "Property 'ContainerID' not properly set.");
            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state after setting property 'ContainerID' should be 'New'.");

            aChildContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aChildContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChildContainerSetIDAfterUnmodified()
        {
            EAVChildContainer aChildContainer = new EAVChildContainer();

            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            aChildContainer.MarkUnmodified();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChildContainerSetIDWhenDeleted()
        {
            EAVChildContainer aChildContainer = new EAVChildContainer();

            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            int id = rng.Next();
            aChildContainer.ContainerID = id;

            Assert.AreEqual(id, aChildContainer.ContainerID, "Property 'ContainerID' not properly set.");
            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state after setting property 'ContainerID' should be 'New'.");

            aChildContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aChildContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aChildContainer.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aChildContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            id = rng.Next();
            aChildContainer.ContainerID = id;
        }
        #endregion

        #region Primitive Properties
        #region Name
        [TestMethod]
        public void ChildContainerSetNameWhenNew()
        {
            EAVChildContainer aChildContainer = new EAVChildContainer();

            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            string value = Guid.NewGuid().ToString();
            aChildContainer.Name = value;

            Assert.AreEqual(value, aChildContainer.Name, "Property 'Name' was not set properly.");
            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ChildContainerSetNameWhenUnmodified()
        {
            EAVChildContainer aChildContainer = new EAVChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            aChildContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aChildContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            aChildContainer.Name = value;

            Assert.AreEqual(value, aChildContainer.Name, "Property 'Name' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aChildContainer.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void ChildContainerSetNameWhenModified()
        {
            EAVChildContainer aChildContainer = new EAVChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            aChildContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aChildContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            aChildContainer.Name = value;

            Assert.AreEqual(value, aChildContainer.Name, "Property 'Name' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aChildContainer.ObjectState, "Object state failed to transition to 'Modified'.");

            value = Guid.NewGuid().ToString();
            aChildContainer.Name = value;

            Assert.AreEqual(value, aChildContainer.Name, "Property 'Name' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aChildContainer.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChildContainerSetNameWhenDeleted()
        {
            EAVChildContainer aChildContainer = new EAVChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            aChildContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aChildContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aChildContainer.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aChildContainer.ObjectState, "Object state failed to transition to 'Deleted'.");

            aChildContainer.Name = Guid.NewGuid().ToString();
        }
        #endregion

        #region DataName
        [TestMethod]
        public void ChildContainerSetDataNameWhenNew()
        {
            EAVChildContainer aChildContainer = new EAVChildContainer();

            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            string value = Guid.NewGuid().ToString();
            aChildContainer.DataName = value;

            Assert.AreEqual(value, aChildContainer.DataName, "Property 'DataName' was not set properly.");
            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ChildContainerSetDataNameWhenUnmodified()
        {
            EAVChildContainer aChildContainer = new EAVChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            aChildContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aChildContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            aChildContainer.DataName = value;

            Assert.AreEqual(value, aChildContainer.DataName, "Property 'DataName' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aChildContainer.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void ChildContainerSetDataNameWhenModified()
        {
            EAVChildContainer aChildContainer = new EAVChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            aChildContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aChildContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            aChildContainer.DataName = value;

            Assert.AreEqual(value, aChildContainer.DataName, "Property 'DataName' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aChildContainer.ObjectState, "Object state failed to transition to 'Modified'.");

            value = Guid.NewGuid().ToString();
            aChildContainer.DataName = value;

            Assert.AreEqual(value, aChildContainer.DataName, "Property 'DataName' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aChildContainer.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChildContainerSetDataNameWhenDeleted()
        {
            EAVChildContainer aChildContainer = new EAVChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            aChildContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aChildContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aChildContainer.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aChildContainer.ObjectState, "Object state failed to transition to 'Deleted'.");

            aChildContainer.DataName = Guid.NewGuid().ToString();
        }
        #endregion

        #region DisplayText
        [TestMethod]
        public void ChildContainerSetDisplayTextWhenNew()
        {
            EAVChildContainer aChildContainer = new EAVChildContainer();

            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            string value = Guid.NewGuid().ToString();
            aChildContainer.DisplayText = value;

            Assert.AreEqual(value, aChildContainer.DisplayText, "Property 'DisplayText' was not set properly.");
            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ChildContainerSetDisplayTextWhenUnmodified()
        {
            EAVChildContainer aChildContainer = new EAVChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            aChildContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aChildContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            aChildContainer.DisplayText = value;

            Assert.AreEqual(value, aChildContainer.DisplayText, "Property 'DisplayText' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aChildContainer.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void ChildContainerSetDisplayTextWhenModified()
        {
            EAVChildContainer aChildContainer = new EAVChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            aChildContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aChildContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            string value = Guid.NewGuid().ToString();
            aChildContainer.DisplayText = value;

            Assert.AreEqual(value, aChildContainer.DisplayText, "Property 'DisplayText' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aChildContainer.ObjectState, "Object state failed to transition to 'Modified'.");

            value = Guid.NewGuid().ToString();
            aChildContainer.DisplayText = value;

            Assert.AreEqual(value, aChildContainer.DisplayText, "Property 'DisplayText' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aChildContainer.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChildContainerSetDisplayTextWhenDeleted()
        {
            EAVChildContainer aChildContainer = new EAVChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            aChildContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aChildContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aChildContainer.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aChildContainer.ObjectState, "Object state failed to transition to 'Deleted'.");

            aChildContainer.DisplayText = Guid.NewGuid().ToString();
        }
        #endregion

        #region IsRepeating
        [TestMethod]
        public void ChildContainerSetIsRepeatingWhenNew()
        {
            EAVChildContainer aChildContainer = new EAVChildContainer();

            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            aChildContainer.IsRepeating = true;

            Assert.AreEqual(true, aChildContainer.IsRepeating, "Property 'IsRepeating' was not set properly.");
            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ChildContainerSetIsRepeatingWhenUnmodified()
        {
            EAVChildContainer aChildContainer = new EAVChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            aChildContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aChildContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aChildContainer.IsRepeating = true;

            Assert.AreEqual(true, aChildContainer.IsRepeating, "Property 'IsRepeating' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aChildContainer.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void ChildContainerSetIsRepeatingWhenModified()
        {
            EAVChildContainer aChildContainer = new EAVChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            aChildContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aChildContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aChildContainer.IsRepeating = true;

            Assert.AreEqual(true, aChildContainer.IsRepeating, "Property 'IsRepeating' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aChildContainer.ObjectState, "Object state failed to transition to 'Modified'.");

            aChildContainer.IsRepeating = false;

            Assert.AreEqual(false, aChildContainer.IsRepeating, "Property 'IsRepeating' was not set properly.");
            Assert.AreEqual(ObjectState.Modified, aChildContainer.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChildContainerSetIsRepeatingWhenDeleted()
        {
            EAVChildContainer aChildContainer = new EAVChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            aChildContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aChildContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aChildContainer.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aChildContainer.ObjectState, "Object state failed to transition to 'Deleted'.");

            aChildContainer.IsRepeating = true;
        }
        #endregion
        #endregion

        #region Object Properties
        #region Context
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChildContainerSetContextWhenNew()
        {
            EAVChildContainer aChildContainer = new EAVChildContainer();

            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            aChildContainer.Context = new EAVContext() { ContextID = rng.Next() };
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChildContainerSetContextWhenUnmodified()
        {
            EAVChildContainer aChildContainer = new EAVChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            aChildContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aChildContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aChildContainer.Context = new EAVContext() { ContextID = rng.Next() };
        }

        // Set When Modified - N/A

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChildContainerSetContextWhenDeleted()
        {
            EAVChildContainer aChildContainer = new EAVChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            aChildContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aChildContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aChildContainer.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aChildContainer.ObjectState, "Object state failed to transition to 'Deleted'.");

            aChildContainer.Context = new EAVContext() { ContextID = rng.Next() };
        }
        #endregion

        #region ParentContainer
        [TestMethod]
        public void ChildContainerSetParentContainerWithObjectWhenNew()
        {
            EAVChildContainer aChildContainer = new EAVChildContainer();

            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            EAVRootContainer aContainer = new EAVRootContainer() { ContainerID = rng.Next() };
            aChildContainer.ParentContainer = aContainer;

            Assert.AreEqual(aContainer, aChildContainer.ParentContainer, "Property 'ParentContainer' was not set properly.");
            Assert.AreEqual(aContainer.ContainerID, aChildContainer.ParentContainerID, "Property 'ParentContainerID' was not reported properly.");
            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state should remain 'New' when property set.");
        }

        [TestMethod]
        public void ChildContainerSetParentContainerWithObjectWhenUnmodified()
        {
            EAVChildContainer aChildContainer = new EAVChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            aChildContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aChildContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVRootContainer aContainer = new EAVRootContainer() { ContainerID = rng.Next() };
            aChildContainer.ParentContainer = aContainer;

            Assert.AreEqual(aContainer, aChildContainer.ParentContainer, "Property 'ParentContainer' was not set properly.");
            Assert.AreEqual(aContainer.ContainerID, aChildContainer.ParentContainerID, "Property 'ParentContainerID' was not reported properly.");
            Assert.AreEqual(ObjectState.Modified, aChildContainer.ObjectState, "Object state failed to transition to 'Modified'.");
        }

        [TestMethod]
        public void ChildContainerSetParentContainerWithObjectWhenModified()
        {
            EAVChildContainer aChildContainer = new EAVChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            aChildContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aChildContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            EAVRootContainer aContainer = new EAVRootContainer() { ContainerID = rng.Next() };
            aChildContainer.ParentContainer = aContainer;

            Assert.AreEqual(aContainer, aChildContainer.ParentContainer, "Property 'ParentContainer' was not set properly.");
            Assert.AreEqual(aContainer.ContainerID, aChildContainer.ParentContainerID, "Property 'ParentContainerID' was not reported properly.");
            Assert.AreEqual(ObjectState.Modified, aChildContainer.ObjectState, "Object state failed to transition to 'Modified'.");

            aContainer = new EAVRootContainer() { ContainerID = rng.Next() };
            aChildContainer.ParentContainer = aContainer;

            Assert.AreEqual(aContainer, aChildContainer.ParentContainer, "Property 'ParentContainer' was not set properly.");
            Assert.AreEqual(aContainer.ContainerID, aChildContainer.ParentContainerID, "Property 'ParentContainerID' was not reported properly.");
            Assert.AreEqual(ObjectState.Modified, aChildContainer.ObjectState, "Object state should remain 'Modified' when property set.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ChildContainerSetParentContainerWithObjectWhenDeleted()
        {
            EAVChildContainer aChildContainer = new EAVChildContainer() { ContainerID = rng.Next() };

            Assert.AreEqual(ObjectState.New, aChildContainer.ObjectState, "Object state should be 'New' on creation.");

            aChildContainer.MarkUnmodified();

            Assert.AreEqual(ObjectState.Unmodified, aChildContainer.ObjectState, "Object state failed to transition to 'Unmodified'.");

            aChildContainer.MarkDeleted();

            Assert.AreEqual(ObjectState.Deleted, aChildContainer.ObjectState, "Object state failed to transition to 'Deleted'.");

            aChildContainer.ParentContainer = new EAVRootContainer() { ContainerID = rng.Next() };
        }
        #endregion
        #endregion

        #region Collection Properties
        //    Add When New
        //    Add When Unmodified
        //        Add When Modified
        //    Add When Deleted


        //    Remove When New
        //    Remove When Unmodified
        //        Remove When Modified
        //    Remove When Deleted


        //    Clear When New
        //    Clear When Unmodified
        //        Clear When Modified
        //    Clear When Deleted
        #endregion
    }
}
