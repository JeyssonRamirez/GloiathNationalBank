using Core.Entities;
using Data.Common.Definition;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Data.Common.Implementation.MongoDb
{
    public class RepositoryMongo<T> : IRepository<T> where T : Entity
    {

        private readonly MongoCollection<T> _collection;
        private readonly MongoDataContext _context;

        public IUnitOfWork UnitOfWork { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public RepositoryMongo(IUnitOfWork unitOfWork)
        {
            _context = (unitOfWork as MongoDataContext);
            _collection = _context.GetDatabase().GetCollection<T>(typeof(T).Name.ToLower());
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
            try
            {
                _collection.Insert<T>(item);

                return item;
            }
            catch (Exception ex)
            {
                //todo: handle exception
                throw ex;
            }
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

        public void Remove(T entity, bool logical = true)
        {
            try
            {
                if (logical)
                {

                    entity.Status = StatusType.Deleted;
                    var query = Query<T>.EQ(o => o.Id, entity.Id);
                    var update = Update<T>.Replace(entity);
                    _collection.Update(query, update);

                }
                else
                {
                    _collection.Remove(Query.EQ("_id", entity.Id));
                }
            }
            catch (Exception ex)
            {
                //todo: handle exception
                throw ex;
            }
        }

        public void Update(T entity)
        {
            try
            {
                var query = Query<T>.EQ(o => o.Id, entity.Id);
                var update = Update<T>.Replace(entity);
                _collection.Update(query, update);
            }
            catch (Exception ex)
            {
                //todo: handle exception
                throw ex;
            }
        }

        public IQueryable<T> GetQueryable()
        {
            return _collection.AsQueryable<T>();
        }
    }
}
