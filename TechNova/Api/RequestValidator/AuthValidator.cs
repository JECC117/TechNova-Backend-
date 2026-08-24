using Core.Dto.Auth;
using FluentValidation;

namespace Api.RequestValidator
{
    /// <summary>
    /// =========================================================================================
    /// CAPA: API -> RequestValidator
    /// =========================================================================================
    /// PROPÓSITO:
    /// Validaciones semánticas y de formato con FluentValidation para las operaciones de autenticación.
    /// =========================================================================================
    /// </summary>
    public class AuthValidator
    {
    }

    public class UserRegisterValidator : AbstractValidator<UserRegisterDto>
    {
        public UserRegisterValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre no puede exceder 100 caracteres.");

            RuleFor(x => x.LastName)
                .MaximumLength(100).WithMessage("El apellido no puede exceder 100 caracteres.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El correo electrónico es obligatorio.")
                .EmailAddress().WithMessage("El formato del correo electrónico no es válido.")
                .MaximumLength(150).WithMessage("El correo no puede exceder 150 caracteres.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("La contraseña es obligatoria.")
                .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres.");

            RuleFor(x => x.PhoneNumber)
                .MaximumLength(20).WithMessage("El número de teléfono no puede exceder 20 caracteres.");
        }
    }

    public class UserLoginValidator : AbstractValidator<UserLoginDto>
    {
        public UserLoginValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("El correo electrónico es obligatorio.")
                .EmailAddress().WithMessage("El formato del correo electrónico no es válido.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("La contraseña es obligatoria.");
        }
    }

    public class UserUpdateValidator : AbstractValidator<UserUpdateDto>
    {
        public UserUpdateValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("El nombre es obligatorio.")
                .MaximumLength(100).WithMessage("El nombre no puede exceder 100 caracteres.");

            RuleFor(x => x.LastName)
                .MaximumLength(100).WithMessage("El apellido no puede exceder 100 caracteres.");

            RuleFor(x => x.PhoneNumber)
                .MaximumLength(20).WithMessage("El número de teléfono no puede exceder 20 caracteres.");

            RuleFor(x => x.ShippingAddress)
                .MaximumLength(300).WithMessage("La dirección no puede exceder 300 caracteres.");
        }
    }
}

