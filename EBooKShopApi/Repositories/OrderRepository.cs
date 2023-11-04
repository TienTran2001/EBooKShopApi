
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
        public async Task<Order?> GetCartItemsAsync(int userId)
        {
            /*var carts = await _context.OrderItems
                    .Include(oi => oi.Book) // Nạp thông tin về sách liên quan đến từng order item
                    .Where(oi => oi.Order.UserId == userId && (OrderStatus)oi.Order.OrderStatus == OrderStatus.InCart)
                    .ToListAsync();*/
            
            var carts = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Book) // lấy thông tin sách của từng order item.
                .FirstOrDefaultAsync(o => o.UserId == userId && (OrderStatus)o.OrderStatus == OrderStatus.InCart);
           
            return carts;
        }

        public async Task<bool> RecalculateOrderTotalAsync(Order order)
        {
            
            order.TotalAmount = order.OrderItems.Sum(oi => oi.Price * oi.Quantity);
            await _context.SaveChangesAsync();
            return true;
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

                order = new Order
                {
                    UserId = userId,
                    OrderStatus = (int)OrderStatus.InCart,
                    OrderDate = DateTime.Now,
                    TotalAmount = 0
                };
                _context.Orders.Add(order);
                await _context.SaveChangesAsync(); 
            }

            
            var book = await _context.Books.FindAsync(bookId);

            if (book != null && book.Stock >= quantity)
            {
                // kiem tra sach do co trong gio hang hay khong
                var existingBook = order?.OrderItems?.FirstOrDefault(o => o.BookId == bookId);

                if (existingBook != null)
                {
                    existingBook.Quantity += quantity;
                } else
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
                // Cập nhật tổng tiền của order
                if (order != null)
                    await RecalculateOrderTotalAsync(order);

                // Giảm số lượng trong kho
                book.Stock -= quantity;
                
                await _context.SaveChangesAsync();
                return true;
            } else
            {
                return false;
            }
   
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

        // ham thay doi so luong don hang
        public async Task<bool> ChangeCartItemAsync(int orderItemId, int quantity)
        {
            var orderItem = await _context.OrderItems.FindAsync(orderItemId);
            if (orderItem == null)
            {
                return false;
            }

            var oldQuantity = orderItem.Quantity;

            var book = await _context.Books.FindAsync(orderItem.BookId);
            if (book != null && (quantity - oldQuantity) > book.Stock)
            {
                // Số lượng mới vượt quá số lượng trong kho
                return false;
            }
            
            orderItem.Quantity = quantity;

            if (book != null)
            {
                book.Stock += oldQuantity - quantity;
            }

            await _context.SaveChangesAsync();

            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .Where(o => (OrderStatus)o.OrderStatus == OrderStatus.InCart)
                .FirstOrDefaultAsync();
            if(order != null)
                await RecalculateOrderTotalAsync(order);

            return true;
        }

        // ham xoa phan tu trong gio hang
        public async Task<bool> RemoveCartItemAsync(int orderItemId, int userId)
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
            var existingItem = order.OrderItems.FirstOrDefault(oi => oi.OrderItemId == orderItemId);

            if (existingItem == null) return false;

            var book = await _context.Books.FindAsync(existingItem.BookId);
            if (book == null)
            {
                return false;
            }
            var quantity = existingItem.Quantity;
            book.Stock += quantity;

            _context.OrderItems.Remove(existingItem);

            await _context.SaveChangesAsync();

            var orderNew = await _context.Orders
                .Include(o => o.OrderItems)
                .Where(o => (OrderStatus)o.OrderStatus == OrderStatus.InCart)
                .FirstOrDefaultAsync();
            if (orderNew != null)
                await RecalculateOrderTotalAsync(orderNew);
            return true;

        }    
        
        // ham doi trang thai 
        public async Task<bool> ChangeOrderStatusAsync(int orderId, OrderStatus statusOld, OrderStatus statusNew )
        {
            // tim order theo id
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null) return false;

            if (statusNew == OrderStatus.Pending)
            {
                order.OrderDate = DateTime.Now;
            }

            if (order.OrderStatus == (int)statusOld)
            {
                order.OrderStatus = (int)statusNew;

                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        // ham huy hoa don
        public async Task<bool> CancelOderAsync (int orderId)
        {
            // tim order theo id
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null) return false;
            
            // phải ở trạng thái chờ và trạng thái xác nhận mới (sau có thể bổ xung) có thể hủy đơn
            if (order.OrderStatus == (int)OrderStatus.Pending || order.OrderStatus == (int)OrderStatus.Confirmed)
            {
                order.OrderStatus = (int)OrderStatus.Cancel;
                foreach (var orderItem in order.OrderItems)
                {
                    
                    var product = await _context.Books.FindAsync(orderItem.BookId);

                    if (product != null)
                    {
                        // Trả lại số lượng đặt cho kho
                        product.Stock += orderItem.Quantity;
                    }
                }

                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        // ham lay ra danh sach order theo trang thai
        public async Task<List<Order>?> GetOrderByOrderStatusAsync(int status, int userId)
        {
           
            var order = await _context.Orders
                .Where( o => o.UserId == userId && o.OrderStatus == status)
                .ToListAsync();
            return order;
        }
    }
}