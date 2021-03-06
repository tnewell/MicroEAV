﻿// MicroEAV
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

namespace EAVStoreLibrary
{
    public class StoreContainer : EAV.Container, EAV.Store.IStoreContainer
    {
        public StoreContainer() { }

        public StoreContainer(EAV.Store.IStoreContainer container)
        {
            this.ContainerID = container.ContainerID;
            this.ParentContainerID = container.ParentContainerID;
            this.ContextID = container.ContextID;
            this.Name = container.Name;
            this.DataName = container.DataName;
            this.DisplayText = container.DisplayText;
            this.IsRepeating = container.IsRepeating;
        }

        public int? ContainerID { get; set; }
        public int? ParentContainerID { get; set; }
        public int? ContextID { get; set; }
    }
}
