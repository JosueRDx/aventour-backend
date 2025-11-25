using Aventour.Application.DTOs.Favoritos;
using Aventour.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims; 

namespace Aventour.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
// [Authorize] // Sigue comentado según tu solicitud
public class FavoritosController : ControllerBase
{
    private readonly IFavoritoService _favoritoService;

    public FavoritosController(IFavoritoService favoritoService)
    {
        _favoritoService = favoritoService;
    }
    
    // Helper MODIFICADO: Devuelve 0 si no hay token o no hay ID, NO lanza excepción.
    private int GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        // Si el claim existe y es un número válido, lo devolvemos.
        if (!string.IsNullOrEmpty(userIdClaim) && int.TryParse(userIdClaim, out int userId))
        {
            return userId;
        }
        
        // Si no hay token o no se puede parsear, devolvemos 0.
        // Esto permite que el controlador siga sin lanzar la excepción de acceso no autorizado.
        return 0; 
    }

    /// <summary>
    /// Marca un destino específico como favorito para el usuario (usará ID 0 si no está autenticado).
    /// </summary>
    [HttpPost("{idDestino}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddFavorite(int idDestino)
    {
        int userId = GetCurrentUserId();
        
        // 🚨 ADVERTENCIA: Si userId es 0, es probable que la base de datos falle.
        if (userId == 0)
        {
            return Unauthorized(new { message = "Se requiere autenticación para agregar favoritos." });
        }
        
        try
        {
            // Se asume que AddFavoriteAsync maneja la lógica de validación
            var added = await _favoritoService.AddFavoriteAsync(userId, idDestino, "DESTINO");
            
            if (!added)
            {
                return Conflict(new { message = "El destino ya está marcado como favorito." });
            }
            
            return Created(string.Empty, null); 
        }
        catch (KeyNotFoundException ex)
        {
             return NotFound(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Elimina un destino de la lista de favoritos del usuario (usará ID 0 si no está autenticado).
    /// </summary>
    [HttpDelete("{idDestino}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> RemoveFavorite(int idDestino)
    {
        int userId = GetCurrentUserId();
        
        // 🚨 ADVERTENCIA: Si userId es 0, es probable que la base de datos falle.
        if (userId == 0)
        {
            // Devolvemos 204 para mantener la idempotencia y evitar que la excepción llegue al usuario.
            return NoContent(); 
        }
        
        await _favoritoService.RemoveFavoriteAsync(userId, idDestino);
        
        return NoContent();
    }

    /// <summary>
    /// Lista todos los destinos favoritos del usuario (usará ID 0 si no está autenticado).
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<DestinoFavoritoResponseDto>))]
    public async Task<IActionResult> GetFavorites()
    {
        int userId = GetCurrentUserId();

        // 🚨 ADVERTENCIA: Si userId es 0, GetUserFavoritesAsync devolverá una lista vacía
        // o fallará si el servicio intenta validar que el usuario exista.
        if (userId == 0)
        {
            return Ok(Enumerable.Empty<DestinoFavoritoResponseDto>()); 
        }
        
        var favorites = await _favoritoService.GetUserFavoritesAsync(userId);
        return Ok(favorites);
    }

    /// <summary>
    /// Verifica si un destino específico está marcado como favorito por el usuario (usará ID 0 si no está autenticado).
    /// </summary>
    [HttpGet("{idDestino}/status")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(object))]
    public async Task<IActionResult> CheckFavoriteStatus(int idDestino)
    {
        int userId = GetCurrentUserId();

        // Si el usuario es anónimo (ID 0), simplemente decimos que no es favorito.
        if (userId == 0)
        {
            return Ok(new { isFavorite = false });
        }
        
        var isFavorite = await _favoritoService.IsDestinationFavoriteAsync(userId, idDestino);
        
        return Ok(new { isFavorite });
    }
}