using Yongqing.Database;

namespace WebApplication1.Interfaces;
public interface IBooksRepository
{
    Task<IReadOnlyList<Books>> GetAllAsync();
    Task<Books?> GetByIdAsync(int id);
    Task<bool> CreateAsync(Books book);
    Task<bool> UpdateAsync(Books book);
    Task<bool> DeleteAsync(int id);
}
