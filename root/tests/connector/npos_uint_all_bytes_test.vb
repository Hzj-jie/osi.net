
Option Explicit On
Option Infer Off
Option Strict On

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with npos_uint_all_bytes_test.vbp ----------
'so change npos_uint_all_bytes_test.vbp instead of this file


Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utt


'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with npos_uint_bytes_test.vbp ----------
'so change npos_uint_bytes_test.vbp instead of this file



Public Class npos_uint_bytes_test
    Inherits [case]

    Public Overrides Function run() As Boolean
        For i As Int32 = 0 To 1024 * 1024 * 8 - 1
            Dim x As npos_uint = Nothing
            If rnd_bool() Then
                x = rnd_int64()
            Else
                x = rnd_uint64()
            End If
            Dim b() As Byte = Nothing
            b = bytes_serializer.to_bytes(x)
            Dim y As npos_uint = Nothing
            Dim p As UInt32 = uint32_0
            assertion.is_true(bytes_serializer.consume_from(b, p, y))
            assertion.equal(p, array_size(b))
            assertion.equal(x, y)
            assertion.is_true(bytes_serializer.from_bytes(b, y))
            assertion.equal(x, y)
            Dim o() As Byte = Nothing
            ReDim o(-1)
            assertion.is_false(bytes_serializer.append_to(y, o, uint32_0))
#If True AndAlso True AndAlso (True OrElse True) Then
#If True Then
            If x.npos() Then
#ElseIf True Then
            If x.infinite() Then
#Else
            FAILURE
#End If
                ReDim o(CInt(sizeof_int8 - uint32_1))
            Else
                ReDim o(CInt(npos_uint.sizeof_value + sizeof_int8 - uint32_1))
            End If
            assertion.is_true(bytes_serializer.append_to(y, o, uint32_0))
            assertion.equal(memcmp(b, o), 0)
#End If
            ReDim o(CInt(npos_uint.sizeof_value + sizeof_int8))
            Dim offset As UInt32 = uint32_0
            assertion.is_true(bytes_serializer.append_to(y, o, offset))
            assertion.equal(memcmp(b, o, offset), 0)
        Next
        Return True
    End Function
End Class

'finish npos_uint_bytes_test.vbp --------


'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with npos_uint_bytes_test.vbp ----------
'so change npos_uint_bytes_test.vbp instead of this file



Public Class npos_uint32_bytes_test
    Inherits [case]

    Public Overrides Function run() As Boolean
        For i As Int32 = 0 To 1024 * 1024 * 8 - 1
            Dim x As npos_uint32 = Nothing
            If rnd_bool() Then
                x = rnd_int64()
            Else
                x = rnd_uint64()
            End If
            Dim b() As Byte = Nothing
            b = bytes_serializer.to_bytes(x)
            Dim y As npos_uint32 = Nothing
            Dim p As UInt32 = uint32_0
            assertion.is_true(bytes_serializer.consume_from(b, p, y))
            assertion.equal(p, array_size(b))
            assertion.equal(x, y)
            assertion.is_true(bytes_serializer.from_bytes(b, y))
            assertion.equal(x, y)
            Dim o() As Byte = Nothing
            ReDim o(-1)
            assertion.is_false(bytes_serializer.append_to(y, o, uint32_0))
#If True AndAlso True AndAlso (True OrElse True) Then
#If True Then
            If x.npos() Then
#ElseIf True Then
            If x.infinite() Then
#Else
            FAILURE
#End If
                ReDim o(CInt(sizeof_int8 - uint32_1))
            Else
                ReDim o(CInt(npos_uint32.sizeof_value + sizeof_int8 - uint32_1))
            End If
            assertion.is_true(bytes_serializer.append_to(y, o, uint32_0))
            assertion.equal(memcmp(b, o), 0)
