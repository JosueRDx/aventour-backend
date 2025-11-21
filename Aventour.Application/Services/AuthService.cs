using Aventour.Application.DTOs.Auth;
using Aventour.Application.Interfaces.Services;
using Aventour.Application.Interfaces.Utilities;
using Aventour.Domain.Interfaces;
using Aventour.Domain.Models;

namespace Aventour.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtProvider _jwtProvider;

    public AuthService(
        IUnitOfWork unitOfWork, 
        IPasswordHasher passwordHasher,
        IJwtProvider jwtProvider)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegistroUsuarioDto dto)
    {
        // 1. Validar reglas de negocio: Email único
        var existingUser = await _unitOfWork.Usuarios.GetByEmailAsync(dto.Email);
        if (existingUser != null)
        {
            throw new InvalidOperationException("El correo electrónico ya se encuentra registrado.");
        }

        // 2. Hashing de Contraseña
        var passwordHash = _passwordHasher.Hash(dto.Password);
        var confirmationToken = Guid.NewGuid().ToString();
        
        // 3. Mapeo DTO -> Entidad
        var newUser = new Usuario
        {
            Nombres = dto.Nombres,
            Apellidos = dto.Apellidos,
            Email = dto.Email,
            PasswordHash = passwordHash,
            Edad = dto.Edad,
            EstadoCivil = dto.EstadoCivil,
            FechaRegistro = DateTime.UtcNow,
            EsAdministrador = false,
            TokenConfirmacion = confirmationToken,
            SesionActiva = true 
        };

        // 4. Persistencia de datos
        await _unitOfWork.Usuarios.AddAsync(newUser);
        await _unitOfWork.CompleteAsync(); 

        // 5. Mapeo Entidad -> Response DTO
        var userResponse = new UsuarioResponseDto
        {
            IdUsuario = newUser.IdUsuario,
            Nombres = newUser.Nombres,
            Apellidos = newUser.Apellidos,
            Email = newUser.Email,
            Edad = newUser.Edad,
            EstadoCivil = newUser.EstadoCivil,
            FechaRegistro = newUser.FechaRegistro,
            EsAdministrador = newUser.EsAdministrador ?? false
        };

        // 6. Generar Token Real
        var token = _jwtProvider.Generate(userResponse);
        
        return new AuthResponseDto
        {
            Usuario = userResponse,
            Token = token,
            ExpiracionToken = DateTime.UtcNow.AddHours(2),
            EsRegistroExitoso = true
        };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginUsuarioDto dto)
    {
        // 1. Validar credenciales
        var user = await _unitOfWork.Usuarios.GetByEmailAsync(dto.Email);
        
        if (user == null)
        {
            throw new UnauthorizedAccessException("Credenciales inválidas.");
        }

        // 2. Verificar Contraseña
        if (!_passwordHasher.Verify(dto.Password, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Credenciales inválidas.");
        }
        
        // 3. Mapeo y Generación de Token
        var userResponse = new UsuarioResponseDto
        {
            IdUsuario = user.IdUsuario,
            Nombres = user.Nombres,
            Apellidos = user.Apellidos,
            Email = user.Email,
            Edad = user.Edad,
            EstadoCivil = user.EstadoCivil,
            FechaRegistro = user.FechaRegistro,
            EsAdministrador = user.EsAdministrador ?? false
        };

        var token = _jwtProvider.Generate(userResponse);

        return new AuthResponseDto
        {
            Usuario = userResponse,
            Token = token,
            ExpiracionToken = DateTime.UtcNow.AddHours(2),
            EsRegistroExitoso = true
        };
    }
    
    public Task<UsuarioResponseDto> UpdateProfileAsync(int userId, ActualizarPerfilDto dto)
    {
        throw new NotImplementedException();
    }
    
    public Task<bool> RequestPasswordResetAsync(string email)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ResetPasswordAsync(string token, string newPassword)
    {
        throw new NotImplementedException();
    }
}