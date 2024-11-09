using Domain.Categories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories
{
    internal sealed class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Add(Category category)
        {
            _context.Categories.Add(category);
        }

        public Task<Category?> GetByIdAsync(CategoryId id)
        {
            return _context.Categories.SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task<List<Category>> GetRangeAsync(List<CategoryId> ids)
        {
            return await _context.Categories
                .Where(c => ids.Contains(c.Id))
                .ToListAsync();
        }

        public async Task<bool> IsNameUniqueAsync(string name)
        {
            return await _context.Categories.AnyAsync(c => c.Name == name);
        }

        public void Remove(Category category)
        {
            _context.Categories.Remove(category);
        }

        public void Update(Category category)
        {
            _context.Categories.Update(category);
        }
    }
}
