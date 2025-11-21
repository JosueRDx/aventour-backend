using System.Reflection;
using Microsoft.OpenApi.Models; 
using Aventour.Application.Configuration; // Para AddApplicationServices
using Aventour.Infrastructure.Configuration; // Para AddInfrastructureServices

// 1. Definir la política CORS
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Registrar servicios de Infraestructura (DB, Repositorios) de monento nada 
builder.Services.AddInfrastructureServices(builder.Configuration);

// Registrar servicios de Aplicación (MediatR, CQRS Handlers) de monento nada 
builder.Services.AddApplicationServices();


// Configuración de Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // A. Información de la API (Metadatos)
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Aventour API - Arequipa Turismo",
        Version = "v1",
        Description = "API del sistema Aventour, diseñado para centralizar la información turística regional de Arequipa, permitiendo descubrir destinos, organizar rutas y contactar prestadores de servicios.",
        Contact = new OpenApiContact
        {
            Name = "Equipo de Desarrollo Aventour",
            Email = "contacto@aventour.com" // Placeholder
        },
        License = new OpenApiLicense
        {
            Name = "Licencia Propia"
        }
    });
    
    // B. Configuración de Seguridad (JWT Bearer Token)
    // Esto añade un botón 'Authorize' en la interfaz de Swagger.
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingrese el token JWT: Bearer {token}"
    });

    // Asignar el requisito de seguridad globalmente a los endpoints
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });

    // C. Habilitar Comentarios XML (Para documentación automática)
    // 1. Obtener el nombre del archivo XML generado por el compilador
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    // 2. Incluir el archivo de documentación de la API principal
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
    
    // 3. Incluir el archivo de documentación de los DTOs/Clases de la capa Application 
    // (Necesitarás configurar tu proyecto Application para generar XML)
    var applicationXmlFile = "Aventour.Application.xml"; 
    var applicationXmlPath = Path.Combine(AppContext.BaseDirectory, applicationXmlFile);
    if (File.Exists(applicationXmlPath))
    {
        options.IncludeXmlComments(applicationXmlPath);
    }
    
    // D. Opcional: Mostrar DTOs en camelCase (Convención de JS)
    options.CustomSchemaIds(type => type.ToString().Replace('+', '.'));
});

var app = builder.Build();

// --- Uso del Middleware de Swagger (Parte que ya tenías) ---
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    // Opcional: Personalizar el título del UI
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Aventour API v1");
        c.RoutePrefix = string.Empty; // Hace que Swagger sea la página de inicio (/)
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();