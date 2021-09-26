
setlocal
path %PATH%;..\..\..\resource\gen\;
zipgen.exe b2style_lib stdio_h stdio.h cstdio cstdio > b2style_lib.vb
move /Y *.vb ..\
endlocal
