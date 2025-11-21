using System.Security.Cryptography;
using System.Text;
using Aventour.Application.Interfaces.Utilities;

namespace Aventour.Infrastructure.Authentication;

public class PasswordHasher : IPasswordHasher
{
    public string Hash(string password)
    {
        // Usamos SHA256 que ya viene en .NET (No requiere instalar nada)
        using (var sha256 = SHA256.Create())
        {
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            var builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }
    }

    public bool Verify(string password, string hashedPassword)
    {
        // Para verificar, volvemos a hashear la contraseña ingresada
        // y miramos si el resultado es idéntico al guardado.
        var hashOfInput = Hash(password);
        return StringComparer.OrdinalIgnoreCase.Compare(hashOfInput, hashedPassword) == 0;
    }
}