#End If
            ReDim o(CInt(npos_uint32.sizeof_value + sizeof_int8))
            Dim offset As UInt32 = uint32_0
            assertion.is_true(bytes_serializer.append_to(y, o, offset))
            assertion.equal(memcmp(b, o, offset), 0)
        Next
        Return True
    End Function
End Class

'finish npos_uint_bytes_test.vbp --------


'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with npos_uint_bytes_test.vbp ----------
'so change npos_uint_bytes_test.vbp instead of this file



Public Class npos_uint64_bytes_test
    Inherits [case]

    Public Overrides Function run() As Boolean
        For i As Int32 = 0 To 1024 * 1024 * 8 - 1
            Dim x As npos_uint64 = Nothing
            If rnd_bool() Then
                x = rnd_int64()
            Else
                x = rnd_uint64()
            End If
            Dim b() As Byte = Nothing
            b = bytes_serializer.to_bytes(x)
            Dim y As npos_uint64 = Nothing
            Dim p As UInt32 = uint32_0
            assertion.is_true(bytes_serializer.consume_from(b, p, y))
            assertion.equal(p, array_size(b))
            assertion.equal(x, y)
            assertion.is_true(bytes_serializer.from_bytes(b, y))
            assertion.equal(x, y)
            Dim o() As Byte = Nothing
            ReDim o(-1)
            assertion.is_false(bytes_serializer.append_to(y, o, uint32_0))
#If True AndAlso True AndAlso (True OrElse True) Then
#If True Then
            If x.npos() Then
#ElseIf True Then
            If x.infinite() Then
#Else
            FAILURE
#End If
                ReDim o(CInt(sizeof_int8 - uint32_1))
            Else
                ReDim o(CInt(npos_uint64.sizeof_value + sizeof_int8 - uint32_1))
            End If
            assertion.is_true(bytes_serializer.append_to(y, o, uint32_0))
            assertion.equal(memcmp(b, o), 0)
#End If
            ReDim o(CInt(npos_uint64.sizeof_value + sizeof_int8))
            Dim offset As UInt32 = uint32_0
            assertion.is_true(bytes_serializer.append_to(y, o, offset))
            assertion.equal(memcmp(b, o, offset), 0)
        Next
        Return True
    End Function
End Class

'finish npos_uint_bytes_test.vbp --------


'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with npos_uint_bytes_test.vbp ----------
'so change npos_uint_bytes_test.vbp instead of this file



Public Class positive_npos_uint_bytes_test
    Inherits [case]

    Public Overrides Function run() As Boolean
        For i As Int32 = 0 To 1024 * 1024 * 8 - 1
            Dim x As positive_npos_uint = Nothing
            If rnd_bool() Then
                x = rnd_int64()
            Else
                x = rnd_uint64()
            End If
            Dim b() As Byte = Nothing
            b = bytes_serializer.to_bytes(x)
            Dim y As positive_npos_uint = Nothing
            Dim p As UInt32 = uint32_0
            assertion.is_true(bytes_serializer.consume_from(b, p, y))
            assertion.equal(p, array_size(b))
            assertion.equal(x, y)
            assertion.is_true(bytes_serializer.from_bytes(b, y))
            assertion.equal(x, y)
            Dim o() As Byte = Nothing
            ReDim o(-1)
            assertion.is_false(bytes_serializer.append_to(y, o, uint32_0))
#If False AndAlso True AndAlso (True OrElse True) Then
#If True Then
            If x.npos() Then
#ElseIf True Then
            If x.infinite() Then
#Else
            FAILURE
#End If
                ReDim o(CInt(sizeof_int8 - uint32_1))
            Else
                ReDim o(CInt(positive_npos_uint.sizeof_value + sizeof_int8 - uint32_1))
            End If
            assertion.is_true(bytes_serializer.append_to(y, o, uint32_0))
            assertion.equal(memcmp(b, o), 0)
#End If
            ReDim o(CInt(positive_npos_uint.sizeof_value + sizeof_int8))
            Dim offset As UInt32 = uint32_0
            assertion.is_true(bytes_serializer.append_to(y, o, offset))
            assertion.equal(memcmp(b, o, offset), 0)
        Next
        Return True
    End Function
