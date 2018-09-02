using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

using EAV.Model;


namespace EAVWebApplication.Models.Data
{
    public class DataModel
    {
        public DataModel()
        {
            contexts = new List<IModelContext>();
        }

        private List<IModelContext> contexts;
        public ICollection<IModelContext> Contexts { get { return (contexts); } set { contexts.Clear(); contexts.AddRange(value); } }

        public int SelectedContextID { get; set; }
        public IModelContext CurrentContext { get { return (contexts.SingleOrDefault(it => it.ContextID == SelectedContextID)); } }

        public int SelectedContainerID { get; set; }
        public IModelRootContainer CurrentContainer { get { return (CurrentContext != null ? CurrentContext.Containers.SingleOrDefault(it => it.ContainerID == SelectedContainerID) : null); } }

        public int SelectedSubjectID { get; set; }
        public IModelSubject CurrentSubject { get { return (CurrentContext != null ? CurrentContext.Subjects.SingleOrDefault(it => it.SubjectID == SelectedSubjectID) : null); } }

        private ViewRootInstance currentInstance;
        public ViewRootInstance CurrentInstance { get { return (currentInstance); } set { currentInstance = value; } }
    }

    public class ViewRootInstance
    {
        private IModelRootInstance eavInstance;

        public ViewRootInstance(IModelRootInstance anInstance) { this.eavInstance = anInstance; }

        public int? InstanceID { get; set; }
        public int? ParentInstanceID { get; set; }
        public int? SubjectID { get; set; }
        public int? ContainerID { get; set; }

        public IModelContainer Container { get { return (eavInstance.Container); } }
        public IModelSubject Subject { get { return (eavInstance.Subject); } }
        public IModelInstance ParentInstannce { get { return (eavInstance.ParentInstance); } }
        public ICollection<IModelChildInstance> ChildInstances { get { return (eavInstance.ChildInstances); } }
    }

    public class ViewChildInstance
    {
        private IModelChildInstance eavInstance;

        public ViewChildInstance(IModelChildInstance anInstance) { this.eavInstance = anInstance; }

        public int? InstanceID { get; set; }
        public int? ParentInstanceID { get; set; }
        public int? SubjectID { get; set; }
        public int? ContainerID { get; set; }

        public IModelContainer Container { get { return (eavInstance.Container); } }
        public IModelSubject Subject { get { return (eavInstance.Subject); } }
        public IModelInstance ParentInstannce { get { return (eavInstance.ParentInstance); } }
        public ICollection<IModelChildInstance> ChildInstances { get { return (eavInstance.ChildInstances); } }
    }

    public class ViewValue : ModelValue
    {
        public new int? InstanceID { get; set; }
        public new int? AttributeID { get; set; }
    }

}