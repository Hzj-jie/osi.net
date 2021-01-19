
setlocal
PATH %PATH%;..\..\..\codegen\precompile;
precompile.exe qless2.vbp > qless2.vb
precompile.exe heapless.vbp > heapless.vb
precompile.exe waitable_slimqless2.vbp > waitable_slimqless2.vb
precompile.exe waitable_slimheapless.vbp > waitable_slimheapless.vb
precompile.exe waitable_qless.vbp > waitable_qless.vb
move /y *.vb ..\
endlocal

