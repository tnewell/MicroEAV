using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace EAVModelTest
{
    public partial class EAVModelTestHarness
    {
        [TestMethod]
        public void CreateEntityFromFactory()
        {
            EAVModelLibrary.ModelObjectFactory factory = new EAVModelLibrary.ModelObjectFactory();

            var obj = factory.Create<EAV.Model.IModelEntity>();

            Assert.IsNotNull(obj, "Attempt to create object with interface 'EAV.Model.IModelEntity' failed (result is null).");
            Assert.IsTrue(obj is EAV.Model.IModelEntity, "Factory returned object with unexpected interface '{0}'. Expected interface was 'EAV.Model.IModelEntity'.", obj.GetType().Name);
        }

        [TestMethod]
        public void CreateContextFromFactory()
        {
            EAVModelLibrary.ModelObjectFactory factory = new EAVModelLibrary.ModelObjectFactory();

            var obj = factory.Create<EAV.Model.IModelContext>();

            Assert.IsNotNull(obj, "Attempt to create object with interface 'EAV.Model.IModelContext' failed (result is null).");
            Assert.IsTrue(obj is EAV.Model.IModelContext, "Factory returned object with unexpected interface '{0}'. Expected interface was 'EAV.Model.IModelContext'.", obj.GetType().Name);
        }

        [TestMethod]
        public void CreateRootContainerFromFactory()
        {
            EAVModelLibrary.ModelObjectFactory factory = new EAVModelLibrary.ModelObjectFactory();

            var obj = factory.Create<EAV.Model.IModelRootContainer>();

            Assert.IsNotNull(obj, "Attempt to create object with interface 'EAV.Model.IModelRootContainer' failed (result is null).");
            Assert.IsTrue(obj is EAV.Model.IModelRootContainer, "Factory returned object with unexpected interface '{0}'. Expected interface was 'EAV.Model.IModelRootContainer'.", obj.GetType().Name);
        }

        [TestMethod]
        public void CreateChildContainerFromFactory()
        {
            EAVModelLibrary.ModelObjectFactory factory = new EAVModelLibrary.ModelObjectFactory();

            var obj = factory.Create<EAV.Model.IModelChildContainer>();

            Assert.IsNotNull(obj, "Attempt to create object with interface 'EAV.Model.IModelChildContainer' failed (result is null).");
            Assert.IsTrue(obj is EAV.Model.IModelChildContainer, "Factory returned object with unexpected interface '{0}'. Expected interface was 'EAV.Model.IModelChildContainer'.", obj.GetType().Name);
        }

        [TestMethod]
        public void CreateAttributeFromFactory()
        {
            EAVModelLibrary.ModelObjectFactory factory = new EAVModelLibrary.ModelObjectFactory();

            var obj = factory.Create<EAV.Model.IModelAttribute>();

            Assert.IsNotNull(obj, "Attempt to create object with interface 'EAV.Model.IModelAttribute' failed (result is null).");
            Assert.IsTrue(obj is EAV.Model.IModelAttribute, "Factory returned object with unexpected interface '{0}'. Expected interface was 'EAV.Model.IModelAttribute'.", obj.GetType().Name);
        }

        [TestMethod]
        public void CreateUnitFromFactory()
        {
            EAVModelLibrary.ModelObjectFactory factory = new EAVModelLibrary.ModelObjectFactory();

            var obj = factory.Create<EAV.Model.IModelUnit>();

            Assert.IsNotNull(obj, "Attempt to create object with interface 'EAV.Model.IModelUnit' failed (result is null).");
            Assert.IsTrue(obj is EAV.Model.IModelUnit, "Factory returned object with unexpected interface '{0}'. Expected interface was 'EAV.Model.IModelUnit'.", obj.GetType().Name);
        }

        [TestMethod]
        public void CreateSubjectFromFactory()
        {
            EAVModelLibrary.ModelObjectFactory factory = new EAVModelLibrary.ModelObjectFactory();

            var obj = factory.Create<EAV.Model.IModelSubject>();

            Assert.IsNotNull(obj, "Attempt to create object with interface 'EAV.Model.IModelSubject' failed (result is null).");
            Assert.IsTrue(obj is EAV.Model.IModelSubject, "Factory returned object with unexpected interface '{0}'. Expected interface was 'EAV.Model.IModelSubject'.", obj.GetType().Name);
        }

        [TestMethod]
        public void CreateRootInstanceFromFactory()
        {
            EAVModelLibrary.ModelObjectFactory factory = new EAVModelLibrary.ModelObjectFactory();

            var obj = factory.Create<EAV.Model.IModelRootInstance>();

            Assert.IsNotNull(obj, "Attempt to create object with interface 'EAV.Model.IModelRootInstance' failed (result is null).");
            Assert.IsTrue(obj is EAV.Model.IModelRootInstance, "Factory returned object with unexpected interface '{0}'. Expected interface was 'EAV.Model.IModelRootInstance'.", obj.GetType().Name);
        }

        [TestMethod]
        public void CreateChildInstanceFromFactory()
        {
            EAVModelLibrary.ModelObjectFactory factory = new EAVModelLibrary.ModelObjectFactory();

            var obj = factory.Create<EAV.Model.IModelChildInstance>();

            Assert.IsNotNull(obj, "Attempt to create object with interface 'EAV.Model.IModelChildInstance' failed (result is null).");
            Assert.IsTrue(obj is EAV.Model.IModelChildInstance, "Factory returned object with unexpected interface '{0}'. Expected interface was 'EAV.Model.IModelChildInstance'.", obj.GetType().Name);
        }

        [TestMethod]
        public void CreateValueFromFactory()
        {
            EAVModelLibrary.ModelObjectFactory factory = new EAVModelLibrary.ModelObjectFactory();

            var obj = factory.Create<EAV.Model.IModelValue>();

            Assert.IsNotNull(obj, "Attempt to create object with interface 'EAV.Model.IModelValue' failed (result is null).");
            Assert.IsTrue(obj is EAV.Model.IModelValue, "Factory returned object with unexpected interface '{0}'. Expected interface was 'EAV.Model.IModelValue'.", obj.GetType().Name);
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void CreateUnknownFromFactory()
        {
            EAVModelLibrary.ModelObjectFactory factory = new EAVModelLibrary.ModelObjectFactory();

            var obj = factory.Create<IComparable>();
        }
    }
}
