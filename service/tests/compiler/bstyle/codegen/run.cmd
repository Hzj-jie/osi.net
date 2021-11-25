
setlocal
path %PATH%;..\..\..\..\resource\gen;
genall --pattern=*.txt _bstyle_test_data > bstyle_test_data.vb
move /Y bstyle_test_data.vb ..\
endlocal

