# CaseItau - Test Documentation

This document describes the testing strategy and implementation for the CaseItau .NET Core application.

## Testing Strategy

The application uses a comprehensive testing approach with:

- **Unit Tests**: Testing individual components in isolation
- **Integration Tests**: Testing the interaction between components and external systems
- **Test Architecture**: Following Clean Architecture principles for maintainable tests

## Test Projects

### CaseItau.UnitTests

Contains unit tests for:

- **Domain Layer**
  - Entity validation and business rules
  - Value object validation (CNPJ)
  - Domain events
- **Application Layer**
  - Command/Query handlers
  - Business logic validation
- **API Layer**
  - Controller behavior
  - Request/Response mapping

### CaseItau.IntegrationTests

Contains integration tests for:

- **API Endpoints**
  - Full HTTP request/response cycle
  - End-to-end scenarios
- **Database Integration**
  - Repository patterns
  - Data persistence and retrieval
- **External Dependencies**
  - Database context interactions

## Test Patterns and Practices

### Builder Pattern
Test data creation using builder pattern for maintainable and readable test setup:

```csharp
var fund = FundBuilder.New()
    .WithCode("FUND001")
    .WithName("Test Fund")
    .WithCnpj("12345678000195")
    .Build();
```

### Test Doubles
Using Moq for mocking dependencies:
- Repository mocks
- Service mocks
- External dependency mocks

### Integration Test Factory
Custom `WebApplicationFactory` for integration tests:
- In-memory database setup
- Test environment configuration
- Test data seeding

## Running Tests

### Prerequisites
- .NET 8.0 SDK
- Visual Studio 2022 or VS Code

### Command Line

```powershell
# Run all tests
dotnet test

# Run unit tests only
dotnet test tests/CaseItau.UnitTests

# Run integration tests only
dotnet test tests/CaseItau.IntegrationTests

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"

# Use the provided script
./run-tests.ps1
```

### Visual Studio
1. Open Test Explorer (Test → Test Explorer)
2. Build solution
3. Run All Tests or select specific test categories

### Unit Tests Structure
```
CaseItau.UnitTests/
├── Domain/
│   ├── Entities/          # Entity tests
│   ├── ValueObjects/      # Value object tests
│   └── Events/           # Domain event tests
├── Application/
│   └── Funds/
│       ├── Commands/     # Command handler tests
│       └── Queries/      # Query handler tests
├── API/
│   └── Controllers/      # Controller tests
└── Common/
    └── Builders/        # Test data builders
```

### Integration Tests Structure
```
CaseItau.IntegrationTests/
├── API/
│   └── Controllers/      # API endpoint tests
├── Infrastructure/
│   └── Persistence/     # Database integration tests
└── Common/
    └── CaseItauWebApplicationFactory.cs
```

## Test Data Management

### Test Builders
- `FundBuilder`: Creates Fund entities with default or custom values
- `FundTypeBuilder`: Creates FundType entities

### Test Fixtures
- Database seeding for integration tests
- Shared test data across test classes
- Cleanup mechanisms

## Common Test Scenarios

### Domain Tests
- Entity creation and validation
- Business rule enforcement
- Domain event publishing

### Application Tests
- Command validation and execution
- Query result formatting
- Error handling and business logic

### Integration Tests
- HTTP endpoint functionality
- Database operations
- Full request/response cycles
- Error scenarios and edge cases
