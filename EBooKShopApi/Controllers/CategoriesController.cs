using EBooKShopApi.Models;
using EBooKShopApi.Repositories;
using EBooKShopApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EBooKShopApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoriesRepository _repository;

        public CategoriesController(ICategoriesRepository repository)
        {
            this._repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var dataes = await _repository.GetAllAsync();
                return Ok(dataes);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet]
        [Route("{id:int}")]
        [Authorize(Roles = "user")]
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
        public async Task<IActionResult> Create([FromBody] CategoryViewModel categoryViewModel)
        {
            try
            {
                var category = new Category
                {
                    Name = categoryViewModel.CategoryName
                };
                await _repository.CreateAsync(category);
                return StatusCode(201, new ApiResponse
                {
                    Success = true,
                    Message = "Created Successfully!",
                    Data = category
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
        public async Task<IActionResult> Update([FromBody] CategoryViewModel categoryViewModel, [FromRoute] int id)
        {
            try
            {
                var category = new Category
                {
                    Name = categoryViewModel.CategoryName
                };
                if (category == null)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Not Found Category!",
                        Data = null
                    });
                }
                category = await _repository.UpdateAsync(category, id);
                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Updated successfully!",
                    Data = category
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
