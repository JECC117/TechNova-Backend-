namespace Core.Services.Interfaces
{
    /// <summary>
    /// =========================================================================================
    /// CAPA: CORE -> Services -> Interfaces
    /// =========================================================================================
    /// PROPÓSITO:
    /// Define el contrato para el hashing seguro y la verificación de contraseñas de usuario.
    /// 
    /// BENEFICIOS:
    /// - Desacopla la lógica de hashing (ej. BCrypt, Argon2, PBKDF2) del dominio.
    /// =========================================================================================
    /// </summary>
    public interface IPasswordHasherService
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string passwordHash);
    }
}