End Class

'finish npos_uint_bytes_test.vbp --------


'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with npos_uint_bytes_test.vbp ----------
'so change npos_uint_bytes_test.vbp instead of this file



Public Class positive_npos_uint32_bytes_test
    Inherits [case]

    Public Overrides Function run() As Boolean
        For i As Int32 = 0 To 1024 * 1024 * 8 - 1
            Dim x As positive_npos_uint32 = Nothing
            If rnd_bool() Then
                x = rnd_int64()
            Else
                x = rnd_uint64()
            End If
            Dim b() As Byte = Nothing
            b = bytes_serializer.to_bytes(x)
            Dim y As positive_npos_uint32 = Nothing
            Dim p As UInt32 = uint32_0
            assertion.is_true(bytes_serializer.consume_from(b, p, y))
            assertion.equal(p, array_size(b))
            assertion.equal(x, y)
            assertion.is_true(bytes_serializer.from_bytes(b, y))
            assertion.equal(x, y)
            Dim o() As Byte = Nothing
            ReDim o(-1)
            assertion.is_false(bytes_serializer.append_to(y, o, uint32_0))
#If False AndAlso True AndAlso (True OrElse True) Then
#If True Then
            If x.npos() Then
#ElseIf True Then
            If x.infinite() Then
#Else
            FAILURE
#End If
                ReDim o(CInt(sizeof_int8 - uint32_1))
            Else
                ReDim o(CInt(positive_npos_uint32.sizeof_value + sizeof_int8 - uint32_1))
            End If
            assertion.is_true(bytes_serializer.append_to(y, o, uint32_0))
            assertion.equal(memcmp(b, o), 0)
#End If
            ReDim o(CInt(positive_npos_uint32.sizeof_value + sizeof_int8))
            Dim offset As UInt32 = uint32_0
            assertion.is_true(bytes_serializer.append_to(y, o, offset))
            assertion.equal(memcmp(b, o, offset), 0)
        Next
        Return True
    End Function
End Class

'finish npos_uint_bytes_test.vbp --------


'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with npos_uint_bytes_test.vbp ----------
'so change npos_uint_bytes_test.vbp instead of this file



Public Class positive_npos_uint64_bytes_test
    Inherits [case]

    Public Overrides Function run() As Boolean
        For i As Int32 = 0 To 1024 * 1024 * 8 - 1
            Dim x As positive_npos_uint64 = Nothing
            If rnd_bool() Then
                x = rnd_int64()
            Else
                x = rnd_uint64()
            End If
            Dim b() As Byte = Nothing
            b = bytes_serializer.to_bytes(x)
            Dim y As positive_npos_uint64 = Nothing
            Dim p As UInt32 = uint32_0
            assertion.is_true(bytes_serializer.consume_from(b, p, y))
            assertion.equal(p, array_size(b))
            assertion.equal(x, y)
            assertion.is_true(bytes_serializer.from_bytes(b, y))
            assertion.equal(x, y)
            Dim o() As Byte = Nothing
            ReDim o(-1)
            assertion.is_false(bytes_serializer.append_to(y, o, uint32_0))
#If False AndAlso True AndAlso (True OrElse True) Then
#If True Then
            If x.npos() Then
#ElseIf True Then
            If x.infinite() Then
#Else
            FAILURE
#End If
                ReDim o(CInt(sizeof_int8 - uint32_1))
            Else
                ReDim o(CInt(positive_npos_uint64.sizeof_value + sizeof_int8 - uint32_1))
            End If
            assertion.is_true(bytes_serializer.append_to(y, o, uint32_0))
            assertion.equal(memcmp(b, o), 0)
#End If
            ReDim o(CInt(positive_npos_uint64.sizeof_value + sizeof_int8))
            Dim offset As UInt32 = uint32_0
            assertion.is_true(bytes_serializer.append_to(y, o, offset))
            assertion.equal(memcmp(b, o, offset), 0)
        Next
        Return True
    End Function
