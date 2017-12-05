﻿using System.Collections.Generic;


namespace EAV.Store
{
    public interface IEAVEntityClient
    {
        IEnumerable<EAV.Model.IEAVEntity> RetrieveEntities();

        EAV.Model.IEAVEntity RetrieveEntity(int entityID);

        EAV.Model.IEAVEntity CreateEntity(EAV.Model.IEAVEntity entity);

        void UpdateEntity(EAV.Model.IEAVEntity entity);

        void DeleteEntity(int entityID);
    }

    public interface IEAVContextClient
    {
        IEnumerable<EAV.Model.IEAVContext> RetrieveContexts();

        EAV.Model.IEAVContext RetrieveContext(int contextID);

        EAV.Model.IEAVContext RetrieveContext(string name);

        EAV.Model.IEAVContext CreateContext(EAV.Model.IEAVContext context);

        void UpdateContext(EAV.Model.IEAVContext context);

        void DeleteContext(int contextID);
    }

    public interface IEAVContainerClient
    {
        IEnumerable<EAV.Model.IEAVContainer> RetrieveRootContainers(int? contextID);

        IEnumerable<EAV.Model.IEAVContainer> RetrieveChildContainers(int? parentContainerID);

        EAV.Model.IEAVContainer RetrieveContainer(int containerID);

        EAV.Model.IEAVContainer CreateRootContainer(EAV.Model.IEAVContainer container, int contextID);

        EAV.Model.IEAVContainer CreateChildContainer(EAV.Model.IEAVContainer container, int parentContainerID);

        void UpdateContainer(EAV.Model.IEAVContainer container);

        void DeleteContainer(int containerID);
    }

    public interface IEAVAttributeClient
    {
        IEnumerable<EAV.Model.IEAVAttribute> RetrieveAttributes(int? containerID);

        EAV.Model.IEAVAttribute RetrieveAttribute(int attributeID);

        EAV.Model.IEAVAttribute CreateAttribute(EAV.Model.IEAVAttribute attribute, int containerID);

        void UpdateAttribute(EAV.Model.IEAVAttribute attribute);

        void DeleteAttribute(int attributeID);
    }

    public interface IEAVSubjectClient
    {
        IEnumerable<EAV.Model.IEAVSubject> RetrieveSubjects(int? contextID, int? entityID);

        EAV.Model.IEAVSubject RetrieveSubject(int subjectID);

        EAV.Model.IEAVSubject CreateSubject(EAV.Model.IEAVSubject subject, int contextID, int entityID);

        void UpdateSubject(EAV.Model.IEAVSubject subject);

        void DeleteSubject(int subjectID);
    }

    public interface IEAVInstanceClient
    {
        IEnumerable<EAV.Model.IEAVInstance> RetrieveRootInstances(int? containerID, int? subjectID);

        IEnumerable<EAV.Model.IEAVInstance> RetrieveChildInstances(int? containerID, int? parentInstanceID);

        EAV.Model.IEAVInstance RetrieveInstance(int instanceID);

        EAV.Model.IEAVInstance CreateRootInstance(EAV.Model.IEAVInstance instance, int containerID, int subjectID);

        EAV.Model.IEAVInstance CreateChildInstance(EAV.Model.IEAVInstance instance, int containerID, int parentInstanceID);

        void UpdateInstance(EAV.Model.IEAVInstance instance);

        void DeleteInstance(int instanceID);
    }

    public interface IEAVValueClient
    {
        IEnumerable<EAV.Model.IEAVValue> RetrieveValues(int? attributeID, int? instanceID);

        EAV.Model.IEAVValue RetrieveValue(int attributeID, int instanceID);

        EAV.Model.IEAVValue CreateValue(EAV.Model.IEAVValue value, int instanceID, int attributeID);

        void UpdateValue(EAV.Model.IEAVValue value);

        void DeleteValue(int attributeID, int instanceID);
    }
}