namespace Core.QueryFilter
{
    /// <summary>
    /// =========================================================================================
    /// CAPA: CORE -> QueryFilter
    /// =========================================================================================
    /// PROPÓSITO:
    /// Encapsula los parámetros de filtrado, búsqueda y paginación para consultas sobre productos.
    /// 
    /// FLUJO ARQUITECTÓNICO:
    /// 1. El cliente envía parámetros en la QueryString: /api/products?name=laptop&minPrice=100&pageNumber=1&pageSize=10
    /// 2. ASP.NET vincula estos parámetros a una instancia de 'ProductQueryFilter' en el método GET del controlador.
    /// 3. El servicio o repositorio utiliza estos filtros dinámicamente sobre 'IQueryable<Product>'
    ///    para ejecutar una consulta SQL optimizada con WHERE, LIMIT y OFFSET en PostgreSQL.
    /// 
    /// GUÍA PARA DESARROLLADORES:
    /// - Crear una clase QueryFilter para cada entidad o módulo que requiera filtros avanzados o paginación.
    /// =========================================================================================
    /// </summary>
    public class ProductQueryFilter
    {
        public string? Name { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public bool? IsActive { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}

