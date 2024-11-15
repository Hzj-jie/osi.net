
setlocal

path %PATH%;..\..\..\..\..\root\codegen\sed;

call :run biguint
call :run bool
call :run integer
call :run string
call :run ufloat

call :run logic_name
call :run param
call :run return_clause
call :run heap_declaration
call :run heap_name
call :run struct
call :run raw_variable_name
call :run static_cast
call :run multi_sentence_paragraph
call :run condition
call :run for_loop
call :run while
call :run delegate
call :run reinterpret_cast
call :run raw_value
call :run value_clause

exit /b 0

:RUN
sed " Class bstyle" " Class b3style" ..\..\..\bstyle\logic\nodes\%~1.vb > %~1.vb
exit /b 0
