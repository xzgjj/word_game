param(
    [string]$UnityExe = "D:\unity\Editor\Unity.exe",
    [string]$ProjectPath = "D:\app_project\game\xiangsu\unity",
    [string]$ResultsPath = "D:\app_project\game\xiangsu\unity\Logs\editmode-test-results.xml",
    [string]$LogPath = "D:\app_project\game\xiangsu\unity\Logs\editmode-test.log"
)

$ErrorActionPreference = "Stop"

if (-not (Test-Path -LiteralPath $UnityExe)) {
    Write-Error "Unity executable not found: $UnityExe"
}

if (-not (Test-Path -LiteralPath $ProjectPath)) {
    Write-Error "Unity project path not found: $ProjectPath"
}

$resultsDirectory = Split-Path -Parent $ResultsPath
if (-not (Test-Path -LiteralPath $resultsDirectory)) {
    New-Item -ItemType Directory -Path $resultsDirectory | Out-Null
}

if (Test-Path -LiteralPath $ResultsPath) {
    Remove-Item -LiteralPath $ResultsPath
}

& $UnityExe `
    -batchmode `
    -projectPath $ProjectPath `
    -runTests `
    -testPlatform editmode `
    -testResults $ResultsPath `
    -logFile $LogPath

$unityExitCode = $LASTEXITCODE
if ($unityExitCode -ne 0) {
    Write-Error "Unity EditMode tests exited with code $unityExitCode. See log: $LogPath"
}

for ($attempt = 0; $attempt -lt 20 -and -not (Test-Path -LiteralPath $ResultsPath); $attempt++) {
    Start-Sleep -Seconds 1
}

if (-not (Test-Path -LiteralPath $ResultsPath)) {
    Write-Error "Unity EditMode test results were not generated. Do not add -quit to this command; Test Runner exits on completion."
}

[xml]$results = Get-Content -Raw -LiteralPath $ResultsPath
$testRun = $results."test-run"
$total = [int]$testRun.total
$failed = [int]$testRun.failed
$passed = [int]$testRun.passed

Write-Host "Unity EditMode tests: $passed/$total passed, $failed failed."

if ($failed -ne 0) {
    exit 1
}
