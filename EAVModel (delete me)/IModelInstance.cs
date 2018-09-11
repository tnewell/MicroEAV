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


namespace EAV.Model
{
    public interface IModelInstance : EAV.Store.IStoreInstance
    {
        ObjectState ObjectState { get; set; }

        void MarkUnmodified();

        void MarkDeleted();

        new int? InstanceID { get; set; }

        IModelInstance ParentInstance { get; set; }

        IModelSubject Subject { get; set; }

        IModelContainer Container { get; set; }

        ICollection<IModelChildInstance> ChildInstances { get; }

        ICollection<IModelValue> Values { get; }
    }

    public interface IModelRootInstance : EAV.Model.IModelInstance
    {
    }

    public interface IModelChildInstance : EAV.Model.IModelInstance
    {
    }
}
