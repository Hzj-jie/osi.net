
Imports System.Threading
Imports osi.root.connector

Namespace counter
    Public Class instance_count_counter(Of T)
        Private Shared ReadOnly c As Int64

        Shared Sub New()
            c = register_counter(strcat(strtoupper(GetType(T).Name()), "_INSTANCE_COUNT"))
        End Sub

        Public Shared Sub alloc()
            increase(c)
        End Sub

        Public Shared Sub dealloc()
            decrease(c)
        End Sub

        Public Shared Function count() As Int64
            Dim c As Int64? = Nothing
            assert(counter(instance_count_counter(Of T).c, Nothing, count:=c))
            assert(c.HasValue())
            Return c.Value()
        End Function
    End Class
End Namespace
