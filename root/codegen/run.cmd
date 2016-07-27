
for /F "delims=" %%i in ('ver') do set VER=%%i
"buildInfoGen\buildInfoGen.exe" "buildInfoGen\buildInfo.vb"
"++--\++--.exe" "++--\++--.vb"
"swap\swap.exe" "swap\swap.vb"
"eva\eva.exe" "eva\eva.vb"
"array\array.exe" "array\array.vb"
"console_key_info_mapping\console_key_info_mapping.exe" < "console_key_info_mapping\console_key_info_mapping.txt" > "console_key_info_mapping\console_key_info_mapping.vb"
"unchecked_int\unchecked_int.exe" > "unchecked_int\unchecked_int.vb"
"adaptive_array\adaptive_array.exe" t:UInt32 oc:big_uint cam:Private > "adaptive_array\big_uint.adaptive_array.vb"
"adaptive_array\adaptive_array.exe" "oc:vector(Of T)" cam:Private ntp:1 > "adaptive_array\vector.adaptive_array.vb"
"unchecked_int2\unchecked_int2.exe" > "unchecked_int2\operators.vb"
"delegate\delegate.exe" "delegate\delegate.vb"
"tfver\tfver.cmd" > "tfver\tfver.vb"

