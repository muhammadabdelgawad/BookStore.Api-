

using System.Threading.Tasks;
using Data_Access.Data;
using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace Data_Access.Repositories
{
    public class ProductRepository:IProductRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public ProductRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddProductAync(Product product)
        {
             await _dbContext.Products.AddAsync(product);
        }

        public void DeleteProduct(Product product)
        {
             _dbContext.Products.Remove(product);
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _dbContext.Products.ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
           return await _dbContext.Products
                .FindAsync(id);
        }
      
        public void UpdateProduct(Product product)
        {
            _dbContext.Products.Update(product);
        }
    }
}
