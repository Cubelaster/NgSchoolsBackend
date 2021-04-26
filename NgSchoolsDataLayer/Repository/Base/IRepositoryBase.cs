using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NgSchoolsDataLayer.Repository.Base
{
    public interface IRepositoryBase<T> where T : class
    {
        List<T> GetAll(Expression<Func<T, bool>> filter = null,
                       Func<IQueryable<T>, IOrderedEnumerable<T>> orderBy = null,
                       string includeProperties = "");

        IQueryable<T> GetAllAsQueryable(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedEnumerable<T>> orderBy = null,
            string includeProperties = "");

        /// <summary>
        /// Adds a AsNoTracking to Query
        /// Use it for reading
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        IQueryable<T> ReadAll(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedEnumerable<T>> orderBy = null,
            string includeProperties = "");

        IQueryable<T> ReadAllActiveAsQueryable();

        T FindSingle(Guid id);

        T FindSingle(int id);

        T FindBy(Expression<Func<T, bool>> predicate, string includeProperties = "");

        void Add(T toAdd);

        void AddRange(List<T> toAdd);

        void Update(T toUpdate);

        void Delete(Guid id);

        void Delete(int id);

        void Delete(T entity);
    }
}
