using System;

namespace Domain.Categories
{
    public sealed class CategoryNotFoundException : Exception
    {
        public CategoryNotFoundException(CategoryId id) 
            : base($"The category with the ID = {id.Value} was not found") { }
    }
}
