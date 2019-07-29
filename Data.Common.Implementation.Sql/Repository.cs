using Core.Entities;
using Data.Common.Definition;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;

namespace Data.Common.Implementation.Sql
{

    public class Repository<T> : IRepository<T>
  where T : Entity
    {


        public Repository(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;

        }


        public IQueryable<T> GetQueryable()
        {
            return (UnitOfWork as DbContext).Set<T>();

        }

        public object Mapper(object entity, object entity2)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            if (entity2 == null) throw new ArgumentNullException(nameof(entity2));
            foreach (var propertyInfo in entity.GetType().GetProperties())
            {
                var name = propertyInfo.Name;
                var value = propertyInfo.GetValue(entity, null);
                entity2.GetType().GetProperty(name).SetValue(entity2, value, null);
            }
            return entity2;
        }

        public T AddItem(T item)
        {
            UnitOfWork.AdddEntity(item);
            UnitOfWork.CommitInt();
            return item;
        }


        public IEnumerable<T> GetForEdit(Expression<Func<T, bool>> filter, IEnumerable<T> entity)
        {
            var dt = GetQueryable().Where(filter).AsEnumerable();
            if (dt == null) return entity;
            var edit = dt as IList<T> ?? dt.ToList();
            return edit.Any() ? edit : entity;
        }


        public long Count(Expression<Func<T, bool>> filter = null)
        {
            return filter != null ? GetQueryable().Count(filter) : GetQueryable().Count();
        }

        public T Get(Expression<Func<T, bool>> filter)
        {
            if (filter == null) return null;
            IQueryable<T> set = GetQueryable();
            try
            {
                return set.FirstOrDefault(filter);
            }
            catch (Exception ex)
            {
                throw new Exception("Se ha producido un error en el metodo Get()", ex);
            }
        }

        public IQueryable<T> GetAll()
        {
            IQueryable<T> set = GetQueryable();
            try
            {
                return set;
            }
            catch (Exception ex)
            {
                throw new Exception("Se ha producido un error en el metodo GetAll()", ex);
            }
        }

        public IQueryable<T> GetFiltered(Expression<Func<T, bool>> filter, bool asNoTracking = false)
        {
            if (filter == null) return null;
            IQueryable<T> set = GetQueryable();
            try
            {
                return set.Where(filter);
            }
            catch (Exception ex)
            {
                throw new Exception("Se ha producido un error en el metodo GetFiltered()", ex);
            }
        }


        public void Update(T entity)
        {
            if (entity == null) return;
            UnitOfWork.AttachEntity(entity);
            UnitOfWork.CommitInt();
        }

        public void Remove(T item, bool logical = true)
        {
            if (logical)
            {
                item.Status = StatusType.Deleted;
                Update(item);
            }
            else
            {
                // attach item if not exist
                UnitOfWork.AttachEntity(item);

                // set as "removed"
                UnitOfWork.RemoveEntity(item);
            }
        }

        public IUnitOfWork UnitOfWork { get; set; }
    }
}
