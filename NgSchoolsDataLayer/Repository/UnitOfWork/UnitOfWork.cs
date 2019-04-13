using Microsoft.EntityFrameworkCore;
using NgSchoolsDataLayer.Context;
using NgSchoolsDataLayer.Models.BaseTypes;
using NgSchoolsDataLayer.Repository.Base;
using NgSchoolsDataLayer.Repository.Provider;
using System;
using System.Linq;

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
            this.context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            if (repositoryProvider == null)
            {
                repositoryProvider = new RepositoryProvider();
            }

            repositoryProvider.DbContext = context;
        }

        public int Save()
        {
            var changeTracker = context.ChangeTracker;

            var modifiedEntries = changeTracker.Entries()
              .Where(x => x.Entity is DatabaseEntity && (x.State == EntityState.Deleted));

            foreach (var entry in modifiedEntries)
            {
                DatabaseEntity entity = entry.Entity as DatabaseEntity;
                if (entity != null)
                {
                    entity.DateModified = DateTime.Now;

                    if (entry.State == EntityState.Deleted)
                    {
                        entry.State = EntityState.Modified;
                        entity.Status = Enums.DatabaseEntityStatusEnum.Deleted;
                    }
                }
            }

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

        public NgSchoolsContext GetContext()
        {
            return context;
        }
    }
}
