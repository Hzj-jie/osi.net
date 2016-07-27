
setlocal
PATH %PATH%;..\..\codegen\precompile\;

precompile iqless_threadpool.vbp > slimqless2_threadpool.vb
precompile qless_threadpool.vbp > qless_threadpool.vb
precompile slimheapless_threadpool.vbp > slimheapless_threadpool.vb

endlocal
