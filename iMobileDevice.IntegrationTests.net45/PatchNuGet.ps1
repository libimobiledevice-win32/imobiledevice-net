$buildPropsPath = $env:SYSTEM_ARTIFACTSDIRECTORY + "\imobiledevice-net\Directory.Build.props"

[xml]$buildProps = Get-Content $buildPropsPath
$version = $buildProps.Project.PropertyGroup.MobileDeviceDotNetNuGetVersion
Write-Host "Current NuGet package version: $version"

$projectFile = Get-Content "iMobileDevice.IntegrationTests.net45.csproj"
$projectFile = $projectFile.Replace("{imobiledevice-version}", $version)
Set-Content "iMobileDevice.IntegrationTests.net45.csproj" $projectFile

$packagesFile = Get-Content "packages.config"
$packagesFile = $packagesFile.Replace("{imobiledevice-version}", $version)
Set-Content "packages.config" $packagesFile