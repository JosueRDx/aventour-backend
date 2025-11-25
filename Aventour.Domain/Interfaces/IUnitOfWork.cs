using System;
using System.Threading.Tasks;
using Aventour.Domain.Models;

namespace Aventour.Domain.Interfaces;

public interface IUnitOfWork : IDisposable
{
    // ... (Mantener IAgenciaRepository Agencias y IUsuarioRepository Usuarios)
    IAgenciaRepository Agencias { get; }
    IUsuarioRepository Usuarios { get; }
    IDestinoTuristicoRepository DestinosTuristicos { get; }
    IResenaRepository Resenas { get; }

    // Repositorios de Proveedores:
    IAgenciaGuiaRepository AgenciasGuias { get; } // Asumiendo que esta existe
    
    // Repositorios de Rutas:
    IGenericRepository<RutasPersonalizada> RutasPersonalizadas { get; }
    IGenericRepository<DetalleRuta> DetalleRutas { get; }
    
    // Repositorios de Packs:
    IGenericRepository<PacksRutasAgencium> PacksRutasAgencia { get; }
    IGenericRepository<DetallePackDestino> DetallePackDestinos { get; }

    // Repositorios de Lugares/Favoritos:
    IGenericRepository<HotelesRestaurante> HotelesRestaurantes { get; }
    
    // **CORRECCIÓN:** Debe ser la interfaz específica (IFavoritoRepository)
    // para exponer los métodos como GetByKeysAsync y GetByUserAsync.
    IFavoritoRepository Favoritos { get; }
    
    Task<int> CompleteAsync();
    new void Dispose();
}