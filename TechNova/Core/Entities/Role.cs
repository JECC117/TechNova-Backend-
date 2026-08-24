namespace Core.Entities
{
    /// <summary>
    /// =========================================================================================
    /// CAPA: CORE -> Entities
    /// =========================================================================================
    /// PROPÓSITO:
    /// Representa los roles de usuario en el sistema (ej. admin, customer, analyst).
    /// 
    /// FLUJO ARQUITECTÓNICO:
    /// 1. Se vincula con 'UserProfile' mediante una relación uno a muchos (1:N).
    /// 2. Es utilizado por 'RoleService' y los filtros de autorización ('AdminOnlyFilter')
    ///    para verificar los privilegios de acceso del usuario autenticado.
    /// =========================================================================================
    /// </summary>
    public class Role
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        public virtual ICollection<UserProfile> UserProfiles { get; set; } = [];
    }
}

