using Microsoft.EntityFrameworkCore;
using SuppGamesBack.Models;
using System.Threading.Tasks;

namespace SuppGamesBack.Data
{
    public class GameRepository : IGameRepository
    {
        private readonly AppDbContext _context;
        public GameRepository(AppDbContext context) { _context = context; }

        public async Task<Game?> GetBySlugAsync(string slug)
        {
            return await _context.Games.FirstOrDefaultAsync(g => g.Slug == slug);
        }

        public async Task<Game?> GetByIdAsync(int id)
        {
            return await _context.Games.FindAsync(id);
        }

        public async Task<Game> AddAsync(Game game)
        {
            _context.Games.Add(game);
            await _context.SaveChangesAsync();
            return game;
        }
    }
}