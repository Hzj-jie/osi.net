vbc /debug- /optimize+ /target:module /out:ilock.netmodule /rootnamespace:osi.root.lock /sdkpath:C:\WINDOWS\Microsoft.NET\Framework\v2.0.50727 ..\ilock.vb ..\slimlocks\islimlock.vb
cl /clr:pure autolock.cpp /FUilock.netmodule /O2 /c /AIC:\WINDOWS\Microsoft.NET\Framework\v2.0.50727 /d1clr:nostdlib
link autolock.obj ilock.netmodule /out:osi.root.lock.autolock.dll /DLL /MANIFEST:NO
csc /debug- /optimize+ /target:exe /reference:osi.root.lock.autolock.dll /out:osi.root.lock.autolock_verifier.exe autolock_verifier.cs
REM csc /debug- /optimize+ /target:module /out:autolock_verifier.netmodule /addmodule:autolock.obj /addmodule:ilock.netmodule autolock_verifier.cs
REM link autolock.obj ilock.netmodule autolock_verifier.netmodule /out:osi.root.lock.autolock_verifier.exe /entry:autolock_verifier.Main /subsystem:console /MANIFEST:NO
del autolock.obj
del autolock.netmodule
del ilock.netmodule
del autolock_verifier.netmodule

..\..\..\service\resource\gen\zipgen.exe autolock_dll ^
    autolock_dll_binary osi.root.lock.autolock.dll ^
    autolock_dll_manifest osi.root.lock.autolock.dll.manifest ^
    msvcr90_x86_manifest Microsoft.VC90.CRT.manifest.x86 ^
    msvcr90_amd64_manifest Microsoft.VC90.CRT.manifest.amd64 ^
    msvcr90_x86_dll msvcr90.dll.x86 ^
    msvcr90_amd64_dll msvcr90.dll.amd64 ^
    msvcm90_x86_dll msvcm90.dll.x86 ^
    msvcm90_amd64_dll msvcm90.dll.amd64 ^
    msvcp90_x86_dll msvcp90.dll.x86 ^
    msvcp90_amd64_dll msvcp90.dll.amd64 ^
    > autolock_dll.vb
