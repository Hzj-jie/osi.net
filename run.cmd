
REM 2010
call "C:\Program Files\Microsoft Visual Studio 10.0\VC\vcvarsall.bat" x86
call "C:\Program Files (x86)\Microsoft Visual Studio 10.0\VC\vcvarsall.bat" x86
REM 2012
call "C:\Program Files\Microsoft Visual Studio 12.0\VC\vcvarsall.bat"
call "C:\Program Files (x86)\Microsoft Visual Studio 12.0\VC\vcvarsall.bat"
REM 2015
call "C:\Program Files\Microsoft Visual Studio 14.0\Common7\Tools\VsDevCmd.bat"
call "C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\Tools\VsDevCmd.bat"
REM 2017 Profestional
call "C:\Program Files (x86)\Microsoft Visual Studio\2017\Professional\Common7\Tools\VsDevCmd.bat"

subst T: D:\

REM This script should be started from osi folder, such as c:\deploys\src\osi\

set SRC_ROOT=%CD%
del build.log
del run.log
for /l %%i in (0,0,1) do (
    if exist prepare.cmd call prepare.cmd
    call force-sync.cmd

    echo ------------ >> %SRC_ROOT%\build.log
    time /t >> %SRC_ROOT%\build.log
    date /t >> %SRC_ROOT%\build.log
    call build.cmd >> %SRC_ROOT%\build.log 2>&1

    mkdir \deploys\apps\osi.root.utt 1>nul 2>&1
    pushd \deploys\apps\osi.root.utt
    call %SRC_ROOT%\root\utt\batch\sync.cmd

    echo ------------ >> %SRC_ROOT%\run.log
    time /t >> %SRC_ROOT%\run.log
    date /t >> %SRC_ROOT%\run.log
    osi.root.utt.exe >> %SRC_ROOT%\run.log 2>&1

    popd
    if not "x%EXIT_NOW%" == "x" goto :end
)

:end
