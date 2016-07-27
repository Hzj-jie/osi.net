
setlocal
path %PATH%;..\..\..\..\root\codegen\precompile;
precompile int_divide_perf.vbp > int_divide_perf.vb
precompile int_modulo_perf.vbp > int_modulo_perf.vb
precompile uint_divide_perf.vbp > uint_divide_perf.vb
precompile uint_modulo_perf.vbp > uint_modulo_perf.vb
precompile long_divide_perf.vbp > long_divide_perf.vb
precompile long_modulo_perf.vbp > long_modulo_perf.vb
precompile ulong_divide_perf.vbp > ulong_divide_perf.vb
precompile ulong_modulo_perf.vbp > ulong_modulo_perf.vb
precompile ulong_divrem_perf.vbp > ulong_divrem_perf.vb
precompile ulong_math_divide_perf.vbp > ulong_math_divide_perf.vb
endlocal
