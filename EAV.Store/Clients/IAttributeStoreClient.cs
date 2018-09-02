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

using System.Collections.Generic;

namespace EAV.Store.Clients
{
    public interface IAttributeStoreClient
    {
        IEnumerable<EAV.Store.IStoreAttribute> RetrieveAttributes(int? containerID);

        EAV.Store.IStoreAttribute RetrieveAttribute(int attributeID);

        EAV.Store.IStoreAttribute CreateAttribute(EAV.Store.IStoreAttribute attribute, int containerID);

        void UpdateAttribute(EAV.Store.IStoreAttribute attribute);

        void DeleteAttribute(int attributeID);

        IEnumerable<EAV.Store.IStoreUnit> RetrieveAttributeUnits(int attributeID);

        void UpdateAttributeUnits(int attributeID, IEnumerable<EAV.Store.IStoreUnit> units);
    }
}
