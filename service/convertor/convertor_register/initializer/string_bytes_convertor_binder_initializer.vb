
#If RETIRED
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utils

<global_init(global_init_level.services)>
Friend Class string_bytes_convertor_binder_initializer
    Private Class R(Of PROTECTOR)
        Private Class RT
            Inherits convertor_register(Of Byte(), String, PROTECTOR)

            Private Sub New()
            End Sub
        End Class

        Private Class RF
            Inherits convertor_register(Of String, Byte(), PROTECTOR)
        End Class

        Public Shared Sub init()
            RT.assert_bind(Function(i() As Byte, ByRef offset As UInt32, ByRef o As String) As Boolean
                               If bytes_str(i, offset, o) Then
                                   offset = array_size(i)
                                   Return True
                               Else
                                   Return False
                               End If
                           End Function)
            RT.assert_bind(Function(i() As Byte, ii As UInt32, il As UInt32, ByRef o As String) As Boolean
                               Return bytes_str(i, ii, il, o)
                           End Function)
            RF.assert_bind(Function(i As String, ByRef ii As UInt32, o() As Byte, ByRef oi As UInt32) As Boolean
                               If str_bytes(i, ii, o, oi) Then
                                   ii = strlen(i)
                                   Return True
                               Else
                                   Return False
                               End If
                           End Function)
            RF.assert_bind(Function(i As String, ByRef ii As UInt32, ByRef o() As Byte) As Boolean
                               If str_bytes(i, ii, o) Then
                                   ii = strlen(i)
                                   Return True
                               Else
                                   Return False
                               End If
                           End Function)
            RF.assert_bind(Function(i As String, ii As UInt32, il As UInt32, o() As Byte, ByRef oi As UInt32) As Boolean
                               Return str_bytes(i, ii, il, o, oi)
                           End Function)
            RF.assert_bind(Function(i As String, ii As UInt32, il As UInt32, ByRef o() As Byte) As Boolean
                               Return str_bytes(i, ii, il, o)
                           End Function)
        End Sub

        Private Sub New()
        End Sub
    End Class

    Shared Sub New()
        R(Of bytes_conversion_binder_protector).init()
        R(Of string_conversion_binder_protector).init()
    End Sub

    Private Shared Sub init()
    End Sub

    Private Sub New()
    End Sub
End Class
#End If
