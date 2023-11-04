using EBooKShopApi.Models;
using EBooKShopApi.ViewModels;

namespace EBooKShopApi.Repositories
{
    public interface IOrderRepository
    {
        Task<Order?> GetCartItemsAsync(int userId);
        Task<Order?> GetDetailOrderAsync(int orderId, int userId);
        Task<bool> AddToCartAsync(int bookId, int quantity, int userId);
        Task<bool> ChangeCartItemAsync(int orderItemId, int quantity);
        Task<bool> RemoveCartItemAsync(int orderItemId, int userId);
        Task<bool> ChangeOrderStatusAsync(int orderId, OrderStatus statusOld, OrderStatus statusNew);
        Task<bool> CancelOderAsync(int orderId);
        Task<List<Order>?> GetOrderByOrderStatusAsync(int status, int userId);
        Task<bool> RecalculateOrderTotalAsync(Order order);
    }
}