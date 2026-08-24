using Core.CustomEntities;
using Core.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Infraestructure.Filters
{
    /// <summary>
    /// =========================================================================================
    /// CAPA: INFRAESTRUCTURE -> Filters
    /// =========================================================================================
    /// PROPÓSITO:
    /// Atributo y filtro de autorización para restringir el acceso exclusivamente a usuarios con rol 'admin'.
    /// 
    /// FLUJO ARQUITECTÓNICO:
    /// 1. Se coloca en controladores o acciones que requieren privilegios de administración: '[AdminOnly]'.
    /// 2. Recupera el 'UserId' desde 'HttpContext.Items' (previamente validado por 'ValidateTokenFilter').
    /// 3. Utiliza 'IRoleService.IsAdmin(userId)' para verificar si el usuario tiene el rol 'admin'.
    /// 4. Si no es administrador, bloquea la ejecución retornando 403 Forbidden estructurado.
    /// =========================================================================================
    /// </summary>
    public class AdminOnlyAttribute : TypeFilterAttribute
    {
        public AdminOnlyAttribute() : base(typeof(AdminOnlyFilter)) { }
    }

    public class AdminOnlyFilter(IRoleService roleService) : IAsyncAuthorizationFilter
    {
        private readonly IRoleService _roleService = roleService;

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var userIdString = context.HttpContext.Items["UserId"]?.ToString();

            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
            {
                context.Result = new ObjectResult(new Response
                {
                    Status = StatusCodes.Status401Unauthorized,
                    Message = "Acceso no autorizado",
                    Description = "Debe iniciar sesión para acceder a este recurso."
                })
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };
                return;
            }

            var isAdmin = await _roleService.IsAdmin(userId);
            if (!isAdmin)
            {
                context.Result = new ObjectResult(new Response
                {
                    Status = StatusCodes.Status403Forbidden,
                    Message = "Acceso denegado",
                    Description = "No posee los permisos de administrador requeridos para realizar esta acción."
                })
                {
                    StatusCode = StatusCodes.Status403Forbidden
                };
            }
        }
    }
}

