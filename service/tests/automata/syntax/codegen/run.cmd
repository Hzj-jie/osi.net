
setlocal
path %PATH%;..\..\..\..\resource\gen\;
zipgen.exe syntaxer_test_rules rlexer_rule rlexer_rule.txt rlexer_rule2 rlexer_rule2.txt syntaxer_rule syntaxer_rule.txt cycle_dependency_syntaxer_rule cycle_dependency_syntaxer_rule.txt > syntaxer_test_rules.vb
move /Y *.vb ..\
endlocal

