
setlocal
path %PATH%;..\..\..\..\resource\gen;
gen _import_executor_cases case1 case1.txt case2 case2.txt case3 case3.txt case4 case4.txt heap heap.txt > import_executor_cases.vb
move /y import_executor_cases.vb ..\
endlocal

