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
    public interface IContainerStoreClient
    {
        IEnumerable<EAV.Store.IStoreContainer> RetrieveRootContainers(int? contextID);

        IEnumerable<EAV.Store.IStoreContainer> RetrieveChildContainers(int? parentContainerID);

        EAV.Store.IStoreContainer RetrieveContainer(int containerID);

        EAV.Store.IStoreContainer CreateRootContainer(EAV.Store.IStoreContainer container, int contextID);

        EAV.Store.IStoreContainer CreateChildContainer(EAV.Store.IStoreContainer container, int parentContainerID);

        void UpdateContainer(EAV.Store.IStoreContainer container);

        void DeleteContainer(int containerID);
    }
}
