namespace FunBooksAndVideos.Users.Data.Cryptography
{
    public interface ICryptographer
    {
        public PasswordStore  CreatePasswordHash(string password);

        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt);

        public string CreateToken(Models.User user);
    }
}