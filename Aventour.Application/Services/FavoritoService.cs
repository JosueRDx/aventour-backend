using Aventour.Application.DTOs.Favoritos;
using Aventour.Application.Interfaces.Services;
using Aventour.Domain.Interfaces;
using Aventour.Domain.Models;

namespace Aventour.Application.Services;

public class FavoritoService : IFavoritoService
{
    private readonly IUnitOfWork _unitOfWork;
    private IFavoritoService _favoritoServiceImplementation;

    public FavoritoService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // 1. CORRECCIÓN CRÍTICA: Convertido a método asíncrono (MapToResponseDtoAsync)
    // para evitar el uso de .Result y el riesgo de deadlock.
    private async Task<DestinoFavoritoResponseDto?> MapToResponseDtoAsync(Favorito favorito)
    {
        // Usamos await en lugar de .Result
        var destino = await _unitOfWork.DestinosTuristicos.GetByIdAsync(favorito.IdEntidad);

        if (destino == null) 
            return null;

        return new DestinoFavoritoResponseDto
        {
            IdDestino = destino.IdDestino,
            Nombre = destino.Nombre,
            DescripcionBreve = destino.DescripcionBreve,
            UrlFotoPrincipal = destino.UrlFotoPrincipal,
            PuntuacionMedia = destino.PuntuacionMedia
        };
    }

    public async Task<bool> AddFavoriteAsync(int userId, int idEntidad, string tipoEntidad)
    {
        tipoEntidad = tipoEntidad.ToUpper();

        // Verificar que la entidad exista según el tipo
        bool entidadExiste = tipoEntidad switch
        {
            "DESTINO" => await _unitOfWork.DestinosTuristicos.GetByIdAsync(idEntidad) != null,
            "AGENCIA" => await _unitOfWork.Agencias.GetByIdAsync(idEntidad) != null,
            _ => false
        };

        if (!entidadExiste)
            throw new KeyNotFoundException($"No existe la entidad {tipoEntidad} con ID {idEntidad}.");

        // Verificar si ya es favorito
        var existingFavorite = await _unitOfWork.Favoritos.GetByKeysAsync(userId, idEntidad, tipoEntidad);
        if (existingFavorite != null) return false;

        // Se asume que el modelo Favorito.cs fue corregido para incluir TipoEntidad
        var newFavorite = new Favorito
        {
            IdUsuario = userId,
            IdEntidad = idEntidad,
            TipoEntidad = tipoEntidad,
            FechaGuardado = DateTime.UtcNow
        };

        await _unitOfWork.Favoritos.AddAsync(newFavorite);
        await _unitOfWork.CompleteAsync();

        return true;
    }
    
    // 2. IMPLEMENTACIÓN SOLICITADA: Implementa la sobrecarga delegando al método completo.
    public async Task<bool> RemoveFavoriteAsync(int userId, int idDestino)
    {
        // Llama al método general, especificando que la entidad es un DESTINO.
        return await RemoveFavoriteAsync(userId, idDestino, "DESTINO");
    }

    public async Task<bool> RemoveFavoriteAsync(int userId, int idEntidad, string tipoEntidad)
    {
        var existingFavorite = await _unitOfWork.Favoritos.GetByKeysAsync(userId, idEntidad, tipoEntidad);

        if (existingFavorite == null) return false;

        _unitOfWork.Favoritos.Delete(existingFavorite);
        await _unitOfWork.CompleteAsync();

        return true;
    }

    public async Task<IEnumerable<DestinoFavoritoResponseDto>> GetUserFavoritesAsync(int userId)
    {
        // Solo cargamos favoritos de tipo "DESTINO"
        var favorites = await _unitOfWork.Favoritos.GetByUserAsync(userId, "DESTINO");

        // 3. MEJORA: Crear tareas de mapeo concurrentes y usar Task.WhenAll
        var mappingTasks = favorites.Select(MapToResponseDtoAsync); 
        
        var results = await Task.WhenAll(mappingTasks);

        // Filtrar nulos (si un destino no fue encontrado)
        return results.Where(f => f != null).ToList()!;
    }

    public async Task<bool> IsDestinationFavoriteAsync(int userId, int idEntidad)
    {
        var existingFavorite = await _unitOfWork.Favoritos.GetByKeysAsync(userId, idEntidad, "DESTINO");
        return existingFavorite != null;
    }

    public Task<bool> AddFavoriteAsync(int userId, int idDestino)
    {
        return _favoritoServiceImplementation.AddFavoriteAsync(userId, idDestino);
    }
}