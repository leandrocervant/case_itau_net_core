using CaseItau.Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CaseItau.IntegrationTests.Common;

public class CaseItauWebApplicationFactory : WebApplicationFactory<Program>
{
    private SqliteTestDatabase _testDatabase = null!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        _testDatabase = SqliteTestDatabase.CreateAndInitialize();

        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<FundsDbContext>));
            services.AddDbContext<FundsDbContext>((sp, options) => options.UseSqlite(_testDatabase.Connection));
        });
    }
}
