using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ServerSide.Database
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<UserModel> UserTable { get; set; }
        public DbSet<RefreshToken> RefreshTokensTable { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {

        }
    }
}
