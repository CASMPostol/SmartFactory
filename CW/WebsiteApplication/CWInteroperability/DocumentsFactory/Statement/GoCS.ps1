Write-Host "This file creates all the c# classes for Configuration.xsd"
Get-Location | Write-host
$env:path += "; C:\Program Files (x86)\Microsoft SDKs\Windows\v8.0A\bin\NETFX 4.0 Tools"
#$cpath = get-location

Write-host XSD processing
xsd.exe Statement.xsd /N:CAS.SmartFactory.CW.Interoperability.DocumentsFactory.Statement /c  |write-host

#set-location $cpath
Write-host Done...
