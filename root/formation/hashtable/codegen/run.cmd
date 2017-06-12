
setlocal
path %PATH%;..\..\..\codegen\precompile;
precompile hashtable.iterator.vbp > hashtable.iterator.vb
move /Y *.vb ..\
endlocal

