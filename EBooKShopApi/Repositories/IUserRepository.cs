using EBooKShopApi.Models;
using EBooKShopApi.ViewModels;

namespace EBooKShopApi.Repositories
{
    public interface IUserRepository
    {
        Task<User?> Register(RegisterViewModel registerViewModel);
        Task<string> Login (LoginViewModel loginViewModel);
        Task<List<User>> GetAllUsersAsync();
        Task<User?> GetUserLoggedAsync(int id);
        Task<bool> DeleteUsesrAsync(int id);
    }
}