End Class

'finish npos_uint_bytes_test.vbp --------


'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with npos_uint_bytes_test.vbp ----------
'so change npos_uint_bytes_test.vbp instead of this file



Public Class positive_size_t_bytes_test
    Inherits [case]

    Public Overrides Function run() As Boolean
        For i As Int32 = 0 To 1024 * 1024 * 8 - 1
            Dim x As positive_size_t = Nothing
            If rnd_bool() Then
                x = rnd_int64()
            Else
                x = rnd_uint64()
            End If
            Dim b() As Byte = Nothing
            b = bytes_serializer.to_bytes(x)
            Dim y As positive_size_t = Nothing
            Dim p As UInt32 = uint32_0
            assertion.is_true(bytes_serializer.consume_from(b, p, y))
            assertion.equal(p, array_size(b))
            assertion.equal(x, y)
            assertion.is_true(bytes_serializer.from_bytes(b, y))
            assertion.equal(x, y)
            Dim o() As Byte = Nothing
            ReDim o(-1)
            assertion.is_false(bytes_serializer.append_to(y, o, uint32_0))
#If False AndAlso True AndAlso (True OrElse True) Then
#If True Then
            If x.npos() Then
#ElseIf True Then
            If x.infinite() Then
#Else
            FAILURE
#End If
                ReDim o(CInt(sizeof_int8 - uint32_1))
            Else
                ReDim o(CInt(positive_size_t.sizeof_value + sizeof_int8 - uint32_1))
            End If
            assertion.is_true(bytes_serializer.append_to(y, o, uint32_0))
            assertion.equal(memcmp(b, o), 0)
#End If
            ReDim o(CInt(positive_size_t.sizeof_value + sizeof_int8))
            Dim offset As UInt32 = uint32_0
            assertion.is_true(bytes_serializer.append_to(y, o, offset))
            assertion.equal(memcmp(b, o, offset), 0)
        Next
        Return True
    End Function
End Class

'finish npos_uint_bytes_test.vbp --------


'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with npos_uint_bytes_test.vbp ----------
'so change npos_uint_bytes_test.vbp instead of this file



Public Class positive_size_t_32_bytes_test
    Inherits [case]

    Public Overrides Function run() As Boolean
        For i As Int32 = 0 To 1024 * 1024 * 8 - 1
            Dim x As positive_size_t_32 = Nothing
            If rnd_bool() Then
                x = rnd_int64()
            Else
                x = rnd_uint64()
            End If
            Dim b() As Byte = Nothing
            b = bytes_serializer.to_bytes(x)
            Dim y As positive_size_t_32 = Nothing
            Dim p As UInt32 = uint32_0
            assertion.is_true(bytes_serializer.consume_from(b, p, y))
            assertion.equal(p, array_size(b))
            assertion.equal(x, y)
            assertion.is_true(bytes_serializer.from_bytes(b, y))
            assertion.equal(x, y)
            Dim o() As Byte = Nothing
            ReDim o(-1)
            assertion.is_false(bytes_serializer.append_to(y, o, uint32_0))
#If False AndAlso True AndAlso (True OrElse True) Then
#If True Then
            If x.npos() Then
#ElseIf True Then
            If x.infinite() Then
#Else
            FAILURE
#End If
                ReDim o(CInt(sizeof_int8 - uint32_1))
            Else
                ReDim o(CInt(positive_size_t_32.sizeof_value + sizeof_int8 - uint32_1))
            End If
            assertion.is_true(bytes_serializer.append_to(y, o, uint32_0))
            assertion.equal(memcmp(b, o), 0)
#End If
            ReDim o(CInt(positive_size_t_32.sizeof_value + sizeof_int8))
            Dim offset As UInt32 = uint32_0
            assertion.is_true(bytes_serializer.append_to(y, o, offset))
            assertion.equal(memcmp(b, o, offset), 0)
        Next
        Return True
    End Function
