
setlocal
path %PATH%;..\..\..\..\resource\gen\;
zipgen.exe b2style_statements prefix prefix.txt suffix suffix.txt > b2style_statements.vb
move /Y *.vb ..\
endlocal

