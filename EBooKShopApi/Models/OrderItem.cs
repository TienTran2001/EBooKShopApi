
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EBooKShopApi.Models;

[Table("orderItems")]
public class OrderItem
{
    [Key]
    [Column("order_item_id")]
    public int OrderItemId { get; set; }

    [Column("order_id")]
    public int OrderId { get; set; }

    [Column("book_id")]
    public int BookId { get; set; }

    [Column("quantity")]
    public int Quantity { get; set; }

    [Column("price")]
    public decimal Price { get; set; }

    // Liên kết với bảng Order
    [ForeignKey("OrderId")]
    public Order Order { get; set; }

    // Liên kết với bảng Book (nếu có)
    [ForeignKey("BookId")]
    public Book Book { get; set; }
}