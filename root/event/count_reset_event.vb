
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Threading
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.template
Imports lock_t = osi.root.lock.slimlock.monitorlock

Public NotInheritable Class count_reset_event
    Inherits count_reset_event(Of _0)
End Class

' An EventWaitHandle, which releases exact count of threads set function has been called, up to _MAX_COUNT.
' If _MAX_COUNT = 1, this class has a same behavior as AutoResetEvent, which is unnecessary.
<Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")>
Public Class count_reset_event(Of _MAX_COUNT As _int64)
    Implements IDisposable

    Private Shared ReadOnly MAX_COUNT As Int32 = Function() As Int32
                                                     Dim c As Int64 = +alloc(Of _MAX_COUNT)()
                                                     assert(c <= max_int32)
                                                     assert(c >= 0)
                                                     assert(c <> 1)
                                                     Return CInt(c)
                                                 End Function()
    Private ReadOnly m As ManualResetEvent
    Private ReadOnly clock As tick_clock
    Private l As lock_t
    Private p As Int32

    Public Sub New()
        m = New ManualResetEvent(False)
        clock = thread_static_implementation_of(Of tick_clock).resolve_or_default(tick_clock.default)
    End Sub

    Public Sub [set]()
        l.wait()
        p += 1
        If p = 1 Then
            ' Any exception will break this event.
            assert(m.force_set())
        End If
        If MAX_COUNT > 0 AndAlso p > MAX_COUNT Then
            p = MAX_COUNT
        End If
        l.release()
    End Sub

    Private Function acquire() As Boolean
        Dim r As Boolean = False
        r = True
        l.wait()
        If p = 1 Then
            p = 0
            assert(m.force_reset())
        ElseIf p > 1 Then
            p -= 1
        Else
            assert(p = 0)
            r = False
        End If
        l.release()
        Return r
    End Function

    Public Function wait(ByVal ms As Int64) As Boolean
        If ms < 0 OrElse ms > max_int32 Then
            wait()
            Return True
        Else
            Dim until_ms As Int64 = int64_0
            until_ms = clock.milliseconds_l() + ms
            While True
                If acquire() Then
                    Return True
                Else
                    Dim wait_ms As Int64 = int64_0
                    wait_ms = until_ms - clock.milliseconds_l()
                    If wait_ms < 0 OrElse Not m.wait(wait_ms) Then
                        Return False
                    End If
                End If
            End While
            assert(False)
            Return False
        End If
    End Function

    Public Sub wait()
        While True
            If acquire() Then
                Return
            Else
                assert(m.wait())
            End If
        End While
    End Sub

    Public Sub reset()
        l.wait()
        p = 0
        assert(m.force_reset())
        l.release()
    End Sub

    <Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")>
    Public Sub Dispose() Implements IDisposable.Dispose
        m.Close()
        GC.SuppressFinalize(Me)
    End Sub

    <Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")>
    Protected Overrides Sub Finalize()
        m.Close()
        MyBase.Finalize()
    End Sub
End Class
