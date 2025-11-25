using System.ComponentModel.DataAnnotations;

namespace Aventour.Application.DTOs.Usuario;

public class UsuarioActualizacionDto
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100)]
    public string Nombres { get; set; } = string.Empty;

    [Required(ErrorMessage = "El apellido es obligatorio.")]
    [StringLength(100)]
    public string Apellidos { get; set; } = string.Empty;

    [Range(18, 120, ErrorMessage = "La edad debe ser un número válido.")]
    public int? Edad { get; set; }

    [StringLength(50)]
    public string? EstadoCivil { get; set; }

    // NOTA: El Email, Password y EsAdministrador no se actualizan desde aquí.
}