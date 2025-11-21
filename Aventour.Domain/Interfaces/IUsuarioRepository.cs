using Aventour.Domain.Entities;
using Aventour.Domain.Models;

namespace Aventour.Domain.Interfaces;

public interface IUsuarioRepository : IGenericRepository<Usuario>
{
    /// <summary>
    /// Busca un Usuario por su dirección de correo electrónico única.
    /// Utilizado para la validación de registro y el inicio de sesión.
    /// </summary>
    /// <param name="email">El correo electrónico del usuario.</param>
    /// <returns>El objeto Usuario o null si no se encuentra.</returns>
    Task<Usuario?> GetByEmailAsync(string email);

    /// <summary>
    /// Busca un Usuario por su Token de Confirmación.
    /// Utilizado para la validación de correo o recuperación de contraseña.
    /// </summary>
    /// <param name="token">El token de confirmación.</param>
    /// <returns>El objeto Usuario o null si no se encuentra.</returns>
    Task<Usuario?> GetByTokenConfirmacionAsync(string token);

    /// <summary>
    /// Obtiene una lista de todos los administradores (EsAdministrador = true).
    /// </summary>
    Task<IEnumerable<Usuario>> GetAllAdministratorsAsync();
}