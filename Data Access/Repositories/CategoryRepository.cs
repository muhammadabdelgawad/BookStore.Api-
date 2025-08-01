
using Data_Access.Data;
using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace Data_Access.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public CategoryRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task AddCategoryAync(Category category)
        {
            await  _dbContext.Categories.AddAsync(category);
             
        }

        public void DeleteCategory(Category category)
        {
            _dbContext.Categories.Remove(category);
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
           return await _dbContext.Categories.ToListAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _dbContext.Categories.FindAsync(id);
        }

        public void UpdateCategory(Category category)
        {
            _dbContext.Categories.Update(category);
        }
    }
}
