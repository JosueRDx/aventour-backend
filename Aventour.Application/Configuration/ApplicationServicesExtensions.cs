// Proyecto: Aventour.Application
// Archivo: Configuration/ApplicationServicesExtension.cs

using Aventour.Application.DTOs.Auth;
using Aventour.Application.Interfaces.Services;
using Aventour.Application.Interfaces.Utilities;
using Aventour.Application.Services;
using Aventour.Infrastructure.Utilities;
using Microsoft.Extensions.DependencyInjection;
// using MediatR; // Si usas MediatR

namespace Aventour.Application.Configuration
{
    public static class ApplicationServicesExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // --- 1. REGISTRO DE SERVICIOS DE LÓGICA DE NEGOCIO ---
            // Aquí se registrarían tus servicios de aplicación (ej: IUserService, IPedidoManager).
            services.AddScoped<IAuthService, AuthService>();

            // --- 2. REGISTRO DE MEDIATR (Si usas CQRS) ---
            // services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            // 3. Registro de Utilidades y Adaptadores
            services.AddSingleton<IPasswordHasher, PasswordHasher>();
    
            // ** NUEVO: Registro del Exportador Excel **
            services.AddScoped<IExcelExporter, ClosedXmlExporter>();
            // 1. Registro de Servicios de Búsqueda y CRUD (General)
            services.AddScoped<IDestinoService, DestinoService>();
        
            // 2. Registro de Servicios de Administración (Específicos para Admin)
            services.AddScoped<IAgenciaAdminService, AgenciaAdminService>();
            
            
            // ** NUEVO: Módulo de Perfil de Usuario **
            services.AddScoped<IUsuarioService, UsuarioService>();
            
            // ** NUEVO: Módulo de Favoritos **
            services.AddScoped<IFavoritoService, FavoritoService>();
            
            // ** NUEVO: Módulo de Reseñas **
            services.AddScoped<IResenaService, ResenaService>();
            return services;
        }
    }
}