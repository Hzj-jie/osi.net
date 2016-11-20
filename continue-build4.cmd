
for /l %%x in (0,0,1) do (
    call force-sync.cmd
    call build4.cmd
    if exist c:\cygwin\bin\sleep.exe ( c:\cygwin\bin\sleep 4h )
    if exist c:\cygwin64\bin\sleep.exe ( c:\cygwin64\bin\sleep 4h )
)
