using EBooKShopApi.Models;
using EBooKShopApi.Repositories;
using EBooKShopApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace EBooKShopApi.Controllers
{
    [Route("api/authors")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorRepository _repository;

        public AuthorController(IAuthorRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var data = await _repository.GetAllAsync();
                return Ok(data);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                var data = await _repository.GetByIdAsync(id);
                if (data == null)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Not Found Category!",
                        Data = null
                    });
                }
                return Ok(data);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create([FromBody] AuthorViewModel authorViewModel)
        {
            try
            {
                var author = new Author
                {
                    Name = authorViewModel.Name
                };
                await _repository.CreateAsync(author);
                return StatusCode(201, new ApiResponse
                {
                    Success = true,
                    Message = "Created Successfully!",
                    Data = author
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Update([FromBody] AuthorViewModel authorViewModel, [FromRoute] int id)
        {
            try
            {
                var author = new Author
                {
                    Name = authorViewModel.Name
                };
                if (author == null)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Not Found Category!",
                        Data = null
                    });
                }
                author = await _repository.UpdateAsync(author, id);
                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Updated successfully!",
                    Data = author
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                var data = await _repository.DeleteAsync(id);
                if (data == null)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Not Found Category!",
                        Data = null
                    });
                }
                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Deleted successfully!",
                    Data = data
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
        }
    }
}
