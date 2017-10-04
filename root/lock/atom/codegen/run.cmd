
setlocal
path %PATH%;..\..\..\codegen\precompile;

precompile atomic_uint.vbp > atomic_uint.vb
precompile atomic_uint32.vbp > atomic_uint32.vb
precompile atomic_ulong.vbp > atomic_ulong.vb
precompile atomic_uint64.vbp > atomic_uint64.vb

precompile atom.vbp > atom.vb

precompile atomic_int.vbp > atomic_int.vb
precompile atomic_int32.vbp > atomic_int32.vb
precompile atomic_long.vbp > atomic_long.vb
precompile atomic_int64.vbp > atomic_int64.vb

precompile atomic_ref.vbp > atomic_ref.vb

move /Y *.vb ..\

endlocal
