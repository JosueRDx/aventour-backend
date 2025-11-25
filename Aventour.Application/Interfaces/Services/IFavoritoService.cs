using Aventour.Application.DTOs.Favoritos;

namespace Aventour.Application.Interfaces.Services;

/// <summary>
/// Contrato para gestionar los destinos favoritos de un usuario.
/// </summary>
public interface IFavoritoService
{
    Task<bool> AddFavoriteAsync(int userId, int idEntidad, string tipoEntidad);
    Task<bool> RemoveFavoriteAsync(int userId, int idDestino);
    Task<IEnumerable<DestinoFavoritoResponseDto>> GetUserFavoritesAsync(int userId);
    Task<bool> IsDestinationFavoriteAsync(int userId, int idDestino);
    Task<bool> AddFavoriteAsync(int userId, int idDestino);
}