
setlocal
PATH %PATH%;..\..\..\root\codegen\precompile\;..\..\resource\gen\;

zipgen.exe math_constants pi_1m pi-1m.txt e_1m e-1m.txt > math_constants.vb
precompile big_int.vbp > big_int.vb
precompile big_dec.vbp > big_dec.vb

move /Y *.vb ..\

endlocal

