using EBooKShopApi.Models;
using EBooKShopApi.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace EBooKShopApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly EBookShopContext _context;
        private readonly IConfiguration _config;

        public UserRepository(EBookShopContext context, IConfiguration config)
        {
            _context = context;
            _config = config;   
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetUserLoggedAsync(int id)
        {
            var user = await _context.Users
                .Where(u => u.UserId == id)
                .Select(u => new User
                {
                    Username = u.Username,
                    Email = u.Email,
                    Phone = u.Phone,
                    FullName = u.FullName,
                    Address = u.Address,
                    Role = ""
                })
                .FirstOrDefaultAsync();
            return user;
        }


        public async Task<bool> DeleteUsesrAsync(int id)
        {
            var userToDelete = await _context.Users.FindAsync(id);
            
            if (userToDelete == null) 
            {
                return false;
            }

            _context.Users.Remove(userToDelete);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<User?> Register(RegisterViewModel registerViewModel)
        {
            // check if the username and email already exists in the database or not     
            // check username already exists
            var existingUserByUsername = await _context.Users
                .SingleOrDefaultAsync(u => u.Username == registerViewModel.Username);

            if (existingUserByUsername != null)
            {
                throw new ArgumentException("Username already exists");
            }
            
            // check email already exists
            var existingUserByEmail = await _context.Users
                .SingleOrDefaultAsync(u => u.Email == registerViewModel.Email);

            if (existingUserByEmail != null)
            {
                throw new ArgumentException("Email already exists");
            }

            // Perform adding a new user
            User newUser = new User
            {
                Username = registerViewModel.Username,
                Email = registerViewModel.Email,
                FullName = registerViewModel.FullName,
                HashedPassword = registerViewModel.HashedPassword,
                Phone = registerViewModel.Phone,
                Address = "",
                Role = "user"
            };
            await _context.AddAsync(newUser); // add newuser to database
            await _context.SaveChangesAsync(); // commit change database

            return newUser;
        }


        public async Task<string> Login (LoginViewModel loginViewModel)
        {
            // check user already exists in database or not
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginViewModel.Email);

            // case user exists
            if (user != null && user.HashedPassword == loginViewModel.HashedPassword)
            {
                // Generate a JWT string to send to the client
                // If authentication is successful, create a JWT token
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_config["Jwt:SecretKey"] ?? "");
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim("userId", user.UserId.ToString()),
                        new Claim(ClaimTypes.Role, user.Role)

                    }),
                    Expires = DateTime.UtcNow.AddDays(30),
                    SigningCredentials = new SigningCredentials
                        (new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var jwtToken = tokenHandler.WriteToken(token);
                return jwtToken;
            }
            else
            {
                throw new ArgumentException("Wrong email or password");
            }



        }
    }
}
