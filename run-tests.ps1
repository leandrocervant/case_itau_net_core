# PowerShell script to run all tests with coverage
Write-Host "Building solution..." -ForegroundColor Green
dotnet build

if ($LASTEXITCODE -ne 0) {
    Write-Host "Build failed!" -ForegroundColor Red
    exit 1
}

Write-Host "Running Unit Tests..." -ForegroundColor Green
dotnet test tests/CaseItau.UnitTests/CaseItau.UnitTests.csproj --no-build --verbosity normal --collect:"XPlat Code Coverage"

if ($LASTEXITCODE -ne 0) {
    Write-Host "Unit tests failed!" -ForegroundColor Red
    exit 1
}

Write-Host "Running Integration Tests..." -ForegroundColor Green
dotnet test tests/CaseItau.IntegrationTests/CaseItau.IntegrationTests.csproj --no-build --verbosity normal

if ($LASTEXITCODE -ne 0) {
    Write-Host "Integration tests failed!" -ForegroundColor Red
    exit 1
}

Write-Host "All tests passed!" -ForegroundColor Green

# Run all tests together with coverage
Write-Host "Running all tests with coverage..." -ForegroundColor Green
dotnet test --collect:"XPlat Code Coverage" --results-directory ./TestResults

Write-Host "Test execution completed. Check TestResults folder for coverage reports." -ForegroundColor Green
