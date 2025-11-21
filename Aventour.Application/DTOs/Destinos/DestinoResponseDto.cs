namespace Aventour.Application.DTOs.Destinos;

public class DestinoResponseDto
{
    public int IdDestino { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string DescripcionBreve { get; set; } = string.Empty;
    public string? Tipo { get; set; }
    public decimal Latitud { get; set; }
    public decimal Longitud { get; set; }
    public decimal? CostoEntrada { get; set; }
    public decimal? PuntuacionMedia { get; set; }
}