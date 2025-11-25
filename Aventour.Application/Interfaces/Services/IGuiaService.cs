using Aventour.Application.DTOs.Guias;

namespace Aventour.Application.Interfaces.Services;

public interface IGuiaService
{
    Task<GuiaResponseDto> CreateGuiaAsync(GuiaCreacionDto dto);
    Task<IEnumerable<GuiaResponseDto>> GetAllGuiasAsync();
    Task<GuiaResponseDto?> GetGuiaByIdAsync(int id);
    Task UpdateGuiaAsync(int id, GuiaCreacionDto dto);
    Task DeleteGuiaAsync(int id);
}