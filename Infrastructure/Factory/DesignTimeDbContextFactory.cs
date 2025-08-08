using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Factory
{
    internal class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<StoreFrontDbContext>
    {
        public StoreFrontDbContext CreateDbContext(string[] args)
        {
            var basePath = Directory.GetCurrentDirectory();
            //var parentDir = Directory.GetParent(basePath)?.FullName;
            //var clientAPIDir = Path.Combine(parentDir, "Client API");
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            //var connectionString = configuration.GetValue<string>("ConnectionString:DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
                throw new InvalidOperationException("Connection string 'DefaultConnection' not found in appsettings.json.");

            var builder = new DbContextOptionsBuilder<StoreFrontDbContext>();
            builder.UseSqlServer(connectionString);

            return new StoreFrontDbContext(builder.Options);
        }
    }
}
