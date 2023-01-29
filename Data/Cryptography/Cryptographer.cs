using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Unicode;
using FunBooksAndVideos.Users.Common;
using FunBooksAndVideos.Users.Models;
using Microsoft.IdentityModel.Tokens;

namespace FunBooksAndVideos.Users.Data.Cryptography
{
    public class Cryptographer : ICryptographer
    {
        private readonly IConfiguration _configuration;
        public Cryptographer(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public PasswordStore  CreatePasswordHash(string password)
        {
            var passwordStore = new PasswordStore();

            using(var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordStore.PasswordSalt = hmac.Key;
                passwordStore.PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }

            return passwordStore;
        }

        public string CreateToken(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var appSettingsToken = _configuration.GetSection("AppSettings:Token").Value;
            var appSettingsHours = int.Parse(_configuration.GetSection("AppSettings:AddExpirationHours").Value);

            if(appSettingsToken == null)
                throw new Exception(Messages.AppSettingsToken);

            SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(appSettingsToken));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddHours(appSettingsHours),
                SigningCredentials = creds
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}