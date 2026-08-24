using System.Net;

namespace Core.Exceptions
{
    /// <summary>
    /// =========================================================================================
    /// CAPA: CORE -> Exceptions
    /// =========================================================================================
    /// PROPÓSITO:
    /// Representa una excepción controlada de lógica de negocio (Domain / Business Rule Exception).
    /// 
    /// FLUJO ARQUITECTÓNICO:
    /// 1. Cuando se incumple una regla de negocio en un Servicio (ej. saldo insuficiente, recurso no encontrado),
    ///    el servicio lanza: throw new BusinessException(HttpStatusCode.NotFound, "No encontrado", "El producto no existe");
    /// 2. La excepción es capturada automáticamente por 'GlobalExceptionFilter' en la capa de Infraestructura.
    /// 3. El filtro transforma la excepción en un 'Response' estructurado con el código HTTP correspondiente,
    ///    evitando que la aplicación falle con un error 500 no controlado y evitando try/catch repetitivos en controladores.
    /// 
    /// GUÍA PARA DESARROLLADORES:
    /// - Usar 'Status' para definir el código HTTP adecuado (ej. HttpStatusCode.BadRequest, HttpStatusCode.NotFound).
    /// - Usar 'DescriptionStatus' como título o categoría del error.
    /// - Usar 'Message' como detalle específico del problema.
    /// =========================================================================================
    /// </summary>
    public class BusinessException : Exception
    {
        public HttpStatusCode Status { get; private set; }
        public string DescriptionStatus { get; private set; }

        public BusinessException(HttpStatusCode status, string descriptionStatus, string message) : base(message)
        {
            Status = status;
            DescriptionStatus = descriptionStatus;
        }
    }
}

