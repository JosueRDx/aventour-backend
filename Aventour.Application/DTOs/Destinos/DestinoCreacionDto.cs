using System.ComponentModel.DataAnnotations;

namespace Aventour.Application.DTOs.Destinos;

public class DestinoCreacionDto
{
    [Required]
    [StringLength(150)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [StringLength(255)]
    public string DescripcionBreve { get; set; } = string.Empty;

    public string? DescripcionCompleta { get; set; }

    [Required]
    public string? Tipo { get; set; }

    [Required]
    [Range(-90.0, 90.0)] // Rango de latitud válido
    public decimal Latitud { get; set; }

    [Required]
    [Range(-180.0, 180.0)] // Rango de longitud válido
    public decimal Longitud { get; set; }

    public string? HorarioAtencion { get; set; }

    [Range(0.0, 10000.0)]
    public decimal? CostoEntrada { get; set; }

    public string? UrlFotoPrincipal { get; set; }
}