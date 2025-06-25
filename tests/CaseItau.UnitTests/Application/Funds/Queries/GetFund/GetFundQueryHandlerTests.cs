using CaseItau.Application.Common.Errors;
using CaseItau.Application.Funds.Dto;
using CaseItau.Application.Funds.Queries;
using CaseItau.Application.Funds.Queries.GetFund;
using FluentAssertions;
using Moq;
using Xunit;

namespace CaseItau.UnitTests.Application.Funds.Queries.GetFund;

public class GetFundQueryHandlerTests
{
    private readonly Mock<IFundQueryProvider> _fundQueryProviderMock;
    private readonly GetFundQueryHandler _handler;

    public GetFundQueryHandlerTests()
    {
        _fundQueryProviderMock = new Mock<IFundQueryProvider>();
        _handler = new GetFundQueryHandler(_fundQueryProviderMock.Object);
    }

    [Fact]
    public async Task Handle_WithExistingFund_ShouldReturnFundDto()
    {
        // Arrange
        const string code = "FUND001";
        var query = new GetFundQuery(code);

        var expectedFund = new FundDto(
            code,
            "Test Fund",
            "12345678000195",
            1,
            "Equity Fund",
            50000.75m);

        _fundQueryProviderMock
            .Setup(x => x.GetFundAsync(code))
            .ReturnsAsync(expectedFund);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.Value.Code.Should().Be(code);
        result.Value.Name.Should().Be("Test Fund");
        result.Value.Cnpj.Should().Be("12345678000195");
        result.Value.TypeId.Should().Be(1);
        result.Value.TypeName.Should().Be("Equity Fund");
        result.Value.Patrimony.Should().Be(50000.75m);

        _fundQueryProviderMock.Verify(x => x.GetFundAsync(code), Times.Once);
    }

    [Fact]
    public async Task Handle_WithNonExistentFund_ShouldReturnFundNotFoundError()
    {
        // Arrange
        const string code = "NON_EXISTENT";
        var query = new GetFundQuery(code);

        _fundQueryProviderMock
            .Setup(x => x.GetFundAsync(code))
            .ReturnsAsync((FundDto?)null);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(ApplicationErrors.FundNotFound);

        _fundQueryProviderMock.Verify(x => x.GetFundAsync(code), Times.Once);
    }

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData(null)]
    public async Task Handle_WithInvalidCode_ShouldStillCallProvider(string? invalidCode)
    {
        // Arrange
        var query = new GetFundQuery(invalidCode!);

        _fundQueryProviderMock
            .Setup(x => x.GetFundAsync(invalidCode!))
            .ReturnsAsync((FundDto?)null);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(ApplicationErrors.FundNotFound);

        _fundQueryProviderMock.Verify(x => x.GetFundAsync(invalidCode!), Times.Once);
    }
}
