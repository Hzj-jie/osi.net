
setlocal
PATH %PATH%;..\..\..\codegen\precompile;
precompile.exe qless2.vbp > qless2.vb
precompile.exe heapless.vbp > heapless.vb
endlocal

