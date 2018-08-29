// MicroEAV
//
// Copyright(C) 2017  Tim Newell

// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with this program.If not, see<http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;


namespace EAVStoreClient
{
    public partial class EAVUnitClient : EAV.Store.IStoreUnitClient
    {
        public IEnumerable<EAV.Store.IStoreUnit> RetrieveUnits()
        {
            using (EAVStoreClient.MicroEAVContext ctx = new MicroEAVContext())
            {
                return (ctx.Units.AsEnumerable().Select(it => (EAV.Store.StoreUnit)it).ToList());
            }
        }

        public EAV.Store.IStoreUnit RetrieveUnit(int unitID)
        {
            using (EAVStoreClient.MicroEAVContext ctx = new MicroEAVContext())
            {
                return ((EAV.Store.StoreUnit)ctx.Units.SingleOrDefault(it => it.Unit_ID == unitID));
            }
        }

        public EAV.Store.IStoreUnit CreateUnit(EAV.Store.IStoreUnit aUnit)
        {
            if (aUnit == null)
                return (null);

            // TODO: Need to check that at least one string property has a non-empty value?

            using (EAVStoreClient.MicroEAVContext ctx = new MicroEAVContext())
            {
                Unit dbUnit = new Unit(aUnit);

                ctx.Units.Add(dbUnit);

                ctx.SaveChanges();

                return ((EAV.Store.StoreUnit)dbUnit);
            }
        }

        public void UpdateUnit(EAV.Store.IStoreUnit aUnit)
        {
            using (EAVStoreClient.MicroEAVContext ctx = new MicroEAVContext())
            {
                EAVStoreClient.Unit dbUnit = ctx.Units.SingleOrDefault(it => it.Unit_ID == aUnit.UnitID);

                if (dbUnit != null)
                {
                    if (dbUnit.Singular_Name != aUnit.SingularName)
                        dbUnit.Singular_Name = aUnit.SingularName;

                    if (dbUnit.Singular_Abbreviation != aUnit.SingularAbbreviation)
                        dbUnit.Singular_Abbreviation = aUnit.SingularAbbreviation;

                    if (dbUnit.Plural_Name != aUnit.PluralName)
                        dbUnit.Plural_Name = aUnit.PluralName;

                    if (dbUnit.Plural_Abbreviation != aUnit.PluralAbbreviation)
                        dbUnit.Plural_Abbreviation = aUnit.PluralAbbreviation;

                    if (dbUnit.Symbol != aUnit.Symbol)
                        dbUnit.Symbol = aUnit.Symbol;

                    if (dbUnit.Display_Text != aUnit.DisplayText)
                        dbUnit.Display_Text = aUnit.DisplayText;

                    if (dbUnit.Curated != aUnit.Curated)
                        dbUnit.Curated = aUnit.Curated;

                    ctx.SaveChanges();
                }
                else
                    throw (new Exception(String.Format("Unable to retrieve Unit ID {0}.", aUnit.UnitID)));
            }
        }

        public void DeleteUnit(int unitID)
        {
            using (EAVStoreClient.MicroEAVContext ctx = new MicroEAVContext())
            {
                var dbUnit = ctx.Units.SingleOrDefault(it => it.Unit_ID == unitID);

                if (dbUnit != null)
                {
                    ctx.Units.Remove(dbUnit);

                    ctx.SaveChanges();
                }
            }
        }
    }
}
