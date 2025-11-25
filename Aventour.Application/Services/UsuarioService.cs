using Aventour.Application.DTOs.Usuario;
using Aventour.Application.Interfaces.Services;
using Aventour.Application.Interfaces.Utilities;
using Aventour.Domain.Interfaces;
using Aventour.Domain.Models;

namespace Aventour.Application.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;

    public UsuarioService(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
    }

    // Helper de mapeo
    private UsuarioResponseDto MapToResponseDto(Usuario entity)
    {
        if (entity == null) throw new KeyNotFoundException("Usuario no encontrado.");
        
        return new UsuarioResponseDto
        {
            IdUsuario = entity.IdUsuario,
            Nombres = entity.Nombres,
            Apellidos = entity.Apellidos,
            Email = entity.Email,
            Edad = entity.Edad,
            EstadoCivil = entity.EstadoCivil,
            FechaRegistro = entity.FechaRegistro,
            EsAdministrador = entity.EsAdministrador
        };
    }

    public async Task<UsuarioResponseDto> GetProfileAsync(int userId)
    {
        var usuario = await _unitOfWork.Usuarios.GetByIdAsync(userId);
        return MapToResponseDto(usuario);
    }

    public async Task<UsuarioResponseDto> UpdateProfileAsync(int userId, UsuarioActualizacionDto dto)
    {
        var usuario = await _unitOfWork.Usuarios.GetByIdAsync(userId);

        if (usuario == null)
        {
            throw new KeyNotFoundException($"Usuario con ID {userId} no encontrado.");
        }

        // Aplicar actualizaciones
        usuario.Nombres = dto.Nombres;
        usuario.Apellidos = dto.Apellidos;
        usuario.Edad = dto.Edad;
        usuario.EstadoCivil = dto.EstadoCivil;

        _unitOfWork.Usuarios.Update(usuario);
        await _unitOfWork.CompleteAsync();

        return MapToResponseDto(usuario);
    }

    public async Task<bool> ChangePasswordAsync(int userId, UsuarioCambioPasswordDto dto)
    {
        var usuario = await _unitOfWork.Usuarios.GetByIdAsync(userId);
        
        if (usuario == null)
        {
            throw new KeyNotFoundException($"Usuario con ID {userId} no encontrado.");
        }

        // 1. Verificar contraseña actual
        if (!_passwordHasher.Verify(dto.CurrentPassword, usuario.PasswordHash))
        {
            // Práctica profesional: Usar un mensaje genérico para no dar pistas al atacante
            throw new UnauthorizedAccessException("Credenciales incorrectas.");
        }

        // 2. Hashear y actualizar nueva contraseña
        usuario.PasswordHash = _passwordHasher.Hash(dto.NewPassword);
        
        _unitOfWork.Usuarios.Update(usuario);
        await _unitOfWork.CompleteAsync();

        return true;
    }
}