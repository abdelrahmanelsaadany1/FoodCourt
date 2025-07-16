using Domain.Entities;
using System.Threading.Tasks;

namespace Services.Abstractions.ICategoryService
{
    public interface ICategoryService
    {
        Task AddCategoryAsync(Category category);
    }
}
