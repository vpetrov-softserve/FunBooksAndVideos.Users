
using Microsoft.EntityFrameworkCore;

namespace FunBooksAndVideos.Users.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<Models.User> Users {get; set;}
    }
}