namespace Aventour.Application.DTOs.Resenas;

public class ResenaResponseDto
{
    public int IdResena { get; set; }
    public int IdDestino { get; set; }
    public int Puntuacion { get; set; }
    public string? Comentario { get; set; }
    public DateTime? FechaCreacion { get; set; }
    public string AutorNombreCompleto { get; set; } = string.Empty;
}