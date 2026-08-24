namespace Core.constants
{
    /// <summary>
    /// =========================================================================================
    /// CAPA: CORE -> constants
    /// =========================================================================================
    /// PROPÓSITO:
    /// Centraliza las constantes, valores fijos y cadenas de configuración compartidas en la solución.
    /// 
    /// FLUJO ARQUITECTÓNICO:
    /// 1. Define nombres de roles para autorización con políticas y atributos ([Authorize(Roles = Roles.Admin)]).
    /// 2. Evita el uso de cadenas "mágicas" ("magic strings") dispersas en el código, reduciendo errores tipográficos.
    /// 
    /// GUÍA PARA DESARROLLADORES:
    /// - Crear clases estáticas adicionales en esta carpeta para constantes de mensajes, reclamos JWT, políticas, etc.
    /// =========================================================================================
    /// </summary>
    public static class Roles
    {
        public const string Admin = "admin";
        public const string Customer = "customer";
        public const string Manager = "manager";
    }
}

