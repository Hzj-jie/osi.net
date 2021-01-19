
setlocal
path %PATH%;..\..\..\..\resource\gen\;
zipgen.exe lang_parser_test_cases lang_parser_test_case0 case0.txt lang_parser_test_case1 case1.txt > lang_parser_test_cases.vb
move /Y *.vb ..\
endlocal
