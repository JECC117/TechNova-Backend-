namespace Core.Dto.Auth
{
    /// <summary>
    /// =========================================================================================
    /// CAPA: CORE -> Dto -> Auth
    /// =========================================================================================
    /// PROPÓSITO:
    /// DTO de salida con los datos públicos y de perfil del usuario autenticado.
    /// 
    /// FLUJO ARQUITECTÓNICO:
    /// 1. Devuelto en los endpoints de consulta de perfil ('/api/auth/me') o tras registro/login.
    /// 2. Nunca incluye información sensible como 'PasswordHash'.
    /// =========================================================================================
    /// </summary>
    public class UserInfoDto
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; }
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? ShippingAddress { get; set; }
        public string? Avatar { get; set; }
        public string Role { get; set; } = string.Empty;
        public long RoleId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

