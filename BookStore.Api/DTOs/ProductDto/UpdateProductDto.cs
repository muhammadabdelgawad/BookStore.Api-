using System.ComponentModel.DataAnnotations;

namespace BookStore.Api.DTOs.ProductDto
{
    public class UpdateProductDto
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
