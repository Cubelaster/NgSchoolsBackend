﻿using Microsoft.EntityFrameworkCore;
using NgSchoolsDataLayer.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NgSchoolsDataLayer.Repository.Base
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        private readonly NgSchoolsContext context;

        public RepositoryBase(NgSchoolsContext context)
        {
            this.context = context;
        }

        public virtual List<T> GetAll(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedEnumerable<T>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<T> query = context.Set<T>();

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (string includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }

            return query.ToList();
        }

        public virtual T FindSingle(Guid id)
        {
            return context.Set<T>().Find(id);
        }

        public virtual T FindBy(Expression<Func<T, bool>> predicate, string includeProperties = "")
        {
            IQueryable<T> query = context.Set<T>();
            foreach (string includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }
            return query.Where(predicate).FirstOrDefault();
        }

        public virtual void Add(T toAdd)
        {
            context.Set<T>().Add(toAdd);
        }

        public virtual void Update(T toUpdate)
        {
            context.Entry(toUpdate).State = EntityState.Modified;
        }

        public virtual void Delete(Guid id)
        {
            T entity = FindSingle(id);
            context.Set<T>().Remove(entity);
        }

        public virtual void Delete(T entity)
        {
            context.Set<T>().Remove(entity);
        }
    }
}