
#If RETIRED
Imports osi.root.constants
Imports osi.root.connector

' Test purpose
<global_init(global_init_level.services)>
Friend Class bytes_bytes_convertor_register
    Private Class RT
        Inherits convertor_register(Of Byte(), Byte(), bytes_conversion_binder_protector)

        Private Sub New()
        End Sub
    End Class

    Shared Sub New()
        RT.assert_bind(Function(i() As Byte, ByRef ii As UInt32, o() As Byte, ByRef oi As UInt32) As Boolean
                           Return bytes_bytes_val(i, ii, o, oi)
                       End Function)
        RT.assert_bind(Function(i() As Byte, ByRef ii As UInt32, ByRef o() As Byte) As Boolean
                           Return bytes_bytes_ref(i, o, ii)
                       End Function)
        RT.assert_bind(Function(i() As Byte, ii As UInt32, len As UInt32, o() As Byte, ByRef oi As UInt32) As Boolean
                           Return bytes_bytes_val(i, ii, len, o, oi)
                       End Function)
        RT.assert_bind(Function(i() As Byte, ii As UInt32, len As UInt32, ByRef o() As Byte) As Boolean
                           Return bytes_bytes_ref(i, ii, len, o)
                       End Function)
    End Sub

    Private Sub New()
    End Sub

    Private Shared Sub init()
    End Sub
End Class
#End If
