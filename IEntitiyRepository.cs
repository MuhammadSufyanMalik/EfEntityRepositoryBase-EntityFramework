using Entities;
using Result;
using System.Linq.Expressions;

namespace EntityFramework
{
    public interface IEntityRepository<T> where T : class, IEntity, new()
    {
        List<T> GetList(Expression<Func<T, bool>> filter = null);
        T Get(Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includeEntities);
        T AddEntity(T entity);
        T UpdateEntity(T entity);
        void DeleteEntity(T entity);
        PagingResult<T> GetListForPaging(int page, string propertyName, bool asc, int itemPerPageCount, Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] includeEntities);

        PagingResult<T> GetListForPaging(int page, string propertyName, bool asc, Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] includeEntities);

        bool Add(T entity);

        bool AddRange(List<T> entities);

        bool Update(T entity);

        bool UpdateRange(List<T> entities);

        bool Delete(T entity);
    }
}
