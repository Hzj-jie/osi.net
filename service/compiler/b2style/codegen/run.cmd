
..\..\..\resource\gen\zipgen.exe b2style_rules nlexer_rule nlexer_rule.txt syntaxer_rule syntaxer_rule.txt > b2style_rules.vb
..\..\..\..\root\codegen\precompile\precompile.exe name_with_namespace.vbp > name_with_namespace.vb
move /Y *.vb ..\

