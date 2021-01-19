
setlocal
PATH %PATH%;..\..\..\codegen\precompile;
precompile.exe ref.vbp > ref.vb
precompile.exe array_ref.vbp > array_ref.vb
precompile.exe weak_ref.vbp > weak_ref.vb
precompile.exe weak_ref_ref.vbp > weak_ref_ref.vb
move /y *.vb ..\
endlocal

