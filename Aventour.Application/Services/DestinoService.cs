using Aventour.Application.DTOs.Destinos;
using Aventour.Application.Interfaces.Services;
using Aventour.Domain.Interfaces;
using Aventour.Domain.Models;
using System.Collections.Generic;

namespace Aventour.Application.Services;

public class DestinoService : IDestinoService
{
    private readonly IUnitOfWork _unitOfWork;

    public DestinoService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // --- Helpers de Mapeo (Profesional: Usar AutoMapper) ---

    private DestinoResponseDto MapToResponseDto(DestinosTuristico entity)
    {
        if (entity == null) return null;
        return new DestinoResponseDto
        {
            IdDestino = entity.IdDestino,
            Nombre = entity.Nombre,
            DescripcionBreve = entity.DescripcionBreve,
            Tipo = entity.Tipo,
            Latitud = entity.Latitud,
            Longitud = entity.Longitud,
            CostoEntrada = entity.CostoEntrada,
            PuntuacionMedia = entity.PuntuacionMedia
        };
    }
    
    private void MapCreationDtoToEntity(DestinosTuristico entity, DestinoCreacionDto dto)
    {
        entity.Nombre = dto.Nombre;
        entity.DescripcionBreve = dto.DescripcionBreve;
        entity.DescripcionCompleta = dto.DescripcionCompleta;
        entity.Tipo = dto.Tipo;
        entity.Latitud = dto.Latitud;
        entity.Longitud = dto.Longitud;
        entity.HorarioAtencion = dto.HorarioAtencion;
        entity.CostoEntrada = dto.CostoEntrada;
        entity.UrlFotoPrincipal = dto.UrlFotoPrincipal;
        // La puntuación media (PuntuacionMedia) no se debe establecer en la creación/actualización por el administrador.
    }

    // --- Implementación de IDestinoService ---

    public async Task<DestinoResponseDto> CreateDestinoAsync(DestinoCreacionDto dto)
    {
        var nuevoDestino = new DestinosTuristico();
        MapCreationDtoToEntity(nuevoDestino, dto);
        nuevoDestino.PuntuacionMedia = 0.0M; // Inicialización de valor

        await _unitOfWork.DestinosTuristicos.AddAsync(nuevoDestino);
        await _unitOfWork.CompleteAsync(); // Persistencia de datos

        return MapToResponseDto(nuevoDestino);
    }

    public async Task<DestinoResponseDto?> GetDestinoByIdAsync(int id)
    {
        var destino = await _unitOfWork.DestinosTuristicos.GetByIdAsync(id);
        return MapToResponseDto(destino);
    }

    public async Task<IEnumerable<DestinoResponseDto>> GetAllDestinosAsync()
    {
        var destinos = await _unitOfWork.DestinosTuristicos.GetAllAsync();
        return destinos.Select(MapToResponseDto).ToList();
    }

    public async Task UpdateDestinoAsync(int id, DestinoCreacionDto dto)
    {
        var destinoExistente = await _unitOfWork.DestinosTuristicos.GetByIdAsync(id);
        
        if (destinoExistente == null)
        {
            // Excepción para manejar el 404. Capturado por el Middleware.
            throw new KeyNotFoundException($"Destino Turístico con ID {id} no encontrado para actualizar.");
        }

        MapCreationDtoToEntity(destinoExistente, dto);
        
        _unitOfWork.DestinosTuristicos.Update(destinoExistente);
        await _unitOfWork.CompleteAsync();
    }

    public async Task DeleteDestinoAsync(int id)
    {
        var destinoExistente = await _unitOfWork.DestinosTuristicos.GetByIdAsync(id);
        
        if (destinoExistente == null)
        {
            // Excepción para manejar el 404. Capturado por el Middleware.
            throw new KeyNotFoundException($"Destino Turístico con ID {id} no encontrado para eliminar.");
        }

        _unitOfWork.DestinosTuristicos.Delete(destinoExistente);
        await _unitOfWork.CompleteAsync();
    }
}