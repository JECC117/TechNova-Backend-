using Core.Entities;
using Core.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infraestructure.Services
{
    /// <summary>
    /// =========================================================================================
    /// CAPA: INFRAESTRUCTURE -> Services
    /// =========================================================================================
    /// PROPÓSITO:
    /// Implementación de generación y validación de tokens JWT nativos mediante HMAC-SHA256.
    /// 
    /// FLUJO ARQUITECTÓNICO:
    /// 1. Lee la configuración de 'JwtSettings' (SecretKey, Issuer, Audience, ExpiryMinutes) desde 'appsettings.json'.
    /// 2. 'GenerateToken' firma los claims del usuario y retorna el JWT codificado en Base64.
    /// 3. 'ValidateToken' valida la firma y vigencia del token entrante en los filtros o middlewares.
    /// =========================================================================================
    /// </summary>
    public class JwtService(IConfiguration configuration) : IJwtService
    {
        private readonly IConfiguration _configuration = configuration;

        public string GenerateToken(UserProfile user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"] ?? "TechNova_Super_Secure_Secret_Key_For_Jwt_Tokens_2026_Unisangil!";
            var issuer = jwtSettings["Issuer"] ?? "TechNovaApi";
            var audience = jwtSettings["Audience"] ?? "TechNovaClients";
            var expiryMinutes = int.TryParse(jwtSettings["ExpiryMinutes"], out var mins) ? mins : 1440; // 24 horas por defecto

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email),
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Name, $"{user.FirstName} {user.LastName}".Trim()),
                new(ClaimTypes.Role, user.Role?.Name ?? "customer"),
                new("role_id", user.RoleId.ToString()),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(expiryMinutes),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public ClaimsPrincipal? ValidateToken(string token)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"] ?? "TechNova_Super_Secure_Secret_Key_For_Jwt_Tokens_2026_Unisangil!";
            var issuer = jwtSettings["Issuer"] ?? "TechNovaApi";
            var audience = jwtSettings["Audience"] ?? "TechNovaClients";

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(secretKey);

            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return principal;
            }
            catch
            {
                return null;
            }
        }

        public long GetTokenExpirationSeconds()
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var expiryMinutes = int.TryParse(jwtSettings["ExpiryMinutes"], out var mins) ? mins : 1440;
            return expiryMinutes * 60;
        }
    }
}

