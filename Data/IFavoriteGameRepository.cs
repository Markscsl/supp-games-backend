using SuppGamesBack.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SuppGamesBack.Data
{
    public interface IFavoriteGameRepository
    {
        Task<FavoriteGame?> GetByIdAsync(int id); 
        Task<List<FavoriteGame>> GetByUserIdAsync(int userId);
        Task<FavoriteGame?> FindAsync(int userId, int gameId);
        Task<bool> ExistsAsync(int userId, int gameId);
        Task<FavoriteGame> AddAsync(FavoriteGame favoriteGame);
        Task<bool> DeleteAsync(int favoriteGameId);
    }
}