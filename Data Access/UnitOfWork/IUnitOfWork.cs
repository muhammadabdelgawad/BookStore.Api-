

using Data_Access.Repositories;
using Models.Entities;

namespace Data_Access.UnitOfWork
{
    public interface IUnitOfWork
    {
        ICategoryRepository Categories { get; }
        IProductRepository Products { get; }
        Task<int> CompleteAsync();
    }
}
