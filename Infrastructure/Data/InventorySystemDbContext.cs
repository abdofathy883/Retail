using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class InventorySystemDbContext: DbContext
    {
        public InventorySystemDbContext(DbContextOptions<InventorySystemDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Add any additional model configurations here
        }
        // Define DbSets for your entities here, e.g.:
        // public DbSet<Product> Products { get; set; }
        // public DbSet<Category> Categories { get; set; }
    }
}
