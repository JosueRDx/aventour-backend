using Aventour.Application.DTOs.Agencias;
using Aventour.Domain.Models; // Usar el modelo de dominio para la respuesta simple

namespace Aventour.Application.Interfaces.Services;

public interface IAgenciaAdminService
{
    Task<IEnumerable<AgenciasGuia>> GetUnvalidatedAgenciesAsync();
    Task<bool> ValidateAgencyAsync(int idAgencia, bool isValidated);
}