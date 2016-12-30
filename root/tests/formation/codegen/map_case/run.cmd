
setlocal
path %PATH%;..\..\..\..\codegen\precompile;
precompile bmap_test.vbp > bmap_test.vb
precompile omap_test.vbp > omap_test.vb
precompile map_test.vbp > map_test.vb
precompile smap_test.vbp > smap_test.vb
precompile hashmap_test.vbp > hashmap_test.vb
endlocal
