using System.Text.Unicode;

namespace FunBooksAndVideos.Users.Data.Cryptography
{
    public class Cryptographer : ICryptographer
    {
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