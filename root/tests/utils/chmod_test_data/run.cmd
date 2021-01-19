
setlocal
path %PATH%;..\..\..\..\service\resource\gen\;
zipgen.exe chmod_test_data chmod_test_exe chmod_test_exe.exe > chmod_test_data.vb
move *.vb ..\
endlocal

