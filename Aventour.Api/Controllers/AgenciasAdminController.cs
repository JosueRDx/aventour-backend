using Aventour.Application.DTOs.Agencias;
using Aventour.Application.Interfaces.Services;
using Aventour.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aventour.Api.Controllers;

[ApiController]
[Route("api/v1/admin/agencias")]
// [Authorize(Roles = "Administrador")] 
public class AgenciasAdminController : ControllerBase
{
    private readonly IAgenciaAdminService _agenciaAdminService;

    public AgenciasAdminController(IAgenciaAdminService agenciaAdminService)
    {
        _agenciaAdminService = agenciaAdminService;
    }

    /// <summary>
    /// Obtiene la lista de agencias/guías pendientes de validación por el administrador.
    /// </summary>
    [HttpGet("pendientes")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<AgenciasGuia>))]
    public async Task<IActionResult> GetPendientesValidacion()
    {
        var result = await _agenciaAdminService.GetUnvalidatedAgenciesAsync();
        return Ok(result);
    }

    /// <summary>
    /// Actualiza el estado de validación de una agencia específica (Aprobación/Rechazo).
    /// </summary>
    /// <param name="id">ID de la agencia a validar.</param>
    [HttpPut("{id}/validar")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ValidateAgency(int id, [FromBody] AgenciaValidacionDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var success = await _agenciaAdminService.ValidateAgencyAsync(id, dto.Validado);
        
        if (!success) 
        {
            // Lanzar una excepción específica o retornar 404 si la agencia no existe.
            return NotFound(new { message = $"Agencia con ID {id} no encontrada." });
        }
        
        return NoContent();
    }
}