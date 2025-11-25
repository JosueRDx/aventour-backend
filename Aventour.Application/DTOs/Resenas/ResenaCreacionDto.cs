using System.ComponentModel.DataAnnotations;

namespace Aventour.Application.DTOs.Resenas;

public class ResenaCreacionDto
{
    [Required]
    [Range(1, 5, ErrorMessage = "La puntuación debe estar entre 1 y 5.")]
    public int Puntuacion { get; set; }

    [MaxLength(500, ErrorMessage = "El comentario no puede exceder los 500 caracteres.")]
    public string? Comentario { get; set; }
}