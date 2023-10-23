using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EBooKShopApi.Models
{
    [Table("books")]
    public class Book
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("book_id")]
        public int BookId { get; set; }

        [Required(ErrorMessage = "Tên sách là bắt buộc")]
        [MaxLength(255)]
        [Column("name")]
        public string Name { get; set; } = "";

        [MaxLength]
        [Column("description")]
        public string Description { get; set; } = "";

        [Column("price")]
        public double Price { get; set; }

        [Required]
        [Column("quantity")]
        public int Stock { get; set; }

        [Column("image")]
        public string Image { get; set; } = "";

        [Column("author_id")]
        public int? AuthorId { get; set; }
        public Author? Author { get; set; }

        [Column("category_id")]
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}