Write-Host "This file creates all the entities classes for the selected site"
$env:path += "; C:\Program Files\Common Files\microsoft shared\Web Server Extensions\14\BIN"
Get-Location | Write-host
SPMetal.exe /web:http://casbw/sites/shrmodel /parameters:Parameters.xml /language:csharp /namespace:CAS.SmartFactory.Shepherd.Client.DataManagement.Linq /code:..\Entities.cs| write-host
write-host "Done . "



