using Microsoft.EntityFrameworkCore;
using SuppGamesBack.Models;

namespace SuppGamesBack.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base (options)
        {
        }

        public DbSet<User> Users { get; set; }


    }
}
