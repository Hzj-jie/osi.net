
setlocal
path %PATH%;..\..\..\codegen\precompile;
precompile npos_uint.vbp > npos_uint.vb
precompile npos_uint32.vbp > npos_uint32.vb
precompile npos_uint64.vbp > npos_uint64.vb
precompile size_t.vbp > size_t.vb
precompile size_t_32.vbp > size_t_32.vb
precompile size_t_64.vbp > size_t_64.vb
precompile timeout_ms_t.vbp > timeout_ms_t.vb
precompile retry_times_t.vbp > retry_times_t.vb
precompile positive_npos_uint.vbp > positive_npos_uint.vb
precompile positive_npos_uint32.vbp > positive_npos_uint32.vb
precompile positive_npos_uint64.vbp > positive_npos_uint64.vb
precompile positive_size_t.vbp > positive_size_t.vb
precompile positive_size_t_32.vbp > positive_size_t_32.vb
precompile positive_size_t_64.vbp > positive_size_t_64.vb
precompile limited_ints.vbp > limited_ints.vb

move /y *.vb ..\
endlocal

