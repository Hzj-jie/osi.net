
call "C:\Program Files (x86)\Microsoft Visual Studio 12.0\VC\vcvarsall.bat"

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
