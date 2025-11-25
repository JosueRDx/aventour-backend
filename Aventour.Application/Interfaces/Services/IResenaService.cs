using Aventour.Application.DTOs.Resenas;

namespace Aventour.Application.Interfaces.Services;

public interface IResenaService
{
    // CRUD del Usuario sobre su Reseña
    Task<ResenaResponseDto> CreateResenaAsync(int userId, int idDestino, ResenaCreacionDto dto);
    Task<ResenaResponseDto> UpdateResenaAsync(int idResena, int userId, ResenaCreacionDto dto);
    Task DeleteResenaAsync(int idResena, int userId);
    
    // Lectura de Reseñas
    Task<IEnumerable<ResenaResponseDto>> GetResenasByDestinoAsync(int idDestino);
    Task<PuntuacionMediaResponseDto> GetAverageRatingByDestinoAsync(int idDestino);
}