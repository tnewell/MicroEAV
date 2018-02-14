// MicroEAV - EAV.Store - Class interfaces to be used as a guide for implementing store clients for the MicroEAV database.
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

/// <summary>
/// This namespace contains interfaces that can be used as a guide for implementing store clients for the MicroEAV database.
/// They are intended to capture the basic CRUD operations necessary to move data in and out of the database.
/// Notably they are also conceived in a way so as to support RESTful operations easily.
/// </summary>
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
