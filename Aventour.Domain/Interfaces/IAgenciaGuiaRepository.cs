using Aventour.Domain.Models;

namespace Aventour.Domain.Interfaces;

public interface IAgenciaGuiaRepository : IGenericRepository<AgenciasGuia>
{
    /// <summary>
    /// Obtiene todas las agencias/guías que aún no han sido validados por un administrador.
    /// </summary>
    Task<IEnumerable<AgenciasGuia>> GetUnvalidatedAgenciesAsync();

    /// <summary>
    /// Actualiza el estado de 'Validado' de una agencia o guía específico.
    /// </summary>
    /// <param name="idAgencia">ID de la agencia a validar.</param>
    /// <param name="isValidated">Nuevo estado de validación.</param>
    /// <returns>True si la actualización fue exitosa.</returns>
    Task<bool> SetValidationStatusAsync(int idAgencia, bool isValidated);
    
    /// <summary>
    /// Obtiene las agencias ordenadas por Puntuación Media.
    /// </summary>
    Task<IEnumerable<AgenciasGuia>> GetTopRatedAsync(int count);
}