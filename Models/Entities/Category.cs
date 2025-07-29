using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entities
{
    public class Category
    {
        public int Id { get; set; }
        
        public string CatName { get; set; }
        
        public int CatOrder { get; set; }

        public bool MarkedAsDeleted { get; set; } = false;

        public ICollection<Product> Products { get; set; }
    }
}
