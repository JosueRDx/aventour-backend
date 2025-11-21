using Aventour.Application.DTOs.Auth;
using Aventour.Application.Interfaces.Services;
using Aventour.Application.Interfaces.Utilities;
using Aventour.Domain.Interfaces;
using Aventour.Domain.Models; // Aquí usamos la Entidad (Usuario)

namespace Aventour.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    // private readonly ITokenGenerator _tokenGenerator; // Requerirías un servicio similar para JWTs
    // private readonly IEmailService _emailService; // Requerirías un servicio similar para emails

    public AuthService(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        // _tokenGenerator = tokenGenerator;
        // _emailService = emailService;
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
        var confirmationToken = Guid.NewGuid().ToString(); // Generar un token para confirmación de correo
        
        // 3. Mapeo DTO -> Entidad (Manual, en un proyecto real se usaría AutoMapper)
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
            SesionActiva = true // Para fines de demostración, se activa inmediatamente
        };

        // 4. Persistencia de datos (Llamada al Unit of Work)
        await _unitOfWork.Usuarios.AddAsync(newUser);
        await _unitOfWork.CompleteAsync(); // Guardar cambios

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

        // 6. Generar Token y Response
        // var tokenResult = _tokenGenerator.GenerateToken(userResponse);
        
        return new AuthResponseDto
        {
            Usuario = userResponse,
            Token = "JWT_TOKEN_GENERADO_AQUI", // Placeholder
            ExpiracionToken = DateTime.UtcNow.AddHours(1), // Placeholder
            EsRegistroExitoso = true
        };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginUsuarioDto dto)
    {
        // 1. Validar credenciales: Buscar usuario por email
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
        
        // 3. Mapeo y Generación de Token (similar al registro)
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

        // 4. Actualizar estado de sesión y Guardar (si es necesario)
        // user.SesionActiva = true; 
        // _unitOfWork.Usuarios.Update(user);
        // await _unitOfWork.CompleteAsync();

        return new AuthResponseDto
        {
            Usuario = userResponse,
            Token = "JWT_TOKEN_GENERADO_AQUI", // Placeholder
            ExpiracionToken = DateTime.UtcNow.AddHours(1), // Placeholder
            EsRegistroExitoso = true
        };
    }
    
    // ... Implementación de UpdateProfileAsync, RequestPasswordResetAsync, ResetPasswordAsync ...
    
    public Task<UsuarioResponseDto> UpdateProfileAsync(int userId, ActualizarPerfilDto dto)
    {
        // Lógica: 
        // 1. Obtener usuario por ID.
        // 2. Aplicar cambios del DTO.
        // 3. Llamar a _unitOfWork.Usuarios.Update(user);
        // 4. Llamar a _unitOfWork.CompleteAsync();
        // 5. Mapear y devolver el UsuarioResponseDto actualizado.
        throw new NotImplementedException();
    }
    
    public Task<bool> RequestPasswordResetAsync(string email)
    {
         // Lógica: 
        // 1. Buscar usuario por email.
        // 2. Generar un Token de Recuperación único.
        // 3. Guardar el token en el campo TokenConfirmacion del Usuario.
        // 4. Enviar email al usuario con el link que contiene el token.
        throw new NotImplementedException();
    }

    public Task<bool> ResetPasswordAsync(string token, string newPassword)
    {
        // Lógica: 
        // 1. Buscar usuario por TokenConfirmacion.
        // 2. Verificar la validez del token (caducidad, etc.).
        // 3. Hashear newPassword.
        // 4. Actualizar PasswordHash y limpiar TokenConfirmacion.
        throw new NotImplementedException();
    }
}