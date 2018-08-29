$version = (& $env:USERPROFILE/.nuget/packages/nerdbank.gitversioning/2.1.65/tools/Get-Version.ps1);

$projectFile = Get-Content "iMobileDevice.IntegrationTest.netcoreapp20.csproj"
$projectFile = $projectFile.Replace("{imobiledevice-version}", $version.NuGetPackageVersion)
Set-Content "iMobileDevice.IntegrationTest.netcoreapp20.csproj" $projectFile
