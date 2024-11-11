using Domain.Categories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    internal class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .HasConversion(
                id => id.Value,
                value => new CategoryId(value));

            builder.Property(c => c.Name)
                .HasMaxLength(50);

            builder.HasIndex(c => c.Name)
                .IsUnique()
                .HasFilter("[IsDeleted] = 0");

            //builder.HasIndex(c => new { c.IsActive, c.Name });
            builder.HasIndex(c => c.IsActive);

            builder.HasIndex(c => c.IsDeleted);

            builder.Property(c => c.Description)
                .HasMaxLength(255);

            builder.HasQueryFilter(c => !c.IsDeleted);
        }
    }
}
