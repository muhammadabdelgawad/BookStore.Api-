using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookStore.Api.DTOs.ProductDto
{
    public class CreateProductDto
    {
        public string Description { get; set; }
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(100)]
        public string Author { get; set; }
        [Required]
        public double Price { get; set; }
        public int CategoryId { get; set; }
    }
}
