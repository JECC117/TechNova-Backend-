using Core.constants;
using Core.Dto.Auth;
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
    /// Implementación de la lógica de negocio para registro, autenticación JWT, consulta y actualización de perfiles.
    /// 
    /// FLUJO ARQUITECTÓNICO:
    /// 1. 'RegisterAsync':
    ///    - Verifica que el email no esté en uso.
    ///    - Asigna el rol 'customer' por defecto si no se especifica otro.
    ///    - Genera el hash de la contraseña mediante 'IPasswordHasherService'.
    ///    - Persiste el usuario con 'IUnitOfWork.SaveChangesAsync()'.
    ///    - Emite el token JWT a través de 'IJwtService' y retorna 'AuthInfoDto'.
    /// 2. 'LoginAsync':
    ///    - Busca el usuario por email incluyendo su rol ('Include(u => u.Role)').
    ///    - Valida la contraseña comparando el hash con 'VerifyPassword'.
    ///    - Si las credenciales son incorrectas, lanza 'BusinessException' con 'HttpStatusCode.Unauthorized'.
    ///    - Genera y retorna el token JWT.
    /// =========================================================================================
    /// </summary>
    public class AuthService(
        IGenericRepository<UserProfile> userRepository,
        IGenericRepository<Role> roleRepository,
        IUnitOfWork unitOfWork,
        IPasswordHasherService passwordHasher,
        IJwtService jwtService) : IAuthService
    {
        private readonly IGenericRepository<UserProfile> _userRepository = userRepository;
        private readonly IGenericRepository<Role> _roleRepository = roleRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IPasswordHasherService _passwordHasher = passwordHasher;
        private readonly IJwtService _jwtService = jwtService;

        public async Task<AuthInfoDto> RegisterAsync(UserRegisterDto registerDto)
        {
            var emailNormalized = registerDto.Email.Trim().ToLower();

            // 1. Verificar si el correo ya está registrado
            var existingUser = await _userRepository.GetQueryable()
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email.ToLower() == emailNormalized);

            if (existingUser != null)
            {
                throw new BusinessException(HttpStatusCode.Conflict, "Email registrado", "El correo electrónico ingresado ya se encuentra en uso.");
            }

            // 2. Obtener rol por defecto 'customer'
            var defaultRole = await _roleRepository.GetQueryable()
                .FirstOrDefaultAsync(r => r.Name.ToLower() == Roles.Customer.ToLower());

            if (defaultRole == null)
            {
                // Si el rol aún no existe en base de datos, lo inicializamos automáticamente
                defaultRole = new Role
                {
                    Name = Roles.Customer,
                    Description = "Rol de cliente estándar"
                };
                await _roleRepository.AddAsync(defaultRole);
                await _unitOfWork.SaveChangesAsync();
            }

            // 3. Crear entidad de usuario con contraseña cifrada
            var user = new UserProfile
            {
                Id = Guid.NewGuid(),
                FirstName = registerDto.FirstName.Trim(),
                LastName = registerDto.LastName?.Trim(),
                Email = emailNormalized,
                PasswordHash = _passwordHasher.HashPassword(registerDto.Password),
                PhoneNumber = registerDto.PhoneNumber,
                ShippingAddress = registerDto.ShippingAddress,
                Avatar = registerDto.Avatar,
                RoleId = defaultRole.Id,
                Role = defaultRole,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            // 4. Generar token JWT nativo
            var token = _jwtService.GenerateToken(user);
            var expiresIn = _jwtService.GetTokenExpirationSeconds();

            return new AuthInfoDto
            {
                AccessToken = token,
                TokenType = "Bearer",
                ExpiresIn = expiresIn,
                IsAdmin = string.Equals(defaultRole.Name, Roles.Admin, StringComparison.OrdinalIgnoreCase),
                User = new UserInfoDto
                {
                    UserId = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    ShippingAddress = user.ShippingAddress,
                    Avatar = user.Avatar,
                    Role = defaultRole.Name,
                    RoleId = defaultRole.Id,
                    CreatedAt = user.CreatedAt
                }
            };
        }

        public async Task<AuthInfoDto> LoginAsync(UserLoginDto loginDto)
        {
            var emailNormalized = loginDto.Email.Trim().ToLower();

            // 1. Buscar usuario por email con su rol asociado
            var user = await _userRepository.GetQueryable()
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email.ToLower() == emailNormalized);

            if (user == null || !user.IsActive)
            {
                throw new BusinessException(HttpStatusCode.Unauthorized, "Credenciales inválidas", "El correo o la contraseña ingresados son incorrectos.");
            }

            // 2. Verificar la contraseña con BCrypt
            var isPasswordValid = _passwordHasher.VerifyPassword(loginDto.Password, user.PasswordHash);
            if (!isPasswordValid)
            {
                throw new BusinessException(HttpStatusCode.Unauthorized, "Credenciales inválidas", "El correo o la contraseña ingresados son incorrectos.");
            }

            // 3. Generar token JWT
            var token = _jwtService.GenerateToken(user);
            var expiresIn = _jwtService.GetTokenExpirationSeconds();
            var isAdmin = string.Equals(user.Role?.Name, Roles.Admin, StringComparison.OrdinalIgnoreCase);

            return new AuthInfoDto
            {
                AccessToken = token,
                TokenType = "Bearer",
                ExpiresIn = expiresIn,
                IsAdmin = isAdmin,
                User = new UserInfoDto
                {
                    UserId = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    ShippingAddress = user.ShippingAddress,
                    Avatar = user.Avatar,
                    Role = user.Role?.Name ?? Roles.Customer,
                    RoleId = user.RoleId,
                    CreatedAt = user.CreatedAt
                }
            };
        }

        public async Task<UserInfoDto> GetProfileAsync(Guid userId)
        {
            var user = await _userRepository.GetQueryable()
                .Include(u => u.Role)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                throw new BusinessException(HttpStatusCode.NotFound, "Usuario no encontrado", "El perfil de usuario solicitado no existe.");
            }

            return new UserInfoDto
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                ShippingAddress = user.ShippingAddress,
                Avatar = user.Avatar,
                Role = user.Role?.Name ?? Roles.Customer,
                RoleId = user.RoleId,
                CreatedAt = user.CreatedAt
            };
        }

        public async Task<UserInfoDto> UpdateProfileAsync(Guid userId, UserUpdateDto updateDto)
        {
            var user = await _userRepository.GetQueryable()
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                throw new BusinessException(HttpStatusCode.NotFound, "Usuario no encontrado", "El perfil de usuario no existe.");
            }

            user.FirstName = updateDto.FirstName.Trim();
            user.LastName = updateDto.LastName?.Trim();
            user.PhoneNumber = updateDto.PhoneNumber;
            user.ShippingAddress = updateDto.ShippingAddress;
            user.Avatar = updateDto.Avatar;
            user.UpdatedAt = DateTime.UtcNow;

            _userRepository.Update(user);
            await _unitOfWork.SaveChangesAsync();

            return new UserInfoDto
            {
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                ShippingAddress = user.ShippingAddress,
                Avatar = user.Avatar,
                Role = user.Role?.Name ?? Roles.Customer,
                RoleId = user.RoleId,
                CreatedAt = user.CreatedAt
            };
        }
    }
}

