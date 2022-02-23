
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Namespace primitive
    Partial Public NotInheritable Class instruction_wrapper
        Implements instruction

        Private i As instruction

        Public Function bytes_size() As UInt32 Implements exportable.bytes_size
            assert(i IsNot Nothing)
            Return i.bytes_size()
        End Function

        Public Function export(ByRef b() As Byte) As Boolean Implements exportable.export
            assert(i IsNot Nothing)
            Return i.export(b)
        End Function

        Public Function export(ByRef s As String) As Boolean Implements exportable.export
            assert(i IsNot Nothing)
            Return i.export(s)
        End Function

        Public Sub execute(ByVal imi As imitation) Implements instruction.execute
            assert(i IsNot Nothing)
            i.execute(imi)
        End Sub

        Public Overrides Function ToString() As String
            Return Convert.ToString(i)
        End Function
    End Class
End Namespace
