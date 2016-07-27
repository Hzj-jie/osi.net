
setlocal
path %PATH%;..\..\..\codegen\precompile;
precompile bmap.vbp > bmap.vb
precompile omap.vbp > omap.vb
precompile smap.vbp > smap.vb
precompile bmap2.vbp > map.vb
endlocal

