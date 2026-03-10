# Test Quantity Measurement Web API
# Compatible with Windows PowerShell 5.1

# Ignore SSL certificate errors
add-type @"
    using System.Net;
    using System.Security.Cryptography.X509Certificates;
    public class TrustAllCertsPolicy : ICertificatePolicy {
        public bool CheckValidationResult(
            ServicePoint srvPoint, X509Certificate certificate,
            WebRequest request, int certificateProblem) {
            return true;
        }
    }
"@
[System.Net.ServicePointManager]::CertificatePolicy = New-Object TrustAllCertsPolicy
[System.Net.ServicePointManager]::SecurityProtocol = [System.Net.SecurityProtocolType]::Tls12

$baseUrl = "http://localhost:5132/api/quantitymeasurement"

Write-Host ""
Write-Host "=== Testing Quantity Measurement Web API ===" -ForegroundColor Yellow
Write-Host "Base URL: $baseUrl" -ForegroundColor Gray
Write-Host ""

# Test 1: Compare
Write-Host "[1/5] Testing Compare..." -ForegroundColor Green
$body = @{
    quantity1 = @{ value = 12; unit = "INCH"; measurementType = "LENGTH" }
    quantity2 = @{ value = 1; unit = "FEET"; measurementType = "LENGTH" }
} | ConvertTo-Json

try {
    $result = Invoke-RestMethod -Uri "$baseUrl/compare" -Method POST -Body $body -ContentType "application/json"
    Write-Host "  SUCCESS: 12 INCH = 1 FEET? $($result.equal)" -ForegroundColor Cyan
} catch {
    Write-Host "  FAILED: $($_.Exception.Message)" -ForegroundColor Red
}

# Test 2: Convert
Write-Host ""
Write-Host "[2/5] Testing Convert..." -ForegroundColor Green
$body = @{
    quantity = @{ value = 1; unit = "FEET"; measurementType = "LENGTH" }
    targetUnit = "INCH"
} | ConvertTo-Json

try {
    $result = Invoke-RestMethod -Uri "$baseUrl/convert" -Method POST -Body $body -ContentType "application/json"
    Write-Host "  SUCCESS: 1 FEET = $($result.value) $($result.unit)" -ForegroundColor Cyan
} catch {
    Write-Host "  FAILED: $($_.Exception.Message)" -ForegroundColor Red
}

# Test 3: Add
Write-Host ""
Write-Host "[3/5] Testing Add..." -ForegroundColor Green
$body = @{
    quantity1 = @{ value = 2; unit = "INCH"; measurementType = "LENGTH" }
    quantity2 = @{ value = 2; unit = "INCH"; measurementType = "LENGTH" }
} | ConvertTo-Json

try {
    $result = Invoke-RestMethod -Uri "$baseUrl/add" -Method POST -Body $body -ContentType "application/json"
    Write-Host "  SUCCESS: 2 INCH + 2 INCH = $($result.value) $($result.unit)" -ForegroundColor Cyan
} catch {
    Write-Host "  FAILED: $($_.Exception.Message)" -ForegroundColor Red
}

# Test 4: Subtract
Write-Host ""
Write-Host "[4/5] Testing Subtract..." -ForegroundColor Green
$body = @{
    quantity1 = @{ value = 5; unit = "FEET"; measurementType = "LENGTH" }
    quantity2 = @{ value = 2; unit = "FEET"; measurementType = "LENGTH" }
} | ConvertTo-Json

try {
    $result = Invoke-RestMethod -Uri "$baseUrl/subtract" -Method POST -Body $body -ContentType "application/json"
    Write-Host "  SUCCESS: 5 FEET - 2 FEET = $($result.value) $($result.unit)" -ForegroundColor Cyan
} catch {
    Write-Host "  FAILED: $($_.Exception.Message)" -ForegroundColor Red
}

# Test 5: Divide
Write-Host ""
Write-Host "[5/5] Testing Divide..." -ForegroundColor Green
$body = @{
    quantity1 = @{ value = 10; unit = "FEET"; measurementType = "LENGTH" }
    quantity2 = @{ value = 2; unit = "FEET"; measurementType = "LENGTH" }
} | ConvertTo-Json

try {
    $result = Invoke-RestMethod -Uri "$baseUrl/divide" -Method POST -Body $body -ContentType "application/json"
    Write-Host "  SUCCESS: 10 FEET / 2 FEET = $($result.value)" -ForegroundColor Cyan
} catch {
    Write-Host "  FAILED: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host ""
Write-Host "=== All Tests Complete ===" -ForegroundColor Yellow
Write-Host ""
