
setlocal
PATH %PATH%;..\..\..\resource\gen\;

zipgen.exe math_constants pi_1m pi-1m.txt pi_10m pi-10m.txt e_1m e-1m.txt e_2m e-2m.txt > math_constants.vb

move /Y *.vb ..\

endlocal


