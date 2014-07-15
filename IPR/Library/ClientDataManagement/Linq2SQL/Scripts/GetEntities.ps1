Write-Host "This file creates all the entities classes for the selected SQL database"
$env:path += "; C:\Program Files (x86)\Microsoft SDKs\Windows\v7.0A\Bin"
Get-Location | Write-host
SQLMetal.exe /server:CAS_SP\MSSQL_01 /database:IPRDEV /namespace:CAS.SmartFactory.IPR.Client.DataManagement.Linq2SQL /code:..\Entities.cs /language:csharp | write-host
write-host "Done . "