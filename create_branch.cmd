rem//  $LastChangedDate: 2011-04-04 09:42:00 +0200 (Pn, 04 kwi 2011) $
rem//  $Rev: 5816 $
rem//  $LastChangedBy: mzbrzezny $
rem//  $URL: svn://svnserver.hq.cas.com.pl/VS/trunk/PR21-CommServer/Scripts/create_branch.cmd $
rem//  $Id: create_branch.cmd 5816 2011-04-04 07:42:00Z mzbrzezny $
if "%1"=="" goto ERROR
set branchtype=%2
if "%branchtype%"=="" goto setbranch

:dothejob
svn mkdir svn://svnserver.hq.cas.com.pl/VS/%branchtype%/SmartFactory/%1  -m "created new SmartFActory tag  %1 (release folder)"
svn copy svn://svnserver.hq.cas.com.pl/VS/trunk/PR42-SmartFactory svn://svnserver.hq.cas.com.pl/VS/%branchtype%/SmartFactory/%1/PR42-SmartFactory -m "created new CommServer tag %1 (project PR21-CommServer)"
svn copy svn://svnserver.hq.cas.com.pl/VS/trunk/PR39-CommonResources svn://svnserver.hq.cas.com.pl/VS/%branchtype%/SmartFactory/%1/PR39-CommonResources -m "created new CommServer tag %1 (project PR39-CommonResources)"
svn copy svn://svnserver.hq.cas.com.pl/VS/trunk/ImageLibrary svn://svnserver.hq.cas.com.pl/VS/%branchtype%/SmartFactory/%1/ImageLibrary -m "created new CommServer tag %1 (project ImageLibrary)"
svn copy svn://svnserver.hq.cas.com.pl/VS/trunk/CommonBinaries svn://svnserver.hq.cas.com.pl/VS/%branchtype%/SmartFactory/%1/CommonBinaries -m "created new CommServer tag %1 (project CommonBinaries)"

goto EXIT

:setbranch
set branchtype=branches
goto dothejob
:ERROR
echo Parametr must be set
:EXIT
