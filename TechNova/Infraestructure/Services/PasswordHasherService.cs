using Core.Services.Interfaces;

namespace Infraestructure.Services
{
    /// <summary>
    /// =========================================================================================
    /// CAPA: INFRAESTRUCTURE -> Services
    /// =========================================================================================
    /// PROPÓSITO:
    /// Implementación de hashing de contraseñas utilizando el algoritmo seguro BCrypt.
    /// 
    /// FLUJO ARQUITECTÓNICO:
    /// 1. Durante el registro, 'HashPassword' genera un hash con sal ("salt") aleatoria automática.
    /// 2. Durante el inicio de sesión, 'VerifyPassword' compara la contraseña ingresada contra el hash almacenado.
    /// =========================================================================================
    /// </summary>
    public class PasswordHasherService : IPasswordHasherService
    {
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, workFactor: 11);
        }

        public bool VerifyPassword(string password, string passwordHash)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(password, passwordHash);
            }
            catch
            {
                return false;
            }
        }
    }
}

