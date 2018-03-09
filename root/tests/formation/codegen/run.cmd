
setlocal
path %PATH%;..\..\..\codegen\precompile;

precompile unordered_set.vbp > unordered_set_test.vb
precompile unordered_set2.vbp > unordered_set2_test.vb
precompile unordered_map.vbp > unordered_map_test.vb
precompile unordered_map2.vbp > unordered_map2_test.vb

move /y *.vb ..\

endlocal
