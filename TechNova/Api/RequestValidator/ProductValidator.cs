using Core.Dto.Product;
using FluentValidation;

namespace Api.RequestValidator
{
    /// <summary>
    /// =========================================================================================
    /// CAPA: API -> RequestValidator
    /// =========================================================================================
    /// PROPÓSITO:
    /// Define las reglas de validación semántica y de formato para las solicitudes HTTP entrantes (DTOs).
    /// 
    /// FLUJO ARQUITECTÓNICO:
    /// 1. Se registran en el contenedor de dependencias mediante 'AddValidatorsFromAssemblies(...)'.
    /// 2. 'ValidationFilter' las ejecuta antes de entrar a la lógica de los Controladores.
    /// 3. Si algún campo no cumple las reglas (ej. nombre vacío, precio menor o igual a cero),
    ///    se retorna un error 400 Bad Request estructurado con los mensajes definidos aquí.
    /// 
    /// GUÍA PARA DESARROLLADORES:
    /// - Crear un 'AbstractValidator<TDTO>' para cada DTO que reciba información en POST/PUT.
    /// - Usar validadores fluidos como 'NotEmpty()', 'GreaterThan()', 'MaximumLength()', 'EmailAddress()', etc.
    /// =========================================================================================
    /// </summary>
    public class ProductValidator
    {
    }

    public class CreateProductValidator : AbstractValidator<CreateProductDto>
    {
        public CreateProductValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre del producto es obligatorio.")
                .MaximumLength(150).WithMessage("El nombre no puede exceder los 150 caracteres.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("El precio del producto debe ser mayor a cero.");

            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0).WithMessage("El stock no puede ser un valor negativo.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("La descripción no puede exceder los 500 caracteres.");
        }
    }

    public class UpdateProductValidator : AbstractValidator<UpdateProductDto>
    {
        public UpdateProductValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("El nombre del producto es obligatorio.")
                .MaximumLength(150).WithMessage("El nombre no puede exceder los 150 caracteres.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("El precio del producto debe ser mayor a cero.");

            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0).WithMessage("El stock no puede ser un valor negativo.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("La descripción no puede exceder los 500 caracteres.");
        }
    }
}

