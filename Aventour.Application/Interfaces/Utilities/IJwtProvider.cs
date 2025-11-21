using Aventour.Application.DTOs.Auth;

namespace Aventour.Application.Interfaces.Utilities;

public interface IJwtProvider
{
    string Generate(UsuarioResponseDto usuario);
}