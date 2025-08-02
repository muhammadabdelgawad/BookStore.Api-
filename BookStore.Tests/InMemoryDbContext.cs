using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data_Access.Data;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Tests
{
    internal class InMemoryDbContext : ApplicationDbContext
    {
        public InMemoryDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseInMemoryDatabase("BookStoreTestDb");
            optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString()); // Use a unique database name for each test run
        }

        public override void Dispose()
        {
            Database.EnsureDeleted(); // Ensure the in-memory database is deleted after tests
            base.Dispose();
        }
    }
}
