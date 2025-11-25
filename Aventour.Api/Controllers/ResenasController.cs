using Aventour.Application.DTOs.Resenas;
using Aventour.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Aventour.Api.Controllers;

[ApiController]
[Route("api/v1/resenas")]
public class ResenasController : ControllerBase
{
    private readonly IResenaService _resenaService;

    public ResenasController(IResenaService resenaService)
    {
        _resenaService = resenaService;
    }
    
    // Helper MODIFICADO: Devuelve el ID si existe, sino, devuelve 0.
    // Esto previene que las acciones PÚBLICAS fallen, pero las acciones 
    // PRIVADAS deben usar [Authorize] o chequear el 0.
    private int GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (!string.IsNullOrEmpty(userIdClaim) && int.TryParse(userIdClaim, out int userId))
        {
            return userId;
        }
        
        return 0; 
    }

    // ------------------------------------------------------------------
    // ENDPOINTS DE CREACIÓN/ACTUALIZACIÓN (Requieren Autorización)
    // ------------------------------------------------------------------

    /// <summary>
    /// Crea una nueva reseña para un destino específico.
    /// </summary>
    [HttpPost("destinos/{idDestino}")]
    [Authorize] // 👈 AHORA SÍ REQUIERE AUTORIZACIÓN PARA ACCEDER
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ResenaResponseDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateResena(int idDestino, [FromBody] ResenaCreacionDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            // Con [Authorize], esta llamada siempre devolverá un ID > 0.
            int userId = GetCurrentUserId(); 
            var result = await _resenaService.CreateResenaAsync(userId, idDestino, dto);
            
            return CreatedAtAction(nameof(GetResenasByDestino), new { idDestino = result.IdDestino }, result);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (KeyNotFoundException)
        {
             return NotFound(new { message = $"El destino con ID {idDestino} no existe." });
        }
    }

    /// <summary>
    /// Actualiza una reseña existente. Solo el autor puede hacer esto.
    /// </summary>
    [HttpPut("{idResena}")]
    [Authorize] // 👈 REQUIERE AUTORIZACIÓN
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResenaResponseDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)] // Usar 403 en lugar de 401 si no es el autor
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateResena(int idResena, [FromBody] ResenaCreacionDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        int userId = GetCurrentUserId();
        
        // Se asume que el servicio lanza UnauthorizedAccessException para manejo de 401/403
        var result = await _resenaService.UpdateResenaAsync(idResena, userId, dto);
        return Ok(result);
    }

    /// <summary>
    /// Elimina una reseña existente. Solo el autor puede hacer esto.
    /// </summary>
    [HttpDelete("{idResena}")]
    [Authorize] // 👈 REQUIERE AUTORIZACIÓN
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)] 
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteResena(int idResena)
    {
        int userId = GetCurrentUserId();
        
        await _resenaService.DeleteResenaAsync(idResena, userId);
        
        return NoContent();
    }
    
    // ------------------------------------------------------------------
    // ENDPOINTS DE LECTURA (Uso General - No requieren Autorización)
    // ------------------------------------------------------------------

    /// <summary>
    /// Lista todas las reseñas para un destino específico.
    /// </summary>
    [HttpGet("destinos/{idDestino}")]
    [AllowAnonymous] // 👈 EXPLÍCITAMENTE PÚBLICO
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ResenaResponseDto>))]
    public async Task<IActionResult> GetResenasByDestino(int idDestino)
    {
        // NO llama a GetCurrentUserId, por lo que nunca fallará.
        var result = await _resenaService.GetResenasByDestinoAsync(idDestino);
        return Ok(result);
    }
    
    /// <summary>
    /// Obtiene la calificación promedio y el número total de reseñas para un destino.
    /// </summary>
    [HttpGet("destinos/{idDestino}/promedio")]
    [AllowAnonymous] // 👈 EXPLÍCITAMENTE PÚBLICO
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PuntuacionMediaResponseDto))]
    public async Task<IActionResult> GetAverageRatingByDestino(int idDestino)
    {
        // NO llama a GetCurrentUserId, por lo que nunca fallará.
        var result = await _resenaService.GetAverageRatingByDestinoAsync(idDestino);
        return Ok(result);
    }
}