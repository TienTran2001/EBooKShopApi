using EBooKShopApi.Models;
using EBooKShopApi.ViewModels;

namespace EBooKShopApi.Repositories
{
    public interface ICategoriesRepository
    {
        Task<List<Category>> GetAllAsync();

        Task<Category?> GetByIdAsync(int id);

        Task<Category> CreateAsync(Category category);

        Task<Category?> UpdateAsync(Category category, int id);

        Task<Category?> DeleteAsync(int id);
    }
}
