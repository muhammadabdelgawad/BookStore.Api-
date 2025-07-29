
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Entities;

namespace Models.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.CatName)
                .IsRequired()
                .HasMaxLength(50);

            // builder.Ignore(c => c.CreatedDate);

            builder.Property(c => c.CatOrder)
                .IsRequired();
            builder.Property<DateTime>("CreatedDate");
        }
    }
}
