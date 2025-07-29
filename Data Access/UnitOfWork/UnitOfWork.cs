

using Data_Access.Data;
using Data_Access.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Data_Access.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        private ICategoryRepository _categoryRepository;
        private IProductRepository _productRepository;
        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public ICategoryRepository Categories
        { 
            get
            {
                _categoryRepository ??= new CategoryRepository(_dbContext); // Assuming CategoryRepository concrete class
                return _categoryRepository;
            }
        }

        public IProductRepository Products
        {
            get
            {
                _productRepository ??= new ProductRepository(_dbContext); // Assuming ProductRepository concrete class
                return _productRepository;
            }
        }

        public Task<int> SaveAsync()
        {
            throw new NotImplementedException();
        }
    }
}
