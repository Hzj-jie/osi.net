
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Threading
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.lock
Imports osi.root.template

Public Class count_down_event
    Inherits count_down_event(Of _0)

    Public Sub New(ByVal count As UInt32)
        MyBase.New(count)
    End Sub
End Class

' Releases only when @COUNT {#set} have been called. This is a manual reset event.
' {#reset} function is not thread-safe, it cannot be called concurrently with other {#set} functions.
Public Class count_down_event(Of COUNT As _int64)
    Inherits disposer

    Private Shared ReadOnly __count As Int64 = assert_which.of(+alloc(Of COUNT)()).is_non_negative()
    Private ReadOnly _count As UInt32
    Private ReadOnly ewh As EventWaitHandle
    Private ReadOnly c As atomic_int32

    Private Sub New(ByVal count As UInt32, ByVal internal As Boolean)
        assert(count > 0)
        _count = count
        c = New atomic_int32(CInt(_count))
        ewh = New ManualResetEvent(False)
    End Sub

    Protected Sub New(ByVal count As UInt32)
        Me.New(count, True)
        assert(__count = 0)
    End Sub

    Public Sub New()
        Me.New(CUInt(__count), True)
    End Sub

    ' Returns true if this {#set} call takes effect.
    Public Function [set]() As Boolean
        Dim r As Int32 = 0
        r = c.decrement()
        If r < 0 Then
            c.increment()
            Return False
        ElseIf r = 0 Then
            assert(ewh.force_set())
            Return True
        Else
            Return False
        End If
    End Function

    Public Sub reset()
        assert(ewh.force_reset())
        c.set(CInt(_count))
    End Sub

    Public Function wait(ByVal timeout_ms As Int64) As Boolean
        Return ewh.wait(timeout_ms)
    End Function

    Public Sub wait()
        assert(ewh.wait())
    End Sub

    Protected Overrides Sub disposer()
        disposable(Of EventWaitHandle).dispose(ewh)
    End Sub
End Class
