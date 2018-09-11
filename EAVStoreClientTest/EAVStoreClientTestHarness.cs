using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace EAVStoreClientTestHarness
{
    [TestClass]
    public partial class EAVStoreClientTestHarness
    {
        private static TestContext myTestContext;
        private static Random rng = new Random((int) DateTime.Now.Ticks);

        private EAVStoreClient.StoreClientFactory factory = new EAVStoreClient.StoreClientFactory();

        private EAVStoreClient.MicroEAVContext dbContext;
        public EAVStoreClient.MicroEAVContext DbContext { get { return (dbContext); } }

        public void ResetDatabaseContext()
        {
            if (dbContext != null)
            {
                dbContext.Dispose();
                dbContext = null;
            }

            dbContext = new EAVStoreClient.MicroEAVContext();
        }

        #region Helper Methods
        private static EAVStoreClient.Entity CreateEntity(string descriptor)
        {
            EAVStoreClient.Entity dbEntity;

            using (EAVStoreClient.MicroEAVContext ctx = new EAVStoreClient.MicroEAVContext())
            {
                dbEntity = ctx.Entities.Add(new EAVStoreClient.Entity()
                {
                    Descriptor = descriptor,
                });

                ctx.SaveChanges();
            }

            return (dbEntity);
        }

        private static EAVStoreClient.Context CreateContext(string name)
        {
            EAVStoreClient.Context dbContext;

            using (EAVStoreClient.MicroEAVContext ctx = new EAVStoreClient.MicroEAVContext())
            {
                dbContext = ctx.Contexts.Add(new EAVStoreClient.Context()
                {
                    Data_Name = name.ToUpper(),
                    Display_Text = name + ":",
                    Name = name,
                });

                ctx.SaveChanges();
            }

            return (dbContext);
        }

        private static EAVStoreClient.Container CreateContainer(int contextID, int? parentContainerID, string name, int sequence, bool isRepeating = false)
        {
            EAVStoreClient.Container dbContainer;

            using (EAVStoreClient.MicroEAVContext ctx = new EAVStoreClient.MicroEAVContext())
            {
                dbContainer = ctx.Containers.Add(new EAVStoreClient.Container()
                {
                    Context_ID = contextID,
                    Data_Name = name.ToUpper(),
                    Display_Text = name + ":",
                    Name = name,
                    Sequence = sequence,
                    Is_Repeating = isRepeating,
                    Parent_Container_ID = parentContainerID
                });

                ctx.SaveChanges();
            }

            return (dbContainer);
        }

        private static EAVStoreClient.Attribute CreateAttribute(int containerID, string name, EAV.EAVDataType dataType, int sequence, bool isKey = false)
        {
            EAVStoreClient.Attribute dbAttribute;

            using (EAVStoreClient.MicroEAVContext ctx = new EAVStoreClient.MicroEAVContext())
            {
                dbAttribute = ctx.Attributes.Add(new EAVStoreClient.Attribute()
                {
                    Container_ID = containerID,
                    Data_Name = name.ToUpper(),
                    Data_Type = ctx.LookupDataType(dataType),
                    Display_Text = name + ":",
                    Name = name,
                    Sequence = sequence,
                    Is_Key = isKey,
                });

                ctx.SaveChanges();
            }

            return (dbAttribute);
        }

        private static EAVStoreClient.Unit CreateUnit(string symbol, string displayText, bool curated = false)
        {
            EAVStoreClient.Unit dbUnit;

            using (EAVStoreClient.MicroEAVContext ctx = new EAVStoreClient.MicroEAVContext())
            {
                dbUnit = ctx.Units.Add(new EAVStoreClient.Unit()
                {
                    Singular_Name = "SN_" + displayText,
                    Singular_Abbreviation = "SA_" + displayText.Substring(0, 4),
                    Plural_Name = "PN_" + displayText,
                    Plural_Abbreviation = "PA_" + displayText.Substring(0,4),
                    Symbol = symbol,
                    Display_Text = displayText,
                    Curated = curated
                });

                ctx.SaveChanges();
            }

            return (dbUnit);
        }

        private static EAVStoreClient.Subject CreateSubject(int contextID, int entityID, string identifier)
        {
            EAVStoreClient.Subject dbSubject;

            using (EAVStoreClient.MicroEAVContext ctx = new EAVStoreClient.MicroEAVContext())
            {
                dbSubject = ctx.Subjects.Add(new EAVStoreClient.Subject()
                {
                    Context_ID = contextID,
                    Identifier = identifier,
                    Entity_ID = entityID,
                });

                ctx.SaveChanges();
            }

            return (dbSubject);
        }

        private static EAVStoreClient.Instance CreateInstance(int containerID, int subjectID, int? parentInstanceID)
        {
            EAVStoreClient.Instance dbInstance;

            using (EAVStoreClient.MicroEAVContext ctx = new EAVStoreClient.MicroEAVContext())
            {
                dbInstance = ctx.Instances.Add(new EAVStoreClient.Instance()
                {
                    Container_ID = containerID,
                    Parent_Instance_ID = parentInstanceID,
                    Subject_ID = subjectID,
                });

                ctx.SaveChanges();
            }

            return (dbInstance);
        }

        private static EAVStoreClient.Value CreateValue(int attributeID, int instanceID, string value, EAVStoreClient.Unit unit = null)
        {
            EAVStoreClient.Value dbValue;

            using (EAVStoreClient.MicroEAVContext ctx = new EAVStoreClient.MicroEAVContext())
            {
                dbValue = ctx.Values.Add(new EAVStoreClient.Value()
                {
                    Attribute_ID = attributeID,
                    Instance_ID = instanceID,
                    Unit = unit,
                    Raw_Value = value,
                });

                ctx.SaveChanges();
            }

            return (dbValue);
        }

        private static T SelectRandomItem<T>(IEnumerable<T> items)
        {
            return (items.AsEnumerable().ElementAtOrDefault(rng.Next(0, items.Count())));
        }

        #endregion

        private static void SeedDatabase()
        {
            using (EAVStoreClient.MicroEAVContext ctx = new EAVStoreClient.MicroEAVContext())
            {
                ctx.Database.ExecuteSqlCommand("DELETE [Value];DELETE [Instance];DBCC CHECKIDENT ([Instance], RESEED, 0);DELETE [Subject];DBCC CHECKIDENT ([Subject], RESEED, 0);DELETE [Unit];DBCC CHECKIDENT ([Unit], RESEED, 0);DELETE [Attribute];DBCC CHECKIDENT ([Attribute], RESEED, 0);DELETE [Container];DBCC CHECKIDENT ([Container], RESEED, 0);DELETE [Context];DBCC CHECKIDENT ([Context], RESEED, 0);DELETE [Entity];DBCC CHECKIDENT ([Entity], RESEED, 0)");

                // Entities
                List<EAVStoreClient.Entity> entities = new List<EAVStoreClient.Entity>();
                for (int i = 0; i < 5; ++i)
                    entities.Add(CreateEntity(String.Format("Entity {0}", i + 1)));

                // Units
                List<EAVStoreClient.Unit> units = new List<EAVStoreClient.Unit>();
                for (int i = 0; i < 5; ++i)
                    units.Add(CreateUnit(String.Format("SYM{0}", i + 1), String.Format("Unit {0}", i + 1)));

                // Contexts
                List<EAVStoreClient.Context> contexts = new List<EAVStoreClient.Context>();
                for (int i = 0; i < 5; ++i)
                    contexts.Add(CreateContext(String.Format("Context {0}", i + 1)));

                // Subjects
                List<EAVStoreClient.Subject> subjects = new List<EAVStoreClient.Subject>();
                for (int i = 0; i < 5; ++i)
                    subjects.Add(CreateSubject(contexts[i].Context_ID, entities[i].Entity_ID, String.Format("Subject {0}", i + 1)));

                EAVStoreClient.Container dbContainer = null;
                EAVStoreClient.Container dbParentContainer = null;
                EAVStoreClient.Instance dbInstance = null;
                EAVStoreClient.Instance dbParentInstance = null;
                Dictionary<EAV.EAVDataType, EAVStoreClient.Attribute> attributes = new Dictionary<EAV.EAVDataType, EAVStoreClient.Attribute>();

                var typeList = new Queue<EAV.EAVDataType>(Enum.GetValues(typeof(EAV.EAVDataType)).OfType<EAV.EAVDataType>());

                // Context 1
                dbContainer = CreateContainer(contexts[0].Context_ID, null, "Root Container 1-1", 1, false);

                attributes.Clear();
                foreach (EAV.EAVDataType dt in Enum.GetValues(typeof(EAV.EAVDataType)))
                    attributes[dt] = CreateAttribute(dbContainer.Container_ID, String.Format("Attribute 1-1-{0}", ((int) dt) + 1), dt, (int) dt, dt == EAV.EAVDataType.String);

                foreach (EAVStoreClient.Subject dbSubject in subjects)
                {
                    dbInstance = CreateInstance(dbContainer.Container_ID, dbSubject.Subject_ID, null);

                    foreach (EAV.EAVDataType dt in typeList.Take(4))
                    {
                        CreateValue(attributes[dt].Attribute_ID, dbInstance.Instance_ID, "N/A", dt == EAV.EAVDataType.Float ? CreateUnit("SYM", "Unit") : null);
                    }

                    typeList.Enqueue(typeList.Dequeue());
                }

                // Context 2
                dbParentContainer = CreateContainer(contexts[1].Context_ID, null, "Root Container 2-1", 2, false);

                attributes.Clear();
                foreach (EAV.EAVDataType dt in Enum.GetValues(typeof(EAV.EAVDataType)))
                    attributes[dt] = CreateAttribute(dbParentContainer.Container_ID, String.Format("Attribute 2-1-{0}", ((int) dt) + 1), dt, (int)dt, dt == EAV.EAVDataType.String);

                foreach (EAVStoreClient.Subject dbSubject in subjects)
                {
                    dbParentInstance = CreateInstance(dbParentContainer.Container_ID, dbSubject.Subject_ID, null);

                    foreach (EAV.EAVDataType dt in typeList.Take(4))
                    {
                        CreateValue(attributes[dt].Attribute_ID, dbParentInstance.Instance_ID, "N/A", dt == EAV.EAVDataType.Float ? CreateUnit("SYM", "Unit") : null);
                    }

                    typeList.Enqueue(typeList.Dequeue());
                }

                dbContainer = CreateContainer(contexts[1].Context_ID, dbParentContainer.Container_ID, "Child Container 2-1-1", 1, false);

                attributes.Clear();
                foreach (EAV.EAVDataType dt in Enum.GetValues(typeof(EAV.EAVDataType)))
                    attributes[dt] = CreateAttribute(dbParentContainer.Container_ID, String.Format("Attribute 2-1-1-{0}", ((int)dt) + 1), dt, (int)dt, dt == EAV.EAVDataType.String);

                foreach (EAVStoreClient.Subject dbSubject in subjects)
                {
                    dbInstance = CreateInstance(dbContainer.Container_ID, dbSubject.Subject_ID, dbParentContainer.Container_ID);

                    foreach (EAV.EAVDataType dt in typeList.Take(4))
                    {
                        CreateValue(attributes[dt].Attribute_ID, dbInstance.Instance_ID, "N/A", dt == EAV.EAVDataType.Float ? CreateUnit("SYM", "Unit") : null);
                    }

                    typeList.Enqueue(typeList.Dequeue());
                }

                dbContainer = CreateContainer(contexts[1].Context_ID, dbParentContainer.Container_ID, "Child Container 2-1-2", 2, true);

                attributes.Clear();
                foreach (EAV.EAVDataType dt in Enum.GetValues(typeof(EAV.EAVDataType)))
                    attributes[dt] = CreateAttribute(dbParentContainer.Container_ID, String.Format("Attribute 2-1-2-{0}", ((int)dt) + 1), dt, (int)dt, dt == EAV.EAVDataType.String);

                foreach (EAVStoreClient.Subject dbSubject in subjects)
                {
                    dbInstance = CreateInstance(dbContainer.Container_ID, dbSubject.Subject_ID, dbParentContainer.Container_ID);

                    foreach (EAV.EAVDataType dt in typeList.Take(4))
                    {
                        CreateValue(attributes[dt].Attribute_ID, dbInstance.Instance_ID, "N/A", dt == EAV.EAVDataType.Float ? CreateUnit("SYM", "Unit") : null);
                    }

                    typeList.Enqueue(typeList.Dequeue());
                }

                // Context 3

                // Context 4

                // Context 5

            }
        }

        [ClassInitialize]
        public static void SetupTestHarness(TestContext testContext)
        {
            myTestContext = testContext;

            int rngSeed = (int) DateTime.Now.Ticks;

            myTestContext.WriteLine("Seeding rng with {0}.", rngSeed);

            rng = new Random(rngSeed);

            SeedDatabase();
        }

        [ClassCleanup]
        public static void TeardownTestHarness()
        {
        }

        [TestInitialize]
        public void SetupTest()
        {
            dbContext = new EAVStoreClient.MicroEAVContext();
        }

        [TestCleanup]
        public void TeardownTest()
        {
            dbContext.Dispose();
            dbContext = null;
        }

        [TestMethod]
        public void Sandbox()
        {
            Assert.IsTrue(true);
        }
    }

    public static class TestExtensions
    {
        public static string Flip(this String value)
        {
            return (new String(value.Reverse().ToArray()));
        }
    }
}
