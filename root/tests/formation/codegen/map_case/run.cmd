
setlocal
path %PATH%;..\..\..\..\codegen\precompile;
precompile bmap_test.vbp > bmap_test.vb
precompile omap_test.vbp > omap_test.vb
precompile map_test.vbp > map_test.vb
precompile smap_test.vbp > smap_test.vb
precompile hashmap_test.vbp > hashmap_test.vb
precompile unordered_map_test.vbp > unordered_map_test_2.vb
precompile unordered_map2_test.vbp > unordered_map2_test_2.vb
precompile unordered_map3_test.vbp > unordered_map3_test_2.vb

move hashmap_test.vb ..\..\
move unordered_map_test_2.vb ..\..\
move unordered_map2_test_2.vb ..\..\
move unordered_map3_test_2.vb ..\..\
move *.vb ..\..\binary_tree\
endlocal
