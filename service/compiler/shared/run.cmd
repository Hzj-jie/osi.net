
setlocal
path %PATH%;..\..\resource\gen\;
genall --pattern=*.txt shared_rules > shared_rules.vb
endlocal

