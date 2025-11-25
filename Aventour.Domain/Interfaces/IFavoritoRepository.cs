using Aventour.Domain.Models;

namespace Aventour.Domain.Interfaces;

// Dentro de la interfaz IFavoritoRepository (Dominio)
public interface IFavoritoRepository : IGenericRepository<Favorito>
{
/// Soluciona los errores de 'GetByKeysAsync'
Task<Favorito?> GetByKeysAsync(int userId, int idEntidad, string tipoEntidad);
    
// Soluciona el error de 'GetUserAsync'
Task<IEnumerable<Favorito>> GetByUserAsync(int userId, string tipoEntidad);
}