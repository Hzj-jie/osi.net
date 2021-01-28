
setlocal

call ..\..\..\..\init_vs_env.cmd

vbc /target:module /out:tmp.mod /define:NO_REFERENCE /rootnamespace:osi.root.connector ..\..\..\connector\string_utils\char_detection.vb
csc /target:exe /addmodule:tmp.mod /main:string_null_or_whitespace_perf /out:string_null_or_whitespace_perf.exe /SUBSYSTEMVERSION:5.0 string_null_or_whitespace_perf.cs
del tmp.mod
del tmp2.mod

path %PATH%;..\..\..\..\service\resource\gen
zipgen _string_null_or_whitespace_perf string_null_or_whitespace_perf_exe string_null_or_whitespace_perf.exe > ..\string_null_or_whitespace_perf_exe.vb
del string_null_or_whitespace_perf.exe

endlocal

