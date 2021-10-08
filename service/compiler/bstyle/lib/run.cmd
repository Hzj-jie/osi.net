
setlocal
path %PATH%;..\..\..\resource\gen\;
zipgen.exe bstyle_lib stdio_h stdio.h cstdio cstdio bstyle_h bstyle.h > bstyle_lib.vb
move /Y *.vb ..\
endlocal
