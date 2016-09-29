
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

REM This script should be started from osi folder, such as c:\deploys\src\osi\

set SRC_ROOT=%CD%
for /l %%i in (0,0,1) do (
    if exist prepare.cmd call prepare.cmd
    call force-sync.cmd
    call build.cmd
    mkdir \deploys\apps\osi.root.utt 1>nul 2>&1
    pushd \deploys\apps\osi.root.utt
    if exist prepare.cmd call prepare.cmd
    call %SRC_ROOT%\root\utt\batch\sync.cmd
    osi.root.utt.exe
    popd
    if not "x%EXIT_NOW%" == "x" goto :end
)

:end
