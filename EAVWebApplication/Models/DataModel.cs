using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

using EAVFramework.Model;


namespace EAVWebApplication.Models.Data
{
    public class DataModel
    {
        public DataModel()
        {
            contexts = new List<EAVContext>();
        }

        private List<EAVContext> contexts;
        public ICollection<EAVContext> Contexts { get { return (contexts); } set { contexts.Clear(); contexts.AddRange(value); } }

        public int SelectedContextID { get; set; }
        public EAVContext CurrentContext { get { return (contexts.SingleOrDefault(it => it.ContextID == SelectedContextID)); } }

        public int SelectedContainerID { get; set; }
        public EAVRootContainer CurrentContainer { get { return (CurrentContext != null ? CurrentContext.Containers.SingleOrDefault(it => it.ContainerID == SelectedContainerID) : null); } }

        public int SelectedSubjectID { get; set; }
        public EAVSubject CurrentSubject { get { return (CurrentContext != null ? CurrentContext.Subjects.SingleOrDefault(it => it.SubjectID == SelectedSubjectID) : null); } }

        private ViewRootInstance currentInstance;
        public ViewRootInstance CurrentInstance { get { return (currentInstance); } set { currentInstance = value; } }
    }

    public class ViewRootInstance
    {
        private EAVRootInstance eavInstance;

        public int? InstanceID { get; set; }
        public int? ParentInstanceID { get; set; }
        public int? SubjectID { get; set; }
        public int? ContainerID { get; set; }

        public EAVContainer Container { get { return (eavInstance.Container); } }
        public EAVSubject Subject { get { return (eavInstance.Subject); } }
        public EAVInstance ParentInstannce { get { return (eavInstance.ParentInstance); } }
        public ICollection<EAVChildInstance> ChildInstances { get { return (eavInstance.ChildInstances); } }
    }

    public class ViewChildInstance
    {
        private EAVChildInstance eavInstance;

        public int? InstanceID { get; set; }
        public int? ParentInstanceID { get; set; }
        public int? SubjectID { get; set; }
        public int? ContainerID { get; set; }

        public EAVContainer Container { get { return (eavInstance.Container); } }
        public EAVSubject Subject { get { return (eavInstance.Subject); } }
        public EAVInstance ParentInstannce { get { return (eavInstance.ParentInstance); } }
        public ICollection<EAVChildInstance> ChildInstances { get { return (eavInstance.ChildInstances); } }
    }

    public class ViewValue : EAVValue
    {
        public new int? InstanceID { get; set; }
        public new int? AttributeID { get; set; }
    }

}