using Microsoft.EntityFrameworkCore;
using SuppGamesBack.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuppGamesBack.Data
{
    public class FavoriteGameRepository : IFavoriteGameRepository
    {
        private readonly AppDbContext _context;
        public FavoriteGameRepository(AppDbContext context) { _context = context; }

        public async Task<FavoriteGame?> GetByIdAsync(int id)
        {
            return await _context.FavoriteGames.FindAsync(id);
        }

        public async Task<List<FavoriteGame>> GetByUserIdAsync(int userId)
        {
            return await _context.FavoriteGames
                .Include(fg => fg.Game)
                .Where(fg => fg.UserId == userId && !fg.Excluded)
                .ToListAsync();
        }

        public async Task<FavoriteGame?> FindAsync(int userId, int gameId)
        {
            return await _context.FavoriteGames
                .FirstOrDefaultAsync(fg => fg.UserId == userId && fg.GameId == gameId);
        }

        public async Task<bool> ExistsAsync(int userId, int gameId)
        {
            return await _context.FavoriteGames
                .AnyAsync(fg => fg.UserId == userId && fg.GameId == gameId);
        }

        public async Task<FavoriteGame> AddAsync(FavoriteGame favoriteGame)
        {
            _context.FavoriteGames.Add(favoriteGame);
            await _context.SaveChangesAsync();
            return favoriteGame;
        }

        public async Task<bool> DeleteAsync(int favoriteGameId)
        {
            var favorite = await _context.FavoriteGames.FindAsync(favoriteGameId);
            if (favorite == null) return false;

            _context.FavoriteGames.Remove(favorite);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}