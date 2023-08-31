using System.ComponentModel.DataAnnotations;

namespace EBooKShopApi.Models
{
    public class Category
    {
        [Key]
        [Required]
        public int category_id { get; set; }

        [Required]
        public string name { get; set; }
    }
}
