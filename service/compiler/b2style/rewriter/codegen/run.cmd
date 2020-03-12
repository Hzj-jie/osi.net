
setlocal
path %PATH%;..\..\..\..\resource\gen\;
zipgen.exe b2style_statements prefix prefix.txt ufloat ufloat.txt > b2style_statements.vb
move /Y *.vb ..\
endlocal

