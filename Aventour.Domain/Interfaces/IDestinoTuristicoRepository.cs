using Aventour.Domain.Models;

namespace Aventour.Domain.Interfaces;

public interface IDestinoTuristicoRepository : IGenericRepository<DestinosTuristico>
{
    /// <summary>
    /// Busca destinos por nombre o tipo, soportando la funcionalidad de búsqueda principal.
    /// </summary>
    /// <param name="searchTerm">Término de búsqueda (nombre, tipo, o parte de la descripción).</param>
    /// <returns>Una colección filtrada de DestinosTuristico.</returns>
    Task<IEnumerable<DestinosTuristico>> SearchAsync(string searchTerm);

    /// <summary>
    /// Obtiene destinos ordenados por la Puntuación Media para el módulo de Recomendaciones.
    /// </summary>
    Task<IEnumerable<DestinosTuristico>> GetTopRatedAsync(int count);

    /// <summary>
    /// Busca destinos por un radio de cercanía (Latitud/Longitud).
    /// </summary>
    /// <param name="lat">Latitud central.</param>
    /// <param name="lon">Longitud central.</param>
    /// <param name="radiusKm">Radio de búsqueda en kilómetros.</param>
    /// <returns>Una colección de DestinosTuristico cercanos.</returns>
    Task<IEnumerable<DestinosTuristico>> GetByProximityAsync(decimal lat, decimal lon, double radiusKm);
}