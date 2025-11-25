using Aventour.Application.DTOs.Guias;
using Aventour.Application.Interfaces.Services;

using Aventour.Domain.Interfaces;
using Aventour.Domain.Enums;
using Aventour.Domain.Models; // <--- Añadir using

namespace Aventour.Application.Services;

public class GuiaService : IGuiaService
{
    private readonly IUnitOfWork _unitOfWork;

    public GuiaService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<GuiaResponseDto> CreateGuiaAsync(GuiaCreacionDto dto)
    {
        var guiaEntity = new AgenciasGuia
        {
            Nombre = dto.NombreComercial,
            WhatsappContacto = dto.WhatsappContacto,
            Email = dto.EmailContacto,
            Descripcion = dto.Descripcion,
            
            // ===> CORRECCIÓN AQUÍ <===
            // Asignamos el valor del Enum directamente
            Tipo = TipoAgenciaGuia.Guia, 
            
            Validado = false,
            PuntuacionMedia = 0
        };

        await _unitOfWork.AgenciasGuias.AddAsync(guiaEntity);
        await _unitOfWork.CompleteAsync();

        return MapToDto(guiaEntity);
    }

    public async Task<IEnumerable<GuiaResponseDto>> GetAllGuiasAsync()
    {
        var todos = await _unitOfWork.AgenciasGuias.GetAllAsync();
        
        // ===> CORRECCIÓN AQUÍ <===
        // Comparamos contra el Enum
        return todos.Where(x => x.Tipo == TipoAgenciaGuia.Guia).Select(MapToDto);
    }

    public async Task<GuiaResponseDto?> GetGuiaByIdAsync(int id)
    {
        var guia = await _unitOfWork.AgenciasGuias.GetByIdAsync(id);
        
        // ===> CORRECCIÓN AQUÍ <===
        if (guia == null || guia.Tipo != TipoAgenciaGuia.Guia) return null;
        
        return MapToDto(guia);
    }

    public async Task UpdateGuiaAsync(int id, GuiaCreacionDto dto)
    {
        var guia = await _unitOfWork.AgenciasGuias.GetByIdAsync(id);
        
        // ===> CORRECCIÓN AQUÍ <===
        if (guia == null || guia.Tipo != TipoAgenciaGuia.Guia) 
            throw new KeyNotFoundException($"No se encontró el guía con ID {id}");

        guia.Nombre = dto.NombreComercial;
        guia.WhatsappContacto = dto.WhatsappContacto;
        guia.Email = dto.EmailContacto;
        guia.Descripcion = dto.Descripcion;

        _unitOfWork.AgenciasGuias.Update(guia);
        await _unitOfWork.CompleteAsync();
    }

    public async Task DeleteGuiaAsync(int id)
    {
        var guia = await _unitOfWork.AgenciasGuias.GetByIdAsync(id);
        
        // ===> CORRECCIÓN AQUÍ <===
        if (guia == null || guia.Tipo != TipoAgenciaGuia.Guia) 
            throw new KeyNotFoundException($"No se encontró el guía con ID {id}");

        _unitOfWork.AgenciasGuias.Delete(guia);
        await _unitOfWork.CompleteAsync();
    }

    private static GuiaResponseDto MapToDto(AgenciasGuia entity)
    {
        return new GuiaResponseDto
        {
            IdAgencia = entity.IdAgencia,
            NombreComercial = entity.Nombre,
            WhatsappContacto = entity.WhatsappContacto,
            EmailContacto = entity.Email,
            Descripcion = entity.Descripcion,
            PuntuacionMedia = entity.PuntuacionMedia
        };
    }
}