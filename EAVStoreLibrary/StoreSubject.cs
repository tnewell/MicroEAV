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

namespace EAVStoreLibrary
{
    public class StoreSubject : EAV.Subject, EAV.Store.IStoreSubject
    {
        public StoreSubject() { }

        public StoreSubject(EAV.Store.IStoreSubject subject)
        {
            this.SubjectID = subject.SubjectID;
            this.EntityID = subject.EntityID;
            this.ContextID = subject.ContextID;
            this.Identifier = subject.Identifier;
        }

        public int? SubjectID { get; set; }
        public int? EntityID { get; set; }
        public int? ContextID { get; set; }
    }
}
