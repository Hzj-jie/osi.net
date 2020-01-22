
setlocal
path %PATH%;..\..\..\..\resource\gen;
gen _bstyle_test_data case1 case1.txt case2 case2.txt case3 case3.txt > bstyle_test_data.vb
move /Y bstyle_test_data.vb ..\
endlocal

