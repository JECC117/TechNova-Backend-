using Core.Dto.Product;
using Core.QueryFilter;

namespace Core.Services.Interfaces
{
    /// <summary>
    /// =========================================================================================
    /// CAPA: CORE -> Services -> Interfaces
    /// =========================================================================================
    /// PROPÓSITO:
    /// Define el contrato de la capa de servicios / lógica de negocio para la gestión de productos.
    /// 
    /// FLUJO ARQUITECTÓNICO:
    /// 1. El Controlador ('ProductController') inyecta y llama a los métodos de 'IProductService'.
    /// 2. La capa de servicio contiene las reglas del negocio, validaciones de dominio, cálculos y orquestación.
    /// 3. La interfaz trabaja exclusivamente con DTOs o tipos de dominio para mantener el desacoplamiento.
    /// 
    /// GUÍA PARA DESARROLLADORES:
    /// - Cada nuevo módulo funcional debe tener su interfaz de servicio en 'Core/Services/Interfaces/'.
    /// - La implementación concreta se ubica en 'Core/Services/Imp/'.
    /// =========================================================================================
    /// </summary>
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllAsync();
        Task<IEnumerable<ProductDto>> GetFilteredAsync(ProductQueryFilter filter);
        Task<ProductDto> GetByIdAsync(long id);
        Task<ProductDto> CreateAsync(CreateProductDto createDto);
        Task<ProductDto> UpdateAsync(long id, UpdateProductDto updateDto);
        Task<bool> DeleteAsync(long id);
    }
}

