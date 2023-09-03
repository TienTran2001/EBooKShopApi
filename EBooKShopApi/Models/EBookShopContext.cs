﻿using Microsoft.EntityFrameworkCore;

namespace EBooKShopApi.Models
{
    public class EBookShopContext : DbContext
    {
        public EBookShopContext(DbContextOptions<EBookShopContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories{ get; set; }
        public DbSet<Book> Books { get; set; }

    }
}
