using System.ComponentModel.DataAnnotations;

namespace EBooKShopApi.ViewModels
{
  public class BookViewModel
  {
    [Required(ErrorMessage = "Tên sách là bắt buộc")]
    [MaxLength(255)]
    public string Name { get; set; } = "";

    [MaxLength]
    public string Description { get; set; } = "";

    public double Price { get; set; }

    [Required]
    public int Quantity { get; set; }

    public string Image { get; set; } = "";

    public int? AuthorId { get; set; }
    public int? CategoryId { get; set; }
  }
}
