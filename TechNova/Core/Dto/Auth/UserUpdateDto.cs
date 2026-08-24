namespace Core.Dto.Auth
{
    /// <summary>
    /// =========================================================================================
    /// CAPA: CORE -> Dto -> Auth
    /// =========================================================================================
    /// PROPÓSITO:
    /// DTO de entrada para actualizar los datos personales y de contacto del perfil de usuario.
    /// =========================================================================================
    /// </summary>
    public class UserUpdateDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ShippingAddress { get; set; }
        public string? Avatar { get; set; }
    }
}

