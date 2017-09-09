
setlocal
cd /d "%~dp0"
path %PATH%;..\..\..\resource\gen;
gen _configuration_test_cases test_config test_config.ini > configuration_test_cases.vb
move /y configuration_test_cases.vb ..\
endlocal

