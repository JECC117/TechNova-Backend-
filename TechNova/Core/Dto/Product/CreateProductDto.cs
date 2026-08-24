namespace Core.Dto.Product
{
    /// <summary>
    /// =========================================================================================
    /// CAPA: CORE -> Dto
    /// =========================================================================================
    /// PROPÓSITO:
    /// DTO de entrada utilizado para la creación de un nuevo producto.
    /// 
    /// FLUJO ARQUITECTÓNICO:
    /// 1. El cliente envía un JSON en el cuerpo de una petición POST.
    /// 2. ASP.NET deserializa el JSON a 'CreateProductDto'.
    /// 3. 'ValidationFilter' ejecuta automáticamente 'CreateProductValidator' (FluentValidation).
    /// 4. Si es válido, llega al Controlador y pasa al 'ProductService.CreateProductAsync(...)'.
    /// =========================================================================================
    /// </summary>
    public class CreateProductDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }
}

