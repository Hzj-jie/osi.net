
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Namespace counter
    Public Class instance_count_counter(Of T)
        Private Shared ReadOnly c As Int64 = register_counter(strcat(strtoupper(GetType(T).Name()), "_INSTANCE_COUNT"))

        Public Shared Sub alloc()
            increase(c)
        End Sub

        Public Shared Sub dealloc()
            decrease(c)
        End Sub

        Public Shared Function count() As Int64
            Dim s As snapshot = Nothing
            s = snapshot.[New](instance_count_counter(Of T).c)
            assert(Not s Is Nothing)
            Return +s.count
        End Function
    End Class
End Namespace
