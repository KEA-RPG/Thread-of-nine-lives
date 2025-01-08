# Clean UnitTests and IntegrationTests TestResults folder before running tests. Might have old data, and we want to remove that
Write-Host "Cleaning UnitTests TestResults..."
Remove-Item -Recurse -Force UnitTests\TestResults\* 2>$null
Write-Host "Cleaning IntegrationTests TestResults..."
Remove-Item -Recurse -Force ThreadOfNineLives.IntegrationTests\TestResults\* 2>$null

# Run tests for UnitTests project
Write-Host "Running UnitTests..."
$resultUnitTests = dotnet test UnitTests/UnitTests.csproj --collect:"XPlat Code Coverage"

# Run tests for IntegrationTests project
Write-Host "Running IntegrationTests..."
$resultIntegrationTests = dotnet test ThreadOfNineLives.IntegrationTests/ThreadOfNineLives.IntegrationTests.csproj --collect:"XPlat Code Coverage"

# Generate the coverage report with exclusions for Domain.DTOs and Migrations
Write-Host "Generating coverage report with exclusions..."
reportgenerator -reports:"UnitTests\TestResults\*\coverage.cobertura.xml;ThreadOfNineLives.IntegrationTests\TestResults\*\coverage.cobertura.xml" -targetdir:"coveragereport" -reporttypes:Html -classfilters:"-Domain.DTOs.*;-*.Migrations.*"

# Open the generated coverage report
Write-Host "Opening coverage report..."
Start-Process "coveragereport\index.html"

Write-Host "Script execution completed."
