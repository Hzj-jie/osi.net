
REM 2010
call "C:\Program Files\Microsoft Visual Studio 10.0\VC\vcvarsall.bat" x86
call "C:\Program Files (x86)\Microsoft Visual Studio 10.0\VC\vcvarsall.bat" x86
REM 2012
call "C:\Program Files\Microsoft Visual Studio 12.0\VC\vcvarsall.bat"
call "C:\Program Files (x86)\Microsoft Visual Studio 12.0\VC\vcvarsall.bat"
REM 2015
call "C:\Program Files\Microsoft Visual Studio 14.0\Common7\Tools\VsDevCmd.bat"
call "C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\Tools\VsDevCmd.bat"

subst T: D:\

for /l %%i in (0,0,1) do (
    if exist prepare.cmd call prepare.cmd
    pushd osi
    call force-sync.cmd
    call build.cmd
    popd
    pushd c:\deploys\apps\osi.root.utt
    if exist batch\sync.cmd (
         call batch\sync.cmd
    ) else (
         call sync.cmd
    )
    osi.root.utt.exe
    popd
    if not "x%EXIT_NOW%" == "x" goto :end
)

:end
