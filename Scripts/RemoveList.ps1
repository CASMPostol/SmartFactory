
Add-PSSnapin Microsoft.SharePoint.PowerShell
Get-SPWebApplication
$url = "http://casmp/sites/Home"
$site=new-object Microsoft.SharePoint.SPSite($url)
$web = $site.OpenWeb()
$list = $web.Lists["CAS.SmartFactory.IPR - ListInstance1"]
$list.Delete()
