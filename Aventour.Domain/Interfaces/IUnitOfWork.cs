using System;
using System.Threading.Tasks;
using Aventour.Domain.Models;

namespace Aventour.Domain.Interfaces;

/// <summary>
/// Define la Unidad de Trabajo que agrupa todos los repositorios y gestiona la transacción de la base de datos.
/// Esto asegura que todos los cambios en una operación de negocio sean atómicos (transacciones).
/// </summary>
public interface IUnitOfWork : IDisposable
{
    // --- Propiedades para acceder a todos los repositorios específicos ---
    
    // Repositorios Core:
    IUsuarioRepository Usuarios { get; }
    IDestinoTuristicoRepository DestinosTuristicos { get; }
    IResenaRepository Resenas { get; }

    // Repositorios de Proveedores:
    IAgenciaGuiaRepository AgenciasGuias { get; }

    // Repositorios de Rutas:
    IGenericRepository<RutasPersonalizada> RutasPersonalizadas { get; }
    IGenericRepository<DetalleRuta> DetalleRutas { get; }
    
    // Repositorios de Packs:
    IGenericRepository<PacksRutasAgencium> PacksRutasAgencia { get; }
    IGenericRepository<DetallePackDestino> DetallePackDestinos { get; }

    // Repositorios de Lugares/Favoritos (Usando el genérico si no requieren lógica específica compleja):
    IGenericRepository<HotelesRestaurante> HotelesRestaurantes { get; }
    IGenericRepository<Favorito> Favoritos { get; }
    
    
    
    
    
    
    // ----------------------------------------------------------------------

    /// <summary>
    /// Guarda de forma asíncrona todos los cambios realizados en el contexto.
    /// Representa la confirmación de la transacción.
    /// </summary>
    /// <returns>El número de objetos escritos en la base de datos.</returns>
    Task<int> CompleteAsync();
    
    /// <summary>
    /// Método para descartar o liberar recursos no gestionados.
    /// </summary>
    new void Dispose();
}