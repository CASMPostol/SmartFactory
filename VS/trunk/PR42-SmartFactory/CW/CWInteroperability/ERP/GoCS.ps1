Write-Host "This file creates all the c# classes for ERP namespace"
$env:path += "; C:\Program Files (x86)\Microsoft SDKs\Windows\v8.0A\bin\NETFX 4.0 Tools"
Write-Host 
xsd.exe DisposalRequest.xsd /N:CAS.SmartFactory.CW.Interoperability.ERP /c  |write-host
Write-host Done...