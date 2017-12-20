
setlocal
path %PATH%;..\..\..\codegen\precompile;
precompile thread_safe_resolver.vbp > thread_safe_resolver.vb
precompile resolver.vbp > resolver.vb
move /Y *.vb ..\
endlocal

