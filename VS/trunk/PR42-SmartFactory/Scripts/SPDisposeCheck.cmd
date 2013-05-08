
set path="C:\Program Files (x86)\Microsoft\SharePoint Dispose Check"
set dashbordsPath="C:\vs\Projects\SmartFactory\PR42-SmartFactory\Shepherd\ShepherdDashboards\bin\Release"

%path%\SPDisposeCheck %dashbordsPath%\CAS.ShepherdDashboards.dll >%dashbordsPath%\CAS.ShepherdDashboards.dclog.txt

pause...