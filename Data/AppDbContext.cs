using Microsoft.EntityFrameworkCore;
using SuppGamesBack.Models;

namespace SuppGamesBack.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Anot> Annotations { get; set; }

        public DbSet<FavoriteGame> FavoriteGames { get; set; }
    }
}
