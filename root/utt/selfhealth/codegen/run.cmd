
setlocal
csc /o+ exec_case_exe.cs
PATH %PATH%;..\..\..\..\service\resource\gen;
zipgen.exe _exec_case_exe exec_case_exe exec_case_exe.exe > exec_case_exe.vb
move /y exec_case_exe.vb ..\
endlocal

