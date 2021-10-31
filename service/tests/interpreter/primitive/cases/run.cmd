
setlocal
path %PATH%;..\..\..\..\resource\gen;
gen _simulator_cases sim1 sim1.txt sim2 sim2.txt sim3 sim3.txt sim4 sim4.txt sim5 sim5.txt sim6 sim6.txt sim7 sim7.txt access_heap access_heap.txt dealloc dealloc.txt sim8 sim8.txt > simulator_cases.vb
move /y simulator_cases.vb ..\
endlocal

