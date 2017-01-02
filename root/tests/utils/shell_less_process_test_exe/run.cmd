
setlocal
path %PATH%;..\..\..\..\service\resource\gen
csc /optimize+ /debug+ shell_less_process_test.cs
zipgen.exe _shell_less_process_test shell_less_process_test_exe shell_less_process_test.exe shell_less_process_test_pdb shell_less_process_test.pdb > shell_less_process_test_exe.vb
endlocal

