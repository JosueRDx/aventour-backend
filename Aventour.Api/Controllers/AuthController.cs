using Aventour.Application.DTOs.Auth;
using Aventour.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Aventour.Api.Controllers;

[ApiController]
// Define la ruta base para el controlador
[Route("api/v1/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    // Inyección de Dependencia del servicio de Autenticación
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Registra un nuevo usuario en la plataforma Aventour.
    /// </summary>
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(AuthResponseDto))]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegistroUsuarioDto dto)
    {
        // 1. Validaciones de DTO a nivel de controlador (si fallan las Data Annotations)
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            // 2. Llamada al Servicio (Lógica de Negocio)
            var result = await _authService.RegisterAsync(dto);
            
            // 3. Respuesta HTTP 201 Created (Recurso creado exitosamente)
            // Se usa CreatedAtAction para retornar la URL del recurso creado.
            return CreatedAtAction(
                actionName: nameof(Login), // Podrías redirigir a un endpoint de perfil si existiera
                routeValues: new { email = result.Usuario.Email }, 
                value: result
            );
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("correo electrónico ya se encuentra registrado"))
        {
            // HTTP 409 Conflict para manejar la regla de negocio de email único
            return Conflict(new { message = ex.Message });
        }
        // Cualquier otra excepción será capturada por el GlobalExceptionHandlerMiddleware
    }

    /// <summary>
    /// Autentica al usuario y retorna un JWT.
    /// </summary>
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AuthResponseDto))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromBody] LoginUsuarioDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var result = await _authService.LoginAsync(dto);
            
            // HTTP 200 OK (La autenticación es exitosa)
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            // HTTP 401 Unauthorized para credenciales inválidas (definido en AuthService)
            return Unauthorized(new { message = ex.Message });
        }
        // El GlobalExceptionHandlerMiddleware captura otros errores.
    }
}