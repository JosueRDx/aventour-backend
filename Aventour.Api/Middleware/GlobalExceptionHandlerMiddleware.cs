using System.Net;
using System.Text.Json;

namespace Aventour.Api.Middleware;

/// <summary>
/// Middleware personalizado para capturar todas las excepciones no controladas 
/// y devolver una respuesta JSON estandarizada.
/// </summary>
public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

    public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Excepción no controlada: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        // 500 Internal Server Error es el estado por defecto
        HttpStatusCode statusCode = HttpStatusCode.InternalServerError; 
        string message = "Ha ocurrido un error inesperado en el servidor.";
        string details = exception.Message;

        // Mapeo de excepciones de negocio a códigos HTTP específicos
        if (exception is InvalidOperationException) 
        {
            // 409 Conflict: Usado para violaciones de reglas de negocio (e.g., email ya existe)
            statusCode = HttpStatusCode.Conflict; 
            message = "Conflicto de recurso o violación de regla de negocio.";
        }
        else if (exception is UnauthorizedAccessException) 
        {
            // 401 Unauthorized: Usado por el AuthService para credenciales incorrectas
            statusCode = HttpStatusCode.Unauthorized; 
            message = "Acceso no autorizado. Credenciales inválidas o falta de permiso.";
        }
        // Implementar más mapeos (ej. KeyNotFoundException -> 404 Not Found)

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var errorResponse = new 
        {
            StatusCode = context.Response.StatusCode,
            Message = message,
            // Los detalles solo se deben enviar en entorno de Desarrollo
            Details = details 
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
    }
}

// Clase de extensión para facilitar el registro en Program.cs
public static class GlobalExceptionHandlerMiddlewareExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<GlobalExceptionHandlerMiddleware>();
    }
}