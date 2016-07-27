
setlocal
path %PATH%;..\..\..\..\resource\gen\;
zipgen.exe syntaxer_test_rules rlexer_rule rlexer_rule.txt rlexer_rule2 rlexer_rule2.txt syntaxer_rule syntaxer_rule.txt > syntaxer_test_rules.vb
zipgen.exe syntaxer_test_cases syntaxer_test_case0 case0.txt syntaxer_test_case1 case1.txt > syntaxer_test_cases.vb
endlocal

