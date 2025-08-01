using System.ComponentModel.DataAnnotations;

namespace BookStore.Api.DTOs.ProductDto
{
    public class ProductResponseDto
    {
        public string Description { get; set; }
       
        public string Title { get; set; }
        public string Author { get; set; }
        public double Price { get; set; }
        public int CategoryId { get; set; }
        public String CategoryName { get; set; }
    }
}
