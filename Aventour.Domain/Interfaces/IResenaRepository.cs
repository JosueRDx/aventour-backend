using Aventour.Domain.Models;

namespace Aventour.Domain.Interfaces;

public interface IResenaRepository : IGenericRepository<Resena>
{
    /// <summary>
    /// Soluciona 'GetResenasByEntityIdAsync' (Línea 153 en ResenaService)
    /// </summary>
    Task<IEnumerable<Resena>> GetResenasByEntityIdAsync(int idEntidad);

    /// <summary>
    /// Soluciona 'CalculateAverageRatingAsync' (Línea 40 y 159 en ResenaService).
    /// El servicio espera una tupla (media, conteo).
    /// </summary>
    Task<(decimal Media, int Count)> CalculateAverageRatingAsync(int idEntidad);

    /// <summary>
    /// Soluciona 'GetByUserAndEntityAsync' (Línea 58 en ResenaService).
    /// El servicio espera la Reseña para verificar si existe (!= null).
    /// </summary>
    Task<Resena?> GetByUserAndEntityAsync(int userId, int idEntidad);
}