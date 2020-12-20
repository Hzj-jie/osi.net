
csc adaptive_array.cs
call "adaptive_array.exe" t:UInt32 oc:big_uint cam:Private > "big_uint.adaptive_array.vb"
call "adaptive_array.exe" "oc:vector(Of T)" cam:Private ntp:1 > "vector.adaptive_array.vb"
move /y big_uint.adaptive_array.vb ..\..\..\service\math\big_uint\
move /y vector.adaptive_array.vb ..\..\formation\

