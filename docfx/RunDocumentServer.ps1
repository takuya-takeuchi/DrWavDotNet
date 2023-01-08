$Current = $PSScriptRoot

$Root = Split-Path $Current -Parent
$SourceRoot = Join-Path $Root src
$DrWavDotNetRoot = Join-Path $SourceRoot DrWavDotNet
$DocumentDir = Join-Path $DrWavDotNetRoot docfx
$Json = Join-Path $Current docfx.json

docfx "${Json}" --serve
Set-Location $Current