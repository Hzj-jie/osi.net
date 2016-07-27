
setlocal
PATH %PATH%;..\..\..\..\codegen\precompile;

precompile qless_case.vbp > qless_case.vb
precompile qless_case2.vbp > qless_case2.vb
precompile qless_case3.vbp > qless_case3.vb
precompile qless_case4.vbp > qless_case4.vb
precompile qless2_case.vbp > qless2_case.vb
precompile qless2_case2.vbp > qless2_case2.vb
precompile qless2_case3.vbp > qless2_case3.vb
precompile qless2_case4.vbp > qless2_case4.vb
precompile heapless_case.vbp > heapless_case.vb
precompile heapless_case2.vbp > heapless_case2.vb
precompile heapless_case3.vbp > heapless_case3.vb
precompile heapless_case4.vbp > heapless_case4.vb
precompile qless_lock_selection_case.vbp > qless_lock_selection_case.vb
precompile cycle_case.vbp > cycle_case.vb
precompile cycle_case2.vbp > cycle_case2.vb
precompile cycle_case3.vbp > cycle_case3.vb
precompile cycle_case4.vbp > cycle_case4.vb

precompile slimqless2_case3.vbp > slimqless2_case3.vb
precompile slimqless2_case4.vbp > slimqless2_case4.vb
precompile slimheapless_case3.vbp > slimheapless_case3.vb
precompile slimheapless_case4.vbp > slimheapless_case4.vb

endlocal
