
setlocal
path %PATH%;..\..\..\..\resource\gen\;
zipgen.exe nlp_test_rules nlexer_rule nlexer_rule.txt syntaxer_rule syntaxer_rule.txt > nlp_test_rules.vb
move /y nlp_test_rules.vb ..\
endlocal

