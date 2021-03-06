
setlocal
path %PATH%;..\..\codegen\precompile;
precompile list.iterator.vbp > list.iterator.vb
precompile mapheap.iterator.vbp > mapheap.iterator.vb
precompile trie.iterator.vbp > trie.iterator.vb
precompile bit_array.vbp > bit_array.vb
precompile bit_array_thread_safe.vbp > bit_array_thread_safe.vb
precompile pair.vbp > pair.vb
precompile const_pair.vbp > const_pair.vb
precompile first_const_pair.vbp > first_const_pair.vb
precompile unordered_map.vbp > unordered_map.vb
precompile unordered_set.vbp > unordered_set.vb
precompile vector.iterator.vbp > vector.iterator.vb

for /l %%x in (2,1,8) do precompile tuple%%x.vbp > tuple%%x.vb
precompile make_tuple.vbp > make_tuple.vb

move /Y *.vb ..\

endlocal

