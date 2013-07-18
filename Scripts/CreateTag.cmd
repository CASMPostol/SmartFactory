
rem//  $LastChangedDate: 2009-10-27 13:31:11 +0100 (Wt, 27 paü 2009) $
rem//  $Rev: 4116 $
rem//  $LastChangedBy: mzbrzezny $
rem//  $URL: svn://svnserver.hq.cas.com.pl/VS/trunk/PR31-DataProviders/Scripts/create_branch.cmd $
rem//  $Id: create_branch.cmd 4116 2009-10-27 12:31:11Z mzbrzezny $


set branchtype=tags
set TagFolder=Rel_1_70_11
set TagPath=svn://svnserver.hq.cas.com.pl/VS/%branchtype%/SmartFactory/rel_1_70_11
set trunkPath=svn://svnserver.hq.cas.com.pl/VS/trunk

svn mkdir %TagPath%  -m "created new %TagPath% (in %branchtype% folder)"

svn copy %trunkPath%/PR42-SmartFactory %TagPath%/PR42-SmartFactory -m "created copy in %TagPath% of the project PR42-SmartFactory"
svn copy %trunkPath%/PR39-CommonResources %TagPath%/PR39-CommonResources -m "created copy in %TagPath% of the project PR39-CommonResources"
svn copy %trunkPath%/ImageLibrary %TagPath%/ImageLibrary -m "created copy in %TagPath% of the project ImageLibrary"
svn copy %trunkPath%/PR44-SharePoint %TagPath%/PR44-SharePoint -m "created copy in %TagPath% of the project PR44-SharePoint"

pause ....

