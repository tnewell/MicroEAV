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


namespace EAV.Model.Clients
{
    public interface IModelClient
    {
        IEnumerable<IModelUnit> LoadUnits();

        void SaveUnit(IModelUnit unit);

        IEnumerable<IModelEntity> LoadEntities();

        void SaveEntity(IModelEntity entity);

        IEnumerable<IModelContext> LoadContexts();

        void SaveContext(IModelContext context);

        void LoadSubjects(IModelContext context);

        void LoadSubjects(IModelEntity entity);

        void SaveSubject(IModelSubject subject);

        void LoadRootContainers(IModelContext context);

        void LoadMetadata(IModelRootContainer container);

        void SaveMetadata(IModelRootContainer container);

        void LoadRootInstances(IModelSubject subject, IModelRootContainer container);

        void LoadData(IModelRootInstance instance);

        void SaveData(IModelRootInstance instance);
    }
}

