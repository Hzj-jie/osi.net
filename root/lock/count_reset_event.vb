
Imports System.Threading
Imports osi.root.constants
Imports osi.root.connector
Imports lock_t = osi.root.lock.slimlock.monitorlock

' An EventWaitHandle, which releases exact count of threads set function has been called.
Public NotInheritable Class count_reset_event
    Implements IDisposable

    Private ReadOnly m As ManualResetEvent
    Private l As lock_t
    Private p As Int32

    Public Sub New()
        m = New ManualResetEvent(False)
    End Sub

    Public Sub [set]()
        l.wait()
        p += 1
        assert(m.Set())
        l.release()
    End Sub

    Private Function acquire() As Boolean
        Dim r As Boolean = False
        r = True
        l.wait()
        If p = 1 Then
            p = 0
            assert(m.Reset())
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
            until_ms = nowadays.milliseconds() + ms
            While True
                If acquire() Then
                    Return True
                Else
                    Dim wait_ms As Int64 = int64_0
                    wait_ms = until_ms - nowadays.milliseconds()
                    If wait_ms < 0 OrElse Not m.WaitOne(wait_ms) Then
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
                assert(m.WaitOne())
            End If
        End While
    End Sub

    Public Sub reset()
        l.wait()
        p = 0
        assert(m.Reset())
        l.release()
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        m.Close()
        GC.SuppressFinalize(Me)
    End Sub

    Protected Overrides Sub Finalize()
        m.Close()
        MyBase.Finalize()
    End Sub
End Class
