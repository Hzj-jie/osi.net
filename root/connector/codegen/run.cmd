
setlocal
path %PATH%;..\..\codegen\precompile;
precompile minmax.vbp > minmax.vb
move /Y *.vb ..\
endlocal

