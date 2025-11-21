using Aventour.Application.Interfaces.Services;
using Aventour.Domain.Interfaces;
using Aventour.Domain.Models;

namespace Aventour.Application.Services;

public class AgenciaAdminService : IAgenciaAdminService
{
    private readonly IUnitOfWork _unitOfWork;

    public AgenciaAdminService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<AgenciasGuia>> GetUnvalidatedAgenciesAsync()
    {
        // Usa el método específico del Repositorio de Agencias (definido en Fase 2)
        return await _unitOfWork.AgenciasGuias.GetUnvalidatedAgenciesAsync();
    }

    public async Task<bool> ValidateAgencyAsync(int idAgencia, bool isValidated)
    {
        // El método del repositorio actualiza la entidad en memoria y retorna un booleano
        // indicando si la agencia fue encontrada.
        var success = await _unitOfWork.AgenciasGuias.SetValidationStatusAsync(idAgencia, isValidated);
        
        if (success)
        {
            // Si la agencia fue encontrada y marcada para cambio, persistir la transacción.
            await _unitOfWork.CompleteAsync();
            return true;
        }
        
        // Si no se encontró, lanzar excepción que será mapeada a 404 por el Middleware
        throw new KeyNotFoundException($"Agencia/Guía con ID {idAgencia} no encontrado para validar.");
    }
}