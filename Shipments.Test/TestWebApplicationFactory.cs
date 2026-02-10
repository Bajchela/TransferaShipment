using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shipments.Infrastructure.Persistance;

namespace Shipments.Tests;

public class TestWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<DbContextOptions<ShipmentsDbContext>>();

            services.AddDbContext<ShipmentsDbContext>(options =>
                options.UseSqlite("Data Source=shipments-test.db"));

            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ShipmentsDbContext>();

            db.Database.EnsureDeleted(); // čisto za svaki test run
            db.Database.EnsureCreated();
        });
    }
}
