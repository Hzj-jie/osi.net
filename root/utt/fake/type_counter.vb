
Imports osi.root.lock

Public Class type_counter(Of T)
    Private Shared ReadOnly c As atomic_int

    Shared Sub New()
        c = New atomic_int()
    End Sub

    Public Shared Function count() As UInt32
        Return CUInt(+c)
    End Function

    Public Shared Function next_id() As UInt32
        Return CUInt(c.increment())
    End Function
End Class
