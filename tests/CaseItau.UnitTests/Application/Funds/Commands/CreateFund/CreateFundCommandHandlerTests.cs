using CaseItau.Application.Common.Errors;
using CaseItau.Application.Funds.Commands.CreateFund;
using CaseItau.Domain.Common;
using CaseItau.Domain.Common.Interfaces;
using CaseItau.Domain.Common.Resouces;
using CaseItau.Domain.Entities;
using CaseItau.Domain.Repositories;
using CaseItau.Domain.ValueObjects;
using FluentAssertions;
using Moq;
using Xunit;

namespace CaseItau.UnitTests.Application.Funds.Commands.CreateFund;

public class CreateFundCommandHandlerTests
{
    private readonly Mock<IFundTypeRepository> _fundTypeRepositoryMock;
    private readonly Mock<IFundRepository> _fundRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly CreateFundCommandHandler _handler;

    public CreateFundCommandHandlerTests()
    {
        _fundTypeRepositoryMock = new Mock<IFundTypeRepository>();
        _fundRepositoryMock = new Mock<IFundRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _handler = new CreateFundCommandHandler(
            _fundTypeRepositoryMock.Object,
            _fundRepositoryMock.Object,
            _unitOfWorkMock.Object);
    }

    [Fact]
    public async Task Handle_WithValidRequest_ShouldCreateFund()
    {
        // Arrange
        var command = new CreateFundCommand("FUND001", "Test Fund", "12345678000195", 1);
        var fundType = new FundType("Equity Fund");

        _fundTypeRepositoryMock
            .Setup(x => x.GetByIdAsync(command.TypeId))
            .ReturnsAsync(fundType);

        _fundRepositoryMock
            .Setup(x => x.GetByCodeAsync(command.Code))
            .ReturnsAsync((Fund?)null);

        _fundRepositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Fund>()))
            .ReturnsAsync(1);

        _unitOfWorkMock
            .Setup(x => x.CommitChangesAsync())
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeFalse();
        result.Value.Should().NotBeNull();
        result.Value.Code.Should().Be(command.Code);
        result.Value.Name.Should().Be(command.Name);
        result.Value.TypeId.Should().Be(command.TypeId);
        result.Value.Cnpj.Value.Should().Be(command.Cnpj);

        _fundTypeRepositoryMock.Verify(x => x.GetByIdAsync(command.TypeId), Times.Once);
        _fundRepositoryMock.Verify(x => x.GetByCodeAsync(command.Code), Times.Once);
        _fundRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Fund>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.CommitChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task Handle_WithNonExistentFundType_ShouldReturnFundTypeNotFoundError()
    {
        // Arrange
        var command = new CreateFundCommand("FUND001", "Test Fund", "12345678000195", 999);

        _fundTypeRepositoryMock
            .Setup(x => x.GetByIdAsync(command.TypeId))
            .ReturnsAsync((FundType?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(ApplicationErrors.FundTypeNotFound);

        _fundTypeRepositoryMock.Verify(x => x.GetByIdAsync(command.TypeId), Times.Once);
        _fundRepositoryMock.Verify(x => x.GetByCodeAsync(It.IsAny<string>()), Times.Never);
        _fundRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Fund>()), Times.Never);
        _unitOfWorkMock.Verify(x => x.CommitChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_WithExistingFundCode_ShouldReturnFundAlreadyExistsError()
    {
        // Arrange
        var command = new CreateFundCommand("FUND001", "Test Fund", "12345678000195", 1);
        var fundType = new FundType("Equity Fund");
        var existingFund = new Fund("FUND001", "Existing Fund", new Cnpj("98765432000111"), 1);

        _fundTypeRepositoryMock
            .Setup(x => x.GetByIdAsync(command.TypeId))
            .ReturnsAsync(fundType);

        _fundRepositoryMock
            .Setup(x => x.GetByCodeAsync(command.Code))
            .ReturnsAsync(existingFund);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsError.Should().BeTrue();
        result.FirstError.Should().Be(ApplicationErrors.FundAlreadyExists);

        _fundTypeRepositoryMock.Verify(x => x.GetByIdAsync(command.TypeId), Times.Once);
        _fundRepositoryMock.Verify(x => x.GetByCodeAsync(command.Code), Times.Once);
        _fundRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Fund>()), Times.Never);
        _unitOfWorkMock.Verify(x => x.CommitChangesAsync(), Times.Never);
    }

    [Theory]
    [InlineData("", "Valid Name", "123456780001951")]
    [InlineData(null, "Valid Name", "123456780001951")]
    [InlineData("FUND001", "", "123456780001951")]
    [InlineData("FUND001", null, "123456780001951")]
    public async Task Handle_WithInvalidCnpj_ShouldThrowDomainException(string? code, string? name, string invalidCnpj)
    {
        // Arrange
        var command = new CreateFundCommand(code!, name!, invalidCnpj, 1);
        var fundType = new FundType("Equity Fund");

        _fundTypeRepositoryMock
            .Setup(x => x.GetByIdAsync(command.TypeId))
            .ReturnsAsync(fundType);

        _fundRepositoryMock
            .Setup(x => x.GetByCodeAsync(command.Code))
            .ReturnsAsync((Fund?)null);

        // Act & Assert
        var act = async () => await _handler.Handle(command, CancellationToken.None);
        await act.Should().ThrowAsync<DomainException>();
    }

    [Fact]
    public async Task Handle_WithInvalidCnpjFormat_ShouldThrowDomainException()
    {
        // Arrange
        var command = new CreateFundCommand("FUND001", "Test Fund", "invalid-cnpj", 1);
        var fundType = new FundType("Equity Fund");

        _fundTypeRepositoryMock
            .Setup(x => x.GetByIdAsync(command.TypeId))
            .ReturnsAsync(fundType);

        _fundRepositoryMock
            .Setup(x => x.GetByCodeAsync(command.Code))
            .ReturnsAsync((Fund?)null);

        // Act & Assert
        var act = async () => await _handler.Handle(command, CancellationToken.None);
        await act.Should().ThrowAsync<DomainException>()
            .WithMessage(Errors.Cnpj_ValueMustBe14Digit);
    }
}
