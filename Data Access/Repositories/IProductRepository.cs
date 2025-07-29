
using Models.Entities;

namespace Data_Access.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product> GetProductyByIdAsync(int id);
        Task AddProductAync(Product product);
        void UpdateProduct(Product product);
        void DeleteProduct(Product product);
    }
}
