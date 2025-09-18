using SuppGamesBack.Models;

namespace SuppGamesBack.Data
{
    public interface IFavoriteGameRepository
    {
        Task<FavoriteGame> GetByIdAsync(int id);
        Task<List<FavoriteGame>> GetAllAsync();
        Task<List<FavoriteGame>> GetByUserIdAsync(int userId);
        Task<FavoriteGame> AddAsync(FavoriteGame game);
        Task UpdateAsync(FavoriteGame game);
        Task<bool> ExistsAsync(int userId, string slug);
        Task<bool> DeleteAsync(int id);
    }
}
