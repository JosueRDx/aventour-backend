using Aventour.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Aventour.Infrastructure.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly AventourDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public GenericRepository(AventourDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    // --- Implementación IGenericRepository ---

    public async Task<T?> GetByIdAsync(int id)
    {
        // Usa FindAsync para buscar por clave primaria directamente en el DbSet
        return await _dbSet.FindAsync(id);
    }

    Task<IReadOnlyList<T>> IGenericRepository<T>.GetAllAsync()
    {
        throw new NotImplementedException();
    }

    Task<T> IGenericRepository<T>.AddAsync(T entity)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.AsNoTracking().ToListAsync();
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }
    
    // Usamos 'void' porque la operación se realiza en memoria; 
    // SaveChanges se llama a través del UnitOfWork.
    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }

    public Task<IReadOnlyList<T>> GetWhereAsync(Expression<Func<T, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public Task SaveChangesAsync()
    {
        throw new NotImplementedException();
    }
}