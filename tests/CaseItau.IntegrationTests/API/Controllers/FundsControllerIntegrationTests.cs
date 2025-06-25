using CaseItau.Contracts.Funds;
using CaseItau.IntegrationTests.Common;
using FluentAssertions;
using System.Net;
using Xunit;

namespace CaseItau.IntegrationTests.API.Controllers;

public class FundsControllerIntegrationTests : IClassFixture<CaseItauWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CaseItauWebApplicationFactory _factory;

    public FundsControllerIntegrationTests(CaseItauWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateFund_WithValidData_ShouldReturnCreated()
    {
        // Arrange
        var request = new CreateFundRequest(
            "FUND001",
            "Integration Test Fund",
            "12345678000195",
            1);

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/funds", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var content = await response.Content.ReadFromJsonAsync<CreateFundResponse>();
        content.Should().NotBeNull();
        content!.Code.Should().Be("FUND001");

        response.Headers.Location.Should().NotBeNull();
        response.Headers.Location!.ToString().Should().ContainEquivalentOf("/api/v1/funds/FUND001");
    }

    [Fact]
    public async Task CreateFund_WithDuplicateCode_ShouldReturnBadRequest()
    {
        // Arrange
        var request1 = new CreateFundRequest(
            "FUND002",
            "First Fund",
            "12345678000195",
            1);

        var request2 = new CreateFundRequest(
            "FUND002",
            "Second Fund",
            "98765432000111",
            2);

        // Act
        var response1 = await _client.PostAsJsonAsync("/api/v1/funds", request1);
        var response2 = await _client.PostAsJsonAsync("/api/v1/funds", request2);

        // Assert
        response1.StatusCode.Should().Be(HttpStatusCode.Created);
        response2.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task CreateFund_WithInvalidCnpj_ShouldReturnBadRequest()
    {
        // Arrange
        var request = new CreateFundRequest(
            "FUND003",
            "Test Fund",
            "invalid-cnpj",
            1);

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/funds", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }

    [Fact]
    public async Task CreateFund_WithNonExistentFundType_ShouldReturnNotFound()
    {
        // Arrange
        var request = new CreateFundRequest(
            "FUND004",
            "Test Fund",
            "12345678000195",
            999); // Non-existent fund type

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/funds", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Theory]
    [InlineData("", "Valid Name", "12345678000195", 1)]
    [InlineData("FUND005", "", "12345678000195", 1)]
    [InlineData("FUND005", "Valid Name", "", 1)]
    public async Task CreateFund_WithMissingRequiredFields_ShouldReturnBadRequest(
        string code,
        string name,
        string cnpj,
        long typeId)
    {
        // Arrange
        var request = new CreateFundRequest(code, name, cnpj, typeId);

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/funds", request);

        // Assert
        response.StatusCode.Should().BeOneOf(HttpStatusCode.BadRequest, HttpStatusCode.InternalServerError);
    }

    [Fact]
    public async Task GetFund_WithExistingCode_ShouldReturnFund()
    {
        // Arrange - First create a fund
        var createRequest = new CreateFundRequest(
            "FUND_GET_001",
            "Fund for Get Test",
            "12345678000195",
            1);

        await _client.PostAsJsonAsync("/api/v1/funds", createRequest);

        // Act
        var response = await _client.GetAsync("/api/v1/funds/FUND_GET_001");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<FundResponse>();
        content.Should().NotBeNull();
        content!.Code.Should().Be("FUND_GET_001");
        content.Name.Should().Be("Fund for Get Test");
        content.Cnpj.Should().Be("12345678000195");
        content.TypeId.Should().Be(1);
    }

    [Fact]
    public async Task GetFund_WithNonExistentCode_ShouldReturnNotFound()
    {
        // Act
        var response = await _client.GetAsync("/api/v1/funds/NON_EXISTENT");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ListFunds_ShouldReturnAllFunds()
    {
        // Arrange - Create multiple funds
        var fund1 = new CreateFundRequest("LIST_001", "Fund 1", "12345678000195", 1);
        var fund2 = new CreateFundRequest("LIST_002", "Fund 2", "98765432000111", 2);

        await _client.PostAsJsonAsync("/api/v1/funds", fund1);
        await _client.PostAsJsonAsync("/api/v1/funds", fund2);

        // Act
        var response = await _client.GetAsync("/api/v1/funds");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<List<FundResponse>>();
        content.Should().NotBeNull();
        content!.Should().HaveCountGreaterOrEqualTo(2);
        content.Should().Contain(f => f.Code == "LIST_001");
        content.Should().Contain(f => f.Code == "LIST_002");
    }
}
