
for /F "delims=" %%i in ('ver') do set VER=%%i
REM copy by root/env proj
call "buildInfoGen\buildInfoGen.exe" "buildInfoGen\buildInfo.vb"

call "++--\++--.exe" "++--\++--.vb"
move /y "++--\++--.vb" ..\connector\

REM Handled by swap/run.cmd
REM call "swap\swap.exe" "swap\swap.vb"

REM Handled by eva/run.cmd
REM call "eva\eva.exe" "eva\eva.vb"

REM Handled by array/run.cmd
REM call "array\array.exe" "array\array.vb"

call "console_key_info_mapping\console_key_info_mapping.exe" < "console_key_info_mapping\console_key_info_mapping.txt" > "console_key_info_mapping\console_key_info_mapping.vb"
move /y "console_key_info_mapping\console_key_info_mapping.vb" ..\constants\

call "unchecked_int\unchecked_int.exe" > "unchecked_int\unchecked_int.vb"
move /y "unchecked_int\unchecked_int.vb" ..\connector\

REM Handled by adaptive_array/run.cmd
REM call "adaptive_array\adaptive_array.exe" t:UInt32 oc:big_uint cam:Private > "adaptive_array\big_uint.adaptive_array.vb"
REM call "adaptive_array\adaptive_array.exe" "oc:vector(Of T)" cam:Private ntp:1 > "adaptive_array\vector.adaptive_array.vb"

call "unchecked_int2\unchecked_int2.exe" > "unchecked_int2\operators.vb"
move /y "unchecked_int2\operators.vb" ..\..\service\math\unchecked\

REM Handled by delegate/run.cmd
REM call "delegate\delegate.exe" "delegate\delegate.vb"
REM move /y "delegate\delegate.vb" ..\connector\delegate\

REM copy by root/env proj
call "tfver\tfver.cmd" > "tfver\tfver.vb"

REM copy by root/env proj
call "gitver\gitver.cmd" > "gitver\gitver.vb"

exit /b 0

