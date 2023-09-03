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

        public BookController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        //https://localhost:port/api/books
        [HttpGet]
        public async Task<IActionResult> GetAllBook()
        {
            try
            {
               // List<User> users = await _userRepository.GetAllUsersAsync();
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
    }
}
