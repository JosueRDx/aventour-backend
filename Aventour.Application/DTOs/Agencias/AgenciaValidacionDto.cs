using System.ComponentModel.DataAnnotations;

namespace Aventour.Application.DTOs.Agencias;

/// <summary>
/// DTO para que un administrador cambie el estado de validación de una agencia.
/// </summary>
public class AgenciaValidacionDto
{
    [Required]
    public bool Validado { get; set; }
}