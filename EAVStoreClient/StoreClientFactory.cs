using System;


namespace EAVStoreClient
{
    public class StoreClientFactory : EAV.Store.Clients.IStoreClientFactory
    {
        public TInterface Create<TInterface>() where TInterface : class
        {
            Type targetInterface = typeof(TInterface);

            if (typeof(EAV.Store.Clients.IEntityStoreClient).IsAssignableFrom(targetInterface))
            {
                return (new EntityStoreClient() as TInterface);
            }
            else if (typeof(EAV.Store.Clients.IContextStoreClient).IsAssignableFrom(targetInterface))
            {
                return (new ContextStoreClient() as TInterface);
            }
            else if (typeof(EAV.Store.Clients.IContainerStoreClient).IsAssignableFrom(targetInterface))
            {
                return (new ContainerStoreClient() as TInterface);
            }
            else if (typeof(EAV.Store.Clients.IAttributeStoreClient).IsAssignableFrom(targetInterface))
            {
                return (new AttributeStoreClient() as TInterface);
            }
            else if (typeof(EAV.Store.Clients.IUnitStoreClient).IsAssignableFrom(targetInterface))
            {
                return (new UnitStoreClient() as TInterface);
            }
            else if (typeof(EAV.Store.Clients.ISubjectStoreClient).IsAssignableFrom(targetInterface))
            {
                return (new SubjectStoreClient() as TInterface);
            }
            else if (typeof(EAV.Store.Clients.IInstanceStoreClient).IsAssignableFrom(targetInterface))
            {
                return (new InstanceStoreClient() as TInterface);
            }
            else if (typeof(EAV.Store.Clients.IValueStoreClient).IsAssignableFrom(targetInterface))
            {
                return (new ValueStoreClient() as TInterface);
            }

            throw (new NotImplementedException(String.Format("Interface '{0}' is not supported by this factory.", targetInterface.Name)));
        }
    }
}
