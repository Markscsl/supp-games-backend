using SuppGamesBack.Models;

namespace SuppGamesBack.Data
{
    public interface IFavoriteGameRepository
    {
        Task<FavoriteGame> GetByIdAsync(int id);
        Task<List<FavoriteGame>> GetAllAsync();
        Task<FavoriteGame> AddAsync(FavoriteGame game);
        Task UpdateAsync(FavoriteGame game);
        Task DeleteAsync(int id);
    }
}
