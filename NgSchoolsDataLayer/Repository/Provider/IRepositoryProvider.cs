using NgSchoolsDataLayer.Context;
using NgSchoolsDataLayer.Repository.Base;
using System;

namespace NgSchoolsDataLayer.Repository.Provider
{
    internal interface IRepositoryProvider
    {
        NgSchoolsContext DbContext { get; set; }

        IRepositoryBase<T> GetGenericRepository<T>() where T : class;

        T GetCustomRepository<T>(Func<NgSchoolsContext, object> factory = null) where T : class;
    }
}
