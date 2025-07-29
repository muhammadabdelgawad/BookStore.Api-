

using Models.Entities;

namespace Data_Access.Repositories
{
    public interface ICategoryRepository
    {
        Task <IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category> GetCategoryByIdAsync(int id);
        Task AddCategoryAync(Category category);
        void UpdateCategory(Category category);
        void DeleteCategory(Category category);
    }
}