End Class

'finish npos_uint_bytes_test.vbp --------


'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with npos_uint_bytes_test.vbp ----------
'so change npos_uint_bytes_test.vbp instead of this file



Public Class positive_size_t_64_bytes_test
    Inherits [case]

    Public Overrides Function run() As Boolean
        For i As Int32 = 0 To 1024 * 1024 * 8 - 1
            Dim x As positive_size_t_64 = Nothing
            If rnd_bool() Then
                x = rnd_int64()
            Else
                x = rnd_uint64()
            End If
            Dim b() As Byte = Nothing
            b = bytes_serializer.to_bytes(x)
            Dim y As positive_size_t_64 = Nothing
            Dim p As UInt32 = uint32_0
            assertion.is_true(bytes_serializer.consume_from(b, p, y))
            assertion.equal(p, array_size(b))
            assertion.equal(x, y)
            assertion.is_true(bytes_serializer.from_bytes(b, y))
            assertion.equal(x, y)
            Dim o() As Byte = Nothing
            ReDim o(-1)
            assertion.is_false(bytes_serializer.append_to(y, o, uint32_0))
#If False AndAlso True AndAlso (True OrElse True) Then
#If True Then
            If x.npos() Then
#ElseIf True Then
            If x.infinite() Then
#Else
            FAILURE
#End If
                ReDim o(CInt(sizeof_int8 - uint32_1))
            Else
                ReDim o(CInt(positive_size_t_64.sizeof_value + sizeof_int8 - uint32_1))
            End If
            assertion.is_true(bytes_serializer.append_to(y, o, uint32_0))
            assertion.equal(memcmp(b, o), 0)
#End If
            ReDim o(CInt(positive_size_t_64.sizeof_value + sizeof_int8))
            Dim offset As UInt32 = uint32_0
            assertion.is_true(bytes_serializer.append_to(y, o, offset))
            assertion.equal(memcmp(b, o, offset), 0)
        Next
        Return True
    End Function
End Class

'finish npos_uint_bytes_test.vbp --------


'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with npos_uint_bytes_test.vbp ----------
'so change npos_uint_bytes_test.vbp instead of this file



Public Class retry_times_t_bytes_test
    Inherits [case]

    Public Overrides Function run() As Boolean
        For i As Int32 = 0 To 1024 * 1024 * 8 - 1
            Dim x As retry_times_t = Nothing
            If rnd_bool() Then
                x = rnd_int64()
            Else
                x = rnd_uint64()
            End If
            Dim b() As Byte = Nothing
            b = bytes_serializer.to_bytes(x)
            Dim y As retry_times_t = Nothing
            Dim p As UInt32 = uint32_0
            assertion.is_true(bytes_serializer.consume_from(b, p, y))
            assertion.equal(p, array_size(b))
            assertion.equal(x, y)
            assertion.is_true(bytes_serializer.from_bytes(b, y))
            assertion.equal(x, y)
            Dim o() As Byte = Nothing
            ReDim o(-1)
            assertion.is_false(bytes_serializer.append_to(y, o, uint32_0))
#If True AndAlso False AndAlso (True OrElse True) Then
#If True Then
            If x.npos() Then
#ElseIf True Then
            If x.infinite() Then
#Else
            FAILURE
#End If
                ReDim o(CInt(sizeof_int8 - uint32_1))
            Else
                ReDim o(CInt(retry_times_t.sizeof_value + sizeof_int8 - uint32_1))
            End If
            assertion.is_true(bytes_serializer.append_to(y, o, uint32_0))
            assertion.equal(memcmp(b, o), 0)
#End If
            ReDim o(CInt(retry_times_t.sizeof_value + sizeof_int8))
            Dim offset As UInt32 = uint32_0
            assertion.is_true(bytes_serializer.append_to(y, o, offset))
            assertion.equal(memcmp(b, o, offset), 0)
        Next
        Return True
    End Function
