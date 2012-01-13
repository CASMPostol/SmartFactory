Write-Host "This file creates the schema file (xsd) for selected class"
Get-Location | Write-host
$env:path += "; C:\Program Files\Microsoft SDKs\Windows\v7.1\Bin\x64\"
$cpath = get-location
Write-host Start processing
set-location ..\bin\debug\
xsd.exe CAS.SmartFactoryDeployment.exe /t:CAS.SmartFactory.Deployment.InstallationStateData|write-host
set-location $cpath