
setlocal
path %PATH%;..\..\..\codegen\precompile;
precompile bmap.vbp > bmap.vb
precompile omap.vbp > omap.vb
precompile smap.vbp > smap.vb
precompile bmap2.vbp > map.vb
precompile bt.iterator.vbp > bt.iterator.vb
precompile bset.vbp > bset.vb
precompile set.vbp > set.vb

move /Y map.vb ..\..\
move /Y set.vb ..\..\
move /Y *.vb ..\
endlocal

