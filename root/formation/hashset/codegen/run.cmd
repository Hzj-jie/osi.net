
setlocal
path %PATH%;..\..\..\codegen\precompile;
precompile hashset.iterator.vbp > hashset.iterator.vb
move /Y *.vb ..\
endlocal

