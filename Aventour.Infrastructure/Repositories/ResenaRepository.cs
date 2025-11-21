using Aventour.Domain.Interfaces;
using Aventour.Domain.Models;
using Aventour.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Aventour.Infrastructure.Repositories;

public class ResenaRepository : GenericRepository<Resena>, IResenaRepository
{
    public ResenaRepository(AventourDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Resena>> GetByEntityIdAsync(int idEntidad)
    {
        return await _dbSet.AsNoTracking()
            .Where(r => r.IdEntidad == idEntidad)
            .OrderByDescending(r => r.FechaCreacion)
            .ToListAsync();
    }

    public async Task<decimal> GetAverageScoreAsync(int idEntidad)
    {
        // Se calcula el promedio en la base de datos (más eficiente)
        var average = await _dbSet.Where(r => r.IdEntidad == idEntidad)
            .Select(r => (decimal?)r.Puntuacion)
            .AverageAsync();
                                 
        return average ?? 0.0M; // Devuelve 0.0 si no hay reseñas
    }

    public async Task<bool> HasUserReviewedAsync(int idUsuario, int idEntidad)
    {
        return await _dbSet.AnyAsync(r => r.IdUsuario == idUsuario && r.IdEntidad == idEntidad);
    }
}