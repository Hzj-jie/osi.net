
Imports osi.root.constants
Imports osi.root.template

Public Class default_to_uint32(Of keyT)
    Inherits _to_uint32(Of keyT)

    Public Overrides Function at(ByRef k As keyT) As UInt32
        Return signing(k)
    End Function

    Public Overrides Function reverse(ByVal i As UInt32) As keyT
        assert(False)
        Return Nothing
    End Function
End Class

Public Class _byte_to_uint32
    Inherits default_to_uint32(Of Byte)

    Public Overrides Function at(ByRef k As Byte) As UInt32
        Return k
    End Function

    Public Overrides Function reverse(ByVal r As UInt32) As Byte
        assert(r >= min_uint8 AndAlso r <= max_uint8)
        Return r
    End Function
End Class

Public Class _char_to_uint32
    Inherits default_to_uint32(Of Char)

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

Public Class _int16_to_uint32
    Inherits default_to_uint32(Of Int16)

    Public Overrides Function at(ByRef k As Int16) As UInt32
        Return CUInt(max_int16) + k
    End Function
End Class

Public Class _uint16_to_uint32
    Inherits default_to_uint32(Of UInt16)

    Public Overrides Function at(ByRef k As UInt16) As UInt32
        Return k
    End Function

    Public Overrides Function reverse(ByVal i As UInt32) As UInt16
        assert(i >= min_uint16 AndAlso i <= max_uint16)
        Return i
    End Function
End Class

Public Class _string_to_uint32
    Inherits default_to_uint32(Of String)
End Class

Public Class _int64_to_uint32
    Inherits default_to_uint32(Of Int64)

    Public Overrides Function at(ByRef k As Int64) As UInt32
        If k < 0 Then
            Return (k Mod max_uint32) + max_uint32
        Else
            Return k Mod max_uint32
        End If
    End Function
End Class

Public Class _uint32_to_uint32
    Inherits default_to_uint32(Of UInt32)

    Public Overrides Function at(ByRef k As UInt32) As UInt32
        Return k
    End Function

    Public Overrides Function reverse(ByVal i As UInt32) As UInt32
        Return i
    End Function
End Class

Public Class _int32_to_uint32
    Inherits default_to_uint32(Of Int32)

    Public Overrides Function at(ByRef k As Int32) As UInt32
        Return int32_uint32(k)
    End Function

    Public Overrides Function reverse(ByVal i As UInt32) As Int32
        Return uint32_int32(i)
    End Function
End Class
