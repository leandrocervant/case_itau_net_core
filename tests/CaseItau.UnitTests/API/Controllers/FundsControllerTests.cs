using CaseItau.API.Controllers;
using CaseItau.Application.Common.Errors;
using CaseItau.Application.Funds.Commands.CreateFund;
using CaseItau.Contracts.Funds;
using CaseItau.Domain.Entities;
using CaseItau.Domain.ValueObjects;
using ErrorOr;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CaseItau.UnitTests.API.Controllers;

public class FundsControllerTests
{
    private readonly Mock<ISender> _mediatorMock;
    private readonly FundsController _controller;

    public FundsControllerTests()
    {
        _mediatorMock = new Mock<ISender>();
        _controller = new FundsController(_mediatorMock.Object);
    }

    [Fact]
    public async Task CreateFund_WithValidRequest_ShouldReturnCreatedResult()
    {
        // Arrange
        var request = new CreateFundRequest("FUND001", "Test Fund", "12345678000195", 1);
        var fund = new Fund("FUND001", "Test Fund", new Cnpj("12345678000195"), 1);

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<CreateFundCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(fund);

        // Act
        var result = await _controller.CreateFund(request);

        // Assert
        result.Should().BeOfType<CreatedAtActionResult>();
        var createdResult = (CreatedAtActionResult)result;

        createdResult.ActionName.Should().Be(nameof(FundsController.GetFund));
        createdResult.RouteValues.Should().ContainKey("code");
        createdResult.RouteValues!["code"].Should().Be("FUND001");

        var response = createdResult.Value.Should().BeOfType<CreateFundResponse>().Subject;
        response.Code.Should().Be("FUND001");

        _mediatorMock.Verify(
            x => x.Send(
                It.Is<CreateFundCommand>(cmd =>
                    cmd.Code == request.Code &&
                    cmd.Name == request.Name &&
                    cmd.Cnpj == request.Cnpj &&
                    cmd.TypeId == request.TypeId),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task CreateFund_WithValidationError_ShouldReturnProblemResult()
    {
        // Arrange
        var request = new CreateFundRequest("FUND001", "Test Fund", "12345678000195", 1);
        var error = ApplicationErrors.FundAlreadyExists;

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<CreateFundCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(error);

        // Act
        var result = await _controller.CreateFund(request);

        // Assert
        result.Should().BeOfType<ObjectResult>();
        var objectResult = (ObjectResult)result;
        objectResult.Value.Should().BeOfType<ValidationProblemDetails>(); // Bad Request for validation errors
    }

    [Fact]
    public async Task CreateFund_WithNotFoundError_ShouldReturnNotFoundResult()
    {
        // Arrange
        var request = new CreateFundRequest("FUND001", "Test Fund", "12345678000195", 999);
        var error = ApplicationErrors.FundTypeNotFound;

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<CreateFundCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(error);

        // Act
        var result = await _controller.CreateFund(request);

        // Assert
        result.Should().BeOfType<ObjectResult>();
        var objectResult = (ObjectResult)result;
        objectResult.StatusCode.Should().Be(404); // Not Found
    }

    [Fact]
    public async Task CreateFund_WithUnexpectedError_ShouldReturnInternalServerError()
    {
        // Arrange
        var request = new CreateFundRequest("FUND001", "Test Fund", "12345678000195", 1);
        var error = Error.Failure("Unexpected.Error", "An unexpected error occurred");

        _mediatorMock
            .Setup(x => x.Send(It.IsAny<CreateFundCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(error);

        // Act
        var result = await _controller.CreateFund(request);

        // Assert
        result.Should().BeOfType<ObjectResult>();
        var objectResult = (ObjectResult)result;
        objectResult.StatusCode.Should().Be(500); // Internal Server Error
    }
}
