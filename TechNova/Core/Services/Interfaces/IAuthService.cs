using Core.Dto.Auth;

namespace Core.Services.Interfaces
{
    /// <summary>
    /// =========================================================================================
    /// CAPA: CORE -> Services -> Interfaces
    /// =========================================================================================
    /// PROPÓSITO:
    /// Define el contrato para el servicio de autenticación y gestión de cuentas de usuario.
    /// =========================================================================================
    /// </summary>
    public interface IAuthService
    {
        Task<AuthInfoDto> RegisterAsync(UserRegisterDto registerDto);
        Task<AuthInfoDto> LoginAsync(UserLoginDto loginDto);
        Task<UserInfoDto> GetProfileAsync(Guid userId);
        Task<UserInfoDto> UpdateProfileAsync(Guid userId, UserUpdateDto updateDto);
    }
}

