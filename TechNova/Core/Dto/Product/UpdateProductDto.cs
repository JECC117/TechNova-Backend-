namespace Core.Dto.Product
{
    /// <summary>
    /// =========================================================================================
    /// CAPA: CORE -> Dto
    /// =========================================================================================
    /// PROPÓSITO:
    /// DTO de entrada utilizado para la actualización de un producto existente.
    /// 
    /// FLUJO ARQUITECTÓNICO:
    /// 1. El cliente envía un JSON en el cuerpo de una petición PUT.
    /// 2. 'ValidationFilter' valida los datos de 'UpdateProductDto' usando 'UpdateProductValidator'.
    /// 3. El 'ProductService' recupera la entidad existente, actualiza sus campos y persiste los cambios.
    /// =========================================================================================
    /// </summary>
    public class UpdateProductDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public bool IsActive { get; set; }
    }
}

