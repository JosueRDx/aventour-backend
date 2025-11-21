// Proyecto: Aventour.Application
// Archivo: Configuration/ApplicationServicesExtension.cs

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
            // services.AddScoped<IPedidoManager, PedidoManager>();

            // --- 2. REGISTRO DE MEDIATR (Si usas CQRS) ---
            // services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            
            return services;
        }
    }
}