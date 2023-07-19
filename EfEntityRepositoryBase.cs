
using Entities;
using Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Result;
using System.Linq.Expressions;

namespace EntityFramework
{
    public class EfEntityRepositoryBase<TEntity, TContext> : IEntityRepository<TEntity>
        where TEntity : class, IEntity, new()
        where TContext : DbContext, new()
    {
        public TEntity AddEntity(TEntity entity)
        {
            using var context = new TContext();
            var addedEntity = context.Add(entity);
            addedEntity.State = EntityState.Added;
            context.SaveChanges();
            return entity;
        }
        public bool Add(TEntity entity)
        {
            using TContext val = new TContext();
            EntityEntry<TEntity> entityEntry = val.Entry(entity);
            entityEntry.State = EntityState.Added;
            int num = val.SaveChanges();
            return num > 0;
        }

        public bool AddRange(List<TEntity> entities)
        {
            using TContext val = new TContext();
            EntityEntry<List<TEntity>> entityEntry = val.Entry(entities);
            entityEntry.State = EntityState.Added;
            int num = val.SaveChanges();
            return num > 0;
        }

        public void DeleteEntity(TEntity entity)
        {
            using var context = new TContext();
            var deletedEntity = context.Remove(entity);
            deletedEntity.State = EntityState.Deleted;
            context.SaveChanges();
        }

        public TEntity Get(Expression<Func<TEntity, bool>> filter)
        {
            using var context = new TContext();
            return context.Set<TEntity>().SingleOrDefault(filter);
        }

        public TEntity Get(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] includeEntities)
        {
            using TContext val = new();
            IQueryable<TEntity> queryable = val.Set<TEntity>().AsQueryable();
            if (includeEntities.Length != 0)
            {
                queryable = queryable.IncludeMultiple(includeEntities);
            }

            return queryable.SingleOrDefault(filter);
        }

        public List<TEntity> GetList(Expression<Func<TEntity, bool>> filter = null)
        {
            using (var context = new TContext())
            {
                return filter == null ? context.Set<TEntity>().ToList() : context.Set<TEntity>().Where(filter).ToList();
            }
        }

        public PagingResult<TEntity> GetListForPaging(int page, string propertyName, bool asc, int itemPerPageCount, Expression<Func<TEntity, bool>> filter = null, params Expression<Func<TEntity, object>>[] includeEntities)
        {
            using TContext val = new TContext();
            IQueryable<TEntity> queryable = val.Set<TEntity>().AsQueryable();
            if (includeEntities.Length != 0)
            {
                queryable = queryable.IncludeMultiple(includeEntities);
            }

            if (filter != null)
            {
                queryable = queryable.Where(filter).AsQueryable();
            }

            queryable = asc ? queryable.OrderBy(propertyName) : queryable.OrderByDesc(propertyName);
            int totalItemCount = queryable.Count();
            int count = (page - 1) * itemPerPageCount;
            queryable = queryable.Skip(count).Take(itemPerPageCount);
            return new PagingResult<TEntity>(queryable.ToList(), totalItemCount, success: true, "");
        }

        public PagingResult<TEntity> GetListForPaging(int page, string propertyName, bool asc, Expression<Func<TEntity, bool>> filter = null, params Expression<Func<TEntity, object>>[] includeEntities)
        {
            return GetListForPaging(page, propertyName, asc, 15, filter, includeEntities);
        }

        public TEntity UpdateEntity(TEntity entity)
        {
            using (var context = new TContext())
            {
                var updateEntity = context.Entry(entity);
                updateEntity.State = EntityState.Modified;
                context.SaveChanges();
                return entity;

            }
        }
        public bool Delete(TEntity entity)
        {
            using TContext val = new TContext();
            EntityEntry<TEntity> entityEntry = val.Entry(entity);
            entityEntry.State = EntityState.Deleted;
            int num = val.SaveChanges();
            return num > 0;
        }

        public bool Update(TEntity entity)
        {
            using TContext val = new TContext();
            EntityEntry<TEntity> entityEntry = val.Entry(entity);
            entityEntry.State = EntityState.Modified;
            int num = val.SaveChanges();
            return num > 0;
        }

        public bool UpdateRange(List<TEntity> entities)
        {
            using TContext val = new TContext();
            EntityEntry<List<TEntity>> entityEntry = val.Entry(entities);
            entityEntry.State = EntityState.Modified;
            int num = val.SaveChanges();
            return num > 0;
        }

    }
}
