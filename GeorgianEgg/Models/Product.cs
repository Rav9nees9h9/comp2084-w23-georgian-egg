using System.ComponentModel.DataAnnotations;

namespace GeorgianEgg.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public String? Name { get; set; }
        public String? Description { get; set; }
        public decimal Price { get; set; }
        public String? Image { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        public int BrandId { get; set; }
        public int CategoryId { get; set; }

        public Brand? Brand { get; set; }
        public Category? Category { get; set; }
    }
}
