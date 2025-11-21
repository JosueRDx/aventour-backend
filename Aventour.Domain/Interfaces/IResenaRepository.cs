using Aventour.Domain.Models;

namespace Aventour.Domain.Interfaces;

public interface IResenaRepository : IGenericRepository<Resena>
{
    /// <summary>
    /// Obtiene todas las reseñas para una entidad específica (Destino, Agencia, Guía, etc.).
    /// </summary>
    /// <param name="idEntidad">ID de la entidad reseñada.</param>
    /// <returns>Una colección de Reseñas.</returns>
    Task<IEnumerable<Resena>> GetByEntityIdAsync(int idEntidad);

    /// <summary>
    /// Calcula y devuelve la puntuación media de una entidad específica.
    /// </summary>
    /// <param name="idEntidad">ID de la entidad.</param>
    /// <returns>La puntuación media (decimal).</returns>
    Task<decimal> GetAverageScoreAsync(int idEntidad);

    /// <summary>
    /// Verifica si un usuario ya ha dejado una reseña para una entidad específica.
    /// </summary>
    /// <returns>True si existe una reseña previa.</returns>
    Task<bool> HasUserReviewedAsync(int idUsuario, int idEntidad);
}