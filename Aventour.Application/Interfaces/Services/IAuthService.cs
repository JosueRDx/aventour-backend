using Aventour.Application.DTOs.Auth;

namespace Aventour.Application.Interfaces.Services;

/// <summary>
/// Contrato para toda la lógica de negocio relacionada con la autenticación y usuarios.
/// </summary>
public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegistroUsuarioDto dto);
    Task<AuthResponseDto> LoginAsync(LoginUsuarioDto dto);
    Task<UsuarioResponseDto> UpdateProfileAsync(int userId, ActualizarPerfilDto dto);
    Task<bool> RequestPasswordResetAsync(string email);
    Task<bool> ResetPasswordAsync(string token, string newPassword);
    
    // Otros métodos, ej.: ConfirmarEmailAsync, LogoutAsync, etc.
}