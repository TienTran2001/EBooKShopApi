using EBooKShopApi.Models;

namespace EBooKShopApi.Repositories
{
    public interface IBookRepository
    {
        Task<List<Book>> GetBooksAsync();
        Task<Book?> GetBookByIdAsync(int id);
        Task<List<Book>> GetBooksByCategoryAsync(int categoryId);
        Task<List<Book>> GetBooksByAuthorAsync(int authorId);

    }
}