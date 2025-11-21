using Aventour.Domain.Interfaces;
using Aventour.Domain.Models;
using Aventour.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Aventour.Infrastructure.Repositories;

public class AgenciaGuiaRepository : GenericRepository<AgenciasGuia>, IAgenciaGuiaRepository
{
    public AgenciaGuiaRepository(AventourDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<AgenciasGuia>> GetUnvalidatedAgenciesAsync()
    {
        return await _dbSet.AsNoTracking()
            .Where(a => a.Validado == false)
            .OrderBy(a => a.Nombre)
            .ToListAsync();
    }

    public async Task<bool> SetValidationStatusAsync(int idAgencia, bool isValidated)
    {
        var agencia = await GetByIdAsync(idAgencia);
        if (agencia == null) return false;

        agencia.Validado = isValidated;
        Update(agencia); // Se marca para actualización en el contexto
        
        // No llamamos a SaveChanges, esto lo hace el UnitOfWork.
        return true;
    }

    public async Task<IEnumerable<AgenciasGuia>> GetTopRatedAsync(int count)
    {
        return await _dbSet.AsNoTracking()
            .OrderByDescending(a => a.PuntuacionMedia)
            .Take(count)
            .ToListAsync();
    }
}