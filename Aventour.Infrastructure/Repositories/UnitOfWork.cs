using Aventour.Domain.Interfaces;
using Aventour.Domain.Models;
using Aventour.Infrastructure.Persistence;

namespace Aventour.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AventourDbContext _context;
    private IFavoritoRepository _favoritos;

    // Propiedades privadas para asegurar la inicialización diferida (opcional) o simple
    // o para asegurar que solo una instancia de cada repositorio se crea por UoW.
    public IAgenciaRepository Agencias { get; }
    public IUsuarioRepository Usuarios { get; }
    public IDestinoTuristicoRepository DestinosTuristicos { get; }
    public IResenaRepository Resenas { get; }
    public IAgenciaGuiaRepository AgenciasGuias { get; }
    
    // Repositorios que solo necesitan las operaciones genéricas
    public IGenericRepository<RutasPersonalizada> RutasPersonalizadas { get; }
    public IGenericRepository<DetalleRuta> DetalleRutas { get; }
    public IGenericRepository<PacksRutasAgencium> PacksRutasAgencia { get; }
    public IGenericRepository<DetallePackDestino> DetallePackDestinos { get; }
    public IGenericRepository<HotelesRestaurante> HotelesRestaurantes { get; }

    IFavoritoRepository IUnitOfWork.Favoritos => _favoritos;

    public IGenericRepository<Favorito> Favoritos { get; }


    public UnitOfWork(AventourDbContext context)
    {
        _context = context;
        
        // Inicialización de Repositorios específicos
        Usuarios = new UsuarioRepository(_context);
        DestinosTuristicos = new DestinoTuristicoRepository(_context);
        Resenas = new ResenaRepository(_context);
        AgenciasGuias = new AgenciaGuiaRepository(_context);
        
        // Inicialización de Repositorios Genéricos
        RutasPersonalizadas = new GenericRepository<RutasPersonalizada>(_context);
        DetalleRutas = new GenericRepository<DetalleRuta>(_context);
        PacksRutasAgencia = new GenericRepository<PacksRutasAgencium>(_context);
        DetallePackDestinos = new GenericRepository<DetallePackDestino>(_context);
        HotelesRestaurantes = new GenericRepository<HotelesRestaurante>(_context);
        Favoritos = new GenericRepository<Favorito>(_context);
    }

    public async Task<int> CompleteAsync()
    {
        // El corazón del UoW: Aquí se guarda la transacción
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
        // Permite la liberación manual de recursos.
    }
}