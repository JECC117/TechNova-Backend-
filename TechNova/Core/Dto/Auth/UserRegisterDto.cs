namespace Core.Dto.Auth
{
    /// <summary>
    /// =========================================================================================
    /// CAPA: CORE -> Dto -> Auth
    /// =========================================================================================
    /// PROPÓSITO:
    /// DTO de entrada para el registro de nuevos usuarios en el sistema.
    /// 
    /// FLUJO ARQUITECTÓNICO:
    /// 1. Recibido por 'AuthController.Register(...)'.
    /// 2. Validado por 'UserRegisterValidator' (formato de email, longitud y complejidad de contraseña).
    /// 3. Procesado por 'AuthService.RegisterAsync(...)'.
    /// =========================================================================================
    /// </summary>
    public class UserRegisterDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? ShippingAddress { get; set; }
        public string? Avatar { get; set; }
    }
}

