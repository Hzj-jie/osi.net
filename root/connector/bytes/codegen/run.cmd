
setlocal
PATH %PATH%;..\..\..\codegen\precompile;
precompile.exe bytes_independent_types.vbp > bytes_independent_types.vb
precompile.exe bytes_dependent_types.vbp > bytes_dependent_types.vb
move /Y *.vb ..\
endlocal

