
setlocal
PATH %PATH%;..\..\..\root\codegen\precompile\;

precompile big_int.vbp > big_int.vb
precompile big_dec.vbp > big_dec.vb

move /Y *.vb ..\

endlocal

