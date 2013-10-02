Write-Host "This file creates all the entities classes for the selected site"
$env:path += "; C:\Program Files\Common Files\Microsoft Shared\Web server extensions\14\Bin"
Get-Location | Write-host
SPMetal.exe /web:http://cas_sp:11225/sites/cw11 /language:csharp /namespace:CAS.SmartFactory.CW.DiposalRequestWebPart.Linq /parameters:Parameters.xml /code:..\Entities.cs| write-host
write-host "Done . "