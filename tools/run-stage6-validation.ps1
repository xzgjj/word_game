$ErrorActionPreference = "Stop"

$scriptDirectory = Split-Path -Parent $MyInvocation.MyCommand.Path

& (Join-Path $scriptDirectory "run-unity-scene-builder.ps1")
Start-Sleep -Seconds 20
& (Join-Path $scriptDirectory "run-unity-editmode-tests.ps1")
Start-Sleep -Seconds 20
& (Join-Path $scriptDirectory "run-unity-playmode-tests.ps1")
Start-Sleep -Seconds 20
& (Join-Path $scriptDirectory "build-unity-windows.ps1")
Start-Sleep -Seconds 5
& (Join-Path $scriptDirectory "run-windows-build-smoke.ps1")

Write-Host "Stage 6 validation passed."
