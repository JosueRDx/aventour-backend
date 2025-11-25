using Aventour.Application.DTOs.Resenas;
using Aventour.Application.Interfaces.Services;
using Aventour.Domain.Interfaces;
using Aventour.Domain.Models;

namespace Aventour.Application.Services;

public class ResenaService : IResenaService
{
    private readonly IUnitOfWork _unitOfWork;
    
    // Usaremos una constante para determinar qué entidad es. En un sistema más grande, usaríamos un Enum o tabla de tipos.
    private const string EntityTypeDestino = "DESTINO"; 

    public ResenaService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    // Helper de Mapeo
    private ResenaResponseDto MapToResponseDto(Resena entity)
    {
        return new ResenaResponseDto
        {
            IdResena = entity.IdResena,
            IdDestino = entity.IdEntidad, // Mapeando IdEntidad a IdDestino
            Puntuacion = entity.Puntuacion,
            Comentario = entity.Comentario,
            FechaCreacion = entity.FechaCreacion,
            // Asume que la navegación Usuario está cargada (eager loading)
            AutorNombreCompleto = $"{entity.IdUsuarioNavigation.Nombres} {entity.IdUsuarioNavigation.Apellidos}" 
        };
    }

    // Lógica Transaccional Central
    private async Task RecalculateAverageRatingAsync(int idDestino)
    {
        // 1. Obtener la nueva media desde la base de datos
        // Se asume un método en el repositorio de Resenas que calcula la media
        var (media, count) = await _unitOfWork.Resenas.CalculateAverageRatingAsync(idDestino);
        
        // 2. Obtener la entidad Destino
        var destino = await _unitOfWork.DestinosTuristicos.GetByIdAsync(idDestino);
        
        if (destino != null)
        {
            // 3. Actualizar la propiedad y marcar para SaveChanges (en UoW.CompleteAsync)
            destino.PuntuacionMedia = media;
            _unitOfWork.DestinosTuristicos.Update(destino);
        }
    }

    // --- Implementación de Métodos CRUD ---

    public async Task<ResenaResponseDto> CreateResenaAsync(int userId, int idDestino, ResenaCreacionDto dto)
    {
        // Validar que no haya una reseña previa del mismo usuario para el mismo destino.
        var existingResena = await _unitOfWork.Resenas.GetByUserAndEntityAsync(userId, idDestino);
        if (existingResena != null)
        {
            throw new InvalidOperationException("Ya existe una reseña de este usuario para este destino.");
        }
        
        // 1. Validar existencia del destino
        var destino = await _unitOfWork.DestinosTuristicos.GetByIdAsync(idDestino);
        if (destino == null)
        {
            throw new KeyNotFoundException($"Destino con ID {idDestino} no encontrado.");
        }

        var newResena = new Resena
        {
            IdUsuario = userId,
            IdEntidad = idDestino, // Usamos IdEntidad como IdDestino
            Puntuacion = dto.Puntuacion,
            Comentario = dto.Comentario,
            // IdTipoEntidad = EntityTypeDestino // Si tuvieras este campo para diferenciar
            FechaCreacion = DateTime.UtcNow
        };

        await _unitOfWork.Resenas.AddAsync(newResena);
        
        // 2. Recalcular y actualizar la media del destino
        await RecalculateAverageRatingAsync(idDestino);
        
        await _unitOfWork.CompleteAsync(); // Persistir ambas operaciones (Reseña y Destino)
        
        // Cargar el usuario para mapear el DTO de respuesta (Asumimos carga manual o auto-mapeo)
        newResena.IdUsuarioNavigation = await _unitOfWork.Usuarios.GetByIdAsync(userId); 
        return MapToResponseDto(newResena);
    }

    public async Task<ResenaResponseDto> UpdateResenaAsync(int idResena, int userId, ResenaCreacionDto dto)
    {
        var resena = await _unitOfWork.Resenas.GetByIdAsync(idResena);

        if (resena == null)
        {
            throw new KeyNotFoundException($"Reseña con ID {idResena} no encontrada.");
        }
        
        // Validación de propiedad
        if (resena.IdUsuario != userId)
        {
            throw new UnauthorizedAccessException("No tiene permiso para editar esta reseña.");
        }

        // 1. Aplicar cambios
        resena.Puntuacion = dto.Puntuacion;
        resena.Comentario = dto.Comentario;
        
        _unitOfWork.Resenas.Update(resena);

        // 2. Recalcular y actualizar la media
        await RecalculateAverageRatingAsync(resena.IdEntidad);
        
        await _unitOfWork.CompleteAsync();
        
        resena.IdUsuarioNavigation = await _unitOfWork.Usuarios.GetByIdAsync(userId); 
        return MapToResponseDto(resena);
    }

    public async Task DeleteResenaAsync(int idResena, int userId)
    {
        var resena = await _unitOfWork.Resenas.GetByIdAsync(idResena);

        if (resena == null)
        {
            throw new KeyNotFoundException($"Reseña con ID {idResena} no encontrada.");
        }
        
        // Validación de propiedad
        if (resena.IdUsuario != userId)
        {
            throw new UnauthorizedAccessException("No tiene permiso para eliminar esta reseña.");
        }
        
        var idDestino = resena.IdEntidad;
        
        // 1. Eliminar reseña
        _unitOfWork.Resenas.Delete(resena);

        // 2. Recalcular y actualizar la media
        await RecalculateAverageRatingAsync(idDestino);
        
        await _unitOfWork.CompleteAsync();
    }
    
    // --- Implementación de Lectura ---
    
    public async Task<IEnumerable<ResenaResponseDto>> GetResenasByDestinoAsync(int idDestino)
    {
        var resenas = await _unitOfWork.Resenas.GetResenasByEntityIdAsync(idDestino);
        return resenas.Select(MapToResponseDto).ToList();
    }
    
    public async Task<PuntuacionMediaResponseDto> GetAverageRatingByDestinoAsync(int idDestino)
    {
        var (media, count) = await _unitOfWork.Resenas.CalculateAverageRatingAsync(idDestino);
        
        return new PuntuacionMediaResponseDto
        {
            Media = media,
            TotalResenas = count
        };
    }
}