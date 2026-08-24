using Core.constants;
using Core.Entities;
using Core.Exceptions;
using Core.Infraestructure;
using Core.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace Core.Services.Imp
{
    /// <summary>
    /// =========================================================================================
    /// CAPA: CORE -> Services -> Imp
    /// =========================================================================================
    /// PROPÓSITO:
    /// Implementación del servicio de verificación y gestión de roles.
    /// =========================================================================================
    /// </summary>
    public class RoleService(
        IGenericRepository<Role> roleRepository,
        IGenericRepository<UserProfile> userRepository) : IRoleService
    {
        private readonly IGenericRepository<Role> _roleRepository = roleRepository;
        private readonly IGenericRepository<UserProfile> _userRepository = userRepository;

        public async Task<bool> IsAdmin(Guid userId)
        {
            var user = await _userRepository.GetQueryable()
                .Include(u => u.Role)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null || user.Role == null)
            {
                return false;
            }

            return string.Equals(user.Role.Name, Roles.Admin, StringComparison.OrdinalIgnoreCase);
        }

        public async Task<IEnumerable<Role>> GetRolesAsync()
        {
            return await _roleRepository.GetQueryable().AsNoTracking().ToListAsync();
        }

        public async Task<Role> GetRoleByIdAsync(long roleId)
        {
            var role = await _roleRepository.FirstOrDefaultAsyncWithIncludes(r => r.Id == roleId);
            if (role == null)
            {
                throw new BusinessException(HttpStatusCode.NotFound, "Rol no encontrado", $"El rol con Id {roleId} no existe.");
            }
            return role;
        }

        public async Task<Role?> GetRoleByNameAsync(string roleName)
        {
            return await _roleRepository.GetQueryable().AsNoTracking()
                .FirstOrDefaultAsync(r => r.Name.ToLower() == roleName.ToLower());
        }
    }
}

