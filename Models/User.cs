namespace FunBooksAndVideos.Users.Models
{
    public class User
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt {get; set;}

        public string Phone { get; set; }

        public string Email { get; set; }
    }
}