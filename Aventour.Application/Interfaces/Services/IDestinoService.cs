using Aventour.Application.DTOs.Destinos;

namespace Aventour.Application.Interfaces.Services;

public interface IDestinoService
{
    Task<DestinoResponseDto> CreateDestinoAsync(DestinoCreacionDto dto);
    Task<DestinoResponseDto?> GetDestinoByIdAsync(int id);
    Task<IEnumerable<DestinoResponseDto>> GetAllDestinosAsync();
    Task UpdateDestinoAsync(int id, DestinoCreacionDto dto); // Usamos el mismo DTO de Creación para simplificar
    Task DeleteDestinoAsync(int id);
}