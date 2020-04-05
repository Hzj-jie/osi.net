
setlocal
PATH %PATH%;..\..\..\resource\gen\;

zipgen.exe math_constants pi_1m pi-1m.txt e_1m e-1m.txt > math_constants.vb

move /Y *.vb ..\

endlocal


