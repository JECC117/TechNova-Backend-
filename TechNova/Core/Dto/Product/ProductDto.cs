namespace Core.Dto.Product
{
    /// <summary>
    /// =========================================================================================
    /// CAPA: CORE -> Dto (Data Transfer Objects)
    /// =========================================================================================
    /// PROPÓSITO:
    /// DTO de lectura y presentación utilizado para transferir información del producto hacia el cliente.
    /// 
    /// FLUJO ARQUITECTÓNICO:
    /// 1. La Base de Datos retorna la entidad 'Product'.
    /// 2. El Servicio ('ProductService') proyecta o mapea la entidad 'Product' a 'ProductDto'.
    /// 3. El Controlador envía 'ProductDto' al cliente envuelto en 'Response.Description'.
    /// 
    /// BENEFICIOS:
    /// - Evita exponer columnas internas o sensibles del modelo de datos.
    /// - Optimiza el payload de red transfiriendo únicamente la información requerida por la vista/cliente.
    /// =========================================================================================
    /// </summary>
    public class ProductDto
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

