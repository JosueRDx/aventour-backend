using System.ComponentModel.DataAnnotations;

namespace Aventour.Application.DTOs.Guias;

public class GuiaCreacionDto
{
    [Required]
    [StringLength(200)]
    public string NombreComercial { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    [Phone] 
    public string WhatsappContacto { get; set; } = string.Empty;

    [Required]
    [StringLength(150)]
    [EmailAddress] 
    public string EmailContacto { get; set; } = string.Empty;

    public string? Descripcion { get; set; }
}