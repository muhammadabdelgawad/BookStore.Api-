
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }

        [Range(0, 10000)]
        [Column("BookPrice")]
        public double Price { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    
    }
}
