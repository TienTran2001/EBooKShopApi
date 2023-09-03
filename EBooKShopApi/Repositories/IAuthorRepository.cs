using EBooKShopApi.Models;

namespace EBooKShopApi.Repositories
{
    public interface IAuthorRepository
    {
        Task<List<Author>> GetAllAsync();

        Task<Author?> GetByIdAsync(int id);

        Task<Author> CreateAsync(Author author);

        Task<Author?> UpdateAsync(Author author, int id);

        Task<Author?> DeleteAsync(int id);
    }
}
