#rem
Echo on
Echo Path=%1
call "%VS110COMNTOOLS%VsDevCmd.bat"
signtool sign /a /n CAS %1 >sign.log
xcopy /y %1 %2 >sign.log


