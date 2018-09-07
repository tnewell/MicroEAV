using System;


namespace EAVModelLibrary
{
    public class ModelObjectFactory : EAV.Model.IModelObjectFactory
    {
        public TInterface Create<TInterface>() where TInterface : class
        {
            Type targetInterface = typeof(TInterface);

            if (typeof(EAV.Model.IModelEntity).IsAssignableFrom(targetInterface))
            {
                return (new ModelEntity() as TInterface);
            }
            else if (typeof(EAV.Model.IModelContext).IsAssignableFrom(targetInterface))
            {
                return (new ModelContext() as TInterface);
            }
            else if (typeof(EAV.Model.IModelRootContainer).IsAssignableFrom(targetInterface))
            {
                return (new ModelRootContainer() as TInterface);
            }
            else if (typeof(EAV.Model.IModelChildContainer).IsAssignableFrom(targetInterface))
            {
                return (new ModelChildContainer() as TInterface);
            }
            else if (typeof(EAV.Model.IModelAttribute).IsAssignableFrom(targetInterface))
            {
                return (new ModelAttribute() as TInterface);
            }
            else if (typeof(EAV.Model.IModelUnit).IsAssignableFrom(targetInterface))
            {
                return (new ModelUnit() as TInterface);
            }
            else if (typeof(EAV.Model.IModelSubject).IsAssignableFrom(targetInterface))
            {
                return (new ModelSubject() as TInterface);
            }
            else if (typeof(EAV.Model.IModelRootInstance).IsAssignableFrom(targetInterface))
            {
                return (new ModelRootInstance() as TInterface);
            }
            else if (typeof(EAV.Model.IModelChildInstance).IsAssignableFrom(targetInterface))
            {
                return (new ModelChildInstance() as TInterface);
            }
            else if (typeof(EAV.Model.IModelValue).IsAssignableFrom(targetInterface))
            {
                return (new ModelValue() as TInterface);
            }

            throw (new NotImplementedException(String.Format("Interface '{0}' is not supported by this factory.", targetInterface.Name)));
        }
    }
}
