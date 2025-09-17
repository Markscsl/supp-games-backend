using Microsoft.EntityFrameworkCore.Infrastructure;

namespace SuppGamesBack.Data
{
    public interface IAnnotationRepository
    {
        Task<List<Annotation>> GetAllAsync();
        Task<Annotation> GetByIdAsync(int id);
        Task<Annotation> AddAsync(Annotation annotation);
        Task UpdateAsync(Annotation annotation);
        Task DeleteAsync(int id);
    }
}
