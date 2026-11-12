#!/usr/bin/env pwsh
# Rebuilds windows-notifier into .\dist as a single self-contained exe.
# Usage:  pwsh ./build.ps1

$ErrorActionPreference = 'Stop'
Set-Location -Path $PSScriptRoot

dotnet publish -c Release -r win-x64 --self-contained true `
    -p:PublishSingleFile=true `
    -p:EnableCompressionInSingleFile=true `
    -o dist
    # Add -p:PublishTrimmed=true to shrink further (~25 MB), but SMOKE-TEST the
    # exe afterward: trimming can strip the WinRT toast code reached by reflection.

if ($LASTEXITCODE -ne 0) { throw "dotnet publish failed (exit $LASTEXITCODE)" }

$exe = Join-Path $PSScriptRoot 'dist\windows-notifier.exe'
$size = [math]::Round((Get-Item $exe).Length / 1MB, 1)
Write-Host "Built $exe ($size MB)" -ForegroundColor Green
