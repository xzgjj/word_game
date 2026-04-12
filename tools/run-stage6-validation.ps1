$ErrorActionPreference = "Stop"

$scriptDirectory = Split-Path -Parent $MyInvocation.MyCommand.Path

& (Join-Path $scriptDirectory "run-unity-editmode-tests.ps1")
& (Join-Path $scriptDirectory "run-unity-playmode-tests.ps1")
& (Join-Path $scriptDirectory "build-unity-windows.ps1")
& (Join-Path $scriptDirectory "run-windows-build-smoke.ps1")

Write-Host "Stage 6 validation passed."
