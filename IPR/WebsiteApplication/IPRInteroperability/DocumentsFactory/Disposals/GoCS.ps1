﻿Write-Host "This file creates all the c# classes for Configuration.xsd"
Get-Location | Write-host
$env:path += "; C:\Program Files\Microsoft SDKs\Windows\v7.1\Bin\x64\"
#$cpath = get-location

Write-host XSD processing
xsd.exe DocumentContent.xsd /N:CAS.SmartFactory.xml.DocumentsFactory.Disposals /c  |write-host

#set-location $cpath
Write-host Done...
