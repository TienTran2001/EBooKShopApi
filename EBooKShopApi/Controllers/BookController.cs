using EBooKShopApi.Models;
using EBooKShopApi.Repositories;
using EBooKShopApi.ViewModels;
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
    }
}