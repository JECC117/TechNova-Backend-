using Core.CustomEntities;
using Core.Dto.Product;
using Core.QueryFilter;
using Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Api.Controllers
{
    /// <summary>
    /// =========================================================================================
    /// CAPA: API -> Controllers
    /// =========================================================================================
    /// PROPÓSITO:
    /// Controlador REST que expone los endpoints HTTP para la gestión de productos.
    /// 
    /// FLUJO ARQUITECTÓNICO:
    /// 1. Recibe la petición HTTP (GET, POST, PUT, DELETE) con su ruta base '/api/products'.
    /// 2. 'ValidationFilter' valida previamente los DTOs entrantes ('CreateProductDto', 'UpdateProductDto').
    /// 3. El controlador delega la lógica de negocio a 'IProductService'.
    /// 4. Empaqueta el resultado en el objeto estandarizado 'Response' con el código de estado correspondiente:
    ///    - 200 OK: Consultas exitosas y actualizaciones.
    ///    - 201 Created: Creación exitosa de recursos.
    /// 5. Cualquier excepción no controlada o 'BusinessException' es interceptada por 'GlobalExceptionFilter'.
    /// 
    /// GUÍA PARA DESARROLLADORES:
    /// - Mantener los controladores limpios y delgados (Thin Controllers).
    /// - No colocar lógica de negocio ni consultas a base de datos dentro del controlador.
    /// - Utilizar siempre el modelo 'Response' para el retorno de datos.
    /// =========================================================================================
    /// </summary>
    [Route("api/products")]
    [ApiController]
    public class ProductController(IProductService productService) : ControllerBase
    {
        private readonly IProductService _productService = productService;

        /// <summary>
        /// Obtiene todos los productos registrados.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllAsync();
            var response = new Response
            {
                Status = (int)HttpStatusCode.OK,
                Message = "Lista de productos obtenida exitosamente.",
                Description = products
            };
            return Ok(response);
        }

        /// <summary>
        /// Obtiene productos filtrados por criterios y paginación.
        /// </summary>
        [HttpGet("filter")]
        public async Task<IActionResult> GetFiltered([FromQuery] ProductQueryFilter filter)
        {
            var products = await _productService.GetFilteredAsync(filter);
            var response = new Response
            {
                Status = (int)HttpStatusCode.OK,
                Message = "Productos filtrados obtenidos exitosamente.",
                Description = products
            };
            return Ok(response);
        }

        /// <summary>
        /// Obtiene el detalle de un producto por su Id.
        /// </summary>
        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(long id)
        {
            var product = await _productService.GetByIdAsync(id);
            var response = new Response
            {
                Status = (int)HttpStatusCode.OK,
                Message = "Producto obtenido exitosamente.",
                Description = product
            };
            return Ok(response);
        }

        /// <summary>
        /// Crea un nuevo producto.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductDto createDto)
        {
            var createdProduct = await _productService.CreateAsync(createDto);
            var response = new Response
            {
                Status = (int)HttpStatusCode.Created,
                Message = "Producto creado exitosamente.",
                Description = createdProduct
            };
            return StatusCode((int)HttpStatusCode.Created, response);
        }

        /// <summary>
        /// Actualiza un producto existente.
        /// </summary>
        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, [FromBody] UpdateProductDto updateDto)
        {
            var updatedProduct = await _productService.UpdateAsync(id, updateDto);
            var response = new Response
            {
                Status = (int)HttpStatusCode.OK,
                Message = "Producto actualizado exitosamente.",
                Description = updatedProduct
            };
            return Ok(response);
        }

        /// <summary>
        /// Elimina un producto por su Id.
        /// </summary>
        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
        {
            await _productService.DeleteAsync(id);
            var response = new Response
            {
                Status = (int)HttpStatusCode.OK,
                Message = $"Producto con Id {id} eliminado exitosamente.",
                Description = true
            };
            return Ok(response);
        }
    }
}

