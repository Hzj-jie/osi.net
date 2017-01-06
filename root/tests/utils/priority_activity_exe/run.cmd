
setlocal
msbuild /p:Configuration=Release
move /Y bin\Release\* .
path %PATH%;..\..\..\..\service\resource\gen
zipgen.exe _priority_activity priority_activity_exe priority_activity.exe priority_activity_pdb priority_activity.pdb > priority_activity_exe.vb
move /Y priority_activity_exe.vb ..\
endlocal

