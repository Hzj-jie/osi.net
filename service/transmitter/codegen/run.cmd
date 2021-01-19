
setlocal
path %PATH%;..\..\..\root\codegen\precompile;
precompile slimqless2_event_sync_T_pump.vbp > slimqless2_event_sync_T_pump.vb
precompile qless2_event_sync_T_pump.vbp > qless2_event_sync_T_pump.vb
move /y *.vb ..\
endlocal
