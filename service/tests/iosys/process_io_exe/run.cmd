
setlocal
csc /optimize+ /debug+ process_io_exe.cs
path %PATH%;..\..\..\resource\gen;
gen _process_io_exe process_io_exe process_io_exe.exe process_io_pdb process_io_exe.pdb > process_io_exe.vb
move /Y process_io_exe.vb ..\
endlocal

