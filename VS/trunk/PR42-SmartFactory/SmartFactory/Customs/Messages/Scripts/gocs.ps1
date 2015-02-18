Write-Host "This file creates all the c# classes for xnl schemas"
Get-Location | Write-host
$env:path += "; C:\Program Files (x86)\Microsoft SDKs\Windows\v8.0A\bin\NETFX 4.0 Tools"
# Set-Location C:\vs\Projects\JTI\Excel2XML
# Write-host press any key to continue .....
# Read-host
$cpath = get-location
# Write-host CELINA processing
set-location ..\CELINA
#xsd.exe SADw2r0.xsd xmldsig-core-schema.xsd /N:CAS.SmartFactory.Customs.Messages.CELINA.SAD /c  |write-host
#xsd.exe CLNEw2r0.xsd xmldsig-core-schema.xsd /N:CAS.SmartFactory.Customs.Messages.CELINA.CLNE /c  |write-host
#xsd.exe PZCw2r0.xsd xmldsig-core-schema.xsd /N:CAS.SmartFactory.Customs.Messages.CELINA.PZC /c  |write-host
# xsd.exe DSw2r0.xsd  xmldsig-core-schema.xsd /N:CAS.SmartFactory.Customs.Messages.CELINA /c  |write-host
# xsd.exe PWDw2r0.xsd xmldsig-core-schema.xsd /N:CAS.SmartFactory.Customs.Messages.CELINA /c  |write-host
xsd.exe SADCollection.xsd /N:CAS.SmartFactory.Customs.Messages.CELINA.SAD.SADCollection /c  |write-host

#Write-host ECS processing
#set-location ..\ECS
#xsd.exe IE529_v1-0.xsd xmldsig-core-schema.xsd /N:CAS.SmartFactory.Customs.Messages.ECS /c  |write-host

set-location $cpath
Write-host Done...
# Read-Host
