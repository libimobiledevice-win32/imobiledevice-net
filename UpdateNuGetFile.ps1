Param(
  [string]$build
)

Write-Host Changing build number to $build

# Update the build number
(gc .\iMobileDevice\imobiledevice-net.nuspec).replace('{build}', $build)|sc .\iMobileDevice\imobiledevice-net.out.nuspec
