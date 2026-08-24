namespace Core.Dto.Auth
{
    /// <summary>
    /// =========================================================================================
    /// CAPA: CORE -> Dto -> Auth
    /// =========================================================================================
    /// PROPÓSITO:
    /// DTO de respuesta con la información de sesión, token de acceso JWT y vigencia.
    /// 
    /// FLUJO ARQUITECTÓNICO:
    /// 1. Generado tras un inicio de sesión o registro exitoso.
    /// 2. El cliente almacena el 'AccessToken' (Bearer token) y lo adjunta en los encabezados
    ///    'Authorization: Bearer <token>' de las peticiones protegidas.
    /// =========================================================================================
    /// </summary>
    public class AuthInfoDto
    {
        public string AccessToken { get; set; } = string.Empty;
        public string TokenType { get; set; } = "Bearer";
        public long ExpiresIn { get; set; }
        public bool IsAdmin { get; set; }
        public UserInfoDto User { get; set; } = null!;
    }
}

