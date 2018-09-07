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
    public class StoreInstance : EAV.Instance, EAV.Store.IStoreInstance
    {
        public StoreInstance() { }

        public StoreInstance(EAV.Store.IStoreInstance instance)
        {
            this.InstanceID = instance.InstanceID;
            this.ParentInstanceID = instance.ParentInstanceID;
            this.SubjectID = instance.SubjectID;
            this.ContainerID = instance.ContainerID;
        }

        public int? InstanceID { get; set; }
        public int? ParentInstanceID { get; set; }
        public int? SubjectID { get; set; }
        public int? ContainerID { get; set; }
    }
}