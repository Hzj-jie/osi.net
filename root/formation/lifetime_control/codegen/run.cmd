
setlocal
PATH %PATH%;..\..\..\codegen\precompile;
precompile.exe disposer.vbp > disposer.vb
precompile.exe dispose_ptr.vbp > dispose_ptr.vb
precompile.exe weak_dispose_ptr.vbp > weak_dispose_ptr.vb
precompile.exe disposer_T.vbp > disposer_T.vb
precompile.exe weak_disposer_T.vbp > weak_disposer_T.vb
move /y *.vb ..\
endlocal

