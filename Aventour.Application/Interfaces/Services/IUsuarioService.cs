using Aventour.Application.DTOs.Usuario;

namespace Aventour.Application.Interfaces.Services;

public interface IUsuarioService
{
    Task<UsuarioResponseDto> GetProfileAsync(int userId);
    Task<UsuarioResponseDto> UpdateProfileAsync(int userId, UsuarioActualizacionDto dto);
    Task<bool> ChangePasswordAsync(int userId, UsuarioCambioPasswordDto dto);
    // Task<string> UploadProfilePictureAsync(int userId, IFormFile file); // Para fotos
}