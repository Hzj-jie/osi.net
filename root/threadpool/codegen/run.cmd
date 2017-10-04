
setlocal
PATH %PATH%;..\..\codegen\precompile\;

precompile iqless_threadpool.vbp > slimqless2_threadpool.vb
precompile qless_threadpool.vbp > qless_threadpool.vb
precompile slimheapless_threadpool.vbp > slimheapless_threadpool.vb
precompile slimqless2_runner.vbp > slimqless2_runner.vb
precompile slimheapless_runner.vbp > slimheapless_runner.vb
precompile slimqless2_threadpool2.vbp > slimqless2_threadpool2.vb
precompile slimheapless_threadpool2.vbp > slimheapless_threadpool2.vb
precompile slimqless2_runner_synchronize_invoke.vbp > slimqless2_runner_synchronize_invoke.vb
precompile slimheapless_runner_synchronize_invoke.vbp > slimheapless_runner_synchronize_invoke.vb

move /Y *.vb ..\

endlocal
