using System.ComponentModel.DataAnnotations;

namespace Aventour.Application.DTOs.Auth;

// DTO para la edición de perfil
public class ActualizarPerfilDto
{
    // Solo los campos que el usuario puede cambiar
    public string Nombres { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public int? Edad { get; set; }
    public string? EstadoCivil { get; set; }
    
    // Opcional: para cambiar el email o contraseña, requerirías un DTO aparte con validaciones de seguridad
}