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
    }
}
