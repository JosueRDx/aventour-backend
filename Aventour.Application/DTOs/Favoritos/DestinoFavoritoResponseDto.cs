namespace Aventour.Application.DTOs.Favoritos;

/// <summary>
/// Representa la información esencial de un destino marcado como favorito.
/// </summary>
public class DestinoFavoritoResponseDto
{
    public int IdDestino { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string DescripcionBreve { get; set; } = string.Empty;
    public string? UrlFotoPrincipal { get; set; }
    public decimal? PuntuacionMedia { get; set; }
}