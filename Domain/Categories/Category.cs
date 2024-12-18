using Domain.Primitives;
using System;

namespace Domain.Categories
{
    public class Category : Entity
    {
        private Category(CategoryId id, string name, string? description)
        {
            Id = id;
            Name = name;
            Description = description;
        }      

        public CategoryId Id { get; private set; }

        public string Name { get; private set; }

        public string? Description { get; private set; }  
        
        public static Category Create(string name, string? description)
        {
            return new Category(new CategoryId(Guid.NewGuid()), name, description);
        }

        public void Update(string name, string? description)
        {
            Name = name;
            Description = description;
        }
    }
}
