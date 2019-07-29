using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Data.Common.Definition
{
    public interface IRepository<T>
        where T : Entity
    {
        IUnitOfWork UnitOfWork { get; set; }

        object Mapper(object entity, object entity2);

        T AddItem(T item);

        IEnumerable<T> GetForEdit(Expression<Func<T, bool>> filter, IEnumerable<T> entity);

        long Count(Expression<Func<T, bool>> filter = null);

        T Get(Expression<Func<T, bool>> filter);

        IQueryable<T> GetAll();

        IQueryable<T> GetFiltered(Expression<Func<T, bool>> filter, bool asNoTracking = false);

        void Remove(T item, bool logical = true);

        void Update(T entity);

        IQueryable<T> GetQueryable();
    }
}
