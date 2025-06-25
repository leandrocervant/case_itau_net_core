using CaseItau.Domain.Entities;
using CaseItau.Domain.Events.Fund;
using CaseItau.Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace CaseItau.UnitTests.Domain.Entities;

public class FundTests
{
    [Fact]
    public void Constructor_WithValidParameters_ShouldCreateFund()
    {
        // Arrange
        const string code = "FUND001";
        const string name = "Test Fund";
        var cnpj = new Cnpj("12345678000195");
        const long typeId = 1;

        // Act
        var fund = new Fund(code, name, cnpj, typeId);

        // Assert
        fund.Code.Should().Be(code);
        fund.Name.Should().Be(name);
        fund.Cnpj.Should().Be(cnpj);
        fund.TypeId.Should().Be(typeId);
        fund.Patrimony.Should().Be(0);
        fund.Id.Should().Be(0);
    }

    [Fact]
    public void Constructor_WhenCalled_ShouldRaiseFundCreatedEvent()
    {
        // Arrange
        const string code = "FUND001";
        const string name = "Test Fund";
        var cnpj = new Cnpj("12345678000195");
        const long typeId = 1;

        // Act
        var fund = new Fund(code, name, cnpj, typeId);

        // Assert
        var domainEvents = fund.PopEvents();
        domainEvents.Should().HaveCount(1);

        var fundCreatedEvent = domainEvents.First();
        fundCreatedEvent.Should().BeOfType<FundCreatedEvent>();

        var createdEvent = (FundCreatedEvent)fundCreatedEvent;
        createdEvent.Id.Should().Be(fund.Id);
        createdEvent.Code.Should().Be(code);
        createdEvent.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Constructor_WithNullCnpj_ShouldThrowException()
    {
        // Arrange
        const string code = "FUND001";
        const string name = "Test Fund";
        const long typeId = 1;

        // Act & Assert
        var act = () => new Fund(code, name, null!, typeId);
        act.Should().Throw<ArgumentNullException>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithInvalidCode_ShouldCreateFundWithInvalidCode(string? invalidCode)
    {
        // Arrange
        const string name = "Test Fund";
        var cnpj = new Cnpj("12345678000195");
        const long typeId = 1;

        // Act & Assert
        var act = () => new Fund(invalidCode!, name, cnpj, typeId);
        act.Should().Throw<ArgumentException>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Constructor_WithInvalidName_ShouldCreateFundWithInvalidName(string? invalidName)
    {
        // Arrange
        const string code = "FUND001";
        var cnpj = new Cnpj("12345678000195");
        const long typeId = 1;

        // Act & Assert
        var act = () => new Fund(code, invalidName!, cnpj, typeId);
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Patrimony_CanBeSetAndRetrieved()
    {
        // Arrange
        var fund = new Fund("FUND001", "Test Fund", new Cnpj("12345678000195"), 1);
        const decimal patrimonyValue = 1000000.50m;

        // Act
        fund.Patrimony = patrimonyValue;

        // Assert
        fund.Patrimony.Should().Be(patrimonyValue);
    }
}
