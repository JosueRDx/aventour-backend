using Aventour.Domain.Interfaces;
using Aventour.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Aventour.Infrastructure.Repositories;

public class DestinoTuristicoRepository : GenericRepository<DestinosTuristico>, IDestinoTuristicoRepository
{
    public DestinoTuristicoRepository(AventourDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<DestinosTuristico>> SearchAsync(string searchTerm)
    {
        // Búsqueda case-insensitive por Nombre, Tipo o Descripción.
        return await _dbSet.AsNoTracking()
                           .Where(d => d.Nombre.ToLower().Contains(searchTerm.ToLower()) ||
                                       (d.Tipo != null && d.Tipo.ToLower().Contains(searchTerm.ToLower())) ||
                                       d.DescripcionBreve.ToLower().Contains(searchTerm.ToLower()))
                           .OrderByDescending(d => d.PuntuacionMedia)
                           .ToListAsync();
    }

    public async Task<IEnumerable<DestinosTuristico>> GetTopRatedAsync(int count)
    {
        return await _dbSet.AsNoTracking()
                           .OrderByDescending(d => d.PuntuacionMedia)
                           .Take(count)
                           .ToListAsync();
    }

    public async Task<IEnumerable<DestinosTuristico>> GetByProximityAsync(decimal lat, decimal lon, double radiusKm)
    {
        // Nota: Implementación simplificada. Una solución profesional usaría PostgreSQL PostGIS o una función de base de datos.
        // Fórmula de la distancia haversine (aproximación, usando solo los campos disponibles):
        // (La lógica real de distancia sería compleja y generalmente se delega a la DB para rendimiento.)
        
        // Asumiendo que esta aproximación es suficiente para la demostración:
        const double tolerance = 0.01; // Simplificación extrema, 0.01 grados es aproximadamente 1.1 km
        
        return await _dbSet.AsNoTracking()
                           .Where(d => Math.Abs((double)d.Latitud - (double)lat) <= tolerance &&
                                       Math.Abs((double)d.Longitud - (double)lon) <= tolerance)
                           .ToListAsync();
    }
}