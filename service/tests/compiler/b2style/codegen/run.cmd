
setlocal
path %PATH%;..\..\..\..\resource\gen;
genall --pattern=*.txt _b2style_test_data > b2style_test_data.vb
move /Y b2style_test_data.vb ..\
endlocal

