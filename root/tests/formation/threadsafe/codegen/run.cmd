
setlocal
PATH %PATH%;..\..\..\..\codegen\precompile;
precompile.exe waitable_slimqless2_test.vbp > waitable_slimqless2_test.vb
precompile.exe waitable_slimheapless_test.vbp > waitable_slimheapless_test.vb
endlocal

