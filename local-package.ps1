param(
    [Parameter(Mandatory=$true)]
    [string]$Mode,
    [Parameter(Mandatory=$true)]
    [string]$LocalRepoPath,
    [Parameter(Mandatory=$false)]
    [string]$MajorVersion = "1.1.0"
)

$ErrorActionPreference = 'Stop'

if ($Mode -eq "add") {
    $configPath = "$LocalRepoPath\.rocketcress-config.json"
    if (Test-Path $configPath -PathType Leaf) {
        $config = Get-Content $configPath -Raw | ConvertFrom-Json
    }
    else {
        $config = @{ lastLocalVersion = 0 }
    }

    Set-Location $PSScriptRoot
    $config.lastLocalVersion++
    dotnet pack "-p:Version=$MajorVersion-local$($config.lastLocalVersion.ToString("000"))" -c Release --force --include-source
    ConvertTo-Json $config -Depth 99 | Set-Content $configPath

    Get-ChildItem "bin\Release\Rocketcress.*.$MajorVersion-local$($config.lastLocalVersion.ToString("000")).symbols.nupkg" | ForEach-Object { nuget add $_ -source $LocalRepoPath }

    Write-Host "Successfully add local packages" -ForegroundColor Green
    Write-Host "Run `"dotnet restore --force`" for the project that should use the local package"
}
elseif ($Mode -eq "cleanup") {
    Get-ChildItem "$LocalRepoPath\rocketcress*" | ForEach-Object { Remove-Item $_ -Force -Recurse }
    Get-ChildItem "$env:USERPROFILE\.nuget\packages\rocketcress*\*-local*" | ForEach-Object { Remove-Item $_ -Force -Recurse }
    Write-Host "Local Rocketcress packages were removed" -ForegroundColor Green
}
else {
    Write-Host "The mode `"$Mode`" is unknwon. You can use `"add`" or `"cleanup`"" -ForegroundColor Red
}