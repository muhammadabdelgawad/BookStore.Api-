
using Microsoft.EntityFrameworkCore;
using Models.Configurations;
using Models.Entities;

namespace Data_Access.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

      
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            base.OnModelCreating(modelBuilder);


            //indexing For Category CatName
            modelBuilder.Entity<Category>()
                        .HasIndex(c => c.CatName)
                        .IsUnique(); // Ensure CatName is unique

            // Seed data for Products
            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Title = "Laptop", Author = "ahmed", Description = "test01", Price = 450, CategoryId = 1, },
                new Product { Id = 2, Title = "Mobile", Author = "muhammad", Description = "test02", Price = 150, CategoryId = 3, },
                new Product { Id = 3, Title = "PC", Author = "kareem", Description = "test03", Price = 320, CategoryId = 3, },
                new Product { Id = 4, Title = "PC", Author = "mido", Description = "test04", Price = 630, CategoryId = 3, },
                new Product { Id = 5, Title = "BOOK", Author = "hamda", Description = "test05", Price = 240, CategoryId = 2, },
                new Product { Id = 6, Title = "BOOK", Author = "mahmoud", Description = "test06", Price = 50, CategoryId = 2, },
                new Product { Id = 7, Title = "Mobile", Author = "abdullah", Description = "test07", Price = 10, CategoryId = 1, },
                new Product { Id = 8, Title = "Mobile", Author = "alaa", Description = "test08", Price = 100, CategoryId = 1, },
                new Product { Id = 9, Title = "Laptop", Author = "abdulrahman", Description = "test09", Price = 270, CategoryId = 2, }
            );
            // Seed data for Categ
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, CatName = "Electronics", CatOrder = 1 , CreatedDate = DateTime.UtcNow },
                new Category { Id = 2, CatName = "Books", CatOrder = 2, CreatedDate = DateTime.UtcNow },
                new Category { Id = 3, CatName = "Computers", CatOrder = 3, CreatedDate = DateTime.UtcNow },
                new Category { Id = 4, CatName = "Phones", CatOrder = 4, CreatedDate = DateTime.UtcNow }
            );

        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }

    }
}

