using CaseItau.Domain.Entities;
using CaseItau.Infrastructure.Persistence.Contexts;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace CaseItau.IntegrationTests.Common;

public class SqliteTestDatabase : IDisposable
{
    public SqliteConnection Connection { get; }

    public static SqliteTestDatabase CreateAndInitialize()
    {
        var testDatabase = new SqliteTestDatabase("DataSource=:memory:");

        testDatabase.InitializeDatabase();

        return testDatabase;
    }

    public void InitializeDatabase()
    {
        Connection.Open();
        var options = new DbContextOptionsBuilder<FundsDbContext>()
            .UseSqlite(Connection)
            .Options;

        var context = new FundsDbContext(options);

        context.Database.EnsureCreated();

        SeedDatabase(context);
    }
    private static void SeedDatabase(FundsDbContext context)
    {
        // Clear existing data
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        // Add test fund types
        var fundTypes = new[]
        {
            new FundType("Equity Fund"),
            new FundType("Bond Fund"),
            new FundType("Mixed Fund")
        };

        context.FundTypes.AddRange(fundTypes);
        context.SaveChanges();
    }

    public void ResetDatabase()
    {
        Connection.Close();

        InitializeDatabase();
    }

    private SqliteTestDatabase(string connectionString)
    {
        Connection = new SqliteConnection(connectionString);
    }

    public void Dispose()
    {
        Connection.Close();
    }
}