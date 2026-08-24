using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Infraestructure.Filters
{
    /// <summary>
    /// =========================================================================================
    /// CAPA: INFRAESTRUCTURE -> Filters
    /// =========================================================================================
    /// PROPÓSITO:
    /// Filtro de acción asíncrono para ejecutar y evaluar automáticamente los validadores de FluentValidation.
    /// 
    /// FLUJO ARQUITECTÓNICO:
    /// 1. Antes de que se ejecute la acción en el Controlador, este filtro inspecciona los argumentos recibidos.
    /// 2. Busca dinámicamente si existe un 'IValidator<TDTO>' registrado en el contenedor de dependencias
    ///    (por ejemplo, 'CreateProductValidator' para 'CreateProductDto').
    /// 3. Si la validación falla:
    ///    - Corta la ejecución de la petición inmediatamente.
    ///    - Retorna una respuesta 400 Bad Request con el diccionario detallado de campos y mensajes de error.
    /// 4. Si la validación es exitosa:
    ///    - Permite que el flujo continúe hacia el método de la acción en el Controlador.
    /// 
    /// GUÍA PARA DESARROLLADORES:
    /// - Al crear un nuevo DTO en Core, solo se debe definir su 'AbstractValidator<TDTO>' en 'Api/RequestValidator/'.
    ///   El filtro lo ejecutará automáticamente sin necesidad de código adicional en los controladores.
    /// =========================================================================================
    /// </summary>
    public class ValidationFilter(IServiceProvider serviceProvider) : IAsyncActionFilter
    {
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            foreach (var argument in context.ActionArguments.Values)
            {
                if (argument == null) continue;

                var validatorType = typeof(IValidator<>).MakeGenericType(argument.GetType());
                var validator = _serviceProvider.GetService(validatorType) as IValidator;

                if (validator != null)
                {
                    var validationContext = new ValidationContext<object>(argument);
                    var validationResult = await validator.ValidateAsync(validationContext);

                    if (!validationResult.IsValid)
                    {
                        context.Result = new BadRequestObjectResult(validationResult.ToDictionary());
                        return;
                    }
                }
            }

            await next();
        }
    }
}

