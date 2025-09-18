using Microsoft.EntityFrameworkCore;
using SuppGamesBack.Models;

namespace SuppGamesBack.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _appDbContext;

        public UserRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _appDbContext.Users.ToListAsync();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _appDbContext.Users.FindAsync(id);
        }

        public async Task<User> AddAsync(User user)
        {
            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            _appDbContext.Users.Add(user);
            await _appDbContext.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateAsync(User user)
        {
            _appDbContext.Users.Update(user);
            await _appDbContext.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var userToDelete = await _appDbContext.Users.FindAsync(id);

            if (userToDelete == null)
            {
                return false;
            }

            userToDelete.IsAtivo = false;
            await _appDbContext.SaveChangesAsync();
            return true;
        }
    }
}
