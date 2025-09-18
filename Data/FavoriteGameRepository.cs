using Microsoft.EntityFrameworkCore;
using SuppGamesBack.Models;

namespace SuppGamesBack.Data
{
    public class FavoriteGameRepository : IFavoriteGameRepository
    {
        private readonly AppDbContext _appDbContext;

        public FavoriteGameRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<FavoriteGame> GetByIdAsync(int id)
        {
            return await _appDbContext.FavoriteGames.FindAsync(id);
        }

        public async Task<List<FavoriteGame>> GetAllAsync()
        {
            return await _appDbContext.FavoriteGames.ToListAsync();
        }

        public async Task<FavoriteGame> AddAsync(FavoriteGame favoriteGame)
        {
            _appDbContext.FavoriteGames.Add(favoriteGame);
            await _appDbContext.SaveChangesAsync();
            return favoriteGame;
        }

        public async Task UpdateAsync(FavoriteGame favoriteGame)
        {
            _appDbContext.FavoriteGames.Update(favoriteGame);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var gameToDelete = await _appDbContext.FavoriteGames.FindAsync(id);

            if(gameToDelete == null)
            {
                return false;
            }

            _appDbContext.FavoriteGames.Remove(gameToDelete);
            await _appDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<FavoriteGame>> GetByUserIdAsync(int userId)
        {
            return await _appDbContext.FavoriteGames.Where(fg => fg.UserId == userId && !fg.Excluded).ToListAsync();
        }

        public async Task<bool> ExistsAsync(int userId, string slug)
        {
            return await _appDbContext.FavoriteGames.AnyAsync(fg => fg.UserId == userId && fg.Slug == slug && !fg.Excluded);
        }
    }
}
