namespace Aventour.Application.DTOs.Auth;

// DTO de respuesta completa de autenticación
public class AuthResponseDto
{
    public UsuarioResponseDto Usuario { get; set; } = null!;
    public string Token { get; set; } = string.Empty; // El JWT (JSON Web Token)
    public DateTime ExpiracionToken { get; set; }
    public bool EsRegistroExitoso { get; set; }
}