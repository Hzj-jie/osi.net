
setlocal
path %PATH%;..\..\codegen\precompile;
precompile bt.iterator.vbp > bt.iterator.vb
precompile list.iterator.vbp > list.iterator.vb
precompile mapheap.iterator.vbp > mapheap.iterator.vb
precompile trie.iterator.vbp > trie.iterator.vb
endlocal

