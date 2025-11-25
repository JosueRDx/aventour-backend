using Aventour.Application.DTOs.Guias;
using Aventour.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Aventour.Api.Controllers;

[ApiController]
[Route("api/v1/guias")]
public class GuiasController : ControllerBase
{
    private readonly IGuiaService _guiaService;

    public GuiasController(IGuiaService guiaService)
    {
        _guiaService = guiaService;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(GuiaResponseDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] GuiaCreacionDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var result = await _guiaService.CreateGuiaAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.IdAgencia }, result);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<GuiaResponseDto>))]
    public async Task<IActionResult> GetAll()
    {
        var result = await _guiaService.GetAllGuiasAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GuiaResponseDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _guiaService.GetGuiaByIdAsync(id);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] GuiaCreacionDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            await _guiaService.UpdateGuiaAsync(id, dto);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _guiaService.DeleteGuiaAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}