
setlocal
path %PATH%;..\..\..\resource\gen\;
zipgen.exe bstyle_rules nlexer_rule nlexer_rule.txt rlexer_rule rlexer_rule.txt syntaxer_rule syntaxer_rule.txt > bstyle_rules.vb
move /Y *.vb ..\
endlocal

