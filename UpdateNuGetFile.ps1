Param(
  [string]$build
)

Write-Host Changing build number to $build

# Update the build number
(gc .\iMobileDevice-net\imobiledevice-net.nuspec).replace('{build}', $build)|sc .\iMobileDevice-net\imobiledevice-net.out.nuspec
