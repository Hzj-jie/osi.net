
setlocal
path %PATH%;..\..\..\codegen\precompile;
precompile slimqless2_threadpool2_test.vbp > slimqless2_threadpool2_test.vb
precompile slimheapless_threadpool2_test.vbp > slimheapless_threadpool2_test.vb
precompile qless_threadpool2_test.vbp > qless_threadpool2_test.vb
precompile managed_threadpool_test.vbp > managed_threadpool_test.vb
precompile slimqless2_runner_test.vbp > slimqless2_runner_test.vb
precompile slimheapless_runner_test.vbp > slimheapless_runner_test.vb
precompile qless_runner_test.vbp > qless_runner_test.vb
precompile slimqless2_runner_synchronize_invoke_test.vbp > slimqless2_runner_synchronize_invoke_test.vb
precompile slimheapless_runner_synchronize_invoke_test.vbp > slimheapless_runner_synchronize_invoke_test.vb
precompile qless_runner_synchronize_invoke_test.vbp > qless_runner_synchronize_invoke_test.vb
move /y *.vb ..\
endlocal
