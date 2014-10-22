
rem//  $LastChangedDate$
rem//  $Rev$
rem//  $LastChangedBy$
rem//  $URL$
rem//  $Id$


set branchtype=tags
set TagFolder=Rel_SHR_2_20_00
set TagPath=svn://svnserver.hq.cas.com.pl/VS/%branchtype%/SmartFactory/rel_SHR_2_20_00
set trunkPath=svn://svnserver.hq.cas.com.pl/VS/trunk

svn mkdir %TagPath%  -m "created new %TagPath% (in %branchtype% folder)"

svn copy %trunkPath%/PR42-SmartFactory %TagPath%/PR42-SmartFactory -m "created copy in %TagPath% of the project PR42-SmartFactory"
svn copy %trunkPath%/PR39-CommonResources %TagPath%/PR39-CommonResources -m "created copy in %TagPath% of the project PR39-CommonResources"
svn copy %trunkPath%/ImageLibrary %TagPath%/ImageLibrary -m "created copy in %TagPath% of the project ImageLibrary"
svn copy %trunkPath%/PR44-SharePoint %TagPath%/PR44-SharePoint -m "created copy in %TagPath% of the project PR44-SharePoint"

pause ....

