using NgSchoolsDataLayer.Context;
using NgSchoolsDataLayer.Repository.Base;
using NgSchoolsDataLayer.Repository.Contracts;
using NgSchoolsDataLayer.Repository.Implementations;
using System;
using System.Collections.Generic;

namespace NgSchoolsDataLayer.Repository.Provider
{
    internal class RepositoryFactory
    {
        private readonly IDictionary<Type, Func<NgSchoolsContext, object>> _factoryCache;

        public RepositoryFactory()
        {
            _factoryCache = GetFactories();
        }

        public Func<NgSchoolsContext, object> GetRepositoryFactoryForEntityType<T>() where T : class
        {
            Func<NgSchoolsContext, object> factory = GetRepositoryFactoryFromCache<T>();
            if (factory != null)
            {
                return factory;
            }

            return DefaultEntityRepositoryFactory<T>();
        }

        public Func<NgSchoolsContext, object> GetRepositoryFactoryFromCache<T>()
        {
            Func<NgSchoolsContext, object> factory;
            _factoryCache.TryGetValue(typeof(T), out factory);
            return factory;
        }

        private IDictionary<Type, Func<NgSchoolsContext, object>> GetFactories()
        {
            Dictionary<Type, Func<NgSchoolsContext, object>> dic = new Dictionary<Type, Func<NgSchoolsContext, object>>();
            dic.Add(typeof(IUserRepository), context => new UserRepository(context));
            //Add Extended and Custom Repositories here
            return dic;
        }

        private Func<NgSchoolsContext, object> DefaultEntityRepositoryFactory<T>() where T : class
        {
            return dbContext => new RepositoryBase<T>(dbContext);
        }
    }
}
