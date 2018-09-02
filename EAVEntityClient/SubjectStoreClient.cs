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
    public partial class SubjectStoreClient : EAV.Store.Clients.ISubjectStoreClient
    {
        public IEnumerable<EAV.Store.IStoreSubject> RetrieveSubjects(int? contextID, int? entityID)
        {
            using (EAVStoreClient.MicroEAVContext ctx = new MicroEAVContext())
            {
                if (contextID != null && entityID != null)
                {
                    return (ctx.Subjects.Where(it => it.Context_ID == contextID && it.Entity_ID == entityID).AsEnumerable().Select(it => (EAV.Store.StoreSubject)it).ToList());
                }
                else if (contextID != null && entityID == null)
                {
                    return (ctx.Subjects.Where(it => it.Context_ID == contextID).AsEnumerable().Select(it => (EAV.Store.StoreSubject)it).ToList());
                }
                else if (contextID == null && entityID != null)
                {
                    return (ctx.Subjects.Where(it => it.Entity_ID == entityID).AsEnumerable().Select(it => (EAV.Store.StoreSubject)it).ToList());
                }
                else
                {
                    return (ctx.Subjects.AsEnumerable().Select(it => (EAV.Store.StoreSubject)it).ToList());
                }
            }
        }

        public EAV.Store.IStoreSubject RetrieveSubject(int subjectID)
        {
            using (EAVStoreClient.MicroEAVContext ctx = new MicroEAVContext())
            {
                return ((EAV.Store.StoreSubject)ctx.Subjects.SingleOrDefault(it => it.Subject_ID == subjectID));
            }
        }

        public EAV.Store.IStoreSubject CreateSubject(EAV.Store.IStoreSubject subject, int contextID, int entityID)
        {
            if (subject == null)
                return (null);

            using (EAVStoreClient.MicroEAVContext ctx = new MicroEAVContext())
            {
                Subject dbSubject = new Subject(subject);

                dbSubject.Context_ID = contextID;
                dbSubject.Entity_ID = entityID;

                ctx.Subjects.Add(dbSubject);

                ctx.SaveChanges();

                return ((EAV.Store.StoreSubject)dbSubject);
            }
        }

        public void UpdateSubject(EAV.Store.IStoreSubject subject)
        {
            using (EAVStoreClient.MicroEAVContext ctx = new MicroEAVContext())
            {
                EAVStoreClient.Subject dbSubject = ctx.Subjects.SingleOrDefault(it => it.Subject_ID == subject.SubjectID);

                if (dbSubject != null)
                {
                    if (dbSubject.Identifier != subject.Identifier)
                        dbSubject.Identifier = subject.Identifier;

                    ctx.SaveChanges();
                }
                else
                    throw (new Exception(String.Format("Unable to retrieve subject ID {0}.", subject.SubjectID)));
            }
        }

        public void DeleteSubject(int subjectID)
        {
            using (EAVStoreClient.MicroEAVContext ctx = new MicroEAVContext())
            {
                var dbSubject = ctx.Subjects.SingleOrDefault(it => it.Subject_ID == subjectID);

                if (dbSubject != null)
                {
                    ctx.Subjects.Remove(dbSubject);

                    ctx.SaveChanges();
                }
            }
        }
    }
}
