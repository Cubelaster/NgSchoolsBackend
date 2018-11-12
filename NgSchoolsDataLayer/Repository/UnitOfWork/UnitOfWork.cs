using NgSchoolsDataLayer.Context;
using NgSchoolsDataLayer.Repository.Base;
using NgSchoolsDataLayer.Repository.Provider;

namespace NgSchoolsDataLayer.Repository.UnitOfWork
{
    // https://offering.solutions/blog/articles/2014/07/01/asp-net-mvc-generic-repositories-and-unitofwork/
    public class UnitOfWork : IUnitOfWork
    {
        private readonly NgSchoolsContext context;
        private readonly IRepositoryProvider repositoryProvider;

        public UnitOfWork(NgSchoolsContext context)
        {
            this.context = context;

            if (repositoryProvider == null)
            {
                repositoryProvider = new RepositoryProvider();
            }

            repositoryProvider.DbContext = context;
        }

        public int Save()
        {
            return context.SaveChanges();
        }

        public void Dispose()
        {
            context.Dispose();
        }

        public IRepositoryBase<T> GetGenericRepository<T>() where T : class
        {
            return repositoryProvider.GetGenericRepository<T>();
        }

        public T GetCustomRepository<T>() where T : class
        {
            return repositoryProvider.GetCustomRepository<T>();
        }
    }
}
