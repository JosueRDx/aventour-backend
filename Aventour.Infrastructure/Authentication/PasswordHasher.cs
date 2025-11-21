using Aventour.Application.Interfaces.Utilities;

namespace Aventour.Infrastructure.Authentication;

public class PasswordHasher : IPasswordHasher
{
    public string Hash(string password)
    {
        // BCrypt genera automáticamente el salt y lo incluye en el hash
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool Verify(string password, string hashedPassword)
    {
        // BCrypt verifica automáticamente el password contra el hash
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}