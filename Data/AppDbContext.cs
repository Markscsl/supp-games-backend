using Microsoft.EntityFrameworkCore;
using SuppGamesBack.Models;

namespace SuppGamesBack.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Anot> Annotations { get; set; }
        public DbSet<FavoriteGame> FavoriteGames { get; set; }
        public DbSet<Game> Games { get; set; }
    }
}