
setlocal
path %PATH%;..\..\..\..\resource\gen;
gen _dotnet_test_data from_source1 from_source1.cs > dotnet_test_data.vb
move /Y dotnet_test_data.vb ..\
endlocal

