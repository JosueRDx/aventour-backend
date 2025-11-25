using Aventour.Application.DTOs.Usuario;
using Aventour.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims; // Necesario para obtener el ID del usuario

namespace Aventour.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
// Aunque omitimos la implementación, marcamos para indicar que requiere Auth.
// [Authorize] 
public class UsuariosController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;

    public UsuariosController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    // Helper para obtener el ID del usuario autenticado
    private int GetCurrentUserId()
    {
        // En un escenario real, esto se haría usando Claims.
        // Aquí simulamos que se obtiene el ID del token JWT.
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim))
        {
            // Lanzaríamos una excepción para el Middleware 401
            throw new UnauthorizedAccessException("Token de usuario inválido.");
        }
        return int.Parse(userIdClaim);
    }

    /// <summary>
    /// Obtiene los datos del perfil del usuario autenticado.
    /// </summary>
    [HttpGet("perfil")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UsuarioResponseDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetProfile()
    {
        int userId = GetCurrentUserId(); // Simular lectura del token
        
        // El servicio maneja el 404/KeyNotFound
        var profile = await _usuarioService.GetProfileAsync(userId);
        return Ok(profile);
    }

    /// <summary>
    /// Actualiza la información personal del perfil.
    /// </summary>
    [HttpPut("perfil")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UsuarioResponseDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> UpdateProfile([FromBody] UsuarioActualizacionDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        int userId = GetCurrentUserId();

        var updatedProfile = await _usuarioService.UpdateProfileAsync(userId, dto);
        return Ok(updatedProfile);
    }

    /// <summary>
    /// Permite al usuario cambiar su contraseña.
    /// </summary>
    [HttpPut("password")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ChangePassword([FromBody] UsuarioCambioPasswordDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        int userId = GetCurrentUserId();

        // El servicio lanza UnauthorizedAccessException si la contraseña actual es incorrecta
        await _usuarioService.ChangePasswordAsync(userId, dto);
        
        return NoContent(); // 204 No Content
    }
}