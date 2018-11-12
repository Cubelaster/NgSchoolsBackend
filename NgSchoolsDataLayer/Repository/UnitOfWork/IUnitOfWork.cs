using NgSchoolsDataLayer.Repository.Base;
using System;

namespace NgSchoolsDataLayer.Repository.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IRepositoryBase<T> GetGenericRepository<T>() where T : class;

        T GetCustomRepository<T>() where T : class;

        int Save();
    }
}
