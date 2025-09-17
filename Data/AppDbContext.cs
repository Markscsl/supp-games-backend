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

        public DbSet<Annotation> Annotations { get; set; }

        public DbSet<FavoriteGame> FavoriteGames { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<FavoriteGame>()
                .HasOne(fg => fg.User)
                .WithMany()
                .HasForeignKey(fg => fg.UserId)
                .OnDelete(DeleteBehavior.NoAction);
         
            modelBuilder.Entity<Annotation>()
                .HasOne(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Annotation>()
                .HasOne(a => a.FavoriteGame)
                .WithMany()
                .HasForeignKey(a => a.FavoriteGameId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
