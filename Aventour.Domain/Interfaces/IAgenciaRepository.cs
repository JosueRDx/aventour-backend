// Archivo: IAgenciaRepository.cs

using Aventour.Domain.Models;

namespace Aventour.Domain.Interfaces;

// Hereda de tu repositorio genérico para obtener métodos como GetByIdAsync
public interface IAgenciaRepository : IGenericRepository<AgenciasGuia>
{
    // Hereda GetByIdAsync de IGenericRepository<AgenciasGuia>
}
