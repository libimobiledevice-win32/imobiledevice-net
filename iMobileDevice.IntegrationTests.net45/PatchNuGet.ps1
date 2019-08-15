$version = (& $env:USERPROFILE/.nuget/packages/nerdbank.gitversioning/2.1.65/tools/Get-Version.ps1);

$projectFile = Get-Content "iMobileDevice.IntegrationTests.net45.csproj"
$projectFile = $projectFile.Replace("{imobiledevice-version}", $version.NuGetPackageVersion)
Set-Content "iMobileDevice.IntegrationTests.net45.csproj" $projectFile
Get-Content "iMobileDevice.IntegrationTests.net45.csproj"

$packagesFile = Get-Content "packages.config"
$packagesFile = $packagesFile.Replace("{imobiledevice-version}", $version.NuGetPackageVersion)
Set-Content "packages.config" $packagesFile
Get-Content "packages.config"