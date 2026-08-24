using Core.CustomEntities;
using Core.Dto.Auth;
using Core.Services.Interfaces;
using Infraestructure.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace Api.Controllers
{
    /// <summary>
    /// =========================================================================================
    /// CAPA: API -> Controllers
    /// =========================================================================================
    /// PROPÓSITO:
    /// Controlador REST para operaciones de autenticación, registro, login e información de usuario.
    /// =========================================================================================
    /// </summary>
    [Route("api/auth")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {
        private readonly IAuthService _authService = authService;

        /// <summary>
        /// Registra un nuevo usuario en la plataforma y retorna sus credenciales con token JWT.
        /// </summary>
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto registerDto)
        {
            var result = await _authService.RegisterAsync(registerDto);
            var response = new Response
            {
                Status = (int)HttpStatusCode.Created,
                Message = "Usuario registrado exitosamente.",
                Description = result
            };
            return StatusCode((int)HttpStatusCode.Created, response);
        }

        /// <summary>
        /// Inicia sesión con credenciales (Email y Contraseña) y genera el token de acceso JWT.
        /// </summary>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UserLoginDto loginDto)
        {
            var result = await _authService.LoginAsync(loginDto);
            var response = new Response
            {
                Status = (int)HttpStatusCode.OK,
                Message = "Inicio de sesión exitoso.",
                Description = result
            };
            return Ok(response);
        }

        /// <summary>
        /// Obtiene los datos del perfil del usuario actualmente autenticado a través del token JWT.
        /// </summary>
        [HttpGet("me")]
        [ValidateToken]
        public async Task<IActionResult> GetProfile()
        {
            var userId = GetCurrentUserId();
            var profile = await _authService.GetProfileAsync(userId);

            var response = new Response
            {
                Status = (int)HttpStatusCode.OK,
                Message = "Perfil de usuario obtenido exitosamente.",
                Description = profile
            };
            return Ok(response);
        }

        /// <summary>
        /// Actualiza la información personal del usuario autenticado.
        /// </summary>
        [HttpPut("update-profile")]
        [ValidateToken]
        public async Task<IActionResult> UpdateProfile([FromBody] UserUpdateDto updateDto)
        {
            var userId = GetCurrentUserId();
            var updatedProfile = await _authService.UpdateProfileAsync(userId, updateDto);

            var response = new Response
            {
                Status = (int)HttpStatusCode.OK,
                Message = "Perfil actualizado exitosamente.",
                Description = updatedProfile
            };
            return Ok(response);
        }

        private Guid GetCurrentUserId()
        {
            var userIdString = HttpContext.Items["UserId"]?.ToString()
                               ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                               ?? User.FindFirst("sub")?.Value;

            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
            {
                throw new UnauthorizedAccessException("No fue posible identificar al usuario autenticado.");
            }

            return userId;
        }
    }
}