End Class

'finish npos_uint_bytes_test.vbp --------


'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with npos_uint_bytes_test.vbp ----------
'so change npos_uint_bytes_test.vbp instead of this file



Public Class size_t_bytes_test
    Inherits [case]

    Public Overrides Function run() As Boolean
        For i As Int32 = 0 To 1024 * 1024 * 8 - 1
            Dim x As size_t = Nothing
            If rnd_bool() Then
                x = rnd_int64()
            Else
                x = rnd_uint64()
            End If
            Dim b() As Byte = Nothing
            b = bytes_serializer.to_bytes(x)
            Dim y As size_t = Nothing
            Dim p As UInt32 = uint32_0
            assertion.is_true(bytes_serializer.consume_from(b, p, y))
            assertion.equal(p, array_size(b))
            assertion.equal(x, y)
            assertion.is_true(bytes_serializer.from_bytes(b, y))
            assertion.equal(x, y)
            Dim o() As Byte = Nothing
            ReDim o(-1)
            assertion.is_false(bytes_serializer.append_to(y, o, uint32_0))
#If True AndAlso True AndAlso (True OrElse True) Then
#If True Then
            If x.npos() Then
#ElseIf True Then
            If x.infinite() Then
#Else
            FAILURE
#End If
                ReDim o(CInt(sizeof_int8 - uint32_1))
            Else
                ReDim o(CInt(size_t.sizeof_value + sizeof_int8 - uint32_1))
            End If
            assertion.is_true(bytes_serializer.append_to(y, o, uint32_0))
            assertion.equal(memcmp(b, o), 0)
#End If
            ReDim o(CInt(size_t.sizeof_value + sizeof_int8))
            Dim offset As UInt32 = uint32_0
            assertion.is_true(bytes_serializer.append_to(y, o, offset))
            assertion.equal(memcmp(b, o, offset), 0)
        Next
        Return True
    End Function
End Class

'finish npos_uint_bytes_test.vbp --------


'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with npos_uint_bytes_test.vbp ----------
'so change npos_uint_bytes_test.vbp instead of this file



Public Class size_t_32_bytes_test
    Inherits [case]

    Public Overrides Function run() As Boolean
        For i As Int32 = 0 To 1024 * 1024 * 8 - 1
            Dim x As size_t_32 = Nothing
            If rnd_bool() Then
                x = rnd_int64()
            Else
                x = rnd_uint64()
            End If
            Dim b() As Byte = Nothing
            b = bytes_serializer.to_bytes(x)
            Dim y As size_t_32 = Nothing
            Dim p As UInt32 = uint32_0
            assertion.is_true(bytes_serializer.consume_from(b, p, y))
            assertion.equal(p, array_size(b))
            assertion.equal(x, y)
            assertion.is_true(bytes_serializer.from_bytes(b, y))
            assertion.equal(x, y)
            Dim o() As Byte = Nothing
            ReDim o(-1)
            assertion.is_false(bytes_serializer.append_to(y, o, uint32_0))
#If True AndAlso True AndAlso (True OrElse True) Then
#If True Then
            If x.npos() Then
#ElseIf True Then
            If x.infinite() Then
#Else
            FAILURE
#End If
                ReDim o(CInt(sizeof_int8 - uint32_1))
            Else
                ReDim o(CInt(size_t_32.sizeof_value + sizeof_int8 - uint32_1))
            End If
            assertion.is_true(bytes_serializer.append_to(y, o, uint32_0))
            assertion.equal(memcmp(b, o), 0)
#End If
            ReDim o(CInt(size_t_32.sizeof_value + sizeof_int8))
            Dim offset As UInt32 = uint32_0
            assertion.is_true(bytes_serializer.append_to(y, o, offset))
            assertion.equal(memcmp(b, o, offset), 0)
        Next
        Return True
    End Function
End Class

'finish npos_uint_bytes_test.vbp --------


