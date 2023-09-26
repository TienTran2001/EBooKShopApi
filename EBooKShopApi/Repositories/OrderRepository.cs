
using EBooKShopApi.Models;
using EBooKShopApi.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EBooKShopApi.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly EBookShopContext _context;

        public OrderRepository(EBookShopContext context)
        {
            _context = context;
        }

        // lay danh sach trong gio hang
        public async Task<IEnumerable<OrderItem>?> GetCartItemsAsync(int userId)
        {
            var carts = await _context.OrderItems
                    .Include(oi => oi.Book) // Nạp thông tin về sách liên quan đến từng order item
                    .Include(oi => oi.Order)
                    .Where(oi => oi.Order.UserId == userId && (OrderStatus)oi.Order.OrderStatus == OrderStatus.InCart)
                    .ToListAsync();

            if (carts != null)
                return carts;
            return null;
        }

        // add to cart 
        public async Task<bool> AddToCartAsync(int bookId, int quantity, int userId)
        {
            // kiem tra don hang co trang thai la InCart hay khong
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .Where(o => o.UserId == userId && (OrderStatus)o.OrderStatus == OrderStatus.InCart)
                .FirstOrDefaultAsync();

            if (order == null)
            {
                return false;
            }

            // kiem tra sach do co trong gio hang hay khong
            var existingBook = order.OrderItems.FirstOrDefault(o => o.BookId == bookId);

            if (existingBook != null)
            {
                existingBook.Quantity += quantity;
            }
            else
            {
                // neu sach chua co trong gio hang thi them moi

                var book = await _context.Books
                    .FindAsync(bookId);
                if (book != null)
                {
                    // Tạo một chi tiết đơn hàng mới và thêm vào đơn hàng
                    var orderItem = new OrderItem
                    {
                        OrderId = order.OrderId,
                        BookId = book.BookId,
                        Quantity = quantity,
                        Price = (decimal)book.Price,
                    };
                    _context.OrderItems.Add(orderItem);
                }
                else
                {
                    // sach khong ton tai
                    return true;
                }
            }

            // Cập nhật tổng tiền của order
            order.TotalAmount = order.OrderItems.Sum(oi => oi.Price * oi.Quantity);


            await _context.SaveChangesAsync();
            return true;
        }

        // xem chi tiết hóa đơn
        public async Task<Order?> GetDetailOrderAsync(int orderId, int userId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Book) // Nếu bạn muốn lấy thông tin sách của từng order item.
                .FirstOrDefaultAsync(o => o.UserId == userId && o.OrderId == orderId);


            return order;
        }

        //

    }
}