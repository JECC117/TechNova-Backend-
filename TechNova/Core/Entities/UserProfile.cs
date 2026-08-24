namespace Core.Entities
{
    /// <summary>
    /// =========================================================================================
    /// CAPA: CORE -> Entities
    /// =========================================================================================
    /// PROPÓSITO:
    /// Representa el perfil, credenciales y datos de identidad de los usuarios en TechNova.
    /// 
    /// FLUJO ARQUITECTÓNICO:
    /// 1. La contraseña nunca se almacena en texto plano; se guarda su hash criptográfico en 'PasswordHash'.
    /// 2. Contiene la relación con 'Role' a través de 'RoleId'.
    /// 3. Utilizado por 'AuthService' para registrar y autenticar usuarios, y generar tokens JWT.
    /// =========================================================================================
    /// </summary>
    public class UserProfile
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? ShippingAddress { get; set; }
        public string? Avatar { get; set; }
        public long RoleId { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public virtual Role Role { get; set; } = null!;
    }
}

