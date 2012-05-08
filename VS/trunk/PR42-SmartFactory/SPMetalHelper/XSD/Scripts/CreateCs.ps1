Write-Host "This file creates all the c# classes for xnl schemas"
Get-Location | Write-host
$env:path += "; C:\Program Files\Microsoft SDKs\Windows\v7.1\Bin\x64\"
$cpath = get-location
Write-host Processing
set-location ..\
xsd.exe ..\..\Schemas\SPMetalParameters.xsd /N:CAS.SmartFactory.SPMetalHelper.XSD /c  |write-host
set-location $cpath
Write-host Done...
