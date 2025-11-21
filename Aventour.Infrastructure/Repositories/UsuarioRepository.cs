using Aventour.Domain.Interfaces;
using Aventour.Domain.Models;
using Aventour.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Aventour.Infrastructure.Repositories;

public class UsuarioRepository : GenericRepository<Usuario>, IUsuarioRepository
{
    public UsuarioRepository(AventourDbContext context) : base(context)
    {
    }

    public async Task<Usuario?> GetByEmailAsync(string email)
    {
        return await _dbSet.AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<Usuario?> GetByTokenConfirmacionAsync(string token)
    {
        return await _dbSet.FirstOrDefaultAsync(u => u.TokenConfirmacion == token && u.SesionActiva == false);
    }

    public async Task<IEnumerable<Usuario>> GetAllAdministratorsAsync()
    {
        return await _dbSet.AsNoTracking()
            .Where(u => u.EsAdministrador == true)
            .ToListAsync();
    }
}