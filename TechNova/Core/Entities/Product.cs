namespace Core.Entities
{
    /// <summary>
    /// =========================================================================================
    /// CAPA: CORE -> Entities
    /// =========================================================================================
    /// PROPÓSITO:
    /// Representa los modelos de dominio (Domain Entities / POCOs) que modelan las tablas y reglas centrales del negocio.
    /// 
    /// FLUJO ARQUITECTÓNICO:
    /// 1. Las entidades son usadas por el 'AppDbContext' y 'GenericRepository' para persistir y consultar datos de la BD.
    /// 2. Los Servicios de negocio ('ProductService') operan sobre estas entidades y las convierten hacia/desde DTOs.
    /// 3. Las entidades NUNCA deben exponerse directamente en los Controladores hacia el cliente web; 
    ///    para ello se deben utilizar siempre los DTOs ('Core/Dto/').
    /// 
    /// GUÍA PARA DESARROLLADORES:
    /// - Cada entidad debe ser una clase POCO simple sin dependencias externas complejas.
    /// - Las configuraciones de columnas, claves primarias/foráneas y relaciones de base de datos
    ///   se realizan en 'Infraestructure/Data/Configurations/{Entity}Configuration.cs' mediante Fluent API.
    /// =========================================================================================
    /// </summary>
    public class Product
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}

