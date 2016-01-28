
rem//  $LastChangedDate$
rem//  $Rev$
rem//  $LastChangedBy$
rem//  $URL$
rem//  $Id$


pause "You are about to create tag; press ctrl + c to break or any key to continue"

set branchtype=tags
set TagFolder=Rel_2_60_25
set TagPath=svn://svnserver.hq.cas.com.pl/VS/%branchtype%/CAS.SmartFactory.rel_2_60_25
set trunkPath=svn://svnserver.hq.cas.com.pl/VS/trunk

svn mkdir %TagPath%  -m "created new %TagPath% (in %branchtype% folder)"

svn copy %trunkPath%/PR39-CommonResources %TagPath%/PR39-CommonResources -m "created copy in %TagPath% of the project PR39-CommonResources"
svn copy %trunkPath%/ImageLibrary %TagPath%/ImageLibrary -m "created copy in %TagPath% of the project ImageLibrary"
svn copy %trunkPath%/PR42-SmartFactory %TagPath%/PR42-SmartFactory -m "created copy in %TagPath% of the project PR42-SmartFactory"

pause ....

