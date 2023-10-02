using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using EBooKShopApi.Models;

[Table("orders")]
public class Order
{
    [Key]
    [Column("order_id")]
    public int OrderId { get; set; }

    [Required]
    [Column("user_id")]
    public int UserId { get; set; }

    [Required]
    [Column("order_date")]
    public DateTime OrderDate { get; set; }

    [Required]
    [Column("total_amount")]
    [DisplayFormat(DataFormatString = "{0:C2}")] // Định dạng hiển thị tiền tệ
    public decimal TotalAmount { get; set; }

    [Required]
    [Column("order_status")]
    public int OrderStatus { get; set; }

    [ForeignKey("UserId")]
    public User User { get; set; }

    // Thuộc tính tham chiếu danh sách OrderItems
  /*  [JsonIgnore]*/
    public ICollection<OrderItem> OrderItems { get; set; }
}
