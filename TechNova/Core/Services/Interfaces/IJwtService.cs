using Core.Entities;
using System.Security.Claims;

namespace Core.Services.Interfaces
{
    /// <summary>
    /// =========================================================================================
    /// CAPA: CORE -> Services -> Interfaces
    /// =========================================================================================
    /// PROPÓSITO:
    /// Define el contrato para la generación, firma y lectura de tokens JWT nativos.
    /// 
    /// FLUJO ARQUITECTÓNICO:
    /// 1. 'AuthService' solicita la generación de un token tras autenticar con éxito a un usuario.
    /// 2. 'JwtService' emite un token firmado con HMAC-SHA256 incluyendo Claims estándar:
    ///    - 'NameIdentifier' (UserId / Guid)
    ///    - 'Email'
    ///    - 'Role' (Nombre del rol)
    ///    - 'GivenName' (Nombre del usuario)
    /// =========================================================================================
    /// </summary>
    public interface IJwtService
    {
        string GenerateToken(UserProfile user);
        ClaimsPrincipal? ValidateToken(string token);
        long GetTokenExpirationSeconds();
    }
}

