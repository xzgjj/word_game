param(
    [string]$UnityExe = "D:\unity\Editor\Unity.exe",
    [string]$ProjectPath = "D:\app_project\game\xiangsu\unity",
    [string]$LogPath = "D:\app_project\game\xiangsu\unity\Logs\windows-build.log",
    [string]$BuildPath = "D:\app_project\game\xiangsu\unity\Builds\Windows\StarryForest.exe"
)

$ErrorActionPreference = "Stop"

if (-not (Test-Path -LiteralPath $UnityExe)) {
    Write-Error "Unity executable not found: $UnityExe"
}

if (-not (Test-Path -LiteralPath $ProjectPath)) {
    Write-Error "Unity project path not found: $ProjectPath"
}

$logDirectory = Split-Path -Parent $LogPath
if (-not (Test-Path -LiteralPath $logDirectory)) {
    New-Item -ItemType Directory -Path $logDirectory | Out-Null
}

if (Test-Path -LiteralPath $LogPath) {
    Remove-Item -LiteralPath $LogPath
}

& $UnityExe `
    -batchmode `
    -projectPath $ProjectPath `
    -executeMethod StarryForest.EditorTools.BuildAutomation.BuildWindows `
    -logFile $LogPath `
    -quit

$unityExitCode = $LASTEXITCODE
if ($unityExitCode -ne 0) {
    Write-Error "Unity Windows build exited with code $unityExitCode. See log: $LogPath"
}

$deadline = (Get-Date).AddMinutes(10)
$buildReady = $false
while ((Get-Date) -lt $deadline) {
    $buildExists = Test-Path -LiteralPath $BuildPath
    $logReportsSuccess = $false
    if (Test-Path -LiteralPath $LogPath) {
        $logReportsSuccess = [bool](Select-String -LiteralPath $LogPath -Pattern "Windows build succeeded|Build Finished, Result: Success" -Quiet)
        if (Select-String -LiteralPath $LogPath -Pattern "Scripts have compiler errors|Build Finished, Result: Failed|Windows build failed" -Quiet) {
            Write-Error "Unity Windows build failed. See log: $LogPath"
        }
    }

    if ($buildExists -and $logReportsSuccess) {
        $buildReady = $true
        break
    }

    Start-Sleep -Seconds 1
}

if (-not $buildReady) {
    Write-Error "Windows build output was not generated or success was not logged: $BuildPath"
}

Write-Host "Unity Windows build generated: $BuildPath"