'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with npos_uint_bytes_test.vbp ----------
'so change npos_uint_bytes_test.vbp instead of this file



Public Class size_t_64_bytes_test
    Inherits [case]

    Public Overrides Function run() As Boolean
        For i As Int32 = 0 To 1024 * 1024 * 8 - 1
            Dim x As size_t_64 = Nothing
            If rnd_bool() Then
                x = rnd_int64()
            Else
                x = rnd_uint64()
            End If
            Dim b() As Byte = Nothing
            b = bytes_serializer.to_bytes(x)
            Dim y As size_t_64 = Nothing
            Dim p As UInt32 = uint32_0
            assertion.is_true(bytes_serializer.consume_from(b, p, y))
            assertion.equal(p, array_size(b))
            assertion.equal(x, y)
            assertion.is_true(bytes_serializer.from_bytes(b, y))
            assertion.equal(x, y)
            Dim o() As Byte = Nothing
            ReDim o(-1)
            assertion.is_false(bytes_serializer.append_to(y, o, uint32_0))
#If True AndAlso True AndAlso (True OrElse True) Then
#If True Then
            If x.npos() Then
#ElseIf True Then
            If x.infinite() Then
#Else
            FAILURE
#End If
                ReDim o(CInt(sizeof_int8 - uint32_1))
            Else
                ReDim o(CInt(size_t_64.sizeof_value + sizeof_int8 - uint32_1))
            End If
            assertion.is_true(bytes_serializer.append_to(y, o, uint32_0))
            assertion.equal(memcmp(b, o), 0)
#End If
            ReDim o(CInt(size_t_64.sizeof_value + sizeof_int8))
            Dim offset As UInt32 = uint32_0
            assertion.is_true(bytes_serializer.append_to(y, o, offset))
            assertion.equal(memcmp(b, o, offset), 0)
        Next
        Return True
    End Function
End Class

'finish npos_uint_bytes_test.vbp --------


'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with npos_uint_bytes_test.vbp ----------
'so change npos_uint_bytes_test.vbp instead of this file



Public Class timeout_ms_t_bytes_test
    Inherits [case]

    Public Overrides Function run() As Boolean
        For i As Int32 = 0 To 1024 * 1024 * 8 - 1
            Dim x As timeout_ms_t = Nothing
            If rnd_bool() Then
                x = rnd_int64()
            Else
                x = rnd_uint64()
            End If
            Dim b() As Byte = Nothing
            b = bytes_serializer.to_bytes(x)
            Dim y As timeout_ms_t = Nothing
            Dim p As UInt32 = uint32_0
            assertion.is_true(bytes_serializer.consume_from(b, p, y))
            assertion.equal(p, array_size(b))
            assertion.equal(x, y)
            assertion.is_true(bytes_serializer.from_bytes(b, y))
            assertion.equal(x, y)
            Dim o() As Byte = Nothing
            ReDim o(-1)
            assertion.is_false(bytes_serializer.append_to(y, o, uint32_0))
#If True AndAlso True AndAlso (False OrElse True) Then
#If False Then
            If x.npos() Then
#ElseIf True Then
            If x.infinite() Then
#Else
            FAILURE
#End If
                ReDim o(CInt(sizeof_int8 - uint32_1))
            Else
                ReDim o(CInt(timeout_ms_t.sizeof_value + sizeof_int8 - uint32_1))
            End If
            assertion.is_true(bytes_serializer.append_to(y, o, uint32_0))
            assertion.equal(memcmp(b, o), 0)
#End If
            ReDim o(CInt(timeout_ms_t.sizeof_value + sizeof_int8))
            Dim offset As UInt32 = uint32_0
            assertion.is_true(bytes_serializer.append_to(y, o, offset))
            assertion.equal(memcmp(b, o, offset), 0)
        Next
        Return True
    End Function
End Class

'finish npos_uint_bytes_test.vbp --------
'finish npos_uint_all_bytes_test.vbp --------
