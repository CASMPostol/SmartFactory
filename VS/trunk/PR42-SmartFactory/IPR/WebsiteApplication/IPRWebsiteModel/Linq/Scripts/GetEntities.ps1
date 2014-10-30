Write-Host "This file creates all the entities classes for the selected site"
$env:path += "; C:\Program Files\Common Files\microsoft shared\Web Server Extensions\14\BIN"
Get-Location | Write-host
SPMetal.exe /web:http://casbw/sites/iprmodel /language:csharp /namespace:CAS.SmartFactory.IPR.WebsiteModel.Linq /parameters:Parameters.xml /code:..\Entities.cs| write-host
write-host "Done . "