using Core.Models;
using System.Linq.Expressions;

namespace Core.Interfaces
{
    public interface IGenericRepo<T> where T : class, IDeletable
    {
        Task<T?> GetByIdAsync(object id);
        Task<T?> GetByIdForAdminAsync(object id);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<IReadOnlyList<T>> GetAllForAdminAsync();
        Task<T> AddAsync(T entity);
        T Update(T entity);
        Task<bool> DeleteByIdAsync(object id);
        Task<bool> SaveAllAsync();
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IQueryable<T>> include = null);
        Task<Product?> GetProductWithVariantsAsync(Guid id);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
    }
}
