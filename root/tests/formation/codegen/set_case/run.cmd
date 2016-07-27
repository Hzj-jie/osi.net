
setlocal
path %PATH%;..\..\..\..\codegen\precompile;
precompile bset_test.vbp > bset_test.vb
precompile oset_test.vbp > oset_test.vb
precompile set_test.vbp > set_test.vb
precompile sset_test.vbp > sset_test.vb
endlocal
