// MicroEAV
//
// Copyright(C) 2017  Tim Newell

// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty ofEAV.Store.
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with this program.If not, see<http://www.gnu.org/licenses/>.

using System.Runtime.Serialization;


namespace EAVModelLibrary
{
    [DataContract(IsReference = true)]
    [KnownType(typeof(ModelUnit))]
    [KnownType(typeof(ModelEntity))]
    [KnownType(typeof(ModelContext))]
    [KnownType(typeof(ModelContainer))]
    [KnownType(typeof(ModelAttribute))]
    [KnownType(typeof(ModelSubject))]
    [KnownType(typeof(ModelInstance))]
    [KnownType(typeof(ModelValue))]
    public abstract class ModelObject
    {
        public ModelObject() { }

        [DataMember()]
        public EAV.Model.ObjectState ObjectState { get; set; }

        public abstract void MarkUnmodified();

        public abstract void MarkDeleted();
    }
}
