using System;
using System.Collections.Generic;
using System.Linq;


namespace EAVStoreClient
{
    public partial class EAVSubjectClient : EAV.Store.IEAVSubjectClient
    {
        public IEnumerable<EAV.Model.IEAVSubject> RetrieveSubjects(int? contextID, int? entityID)
        {
            using (EAVStoreClient.MicroEAVContext ctx = new MicroEAVContext())
            {
                if (contextID != null && entityID != null)
                {
                    return (ctx.Subjects.Where(it => it.Context_ID == contextID && it.Entity_ID == entityID).AsEnumerable().Select(it => (EAV.Model.BaseEAVSubject)it).ToList());
                }
                else if (contextID != null && entityID == null)
                {
                    return (ctx.Subjects.Where(it => it.Context_ID == contextID).AsEnumerable().Select(it => (EAV.Model.BaseEAVSubject)it).ToList());
                }
                else if (contextID == null && entityID != null)
                {
                    return (ctx.Subjects.Where(it => it.Entity_ID == entityID).AsEnumerable().Select(it => (EAV.Model.BaseEAVSubject)it).ToList());
                }
                else
                {
                    return (ctx.Subjects.AsEnumerable().Select(it => (EAV.Model.BaseEAVSubject)it).ToList());
                }
            }
        }

        public EAV.Model.IEAVSubject RetrieveSubject(int subjectID)
        {
            using (EAVStoreClient.MicroEAVContext ctx = new MicroEAVContext())
            {
                return ((EAV.Model.BaseEAVSubject)ctx.Subjects.SingleOrDefault(it => it.Subject_ID == subjectID));
            }
        }

        public EAV.Model.IEAVSubject CreateSubject(EAV.Model.IEAVSubject subject, int contextID, int entityID)
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

                return ((EAV.Model.BaseEAVSubject)dbSubject);
            }
        }

        public void UpdateSubject(EAV.Model.IEAVSubject subject)
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
