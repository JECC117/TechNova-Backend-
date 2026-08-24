namespace Core.Dto.Auth
{
    /// <summary>
    /// =========================================================================================
    /// CAPA: CORE -> Dto -> Auth
    /// =========================================================================================
    /// PROPÓSITO:
    /// DTO de entrada para las credenciales de inicio de sesión.
    /// 
    /// FLUJO ARQUITECTÓNICO:
    /// 1. Recibido por 'AuthController.Login(...)'.
    /// 2. Validado por 'UserLoginValidator'.
    /// 3. Procesado por 'AuthService.LoginAsync(...)', retornando un 'AuthInfoDto' con el token JWT.
    /// =========================================================================================
    /// </summary>
    public class UserLoginDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}

