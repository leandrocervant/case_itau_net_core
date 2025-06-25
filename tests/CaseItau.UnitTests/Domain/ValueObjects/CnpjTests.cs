using CaseItau.Domain.ValueObjects;
using FluentAssertions;
using Xunit;

namespace CaseItau.UnitTests.Domain.ValueObjects;

public class CnpjTests
{
    [Fact]
    public void Constructor_WithValidCnpj_ShouldCreateInstance()
    {
        // Arrange
        const string validCnpj = "12345678000195";

        // Act
        var cnpj = new Cnpj(validCnpj);

        // Assert
        cnpj.Value.Should().Be(validCnpj);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Constructor_WithNullOrEmptyCnpj_ShouldThrowArgumentException(string? invalidCnpj)
    {
        // Act & Assert
        var act = () => new Cnpj(invalidCnpj!);
        act.Should().Throw<ArgumentException>()
            .WithMessage("CNPJ cannot be null or empty. (Parameter 'Value')");
    }

    [Theory]
    [InlineData("123456789")] // Too short
    [InlineData("123456789001234")] // Too long
    [InlineData("1234567800019a")] // Contains letter
    [InlineData("12.345.678/0001-95")] // With formatting
    public void Constructor_WithInvalidFormat_ShouldThrowArgumentException(string invalidCnpj)
    {
        // Act & Assert
        var act = () => new Cnpj(invalidCnpj);
        act.Should().Throw<ArgumentException>()
            .WithMessage("CNPJ must be a 14-digit number. (Parameter 'Value')");
    }

    [Fact]
    public void Equals_WithSameCnpj_ShouldReturnTrue()
    {
        // Arrange
        const string cnpjValue = "12345678000195";
        var cnpj1 = new Cnpj(cnpjValue);
        var cnpj2 = new Cnpj(cnpjValue);

        // Act & Assert
        cnpj1.Should().Be(cnpj2);
        cnpj1.GetHashCode().Should().Be(cnpj2.GetHashCode());
    }

    [Fact]
    public void Equals_WithDifferentCnpj_ShouldReturnFalse()
    {
        // Arrange
        var cnpj1 = new Cnpj("12345678000195");
        var cnpj2 = new Cnpj("98765432000111");

        // Act & Assert
        cnpj1.Should().NotBe(cnpj2);
    }
}
