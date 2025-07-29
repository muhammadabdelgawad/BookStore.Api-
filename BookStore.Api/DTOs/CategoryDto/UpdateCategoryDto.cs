using System.ComponentModel.DataAnnotations;

namespace BookStore.Api.DTOs.CategoryDto
{
    public class UpdateCategoryDto
    {
        [Required]
        [StringLength(100)]
        public string CatName { get; set; }

        [Required]
        public int CatOrder { get; set; }
    }
}
}
