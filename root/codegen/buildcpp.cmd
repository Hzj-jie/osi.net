
setlocal

call "%~dp0\..\..\init_vs_env.cmd"

cl %* /Ox /link /SUBSYSTEM:CONSOLE,"5.01" /libpath:"C:\Program Files (x86)\Microsoft SDKs\Windows\v7.1A\Lib"
del *.obj

endlocal
