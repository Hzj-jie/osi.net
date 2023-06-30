
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.lock

Public NotInheritable Class instance_count(Of T)
    Private Shared ReadOnly c As atomic_int64 = New atomic_int64()

    Public Shared Function alloc() As Int64
        Return c.increment()
    End Function

    Public Shared Function dealloc() As Int64
        Return c.decrement()
    End Function

    Public Shared Function count() As Int64
        Return +c
    End Function

    Private Sub New()
    End Sub
End Class
