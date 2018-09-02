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

namespace EAV.Store
{
    public class StoreUnit : EAV.Unit, EAV.Store.IStoreUnit
    {
        public StoreUnit() { }

        public StoreUnit(EAV.Store.IStoreUnit unit)
        {
            this.UnitID = unit.UnitID;
            this.SingularName = unit.SingularName;
            this.SingularAbbreviation = unit.SingularAbbreviation;
            this.PluralName = unit.PluralName;
            this.PluralAbbreviation = unit.PluralAbbreviation;
            this.Symbol = unit.Symbol;
            this.Curated = unit.Curated;
        }

        public int? UnitID { get; set; }
    }
}
