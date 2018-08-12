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
    public partial class EAVUnitClient : EAV.Store.IEAVUnitClient
    {
        public IEnumerable<EAV.Model.IEAVUnit> RetrieveUnits()
        {
            using (EAVStoreClient.MicroEAVContext ctx = new MicroEAVContext())
            {
                return (ctx.Units.AsEnumerable().Select(it => (EAV.Model.BaseEAVUnit)it).ToList());
            }
        }

        public EAV.Model.IEAVUnit RetrieveUnit(int UnitID)
        {
            using (EAVStoreClient.MicroEAVContext ctx = new MicroEAVContext())
            {
                return ((EAV.Model.BaseEAVUnit)ctx.Units.SingleOrDefault(it => it.Unit_ID == UnitID));
            }
        }

        public EAV.Model.IEAVUnit CreateUnit(EAV.Model.IEAVUnit Unit)
        {
            if (Unit == null)
                return (null);

            // TODO: Need to check that at least one string property has a non-empty value?

            using (EAVStoreClient.MicroEAVContext ctx = new MicroEAVContext())
            {
                Unit dbUnit = new Unit(Unit);

                ctx.Units.Add(dbUnit);

                ctx.SaveChanges();

                return ((EAV.Model.BaseEAVUnit)dbUnit);
            }
        }

        public void UpdateUnit(EAV.Model.IEAVUnit Unit)
        {
            using (EAVStoreClient.MicroEAVContext ctx = new MicroEAVContext())
            {
                EAVStoreClient.Unit dbUnit = ctx.Units.SingleOrDefault(it => it.Unit_ID == Unit.UnitID);

                if (dbUnit != null)
                {
                    if (dbUnit.Singular_Name != Unit.SingularName)
                        dbUnit.Singular_Name = Unit.SingularName;

                    if (dbUnit.Singular_Abbreviation != Unit.SingularAbbreviation)
                        dbUnit.Singular_Abbreviation = Unit.SingularAbbreviation;

                    if (dbUnit.Plural_Name != Unit.PluralName)
                        dbUnit.Plural_Name = Unit.PluralName;

                    if (dbUnit.Plural_Abbreviation != Unit.PluralAbbreviation)
                        dbUnit.Plural_Abbreviation = Unit.PluralAbbreviation;

                    if (dbUnit.Symbol != Unit.Symbol)
                        dbUnit.Symbol = Unit.Symbol;

                    if (dbUnit.Display_Text != Unit.DisplayText)
                        dbUnit.Display_Text = Unit.DisplayText;

                    if (dbUnit.Curated != Unit.Curated)
                        dbUnit.Curated = Unit.Curated;

                    ctx.SaveChanges();
                }
                else
                    throw (new Exception(String.Format("Unable to retrieve Unit ID {0}.", Unit.UnitID)));
            }
        }

        public void DeleteUnit(int UnitID)
        {
            using (EAVStoreClient.MicroEAVContext ctx = new MicroEAVContext())
            {
                var dbUnit = ctx.Units.SingleOrDefault(it => it.Unit_ID == UnitID);

                if (dbUnit != null)
                {
                    ctx.Units.Remove(dbUnit);

                    ctx.SaveChanges();
                }
            }
        }
    }
}
