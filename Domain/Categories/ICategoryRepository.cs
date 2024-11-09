using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Categories
{
    public interface ICategoryRepository
    {
        void Add(Category category);

        Task<Category?> GetByIdAsync(CategoryId id);

        Task<List<Category>> GetRangeAsync(List<CategoryId> ids);

        void Update(Category category);

        void Remove(Category category);

        Task<bool> IsNameUniqueAsync(string name);
    }
}
