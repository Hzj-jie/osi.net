
setlocal
path %PATH%;..\..\..\..\resource\gen;
gen _bstyle_test_data case1 case1.txt case2 case2.txt global_variable global_variable.txt overload_function overload_function.txt single_level_struct single_level_struct.txt nested_struct nested_struct.txt function_struct function_struct.txt return_struct return_struct.txt > bstyle_test_data.vb
move /Y bstyle_test_data.vb ..\
endlocal

