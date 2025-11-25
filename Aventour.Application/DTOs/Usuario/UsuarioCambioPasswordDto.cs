using System.ComponentModel.DataAnnotations;

namespace Aventour.Application.DTOs.Usuario;

public class UsuarioCambioPasswordDto
{
    [Required(ErrorMessage = "La contraseña actual es obligatoria.")]
    public string CurrentPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "La nueva contraseña es obligatoria.")]
    [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
    public string NewPassword { get; set; } = string.Empty;
}