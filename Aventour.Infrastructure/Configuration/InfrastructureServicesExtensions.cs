// Proyecto: Aventour.Infrastructure
// Archivo: Configuration/InfrastructureServicesExtension.cs

using Aventour.Domain.Interfaces;
using Aventour.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql.EntityFrameworkCore.PostgreSQL;
 
// Asume que tus Interfaces de Repositorio están en: Aventour.Domain.Interfaces
// using Aventour.Domain.Interfaces; 

namespace Aventour.Infrastructure.Configuration
{
    public static class InfrastructureServicesExtension
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services, 
            IConfiguration configuration)
        {
            // --- 1. CONEXIÓN A LA BASE DE DATOS (RENDER) ---
            var connectionString = configuration.GetConnectionString("PostgresConnection");
            
            services.AddDbContext<AventourDbContext>(options =>
                // Usar la cadena de Render y el proveedor Npgsql
                options.UseNpgsql(connectionString)
            );

            // 1. Registro del Patrón Repository (Adaptadores)
            // Registro del Genérico
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            // Registro de los Específicos
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IDestinoTuristicoRepository, DestinoTuristicoRepository>();
            services.AddScoped<IAgenciaGuiaRepository, AgenciaGuiaRepository>();
            services.AddScoped<IResenaRepository, ResenaRepository>();

            // 2. Registro del Patrón Unit of Work
            // Este es el punto principal de inyección para la capa Application
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}