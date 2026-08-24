using Core.CustomEntities;
using Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Reflection;
using System.Security.Claims;

namespace Infraestructure.Filters
{
    /// <summary>
    /// =========================================================================================
    /// CAPA: INFRAESTRUCTURE -> Filters
    /// =========================================================================================
    /// PROPÓSITO:
    /// Atributo y filtro de autorización para validar tokens JWT nativos en solicitudes entrantes.
    /// 
    /// FLUJO ARQUITECTÓNICO:
    /// 1. Extrae el Bearer Token del encabezado 'Authorization: Bearer <token>' o cookie 'auth_token'.
    /// 2. Omite la validación si el método o controlador tiene la anotación '[AllowAnonymous]'.
    /// 3. Utiliza 'IJwtService' para validar la firma HMAC-SHA256 y la vigencia del token.
    /// 4. Si el token es válido, extrae los Claims y almacena 'UserId', 'Email' y 'Role' en 'HttpContext.Items'
    ///    y en 'HttpContext.User' para que estén disponibles en toda la petición.
    /// 5. Si es inválido o no existe, responde con 401 Unauthorized y un 'Response' estructurado.
    /// =========================================================================================
    /// </summary>
    public class ValidateTokenAttribute : TypeFilterAttribute
    {
        public ValidateTokenAttribute() : base(typeof(ValidateTokenFilter)) { }
    }

    public class ValidateTokenFilter(IJwtService jwtService, ILogger<ValidateTokenFilter> logger) : IAsyncAuthorizationFilter
    {
        private readonly IJwtService _jwtService = jwtService;
        private readonly ILogger<ValidateTokenFilter> _logger = logger;

        public Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            // 1. Comprobar si la acción o el controlador permiten acceso anónimo
            if (context.ActionDescriptor is ControllerActionDescriptor descriptor)
            {
                var allowAnonymous = descriptor.MethodInfo.GetCustomAttribute<AllowAnonymousAttribute>() != null ||
                                     descriptor.ControllerTypeInfo.GetCustomAttribute<AllowAnonymousAttribute>() != null;

                if (allowAnonymous)
                {
                    return Task.CompletedTask;
                }
            }

            // 2. Extraer el token
            var token = ExtractToken(context.HttpContext.Request);
            if (string.IsNullOrEmpty(token))
            {
                context.Result = new ObjectResult(new Response
                {
                    Status = StatusCodes.Status401Unauthorized,
                    Message = "Acceso no autorizado",
                    Description = "No se proporcionó un token de autenticación válido."
                })
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };
                return Task.CompletedTask;
            }

            // 3. Validar token JWT
            try
            {
                var principal = _jwtService.ValidateToken(token);
                if (principal == null)
                {
                    context.Result = new ObjectResult(new Response
                    {
                        Status = StatusCodes.Status401Unauthorized,
                        Message = "Token inválido",
                        Description = "El token de autenticación ha expirado o es inválido."
                    })
                    {
                        StatusCode = StatusCodes.Status401Unauthorized
                    };
                    return Task.CompletedTask;
                }

                // 4. Asignar Claims al HttpContext
                context.HttpContext.User = principal;
                var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value
                                  ?? principal.FindFirst("sub")?.Value;

                if (!string.IsNullOrEmpty(userIdClaim))
                {
                    context.HttpContext.Items["UserId"] = userIdClaim;
                }

                var roleClaim = principal.FindFirst(ClaimTypes.Role)?.Value;
                if (!string.IsNullOrEmpty(roleClaim))
                {
                    context.HttpContext.Items["Role"] = roleClaim;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error al procesar el token JWT: {message}", ex.Message);
                context.Result = new ObjectResult(new Response
                {
                    Status = StatusCodes.Status401Unauthorized,
                    Message = "Error de autenticación",
                    Description = "Ocurrió un error al validar su identidad."
                })
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };
            }

            return Task.CompletedTask;
        }

        private static string? ExtractToken(HttpRequest request)
        {
            if (request.Cookies.TryGetValue("auth_token", out var cookieToken) && !string.IsNullOrEmpty(cookieToken))
            {
                return cookieToken;
            }

            var authHeader = request.Headers["Authorization"].ToString();
            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                return authHeader["Bearer ".Length..].Trim();
            }

            return null;
        }
    }
}

