$targets = @(
   "CPU"
)

$ScriptPath = $PSScriptRoot
$DrWavDotNet = Split-Path $ScriptPath -Parent

$source = Join-Path $DrWavDotNet src | `
          Join-Path -ChildPath DrWavDotNet
dotnet restore ${source}
dotnet build -c Release ${source}

foreach ($target in $targets)
{
   pwsh CreatePackage.ps1 $target
}