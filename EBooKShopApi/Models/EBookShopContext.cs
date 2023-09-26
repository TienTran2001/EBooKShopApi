using Microsoft.EntityFrameworkCore;

namespace EBooKShopApi.Models
{
    public class EBookShopContext : DbContext
    {
        public EBookShopContext(DbContextOptions<EBookShopContext> options)
            : base(options)
        {
        }


        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

    }
}
