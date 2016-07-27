
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utils

Namespace primitive
    Partial Public Class instruction_wrapper
        Implements instruction

        Private i As instruction

        Public Function bytes_size() As UInt32 Implements exportable.bytes_size
            assert(Not i Is Nothing)
            Return i.bytes_size()
        End Function

        Public Function export(ByRef b() As Byte) As Boolean Implements exportable.export
            assert(Not i Is Nothing)
            Return i.export(b)
        End Function

        Public Function export(ByRef s As String) As Boolean Implements exportable.export
            assert(Not i Is Nothing)
            Return i.export(s)
        End Function

        Public Sub execute(ByVal imi As imitation) Implements instruction.execute
            assert(Not i Is Nothing)
            i.execute(imi)
        End Sub
    End Class
End Namespace
