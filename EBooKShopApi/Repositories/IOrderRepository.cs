using EBooKShopApi.Models;
using EBooKShopApi.ViewModels;

namespace EBooKShopApi.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<OrderItem>?> GetCartItemsAsync(int userId);
        Task<Order?> GetDetailOrderAsync(int orderId, int userId);
        Task<bool> AddToCartAsync(int bookId, int quantity, int userId);
    }
}