Param()

# import class and function
$ScriptPath = $PSScriptRoot
$DrWavDotNet = Split-Path $ScriptPath -Parent
$NugetPath = Join-Path $DrWavDotNet "nuget" | `
             Join-Path -ChildPath "BuildUtils.ps1"
import-module $NugetPath -function *

$OperatingSystem="linux"
$Distribution="ubuntu"
$DistributionVersion="18"

# Store current directory
$Current = Get-Location
$DrWavDotNet = (Split-Path (Get-Location) -Parent)
$DrWavDotNetSourceRoot = Join-Path $DrWavDotNet src
$DockerDir = Join-Path $DrWavDotNet docker

Set-Location -Path $DockerDir

$DockerFileDir = Join-Path $DockerDir build  | `
                 Join-Path -ChildPath $Distribution | `
                 Join-Path -ChildPath $DistributionVersion

$BuildSourceHash = [Config]::GetBinaryLibraryLinuxHash()

# https://github.com/dotnet/coreclr/issues/9265
# linux-x86 does not support
$BuildTargets = @()
$BuildTargets += New-Object PSObject -Property @{ Platform = "desktop"; Target = "cpu";  Architecture = 64; Postfix = "/x64"; RID = "$OperatingSystem-x64";   CUDA = 0   }

foreach($BuildTarget in $BuildTargets)
{
   $platform = $BuildTarget.Platform
   $target = $BuildTarget.Target
   $architecture = $BuildTarget.Architecture
   $rid = $BuildTarget.RID
   $cudaVersion = $BuildTarget.CUDA
   $postfix = $BuildTarget.Postfix

   if ($target -ne "cuda")
   {
      $option = ""
      
      $dockername = "drwavdotnet/build/$Distribution/$DistributionVersion/$Target" + $postfix
      $imagename  = "drwavdotnet/devel/$Distribution/$DistributionVersion/$Target" + $postfix
   }
   else
   {
      $option = $cudaVersion

      $cudaVersion = ($cudaVersion / 10).ToString("0.0")
      $dockername = "drwavdotnet/build/$Distribution/$DistributionVersion/$Target/$cudaVersion"
      $imagename  = "drwavdotnet/devel/$Distribution/$DistributionVersion/$Target/$cudaVersion"
   }

   $Config = [Config]::new($DrWavDotNet, "Release", $target, $architecture, $platform, $option)
   $libraryDir = Join-Path "artifacts" $Config.GetArtifactDirectoryName()
   $build = $Config.GetBuildDirectoryName($OperatingSystem)

   Write-Host "Start 'docker build -t $dockername $DockerFileDir --build-arg IMAGE_NAME=""$imagename""'" -ForegroundColor Green
   docker build --network host --force-rm=true -t $dockername $DockerFileDir --build-arg IMAGE_NAME="$imagename"

   if ($lastexitcode -ne 0)
   {
      Set-Location -Path $Current
      exit -1
   }

   # Build binary
   foreach ($key in $BuildSourceHash.keys)
   {
      Write-Host "Start 'docker run --rm -v ""$($DrWavDotNet):/opt/data/DrWavDotNet"" -e LOCAL_UID=$(id -u $env:USER) -e LOCAL_GID=$(id -g $env:USER) -t $dockername'" -ForegroundColor Green
      docker run --rm --network host `
                  -v "$($DrWavDotNet):/opt/data/DrWavDotNet" `
                  -e "LOCAL_UID=$(id -u $env:USER)" `
                  -e "LOCAL_GID=$(id -g $env:USER)" `
                  -t "$dockername" $key $target $architecture $platform $option
   
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

      CopyToArtifact -srcDir $srcDir -build $build -libraryName $dll -dstDir $dstDir -rid $rid
   }
}

# Move to Root directory 
Set-Location -Path $Current
