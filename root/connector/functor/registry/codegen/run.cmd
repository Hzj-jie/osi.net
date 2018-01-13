
setlocal
path %PATH%;..\..\..\..\codegen\precompile;
precompile binary_operator_registry.vbp > binary_operator_registry.vb
precompile bytes_serializer_registry.vbp > bytes_serializer_registry.vb
precompile string_serializer_registry.vbp > string_serializer_registry.vb
move /Y *.vb ..\
endlocal

