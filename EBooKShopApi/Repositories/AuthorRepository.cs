using EBooKShopApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EBooKShopApi.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {

        private readonly EBookShopContext _dbContext;

        public AuthorRepository(EBookShopContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<List<Author>> GetAllAsync()
        {
            return await _dbContext.Authors.ToListAsync();
        }

        public async Task<Author?> GetByIdAsync(int id)
        {
            return await _dbContext.Authors.FirstOrDefaultAsync(a => a.AuthorId == id);
        }

        public async Task<Author> CreateAsync(Author author)
        {
            await _dbContext.Authors.AddAsync(author);
            await _dbContext.SaveChangesAsync();
            return author;
        }

        public async Task<Author?> UpdateAsync(Author author, int id)
        {
            var authorExist = await _dbContext.Authors.FirstOrDefaultAsync(a => a.AuthorId == id);
            if (authorExist == null) { return null; }
            authorExist.Name = author.Name;
            await _dbContext.SaveChangesAsync();
            return authorExist;
        }

        public async Task<Author?> DeleteAsync(int id)
        {
            var authorExist = await _dbContext.Authors.FirstOrDefaultAsync(a => a.AuthorId == id);
            if (authorExist == null) { return null; }
            _dbContext.Authors.Remove(authorExist);
            await _dbContext.SaveChangesAsync();
            return authorExist;
        }
    }
}
