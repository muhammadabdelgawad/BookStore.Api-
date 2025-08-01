

using Data_Access.Data;
using Data_Access.Repositories;
using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace Data_Access.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        public IProductRepository Products { get; private set; }
        public ICategoryRepository Categories { get; private set; }
        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            Categories = new CategoryRepository(dbContext);
            Products = new ProductRepository(dbContext);
        }

        public async Task SaveAsync() => await _dbContext.SaveChangesAsync();
        public void Dispose() => _dbContext.Dispose();

      
            public async Task<int> CompleteAsync() => await _dbContext.SaveChangesAsync();
    
    }
}
