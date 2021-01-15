
setlocal
path %PATH%;..\..\..\codegen\precompile;
precompile npos_uint_all_bytes_test.vbp > npos_uint_all_bytes_test.vb

setlocal
path %PATH%;..\..\..\..\service\resource\gen
zipgen _guess_encoding unicode_be unicode-be.txt unicode unicode.txt utf8 utf8.txt utf8_bom utf8-bom.txt gbk gbk.txt utf8_2 utf8-2.txt> guess_encoding_tests.vb
endlocal

move /y *.vb ..\
endlocal

