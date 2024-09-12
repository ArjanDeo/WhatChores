using DataAccess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace API.IntegrationTests
{
    internal class WhatChoresWebApplicationFactory : WebApplicationFactory<Program>
    {
        override protected void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                // Remove the existing DbContextOptions
                services.RemoveAll(typeof(DbContextOptions<WhatChoresDbContext>));

                // Register a new DBContext that will use our test connection string
                string? connString = GetConnectionString();
                services.AddSqlServer<WhatChoresDbContext>(connString);

                // Delete the database (if exists) to ensure we start clean
                WhatChoresDbContext dbContext = CreateDbContext(services);
                dbContext.Database.EnsureDeleted();
            });
        }

        private static string? GetConnectionString()
        {
            var configuration = new ConfigurationBuilder()
                .AddUserSecrets<WhatChoresWebApplicationFactory>()
                .Build();

            var connString = configuration.GetConnectionString("WhatChores-Testing");
            return connString;
        }

        private static WhatChoresDbContext CreateDbContext(IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<WhatChoresDbContext>();
            return dbContext;
        }
    }

}
