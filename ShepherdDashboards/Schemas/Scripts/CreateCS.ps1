﻿Write-Host "This file creates all the c# classes for PreliminaryData.xsd"
Get-Location | Write-host
$env:path += "; C:\Program Files\Microsoft SDKs\Windows\v7.1\Bin\x64\"
#$cpath = get-location

Write-host XSD processing
xsd.exe ..\PreliminaryDataRoute.xsd /N:CAS.SmartFactory.Shepherd.Dashboards.Schemas /c /o:.. |write-host

#set-location $cpath
Write-host Done...