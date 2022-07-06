
setlocal
path %PATH%;..\..\..\resource\gen\;
zipgen.exe b3style_rules nlexer_rule nlexer_rule.txt syntaxer_rule syntaxer_rule.txt > b3style_rules.vb
move /Y *.vb ..\
endlocal

