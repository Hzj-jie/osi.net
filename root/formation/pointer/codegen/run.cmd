
setlocal
PATH %PATH%;..\..\..\codegen\precompile;
precompile.exe pointer.vbp > pointer.vb
precompile.exe array_pointer.vbp > array_pointer.vb
precompile.exe weak_pointer.vbp > weak_pointer.vb
precompile.exe weak_ref_pointer.vbp > weak_ref_pointer.vb
move /y *.vb ..\
endlocal

