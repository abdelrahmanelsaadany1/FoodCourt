using Domain.Entities;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface ICategoryService
    {
        Task AddCategoryAsync(Category category);
    }
}
