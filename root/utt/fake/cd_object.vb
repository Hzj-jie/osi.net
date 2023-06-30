
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.lock

<Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")>
Public Class cd_object(Of T)
    Implements IDisposable

    Private Shared ReadOnly c As atomic_int = New atomic_int()
    Private Shared ReadOnly d As atomic_int = New atomic_int()
    Private Shared ReadOnly f As atomic_int = New atomic_int()

    Public Shared Sub reset()
        c.set(0)
        d.set(0)
        f.set(0)
    End Sub

    Public Shared Function constructed() As UInt32
        Return CUInt(+c)
    End Function

    Public Shared Function next_id() As UInt32
        Return constructed()
    End Function

    Public Shared Function disposed() As UInt32
        Return CUInt(+d)
    End Function

    Public Shared Function destructed() As UInt32
        Return CUInt(+f)
    End Function

    Public Shared Function finalized() As UInt32
        Return destructed()
    End Function

    Public ReadOnly id As UInt32

    Public Sub New()
        id = CUInt(c.increment() - 1)
    End Sub

    <Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")>
    Public Sub Dispose() Implements IDisposable.Dispose
        d.increment()
    End Sub

    <Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")>
    Protected Overrides Sub Finalize()
        f.increment()
        MyBase.Finalize()
    End Sub
End Class
