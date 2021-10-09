
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.lock

Public NotInheritable Class type_counter(Of T)
    Private Shared ReadOnly c As New atomic_int()

    Public Shared Function count() As UInt32
        Return CUInt(+c)
    End Function

    Public Shared Function next_id() As UInt32
        Return CUInt(c.increment())
    End Function

    Private Sub New()
    End Sub
End Class
