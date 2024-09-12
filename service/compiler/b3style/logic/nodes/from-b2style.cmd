
setlocal

path %PATH%;..\..\..\..\..\root\codegen\sed;

call :run class
call :run function.template
call :run function_call_with_template
call :run function_name_with_template
call :run delegate_with_semi_colon

exit /b 0

:RUN
sed " Class b2style" " Class b3style" ..\..\..\b2style\rewriter\nodes\%~1.vb | sed " typed_node_writer" " logic_writer" > %~1.vb
exit /b 0
