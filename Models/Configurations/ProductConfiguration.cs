using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Models.Entities;

namespace Models.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Title)
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(p => p.Description)
                 .HasMaxLength(250)
                .IsRequired(false);

            builder.Property(p => p.Author)
                 .HasMaxLength(50)
                .IsRequired();

            builder.Property(p => p.Price)
                .IsRequired();
            builder.HasOne(p => p.Category)
                   .WithMany(c => c.Products)
                   .HasForeignKey(p => p.CategoryId)
                   .OnDelete(DeleteBehavior.Cascade);

            
        }
    }
}
