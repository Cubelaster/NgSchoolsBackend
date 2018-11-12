using NgSchoolsDataLayer.Context;
using NgSchoolsDataLayer.Repository.Base;
using System;
using System.Collections.Generic;

namespace NgSchoolsDataLayer.Repository.Provider
{
    internal class RepositoryProvider : IRepositoryProvider
    {
        public NgSchoolsContext DbContext { get; set; }

        private readonly RepositoryFactory _factory;

        protected Dictionary<Type, object> Repositories { get; private set; }

        public RepositoryProvider()
        {
            _factory = new RepositoryFactory();
            Repositories = new Dictionary<Type, object>();
        }

        public IRepositoryBase<T> GetGenericRepository<T>() where T : class
        {
            Func<NgSchoolsContext, object> repositoryFactoryForEntityTypeDelegate = _factory.GetRepositoryFactoryForEntityType<T>();
            return GetCustomRepository<IRepositoryBase<T>>(repositoryFactoryForEntityTypeDelegate);
        }

        public virtual T GetCustomRepository<T>(Func<NgSchoolsContext, object> factory = null) where T : class
        {
            object repository;
            Repositories.TryGetValue(typeof(T), out repository);
            if (repository != null)
            {
                return (T)repository;
            }
            return CreateRepository<T>(factory, DbContext);
        }

        private T CreateRepository<T>(Func<NgSchoolsContext, object> factory, NgSchoolsContext dbContext)
        {
            Func<NgSchoolsContext, object> repositoryFactory;
            if (factory != null)
            {
                repositoryFactory = factory;
            }
            else
            {
                repositoryFactory = _factory.GetRepositoryFactoryFromCache<T>();
            }
            if (repositoryFactory == null)
            {
                throw new NotSupportedException(typeof(T).FullName);
            }
            T repository = (T)repositoryFactory(dbContext);
            Repositories[typeof(T)] = repository;
            return repository;
        }
    }
}
