# TailorApp — Azure App Service Deploy Script
# Usage: .\deploy.ps1 -AppName "your-azure-app-name"
# Requires: Node.js, .NET 10 SDK, Azure CLI (az)

param(
    [Parameter(Mandatory=$true)]
    [string]$AppName,

    [string]$ResourceGroup = "TailorAppRG"
)

$ErrorActionPreference = "Stop"

$FrontendDir = "$PSScriptRoot\Frontend\tailor-app-ui"
$BackendDir  = "$PSScriptRoot\Backend\TailorApp.API"
$PublishDir  = "$PSScriptRoot\_publish"
$WwwrootDir  = "$BackendDir\wwwroot"

Write-Host "`n=== Step 1: Build Angular frontend ===" -ForegroundColor Cyan
Set-Location $FrontendDir
npm ci
npm run build
# Angular 21 outputs to dist/tailor-app-ui/browser/
$AngularOut = "$FrontendDir\dist\tailor-app-ui\browser"

Write-Host "`n=== Step 2: Copy Angular output to .NET wwwroot ===" -ForegroundColor Cyan
if (Test-Path $WwwrootDir) { Remove-Item $WwwrootDir -Recurse -Force }
New-Item -ItemType Directory -Path $WwwrootDir | Out-Null
Copy-Item "$AngularOut\*" $WwwrootDir -Recurse
Write-Host "Copied Angular build to $WwwrootDir"

Write-Host "`n=== Step 3: Publish .NET API ===" -ForegroundColor Cyan
if (Test-Path $PublishDir) { Remove-Item $PublishDir -Recurse -Force }
Set-Location $BackendDir
dotnet publish -c Release -o $PublishDir

Write-Host "`n=== Step 4: Zip publish output ===" -ForegroundColor Cyan
$ZipPath = "$PSScriptRoot\_publish.zip"
if (Test-Path $ZipPath) { Remove-Item $ZipPath -Force }
Compress-Archive -Path "$PublishDir\*" -DestinationPath $ZipPath
Write-Host "Created $ZipPath"

Write-Host "`n=== Step 5: Deploy to Azure App Service ===" -ForegroundColor Cyan
az webapp deploy `
    --resource-group $ResourceGroup `
    --name $AppName `
    --src-path $ZipPath `
    --type zip

Write-Host "`n=== Done! ===" -ForegroundColor Green
Write-Host "App URL: https://$AppName.azurewebsites.net"
