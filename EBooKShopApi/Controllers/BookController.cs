using EBooKShopApi.Models;
using EBooKShopApi.Repositories;
using EBooKShopApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EBooKShopApi.Controllers
{
    [ApiController]
    [Route("api/books")]
    public class BookController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        private readonly ICategoriesRepository _categoriesRepository;

        public BookController(IBookRepository bookRepository, ICategoriesRepository categoriesRepository)
        {
            _bookRepository = bookRepository;
            _categoriesRepository = categoriesRepository;
        }

        //https://localhost:port/api/books
        [HttpGet]
        public async Task<IActionResult> GetAllBook()
        {
            try
            {
                List<Book> books = await _bookRepository.GetBooksAsync();
                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Get Books is successful",
                    Data = books
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        //https://localhost:port/api/books/{id}
        [HttpGet]
        [Route("{id:int}")]
        public async Task<ActionResult> GetBookById(int id)
        {
            try
            {

                var book = await _bookRepository.GetBookByIdAsync(id);
                bool isSuccess = book != null;

                return Ok(new ApiResponse
                {
                    Success = isSuccess ? true : false,
                    Message = isSuccess ? "Get book is successful!" : "book not found!",
                    Data = book
                });

            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        //https://localhost:port/api/books/{name}
        [HttpGet]
        [Route("search/{name}")]
        public async Task<ActionResult> SearchBooksByName(string name)
        {
            try
            {
                var books = await _bookRepository.SearchBooksByNameAsync(name);

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Get books is successful!",
                    Data = books
                });

            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }


        //https://localhost:port/api/books/category/{id}
        [HttpGet("category/{id:int}")]
        public async Task<ActionResult> GetBooksByCategory(int id)
        {
            try
            {

                List<Book> books = await _bookRepository.GetBooksByCategoryAsync(id);
                bool isSuccess = books.Count != 0;

                return Ok(new ApiResponse
                {
                    Success = isSuccess ? true : false,
                    Message = isSuccess ? "Get books is successful!" : "books not found!",
                    Data = books
                });

            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        //https://localhost:port/api/books/author/{id}
        [HttpGet("author/{id:int}")]
        public async Task<ActionResult> GetBooksByAuthor(int id)
        {
            try
            {

                List<Book> books = await _bookRepository.GetBooksByAuthorAsync(id);
                bool isSuccess = books.Count != 0;

                return Ok(new ApiResponse
                {
                    Success = isSuccess ? true : false,
                    Message = isSuccess ? "Get books is successful!" : "books not found!",
                    Data = books
                });

            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        //https://localhost:port/api/books
        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult> AddBook([FromForm] Book book, IFormFile imageFile)
        {
            try
            {
                Book newBook = await _bookRepository.AddBookAsync(book, imageFile);

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Add book is successful",
                    Data = newBook
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        //https://localhost:port/api/books/id
        [Authorize(Roles = "admin")]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteBook(int id)
        {
            try
            {
                bool res = await _bookRepository.DeleteBookAsync(id);
                return Ok(new ApiResponse
                {
                    Success = res,
                    Message = res ? "Delete is successful" : "Book does not exist"
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        //https://localhost:port/api/books/id
        [Authorize(Roles = "admin")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateBook(int id, [FromForm] BookViewModel bookViewModel, IFormFile imageFile)
        {
            try
            {
                var res = await _bookRepository.UpdateBookAsync(id, bookViewModel, imageFile);
                return Ok(new ApiResponse
                {
                    Success = res != null ? true : false,
                    Message = res != null ? "Update is successful" : "Update is not successful",
                    Data = res
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}