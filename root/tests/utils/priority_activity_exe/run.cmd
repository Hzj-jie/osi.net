
setlocal
msbuild /p:Configuration=Debug
move /Y bin\Debug\* .
path %PATH%;..\..\..\..\service\resource\gen
zipgen.exe _priority_activity priority_activity_exe priority_activity.exe priority_activity_pdb priority_activity.pdb > priority_activity_exe.vb
move /Y priority_activity_exe.vb ..\
endlocal

