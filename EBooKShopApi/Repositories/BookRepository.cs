using EBooKShopApi.Models;
using EBooKShopApi.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EBooKShopApi.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly EBookShopContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public BookRepository(EBookShopContext context, IWebHostEnvironment hostingEnvironment)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
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

        // ham tim kiem book theo ten
        public async Task<List<Book>> SearchBooksByNameAsync(string name)
        {
            var books = await _context.Books
                .Where(x => x.Name.Contains(name))
                .ToListAsync();

            return books;
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

        // ham lay sach theo author
        public async Task<List<Book>> GetBooksByAuthorAsync(int authorId)
        {
            return await _context.Books
                .Include(b => b.Author)
                .Include(b => b.Category)
                .Where(b => b.AuthorId == authorId)
                .ToListAsync();
        }

        // ham them sach
        public async Task<Book> AddBookAsync(Book book, IFormFile imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {

                string imagePath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                var filePath = Path.Combine(imagePath, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }

                // Lưu đường dẫn ảnh vào trường Image của db
                book.Image = fileName;
            }

            _context.Books.Add(book);
            await _context.SaveChangesAsync();

            return book;
        }

        // ham xoa sach theo id
        public async Task<bool> DeleteBookAsync(int id)
        {
            var bookDelete = await _context.Books.FindAsync(id);
            if (bookDelete == null)
            {
                return false;
            }
            _context.Books.Remove(bookDelete);
            await _context.SaveChangesAsync();
            return true;
        }

        // ham update book tu admin
        public async Task<Book?> UpdateBookAsync(int id, BookViewModel bookViewModel, IFormFile imageFile)
        {
            var existBook = await _context.Books.FindAsync(id);
            if (existBook == null)
            {
                return null;
            }

            // update
            existBook.Name = bookViewModel.Name;
            existBook.Description = bookViewModel.Description;
            existBook.Price = bookViewModel.Price;
            existBook.Stock = bookViewModel.Quantity;
            existBook.AuthorId = bookViewModel.AuthorId;
            existBook.CategoryId = bookViewModel.CategoryId;
            if (imageFile != null)
            {
                var imagePath = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                var filePath = Path.Combine(imagePath, fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }
                existBook.Image = fileName;
            }
            await _context.SaveChangesAsync();

            return existBook;
        }

    }
}
