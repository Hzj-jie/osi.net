
setlocal
path %PATH%;..\..\..\..\codegen\precompile;
precompile bset_test.vbp > bset_test.vb
precompile oset_test.vbp > oset_test.vb
precompile set_test.vbp > set_test.vb
precompile sset_test.vbp > sset_test.vb
precompile unordered_set_test.vbp > unordered_set_test_2.vb
precompile unordered_set2_test.vbp > unordered_set2_test_2.vb

move unordered_set_test_2.vb ..\..\
move unordered_set2_test_2.vb ..\..\
move *.vb ..\..\binary_tree\

endlocal
