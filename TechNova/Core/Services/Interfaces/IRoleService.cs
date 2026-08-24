using Core.Entities;

namespace Core.Services.Interfaces
{
    /// <summary>
    /// =========================================================================================
    /// CAPA: CORE -> Services -> Interfaces
    /// =========================================================================================
    /// PROPÓSITO:
    /// Define el contrato para el servicio de administración y verificación de roles.
    /// =========================================================================================
    /// </summary>
    public interface IRoleService
    {
        Task<bool> IsAdmin(Guid userId);
        Task<IEnumerable<Role>> GetRolesAsync();
        Task<Role> GetRoleByIdAsync(long roleId);
        Task<Role?> GetRoleByNameAsync(string roleName);
    }
}

