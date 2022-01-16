
setlocal
path %PATH%;..\..\..\..\resource\gen;
genall --pattern=*.txt _import_executor_cases > import_executor_cases.vb
move /y import_executor_cases.vb ..\
endlocal

