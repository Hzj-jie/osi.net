
setlocal

path %PATH%;..\..\..\..\..\root\codegen\sed;

call :run raw_value.vb
call :run biguint.vb
call :run bool.vb
call :run integer.vb
call :run string.vb
call :run ufloat.vb

call :run logic_name.vb
call :run function.vb
call :run function_call.vb
call :run ignore_result_function_call.vb
call :run param.vb
call :run return_clause.vb
call :run value_clause.vb
call :run value_declaration.vb
call :run heap_declaration.vb
call :run value_definition.vb
call :run heap_name.vb
call :run struct.vb
call :run raw_variable_name.vb
call :run value_list.vb
call :run typedef.vb
call :run static_cast.vb

exit /b 0

:RUN
sed " Class bstyle" " Class b3style" ..\..\..\bstyle\logic\nodes\%~1 > %~1
exit /b 0
