
Imports System.Threading
Imports osi.root.connector
Imports osi.root.formation
Imports lock_t = osi.root.lock.slimlock.monitorlock

' A signal, which is always in one of two states, i.e. black or white
' Users can swith its state or wait for one state.
Public Class bw_event
    Inherits disposer

    Private ReadOnly w As ManualResetEvent
    Private ReadOnly b As ManualResetEvent
    Private l As lock_t

    Public Sub New(Optional initial_white As Boolean = True)
        w = New ManualResetEvent(initial_white)
        b = New ManualResetEvent(Not initial_white)
    End Sub

    Public Function wait_white(ByVal ms As Int32) As Boolean
        Return w.WaitOne(ms)
    End Function

    Public Sub wait_white()
        assert(w.WaitOne())
    End Sub

    Public Function wait_black(ByVal ms As Int32) As Boolean
        Return b.WaitOne(ms)
    End Function

    Public Sub wait_black()
        assert(b.WaitOne())
    End Sub

    Public Function white() As Boolean
        Return w.WaitOne(0)
    End Function

    Public Function black() As Boolean
        Return b.WaitOne(0)
    End Function

    Public Function white_to_black() As Boolean
        l.wait()
        Try
            If white() Then
#If DEBUG Then
                assert(Not black())
#End If
                assert(w.Reset())
                assert(b.Set())
                Return True
            Else
                Return False
            End If
        Finally
            l.release()
        End Try
    End Function

    Public Function black_to_white() As Boolean
        l.wait()
        Try
            If black() Then
#If DEBUG Then
                assert(Not white())
#End If
                assert(b.Reset())
                assert(w.Set())
                Return True
            Else
                Return False
            End If
        Finally
            l.release()
        End Try
    End Function

    Public Sub flip()
        l.wait()
        Try
            If white() Then
                assert(white_to_black())
            Else
                assert(black_to_white())
            End If
        Finally
            l.release()
        End Try
    End Sub

    Protected Overrides Sub disposer()
        w.Close()
        b.Close()
    End Sub
End Class
