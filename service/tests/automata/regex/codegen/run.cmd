
setlocal
path %PATH%;..\..\..\..\resource\gen\;
zipgen.exe rlexer_test_rules rule1 rlexer_test_rule1.txt rule2 rlexer_test_rule2.txt rule3 rlexer_test_rule3.txt > rlexer_test_rules.vb
endlocal

