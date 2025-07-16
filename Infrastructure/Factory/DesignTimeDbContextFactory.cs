using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Factory
{
    internal class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<InventorySystemDbContext>
    {
        public InventorySystemDbContext CreateDbContext(string[] args)
        {
            var currentDir = Directory.GetCurrentDirectory();

            var parentDir = Directory.GetParent(currentDir)?.FullName;

            var clientAPIDir = Path.Combine(parentDir, "Client API");

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(clientAPIDir)
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<InventorySystemDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnectionString");

            builder.UseSqlServer(connectionString);

            return new InventorySystemDbContext(builder.Options);
        }
    }
}
