namespace Core.CustomEntities
{
    /// <summary>
    /// =========================================================================================
    /// CAPA: CORE -> CustomEntities
    /// =========================================================================================
    /// PROPÓSITO:
    /// Representa la estructura estándar y unificada de respuesta para todas las peticiones de la API.
    /// 
    /// FLUJO ARQUITECTÓNICO:
    /// 1. El Controlador (API) o el Filtro de Excepciones empaqueta el resultado en esta clase.
    /// 2. El cliente (Frontend / Mobile) siempre recibe un JSON consistente con la misma estructura,
    ///    independientemente de si la operación fue exitosa o fallida.
    /// 
    /// GUÍA PARA DESARROLLADORES:
    /// - Usar 'Status' para el código HTTP (ej. 200, 201, 400, 404, 500).
    /// - Usar 'Message' para un mensaje descriptivo de alto nivel amigable para el usuario.
    /// - Usar 'Description' para la carga útil de datos (Payload / DTO) o el detalle del error.
    /// =========================================================================================
    /// </summary>
    public class Response
    {
        /// <summary>
        /// Código de estado HTTP de la respuesta (ej. 200, 400, 500).
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Mensaje general o estado de la operación (ej. "Operación exitosa", "Error de validación").
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Cuerpo del resultado (datos devueltos, objeto DTO, lista o descripción del error).
        /// </summary>
        public object Description { get; set; } = string.Empty;
    }
}

