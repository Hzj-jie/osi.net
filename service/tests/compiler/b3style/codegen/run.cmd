
setlocal
path %PATH%;..\..\..\..\resource\gen;
genall --pattern=*.txt _b3style_test_data > b3style_test_data.vb
move /Y b3style_test_data.vb ..\
endlocal
