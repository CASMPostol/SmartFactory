Write-Host "This file creates the schema file (xsd) for selected class"
Get-Location | Write-host
$env:path += "; C:\Program Files (x86)\Microsoft SDKs\Windows\v8.0A\bin\NETFX 4.0 Tools"
# Set-Location C:\vs\Projects\JTI\Excel2XML
#Write-host press any key to continue .....
#Read-host
$cpath = get-location
Write-host CAS.SmartFactory.xml.dll processing
set-location ..\..\bin\debug\
xsd.exe CAS.CWInteroperability.dll /t:CAS.SmartFactory.CW.Interoperability.DocumentsFactory.RequestContent.RequestContentContentType | write-host
set-location $cpath
