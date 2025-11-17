using Aventour.Application.Configuration; // Para AddApplicationServices
using Aventour.Infrastructure.Configuration; // Para AddInfrastructureServices

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Registrar servicios de Infraestructura (DB, Repositorios)
builder.Services.AddInfrastructureServices(builder.Configuration);

// Registrar servicios de Aplicación (MediatR, CQRS Handlers)
builder.Services.AddApplicationServices();


// Configuración de Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// --- 2. Middleware y Configuración de la Pipeline ---
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();