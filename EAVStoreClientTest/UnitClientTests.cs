using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace EAVStoreClientTestHarness
{
    public partial class EAVStoreClientTestHarness
    {
        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Retrieve")]
        [TestCategory("Unit")]
        public void RetrieveAllUnits()
        {
            EAVStoreClient.EAVUnitClient client = new EAVStoreClient.EAVUnitClient();

            int nDbUnits = this.DbContext.Units.Count();
            int nClientUnits = client.RetrieveUnits().Count();

            Assert.AreEqual(nDbUnits, nClientUnits, "The number of Units retrieved by the client does not match the number in the database.");
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Retrieve")]
        [TestCategory("Unit")]
        public void RetrieveNonExistentUnit()
        {
            EAVStoreClient.EAVUnitClient client = new EAVStoreClient.EAVUnitClient();

            var Unit = client.RetrieveUnit(-1);

            Assert.IsNull(Unit, "Unexpected Unit object retrieved.");
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Retrieve")]
        [TestCategory("Unit")]
        public void RetrieveRandomUnit()
        {
            var dbUnit = SelectRandomItem(this.DbContext.Units);

            if (dbUnit != null)
            {
                EAVStoreClient.EAVUnitClient client = new EAVStoreClient.EAVUnitClient();

                var Unit = client.RetrieveUnit(dbUnit.Unit_ID);

                Assert.IsNotNull(Unit, "Failed to retrieve Unit {0}.", dbUnit.Unit_ID);
                Assert.AreEqual(dbUnit.Unit_ID, Unit.UnitID, "Unit ID values do not match.");
            }
            else
            {
                Assert.Inconclusive("No Units were found in the database.");
            }
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Create")]
        [TestCategory("Unit")]
        public void CreateUnit()
        {
            EAVStoreClient.EAVUnitClient client = new EAVStoreClient.EAVUnitClient();
            string UnitDisplayText = Guid.NewGuid().ToString();

            EAV.Store.IStoreUnit Unit = client.CreateUnit(new EAV.Store.StoreUnit()
            {
                SingularName = "SN_" + UnitDisplayText,
                SingularAbbreviation = "SA_" + UnitDisplayText.Substring(0, 4),
                PluralName = "PN_" + UnitDisplayText,
                PluralAbbreviation = "PA_" + UnitDisplayText.Substring(0, 4),
                Symbol = "SYM",
                DisplayText = UnitDisplayText,
                Curated = false,
            });

            Assert.IsNotNull(Unit, "Failed to create Unit with display text '{0}'", UnitDisplayText);

            ResetDatabaseContext();

            var dbUnit = this.DbContext.Units.SingleOrDefault(it => it.Unit_ID == Unit.UnitID);

            Assert.IsNotNull(dbUnit, String.Format("Failed to retrieve Unit ID {0} from the database.", Unit.UnitID));

            Assert.AreEqual(Unit.SingularName, dbUnit.Singular_Name, "Property 'SingularName' was not created correctly.");
            Assert.AreEqual(Unit.SingularAbbreviation, dbUnit.Singular_Abbreviation, "Property 'SingularAbbreviation' was not created correctly.");
            Assert.AreEqual(Unit.PluralName, dbUnit.Plural_Name, "Property 'PluralName' was not created correctly.");
            Assert.AreEqual(Unit.PluralAbbreviation, dbUnit.Plural_Abbreviation, "Property 'PluralAbbreviation' was not created correctly.");
            Assert.AreEqual(Unit.Symbol, dbUnit.Symbol, "Property 'Symbol' was not created correctly.");
            Assert.AreEqual(Unit.DisplayText, dbUnit.Display_Text, "Property 'DisplayText' was not created correctly.");
            Assert.AreEqual(Unit.Curated, dbUnit.Curated, "Property 'Curated' was not created correctly.");
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Update")]
        [TestCategory("Unit")]
        public void UpdateUnit()
        {
            var dbUnit = SelectRandomItem(this.DbContext.Units);
            string oldSingularName = dbUnit.Singular_Name;
            string oldSingularAbbreviation = dbUnit.Singular_Abbreviation;
            string oldPluralName = dbUnit.Plural_Name;
            string oldPluralAbbreviation = dbUnit.Plural_Abbreviation;
            string oldSymbol = dbUnit.Symbol;
            string oldDisplayText = dbUnit.Display_Text;
            bool oldCurated = dbUnit.Curated;

            EAVStoreClient.EAVUnitClient client = new EAVStoreClient.EAVUnitClient();

            var Unit = (EAV.Store.StoreUnit)dbUnit;

            Unit.SingularName = oldSingularName.Flip();
            Unit.SingularAbbreviation = oldSingularAbbreviation.Flip();
            Unit.PluralName = oldPluralName.Flip();
            Unit.PluralAbbreviation = oldPluralAbbreviation.Flip();
            Unit.Symbol = oldSymbol.Flip();
            Unit.DisplayText = oldDisplayText.Flip();
            Unit.Curated = !oldCurated;

            client.UpdateUnit(Unit);

            ResetDatabaseContext();

            dbUnit = this.dbContext.Units.Single(it => it.Unit_ID == Unit.UnitID);

            Assert.AreEqual(Unit.SingularName, dbUnit.Singular_Name);
            Assert.AreNotEqual(oldSingularName, dbUnit.Singular_Name);
            Assert.AreEqual(Unit.SingularAbbreviation, dbUnit.Singular_Abbreviation);
            Assert.AreNotEqual(oldSingularAbbreviation, dbUnit.Singular_Abbreviation);
            Assert.AreEqual(Unit.PluralName, dbUnit.Plural_Name);
            Assert.AreNotEqual(oldPluralName, dbUnit.Plural_Name);
            Assert.AreEqual(Unit.PluralAbbreviation, dbUnit.Plural_Abbreviation);
            Assert.AreNotEqual(oldPluralAbbreviation, dbUnit.Plural_Abbreviation);
            Assert.AreEqual(Unit.Symbol, dbUnit.Symbol);
            Assert.AreNotEqual(oldSymbol, dbUnit.Symbol);
            Assert.AreEqual(Unit.DisplayText, dbUnit.Display_Text);
            Assert.AreNotEqual(oldDisplayText, dbUnit.Display_Text);
            Assert.AreEqual(Unit.Curated, dbUnit.Curated);
            Assert.AreNotEqual(oldCurated, dbUnit.Curated);
        }

        [TestMethod]
        [TestCategory("CRUD")]
        [TestCategory("Delete")]
        [TestCategory("Unit")]
        public void DeleteUnit()
        {
            EAVStoreClient.EAVUnitClient client = new EAVStoreClient.EAVUnitClient();
            EAVStoreClient.Unit dbUnitIn = CreateUnit("SYM", Guid.NewGuid().ToString());

            client.DeleteUnit(dbUnitIn.Unit_ID);

            EAVStoreClient.Unit dbUnitOut = this.DbContext.Units.SingleOrDefault(it => it.Unit_ID == dbUnitIn.Unit_ID);

            Assert.IsNull(dbUnitOut, "Failed to delete Unit ID {0} from the database.", dbUnitIn.Unit_ID);
        }
    }
}
