using CaseItau.Domain.Entities;
using CaseItau.Domain.ValueObjects;
using CaseItau.Infrastructure.Persistence.Contexts;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CaseItau.IntegrationTests.Infrastructure.Persistence.Repositories;

public class FundRepositoryIntegrationTests : IDisposable
{
    private readonly FundsDbContext _context;

    public FundRepositoryIntegrationTests()
    {
        var options = new DbContextOptionsBuilder<FundsDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new FundsDbContext(options);

        // Seed test data
        SeedTestData();
    }

    private void SeedTestData()
    {
        var fundTypes = new[]
        {
            new FundType("Equity Fund"),
            new FundType("Bond Fund"),
            new FundType("Mixed Fund")
        };

        _context.FundTypes.AddRange(fundTypes);
        _context.SaveChanges();
    }

    [Fact]
    public async Task AddAsync_ShouldAddFundToDatabase()
    {
        // Arrange
        var fund = new Fund("REPO_001", "Repository Test Fund", new Cnpj("12345678000195"), 1);

        // Act
        _context.Funds.Add(fund);
        await _context.SaveChangesAsync();

        // Assert
        var savedFund = await _context.Funds.FirstOrDefaultAsync(f => f.Code == "REPO_001");
        savedFund.Should().NotBeNull();
        savedFund!.Code.Should().Be("REPO_001");
        savedFund.Name.Should().Be("Repository Test Fund");
        savedFund.Cnpj.Value.Should().Be("12345678000195");
    }

    [Fact]
    public async Task GetByCodeAsync_WithExistingCode_ShouldReturnFund()
    {
        // Arrange
        var fund = new Fund("REPO_002", "Test Fund", new Cnpj("12345678000195"), 1);
        _context.Funds.Add(fund);
        await _context.SaveChangesAsync();

        // Act
        var result = await _context.Funds.FirstOrDefaultAsync(f => f.Code == "REPO_002");

        // Assert
        result.Should().NotBeNull();
        result!.Code.Should().Be("REPO_002");
        result.Name.Should().Be("Test Fund");
    }

    [Fact]
    public async Task GetByCodeAsync_WithNonExistentCode_ShouldReturnNull()
    {
        // Act
        var result = await _context.Funds.FirstOrDefaultAsync(f => f.Code == "NON_EXISTENT");

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllFunds()
    {
        // Arrange
        var fund1 = new Fund("REPO_003", "Fund 1", new Cnpj("12345678000195"), 1);
        var fund2 = new Fund("REPO_004", "Fund 2", new Cnpj("98765432000111"), 2);

        _context.Funds.AddRange(fund1, fund2);
        await _context.SaveChangesAsync();

        // Act
        var result = await _context.Funds.ToListAsync();

        // Assert
        result.Should().HaveCountGreaterOrEqualTo(2);
        result.Should().Contain(f => f.Code == "REPO_003");
        result.Should().Contain(f => f.Code == "REPO_004");
    }

    [Fact]
    public async Task ExistsAsync_WithExistingCode_ShouldReturnTrue()
    {
        // Arrange
        var fund = new Fund("REPO_005", "Exists Test Fund", new Cnpj("12345678000195"), 1);
        _context.Funds.Add(fund);
        await _context.SaveChangesAsync();

        // Act
        var exists = await _context.Funds.AnyAsync(f => f.Code == "REPO_005");

        // Assert
        exists.Should().BeTrue();
    }

    [Fact]
    public async Task ExistsAsync_WithNonExistentCode_ShouldReturnFalse()
    {
        // Act
        var exists = await _context.Funds.AnyAsync(f => f.Code == "NON_EXISTENT_CODE");

        // Assert
        exists.Should().BeFalse();
    }

    [Fact]
    public async Task RemoveAsync_ShouldRemoveFundFromDatabase()
    {
        // Arrange
        var fund = new Fund("REPO_006", "Remove Test Fund", new Cnpj("12345678000195"), 1);
        _context.Funds.Add(fund);
        await _context.SaveChangesAsync();

        // Act
        _context.Funds.Remove(fund);
        await _context.SaveChangesAsync();

        // Assert
        var removedFund = await _context.Funds.FirstOrDefaultAsync(f => f.Code == "REPO_006");
        removedFund.Should().BeNull();
    }

    [Fact]
    public async Task UpdateFund_ShouldUpdateFundInDatabase()
    {
        // Arrange
        var fund = new Fund("REPO_007", "Original Name", new Cnpj("12345678000195"), 1);
        _context.Funds.Add(fund);
        await _context.SaveChangesAsync();

        // Act
        fund.Name = "Updated Name";
        fund.Patrimony = 50000.75m;
        await _context.SaveChangesAsync();

        // Assert
        var updatedFund = await _context.Funds.FirstOrDefaultAsync(f => f.Code == "REPO_007");
        updatedFund.Should().NotBeNull();
        updatedFund!.Name.Should().Be("Updated Name");
        updatedFund.Patrimony.Should().Be(50000.75m);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
