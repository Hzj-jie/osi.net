
Imports osi.root.lock

Public Class cd_object(Of T)
    Private Shared ReadOnly c As atomic_int
    Private Shared ReadOnly d As atomic_int

    Shared Sub New()
        c = New atomic_int()
        d = New atomic_int()
    End Sub

    Public Shared Sub reset()
        c.set(0)
        d.set(0)
    End Sub

    Public Shared Function constructed() As UInt32
        Return CUInt(+c)
    End Function

    Public Shared Function next_id() As UInt32
        Return constructed()
    End Function

    Public Shared Function destructed() As UInt32
        Return CUInt(+d)
    End Function

    Public ReadOnly id As UInt32

    Public Sub New()
        id = CUInt(c.increment() - 1)
    End Sub

    Protected Overrides Sub Finalize()
        d.increment()
        MyBase.Finalize()
    End Sub
End Class
