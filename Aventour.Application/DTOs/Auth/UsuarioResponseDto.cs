namespace Aventour.Application.DTOs.Auth;

// DTO con la información pública del usuario (sin el hash de la contraseña)
public class UsuarioResponseDto
{
    public int IdUsuario { get; set; }
    public string Nombres { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int? Edad { get; set; }
    public string? EstadoCivil { get; set; }
    public DateTime? FechaRegistro { get; set; }
    public bool EsAdministrador { get; set; }
}