using FoodOrderSystemAPI.DTOs.Auth;
using FoodOrderSystemAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace FoodOrderSystemAPI.Services
{
    public class AuthService
    {
        private readonly FoodOrderingSystemContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(FoodOrderingSystemContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<string> Register(UserRegisterDTO user)
        {

            var isDuplicate = await _context.Users
                .AsNoTracking()
                .Where(Q => Q.Username == user.Username)
                .FirstOrDefaultAsync();

            if (isDuplicate != null)
            {
                return $"username {user.Username} is already registered";
            }
            
            var hmac = new HMACSHA512();
            var passwordSalt = hmac.Key;
            var passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(user.Password));

            var newUser = new User
            {
                Name = user.Name,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                RoleId = user.RoleId,
                Username = user.Username
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return $"New user ({user.Username}) is successfully created";
        }

        public async Task<string> Login(UserLoginDTO user)
        {

            var userExists = await _context.Users
                .AsNoTracking()
                .Where(Q => Q.Username == user.Username)
                .FirstOrDefaultAsync();

            if (userExists == null)
            {
                return "Wrong credentials";
            }

            var passwordSalt = userExists.PasswordSalt;
            var passwordHash = userExists.PasswordHash;
            var hmac = new HMACSHA512(passwordSalt);

            var validationHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(user.Password));
            if (validationHash.SequenceEqual(passwordHash) == false)
            {
                return "Wrong credentials";
            }

            var roleName = await _context.Roles
                .AsNoTracking()
                .Where(Q => Q.RoleId == userExists.RoleId)
                .Select(Q => Q.Name)
                .FirstAsync();


            // Generate JWT
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userExists.Username),
                new Claim(ClaimTypes.Role, roleName)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims, 
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;

        }

    }
}
