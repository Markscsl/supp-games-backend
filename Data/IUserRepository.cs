using SuppGamesBack.Models;

namespace SuppGamesBack.Data
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync();
        Task<User> GetByIdAsync(int id);
        Task<User> AddAsync(User user);
        Task<User> UpdateAsync(User user);
        Task<bool> DeleteAsync(int id);
    }
}
