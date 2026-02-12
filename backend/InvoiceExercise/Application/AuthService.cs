using Application.Interfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _users;

        public AuthService(IUserRepository users)
        {
            _users = users;
        }

        public async Task RegisterAsync(string username, string password)
        {
            using var sha = SHA256.Create();
            var hash = Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(password)));

            var user = new User
            {
                Username = username,
                PasswordHash = hash,
                Role = "User"
            };

            await _users.CreateAsync(user);
        }

        public async Task<string?> LoginAsync(string username, string password)
        {
            var user = await _users.GetByUsernameAsync(username);
            if (user == null) return null;

            using var sha = SHA256.Create();
            var hash = Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(password)));

            if (hash != user.PasswordHash) return null;

            // JWT no se genera aquí, sino en el controlador (API)
            return user.Id.ToString();
        }
    }
}
