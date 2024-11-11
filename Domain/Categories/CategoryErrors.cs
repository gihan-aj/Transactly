using SharedKernel;
using System;

namespace Domain.Categories
{
    public static class CategoryErrors
    {
        public static readonly Error BulkNotFound = new("Category.BulkNotFound", "Categories were not found");

        public static Error NotFound(Guid id) => new("Category.NotFound", $"The category with the Id = '{id}' was not found");

        public static Error DuplicateName(string name) => new("Category.DuplicateName", $"The name, '{name}' is not unique");
    }
}
