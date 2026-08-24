using Core.CustomEntities;
using Core.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace Infraestructure.Filters
{
    /// <summary>
    /// =========================================================================================
    /// CAPA: INFRAESTRUCTURE -> Filters
    /// =========================================================================================
    /// PROPÓSITO:
    /// Filtro global de excepciones de ASP.NET Core para capturar y estandarizar todos los errores no controlados.
    /// 
    /// FLUJO ARQUITECTÓNICO:
    /// 1. Intercepta cualquier excepción ocurrida en el pipeline de la aplicación (Controladores, Servicios, Repositorios).
    /// 2. Si la excepción es de tipo 'BusinessException' (regla de negocio lanzada intencionalmente):
    ///    - Genera un 'Response' con el código HTTP correspondiente (ej. 404, 400, 409).
    ///    - Registra un log informativo sin romper el servidor.
    /// 3. Si la excepción es inesperada (ej. NullReferenceException, fallo de base de datos):
    ///    - Devuelve un error 500 genérico seguro hacia el cliente ("Ha ocurrido un error...").
    ///    - Registra el error técnico y su traza en los logs para auditoría y diagnóstico de los desarrolladores.
    /// =========================================================================================
    /// </summary>
    public class GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger) : IExceptionFilter
    {
        private const string DEFAULT_ERROR_MESSAGE = "Ha ocurrido un error en el servidor, por favor inténtelo de nuevo o contacte con el administrador.";
        private readonly ILogger<GlobalExceptionFilter> _logger = logger;

        public void OnException(ExceptionContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            if (context.Exception is BusinessException exception)
            {
                HandleBusinessException(context, exception);
            }
            else
            {
                HandleGenericException(context, context.Exception);
            }
        }

        private void HandleBusinessException(ExceptionContext context, BusinessException exception)
        {
            var response = new Response
            {
                Status = (int)exception.Status,
                Message = exception.DescriptionStatus,
                Description = exception.Message
            };

            context.Result = new ObjectResult(response)
            {
                StatusCode = (int)exception.Status
            };
            context.HttpContext.Response.StatusCode = (int)exception.Status;
            context.ExceptionHandled = true;

            _logger.LogInformation("Excepción de negocio controlada: {message}", exception.Message);
        }

        private void HandleGenericException(ExceptionContext context, Exception exception)
        {
            var response = new Response
            {
                Status = StatusCodes.Status500InternalServerError,
                Message = DEFAULT_ERROR_MESSAGE,
                Description = exception.Message
            };

            context.Result = new ObjectResult(response)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
            context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.ExceptionHandled = true;

            _logger.LogError(
                "Excepción no controlada: {message} | Detalle: {detail} | Hora: {time}",
                exception.Message,
                exception.InnerException?.Message ?? string.Empty,
                DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)
            );
        }
    }
}

