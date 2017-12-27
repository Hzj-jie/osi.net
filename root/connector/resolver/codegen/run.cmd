
setlocal
path %PATH%;..\..\..\codegen\precompile;
precompile thread_safe_resolver.vbp > thread_safe_resolver.vb
precompile resolver.vbp > resolver.vb
precompile thread_static_resolver.vbp > thread_static_resolver.vb
precompile global_resolver.vbp > global_resolver.vb
move /Y *.vb ..\
endlocal

