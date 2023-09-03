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

        [Required]
        [MaxLength(255)]
        [Column("name")]
        public string Name { get; set; } = "";

        [MaxLength]
        [Column("description")]
        public string Description { get; set; } = "";

        [Required]
        [Column("price")]
        public float Price { get; set; } = 0;

        [Required]
        [Column("quantity")]
        public int Quantity { get; set; } = 0;

        [Column("author_id")]
        public int? AuthorId { get; set; }
        public Author? Author { get; set; }

        [Column("category_id")]

        public int? CategoryId { get; set; }
        public Category? Category { get; set; }
    }


}
