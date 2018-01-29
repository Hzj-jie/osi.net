
setlocal
path %PATH%;..\..\..\codegen\precompile;
precompile hasharray.iterator.vbp > hasharray.iterator.vb
move /Y *.vb ..\
endlocal

