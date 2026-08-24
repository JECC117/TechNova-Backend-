using Core.Entities;
using Core.QueryFilter;

namespace Core.Infraestructure
{
    /// <summary>
    /// =========================================================================================
    /// CAPA: CORE -> Infraestructure
    /// =========================================================================================
    /// PROPÓSITO:
    /// Define el contrato de repositorio especializado para la entidad 'Product'.
    /// 
    /// FLUJO ARQUITECTÓNICO:
    /// 1. Cuando las operaciones estándar de 'IGenericRepository<Product>' no son suficientes 
    ///    (ej. consultas SQL directas, procedimientos almacenados, reportes con agregaciones o filtros complejos),
    ///    se definen métodos personalizados en esta interfaz.
    /// 2. Su implementación se encuentra en 'Infraestructure/Repositories/ProductRepository.cs'.
    /// 
    /// GUÍA PARA DESARROLLADORES:
    /// - Usar repositorios especializados para consultas optimizadas, funciones de PostgreSQL o lógica de persistencia específica.
    /// =========================================================================================
    /// </summary>
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<IEnumerable<Product>> GetFilteredProductsAsync(ProductQueryFilter filter);
        Task<bool> ExistsByNameAsync(string name, long? excludeId = null);
    }
}

