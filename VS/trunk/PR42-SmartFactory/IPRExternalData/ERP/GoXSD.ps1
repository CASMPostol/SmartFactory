Write-Host "This file creates all the c# classes for ERP namespace"
$env:path += "; C:\Program Files\Microsoft SDKs\Windows\v7.1\Bin\x64\"
# xsd.exe Invoice.xsd /N:CAS.SmartFactory.xml.erp /c  |write-host
xsd.exe stock.xsd /N:CAS.SmartFactory.xml.erp /c  |write-host
#xsd.exe batch.xsd /N:CAS.SmartFactory.xml.erp /c  |write-host
#xsd.exe SKUCigarettesSchema.xsd /N:CAS.SmartFactory.xml.erp /c  |write-host
#xsd.exe SKUCutfillerSchema.xsd /N:CAS.SmartFactory.xml.erp /c  |write-host
Write-host Done...
