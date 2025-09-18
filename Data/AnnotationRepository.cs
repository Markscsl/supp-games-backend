using Microsoft.EntityFrameworkCore;

namespace SuppGamesBack.Data
{
    public class AnnotationRepository : IAnnotationRepository
    {
        private readonly AppDbContext _appDbContext;

        public AnnotationRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<Anot>> GetAllAsync()
        {
            return await _appDbContext.Annotations.ToListAsync();
        }

        public async Task<Anot> GetByIdAsync(int id)
        {
            return await _appDbContext.Annotations.FindAsync(id);
        }

        public async Task<Anot> AddAsync(Anot anot)
        {
            _appDbContext.Annotations.Add(anot);
            await _appDbContext.SaveChangesAsync();
            return anot;
        }

        public async Task UpdateAsync(Anot anot)
        {
            _appDbContext.Annotations.Update(anot);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var anotToDelete = await _appDbContext.Annotations.FindAsync(id);
            if (anotToDelete == null)
            {
                return false;
            }

            _appDbContext.Annotations.Remove(anotToDelete);
            await _appDbContext.SaveChangesAsync();
            return true;
        }
    }
}