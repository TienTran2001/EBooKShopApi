﻿using EBooKShopApi.Models;
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

    }
}