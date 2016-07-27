
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utils

<global_init(global_init_level.services)>
Friend Class bytes_convertor_binder_initializer
    Shared Sub New()
        bytes_convertor_register(Of piece).assert_bind(
            Function(b() As Byte, ByRef offset As UInt32, ByRef o As piece) As Boolean
                Dim l As UInt32 = uint32_0
                l = array_size(b)
                If l <= offset Then
                    Return False
                Else
                    o = New piece(b, offset, l - offset)
                    offset = l
                    Return True
                End If
            End Function,
            Function(b() As Byte, offset As UInt32, count As UInt32, ByRef o As piece) As Boolean
                Return piece.create(b, offset, count, o)
            End Function,
            Function(p As piece, o() As Byte, ByRef offset As UInt32) As Boolean
                If p Is Nothing Then
                    Return False
                Else
                    Dim l As UInt32 = uint32_0
                    l = array_size(o)
                    If l <= offset Then
                        Return False
                    Else
                        p.consume(o, offset, l - offset)
                        offset = l
                        Return True
                    End If
                End If
            End Function,
            Function(p As piece, ByRef o() As Byte) As Boolean
                If p Is Nothing Then
                    Return False
                Else
                    o = p.export()
                    Return True
                End If
            End Function)
    End Sub

    Private Shared Sub init()
    End Sub
End Class
