
for /F "delims=" %%i in ('ver') do set VER=%%i
call "buildInfoGen\buildInfoGen.exe" "buildInfoGen\buildInfo.vb"
call "++--\++--.exe" "++--\++--.vb"
call "swap\swap.exe" "swap\swap.vb"
call "eva\eva.exe" "eva\eva.vb"
call "array\array.exe" "array\array.vb"
call "console_key_info_mapping\console_key_info_mapping.exe" < "console_key_info_mapping\console_key_info_mapping.txt" > "console_key_info_mapping\console_key_info_mapping.vb"
call "unchecked_int\unchecked_int.exe" > "unchecked_int\unchecked_int.vb"
call "adaptive_array\adaptive_array.exe" t:UInt32 oc:big_uint cam:Private > "adaptive_array\big_uint.adaptive_array.vb"
call "adaptive_array\adaptive_array.exe" "oc:vector(Of T)" cam:Private ntp:1 > "adaptive_array\vector.adaptive_array.vb"
call "unchecked_int2\unchecked_int2.exe" > "unchecked_int2\operators.vb"
call "delegate\delegate.exe" "delegate\delegate.vb"
call "tfver\tfver.cmd" > "tfver\tfver.vb"
call "gitver\gitver.cmd" > "gitver\gitver.vb"

