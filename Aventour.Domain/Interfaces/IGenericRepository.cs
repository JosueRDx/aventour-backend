using System.Linq.Expressions;
using Aventour.Domain.Entities;

namespace Aventour.Domain.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity); 
        Task<IReadOnlyList<T>> GetWhereAsync(Expression<Func<T, bool>> predicate);
        Task SaveChangesAsync(); 
    }
}