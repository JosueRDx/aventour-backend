using System.ComponentModel.DataAnnotations;

namespace Aventour.Application.DTOs.Auth;

// DTO para el inicio de sesión
public class LoginUsuarioDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}