using EBooKShopApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EBooKShopApi.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly EBookShopContext _context;

        public BookRepository(EBookShopContext context)
        {
            _context = context;
        }

        // ham lay tat ca cac cuon sach
        public async Task<List<Book>> GetBooksAsync()
        {
            // lay list books trong db
            return await _context.Books
            .Include(b => b.Author)
            .Include(b => b.Category)
            .ToListAsync();
            
        }

        // ham lay detail book 
        public async Task<Book?> GetBookByIdAsync(int id)
        {
            return await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .FirstOrDefaultAsync(b => b.BookId == id);     
        }

        // ham lay sach theo categogy
        public async Task<List<Book>> GetBooksByCategoryAsync(int categoryId)
        {
            return await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Where(b => b.CategoryId == categoryId)
                .ToListAsync();
        }
    }
}