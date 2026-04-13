param(
    [string]$UnityExe = "D:\unity\Editor\Unity.exe",
    [string]$ProjectPath = "D:\app_project\game\xiangsu\unity",
    [string]$LogPath = "D:\app_project\game\xiangsu\unity\Logs\scene-builder.log"
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
    -executeMethod StarryForest.EditorTools.WorldHubSceneBuilder.BuildVisualPlayableScenes `
    -logFile $LogPath `
    -quit

$unityExitCode = $LASTEXITCODE
if ($unityExitCode -ne 0) {
    Write-Error "Unity scene builder exited with code $unityExitCode. See log: $LogPath"
}

Write-Host "Unity visual playable scenes rebuilt."
