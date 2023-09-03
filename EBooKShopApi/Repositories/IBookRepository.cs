using EBooKShopApi.Models;

namespace EBooKShopApi.Repositories
{
    public interface IBookRepository
    {       
        Task<List<Book>> GetBooksAsync();
    }
}
