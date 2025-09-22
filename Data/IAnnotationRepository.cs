namespace SuppGamesBack.Data
{
    public interface IAnnotationRepository
    {
        Task<List<Anot>> GetAllAsync();
        Task<List<Anot>> GetAllByUserId(int userId);
        Task<Anot> GetByIdAsync(int id);
        Task<Anot> AddAsync(Anot anot);
        Task UpdateAsync(Anot anot);
        Task<bool> DeleteAsync(int id);
    }
}