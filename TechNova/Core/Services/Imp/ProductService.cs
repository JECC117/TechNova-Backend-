using Core.Dto.Product;
using Core.Entities;
using Core.Exceptions;
using Core.Infraestructure;
using Core.QueryFilter;
using Core.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Core.Services.Imp
{
    /// <summary>
    /// =========================================================================================
    /// CAPA: CORE -> Services -> Imp
    /// =========================================================================================
    /// PROPÓSITO:
    /// Implementación concreta de la lógica de negocio para 'Product'.
    /// 
    /// FLUJO ARQUITECTÓNICO:
    /// 1. Inyecta 'IProductRepository' (o 'IGenericRepository<Product>') y 'IUnitOfWork'.
    /// 2. Valida reglas de negocio (ej. verificar nombres duplicados, existencia previa del recurso).
    /// 3. Lanza 'BusinessException' cuando una regla de negocio se viola.
    /// 4. Mapea entidades hacia DTOs antes de devolverlas al controlador.
    /// 5. Utiliza 'IUnitOfWork.SaveChangesAsync()' para asegurar la persistencia transaccional.
    /// 
    /// GUÍA PARA DESARROLLADORES:
    /// - Mantener los métodos asíncronos ('async/await').
    /// - Usar 'AsNoTracking()' en consultas de solo lectura para máxima eficiencia.
    /// - No mezclar lógica de infraestructura directa (como llamadas HTTP directas o SQL embebido);
    ///   delegar siempre en repositorios o servicios correspondientes.
    /// =========================================================================================
    /// </summary>
    public class ProductService(IProductRepository productRepository, IUnitOfWork unitOfWork) : IProductService
    {
        private readonly IProductRepository _productRepository = productRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            var query = _productRepository.GetQueryable().AsNoTracking();

            return await query.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Stock = p.Stock,
                IsActive = p.IsActive,
                CreatedAt = p.CreatedAt
            }).ToListAsync();
        }

        public async Task<IEnumerable<ProductDto>> GetFilteredAsync(ProductQueryFilter filter)
        {
            var products = await _productRepository.GetFilteredProductsAsync(filter);

            return products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Stock = p.Stock,
                IsActive = p.IsActive,
                CreatedAt = p.CreatedAt
            });
        }

        public async Task<ProductDto> GetByIdAsync(long id)
        {
            var product = await _productRepository.FirstOrDefaultAsyncWithIncludes(p => p.Id == id);
            if (product == null)
            {
                throw new BusinessException(HttpStatusCode.NotFound, "Producto no encontrado", $"El producto con Id {id} no existe en el sistema.");
            }

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                IsActive = product.IsActive,
                CreatedAt = product.CreatedAt
            };
        }

        public async Task<ProductDto> CreateAsync(CreateProductDto createDto)
        {
            var exists = await _productRepository.ExistsByNameAsync(createDto.Name);
            if (exists)
            {
                throw new BusinessException(HttpStatusCode.Conflict, "Producto duplicado", $"Ya existe un producto registrado con el nombre '{createDto.Name}'.");
            }

            var product = new Product
            {
                Name = createDto.Name,
                Description = createDto.Description,
                Price = createDto.Price,
                Stock = createDto.Stock,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _productRepository.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                IsActive = product.IsActive,
                CreatedAt = product.CreatedAt
            };
        }

        public async Task<ProductDto> UpdateAsync(long id, UpdateProductDto updateDto)
        {
            var product = await _productRepository.FirstOrDefaultAsyncWithIncludes(p => p.Id == id);
            if (product == null)
            {
                throw new BusinessException(HttpStatusCode.NotFound, "Producto no encontrado", $"El producto con Id {id} no existe.");
            }

            var existsName = await _productRepository.ExistsByNameAsync(updateDto.Name, id);
            if (existsName)
            {
                throw new BusinessException(HttpStatusCode.Conflict, "Nombre en uso", $"Ya existe otro producto con el nombre '{updateDto.Name}'.");
            }

            product.Name = updateDto.Name;
            product.Description = updateDto.Description;
            product.Price = updateDto.Price;
            product.Stock = updateDto.Stock;
            product.IsActive = updateDto.IsActive;
            product.UpdatedAt = DateTime.UtcNow;

            _productRepository.Update(product);
            await _unitOfWork.SaveChangesAsync();

            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Stock = product.Stock,
                IsActive = product.IsActive,
                CreatedAt = product.CreatedAt
            };
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var product = await _productRepository.FirstOrDefaultAsyncWithIncludes(p => p.Id == id);
            if (product == null)
            {
                throw new BusinessException(HttpStatusCode.NotFound, "Producto no encontrado", $"El producto con Id {id} no existe.");
            }

            _productRepository.Delete(product);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}

