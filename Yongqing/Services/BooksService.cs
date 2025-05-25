using WebApplication1.Interfaces;
using Yongqing.Database;

namespace WebApplication1.Services;
public class BooksService : IBooksService
{
    private readonly IBooksRepository _repo;

    public BooksService(IBooksRepository repo)
    {
        _repo = repo;
    }

    public Task<IReadOnlyList<Books>> GetAllAsync()
        => _repo.GetAllAsync();
    public Task<Books?> GetByIdAsync(int id) 
        => _repo.GetByIdAsync(id);
    public Task<bool> CreateAsync(Books book) 
        => _repo.CreateAsync(book);
    public Task<bool> UpdateAsync(Books book) 
        => _repo.UpdateAsync(book);
    public Task<bool> DeleteAsync(int id) 
        => _repo.DeleteAsync(id);
}
