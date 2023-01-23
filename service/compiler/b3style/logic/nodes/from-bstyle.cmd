
setlocal

path %PATH%;..\..\..\..\..\root\codegen\sed;

call :run raw_value
call :run biguint
call :run bool
call :run integer
call :run string
call :run ufloat

call :run logic_name
call :run function
call :run function_call
call :run ignore_result_function_call
call :run param
call :run return_clause
call :run value_clause
call :run value_declaration
call :run heap_declaration
call :run value_definition
call :run heap_name
call :run struct
call :run raw_variable_name
call :run value_list
call :run typedef
call :run static_cast
call :run multi_sentence_paragraph
call :run value

exit /b 0

:RUN
sed " Class bstyle" " Class b3style" ..\..\..\bstyle\logic\nodes\%~1.vb > %~1.vb
exit /b 0
