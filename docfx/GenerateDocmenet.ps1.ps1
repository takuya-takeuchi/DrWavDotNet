$Current = $PSScriptRoot

$Root = Split-Path $Current -Parent
$SourceRoot = Join-Path $Root src
$DrWavDotNetRoot = Join-Path $SourceRoot DrWavDotNet

docfx init -q -o docs
Set-Location $Current