
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.template

Public NotInheritable Class fast_to_uint32(Of T)
    Inherits _to_uint32(Of T)

    Private Shared ReadOnly f As Func(Of T, UInt32) = Function() As Func(Of T, UInt32)
                                                          If type_info(Of T).is_valuetype Then
                                                              Return Function(i As T) As UInt32
                                                                         Return int32_uint32(i.GetHashCode())
                                                                     End Function
                                                          End If
                                                          Return Function(i As T) As UInt32
                                                                     If i Is Nothing Then
                                                                         Return 0
                                                                     End If
                                                                     Return int32_uint32(i.GetHashCode())
                                                                 End Function
                                                      End Function()

    Public Shared Function [on](ByVal i As T) As UInt32
        Return f(i)
    End Function

    Public Overrides Function at(ByRef k As T) As UInt32
        Return f(k)
    End Function

    Public Overrides Function reverse(ByVal i As UInt32) As T
        assert(False)
        Return Nothing
    End Function
End Class

Public NotInheritable Class default_to_uint32(Of T)
    Inherits _to_uint32(Of T)

    Public Overrides Function at(ByRef k As T) As UInt32
        Return signing(k)
    End Function

    Public Overrides Function reverse(ByVal i As UInt32) As T
        assert(False)
        Return Nothing
    End Function
End Class

Public NotInheritable Class _byte_to_uint32
    Inherits _to_uint32(Of Byte)

    Public Overrides Function at(ByRef k As Byte) As UInt32
        Return k
    End Function

    Public Overrides Function reverse(ByVal r As UInt32) As Byte
        assert(r >= min_uint8 AndAlso r <= max_uint8)
        Return CByte(r)
    End Function
End Class

Public NotInheritable Class _char_to_uint32
    Inherits _to_uint32(Of Char)

    Public Overrides Function at(ByRef k As Char) As UInt32
        Return Convert.ToUInt32(k)
    End Function

    Public Overrides Function reverse(ByVal i As UInt32) As Char
        Try
            Return Convert.ToChar(i)
        Catch
            assert(False)
            Return character.null
        End Try
    End Function
End Class

Public NotInheritable Class _int16_to_uint32
    Inherits _to_uint32(Of Int16)

    Public Overrides Function at(ByRef k As Int16) As UInt32
        Return CUInt(CInt(k) - CInt(min_int16))
    End Function

    Public Overrides Function reverse(ByVal i As UInt32) As Int16
        assert(i <= (CInt(max_int16) - min_int16))
        Return CShort(CInt(i) + min_int16)
    End Function
End Class

Public NotInheritable Class _uint16_to_uint32
    Inherits _to_uint32(Of UInt16)

    Public Overrides Function at(ByRef k As UInt16) As UInt32
        Return k
    End Function

    Public Overrides Function reverse(ByVal i As UInt32) As UInt16
        assert(i >= min_uint16 AndAlso i <= max_uint16)
        Return CUShort(i)
    End Function
End Class

Public NotInheritable Class _int64_to_uint32
    Inherits _to_uint32(Of Int64)

    Public Overrides Function at(ByRef k As Int64) As UInt32
        If k < 0 Then
            Return CUInt((k Mod max_uint32) + max_uint32)
        End If
        Return CUInt(k Mod max_uint32)
    End Function

    Public Overrides Function reverse(ByVal i As UInt32) As Int64
        Return CLng(i)
    End Function
End Class

Public NotInheritable Class _uint32_to_uint32
    Inherits _to_uint32(Of UInt32)

    Public Overrides Function at(ByRef k As UInt32) As UInt32
        Return k
    End Function

    Public Overrides Function reverse(ByVal i As UInt32) As UInt32
        Return i
    End Function
End Class

Public NotInheritable Class _int32_to_uint32
    Inherits _to_uint32(Of Int32)

    Public Overrides Function at(ByRef k As Int32) As UInt32
        Return int32_uint32(k)
    End Function

    Public Overrides Function reverse(ByVal i As UInt32) As Int32
        Return uint32_int32(i)
    End Function
End Class
