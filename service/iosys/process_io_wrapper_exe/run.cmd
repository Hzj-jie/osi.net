
setlocal
msbuild /p:Configuration=Release
move /Y bin\Release\* .
path %PATH%;..\..\resource\gen
zipgen.exe _process_io_wrapper process_io_wrapper_exe process_io_wrapper.exe process_io_wrapper_pdb process_io_wrapper.pdb > process_io_wrapper.vb
move /Y process_io_wrapper.vb ..\
endlocal

