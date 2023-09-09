using EBooKShopApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EBooKShopApi.Repositories
{
    public class CategoriesRepository : ICategoriesRepository
    {
        private readonly EBookShopContext _dbContext;

        public CategoriesRepository(EBookShopContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<List<Category>> GetAllAsync()
        {
            return await _dbContext.Categories.ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _dbContext.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);
        }

        public async Task<Category> CreateAsync(Category category)
        {
            await _dbContext.Categories.AddAsync(category);
            await _dbContext.SaveChangesAsync();
            return category;
        }

        public async Task<Category?> UpdateAsync(Category category, int id)
        {
            var categoryExist = await _dbContext.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);
            if (categoryExist == null) { return null; }
            categoryExist.Name = category.Name;
            await _dbContext.SaveChangesAsync();
            return categoryExist;
        }

        public async Task<Category?> DeleteAsync(int id)
        {
            var categoryExist = await _dbContext.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);
            if (categoryExist == null) { return null; }
            _dbContext.Categories.Remove(categoryExist);
            await _dbContext.SaveChangesAsync();
            return categoryExist;
        }
    }
}
