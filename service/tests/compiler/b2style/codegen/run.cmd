
setlocal
path %PATH%;..\..\..\..\resource\gen;
gen _b2style_test_data case1 case1.txt case2 case2.txt bool_and_bool bool_and_bool.txt > b2style_test_data.vb
move /Y b2style_test_data.vb ..\
endlocal

