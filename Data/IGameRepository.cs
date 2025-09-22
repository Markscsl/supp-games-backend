using SuppGamesBack.Models;
using System.Threading.Tasks;

namespace SuppGamesBack.Data
{
    public interface IGameRepository
    {
        Task<Game?> GetBySlugAsync(string slug);
        Task<Game?> GetByIdAsync(int id);
        Task<Game> AddAsync(Game game);
    }
}