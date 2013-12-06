Write-Host "This file creates the schema file (xsd) for selected class"
Get-Location | Write-host
$env:path += "; C:\Program Files\Microsoft SDKs\Windows\v7.1\Bin\x64\"
# Set-Location C:\vs\Projects\JTI\Excel2XML
#Write-host press any key to continue .....
#Read-host
$cpath = get-location
Write-host CAS.SmartFactory.xml.dll processing
set-location ..\..\bin\debug\
xsd.exe CAS.CWInteroperability.dll /t:CAS.SmartFactory.CW.Interoperability.DocumentsFactory.Statement.StatementContent | write-host
set-location $cpath
