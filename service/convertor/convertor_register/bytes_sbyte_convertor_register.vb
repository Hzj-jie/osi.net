
#If RETIRED
Imports osi.root.constants
Imports osi.root.connector

Public Class bytes_sbyte_convertor_register(Of T)
    Public Shared Function bind() As Boolean
        Return bytes_convertor_register(Of T).bind(
                   Function(b() As Byte, ByRef offset As UInt32, ByRef o As T) As Boolean
                       Dim v As SByte = sbyte_0
                       Return bytes_sbyte(b, v, offset) AndAlso
                              cast(Of T)(v, o)
                   End Function,
                   Function(b() As Byte, ii As UInt32, il As UInt32, ByRef o As T) As Boolean
                       Dim v As SByte = sbyte_0
                       Return bytes_sbyte(b, ii, il, v) AndAlso
                              cast(Of T)(v, o)
                   End Function,
                   Function(i As T, b() As Byte, ByRef offset As UInt32) As Boolean
                       Dim v As SByte = sbyte_0
                       Return cast(Of SByte)(i, v) AndAlso
                              sbyte_bytes(v, b, offset)
                   End Function,
                   Function(i As T, ByRef o() As Byte) As Boolean
                       Dim v As SByte = sbyte_0
                       If cast(Of SByte)(i, v) Then
                           o = sbyte_bytes(v)
                           Return True
                       Else
                           Return False
                       End If
                   End Function)
    End Function

    Public Shared Sub assert_bind()
        assert(bind())
    End Sub

    Private Sub New()
    End Sub
End Class
#End If
