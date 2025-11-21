namespace Aventour.Application.Interfaces.Utilities;

/// <summary>
/// Abstracción para el manejo de hashing de contraseñas (e.g., usando BCrypt).
/// Su implementación real va en la capa de Infraestructura.
/// </summary>
public interface IPasswordHasher
{
    string Hash(string password);
    bool Verify(string password, string hashedPassword);
}