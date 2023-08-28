
using EBooKShopApi.Models;
using EBooKShopApi.Repositories;
using EBooKShopApi.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



namespace EBooKShopApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;

        }

        //https://localhost:port/api/user/getalluser
        [HttpGet("getalluser")]
        [Authorize]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAllUser ()
        {
            try
            {
                List<User> users = await _userRepository.GetAllUsersAsync();
                return Ok(users);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        //https://localhost:port/api/user/delete
        [HttpDelete("delete")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> DeleteUser()
        {
            try
            {
                // lấy thông tin của người dùng từ claims
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId");
                if (userIdClaim != null)
                {
                    int userId = int.Parse(userIdClaim.Value);
                    
                    // call delete user in repo
                    var checkDelete = await _userRepository.DeleteUsesrAsync(userId);
                    if (checkDelete == false)
                    {
                        return Ok(new ApiResponse
                        {
                            Success = false,
                            Message = "Do not delete user",
                            Data = null
                        });
                    } 
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Message = "Delete user is successfull",
                        Data = null
                    });

                }
                return Unauthorized();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }


        //https://localhost:port/api/user/getuserlogged
        [HttpGet("getuserlogged")]
        [Authorize]
        public async Task<IActionResult> GetUserLogged() // lấy thông tin user đang login
        {
            try
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "userId");
                if (userIdClaim != null) 
                {
                    int userId = int.Parse(userIdClaim.Value);
                    var user = await _userRepository.GetUserLoggedAsync(userId);
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Message = "Get user is successfull",
                        Data = user
                    });

                }
                return Ok(new ApiResponse
                {
                    Success = false,
                    Message = "Not logged in",
                    Data = null
                });
            }
            catch(ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message }); ;
            }
        }

        //https://localhost:port/api/user/register
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            try
            {
                User? user = await _userRepository.Register(registerViewModel);
                return Ok(user);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        //https://localhost:port/api/user/login
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            try
            {
                string jwtToken = await _userRepository.Login(loginViewModel);
                return Ok(new { jwtToken });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
