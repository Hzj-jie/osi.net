
setlocal
path %PATH%;..\..\..\..\resource\gen;
gen _simulator_cases sim1 sim1.txt sim2 sim2.txt sim3 sim3.txt > simulator_cases.vb
move /y simulator_cases.vb ..\
endlocal

