using Core.Entities;
using Core.Infraestructure;
using Core.QueryFilter;
using Infraestructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infraestructure.Repositories
{
    /// <summary>
    /// =========================================================================================
    /// CAPA: INFRAESTRUCTURE -> Repositories
    /// =========================================================================================
    /// PROPÓSITO:
    /// Implementación especializada de repositorio para la entidad 'Product'.
    /// 
    /// FLUJO ARQUITECTÓNICO:
    /// 1. Hereda de 'GenericRepository<Product>' obteniendo todos los métodos CRUD base.
    /// 2. Implementa 'IProductRepository' agregando lógica especializada de persistencia,
    ///    consultas con filtros avanzados ('ProductQueryFilter') y comprobaciones de existencia.
    /// 
    /// GUÍA PARA DESARROLLADORES:
    /// - Al requerir consultas optimizadas con índices, SQL nativo ('SqlQueryRaw', 'ExecuteSqlRawAsync')
    ///   o procedimientos de PostgreSQL, implementarlos en repositorios especializados como este.
    /// =========================================================================================
    /// </summary>
    public class ProductRepository(AppDbContext context) : GenericRepository<Product>(context), IProductRepository
    {
        public async Task<IEnumerable<Product>> GetFilteredProductsAsync(ProductQueryFilter filter)
        {
            var query = _dbSet.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Name))
            {
                query = query.Where(p => EF.Functions.ILike(p.Name, $"%{filter.Name}%"));
            }

            if (filter.MinPrice.HasValue)
            {
                query = query.Where(p => p.Price >= filter.MinPrice.Value);
            }

            if (filter.MaxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= filter.MaxPrice.Value);
            }

            if (filter.IsActive.HasValue)
            {
                query = query.Where(p => p.IsActive == filter.IsActive.Value);
            }

            // Paginación
            var skip = (filter.PageNumber - 1) * filter.PageSize;
            return await query.OrderByDescending(p => p.CreatedAt)
                              .Skip(skip)
                              .Take(filter.PageSize)
                              .ToListAsync();
        }

        public async Task<bool> ExistsByNameAsync(string name, long? excludeId = null)
        {
            var query = _dbSet.AsNoTracking().Where(p => p.Name.ToLower() == name.ToLower());

            if (excludeId.HasValue)
            {
                query = query.Where(p => p.Id != excludeId.Value);
            }

            return await query.AnyAsync();
        }
    }
}

