
setlocal
PATH %PATH%;..\..\codegen\precompile\;

precompile slimqless2_runner.vbp > slimqless2_runner.vb
precompile slimheapless_runner.vbp > slimheapless_runner.vb
precompile qless_runner.vbp > qless_runner.vb
precompile slimqless2_threadpool.vbp > slimqless2_threadpool.vb
precompile slimheapless_threadpool.vbp > slimheapless_threadpool.vb
precompile qless_threadpool.vbp > qless_threadpool.vb
precompile slimqless2_runner_synchronize_invoke.vbp > slimqless2_runner_synchronize_invoke.vb
precompile slimheapless_runner_synchronize_invoke.vbp > slimheapless_runner_synchronize_invoke.vb
precompile qless_runner_synchronize_invoke.vbp > qless_runner_synchronize_invoke.vb

move /Y *.vb ..\

endlocal
