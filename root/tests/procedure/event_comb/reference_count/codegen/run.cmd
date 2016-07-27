
setlocal
PATH %PATH%;..\..\..\..\..\codegen\precompile\;
precompile reference_count_event_comb_test.vbp > reference_count_event_comb_test.vb
precompile reference_count_event_comb_1_test.vbp > reference_count_event_comb_1_test.vb
precompile reference_count_event_comb_2_test.vbp > reference_count_event_comb_2_test.vb
endlocal

