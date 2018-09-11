using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace EAVStoreTest
{
    public partial class EAVStoreTestHarness
    {
        [TestMethod]
        public void CreateEntityFromFactory()
        {
            var obj = factory.Create<EAV.Store.IStoreEntity>();

            Assert.IsNotNull(obj, "Attempt to create object with interface 'EAV.Store.IStoreEntity' failed (result is null).");
            Assert.IsTrue(obj is EAV.Store.IStoreEntity, "Factory returned object with unexpected interface '{0}'. Expected interface was 'EAV.Store.IStoreEntity'.", obj.GetType().Name);
        }

        [TestMethod]
        public void CreateContextFromFactory()
        {
            var obj = factory.Create<EAV.Store.IStoreContext>();

            Assert.IsNotNull(obj, "Attempt to create object with interface 'EAV.Store.IStoreContext' failed (result is null).");
            Assert.IsTrue(obj is EAV.Store.IStoreContext, "Factory returned object with unexpected interface '{0}'. Expected interface was 'EAV.Store.IStoreContext'.", obj.GetType().Name);
        }

        [TestMethod]
        public void CreateContainerFromFactory()
        {
            var obj = factory.Create<EAV.Store.IStoreContainer>();

            Assert.IsNotNull(obj, "Attempt to create object with interface 'EAV.Store.IStoreContainer' failed (result is null).");
            Assert.IsTrue(obj is EAV.Store.IStoreContainer, "Factory returned object with unexpected interface '{0}'. Expected interface was 'EAV.Store.IStoreContainer'.", obj.GetType().Name);
        }

        [TestMethod]
        public void CreateAttributeFromFactory()
        {
            var obj = factory.Create<EAV.Store.IStoreAttribute>();

            Assert.IsNotNull(obj, "Attempt to create object with interface 'EAV.Store.IStoreAttribute' failed (result is null).");
            Assert.IsTrue(obj is EAV.Store.IStoreAttribute, "Factory returned object with unexpected interface '{0}'. Expected interface was 'EAV.Store.IStoreAttribute'.", obj.GetType().Name);
        }

        [TestMethod]
        public void CreateUnitFromFactory()
        {
            var obj = factory.Create<EAV.Store.IStoreUnit>();

            Assert.IsNotNull(obj, "Attempt to create object with interface 'EAV.Store.IStoreUnit' failed (result is null).");
            Assert.IsTrue(obj is EAV.Store.IStoreUnit, "Factory returned object with unexpected interface '{0}'. Expected interface was 'EAV.Store.IStoreUnit'.", obj.GetType().Name);
        }

        [TestMethod]
        public void CreateSubjectFromFactory()
        {
            var obj = factory.Create<EAV.Store.IStoreSubject>();

            Assert.IsNotNull(obj, "Attempt to create object with interface 'EAV.Store.IStoreSubject' failed (result is null).");
            Assert.IsTrue(obj is EAV.Store.IStoreSubject, "Factory returned object with unexpected interface '{0}'. Expected interface was 'EAV.Store.IStoreSubject'.", obj.GetType().Name);
        }

        [TestMethod]
        public void CreateInstanceFromFactory()
        {
            var obj = factory.Create<EAV.Store.IStoreInstance>();

            Assert.IsNotNull(obj, "Attempt to create object with interface 'EAV.Store.IStoreInstance' failed (result is null).");
            Assert.IsTrue(obj is EAV.Store.IStoreInstance, "Factory returned object with unexpected interface '{0}'. Expected interface was 'EAV.Store.IStoreInstance'.", obj.GetType().Name);
        }

        [TestMethod]
        public void CreateValueFromFactory()
        {
            var obj = factory.Create<EAV.Store.IStoreValue>();

            Assert.IsNotNull(obj, "Attempt to create object with interface 'EAV.Store.IStoreValue' failed (result is null).");
            Assert.IsTrue(obj is EAV.Store.IStoreValue, "Factory returned object with unexpected interface '{0}'. Expected interface was 'EAV.Store.IStoreValue'.", obj.GetType().Name);
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void CreateUnknownFromFactory()
        {
            var obj = factory.Create<IComparable>();
        }
    }
}
