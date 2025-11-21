using System.ComponentModel.DataAnnotations;

namespace Aventour.Application.DTOs.Auth;

// DTO para registrar un nuevo usuario
public class RegistroUsuarioDto
{
    [Required]
    [StringLength(100)]
    public string Nombres { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Apellidos { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
    public string Password { get; set; } = string.Empty;

    public int? Edad { get; set; }
    public string? EstadoCivil { get; set; }
}