param(
    [string]$BuildPath = "D:\app_project\game\xiangsu\unity\Builds\Windows\StarryForest.exe",
    [string]$LogPath = "D:\app_project\game\xiangsu\unity\Logs\windows-player-smoke.log",
    [int]$Seconds = 10
)

$ErrorActionPreference = "Stop"

if (-not (Test-Path -LiteralPath $BuildPath)) {
    Write-Error "Windows build output not found: $BuildPath"
}

$logDirectory = Split-Path -Parent $LogPath
if (-not (Test-Path -LiteralPath $logDirectory)) {
    New-Item -ItemType Directory -Path $logDirectory | Out-Null
}

if (Test-Path -LiteralPath $LogPath) {
    Remove-Item -LiteralPath $LogPath
}

$process = Start-Process `
    -FilePath $BuildPath `
    -ArgumentList @("-batchmode", "-nographics", "-logFile", $LogPath) `
    -PassThru

Start-Sleep -Seconds $Seconds

try {
    $process.Refresh()
    if ($process.HasExited -and $process.ExitCode -ne 0) {
        Write-Error "Windows build smoke exited early with code $($process.ExitCode). See log: $LogPath"
    }
}
finally {
    $process.Refresh()
    if (-not $process.HasExited) {
        Stop-Process -Id $process.Id
    }
}

for ($attempt = 0; $attempt -lt 10 -and -not (Test-Path -LiteralPath $LogPath); $attempt++) {
    Start-Sleep -Seconds 1
}

if (-not (Test-Path -LiteralPath $LogPath)) {
    Write-Error "Windows build smoke log was not generated: $LogPath"
}

Write-Host "Windows build smoke launched successfully: $BuildPath"
