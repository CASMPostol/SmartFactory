Write-Host "This file creates all the c# classes for PreliminaryData.xsd"
Get-Location | Write-host
$env:path += "; C:\Program Files\Microsoft SDKs\Windows\v7.1\Bin\x64\"
#$cpath = get-location

Write-host XSD processing
xsd.exe ..\PreliminaryData.xsd /N:CAS.SmartFactory.Shepherd.ImportExport.XML /c  |write-host

#set-location $cpath
Write-host Done...