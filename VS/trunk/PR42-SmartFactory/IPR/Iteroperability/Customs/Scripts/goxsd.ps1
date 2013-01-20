Write-Host "This file creates all the c# classes for xnl schemas"
Get-Location | Write-host
$env:path += "; C:\Program Files\Microsoft SDKs\Windows\v7.1\Bin\x64\"
# Set-Location C:\vs\Projects\JTI\Excel2XML
# Write-host press any key to continue .....
# Read-host
$cpath = get-location
# Write-host CELINA processing
# set-location ..\CELINA
# xsd.exe SADw2r0.xsd xmldsig-core-schema.xsd /N:CAS.SmartFactory.xml.Customs.SAD /c  |write-host
# xsd.exe CLNEw2r0.xsd xmldsig-core-schema.xsd /N:CAS.SmartFactory.xml.Customs.CLNE /c  |write-host
# xsd.exe PZCw2r0.xsd xmldsig-core-schema.xsd /N:CAS.SmartFactory.xml.Customs.PZC /c  |write-host
# xsd.exe DSw2r0.xsd  xmldsig-core-schema.xsd /N:CAS.SmartFactory.xml.Customs.DS /c  |write-host
# xsd.exe PWDw2r0.xsd xmldsig-core-schema.xsd /N:CAS.SmartFactory.xml.Customs.PWD /c  |write-host

# Write-host ECS processing
# set-location ..\ECS
# xsd.exe IE529_v1-0.xsd xmldsig-core-schema.xsd /N:CAS.SmartFactory.xml.Customs.IE529 /c  |write-host

Write-host IPR Processing
set-location ..\IPR
# xsd.exe Invoice.xsd /N:CAS.SmartFactory.xml.ERP /c  |write-host
xsd.exe stock.xsd /N:CAS.SmartFactory.xml.ERP /c  |write-host
#xsd.exe batch.xsd /N:CAS.SmartFactory.xml.ERP /c  |write-host
#xsd.exe SKUCigarettesSchema.xsd /N:CAS.SmartFactory.xml.ERP /c  |write-host
#xsd.exe SKUCutfillerSchema.xsd /N:CAS.SmartFactory.xml.ERP /c  |write-host

set-location $cpath
Write-host Done...
# Read-Host
