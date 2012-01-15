rem//  $LastChangedDate: 2009-10-30 12:55:21 +0100 (Pt, 30 paü 2009) $
rem//  $Rev: 4192 $
rem//  $LastChangedBy: mzbrzezny $
rem//  $URL: svn://svnserver.hq.cas.com.pl/VS/trunk/PR21-CommServer/Scripts/signdeliverables.cmd $
rem//  $Id: signdeliverables.cmd 4192 2009-10-30 11:55:21Z mzbrzezny $
Echo on
pause ....
call "c:\Program Files (x86)\Microsoft Visual Studio 10.0\VC\vcvarsall.bat" x64
Echo on
signtool sign /a /n CAS ..\Setup\Debug\CAS.SmartFactorySetup.msi
signtool sign /a /n CAS ..\Setup\Debug\Setup.exe
rem "%net20sdk%\signtool.exe" sign /n "CAS" /a "..\Deliverables\CommserverSetup\release\CommServerSetup_4_00_02.exe"
pause ...