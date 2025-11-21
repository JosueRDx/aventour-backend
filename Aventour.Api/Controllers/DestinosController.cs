using Aventour.Application.DTOs.Destinos;
using Aventour.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aventour.Api.Controllers;

[ApiController]
[Route("api/v1/admin/destinos")]
// Asume que solo los administradores tienen acceso a este controlador
// [Authorize(Roles = "Administrador")] 
public class DestinosController : ControllerBase
{
    private readonly IDestinoService _destinoService;

    public DestinosController(IDestinoService destinoService)
    {
        _destinoService = destinoService;
    }

    /// <summary>
    /// Crea un nuevo destino turístico (Solo Admin).
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(DestinoResponseDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateDestino([FromBody] DestinoCreacionDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _destinoService.CreateDestinoAsync(dto);
        return CreatedAtAction(nameof(GetDestinoById), new { id = result.IdDestino }, result);
    }
    
    /// <summary>
    /// Obtiene todos los destinos (Uso general o Admin).
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<DestinoResponseDto>))]
    public async Task<IActionResult> GetAllDestinos()
    {
        var result = await _destinoService.GetAllDestinosAsync();
        return Ok(result);
    }
    
    /// <summary>
    /// Obtiene un destino por ID.
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DestinoResponseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDestinoById(int id)
    {
        var result = await _destinoService.GetDestinoByIdAsync(id);
        if (result == null) return NotFound();
        return Ok(result);
    }

    /// <summary>
    /// Actualiza un destino existente (Solo Admin).
    /// </summary>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateDestino(int id, [FromBody] DestinoCreacionDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        // El servicio debe manejar el NotFound y lanzar una excepción si el ID no existe.
        await _destinoService.UpdateDestinoAsync(id, dto);
        return NoContent(); // 204 No Content, estándar para actualización exitosa
    }

    /// <summary>
    /// Elimina un destino existente (Solo Admin).
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteDestino(int id)
    {
        // El servicio debe manejar el NotFound.
        await _destinoService.DeleteDestinoAsync(id);
        return NoContent();
    }
}