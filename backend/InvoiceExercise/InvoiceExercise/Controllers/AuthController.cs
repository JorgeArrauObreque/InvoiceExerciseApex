using Application.Dtos;
using Application.Interfaces;
using Domain.Models;
using InvoiceExercise.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace InvoiceExercise.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _auth; // <-- FALTABA ESTO

        public AuthController(IUserRepository userRepository, IJwtService auth)
        {
            _userRepository = userRepository;
            _auth = auth; // <-- FALTABA ESTO
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]LoginDto dto)
        {
            // Revisar si ya existe el usuario
            var existingUser = await _userRepository.GetByUsernameAsync(dto.Email);
            if (existingUser != null)
                return BadRequest("El usuario ya existe");

            // Crear hash de la contraseña
            using var sha = SHA256.Create();
            var hash = Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(dto.Password)));

            // Crear usuario
            var user = new User
            {
                Username = dto.Email,
                PasswordHash = hash,
                Role = "User" // rol por defecto
            };

            await _userRepository.CreateAsync(user);

            // Generar token JWT
            var token = _auth.GenerateToken(user.Id.ToString(), user.Role, user.Username);

            return Ok(new { token });
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginDto dto)
        {
            var user = await _userRepository.GetByUsernameAsync(dto.Email);
            if (user == null)
                return Unauthorized("Usuario no encontrado");

            using var sha = SHA256.Create();
            var hash = Convert.ToBase64String(
                sha.ComputeHash(Encoding.UTF8.GetBytes(dto.Password))
            );

            if (hash != user.PasswordHash)
                return Unauthorized("Contraseña incorrecta");

            var token = _auth.GenerateToken(
                user.Id.ToString(),
                user.Role,
                user.Username
            );

            return Ok(new { token });
        }
        [HttpGet("test-token")]
        public IActionResult TestToken()
        {
            var token = _auth.GenerateToken("1", "Admin", "jorge");
            return Ok(token);
        }
    }
}