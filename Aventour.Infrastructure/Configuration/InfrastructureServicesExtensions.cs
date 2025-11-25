using Aventour.Application.Interfaces.Utilities;
using Aventour.Domain.Enums;
using Aventour.Domain.Interfaces;
using Aventour.Infrastructure.Authentication;
using Aventour.Infrastructure.Persistence;
using Aventour.Infrastructure.Repositories;
using Aventour.Infrastructure.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace Aventour.Infrastructure.Configuration
{
    public static class InfrastructureServicesExtension
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services, 
            IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("PostgresConnection");

            // 1. Configuración del DbContext con mapeo de Enums Directo
            services.AddDbContext<AventourDbContext>(options =>
                options.UseNpgsql(connectionString, o => 
                {
                    // Aquí le decimos explícitamente a EF/Npgsql que trate estos tipos como Enums
                    o.MapEnum<TipoAgenciaGuia>("tipo_agencia_guia");
                    o.MapEnum<TipoResena>("tipo_resena");
                    o.MapEnum<TipoFavorito>("tipo_favorito");
                    o.MapEnum<TipoHotelRest>("tipo_hotel_rest");
                })
            );

            // 2. Registro de Repositorios
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IDestinoTuristicoRepository, DestinoTuristicoRepository>();
            services.AddScoped<IAgenciaGuiaRepository, AgenciaGuiaRepository>();
            services.AddScoped<IResenaRepository, ResenaRepository>();

            // 3. Registro de Unit of Work y Servicios
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<IPasswordHasher, PasswordHasher>();
            services.AddSingleton<IJwtProvider, JwtProvider>();
            services.AddScoped<IExcelExporter, ClosedXmlExporter>();

            return services;
        }
    }
}