using System;


namespace EAVStoreLibrary
{
    public class StoreObjectFactory : EAV.Store.IStoreObjectFactory
    {
        public TInterface Create<TInterface>() where TInterface : class
        {
            Type targetInterface = typeof(TInterface);

            if (typeof(EAV.Store.IStoreEntity).IsAssignableFrom(targetInterface))
            {
                return (new StoreEntity() as TInterface);
            }
            else if (typeof(EAV.Store.IStoreContext).IsAssignableFrom(targetInterface))
            {
                return (new StoreContext() as TInterface);
            }
            else if (typeof(EAV.Store.IStoreContainer).IsAssignableFrom(targetInterface))
            {
                return (new StoreContainer() as TInterface);
            }
            else if (typeof(EAV.Store.IStoreAttribute).IsAssignableFrom(targetInterface))
            {
                return (new StoreAttribute() as TInterface);
            }
            else if (typeof(EAV.Store.IStoreUnit).IsAssignableFrom(targetInterface))
            {
                return (new StoreUnit() as TInterface);
            }
            else if (typeof(EAV.Store.IStoreSubject).IsAssignableFrom(targetInterface))
            {
                return (new StoreSubject() as TInterface);
            }
            else if (typeof(EAV.Store.IStoreInstance).IsAssignableFrom(targetInterface))
            {
                return (new StoreInstance() as TInterface);
            }
            else if (typeof(EAV.Store.IStoreValue).IsAssignableFrom(targetInterface))
            {
                return (new StoreValue() as TInterface);
            }

            throw (new NotImplementedException(String.Format("Interface '{0}' is not supported by this factory.", targetInterface.Name)));
        }
    }
}
