Param()

# import class and function
$ScriptPath = $PSScriptRoot
$DrWavDotNet = Split-Path $ScriptPath -Parent
$NugetPath = Join-Path $DrWavDotNet "nuget" | `
             Join-Path -ChildPath "BuildUtils.ps1"
import-module $NugetPath -function *

$OperatingSystem="win"

# Store current directory
$Current = Get-Location
$DrWavDotNet = (Split-Path (Get-Location) -Parent)
$DrWavDotNetSourceRoot = Join-Path $DrWavDotNet src

$BuildSourceHash = [Config]::GetBinaryLibraryWindowsHash()

$BuildTargets = @()
$BuildTargets += New-Object PSObject -Property @{Target = "cpu";  Architecture = 64; RID = "$OperatingSystem-x64";   CUDA = 0   }
$BuildTargets += New-Object PSObject -Property @{Target = "cpu";  Architecture = 32; RID = "$OperatingSystem-x86";   CUDA = 0   }

foreach ($BuildTarget in $BuildTargets)
{
   $target = $BuildTarget.Target
   $architecture = $BuildTarget.Architecture
   $rid = $BuildTarget.RID

   if ($target -eq "cpu")
   {
      $option = ""
   }

   $Config = [Config]::new($DrWavDotNet, "Release", $target, $architecture, "desktop", $option)
   $libraryDir = Join-Path "artifacts" $Config.GetArtifactDirectoryName()
   $build = $Config.GetBuildDirectoryName($OperatingSystem)

   foreach ($key in $BuildSourceHash.keys)
   {
      $srcDir = Join-Path $DrWavDotNetSourceRoot $key

      # Move to build target directory
      Set-Location -Path $srcDir

      $arc = $Config.GetArchitectureName()
      Write-Host "Build $key [$arc] for $target" -ForegroundColor Green
      Build -Config $Config

      if ($lastexitcode -ne 0)
      {
         Set-Location -Path $Current
         exit -1
      }
   }

   # Copy output binary
   foreach ($key in $BuildSourceHash.keys)
   {
      $srcDir = Join-Path $DrWavDotNetSourceRoot $key
      $dll = $BuildSourceHash[$key]
      $dstDir = Join-Path $Current $libraryDir

      CopyToArtifact -configuration "Release" -srcDir $srcDir -build $build -libraryName $dll -dstDir $dstDir -rid $rid
   }
}

# Move to Root directory 
Set-Location -Path $Current